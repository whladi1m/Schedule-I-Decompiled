using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A7 RID: 423
	[Serializable]
	public class TextureKeyframeGroup : KeyframeGroup<TextureKeyframe>
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x0002720C File Offset: 0x0002540C
		public TextureKeyframeGroup(string name, TextureKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002721C File Offset: 0x0002541C
		public Texture TextureForTime(float time)
		{
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return texture without any keyframes");
				return null;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).texture;
			}
			int index;
			int num;
			base.GetSurroundingKeyFrames(time, out index, out num);
			return base.GetKeyframe(index).texture;
		}
	}
}
