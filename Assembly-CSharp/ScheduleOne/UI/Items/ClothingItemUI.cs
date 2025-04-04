using System;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B43 RID: 2883
	public class ClothingItemUI : ItemUI
	{
		// Token: 0x06004CAC RID: 19628 RVA: 0x00142CD0 File Offset: 0x00140ED0
		public override void UpdateUI()
		{
			base.UpdateUI();
			ClothingInstance clothingInstance = this.itemInstance as ClothingInstance;
			if (this.itemInstance != null && (this.itemInstance.Definition as ClothingDefinition).Colorable)
			{
				this.IconImg.color = clothingInstance.Color.GetActualColor();
			}
			else
			{
				this.IconImg.color = Color.white;
			}
			if (this.itemInstance != null)
			{
				this.ClothingTypeIcon.sprite = Singleton<ClothingUtility>.Instance.GetSlotData((this.itemInstance.Definition as ClothingDefinition).Slot).Icon;
				return;
			}
			this.ClothingTypeIcon.sprite = null;
		}

		// Token: 0x040039FD RID: 14845
		public Image ClothingTypeIcon;
	}
}
