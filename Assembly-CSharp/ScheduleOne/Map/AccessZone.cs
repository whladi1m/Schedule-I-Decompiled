using System;
using FishNet;
using ScheduleOne.Doors;
using ScheduleOne.Misc;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE3 RID: 3043
	public class AccessZone : MonoBehaviour
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x00166FA7 File Offset: 0x001651A7
		// (set) Token: 0x0600554D RID: 21837 RVA: 0x00166FAF File Offset: 0x001651AF
		public bool IsOpen { get; protected set; }

		// Token: 0x0600554E RID: 21838 RVA: 0x00166FB8 File Offset: 0x001651B8
		protected virtual void Awake()
		{
			this.IsOpen = true;
			this.SetIsOpen(false);
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x00166FC8 File Offset: 0x001651C8
		public virtual void SetIsOpen(bool open)
		{
			bool isOpen = this.IsOpen;
			this.IsOpen = open;
			foreach (DoorController doorController in this.Doors)
			{
				if (this.IsOpen)
				{
					doorController.PlayerAccess = EDoorAccess.Open;
				}
				else if (this.AllowExitWhenClosed)
				{
					doorController.PlayerAccess = EDoorAccess.ExitOnly;
				}
				else
				{
					doorController.PlayerAccess = EDoorAccess.Locked;
				}
			}
			for (int j = 0; j < this.Lights.Length; j++)
			{
				this.Lights[j].isOn = this.IsOpen;
			}
			if (this.IsOpen && !isOpen && this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
			if (!this.IsOpen && isOpen && this.onClose != null)
			{
				this.onClose.Invoke();
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsOpen && this.AutoCloseDoor)
			{
				foreach (DoorController doorController2 in this.Doors)
				{
					if ((!doorController2.openedByNPC || doorController2.timeSinceNPCSensed >= 1f) && doorController2.IsOpen && ((doorController2.timeSincePlayerSensed > 0.5f && doorController2.playerDetectedSinceOpened) || doorController2.timeSincePlayerSensed > 15f))
					{
						doorController2.SetIsOpen(null, false, EDoorSide.Interior);
					}
				}
			}
		}

		// Token: 0x04003F51 RID: 16209
		[Header("Settings")]
		public bool AllowExitWhenClosed;

		// Token: 0x04003F52 RID: 16210
		public bool AutoCloseDoor = true;

		// Token: 0x04003F53 RID: 16211
		[Header("References")]
		public DoorController[] Doors;

		// Token: 0x04003F54 RID: 16212
		public ToggleableLight[] Lights;

		// Token: 0x04003F55 RID: 16213
		public UnityEvent onOpen;

		// Token: 0x04003F56 RID: 16214
		public UnityEvent onClose;
	}
}
