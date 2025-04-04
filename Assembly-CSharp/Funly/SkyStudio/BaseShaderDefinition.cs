using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public abstract class BaseShaderDefinition : IProfileDefinition
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x000278DB File Offset: 0x00025ADB
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x000278E3 File Offset: 0x00025AE3
		public string shaderName { get; protected set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000278EC File Offset: 0x00025AEC
		public ProfileGroupSection[] groups
		{
			get
			{
				ProfileGroupSection[] result;
				if ((result = this.m_ProfileDefinitions) == null)
				{
					result = (this.m_ProfileDefinitions = this.ProfileDefinitionTable());
				}
				return result;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00027914 File Offset: 0x00025B14
		public ProfileFeatureSection[] features
		{
			get
			{
				ProfileFeatureSection[] result;
				if ((result = this.m_ProfileFeatures) == null)
				{
					result = (this.m_ProfileFeatures = this.ProfileFeatureSection());
				}
				return result;
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002793C File Offset: 0x00025B3C
		public ProfileFeatureDefinition GetFeatureDefinition(string featureKey)
		{
			if (this.m_KeyToFeature == null)
			{
				this.m_KeyToFeature = new Dictionary<string, ProfileFeatureDefinition>();
				ProfileFeatureSection[] features = this.features;
				for (int i = 0; i < features.Length; i++)
				{
					foreach (ProfileFeatureDefinition profileFeatureDefinition in features[i].featureDefinitions)
					{
						if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
						{
							this.m_KeyToFeature[profileFeatureDefinition.featureKey] = profileFeatureDefinition;
						}
						else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
						{
							foreach (string key in profileFeatureDefinition.featureKeys)
							{
								this.m_KeyToFeature[key] = profileFeatureDefinition;
							}
						}
					}
				}
			}
			if (featureKey == null)
			{
				return null;
			}
			if (!this.m_KeyToFeature.ContainsKey(featureKey))
			{
				return null;
			}
			return this.m_KeyToFeature[featureKey];
		}

		// Token: 0x060008E1 RID: 2273
		protected abstract ProfileFeatureSection[] ProfileFeatureSection();

		// Token: 0x060008E2 RID: 2274
		protected abstract ProfileGroupSection[] ProfileDefinitionTable();

		// Token: 0x04000A93 RID: 2707
		private ProfileGroupSection[] m_ProfileDefinitions;

		// Token: 0x04000A94 RID: 2708
		[SerializeField]
		private ProfileFeatureSection[] m_ProfileFeatures;

		// Token: 0x04000A95 RID: 2709
		private Dictionary<string, ProfileFeatureDefinition> m_KeyToFeature;
	}
}
