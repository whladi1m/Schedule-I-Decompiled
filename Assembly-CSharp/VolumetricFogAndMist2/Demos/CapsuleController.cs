using System;
using UnityEngine;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x02000161 RID: 353
	public class CapsuleController : MonoBehaviour
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
		private void Update()
		{
			float num = Time.deltaTime * this.moveSpeed;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.Translate(-num, 0f, 0f);
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.Translate(num, 0f, 0f);
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				base.transform.Translate(0f, 0f, num);
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				base.transform.Translate(0f, 0f, -num);
			}
			if ((base.transform.position - this.lastPos).magnitude > this.distanceCheck)
			{
				this.lastPos = base.transform.position;
				this.fogVolume.SetFogOfWarAlpha(base.transform.position, this.fogHoleRadius, 0f, this.clearDuration);
			}
		}

		// Token: 0x04000788 RID: 1928
		public VolumetricFog fogVolume;

		// Token: 0x04000789 RID: 1929
		public float moveSpeed = 10f;

		// Token: 0x0400078A RID: 1930
		public float fogHoleRadius = 8f;

		// Token: 0x0400078B RID: 1931
		public float clearDuration = 0.2f;

		// Token: 0x0400078C RID: 1932
		public float distanceCheck = 1f;

		// Token: 0x0400078D RID: 1933
		private Vector3 lastPos = new Vector3(float.MaxValue, 0f, 0f);
	}
}
