using System;
using EasyButtons;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D9 RID: 1753
	public class OrbitCamera : MonoBehaviour
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002FC8 RID: 12232 RVA: 0x000C7103 File Offset: 0x000C5303
		// (set) Token: 0x06002FC9 RID: 12233 RVA: 0x000C710B File Offset: 0x000C530B
		public bool isEnabled { get; protected set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x00044D66 File Offset: 0x00042F66
		protected Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000C7114 File Offset: 0x000C5314
		protected virtual void Awake()
		{
			this.targetTransform = new GameObject("_OrbitCamTarget").transform;
			this.targetTransform.SetParent(GameObject.Find("_Temp").transform);
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000C7145 File Offset: 0x000C5345
		protected virtual void Update()
		{
			if (this.isEnabled)
			{
				this.UpdateRotation();
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000C7155 File Offset: 0x000C5355
		protected virtual void LateUpdate()
		{
			if (this.isEnabled)
			{
				this.FinalizeCameraMovement();
			}
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000C7168 File Offset: 0x000C5368
		[Button]
		public void Enable()
		{
			this.isEnabled = true;
			this.cameraStartPoint.LookAt(this.centrePoint);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(80f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.cam.position, this.cam.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.blockNextStopTransformOverride = true;
			Vector3 eulerAngles = this.cameraStartPoint.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.targetTransform.position = this.cameraStartPoint.position;
			this.targetTransform.rotation = this.cameraStartPoint.rotation;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000C7222 File Offset: 0x000C5422
		public void Disable()
		{
			this.isEnabled = false;
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, false, true);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000C724C File Offset: 0x000C544C
		protected void UpdateRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick))
			{
				this.distance = Vector3.Distance(this.centrePoint.position, this.targetTransform.position);
				this.rotationOriginPoint = this.centrePoint.position;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick))
			{
				this.x += GameInput.MouseDelta.x * OrbitCamera.xSpeed * 0.02f;
				this.y -= GameInput.MouseDelta.y * OrbitCamera.ySpeed * 0.02f;
				this.y = OrbitCamera.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.rotationOriginPoint;
				this.targetTransform.rotation = rotation;
				this.targetTransform.position = position;
			}
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x00007EE5 File Offset: 0x000060E5
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

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000C7358 File Offset: 0x000C5558
		private void FinalizeCameraMovement()
		{
			this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * this.targetFollowSpeed);
			this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * this.targetFollowSpeed);
		}

		// Token: 0x04002215 RID: 8725
		[Header("References")]
		[SerializeField]
		protected Transform cameraStartPoint;

		// Token: 0x04002216 RID: 8726
		[SerializeField]
		protected Transform centrePoint;

		// Token: 0x04002217 RID: 8727
		[Header("Settings")]
		public float targetFollowSpeed = 1f;

		// Token: 0x04002218 RID: 8728
		public float yMinLimit = -20f;

		// Token: 0x04002219 RID: 8729
		public float yMaxLimit = 80f;

		// Token: 0x0400221A RID: 8730
		public static float xSpeed = 200f;

		// Token: 0x0400221B RID: 8731
		public static float ySpeed = 100f;

		// Token: 0x0400221D RID: 8733
		private Vector3 rotationOriginPoint = Vector3.zero;

		// Token: 0x0400221E RID: 8734
		private float distance = 10f;

		// Token: 0x0400221F RID: 8735
		private float prevDistance;

		// Token: 0x04002220 RID: 8736
		private float x;

		// Token: 0x04002221 RID: 8737
		private float y;

		// Token: 0x04002222 RID: 8738
		private Transform targetTransform;
	}
}
