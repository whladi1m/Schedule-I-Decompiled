using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001CA RID: 458
	public abstract class ColorHelper
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x0002D3D6 File Offset: 0x0002B5D6
		public static Color ColorWithHex(uint hex)
		{
			return ColorHelper.ColorWithHexAlpha(hex << 8 | 255U);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0002D3E8 File Offset: 0x0002B5E8
		public static Color ColorWithHexAlpha(uint hex)
		{
			float r = (hex >> 24 & 255U) / 255f;
			float g = (hex >> 16 & 255U) / 255f;
			float b = (hex >> 8 & 255U) / 255f;
			float a = (hex & 255U) / 255f;
			return new Color(r, g, b, a);
		}
	}
}
