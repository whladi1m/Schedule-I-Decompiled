using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008C0 RID: 2240
	[Serializable]
	public class CocaineAppearanceSettings
	{
		// Token: 0x06003CC3 RID: 15555 RVA: 0x000FF140 File Offset: 0x000FD340
		public CocaineAppearanceSettings(Color32 mainColor, Color32 secondaryColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0000494F File Offset: 0x00002B4F
		public CocaineAppearanceSettings()
		{
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x000FF156 File Offset: 0x000FD356
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear;
		}

		// Token: 0x04002BE9 RID: 11241
		public Color32 MainColor;

		// Token: 0x04002BEA RID: 11242
		public Color32 SecondaryColor;
	}
}
