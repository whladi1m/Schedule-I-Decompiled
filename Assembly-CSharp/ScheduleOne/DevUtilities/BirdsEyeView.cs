using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006C4 RID: 1732
	public class BirdsEyeView : Singleton<BirdsEyeView>
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002F36 RID: 12086 RVA: 0x00044D66 File Offset: 0x00042F66
		private Transform playerCam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002F37 RID: 12087 RVA: 0x000C4E5B File Offset: 0x000C305B
		// (set) Token: 0x06002F38 RID: 12088 RVA: 0x000C4E63 File Offset: 0x000C3063
		public bool isEnabled { get; protected set; }

		// Token: 0x06002F39 RID: 12089 RVA: 0x000C4E6C File Offset: 0x000C306C
		protected override void Awake()
		{
			base.Awake();
			this.targetTransform = new GameObject("_TargetCameraTransform").transform;
			this.targetTransform.SetParent(GameObject.Find("_Temp").transform);
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000C4EA3 File Offset: 0x000C30A3
		protected virtual void Update()
		{
			if (this.isEnabled)
			{
				this.UpdateLateralMovement();
				this.UpdateRotation();
				this.UpdateScrollMovement();
			}
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000C4EBF File Offset: 0x000C30BF
		protected virtual void LateUpdate()
		{
			if (this.isEnabled)
			{
				this.FinalizeCameraMovement();
			}
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000C4ED0 File Offset: 0x000C30D0
		public void Enable(Vector3 startPosition, Quaternion startRotation)
		{
			this.isEnabled = true;
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(startPosition, startRotation, 0f, false);
			Vector3 eulerAngles = startRotation.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.targetTransform.position = startPosition;
			this.targetTransform.rotation = startRotation;
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000C4F42 File Offset: 0x000C3142
		public void Disable(bool reenableCameraLook = true)
		{
			this.isEnabled = false;
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, reenableCameraLook, true);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000C4F6C File Offset: 0x000C316C
		protected void UpdateLateralMovement()
		{
			float num = GameInput.MotionAxis.y;
			float d = GameInput.MotionAxis.x;
			int num2 = 0;
			if (Input.GetKey(KeyCode.Space))
			{
				num2++;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				num2--;
			}
			if (num != 0f || num2 != 0)
			{
				this.CancelOriginSlide();
			}
			Vector3 forward = this.playerCam.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = this.playerCam.right;
			right.y = 0f;
			right.Normalize();
			Vector3 b = forward * num * this.lateralMovementSpeed * Time.deltaTime;
			Vector3 b2 = right * d * this.lateralMovementSpeed * Time.deltaTime;
			Vector3 b3 = Vector3.up * (float)num2 * this.lateralMovementSpeed * Time.deltaTime * 0.5f;
			this.targetTransform.position += b;
			this.targetTransform.position += b2;
			this.targetTransform.position += b3;
			this.rotationOriginPoint += b;
			this.rotationOriginPoint += b2;
			this.rotationOriginPoint += b3;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000C50EC File Offset: 0x000C32EC
		protected void UpdateScrollMovement()
		{
			float num = Input.mouseScrollDelta.y;
			Vector3 normalized = this.playerCam.forward.normalized;
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				this.distance += num * this.scrollMovementSpeed * Time.deltaTime;
				return;
			}
			this.targetTransform.position += normalized * num * this.scrollMovementSpeed * Time.deltaTime;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000C5178 File Offset: 0x000C3378
		protected void UpdateRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
				Ray ray = new Ray(this.targetTransform.position, this.targetTransform.forward);
				float num = 0f;
				plane.Raycast(ray, out num);
				this.distance = num;
				this.rotationOriginPoint = ray.GetPoint(num);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				this.x += GameInput.MouseDelta.x * this.xSpeed * 0.02f;
				this.y -= GameInput.MouseDelta.y * this.ySpeed * 0.02f;
				this.y = BirdsEyeView.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.rotationOriginPoint;
				this.targetTransform.rotation = rotation;
				this.targetTransform.position = position;
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000C52CC File Offset: 0x000C34CC
		private void FinalizeCameraMovement()
		{
			this.playerCam.position = Vector3.Lerp(this.playerCam.position, this.targetTransform.position, Time.deltaTime * this.targetFollowSpeed);
			this.playerCam.rotation = Quaternion.Lerp(this.playerCam.rotation, this.targetTransform.rotation, Time.deltaTime * this.targetFollowSpeed);
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x00007EE5 File Offset: 0x000060E5
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

		// Token: 0x06002F43 RID: 12099 RVA: 0x000C533D File Offset: 0x000C353D
		private void CancelOriginSlide()
		{
			if (this.originSlideRoutine != null)
			{
				base.StopCoroutine(this.originSlideRoutine);
				this.originSlideRoutine = null;
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000C535C File Offset: 0x000C355C
		public void SlideCameraOrigin(Vector3 position, float offsetDistance, float time = 0f)
		{
			BirdsEyeView.<>c__DisplayClass33_0 CS$<>8__locals1 = new BirdsEyeView.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.position = position;
			CS$<>8__locals1.time = time;
			if (this.originSlideRoutine != null)
			{
				base.StopCoroutine(this.originSlideRoutine);
			}
			Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
			Ray ray = new Ray(this.targetTransform.position, this.targetTransform.forward);
			float num = 0f;
			plane.Raycast(ray, out num);
			Vector3 point = ray.GetPoint(num);
			Vector3 vector = this.targetTransform.position - point;
			CS$<>8__locals1.position += vector.normalized * offsetDistance;
			this.originSlideRoutine = base.StartCoroutine(CS$<>8__locals1.<SlideCameraOrigin>g__Routine|0());
		}

		// Token: 0x0400219F RID: 8607
		[Header("Settings")]
		public Vector3 bounds_Min;

		// Token: 0x040021A0 RID: 8608
		public Vector3 bounds_Max;

		// Token: 0x040021A1 RID: 8609
		[Header("Camera settings")]
		public float lateralMovementSpeed = 1f;

		// Token: 0x040021A2 RID: 8610
		public float scrollMovementSpeed = 1f;

		// Token: 0x040021A3 RID: 8611
		public float targetFollowSpeed = 1f;

		// Token: 0x040021A4 RID: 8612
		[Header("Camera orbit settings")]
		public float xSpeed = 250f;

		// Token: 0x040021A5 RID: 8613
		public float ySpeed = 120f;

		// Token: 0x040021A6 RID: 8614
		public float yMinLimit = -20f;

		// Token: 0x040021A7 RID: 8615
		public float yMaxLimit = 80f;

		// Token: 0x040021A8 RID: 8616
		private Vector3 rotationOriginPoint = Vector3.zero;

		// Token: 0x040021A9 RID: 8617
		private float distance = 10f;

		// Token: 0x040021AA RID: 8618
		private float prevDistance;

		// Token: 0x040021AB RID: 8619
		private float x;

		// Token: 0x040021AC RID: 8620
		private float y;

		// Token: 0x040021AD RID: 8621
		private Transform targetTransform;

		// Token: 0x040021AF RID: 8623
		private Coroutine originSlideRoutine;
	}
}
