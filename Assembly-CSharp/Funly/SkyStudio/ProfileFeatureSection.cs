using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001B9 RID: 441
	[Serializable]
	public class ProfileFeatureSection
	{
		// Token: 0x060008D1 RID: 2257 RVA: 0x00027835 File Offset: 0x00025A35
		public ProfileFeatureSection(string sectionTitle, string sectionKey, ProfileFeatureDefinition[] featureDefinitions)
		{
			this.sectionTitle = sectionTitle;
			this.sectionKey = sectionKey;
			this.featureDefinitions = featureDefinitions;
		}

		// Token: 0x040009CB RID: 2507
		public string sectionTitle;

		// Token: 0x040009CC RID: 2508
		public string sectionKey;

		// Token: 0x040009CD RID: 2509
		public string sectionIcon;

		// Token: 0x040009CE RID: 2510
		public ProfileFeatureDefinition[] featureDefinitions;
	}
}
