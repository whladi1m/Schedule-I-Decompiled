using System;
using ScheduleOne;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x02000165 RID: 357
	public class OrbitCamera : MonoBehaviour
	{
		// Token: 0x060006D6 RID: 1750 RVA: 0x0001F0AC File Offset: 0x0001D2AC
		private void Start()
		{
			this.cam = Camera.main.transform;
			this.cameraRotSide = base.transform.eulerAngles.y;
			this.cameraRotSideCur = base.transform.eulerAngles.y;
			this.cameraRotUp = base.transform.eulerAngles.x;
			this.cameraRotUpCur = base.transform.eulerAngles.x;
			this.distance = -this.cam.localPosition.z;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001F138 File Offset: 0x0001D338
		private void LateUpdate()
		{
			Cursor.visible = false;
			if (!this.pivot)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.enableMouse)
			{
				this.cameraRotSide += GameInput.MouseDelta.x * 5f;
				this.cameraRotUp -= GameInput.MouseDelta.y * 5f;
			}
			else
			{
				this.cameraRotSide += this.idleRotationSpeed;
			}
			this.cameraRotSideCur = Mathf.LerpAngle(this.cameraRotSideCur, this.cameraRotSide, Time.deltaTime * this.lookSmoothSpeed);
			this.cameraRotUpCur = Mathf.Lerp(this.cameraRotUpCur, this.cameraRotUp, Time.deltaTime * this.lookSmoothSpeed);
			if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && this.enableMouse)
			{
				this.distance *= 1f - 0.1f * GameInput.MouseDelta.y;
			}
			if (this.enableMouse)
			{
				this.distance *= 1f - 1f * GameInput.MouseScrollDelta;
			}
			Vector3 position = this.pivot.position;
			base.transform.position = Vector3.Lerp(base.transform.position, position, Time.deltaTime * this.moveSmoothSpeed);
			base.transform.rotation = Quaternion.Euler(this.cameraRotUpCur, this.cameraRotSideCur, 0f);
			float d = Mathf.Lerp(-this.cam.transform.localPosition.z, this.distance, Time.deltaTime * this.scrollSmoothSpeed);
			this.cam.localPosition = -Vector3.forward * d;
		}

		// Token: 0x040007AA RID: 1962
		[Space]
		public Transform pivot;

		// Token: 0x040007AB RID: 1963
		[Space]
		public bool enableMouse = true;

		// Token: 0x040007AC RID: 1964
		public float idleRotationSpeed = 0.05f;

		// Token: 0x040007AD RID: 1965
		public float lookSmoothSpeed = 5f;

		// Token: 0x040007AE RID: 1966
		public float moveSmoothSpeed = 5f;

		// Token: 0x040007AF RID: 1967
		public float scrollSmoothSpeed = 5f;

		// Token: 0x040007B0 RID: 1968
		private Transform cam;

		// Token: 0x040007B1 RID: 1969
		private float cameraRotSide;

		// Token: 0x040007B2 RID: 1970
		private float cameraRotUp;

		// Token: 0x040007B3 RID: 1971
		private float cameraRotSideCur;

		// Token: 0x040007B4 RID: 1972
		private float cameraRotUpCur;

		// Token: 0x040007B5 RID: 1973
		private float distance;
	}
}
