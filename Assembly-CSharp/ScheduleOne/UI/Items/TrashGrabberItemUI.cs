using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B4F RID: 2895
	public class TrashGrabberItemUI : ItemUI
	{
		// Token: 0x06004CEE RID: 19694 RVA: 0x00144BD0 File Offset: 0x00142DD0
		public override void Setup(ItemInstance item)
		{
			this.trashGrabberInstance = (item as TrashGrabberInstance);
			base.Setup(item);
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x00144BE8 File Offset: 0x00142DE8
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ValueLabel.text = Mathf.FloorToInt(Mathf.Clamp01((float)this.trashGrabberInstance.GetTotalSize() / 20f) * 100f).ToString() + "%";
			base.UpdateUI();
		}

		// Token: 0x04003A3A RID: 14906
		public TextMeshProUGUI ValueLabel;

		// Token: 0x04003A3B RID: 14907
		protected TrashGrabberInstance trashGrabberInstance;
	}
}
