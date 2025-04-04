using System;
using System.Collections.Generic;
using ScheduleOne.Clothing;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B33 RID: 2867
	public class CharacterCreatorColor : CharacterCreatorField<Color>
	{
		// Token: 0x06004C5A RID: 19546 RVA: 0x00141978 File Offset: 0x0013FB78
		protected override void Awake()
		{
			base.Awake();
			if (this.UseClothingColors)
			{
				this.Colors = new List<Color>();
				foreach (EClothingColor color in CharacterCreatorColor.ClothingColorsToUse)
				{
					this.Colors.Add(color.GetActualColor());
				}
			}
			for (int j = 0; j < this.Colors.Count; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer);
				gameObject.transform.Find("Color").GetComponent<Image>().color = this.Colors[j];
				Color col = this.Colors[j];
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OptionClicked(col);
				});
				this.optionButtons.Add(gameObject.GetComponent<Button>());
			}
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x00141A70 File Offset: 0x0013FC70
		public override void ApplyValue()
		{
			base.ApplyValue();
			Button button = null;
			for (int i = 0; i < this.Colors.Count; i++)
			{
				if (ClothingColorExtensions.ColorEquals(base.value, this.Colors[i], 0.004f) && i < this.optionButtons.Count)
				{
					button = this.optionButtons[i];
					break;
				}
			}
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = true;
			}
			this.selectedButton = button;
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = false;
			}
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x00141B11 File Offset: 0x0013FD11
		public void OptionClicked(Color color)
		{
			base.value = color;
			this.WriteValue(true);
		}

		// Token: 0x040039B2 RID: 14770
		public static EClothingColor[] ClothingColorsToUse = new EClothingColor[]
		{
			EClothingColor.White,
			EClothingColor.LightGrey,
			EClothingColor.DarkGrey,
			EClothingColor.Charcoal,
			EClothingColor.Black,
			EClothingColor.Red,
			EClothingColor.Crimson,
			EClothingColor.Orange,
			EClothingColor.Tan,
			EClothingColor.Brown,
			EClothingColor.Yellow,
			EClothingColor.Lime,
			EClothingColor.DarkGreen,
			EClothingColor.Cyan,
			EClothingColor.SkyBlue,
			EClothingColor.Blue,
			EClothingColor.Navy,
			EClothingColor.Purple,
			EClothingColor.Magenta,
			EClothingColor.BrightPink
		};

		// Token: 0x040039B3 RID: 14771
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x040039B4 RID: 14772
		[Header("Settings")]
		public bool UseClothingColors;

		// Token: 0x040039B5 RID: 14773
		public List<Color> Colors;

		// Token: 0x040039B6 RID: 14774
		public GameObject OptionPrefab;

		// Token: 0x040039B7 RID: 14775
		private List<Button> optionButtons = new List<Button>();

		// Token: 0x040039B8 RID: 14776
		private Button selectedButton;
	}
}
