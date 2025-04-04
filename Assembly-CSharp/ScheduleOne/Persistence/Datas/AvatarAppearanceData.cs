using System;
using ScheduleOne.AvatarFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003C9 RID: 969
	[Serializable]
	public class AvatarAppearanceData : SaveData
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x0005EC38 File Offset: 0x0005CE38
		public AvatarAppearanceData(AvatarSettings avatarSettings)
		{
			this.AvatarSettings = avatarSettings;
		}

		// Token: 0x0400135F RID: 4959
		public AvatarSettings AvatarSettings;
	}
}
