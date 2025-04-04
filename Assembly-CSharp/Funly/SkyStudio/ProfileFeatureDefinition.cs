using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001B1 RID: 433
	[Serializable]
	public class ProfileFeatureDefinition
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x000274C4 File Offset: 0x000256C4
		public static ProfileFeatureDefinition CreateShaderFeature(string featureKey, string shaderKeyword, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.ShaderKeyword,
				featureKey = featureKey,
				shaderKeyword = shaderKeyword,
				name = name,
				value = value,
				tooltip = tooltip,
				dependsOnFeature = dependsOnFeature,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00027514 File Offset: 0x00025714
		public static ProfileFeatureDefinition CreateShaderFeatureDropdown(string[] featureKeys, string[] shaderKeywords, string[] labels, int selectedIndex, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown,
				featureKeys = featureKeys,
				shaderKeywords = shaderKeywords,
				dropdownLabels = labels,
				name = name,
				dropdownSelectedIndex = selectedIndex,
				tooltip = tooltip,
				dependsOnFeature = dependsOnFeature,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00027569 File Offset: 0x00025769
		public static ProfileFeatureDefinition CreateBooleanFeature(string featureKey, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.BooleanValue,
				featureKey = featureKey,
				name = name,
				value = value,
				tooltip = tooltip
			};
		}

		// Token: 0x0400096C RID: 2412
		public string featureKey;

		// Token: 0x0400096D RID: 2413
		public string[] featureKeys;

		// Token: 0x0400096E RID: 2414
		public ProfileFeatureDefinition.FeatureType featureType;

		// Token: 0x0400096F RID: 2415
		public string shaderKeyword;

		// Token: 0x04000970 RID: 2416
		public string[] shaderKeywords;

		// Token: 0x04000971 RID: 2417
		public string[] dropdownLabels;

		// Token: 0x04000972 RID: 2418
		public int dropdownSelectedIndex;

		// Token: 0x04000973 RID: 2419
		public string name;

		// Token: 0x04000974 RID: 2420
		public bool value;

		// Token: 0x04000975 RID: 2421
		public string tooltip;

		// Token: 0x04000976 RID: 2422
		public string dependsOnFeature;

		// Token: 0x04000977 RID: 2423
		public bool dependsOnValue;

		// Token: 0x04000978 RID: 2424
		public bool isShaderKeywordFeature;

		// Token: 0x020001B2 RID: 434
		public enum FeatureType
		{
			// Token: 0x0400097A RID: 2426
			ShaderKeyword,
			// Token: 0x0400097B RID: 2427
			BooleanValue,
			// Token: 0x0400097C RID: 2428
			ShaderKeywordDropdown
		}
	}
}
