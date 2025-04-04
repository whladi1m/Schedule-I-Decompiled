using System;
using ScheduleOne.Persistence;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B14 RID: 2836
	public class NewGameScreen : MainMenuScreen
	{
		// Token: 0x06004BA1 RID: 19361 RVA: 0x0013D1D0 File Offset: 0x0013B3D0
		public void SlotSelected(int slotIndex)
		{
			if (LoadManager.SaveGames[slotIndex] != null)
			{
				this.ConfirmOverwriteScreen.Initialize(slotIndex);
				this.ConfirmOverwriteScreen.Open(true);
				return;
			}
			this.SetupScreen.Initialize(slotIndex);
			this.SetupScreen.Open(false);
			this.Close(false);
		}

		// Token: 0x040038E8 RID: 14568
		public ConfirmOverwriteScreen ConfirmOverwriteScreen;

		// Token: 0x040038E9 RID: 14569
		public SetupScreen SetupScreen;
	}
}
