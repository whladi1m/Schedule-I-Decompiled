using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000935 RID: 2357
	public static class ItemQuality
	{
		// Token: 0x06003FCC RID: 16332 RVA: 0x0010C180 File Offset: 0x0010A380
		public static EQuality GetQuality(float qualityScalar)
		{
			if (qualityScalar > 0.9f)
			{
				return EQuality.Heavenly;
			}
			if (qualityScalar > 0.75f)
			{
				return EQuality.Premium;
			}
			if (qualityScalar > 0.4f)
			{
				return EQuality.Standard;
			}
			if (qualityScalar > 0.25f)
			{
				return EQuality.Poor;
			}
			return EQuality.Trash;
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0010C1AC File Offset: 0x0010A3AC
		public static Color GetColor(EQuality quality)
		{
			switch (quality)
			{
			case EQuality.Trash:
				return ItemQuality.Trash_Color;
			case EQuality.Poor:
				return ItemQuality.Poor_Color;
			case EQuality.Standard:
				return ItemQuality.Standard_Color;
			case EQuality.Premium:
				return ItemQuality.Premium_Color;
			case EQuality.Heavenly:
				return ItemQuality.Heavenly_Color;
			default:
				Console.LogWarning("Quality color not found!", null);
				return Color.white;
			}
		}

		// Token: 0x04002DF8 RID: 11768
		public const float Heavenly_Threshold = 0.9f;

		// Token: 0x04002DF9 RID: 11769
		public const float Premium_Threshold = 0.75f;

		// Token: 0x04002DFA RID: 11770
		public const float Standard_Threshold = 0.4f;

		// Token: 0x04002DFB RID: 11771
		public const float Poor_Threshold = 0.25f;

		// Token: 0x04002DFC RID: 11772
		public static Color Heavenly_Color = new Color32(byte.MaxValue, 200, 50, byte.MaxValue);

		// Token: 0x04002DFD RID: 11773
		public static Color Premium_Color = new Color32(225, 75, byte.MaxValue, byte.MaxValue);

		// Token: 0x04002DFE RID: 11774
		public static Color Standard_Color = new Color32(100, 190, byte.MaxValue, byte.MaxValue);

		// Token: 0x04002DFF RID: 11775
		public static Color Poor_Color = new Color32(80, 145, 50, byte.MaxValue);

		// Token: 0x04002E00 RID: 11776
		public static Color Trash_Color = new Color32(125, 50, 50, byte.MaxValue);
	}
}
