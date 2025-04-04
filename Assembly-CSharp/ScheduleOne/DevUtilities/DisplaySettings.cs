using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006EF RID: 1775
	[Serializable]
	public struct DisplaySettings
	{
		// Token: 0x06003033 RID: 12339 RVA: 0x000C8B78 File Offset: 0x000C6D78
		public static List<Resolution> GetResolutions()
		{
			Resolution[] resolutions = Screen.resolutions;
			RefreshRate refreshRateRatio = resolutions[resolutions.Length - 1].refreshRateRatio;
			float num = refreshRateRatio.numerator / refreshRateRatio.denominator;
			List<Resolution> list = new List<Resolution>();
			int i;
			Predicate<Resolution> <>9__0;
			int j;
			for (i = 0; i < resolutions.Length; i = j + 1)
			{
				List<Resolution> list2 = list;
				Predicate<Resolution> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Resolution x) => x.width == resolutions[i].width && x.height == resolutions[i].height));
				}
				if (!list2.Exists(match))
				{
					Resolution item = resolutions[i];
					if (item.refreshRateRatio.numerator / item.refreshRateRatio.denominator >= num - 0.1f)
					{
						list.Add(item);
					}
				}
				j = i;
			}
			return list;
		}

		// Token: 0x0400226B RID: 8811
		public int ResolutionIndex;

		// Token: 0x0400226C RID: 8812
		public DisplaySettings.EDisplayMode DisplayMode;

		// Token: 0x0400226D RID: 8813
		public bool VSync;

		// Token: 0x0400226E RID: 8814
		public int TargetFPS;

		// Token: 0x0400226F RID: 8815
		public float UIScale;

		// Token: 0x04002270 RID: 8816
		public float CameraBobbing;

		// Token: 0x04002271 RID: 8817
		public int ActiveDisplayIndex;

		// Token: 0x020006F0 RID: 1776
		public enum EDisplayMode
		{
			// Token: 0x04002273 RID: 8819
			Windowed,
			// Token: 0x04002274 RID: 8820
			FullscreenWindow,
			// Token: 0x04002275 RID: 8821
			ExclusiveFullscreen
		}
	}
}
