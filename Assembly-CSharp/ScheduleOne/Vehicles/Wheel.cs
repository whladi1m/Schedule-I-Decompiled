using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007C6 RID: 1990
	public class Wheel : MonoBehaviour
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000E525A File Offset: 0x000E345A
		// (set) Token: 0x06003679 RID: 13945 RVA: 0x000E5262 File Offset: 0x000E3462
		public bool isStatic { get; protected set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x000E526B File Offset: 0x000E346B
		// (set) Token: 0x0600367B RID: 13947 RVA: 0x000E5273 File Offset: 0x000E3473
		public bool IsDrifting { get; protected set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000E527C File Offset: 0x000E347C
		public bool IsDrifting_Smoothed
		{
			get
			{
				return this.DriftTime > 0.2f;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x000E528B File Offset: 0x000E348B
		// (set) Token: 0x0600367E RID: 13950 RVA: 0x000E5293 File Offset: 0x000E3493
		public float DriftTime { get; protected set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x000E529C File Offset: 0x000E349C
		// (set) Token: 0x06003680 RID: 13952 RVA: 0x000E52A4 File Offset: 0x000E34A4
		public float DriftIntensity { get; protected set; }

		// Token: 0x06003681 RID: 13953 RVA: 0x000E52B0 File Offset: 0x000E34B0
		protected virtual void Start()
		{
			this.vehicle = base.GetComponentInParent<LandVehicle>();
			this.wheelCollider.ConfigureVehicleSubsteps(5f, 12, 15);
			this.defaultForwardStiffness = this.wheelCollider.forwardFriction.stiffness;
			this.defaultSidewaysStiffness = this.wheelCollider.sidewaysFriction.stiffness;
			this.wheelTransform = base.transform;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000E531B File Offset: 0x000E351B
		protected virtual void LateUpdate()
		{
			this.lastFramePosition = this.wheelTransform.position;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000E5330 File Offset: 0x000E3530
		private void FixedUpdate()
		{
			if (this.wheelCollider.enabled && !this.vehicle.Agent.KinematicMode && this.vehicle.DistanceToLocalCamera < 40f)
			{
				Vector3 position;
				Quaternion rotation;
				this.wheelCollider.GetWorldPose(out position, out rotation);
				this.wheelModel.transform.position = position;
				if (this.vehicle.localPlayerIsDriver)
				{
					this.modelContainer.transform.localRotation = Quaternion.identity;
					this.wheelModel.transform.rotation = rotation;
				}
				else
				{
					Vector3 vector = this.wheelTransform.position - this.lastFramePosition;
					float xAngle = this.wheelTransform.InverseTransformVector(vector).z / (6.2831855f * this.wheelCollider.radius) * 360f;
					this.wheelModel.transform.Rotate(xAngle, 0f, 0f, Space.Self);
					this.modelContainer.transform.localEulerAngles = new Vector3(0f, this.wheelCollider.steerAngle, 0f);
				}
				if (this.DriftParticlesEnabled)
				{
					this.DriftParticles.transform.position = this.wheelTransform.position - Vector3.up * this.wheelCollider.radius;
				}
			}
			if (!this.vehicle.localPlayerIsDriver)
			{
				this.DriftParticles.Stop();
				this.DriftAudioSource.Stop();
				return;
			}
			if (this.vehicle.isStatic)
			{
				return;
			}
			this.ApplyFriction();
			this.CheckDrifting();
			this.UpdateDriftEffects();
			this.UpdateDriftAudio();
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000E54E0 File Offset: 0x000E36E0
		private void CheckDrifting()
		{
			if (!this.wheelCollider.enabled)
			{
				this.IsDrifting = false;
				this.DriftTime = 0f;
				this.DriftIntensity = 0f;
				return;
			}
			if (Mathf.Abs(this.vehicle.speed_Kmh) < 8f)
			{
				this.IsDrifting = false;
				this.DriftTime = 0f;
				this.DriftIntensity = 0f;
				return;
			}
			this.wheelCollider.GetGroundHit(out this.wheelData);
			this.IsDrifting = ((Mathf.Abs(this.wheelData.sidewaysSlip) > 0.2f || Mathf.Abs(this.wheelData.forwardSlip) > 0.8f) && Mathf.Abs(this.vehicle.speed_Kmh) > 2f);
			float a = Mathf.Clamp01(Mathf.Abs(this.wheelData.sidewaysSlip));
			float b = Mathf.Clamp01(Mathf.Abs(this.wheelData.forwardSlip));
			this.DriftIntensity = Mathf.Max(a, b);
			if (this.IsDrifting)
			{
				this.DriftTime += Time.fixedDeltaTime;
			}
			else
			{
				this.DriftTime = 0f;
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Sideways slip: " + this.wheelData.sidewaysSlip.ToString() + "\nForward slip: " + this.wheelData.forwardSlip.ToString());
				Debug.Log("Drifting: " + this.IsDrifting.ToString());
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000E566C File Offset: 0x000E386C
		private void UpdateDriftEffects()
		{
			if (this.IsDrifting_Smoothed && this.DriftParticlesEnabled)
			{
				if (!this.DriftParticles.isPlaying)
				{
					this.DriftParticles.Play();
					return;
				}
			}
			else if (this.DriftParticles.isPlaying)
			{
				this.DriftParticles.Stop();
			}
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000E56BC File Offset: 0x000E38BC
		private void UpdateDriftAudio()
		{
			if (!this.DriftAudioEnabled)
			{
				return;
			}
			if (this.IsDrifting_Smoothed && this.DriftIntensity > 0.2f && !this.DriftAudioSource.isPlaying)
			{
				this.DriftAudioSource.Play();
			}
			if (this.DriftAudioSource.isPlaying)
			{
				float volumeMultiplier = Mathf.Clamp01(Mathf.InverseLerp(0.2f, 1f, this.DriftIntensity));
				this.DriftAudioSource.VolumeMultiplier = volumeMultiplier;
			}
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x000E5734 File Offset: 0x000E3934
		private void ApplyFriction()
		{
			this.forwardCurve = this.wheelCollider.forwardFriction;
			this.forwardCurve.stiffness = this.defaultForwardStiffness * ((this.vehicle.handbrakeApplied && this.vehicle.isOccupied) ? this.ForwardStiffnessMultiplier_Handbrake : 1f);
			this.wheelCollider.forwardFriction = this.forwardCurve;
			this.sidewaysCurve = this.wheelCollider.sidewaysFriction;
			this.sidewaysCurve.stiffness = this.defaultSidewaysStiffness * ((this.vehicle.handbrakeApplied && this.vehicle.isOccupied) ? this.SidewayStiffnessMultiplier_Handbrake : 1f);
			this.wheelCollider.sidewaysFriction = this.sidewaysCurve;
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x000E57F8 File Offset: 0x000E39F8
		public virtual void SetIsStatic(bool s)
		{
			this.isStatic = s;
			if (this.isStatic)
			{
				this.wheelCollider.enabled = false;
				this.wheelModel.transform.localPosition = new Vector3(this.wheelModel.transform.localPosition.x, -this.wheelCollider.suspensionDistance * this.wheelCollider.suspensionSpring.targetPosition, this.wheelModel.transform.localPosition.z);
				this.staticCollider.enabled = true;
				this.GroundWheelModel();
				return;
			}
			this.wheelCollider.enabled = true;
			this.staticCollider.enabled = false;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000E58A8 File Offset: 0x000E3AA8
		private void GroundWheelModel()
		{
			this.wheelModel.localPosition = Vector3.zero;
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000E58C8 File Offset: 0x000E3AC8
		public bool IsWheelGrounded()
		{
			WheelHit wheelHit;
			return this.wheelCollider.GetGroundHit(out wheelHit);
		}

		// Token: 0x0400273C RID: 10044
		public const float SIDEWAY_SLIP_THRESHOLD = 0.2f;

		// Token: 0x0400273D RID: 10045
		public const float FORWARD_SLIP_THRESHOLD = 0.8f;

		// Token: 0x0400273E RID: 10046
		public const float DRIFT_AUDIO_THRESHOLD = 0.2f;

		// Token: 0x0400273F RID: 10047
		public const float MIN_SPEED_FOR_DRIFT = 8f;

		// Token: 0x04002740 RID: 10048
		public const float WHEEL_ANIMATION_DISTANCE = 40f;

		// Token: 0x04002741 RID: 10049
		public bool DEBUG_MODE;

		// Token: 0x04002742 RID: 10050
		[Header("References")]
		public Transform wheelModel;

		// Token: 0x04002743 RID: 10051
		public Transform modelContainer;

		// Token: 0x04002744 RID: 10052
		public WheelCollider wheelCollider;

		// Token: 0x04002745 RID: 10053
		public Transform axleConnectionPoint;

		// Token: 0x04002746 RID: 10054
		public Collider staticCollider;

		// Token: 0x04002747 RID: 10055
		public ParticleSystem DriftParticles;

		// Token: 0x04002748 RID: 10056
		[Header("Settings")]
		public bool DriftParticlesEnabled = true;

		// Token: 0x04002749 RID: 10057
		public float ForwardStiffnessMultiplier_Handbrake = 0.5f;

		// Token: 0x0400274A RID: 10058
		public float SidewayStiffnessMultiplier_Handbrake = 0.5f;

		// Token: 0x0400274B RID: 10059
		[Header("Drift Audio")]
		public bool DriftAudioEnabled;

		// Token: 0x0400274C RID: 10060
		public AudioSourceController DriftAudioSource;

		// Token: 0x0400274D RID: 10061
		private float defaultForwardStiffness = 1f;

		// Token: 0x0400274E RID: 10062
		private float defaultSidewaysStiffness = 1f;

		// Token: 0x04002753 RID: 10067
		private LandVehicle vehicle;

		// Token: 0x04002754 RID: 10068
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x04002755 RID: 10069
		private WheelHit wheelData;

		// Token: 0x04002756 RID: 10070
		private WheelFrictionCurve forwardCurve;

		// Token: 0x04002757 RID: 10071
		private WheelFrictionCurve sidewaysCurve;

		// Token: 0x04002758 RID: 10072
		private Transform wheelTransform;
	}
}
