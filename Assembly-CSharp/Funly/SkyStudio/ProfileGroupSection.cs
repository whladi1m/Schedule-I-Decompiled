using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001B8 RID: 440
	public class ProfileGroupSection
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x00027800 File Offset: 0x00025A00
		public ProfileGroupSection(string sectionTitle, string sectionKey, string sectionIcon, string dependsOnFeature, bool dependsOnValue, ProfileGroupDefinition[] groups)
		{
			this.sectionTitle = sectionTitle;
			this.sectionIcon = sectionIcon;
			this.sectionKey = sectionKey;
			this.groups = groups;
			this.dependsOnFeature = dependsOnFeature;
			this.dependsOnValue = dependsOnValue;
		}

		// Token: 0x040009C5 RID: 2501
		public string sectionTitle;

		// Token: 0x040009C6 RID: 2502
		public string sectionIcon;

		// Token: 0x040009C7 RID: 2503
		public string sectionKey;

		// Token: 0x040009C8 RID: 2504
		public string dependsOnFeature;

		// Token: 0x040009C9 RID: 2505
		public bool dependsOnValue;

		// Token: 0x040009CA RID: 2506
		public ProfileGroupDefinition[] groups;
	}
}
