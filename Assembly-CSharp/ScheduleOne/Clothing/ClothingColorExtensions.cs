using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200073A RID: 1850
	public static class ClothingColorExtensions
	{
		// Token: 0x06003207 RID: 12807 RVA: 0x000CFF8F File Offset: 0x000CE18F
		public static Color GetActualColor(this EClothingColor color)
		{
			return Singleton<ClothingUtility>.Instance.GetColorData(color).ActualColor;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000CFFA1 File Offset: 0x000CE1A1
		public static Color GetLabelColor(this EClothingColor color)
		{
			return Singleton<ClothingUtility>.Instance.GetColorData(color).LabelColor;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000CFFB3 File Offset: 0x000CE1B3
		public static string GetLabel(this EClothingColor color)
		{
			return color.ToString();
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000CFFC4 File Offset: 0x000CE1C4
		public static EClothingColor GetClothingColor(Color color)
		{
			foreach (object obj in Enum.GetValues(typeof(EClothingColor)))
			{
				EClothingColor eclothingColor = (EClothingColor)obj;
				if (ClothingColorExtensions.ColorEquals(eclothingColor.GetActualColor(), color, 0.004f))
				{
					return eclothingColor;
				}
			}
			string str = "Could not find clothing color for color ";
			Color color2 = color;
			Console.LogError(str + color2.ToString(), null);
			return EClothingColor.White;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000D005C File Offset: 0x000CE25C
		public static bool ColorEquals(Color a, Color b, float tolerance = 0.004f)
		{
			return a.r <= b.r + tolerance && a.g <= b.g + tolerance && a.b <= b.b + tolerance && a.r >= b.r - tolerance && a.g >= b.g - tolerance && a.b >= b.b - tolerance;
		}
	}
}
