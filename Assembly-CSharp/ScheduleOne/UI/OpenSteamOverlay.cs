using System;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x0200099B RID: 2459
	public class OpenSteamOverlay : MonoBehaviour
	{
		// Token: 0x06004281 RID: 17025 RVA: 0x00116E90 File Offset: 0x00115090
		public void OpenOverlay()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			OpenSteamOverlay.EType type = this.Type;
			if (type == OpenSteamOverlay.EType.Store)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(3164500U), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				return;
			}
			if (type != OpenSteamOverlay.EType.CustomLink)
			{
				return;
			}
			SteamFriends.ActivateGameOverlayToWebPage(this.CustomLink, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
		}

		// Token: 0x04003078 RID: 12408
		public const uint APP_ID = 3164500U;

		// Token: 0x04003079 RID: 12409
		public OpenSteamOverlay.EType Type;

		// Token: 0x0400307A RID: 12410
		public string CustomLink;

		// Token: 0x0200099C RID: 2460
		public enum EType
		{
			// Token: 0x0400307C RID: 12412
			Store,
			// Token: 0x0400307D RID: 12413
			CustomLink
		}
	}
}
