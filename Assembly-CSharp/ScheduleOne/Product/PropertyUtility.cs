using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008E9 RID: 2281
	public class PropertyUtility : Singleton<PropertyUtility>
	{
		// Token: 0x06003E0E RID: 15886 RVA: 0x00106030 File Offset: 0x00104230
		protected override void Awake()
		{
			base.Awake();
			foreach (Property property in this.AllProperties)
			{
				this.PropertiesDict.Add(property.ID, property);
			}
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00106094 File Offset: 0x00104294
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0010609C File Offset: 0x0010429C
		public List<Property> GetProperties(int tier)
		{
			bool excludePostMixingRework = false;
			if (SaveManager.GetVersionNumber(Singleton<MetadataManager>.Instance.CreationVersion) < 27f)
			{
				excludePostMixingRework = true;
			}
			return this.AllProperties.FindAll((Property x) => x.Tier == tier && (!excludePostMixingRework || x.ImplementedPriorMixingRework));
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x001060F4 File Offset: 0x001042F4
		public List<Property> GetProperties(List<string> ids)
		{
			List<Property> list = new List<Property>();
			using (List<string>.Enumerator enumerator = ids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string id = enumerator.Current;
					if (this.AllProperties.FirstOrDefault((Property x) => x.ID == id) == null)
					{
						Console.LogWarning("PropertyUtility: Property ID '" + id + "' not found!", null);
					}
					else
					{
						list.Add(this.PropertiesDict[id]);
					}
				}
			}
			return this.AllProperties.FindAll((Property x) => ids.Contains(x.ID));
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x001061C8 File Offset: 0x001043C8
		public static PropertyUtility.PropertyData GetPropertyData(EProperty property)
		{
			return Singleton<PropertyUtility>.Instance.PropertyDatas.Find((PropertyUtility.PropertyData x) => x.Property == property);
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00106200 File Offset: 0x00104400
		public static PropertyUtility.DrugTypeData GetDrugTypeData(EDrugType drugType)
		{
			return Singleton<PropertyUtility>.Instance.DrugTypeDatas.Find((PropertyUtility.DrugTypeData x) => x.DrugType == drugType);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00106238 File Offset: 0x00104438
		public static List<Color32> GetOrderedPropertyColors(List<Property> properties)
		{
			properties.Sort((Property x, Property y) => x.Tier.CompareTo(y.Tier));
			List<Color32> list = new List<Color32>();
			foreach (Property property in properties)
			{
				list.Add(property.ProductColor);
			}
			return list;
		}

		// Token: 0x04002C97 RID: 11415
		public List<PropertyUtility.PropertyData> PropertyDatas = new List<PropertyUtility.PropertyData>();

		// Token: 0x04002C98 RID: 11416
		public List<PropertyUtility.DrugTypeData> DrugTypeDatas = new List<PropertyUtility.DrugTypeData>();

		// Token: 0x04002C99 RID: 11417
		public List<Property> AllProperties = new List<Property>();

		// Token: 0x04002C9A RID: 11418
		[Header("Test Mixing")]
		public List<ProductDefinition> Products = new List<ProductDefinition>();

		// Token: 0x04002C9B RID: 11419
		public List<PropertyItemDefinition> Properties = new List<PropertyItemDefinition>();

		// Token: 0x04002C9C RID: 11420
		private Dictionary<string, Property> PropertiesDict = new Dictionary<string, Property>();

		// Token: 0x020008EA RID: 2282
		[Serializable]
		public class PropertyData
		{
			// Token: 0x04002C9D RID: 11421
			public EProperty Property;

			// Token: 0x04002C9E RID: 11422
			public string Name;

			// Token: 0x04002C9F RID: 11423
			public string Description;

			// Token: 0x04002CA0 RID: 11424
			public Color Color;
		}

		// Token: 0x020008EB RID: 2283
		[Serializable]
		public class DrugTypeData
		{
			// Token: 0x04002CA1 RID: 11425
			public EDrugType DrugType;

			// Token: 0x04002CA2 RID: 11426
			public string Name;

			// Token: 0x04002CA3 RID: 11427
			public Color Color;
		}
	}
}
