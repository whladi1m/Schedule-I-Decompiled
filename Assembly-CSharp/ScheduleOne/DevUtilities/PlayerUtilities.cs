using System;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E3 RID: 1763
	public static class PlayerUtilities
	{
		// Token: 0x06002FF6 RID: 12278 RVA: 0x000C7F85 File Offset: 0x000C6185
		public static void OpenMenu()
		{
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000C7FBD File Offset: 0x000C61BD
		public static void CloseMenu(bool reenableLookInstantly = false, bool reenableInventory = true)
		{
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			if (reenableLookInstantly)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			}
			if (reenableInventory)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
		}
	}
}
