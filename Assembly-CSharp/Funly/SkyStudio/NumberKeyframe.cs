using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001AE RID: 430
	[Serializable]
	public class NumberKeyframe : BaseKeyframe
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x000273B2 File Offset: 0x000255B2
		public NumberKeyframe(float time, float value) : base(time)
		{
			this.value = value;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000273C2 File Offset: 0x000255C2
		public NumberKeyframe(NumberKeyframe keyframe) : base(keyframe.time)
		{
			this.value = keyframe.value;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000969 RID: 2409
		public float value;
	}
}
