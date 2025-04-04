using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A3 RID: 419
	public interface IKeyframeGroup
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000876 RID: 2166
		// (set) Token: 0x06000877 RID: 2167
		string name { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000878 RID: 2168
		string id { get; }

		// Token: 0x06000879 RID: 2169
		void SortKeyframes();

		// Token: 0x0600087A RID: 2170
		void TrimToSingleKeyframe();

		// Token: 0x0600087B RID: 2171
		void RemoveKeyFrame(IBaseKeyframe keyframe);

		// Token: 0x0600087C RID: 2172
		int GetKeyFrameCount();
	}
}
