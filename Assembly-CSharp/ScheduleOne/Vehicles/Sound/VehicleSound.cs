using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles.Sound
{
	// Token: 0x020007C7 RID: 1991
	public class VehicleSound : MonoBehaviour
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x0600368C RID: 13964 RVA: 0x000E5935 File Offset: 0x000E3B35
		// (set) Token: 0x0600368D RID: 13965 RVA: 0x000E593D File Offset: 0x000E3B3D
		public LandVehicle Vehicle { get; private set; }

		// Token: 0x0600368E RID: 13966 RVA: 0x000E5948 File Offset: 0x000E3B48
		protected virtual void Awake()
		{
			this.Vehicle = base.GetComponentInParent<LandVehicle>();
			if (this.Vehicle == null)
			{
				return;
			}
			this.Vehicle.onHandbrakeApplied.AddListener(new UnityAction(this.HandbrakeApplied));
			this.Vehicle.onVehicleStart.AddListener(new UnityAction(this.EngineStart));
			this.EngineIdleSource.VolumeMultiplier = 0f;
			this.EngineLoopSource.VolumeMultiplier = 0f;
			this.Vehicle.onCollision.AddListener(new UnityAction<Collision>(this.OnCollision));
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000E59E4 File Offset: 0x000E3BE4
		protected virtual void FixedUpdate()
		{
			this.UpdateIdle();
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000E59EC File Offset: 0x000E3BEC
		private void UpdateIdle()
		{
			if (this.Vehicle.isOccupied)
			{
				this.currentIdleVolume = Mathf.MoveTowards(this.currentIdleVolume, 1f, Time.fixedDeltaTime * 2f);
				float time = Mathf.Abs(this.Vehicle.VelocityCalculator.Velocity.magnitude * 3.6f / this.Vehicle.TopSpeed);
				this.EngineLoopSource.AudioSource.pitch = this.EngineLoopPitchCurve.Evaluate(time) * this.EngineLoopPitchMultiplier;
				this.EngineLoopSource.VolumeMultiplier = this.EngineLoopVolumeCurve.Evaluate(time) * this.VolumeMultiplier;
				if (!this.EngineLoopSource.AudioSource.isPlaying)
				{
					this.EngineLoopSource.Play();
				}
			}
			else
			{
				this.currentIdleVolume = Mathf.MoveTowards(this.currentIdleVolume, 0f, Time.fixedDeltaTime * 2f);
				if (this.EngineLoopSource.AudioSource.isPlaying)
				{
					this.EngineLoopSource.Stop();
				}
			}
			this.EngineIdleSource.VolumeMultiplier = this.currentIdleVolume * this.VolumeMultiplier;
			if (this.currentIdleVolume > 0f)
			{
				if (!this.EngineIdleSource.AudioSource.isPlaying)
				{
					this.EngineIdleSource.Play();
					return;
				}
			}
			else
			{
				this.EngineIdleSource.Stop();
			}
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000E5B43 File Offset: 0x000E3D43
		protected void HandbrakeApplied()
		{
			this.HandbrakeSource.VolumeMultiplier = this.VolumeMultiplier;
			this.HandbrakeSource.Play();
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000E5B61 File Offset: 0x000E3D61
		protected void EngineStart()
		{
			this.EngineStartSource.VolumeMultiplier = this.VolumeMultiplier;
			this.EngineStartSource.Play();
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000E5B7F File Offset: 0x000E3D7F
		public void Honk()
		{
			this.HonkSource.Play();
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000E5B8C File Offset: 0x000E3D8C
		private void OnCollision(Collision collision)
		{
			float num = collision.relativeVelocity.magnitude * this.Vehicle.Rb.mass;
			if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
			{
				num *= 0.2f;
			}
			if (num < this.MinCollisionMomentum)
			{
				return;
			}
			if (Time.time - this.lastCollisionTime < 0.5f && num < this.lastCollisionMomentum)
			{
				return;
			}
			float t = Mathf.InverseLerp(this.MinCollisionMomentum, this.MaxCollisionMomentum, num);
			this.ImpactSound.VolumeMultiplier = Mathf.Lerp(this.MinCollisionVolume, this.MaxCollisionVolume, t);
			this.ImpactSound.PitchMultiplier = Mathf.Lerp(this.MaxCollisionPitch, this.MinCollisionPitch, t);
			this.ImpactSound.transform.position = collision.contacts[0].point;
			this.ImpactSound.Play();
			this.lastCollisionTime = Time.time;
			this.lastCollisionMomentum = num;
		}

		// Token: 0x04002759 RID: 10073
		public const float COLLISION_SOUND_COOLDOWN = 0.5f;

		// Token: 0x0400275A RID: 10074
		public float VolumeMultiplier = 1f;

		// Token: 0x0400275B RID: 10075
		[Header("References")]
		public AudioSourceController EngineStartSource;

		// Token: 0x0400275C RID: 10076
		public AudioSourceController EngineIdleSource;

		// Token: 0x0400275D RID: 10077
		public AudioSourceController EngineLoopSource;

		// Token: 0x0400275E RID: 10078
		public AudioSourceController HandbrakeSource;

		// Token: 0x0400275F RID: 10079
		public AudioSourceController HonkSource;

		// Token: 0x04002760 RID: 10080
		public AudioSourceController ImpactSound;

		// Token: 0x04002761 RID: 10081
		[Header("Impact Sounds")]
		public float MinCollisionMomentum = 3000f;

		// Token: 0x04002762 RID: 10082
		public float MaxCollisionMomentum = 20000f;

		// Token: 0x04002763 RID: 10083
		public float MinCollisionVolume = 0.2f;

		// Token: 0x04002764 RID: 10084
		public float MaxCollisionVolume = 1f;

		// Token: 0x04002765 RID: 10085
		public float MinCollisionPitch = 0.6f;

		// Token: 0x04002766 RID: 10086
		public float MaxCollisionPitch = 1.1f;

		// Token: 0x04002767 RID: 10087
		[Header("Engine Loop Settings")]
		public AnimationCurve EngineLoopPitchCurve;

		// Token: 0x04002768 RID: 10088
		public float EngineLoopPitchMultiplier = 1f;

		// Token: 0x04002769 RID: 10089
		public AnimationCurve EngineLoopVolumeCurve;

		// Token: 0x0400276B RID: 10091
		private float currentIdleVolume;

		// Token: 0x0400276C RID: 10092
		private float lastCollisionTime;

		// Token: 0x0400276D RID: 10093
		private float lastCollisionMomentum;
	}
}
