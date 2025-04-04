using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008F3 RID: 2291
	[CreateAssetMenu(fileName = "WeedDefinition", menuName = "ScriptableObjects/Item Definitions/WeedDefinition", order = 1)]
	[Serializable]
	public class WeedDefinition : ProductDefinition
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x00106415 File Offset: 0x00104615
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new WeedInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x00106420 File Offset: 0x00104620
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, WeedAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Weed definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = WeedDefinition.GetAppearanceSettings(properties);
			}
			this.appearance = _appearance;
			this.MainMat = new Material(this.MainMat);
			this.MainMat.color = this.appearance.MainColor;
			this.SecondaryMat = new Material(this.SecondaryMat);
			this.SecondaryMat.color = this.appearance.SecondaryColor;
			this.LeafMat = new Material(this.LeafMat);
			this.LeafMat.color = this.appearance.LeafColor;
			this.StemMat = new Material(this.StemMat);
			this.StemMat.color = this.appearance.StemColor;
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0010651C File Offset: 0x0010471C
		public override string GetSaveString()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new WeedProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.appearance).GetJson(true);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00106594 File Offset: 0x00104794
		public static WeedAppearanceSettings GetAppearanceSettings(List<Property> properties)
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
			Color32 a = new Color32(90, 100, 70, byte.MaxValue);
			Color32 a2 = new Color32(120, 120, 80, byte.MaxValue);
			Color32 color = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.15f);
			Color32 color2 = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			Color32 a3 = new Color32(0, 0, 0, byte.MaxValue);
			return new WeedAppearanceSettings
			{
				MainColor = color,
				SecondaryColor = color2,
				LeafColor = Color32.Lerp(color, color2, 0.5f),
				StemColor = Color32.Lerp(a3, color, 0.8f)
			};
		}

		// Token: 0x04002CB0 RID: 11440
		[Header("Weed Materials")]
		public Material MainMat;

		// Token: 0x04002CB1 RID: 11441
		public Material SecondaryMat;

		// Token: 0x04002CB2 RID: 11442
		public Material LeafMat;

		// Token: 0x04002CB3 RID: 11443
		public Material StemMat;

		// Token: 0x04002CB4 RID: 11444
		private WeedAppearanceSettings appearance;
	}
}
