using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using TMPro;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B41 RID: 2881
	public class ItemUI_Cash : ItemUI
	{
		// Token: 0x06004CA5 RID: 19621 RVA: 0x00142C07 File Offset: 0x00140E07
		public override void Setup(ItemInstance item)
		{
			this.cashInstance = (item as CashInstance);
			base.Setup(item);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x00142C1C File Offset: 0x00140E1C
		public override void UpdateUI()
		{
			base.UpdateUI();
			if (this.Destroyed)
			{
				return;
			}
			this.SetDisplayedBalance(this.cashInstance.Balance);
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x00142C3E File Offset: 0x00140E3E
		public void SetDisplayedBalance(float balance)
		{
			this.AmountLabel.text = MoneyManager.FormatAmount(balance, false, false);
		}

		// Token: 0x040039F9 RID: 14841
		protected CashInstance cashInstance;

		// Token: 0x040039FA RID: 14842
		public TextMeshProUGUI AmountLabel;
	}
}
