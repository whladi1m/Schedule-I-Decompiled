using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B0 RID: 432
	[Serializable]
	public class TextureKeyframe : BaseKeyframe
	{
		// Token: 0x060008BB RID: 2235 RVA: 0x00027482 File Offset: 0x00025682
		public TextureKeyframe(Texture texture, float time) : base(time)
		{
			this.texture = texture;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00027492 File Offset: 0x00025692
		public TextureKeyframe(TextureKeyframe keyframe) : base(keyframe.time)
		{
			this.texture = keyframe.texture;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x0400096B RID: 2411
		public Texture texture;
	}
}
