using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public class SpherePointKeyframe : BaseKeyframe
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x000273F4 File Offset: 0x000255F4
		public SpherePointKeyframe(SpherePoint spherePoint, float time) : base(time)
		{
			if (spherePoint == null)
			{
				Debug.LogError("Passed null sphere point, created empty point");
				this.spherePoint = new SpherePoint(0f, 0f);
			}
			else
			{
				this.spherePoint = spherePoint;
			}
			base.interpolationDirection = InterpolationDirection.Auto;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00027430 File Offset: 0x00025630
		public SpherePointKeyframe(SpherePointKeyframe keyframe) : base(keyframe.time)
		{
			this.spherePoint = new SpherePoint(keyframe.spherePoint.horizontalRotation, keyframe.spherePoint.verticalRotation);
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x0400096A RID: 2410
		public SpherePoint spherePoint;
	}
}
