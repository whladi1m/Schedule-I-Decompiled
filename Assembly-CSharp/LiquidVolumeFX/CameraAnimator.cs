using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200016D RID: 365
	public class CameraAnimator : MonoBehaviour
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x0001FA59 File Offset: 0x0001DC59
		private void Start()
		{
			this.y = base.transform.position.y;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001FA74 File Offset: 0x0001DC74
		private void Update()
		{
			base.transform.RotateAround(this.lookAt, Vector3.up, Time.deltaTime * this.speedX);
			this.y += this.dy;
			this.dy -= (base.transform.position.y - this.baseHeight) * Time.deltaTime * this.speedY;
			base.transform.position = new Vector3(base.transform.position.x, this.y, base.transform.position.z);
			Quaternion rotation = base.transform.rotation;
			base.transform.LookAt(this.lookAt);
			base.transform.rotation = Quaternion.Lerp(rotation, base.transform.rotation, 0.2f);
			base.transform.position += base.transform.forward * this.distSum;
			this.distSum += this.distSpeed;
			this.distDirection = ((this.distSum < 0f) ? 1f : -1f);
			this.distSpeed += Time.deltaTime * this.distDirection * this.distAcceleration;
		}

		// Token: 0x040007D3 RID: 2003
		public float baseHeight = 0.6f;

		// Token: 0x040007D4 RID: 2004
		public float speedY = 0.005f;

		// Token: 0x040007D5 RID: 2005
		public float speedX = 5f;

		// Token: 0x040007D6 RID: 2006
		public float distAcceleration = 0.0002f;

		// Token: 0x040007D7 RID: 2007
		public float distSpeed = 0.0001f;

		// Token: 0x040007D8 RID: 2008
		public Vector3 lookAt;

		// Token: 0x040007D9 RID: 2009
		private float y;

		// Token: 0x040007DA RID: 2010
		private float dy;

		// Token: 0x040007DB RID: 2011
		private float distDirection = 1f;

		// Token: 0x040007DC RID: 2012
		private float distSum;
	}
}
