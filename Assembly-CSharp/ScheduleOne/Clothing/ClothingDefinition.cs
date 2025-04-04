using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200072E RID: 1838
	[CreateAssetMenu(fileName = "ClothingDefinition", menuName = "ScriptableObjects/ClothingDefinition", order = 1)]
	[Serializable]
	public class ClothingDefinition : StorableItemDefinition
	{
		// Token: 0x060031EF RID: 12783 RVA: 0x000CFBEC File Offset: 0x000CDDEC
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new ClothingInstance(this, quantity, EClothingColor.White);
		}

		// Token: 0x040023B6 RID: 9142
		public EClothingSlot Slot;

		// Token: 0x040023B7 RID: 9143
		public EClothingApplicationType ApplicationType;

		// Token: 0x040023B8 RID: 9144
		public string ClothingAssetPath = "Path/To/Clothing/Asset";

		// Token: 0x040023B9 RID: 9145
		public bool Colorable = true;

		// Token: 0x040023BA RID: 9146
		public List<EClothingSlot> SlotsToBlock = new List<EClothingSlot>();
	}
}
