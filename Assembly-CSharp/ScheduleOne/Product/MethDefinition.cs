using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008CB RID: 2251
	[CreateAssetMenu(fileName = "MethDefinition", menuName = "ScriptableObjects/Item Definitions/MethDefinition", order = 1)]
	[Serializable]
	public class MethDefinition : ProductDefinition
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x000FF8AD File Offset: 0x000FDAAD
		// (set) Token: 0x06003CE4 RID: 15588 RVA: 0x000FF8B5 File Offset: 0x000FDAB5
		public MethAppearanceSettings AppearanceSettings { get; private set; }

		// Token: 0x06003CE5 RID: 15589 RVA: 0x000FF8BE File Offset: 0x000FDABE
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			if (NetworkSingleton<ProductManager>.InstanceExists && !ProductManager.MethDiscovered)
			{
				NetworkSingleton<ProductManager>.Instance.SetMethDiscovered();
			}
			return new MethInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000FF8E4 File Offset: 0x000FDAE4
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, MethAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Meth definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = MethDefinition.GetAppearanceSettings(properties);
			}
			this.AppearanceSettings = _appearance;
			this.CrystalMaterial = new Material(this.CrystalMaterial);
			this.CrystalMaterial.color = this.AppearanceSettings.MainColor;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x000FF95C File Offset: 0x000FDB5C
		public override string GetSaveString()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new MethProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.AppearanceSettings).GetJson(true);
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x000FF9D4 File Offset: 0x000FDBD4
		public static MethAppearanceSettings GetAppearanceSettings(List<Property> properties)
		{
			properties.Sort((Property x, Property y) => x.Tier.CompareTo(y.Tier));
			List<Color32> list = new List<Color32>();
			foreach (Property property in properties)
			{
				list.Add(property.ProductColor);
			}
			if (list.Count == 1)
			{
				list.Add(list[0]);
			}
			Color32 a = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Color32 a2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Color32 mainColor = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.2f);
			Color32 secondaryColor = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			return new MethAppearanceSettings
			{
				MainColor = mainColor,
				SecondaryColor = secondaryColor
			};
		}

		// Token: 0x04002C0C RID: 11276
		public Material CrystalMaterial;

		// Token: 0x04002C0D RID: 11277
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
