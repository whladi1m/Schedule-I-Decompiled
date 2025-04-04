using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002CF RID: 719
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardCamera : NetworkBehaviour
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x00044D66 File Offset: 0x00042F66
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00044D74 File Offset: 0x00042F74
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Skating.SkateboardCamera_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00044D93 File Offset: 0x00042F93
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!this.board.IsOwner)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00044DAF File Offset: 0x00042FAF
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this.targetTransform.gameObject);
			UnityEngine.Object.Destroy(this.cameraDolly.gameObject);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00044DD1 File Offset: 0x00042FD1
		private void Update()
		{
			if (!base.IsSpawned)
			{
				return;
			}
			this.timeSinceCameraManuallyAdjusted += Time.deltaTime;
			this.CheckForClick();
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00044DF4 File Offset: 0x00042FF4
		private void CheckForClick()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick) && this.timeSinceCameraManuallyAdjusted > 0.01f)
				{
					this.cameraAdjusted = true;
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
					this.orbitDistance = Mathf.Sqrt(Mathf.Pow(this.HorizontalOffset, 2f) + Mathf.Pow(this.VerticalOffset, 2f));
				}
				if (this.cameraAdjusted)
				{
					this.timeSinceCameraManuallyAdjusted = 0f;
					return;
				}
			}
			else
			{
				this.cameraAdjusted = false;
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00044E9B File Offset: 0x0004309B
		private void LateUpdate()
		{
			if (!base.IsSpawned)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.board.Owner.IsLocalClient)
			{
				return;
			}
			this.UpdateCamera();
			this.UpdateFOV();
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00044ED0 File Offset: 0x000430D0
		private void UpdateCamera()
		{
			this.targetTransform.position = this.LimitCameraPosition(this.GetTargetCameraPosition());
			this.targetTransform.LookAt(this.cameraOrigin);
			this.cameraDolly.position = Vector3.Lerp(this.cameraDolly.position, this.targetTransform.position, Time.deltaTime * 7.5f);
			this.cameraDolly.rotation = Quaternion.Lerp(this.cameraDolly.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			this.orbitDistance = Mathf.Clamp(Vector3.Distance(this.cameraOrigin.position, this.cameraDolly.position), Mathf.Sqrt(Mathf.Pow(this.HorizontalOffset, 2f) + Mathf.Pow(this.VerticalOffset, 2f)), 100f);
			if (this.timeSinceCameraManuallyAdjusted <= 0.01f)
			{
				if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
				{
					this.x += GameInput.MouseDelta.x * 60f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
					this.y -= GameInput.MouseDelta.y * 40f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
					this.y = SkateboardCamera.ClampAngle(this.y, -20f, 89f);
					Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
					Vector3 targetPosition = rotation * new Vector3(0f, 0f, -this.orbitDistance) + this.cameraOrigin.position;
					this.cam.rotation = rotation;
					this.cam.position = this.LimitCameraPosition(targetPosition);
				}
				else
				{
					Vector3 normalized = (this.cameraOrigin.TransformPoint(this.lastFrameCameraOffset) - this.cameraOrigin.position).normalized;
					Vector3 targetPosition2 = this.cameraOrigin.position + normalized * this.orbitDistance;
					this.cam.position = this.LimitCameraPosition(targetPosition2);
					this.cam.LookAt(this.cameraOrigin);
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
				}
				this.lastManualOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
			}
			else if (this.timeSinceCameraManuallyAdjusted < 0.61f)
			{
				this.targetTransform.position = Vector3.Lerp(this.cameraOrigin.TransformPoint(this.lastManualOffset), this.targetTransform.position, (this.timeSinceCameraManuallyAdjusted - 0.01f) / 0.6f);
				this.targetTransform.LookAt(this.cameraOrigin);
				this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 7.5f);
				this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			}
			else
			{
				this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 7.5f);
				this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			}
			this.lastFrameCameraOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000452B0 File Offset: 0x000434B0
		private void UpdateFOV()
		{
			float b = Mathf.Lerp(this.FOVMultiplier_MinSpeed, this.FOVMultiplier_MaxSpeed, Mathf.Clamp01(this.board.Rb.velocity.magnitude / this.board.TopSpeed_Ms));
			this.currentFovMultiplier = Mathf.Lerp(this.currentFovMultiplier, b, Time.deltaTime * this.FOVMultiplierChangeRate);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.currentFovMultiplier * Singleton<Settings>.Instance.CameraFOV, 0f);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00007EE5 File Offset: 0x000060E5
		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00045338 File Offset: 0x00043538
		private Vector3 GetTargetCameraPosition()
		{
			Vector3 a = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			Vector3 up = Vector3.up;
			return base.transform.position + a * this.HorizontalOffset + up * this.VerticalOffset;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00045390 File Offset: 0x00043590
		private Vector3 LimitCameraPosition(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition;
			default(LayerMask) | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			float num = 0.45f;
			Vector3 vector2 = Vector3.Normalize(vector - this.cameraOrigin.position);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.cameraOrigin.position, vector2, out raycastHit, Vector3.Distance(base.transform.position, vector) + num, 1 << LayerMask.NameToLayer("Default")))
			{
				vector = raycastHit.point - vector2 * num;
			}
			return vector;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000454D4 File Offset: 0x000436D4
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x000454E7 File Offset: 0x000436E7
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000454FA File Offset: 0x000436FA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00045508 File Offset: 0x00043708
		private void dll()
		{
			this.board = base.GetComponent<Skateboard>();
			this.targetTransform = new GameObject("VehicleCameraTargetTransform").transform;
			this.targetTransform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.cameraDolly = new GameObject("VehicleCameraDolly").transform;
			this.cameraDolly.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
		}

		// Token: 0x04001021 RID: 4129
		private const float followDelta = 7.5f;

		// Token: 0x04001022 RID: 4130
		private const float yMinLimit = -20f;

		// Token: 0x04001023 RID: 4131
		private const float manualOverrideTime = 0.01f;

		// Token: 0x04001024 RID: 4132
		private const float manualOverrideReturnTime = 0.6f;

		// Token: 0x04001025 RID: 4133
		private const float xSpeed = 60f;

		// Token: 0x04001026 RID: 4134
		private const float ySpeed = 40f;

		// Token: 0x04001027 RID: 4135
		private const float yMaxLimit = 89f;

		// Token: 0x04001028 RID: 4136
		[Header("References")]
		public Transform cameraOrigin;

		// Token: 0x04001029 RID: 4137
		[Header("Settings")]
		public float CameraFollowSpeed = 10f;

		// Token: 0x0400102A RID: 4138
		public float HorizontalOffset = -2.5f;

		// Token: 0x0400102B RID: 4139
		public float VerticalOffset = 2f;

		// Token: 0x0400102C RID: 4140
		public float CameraDownAngle = 18f;

		// Token: 0x0400102D RID: 4141
		[Header("Settings")]
		public float FOVMultiplier_MinSpeed = 1f;

		// Token: 0x0400102E RID: 4142
		public float FOVMultiplier_MaxSpeed = 1.3f;

		// Token: 0x0400102F RID: 4143
		public float FOVMultiplierChangeRate = 3f;

		// Token: 0x04001030 RID: 4144
		private Skateboard board;

		// Token: 0x04001031 RID: 4145
		private float currentFovMultiplier = 1f;

		// Token: 0x04001032 RID: 4146
		private bool cameraReversed;

		// Token: 0x04001033 RID: 4147
		private bool cameraAdjusted;

		// Token: 0x04001034 RID: 4148
		private float timeSinceCameraManuallyAdjusted = float.MaxValue;

		// Token: 0x04001035 RID: 4149
		private float orbitDistance;

		// Token: 0x04001036 RID: 4150
		private Vector3 lastFrameCameraOffset = Vector3.zero;

		// Token: 0x04001037 RID: 4151
		private Vector3 lastManualOffset = Vector3.zero;

		// Token: 0x04001038 RID: 4152
		private Transform targetTransform;

		// Token: 0x04001039 RID: 4153
		private Transform cameraDolly;

		// Token: 0x0400103A RID: 4154
		private float x;

		// Token: 0x0400103B RID: 4155
		private float y;

		// Token: 0x0400103C RID: 4156
		private bool dll_Excuted;

		// Token: 0x0400103D RID: 4157
		private bool dll_Excuted;
	}
}
