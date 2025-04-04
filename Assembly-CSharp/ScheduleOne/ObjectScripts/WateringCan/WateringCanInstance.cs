using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BCB RID: 3019
	[Serializable]
	public class WateringCanInstance : StorableItemInstance
	{
		// Token: 0x060054D5 RID: 21717 RVA: 0x000CFC51 File Offset: 0x000CDE51
		public WateringCanInstance()
		{
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x001652CC File Offset: 0x001634CC
		public WateringCanInstance(ItemDefinition definition, int quantity, float fillAmount) : base(definition, quantity)
		{
			this.CurrentFillAmount = fillAmount;
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x001652E0 File Offset: 0x001634E0
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new WateringCanInstance(base.Definition, quantity, this.CurrentFillAmount);
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x0016530C File Offset: 0x0016350C
		public void ChangeFillAmount(float change)
		{
			this.CurrentFillAmount = Mathf.Clamp(this.CurrentFillAmount + change, 0f, 15f);
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x0016533E File Offset: 0x0016353E
		public override ItemData GetItemData()
		{
			return new WateringCanData(this.ID, this.Quantity, this.CurrentFillAmount);
		}

		// Token: 0x04003ED6 RID: 16086
		public float CurrentFillAmount;
	}
}
