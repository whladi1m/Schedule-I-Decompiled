using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008C1 RID: 2241
	[CreateAssetMenu(fileName = "CocaineDefinition", menuName = "ScriptableObjects/Item Definitions/CocaineDefinition", order = 1)]
	[Serializable]
	public class CocaineDefinition : ProductDefinition
	{
		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x000FF186 File Offset: 0x000FD386
		// (set) Token: 0x06003CC7 RID: 15559 RVA: 0x000FF18E File Offset: 0x000FD38E
		public CocaineAppearanceSettings AppearanceSettings { get; private set; }

		// Token: 0x06003CC8 RID: 15560 RVA: 0x000FF197 File Offset: 0x000FD397
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			if (NetworkSingleton<ProductManager>.InstanceExists && !ProductManager.CocaineDiscovered)
			{
				NetworkSingleton<ProductManager>.Instance.SetCocaineDiscovered();
			}
			return new CocaineInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x000FF1BC File Offset: 0x000FD3BC
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, CocaineAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Coke definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = CocaineDefinition.GetAppearanceSettings(properties);
			}
			this.AppearanceSettings = _appearance;
			this.RockMaterial = new Material(this.RockMaterial);
			this.RockMaterial.color = this.AppearanceSettings.MainColor;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x000FF234 File Offset: 0x000FD434
		public override string GetSaveString()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new CocaineProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.AppearanceSettings).GetJson(true);
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x000FF2AC File Offset: 0x000FD4AC
		public static CocaineAppearanceSettings GetAppearanceSettings(List<Property> properties)
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
			Color32 mainColor = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.13f);
			Color32 secondaryColor = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			return new CocaineAppearanceSettings
			{
				MainColor = mainColor,
				SecondaryColor = secondaryColor
			};
		}

		// Token: 0x04002BEB RID: 11243
		[Header("Materials")]
		public Material RockMaterial;
	}
}
