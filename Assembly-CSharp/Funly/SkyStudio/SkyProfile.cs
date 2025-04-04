using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001C6 RID: 454
	[CreateAssetMenu(fileName = "skyProfile.asset", menuName = "Sky Studio/Sky Profile", order = 0)]
	public class SkyProfile : ScriptableObject
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0002985A File Offset: 0x00027A5A
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x00029864 File Offset: 0x00027A64
		public Material skyboxMaterial
		{
			get
			{
				return this.m_SkyboxMaterial;
			}
			set
			{
				if (value == null)
				{
					this.m_SkyboxMaterial = null;
					return;
				}
				if (this.m_SkyboxMaterial && this.m_SkyboxMaterial.shader.name != value.shader.name)
				{
					this.m_SkyboxMaterial = value;
					this.m_ShaderName = value.shader.name;
					this.ReloadDefinitions();
					return;
				}
				this.m_SkyboxMaterial = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x000298D7 File Offset: 0x00027AD7
		public string shaderName
		{
			get
			{
				return this.m_ShaderName;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x000298DF File Offset: 0x00027ADF
		public ProfileGroupSection[] groupDefinitions
		{
			get
			{
				if (this.profileDefinition == null)
				{
					return null;
				}
				return this.profileDefinition.groups;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x000298F6 File Offset: 0x00027AF6
		public ProfileFeatureSection[] featureDefinitions
		{
			get
			{
				if (this.profileDefinition == null)
				{
					return null;
				}
				return this.profileDefinition.features;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002990D File Offset: 0x00027B0D
		public float GetNumberPropertyValue(string propertyKey)
		{
			return this.GetNumberPropertyValue(propertyKey, 0f);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002991C File Offset: 0x00027B1C
		public float GetNumberPropertyValue(string propertyKey, float timeOfDay)
		{
			NumberKeyframeGroup group = this.GetGroup<NumberKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find number group with property key: " + propertyKey);
				return -1f;
			}
			return group.NumericValueAtTime(timeOfDay);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00029951 File Offset: 0x00027B51
		public Color GetColorPropertyValue(string propertyKey)
		{
			return this.GetColorPropertyValue(propertyKey, 0f);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00029960 File Offset: 0x00027B60
		public Color GetColorPropertyValue(string propertyKey, float timeOfDay)
		{
			ColorKeyframeGroup group = this.GetGroup<ColorKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find color group with property key: " + propertyKey);
				return Color.white;
			}
			return group.ColorForTime(timeOfDay);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00029995 File Offset: 0x00027B95
		public Texture GetTexturePropertyValue(string propertyKey)
		{
			return this.GetTexturePropertyValue(propertyKey, 0f);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000299A4 File Offset: 0x00027BA4
		public Texture GetTexturePropertyValue(string propertyKey, float timeOfDay)
		{
			TextureKeyframeGroup group = this.GetGroup<TextureKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find texture group with property key: " + propertyKey);
				return null;
			}
			return group.TextureForTime(timeOfDay);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000299D5 File Offset: 0x00027BD5
		public SpherePoint GetSpherePointPropertyValue(string propertyKey)
		{
			return this.GetSpherePointPropertyValue(propertyKey, 0f);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000299E4 File Offset: 0x00027BE4
		public SpherePoint GetSpherePointPropertyValue(string propertyKey, float timeOfDay)
		{
			SpherePointKeyframeGroup group = this.GetGroup<SpherePointKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find a sphere point group with property key: " + propertyKey);
				return null;
			}
			return group.SpherePointForTime(timeOfDay);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00029A15 File Offset: 0x00027C15
		public bool GetBoolPropertyValue(string propertyKey)
		{
			return this.GetBoolPropertyValue(propertyKey, 0f);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00029A24 File Offset: 0x00027C24
		public bool GetBoolPropertyValue(string propertyKey, float timeOfDay)
		{
			BoolKeyframeGroup group = this.GetGroup<BoolKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find boolean group with property key: " + propertyKey);
				return false;
			}
			return group.BoolForTime(timeOfDay);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00029A58 File Offset: 0x00027C58
		public SkyProfile()
		{
			this.ReloadFullProfile();
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00029AA4 File Offset: 0x00027CA4
		private void OnEnable()
		{
			this.ReloadFullProfile();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00029AAC File Offset: 0x00027CAC
		private void ReloadFullProfile()
		{
			this.ReloadDefinitions();
			this.MergeProfileWithDefinitions();
			this.RebuildKeyToGroupInfoMapping();
			this.ValidateTimelineGroupKeys();
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00029AC6 File Offset: 0x00027CC6
		private void ReloadDefinitions()
		{
			this.profileDefinition = this.GetShaderInfoForMaterial(this.m_ShaderName);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00029ADA File Offset: 0x00027CDA
		private IProfileDefinition GetShaderInfoForMaterial(string shaderName)
		{
			return new Standard3dShaderDefinition();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00029AE1 File Offset: 0x00027CE1
		public void MergeProfileWithDefinitions()
		{
			this.MergeGroupsWithDefinitions();
			this.MergeShaderKeywordsWithDefinitions();
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00029AF0 File Offset: 0x00027CF0
		public void MergeGroupsWithDefinitions()
		{
			HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					if (propertyKeysSet.Contains(profileGroupDefinition.propertyKey))
					{
						if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Color)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddColorGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.color);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Number)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddNumericGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.minimumValue, profileGroupDefinition.maximumValue, profileGroupDefinition.value);
							}
							else
							{
								NumberKeyframeGroup group = this.keyframeGroups.GetGroup<NumberKeyframeGroup>(profileGroupDefinition.propertyKey);
								group.name = profileGroupDefinition.groupName;
								group.minValue = profileGroupDefinition.minimumValue;
								group.maxValue = profileGroupDefinition.maximumValue;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Texture)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddTextureGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.texture);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.SpherePoint)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddSpherePointGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.spherePoint);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Boolean)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddBooleanGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.boolValue);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00029D58 File Offset: 0x00027F58
		public Dictionary<string, ProfileGroupDefinition> GroupDefinitionDictionary()
		{
			ProfileGroupSection[] array = this.ProfileDefinitionTable();
			Dictionary<string, ProfileGroupDefinition> dictionary = new Dictionary<string, ProfileGroupDefinition>();
			ProfileGroupSection[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in array2[i].groups)
				{
					dictionary.Add(profileGroupDefinition.propertyKey, profileGroupDefinition);
				}
			}
			return dictionary;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00029DB1 File Offset: 0x00027FB1
		public ProfileGroupSection[] ProfileDefinitionTable()
		{
			return this.groupDefinitions;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00029DBC File Offset: 0x00027FBC
		private void AddNumericGroup(string propKey, string groupName, float min, float max, float value)
		{
			NumberKeyframeGroup value2 = new NumberKeyframeGroup(groupName, min, max, new NumberKeyframe(0f, value));
			this.keyframeGroups[propKey] = value2;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00029DEC File Offset: 0x00027FEC
		private void AddColorGroup(string propKey, string groupName, Color color)
		{
			ColorKeyframeGroup value = new ColorKeyframeGroup(groupName, new ColorKeyframe(color, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00029E18 File Offset: 0x00028018
		private void AddTextureGroup(string propKey, string groupName, Texture2D texture)
		{
			TextureKeyframeGroup value = new TextureKeyframeGroup(groupName, new TextureKeyframe(texture, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00029E44 File Offset: 0x00028044
		private void AddSpherePointGroup(string propKey, string groupName, SpherePoint point)
		{
			SpherePointKeyframeGroup value = new SpherePointKeyframeGroup(groupName, new SpherePointKeyframe(point, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00029E70 File Offset: 0x00028070
		private void AddBooleanGroup(string propKey, string groupName, bool value)
		{
			BoolKeyframeGroup value2 = new BoolKeyframeGroup(groupName, new BoolKeyframe(0f, value));
			this.keyframeGroups[propKey] = value2;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00029E9C File Offset: 0x0002809C
		public T GetGroup<T>(string propertyKey) where T : class
		{
			if (!this.keyframeGroups.ContainsKey(propertyKey))
			{
				Debug.Log("Key does not exist in sky profile, ignoring: " + propertyKey);
				return default(T);
			}
			return this.keyframeGroups[propertyKey] as T;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00029EE7 File Offset: 0x000280E7
		public IKeyframeGroup GetGroup(string propertyKey)
		{
			return this.keyframeGroups[propertyKey];
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00029EF8 File Offset: 0x000280F8
		public IKeyframeGroup GetGroupWithId(string groupId)
		{
			if (groupId == null)
			{
				return null;
			}
			foreach (string aKey in this.keyframeGroups)
			{
				IKeyframeGroup keyframeGroup = this.keyframeGroups[aKey];
				if (keyframeGroup.id == groupId)
				{
					return keyframeGroup;
				}
			}
			return null;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029DB1 File Offset: 0x00027FB1
		public ProfileGroupSection[] GetProfileDefinitions()
		{
			return this.groupDefinitions;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00029F68 File Offset: 0x00028168
		public ProfileGroupSection GetSectionInfo(string sectionKey)
		{
			foreach (ProfileGroupSection profileGroupSection in this.groupDefinitions)
			{
				if (profileGroupSection.sectionKey == sectionKey)
				{
					return profileGroupSection;
				}
			}
			return null;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00029F9F File Offset: 0x0002819F
		public bool IsManagedByTimeline(string propertyKey)
		{
			return this.timelineManagedKeys.Contains(propertyKey);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00029FB0 File Offset: 0x000281B0
		public void ValidateTimelineGroupKeys()
		{
			List<string> list = new List<string>();
			HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
			foreach (string text in this.timelineManagedKeys)
			{
				if (!this.IsManagedByTimeline(text) || !propertyKeysSet.Contains(text))
				{
					list.Add(text);
				}
			}
			foreach (string item in list)
			{
				if (this.timelineManagedKeys.Contains(item))
				{
					this.timelineManagedKeys.Remove(item);
				}
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002A078 File Offset: 0x00028278
		public List<ProfileGroupDefinition> GetGroupDefinitionsManagedByTimeline()
		{
			List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
			foreach (string propertyKey in this.timelineManagedKeys)
			{
				ProfileGroupDefinition groupDefinitionForKey = this.GetGroupDefinitionForKey(propertyKey);
				if (groupDefinitionForKey != null)
				{
					list.Add(groupDefinitionForKey);
				}
			}
			return list;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002A0E0 File Offset: 0x000282E0
		public List<ProfileGroupDefinition> GetGroupDefinitionsNotManagedByTimeline()
		{
			List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					if (!this.IsManagedByTimeline(profileGroupDefinition.propertyKey) && this.CanGroupBeOnTimeline(profileGroupDefinition))
					{
						list.Add(profileGroupDefinition);
					}
				}
			}
			return list;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002A14C File Offset: 0x0002834C
		public ProfileGroupDefinition GetGroupDefinitionForKey(string propertyKey)
		{
			ProfileGroupDefinition result = null;
			if (this.m_KeyToGroupInfo.TryGetValue(propertyKey, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002A170 File Offset: 0x00028370
		public void RebuildKeyToGroupInfoMapping()
		{
			this.m_KeyToGroupInfo = new Dictionary<string, ProfileGroupDefinition>();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					this.m_KeyToGroupInfo[profileGroupDefinition.propertyKey] = profileGroupDefinition;
				}
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002A1D0 File Offset: 0x000283D0
		public void TrimGroupToSingleKeyframe(string propertyKey)
		{
			IKeyframeGroup group = this.GetGroup(propertyKey);
			if (group == null)
			{
				return;
			}
			group.TrimToSingleKeyframe();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002A1F0 File Offset: 0x000283F0
		public bool CanGroupBeOnTimeline(ProfileGroupDefinition definition)
		{
			return definition.type != ProfileGroupDefinition.GroupType.Texture && (!definition.propertyKey.Contains("Star") || !definition.propertyKey.Contains("Density")) && !definition.propertyKey.Contains("Sprite") && definition.type != ProfileGroupDefinition.GroupType.Boolean;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002A248 File Offset: 0x00028448
		protected void MergeShaderKeywordsWithDefinitions()
		{
			ProfileFeatureSection[] features = this.profileDefinition.features;
			for (int i = 0; i < features.Length; i++)
			{
				foreach (ProfileFeatureDefinition profileFeatureDefinition in features[i].featureDefinitions)
				{
					string text = null;
					bool value = false;
					if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
					{
						text = profileFeatureDefinition.featureKey;
						value = profileFeatureDefinition.value;
					}
					else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
					{
						text = profileFeatureDefinition.featureKeys[profileFeatureDefinition.dropdownSelectedIndex];
						value = true;
					}
					if (text != null && !this.featureStatus.dict.ContainsKey(text))
					{
						this.SetFeatureEnabled(text, value);
					}
				}
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002A300 File Offset: 0x00028500
		public bool IsFeatureEnabled(string featureKey, bool recursive = true)
		{
			if (featureKey == null)
			{
				return false;
			}
			ProfileFeatureDefinition featureDefinition = this.profileDefinition.GetFeatureDefinition(featureKey);
			if (featureDefinition == null)
			{
				return false;
			}
			if (!this.featureStatus.dict.ContainsKey(featureKey) || !this.featureStatus[featureKey])
			{
				return false;
			}
			if (!recursive)
			{
				return true;
			}
			ProfileFeatureDefinition featureDefinition2;
			for (ProfileFeatureDefinition profileFeatureDefinition = featureDefinition; profileFeatureDefinition != null; profileFeatureDefinition = featureDefinition2)
			{
				featureDefinition2 = this.profileDefinition.GetFeatureDefinition(profileFeatureDefinition.dependsOnFeature);
				if (featureDefinition2 == null || featureDefinition2.featureKey == null)
				{
					break;
				}
				if (this.featureStatus[featureDefinition2.featureKey] != profileFeatureDefinition.dependsOnValue)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002A38E File Offset: 0x0002858E
		public void SetFeatureEnabled(string featureKey, bool value)
		{
			if (featureKey == null)
			{
				Debug.LogError("Can't set null feature key value");
				return;
			}
			this.featureStatus[featureKey] = value;
		}

		// Token: 0x04000A9F RID: 2719
		public const string DefaultShaderName = "Funly/Sky Studio/Skybox/3D Standard";

		// Token: 0x04000AA0 RID: 2720
		public const string DefaultLegacyShaderName = "Funly/Sky Studio/Skybox/3D Standard - Global Keywords";

		// Token: 0x04000AA1 RID: 2721
		[SerializeField]
		private Material m_SkyboxMaterial;

		// Token: 0x04000AA2 RID: 2722
		[SerializeField]
		private string m_ShaderName = "Funly/Sky Studio/Skybox/3D Standard";

		// Token: 0x04000AA3 RID: 2723
		public IProfileDefinition profileDefinition;

		// Token: 0x04000AA4 RID: 2724
		public List<string> timelineManagedKeys = new List<string>();

		// Token: 0x04000AA5 RID: 2725
		public KeyframeGroupDictionary keyframeGroups = new KeyframeGroupDictionary();

		// Token: 0x04000AA6 RID: 2726
		public BoolDictionary featureStatus = new BoolDictionary();

		// Token: 0x04000AA7 RID: 2727
		public LightningArtSet lightningArtSet;

		// Token: 0x04000AA8 RID: 2728
		public RainSplashArtSet rainSplashArtSet;

		// Token: 0x04000AA9 RID: 2729
		public Texture2D starLayer1DataTexture;

		// Token: 0x04000AAA RID: 2730
		public Texture2D starLayer2DataTexture;

		// Token: 0x04000AAB RID: 2731
		public Texture2D starLayer3DataTexture;

		// Token: 0x04000AAC RID: 2732
		[SerializeField]
		private int m_ProfileVersion = 2;

		// Token: 0x04000AAD RID: 2733
		private Dictionary<string, ProfileGroupDefinition> m_KeyToGroupInfo;
	}
}
