using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000134 RID: 308
	public class PlatformHelper
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00019537 File Offset: 0x00017737
		public static string GetCurrentPlatformSuffix()
		{
			return PlatformHelper.GetPlatformSuffix(Application.platform);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00019543 File Offset: 0x00017743
		private static string GetPlatformSuffix(RuntimePlatform platform)
		{
			return platform.ToString();
		}
	}
}
