using System;
using ScheduleOne.Doors;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Building.Doors
{
	// Token: 0x02000781 RID: 1921
	public class PropertyDoorController : DoorController
	{
		// Token: 0x0600344C RID: 13388 RVA: 0x000DBED0 File Offset: 0x000DA0D0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Building.Doors.PropertyDoorController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000DBEEF File Offset: 0x000DA0EF
		public void Unlock()
		{
			this.PlayerAccess = EDoorAccess.Open;
			this.IsUnlocked = true;
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000DBF00 File Offset: 0x000DA100
		private void CheckClose()
		{
			if (!base.IsOpen)
			{
				return;
			}
			if (!this.IsUnlocked)
			{
				return;
			}
			if (base.timeInCurrentState < 2f)
			{
				return;
			}
			Player nearestWantedPlayer = this.GetNearestWantedPlayer();
			if (nearestWantedPlayer == null)
			{
				return;
			}
			if (Vector3.Distance(base.transform.position, nearestWantedPlayer.Avatar.CenterPoint) < 20f)
			{
				base.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000DBF6C File Offset: 0x000DA16C
		protected override bool CanPlayerAccess(EDoorSide side, out string reason)
		{
			if (side == EDoorSide.Exterior)
			{
				Player nearestWantedPlayer = this.GetNearestWantedPlayer();
				if (nearestWantedPlayer != null && Vector3.Distance(nearestWantedPlayer.transform.position, base.transform.position) < 15f)
				{
					PoliceOfficer nearestOfficer = nearestWantedPlayer.CrimeData.NearestOfficer;
					float num = 100000f;
					if (nearestOfficer != null)
					{
						num = Vector3.Distance(nearestOfficer.Avatar.CenterPoint, nearestWantedPlayer.Avatar.CenterPoint);
					}
					if (nearestWantedPlayer.CrimeData.TimeSinceSighted < 5f || num < 15f)
					{
						reason = "Police are nearby!";
						return false;
					}
				}
			}
			return base.CanPlayerAccess(side, out reason);
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000DC014 File Offset: 0x000DA214
		private Player GetNearestWantedPlayer()
		{
			Player player = null;
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && (player == null || Vector3.Distance(base.transform.position, Player.PlayerList[i].Avatar.CenterPoint) < Vector3.Distance(base.transform.position, player.Avatar.CenterPoint)))
				{
					player = Player.PlayerList[i];
				}
			}
			return player;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000DC0AF File Offset: 0x000DA2AF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000DC0C8 File Offset: 0x000DA2C8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Building.Doors.PropertyDoorControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000DC0E1 File Offset: 0x000DA2E1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000DC0F0 File Offset: 0x000DA2F0
		protected virtual void dll()
		{
			base.Awake();
			this.PlayerAccess = EDoorAccess.ExitOnly;
			if (this.Property != null)
			{
				this.Property.onThisPropertyAcquired.AddListener(new UnityAction(this.Unlock));
			}
			base.InvokeRepeating("CheckClose", 0f, 1f);
		}

		// Token: 0x04002583 RID: 9603
		public const float WANTED_PLAYER_CLOSE_DISTANCE = 20f;

		// Token: 0x04002584 RID: 9604
		public Property Property;

		// Token: 0x04002585 RID: 9605
		private bool IsUnlocked;

		// Token: 0x04002586 RID: 9606
		private bool dll_Excuted;

		// Token: 0x04002587 RID: 9607
		private bool dll_Excuted;
	}
}
