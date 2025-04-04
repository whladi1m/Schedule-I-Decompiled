using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200014F RID: 335
	public static class TransformUtils
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x0001CA48 File Offset: 0x0001AC48
		public static TransformUtils.Packed GetWorldPacked(this Transform self)
		{
			return new TransformUtils.Packed
			{
				position = self.position,
				rotation = self.rotation,
				lossyScale = self.lossyScale
			};
		}

		// Token: 0x02000150 RID: 336
		public struct Packed
		{
			// Token: 0x06000651 RID: 1617 RVA: 0x0001CA85 File Offset: 0x0001AC85
			public bool IsSame(Transform transf)
			{
				return transf.position == this.position && transf.rotation == this.rotation && transf.lossyScale == this.lossyScale;
			}

			// Token: 0x04000741 RID: 1857
			public Vector3 position;

			// Token: 0x04000742 RID: 1858
			public Quaternion rotation;

			// Token: 0x04000743 RID: 1859
			public Vector3 lossyScale;
		}
	}
}
