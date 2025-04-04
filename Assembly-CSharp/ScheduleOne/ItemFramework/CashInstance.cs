using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000917 RID: 2327
	[Serializable]
	public class CashInstance : StorableItemInstance
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x0010B509 File Offset: 0x00109709
		// (set) Token: 0x06003F5E RID: 16222 RVA: 0x0010B511 File Offset: 0x00109711
		public float Balance { get; protected set; }

		// Token: 0x06003F5F RID: 16223 RVA: 0x000CFC51 File Offset: 0x000CDE51
		public CashInstance()
		{
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0010B51A File Offset: 0x0010971A
		public CashInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0010B524 File Offset: 0x00109724
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new CashInstance(base.Definition, quantity);
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0010B54A File Offset: 0x0010974A
		public void ChangeBalance(float amount)
		{
			this.SetBalance(this.Balance + amount, false);
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0010B55C File Offset: 0x0010975C
		public void SetBalance(float newBalance, bool blockClear = false)
		{
			this.Balance = Mathf.Clamp(newBalance, 0f, 1E+09f);
			if (this.Balance <= 0f && !blockClear)
			{
				base.RequestClearSlot();
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0010B5A8 File Offset: 0x001097A8
		public override ItemData GetItemData()
		{
			return new CashData(this.ID, this.Quantity, this.Balance);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x0010B5C1 File Offset: 0x001097C1
		public override float GetMonetaryValue()
		{
			return this.Balance;
		}

		// Token: 0x04002DA0 RID: 11680
		public const float MAX_BALANCE = 1E+09f;
	}
}
