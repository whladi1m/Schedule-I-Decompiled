using System;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009DF RID: 2527
	public class ClothingSlotUI : ItemSlotUI
	{
		// Token: 0x06004432 RID: 17458 RVA: 0x0011D8AD File Offset: 0x0011BAAD
		private void Start()
		{
			this.SlotTypeImage.sprite = Singleton<ClothingUtility>.Instance.GetSlotData(this.SlotType).Icon;
		}

		// Token: 0x0400320E RID: 12814
		public EClothingSlot SlotType;

		// Token: 0x0400320F RID: 12815
		public Image SlotTypeImage;
	}
}
