using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200092C RID: 2348
	public class IntegerItemInstance : StorableItemInstance
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x000CFC51 File Offset: 0x000CDE51
		public IntegerItemInstance()
		{
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0010BA36 File Offset: 0x00109C36
		public IntegerItemInstance(ItemDefinition definition, int quantity, int value) : base(definition, quantity)
		{
			this.Value = value;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0010BA48 File Offset: 0x00109C48
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new IntegerItemInstance(base.Definition, quantity, this.Value);
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0010BA74 File Offset: 0x00109C74
		public void ChangeValue(int change)
		{
			this.Value += change;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0010BA97 File Offset: 0x00109C97
		public void SetValue(int value)
		{
			this.Value = value;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0010BAB3 File Offset: 0x00109CB3
		public override ItemData GetItemData()
		{
			return new IntegerItemData(this.ID, this.Quantity, this.Value);
		}

		// Token: 0x04002DC8 RID: 11720
		public int Value;
	}
}
