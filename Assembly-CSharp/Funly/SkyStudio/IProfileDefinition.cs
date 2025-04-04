using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001C4 RID: 452
	public interface IProfileDefinition
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060008E4 RID: 2276
		string shaderName { get; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060008E5 RID: 2277
		ProfileFeatureSection[] features { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008E6 RID: 2278
		ProfileGroupSection[] groups { get; }

		// Token: 0x060008E7 RID: 2279
		ProfileFeatureDefinition GetFeatureDefinition(string featureKey);
	}
}
