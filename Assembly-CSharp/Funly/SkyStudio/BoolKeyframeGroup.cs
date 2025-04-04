using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A1 RID: 417
	[Serializable]
	public class BoolKeyframeGroup : KeyframeGroup<BoolKeyframe>
	{
		// Token: 0x06000870 RID: 2160 RVA: 0x00026A58 File Offset: 0x00024C58
		public BoolKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00026A61 File Offset: 0x00024C61
		public BoolKeyframeGroup(string name, BoolKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00026A74 File Offset: 0x00024C74
		public bool BoolForTime(float time)
		{
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't sample bool without any keyframes");
				return false;
			}
			if (this.keyframes.Count == 1)
			{
				return this.keyframes[0].value;
			}
			if (time < this.keyframes[0].time)
			{
				return this.keyframes[this.keyframes.Count - 1].value;
			}
			int index = 0;
			int num = 1;
			while (num < this.keyframes.Count && this.keyframes[num].time <= time)
			{
				index = num;
				num++;
			}
			return this.keyframes[index].value;
		}
	}
}
