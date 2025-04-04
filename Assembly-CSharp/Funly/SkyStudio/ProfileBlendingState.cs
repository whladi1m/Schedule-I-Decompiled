using System;

namespace Funly.SkyStudio
{
	// Token: 0x0200019F RID: 415
	public struct ProfileBlendingState
	{
		// Token: 0x0600086F RID: 2159 RVA: 0x00026A21 File Offset: 0x00024C21
		public ProfileBlendingState(SkyProfile blendedProfile, SkyProfile fromProfile, SkyProfile toProfile, float progress, float outProgress, float inProgress, float timeOfDay)
		{
			this.blendedProfile = blendedProfile;
			this.fromProfile = fromProfile;
			this.toProfile = toProfile;
			this.progress = progress;
			this.inProgress = inProgress;
			this.outProgress = outProgress;
			this.timeOfDay = timeOfDay;
		}

		// Token: 0x04000946 RID: 2374
		public SkyProfile blendedProfile;

		// Token: 0x04000947 RID: 2375
		public SkyProfile fromProfile;

		// Token: 0x04000948 RID: 2376
		public SkyProfile toProfile;

		// Token: 0x04000949 RID: 2377
		public float progress;

		// Token: 0x0400094A RID: 2378
		public float outProgress;

		// Token: 0x0400094B RID: 2379
		public float inProgress;

		// Token: 0x0400094C RID: 2380
		public float timeOfDay;
	}
}
