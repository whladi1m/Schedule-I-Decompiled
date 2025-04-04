using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000193 RID: 403
	public static class ColorBlendingExtensions
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x000260A4 File Offset: 0x000242A4
		public static Color Clear(this Color color)
		{
			return new Color(color.r, color.g, color.b, 0f);
		}
	}
}
