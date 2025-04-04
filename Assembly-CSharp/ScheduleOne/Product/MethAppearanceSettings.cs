using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008CA RID: 2250
	[Serializable]
	public class MethAppearanceSettings
	{
		// Token: 0x06003CE0 RID: 15584 RVA: 0x000FF867 File Offset: 0x000FDA67
		public MethAppearanceSettings(Color32 mainColor, Color32 secondaryColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x0000494F File Offset: 0x00002B4F
		public MethAppearanceSettings()
		{
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x000FF87D File Offset: 0x000FDA7D
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear;
		}

		// Token: 0x04002C0A RID: 11274
		public Color32 MainColor;

		// Token: 0x04002C0B RID: 11275
		public Color32 SecondaryColor;
	}
}
