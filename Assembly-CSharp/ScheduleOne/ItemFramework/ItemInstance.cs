using System;
using FishNet.Serializing.Helping;
using ScheduleOne.Equipping;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000930 RID: 2352
	[Serializable]
	public abstract class ItemInstance
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003F9F RID: 16287 RVA: 0x0010BB1C File Offset: 0x00109D1C
		[CodegenExclude]
		public ItemDefinition Definition
		{
			get
			{
				if (this.definition == null)
				{
					this.definition = Registry.GetItem(this.ID);
					if (this.definition == null)
					{
						Console.LogError("Failed to find definition with ID: " + this.ID, null);
					}
				}
				return this.definition;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003FA0 RID: 16288 RVA: 0x0010BB72 File Offset: 0x00109D72
		[CodegenExclude]
		public virtual string Name
		{
			get
			{
				return this.Definition.Name;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x0010BB7F File Offset: 0x00109D7F
		[CodegenExclude]
		public virtual string Description
		{
			get
			{
				return this.Definition.Description;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x0010BB8C File Offset: 0x00109D8C
		[CodegenExclude]
		public virtual Sprite Icon
		{
			get
			{
				return this.Definition.Icon;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x0010BB99 File Offset: 0x00109D99
		[CodegenExclude]
		public virtual EItemCategory Category
		{
			get
			{
				return this.Definition.Category;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003FA4 RID: 16292 RVA: 0x0010BBA6 File Offset: 0x00109DA6
		[CodegenExclude]
		public virtual int StackLimit
		{
			get
			{
				return this.Definition.StackLimit;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x0010BBB3 File Offset: 0x00109DB3
		[CodegenExclude]
		public virtual Color LabelDisplayColor
		{
			get
			{
				return this.Definition.LabelDisplayColor;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x0010BBC0 File Offset: 0x00109DC0
		[CodegenExclude]
		public virtual Equippable Equippable
		{
			get
			{
				return this.Definition.Equippable;
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0010BBCD File Offset: 0x00109DCD
		public ItemInstance()
		{
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0010BBE7 File Offset: 0x00109DE7
		public ItemInstance(ItemDefinition definition, int quantity)
		{
			this.definition = definition;
			this.Quantity = quantity;
			this.ID = definition.ID;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0010BC1B File Offset: 0x00109E1B
		public virtual bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			return other != null && !(other.ID != this.ID) && (!checkQuantities || this.Quantity + other.Quantity <= this.StackLimit);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0010BACC File Offset: 0x00109CCC
		public virtual ItemInstance GetCopy(int overrideQuantity = -1)
		{
			Console.LogError("This should be overridden in the definition class!", null);
			return null;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0010BC52 File Offset: 0x00109E52
		public virtual bool IsValidInstance()
		{
			return this.ID != string.Empty && this.Definition != null && this.Quantity > 0;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0010BC7F File Offset: 0x00109E7F
		protected void InvokeDataChange()
		{
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0010BC94 File Offset: 0x00109E94
		public void SetQuantity(int quantity)
		{
			if (quantity < 0)
			{
				Debug.LogError("SetQuantity called and passed quantity less than zero.");
				return;
			}
			if (quantity > this.StackLimit && quantity > this.Quantity)
			{
				Debug.LogError("SetQuantity called and passed quantity larger than stack limit.");
				return;
			}
			this.Quantity = quantity;
			this.InvokeDataChange();
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0010BCD0 File Offset: 0x00109ED0
		public void ChangeQuantity(int change)
		{
			int num = this.Quantity + change;
			if (num < 0)
			{
				Debug.LogError("ChangeQuantity called and passed quantity less than zero.");
				return;
			}
			if (num > this.StackLimit)
			{
				Debug.LogError("ChangeQuantity called and passed quantity larger than stack limit.");
				return;
			}
			this.Quantity = num;
			this.InvokeDataChange();
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0010BD16 File Offset: 0x00109F16
		public virtual ItemData GetItemData()
		{
			return new ItemData(this.ID, this.Quantity);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0010BD29 File Offset: 0x00109F29
		public virtual float GetMonetaryValue()
		{
			return 0f;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0010BD30 File Offset: 0x00109F30
		public void RequestClearSlot()
		{
			if (this.requestClearSlot != null)
			{
				this.requestClearSlot();
			}
		}

		// Token: 0x04002DDF RID: 11743
		[CodegenExclude]
		protected ItemDefinition definition;

		// Token: 0x04002DE0 RID: 11744
		public string ID = string.Empty;

		// Token: 0x04002DE1 RID: 11745
		public int Quantity = 1;

		// Token: 0x04002DE2 RID: 11746
		[CodegenExclude]
		public Action onDataChanged;

		// Token: 0x04002DE3 RID: 11747
		[CodegenExclude]
		public Action requestClearSlot;
	}
}
