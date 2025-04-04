using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x02000730 RID: 1840
	public class ClothingUtility : Singleton<ClothingUtility>
	{
		// Token: 0x060031F6 RID: 12790 RVA: 0x000CFCB4 File Offset: 0x000CDEB4
		protected override void Awake()
		{
			base.Awake();
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					if (this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color) == null)
					{
						Debug.LogError("Color " + color.ToString() + " is missing from the ColorDataList");
					}
				}
			}
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000CFD60 File Offset: 0x000CDF60
		private void OnValidate()
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					if (this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color) == null)
					{
						this.ColorDataList.Add(new ClothingUtility.ColorData
						{
							ColorType = color,
							ActualColor = Color.white,
							LabelColor = Color.white
						});
					}
				}
			}
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingSlot)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingSlot slot = (EClothingSlot)enumerator.Current;
					if (this.ClothingSlotDataList.Find((ClothingUtility.ClothingSlotData x) => x.Slot == slot) == null)
					{
						this.ClothingSlotDataList.Add(new ClothingUtility.ClothingSlotData
						{
							Slot = slot,
							Name = slot.ToString(),
							Icon = null
						});
					}
				}
			}
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000CFEBC File Offset: 0x000CE0BC
		public ClothingUtility.ColorData GetColorData(EClothingColor color)
		{
			return this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color);
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000CFEF0 File Offset: 0x000CE0F0
		public ClothingUtility.ClothingSlotData GetSlotData(EClothingSlot slot)
		{
			return this.ClothingSlotDataList.Find((ClothingUtility.ClothingSlotData x) => x.Slot == slot);
		}

		// Token: 0x040023BC RID: 9148
		public List<ClothingUtility.ColorData> ColorDataList = new List<ClothingUtility.ColorData>();

		// Token: 0x040023BD RID: 9149
		public List<ClothingUtility.ClothingSlotData> ClothingSlotDataList = new List<ClothingUtility.ClothingSlotData>();

		// Token: 0x02000731 RID: 1841
		[Serializable]
		public class ColorData
		{
			// Token: 0x040023BE RID: 9150
			public EClothingColor ColorType;

			// Token: 0x040023BF RID: 9151
			public Color ActualColor;

			// Token: 0x040023C0 RID: 9152
			public Color LabelColor;
		}

		// Token: 0x02000732 RID: 1842
		[Serializable]
		public class ClothingSlotData
		{
			// Token: 0x040023C1 RID: 9153
			public EClothingSlot Slot;

			// Token: 0x040023C2 RID: 9154
			public string Name;

			// Token: 0x040023C3 RID: 9155
			public Sprite Icon;
		}
	}
}
