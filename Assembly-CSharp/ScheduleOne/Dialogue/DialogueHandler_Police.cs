using System;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006A8 RID: 1704
	public class DialogueHandler_Police : ControlledDialogueHandler
	{
		// Token: 0x06002EED RID: 12013 RVA: 0x000C4397 File Offset: 0x000C2597
		protected override void Awake()
		{
			base.Awake();
			this.officer = (base.NPC as PoliceOfficer);
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000C43B0 File Offset: 0x000C25B0
		public override void Hovered()
		{
			base.Hovered();
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000C43B8 File Offset: 0x000C25B8
		public override void Interacted()
		{
			base.Interacted();
			if (this.CanTalk_Checkpoint())
			{
				this.officer.PlayVO(EVOLineType.Question);
				base.InitializeDialogue(this.CheckpointRequestDialogue.name, true, "ENTRY");
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000C43EC File Offset: 0x000C25EC
		private bool CanTalk_Checkpoint()
		{
			return this.officer.behaviour.activeBehaviour != null && this.officer.behaviour.activeBehaviour is CheckpointBehaviour && !(this.officer.behaviour.activeBehaviour as CheckpointBehaviour).IsSearching;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000C4448 File Offset: 0x000C2648
		protected override int CheckBranch(string branchLabel)
		{
			if (!(branchLabel == "BRANCH_VEHICLE_EXISTS"))
			{
				return base.CheckBranch(branchLabel);
			}
			LandVehicle lastDrivenVehicle = Player.Local.LastDrivenVehicle;
			CheckpointBehaviour checkpointBehaviour = this.officer.CheckpointBehaviour;
			if (lastDrivenVehicle != null && (checkpointBehaviour.Checkpoint.SearchArea1.vehicles.Contains(lastDrivenVehicle) || checkpointBehaviour.Checkpoint.SearchArea2.vehicles.Contains(lastDrivenVehicle)))
			{
				checkpointBehaviour.StartSearch(lastDrivenVehicle.NetworkObject, Player.Local.NetworkObject);
				return 1;
			}
			return 0;
		}

		// Token: 0x04002164 RID: 8548
		[Header("References")]
		public DialogueContainer CheckpointRequestDialogue;

		// Token: 0x04002165 RID: 8549
		private PoliceOfficer officer;
	}
}
