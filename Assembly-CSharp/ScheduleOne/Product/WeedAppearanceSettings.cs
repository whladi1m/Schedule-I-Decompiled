using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008F2 RID: 2290
	[Serializable]
	public class WeedAppearanceSettings
	{
		// Token: 0x06003E25 RID: 15909 RVA: 0x00106385 File Offset: 0x00104585
		public WeedAppearanceSettings(Color32 mainColor, Color32 secondaryColor, Color32 leafColor, Color32 stemColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
			this.LeafColor = leafColor;
			this.StemColor = stemColor;
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0000494F File Offset: 0x00002B4F
		public WeedAppearanceSettings()
		{
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x001063AC File Offset: 0x001045AC
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear || this.LeafColor == Color.clear || this.StemColor == Color.clear;
		}

		// Token: 0x04002CAC RID: 11436
		public Color32 MainColor;

		// Token: 0x04002CAD RID: 11437
		public Color32 SecondaryColor;

		// Token: 0x04002CAE RID: 11438
		public Color32 LeafColor;

		// Token: 0x04002CAF RID: 11439
		public Color32 StemColor;
	}
}
