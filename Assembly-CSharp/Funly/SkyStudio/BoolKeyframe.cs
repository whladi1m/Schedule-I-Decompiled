using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A9 RID: 425
	[Serializable]
	public class BoolKeyframe : BaseKeyframe
	{
		// Token: 0x060008AC RID: 2220 RVA: 0x00027318 File Offset: 0x00025518
		public BoolKeyframe(float time, bool value) : base(time)
		{
			this.value = value;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00027328 File Offset: 0x00025528
		public BoolKeyframe(BoolKeyframe keyframe) : base(keyframe.time)
		{
			this.value = keyframe.value;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x0400095F RID: 2399
		public bool value;
	}
}
