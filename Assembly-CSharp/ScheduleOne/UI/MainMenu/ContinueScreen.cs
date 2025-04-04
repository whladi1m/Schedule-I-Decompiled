using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B0A RID: 2826
	public class ContinueScreen : MainMenuScreen
	{
		// Token: 0x06004B79 RID: 19321 RVA: 0x0013C9DA File Offset: 0x0013ABDA
		private void Update()
		{
			if (base.IsOpen)
			{
				this.NotHostWarning.gameObject.SetActive(!Singleton<Lobby>.Instance.IsHost);
			}
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x0013CA01 File Offset: 0x0013AC01
		public void LoadGame(int index)
		{
			if (!Singleton<Lobby>.Instance.IsHost)
			{
				Console.LogWarning("Only the host can start the game.", null);
				return;
			}
			Singleton<LoadManager>.Instance.StartGame(LoadManager.SaveGames[index], false);
		}

		// Token: 0x040038C2 RID: 14530
		public RectTransform NotHostWarning;
	}
}
