using System;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B09 RID: 2825
	public class ConfirmOverwriteScreen : MainMenuScreen
	{
		// Token: 0x06004B76 RID: 19318 RVA: 0x0013C9AB File Offset: 0x0013ABAB
		public void Initialize(int index)
		{
			this.slotIndex = index;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0013C9B4 File Offset: 0x0013ABB4
		public void Confirm()
		{
			this.Close(false);
			this.SetupScreen.Initialize(this.slotIndex);
			this.SetupScreen.Open(false);
		}

		// Token: 0x040038C0 RID: 14528
		public SetupScreen SetupScreen;

		// Token: 0x040038C1 RID: 14529
		private int slotIndex;
	}
}
