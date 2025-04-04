using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Properties;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008D8 RID: 2264
	[Serializable]
	public class ProductItemInstance : QualityItemInstance
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x001008C8 File Offset: 0x000FEAC8
		[CodegenExclude]
		public PackagingDefinition AppliedPackaging
		{
			get
			{
				if (this.packaging == null && this.PackagingID != string.Empty)
				{
					this.packaging = (Registry.GetItem(this.PackagingID) as PackagingDefinition);
					if (this.packaging == null)
					{
						Console.LogError("Failed to load packaging with ID (" + this.PackagingID + ")", null);
					}
				}
				return this.packaging;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06003D29 RID: 15657 RVA: 0x0010093A File Offset: 0x000FEB3A
		[CodegenExclude]
		public int Amount
		{
			get
			{
				if (!(this.AppliedPackaging != null))
				{
					return 1;
				}
				return this.AppliedPackaging.Quantity;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x00100957 File Offset: 0x000FEB57
		public override string Name
		{
			get
			{
				return base.Name + ((this.packaging == null) ? " (Unpackaged)" : string.Empty);
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003D2B RID: 15659 RVA: 0x0010097E File Offset: 0x000FEB7E
		[CodegenExclude]
		public override Equippable Equippable
		{
			get
			{
				return this.GetEquippable();
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003D2C RID: 15660 RVA: 0x00100986 File Offset: 0x000FEB86
		[CodegenExclude]
		public override StoredItem StoredItem
		{
			get
			{
				return this.GetStoredItem();
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x0010098E File Offset: 0x000FEB8E
		[CodegenExclude]
		public override Sprite Icon
		{
			get
			{
				return this.GetIcon();
			}
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00100996 File Offset: 0x000FEB96
		public ProductItemInstance()
		{
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x001009AC File Offset: 0x000FEBAC
		public ProductItemInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition _packaging = null) : base(definition, quantity, quality)
		{
			this.packaging = _packaging;
			if (this.packaging != null)
			{
				this.PackagingID = this.packaging.ID;
				return;
			}
			this.PackagingID = string.Empty;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00100A00 File Offset: 0x000FEC00
		public override bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			if (!(other is ProductItemInstance))
			{
				return false;
			}
			if ((other as ProductItemInstance).AppliedPackaging != null)
			{
				if (this.AppliedPackaging == null)
				{
					return false;
				}
				if ((other as ProductItemInstance).AppliedPackaging.ID != this.AppliedPackaging.ID)
				{
					return false;
				}
			}
			else if (this.AppliedPackaging != null)
			{
				return false;
			}
			return base.CanStackWith(other, checkQuantities);
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00100A78 File Offset: 0x000FEC78
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new ProductItemInstance(base.Definition, quantity, this.Quality, this.AppliedPackaging);
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00100AAC File Offset: 0x000FECAC
		public virtual void SetPackaging(PackagingDefinition def)
		{
			this.packaging = def;
			if (this.packaging != null)
			{
				this.PackagingID = this.packaging.ID;
			}
			else
			{
				this.PackagingID = string.Empty;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00100AFF File Offset: 0x000FECFF
		private Equippable GetEquippable()
		{
			if (this.AppliedPackaging != null)
			{
				return this.AppliedPackaging.Equippable_Filled;
			}
			return base.Equippable;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00100B21 File Offset: 0x000FED21
		private StoredItem GetStoredItem()
		{
			if (this.AppliedPackaging != null)
			{
				return this.AppliedPackaging.StoredItem_Filled;
			}
			return base.StoredItem;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00100B43 File Offset: 0x000FED43
		public virtual void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			visuals.ResetVisuals();
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00100B4B File Offset: 0x000FED4B
		private Sprite GetIcon()
		{
			if (this.AppliedPackaging != null)
			{
				return Singleton<ProductIconManager>.Instance.GetIcon(this.ID, this.AppliedPackaging.ID, false);
			}
			return base.Icon;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00100B7E File Offset: 0x000FED7E
		public override ItemData GetItemData()
		{
			return new ProductItemData(this.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00100BA8 File Offset: 0x000FEDA8
		public virtual float GetAddictiveness()
		{
			return (base.Definition as ProductDefinition).GetAddictiveness();
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00100BBC File Offset: 0x000FEDBC
		public float GetSimilarity(ProductDefinition other, EQuality quality)
		{
			ProductDefinition productDefinition = base.Definition as ProductDefinition;
			float num = 0f;
			if (other.DrugType == productDefinition.DrugType)
			{
				num = 0.4f;
			}
			int num2 = 0;
			for (int i = 0; i < other.Properties.Count; i++)
			{
				if (productDefinition.HasProperty(other.Properties[i]))
				{
					num2++;
				}
			}
			for (int j = 0; j < productDefinition.Properties.Count; j++)
			{
				if (!other.HasProperty(productDefinition.Properties[j]))
				{
					num2--;
				}
			}
			float num3 = 0.3f;
			int num4 = Mathf.Max(productDefinition.Properties.Count, other.Properties.Count);
			if (num4 > 0)
			{
				num3 *= Mathf.Clamp01((float)num2 / (float)num4);
			}
			float num5 = Mathf.Clamp((float)this.Quality / (float)quality, 0f, 1f) * 0.3f;
			return Mathf.Clamp01(num + num3 + num5);
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00100CBC File Offset: 0x000FEEBC
		public virtual void ApplyEffectsToNPC(NPC npc)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ApplyToNPC(npc);
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00100D30 File Offset: 0x000FEF30
		public virtual void ClearEffectsFromNPC(NPC npc)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ClearFromNPC(npc);
			}
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00100DA4 File Offset: 0x000FEFA4
		public virtual void ApplyEffectsToPlayer(Player player)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ApplyToPlayer(player);
			}
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00100E18 File Offset: 0x000FF018
		public virtual void ClearEffectsFromPlayer(Player Player)
		{
			List<Property> list = new List<Property>();
			list.AddRange((base.Definition as ProductDefinition).Properties);
			list = (from x in list
			orderby x.Tier
			select x).ToList<Property>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ClearFromPlayer(Player);
			}
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00100E8A File Offset: 0x000FF08A
		public override float GetMonetaryValue()
		{
			if (this.definition == null)
			{
				Console.LogWarning("ProductItemInstance.GetMonetaryValue() - Definition is null", null);
				return 0f;
			}
			return (this.definition as ProductDefinition).MarketValue * (float)this.Quantity;
		}

		// Token: 0x04002C3F RID: 11327
		public string PackagingID = string.Empty;

		// Token: 0x04002C40 RID: 11328
		[CodegenExclude]
		private PackagingDefinition packaging;
	}
}
