using System;
using FishNet.Serializing.Helping;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Storage
{
	// Token: 0x0200088D RID: 2189
	[Serializable]
	public class StorableItemInstance : ItemInstance
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x000FA1C4 File Offset: 0x000F83C4
		[CodegenExclude]
		public virtual StoredItem StoredItem
		{
			get
			{
				if (base.Definition != null && base.Definition is StorableItemDefinition)
				{
					return (base.Definition as StorableItemDefinition).StoredItem;
				}
				string str = "StorableItemInstance has invalid definition: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return null;
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x000FA220 File Offset: 0x000F8420
		public StorableItemInstance()
		{
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x000FA228 File Offset: 0x000F8428
		public StorableItemInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
			if (definition as StorableItemDefinition == null)
			{
				Console.LogError("StoredItemInstance initialized with invalid definition!", null);
				return;
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x000FA24C File Offset: 0x000F844C
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new StorableItemInstance(base.Definition, quantity);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x000FA272 File Offset: 0x000F8472
		public override float GetMonetaryValue()
		{
			return (base.Definition as StorableItemDefinition).BasePurchasePrice;
		}
	}
}
