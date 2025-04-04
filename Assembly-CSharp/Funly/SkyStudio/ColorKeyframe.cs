using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001AA RID: 426
	[Serializable]
	public class ColorKeyframe : BaseKeyframe
	{
		// Token: 0x060008AE RID: 2222 RVA: 0x0002735A File Offset: 0x0002555A
		public ColorKeyframe(Color c, float time) : base(time)
		{
			this.color = c;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00027375 File Offset: 0x00025575
		public ColorKeyframe(ColorKeyframe keyframe) : base(keyframe.time)
		{
			this.color = keyframe.color;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000960 RID: 2400
		public Color color = Color.white;
	}
}
