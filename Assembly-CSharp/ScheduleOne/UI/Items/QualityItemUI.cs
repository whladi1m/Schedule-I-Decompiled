using System;
using ScheduleOne.ItemFramework;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B4E RID: 2894
	public class QualityItemUI : ItemUI
	{
		// Token: 0x06004CEB RID: 19691 RVA: 0x00144B83 File Offset: 0x00142D83
		public override void Setup(ItemInstance item)
		{
			this.qualityItemInstance = (item as QualityItemInstance);
			base.Setup(item);
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x00144B98 File Offset: 0x00142D98
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.QualityIcon.enabled = true;
			this.QualityIcon.color = ItemQuality.GetColor(this.qualityItemInstance.Quality);
			base.UpdateUI();
		}

		// Token: 0x04003A38 RID: 14904
		public Image QualityIcon;

		// Token: 0x04003A39 RID: 14905
		protected QualityItemInstance qualityItemInstance;
	}
}
