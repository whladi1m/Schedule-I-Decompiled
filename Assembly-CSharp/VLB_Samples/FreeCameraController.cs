using System;
using UnityEngine;

namespace VLB_Samples
{
	// Token: 0x0200015E RID: 350
	public class FreeCameraController : MonoBehaviour
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001E3B0 File Offset: 0x0001C5B0
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x0001E3B8 File Offset: 0x0001C5B8
		private bool useMouseView
		{
			get
			{
				return this.m_UseMouseView;
			}
			set
			{
				this.m_UseMouseView = value;
				Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !value;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		private void Start()
		{
			this.useMouseView = true;
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			this.rotationH = eulerAngles.y;
			this.rotationV = eulerAngles.x;
			if (this.rotationV > 180f)
			{
				this.rotationV -= 360f;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001E438 File Offset: 0x0001C638
		private void Update()
		{
			if (this.useMouseView)
			{
				this.rotationH += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
				this.rotationV -= Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
			}
			this.rotationV = Mathf.Clamp(this.rotationV, -90f, 90f);
			base.transform.rotation = Quaternion.AngleAxis(this.rotationH, Vector3.up);
			base.transform.rotation *= Quaternion.AngleAxis(this.rotationV, Vector3.right);
			float num = this.speedNormal;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				num *= this.speedFactorFast;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				num *= this.speedFactorSlow;
			}
			base.transform.position += num * Input.GetAxis("Vertical") * Time.deltaTime * base.transform.forward;
			base.transform.position += num * Input.GetAxis("Horizontal") * Time.deltaTime * base.transform.right;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.up;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.down;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				this.useMouseView = !this.useMouseView;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.useMouseView = false;
			}
		}

		// Token: 0x04000779 RID: 1913
		public float cameraSensitivity = 90f;

		// Token: 0x0400077A RID: 1914
		public float speedNormal = 10f;

		// Token: 0x0400077B RID: 1915
		public float speedFactorSlow = 0.25f;

		// Token: 0x0400077C RID: 1916
		public float speedFactorFast = 3f;

		// Token: 0x0400077D RID: 1917
		public float speedClimb = 4f;

		// Token: 0x0400077E RID: 1918
		private float rotationH;

		// Token: 0x0400077F RID: 1919
		private float rotationV;

		// Token: 0x04000780 RID: 1920
		private bool m_UseMouseView = true;
	}
}
