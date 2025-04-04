using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000185 RID: 389
	public class RotateBody : MonoBehaviour
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00025869 File Offset: 0x00023A69
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x00025871 File Offset: 0x00023A71
		public float SpinSpeed
		{
			get
			{
				return this.m_SpinSpeed;
			}
			set
			{
				this.m_SpinSpeed = value;
				this.UpdateOrbitBodyRotation();
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00025880 File Offset: 0x00023A80
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00025888 File Offset: 0x00023A88
		public bool AllowSpinning
		{
			get
			{
				return this.m_AllowSpinning;
			}
			set
			{
				this.m_AllowSpinning = value;
				this.UpdateOrbitBodyRotation();
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00025898 File Offset: 0x00023A98
		public void UpdateOrbitBodyRotation()
		{
			float num = (float)(this.m_AllowSpinning ? 1 : 0);
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 euler = new Vector3(0f, -180f, (eulerAngles.z + -10f * this.SpinSpeed * Time.deltaTime) * num);
			base.transform.localRotation = Quaternion.Euler(euler);
		}

		// Token: 0x04000914 RID: 2324
		private float m_SpinSpeed;

		// Token: 0x04000915 RID: 2325
		private bool m_AllowSpinning;
	}
}
