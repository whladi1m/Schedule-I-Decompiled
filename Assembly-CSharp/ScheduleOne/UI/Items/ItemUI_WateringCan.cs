using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B42 RID: 2882
	public class ItemUI_WateringCan : ItemUI
	{
		// Token: 0x06004CA9 RID: 19625 RVA: 0x00142C5B File Offset: 0x00140E5B
		public override void Setup(ItemInstance item)
		{
			this.wcInstance = (item as WateringCanInstance);
			base.Setup(item);
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x00142C70 File Offset: 0x00140E70
		public override void UpdateUI()
		{
			base.UpdateUI();
			if (this.Destroyed)
			{
				return;
			}
			if (this.wcInstance == null)
			{
				return;
			}
			this.AmountLabel.text = ((float)Mathf.RoundToInt(this.wcInstance.CurrentFillAmount * 10f) / 10f).ToString() + "L";
		}

		// Token: 0x040039FB RID: 14843
		protected WateringCanInstance wcInstance;

		// Token: 0x040039FC RID: 14844
		public Text AmountLabel;
	}
}
