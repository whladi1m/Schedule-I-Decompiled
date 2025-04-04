using System;
using ScheduleOne.Police;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000690 RID: 1680
	public class DialogueController_Police : DialogueController
	{
		// Token: 0x06002E7E RID: 11902 RVA: 0x000C2C75 File Offset: 0x000C0E75
		protected override void Start()
		{
			base.Start();
			this.officer = (this.npc as PoliceOfficer);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000C2C90 File Offset: 0x000C0E90
		public override bool CanStartDialogue()
		{
			return !this.officer.PursuitBehaviour.Active && !this.officer.VehiclePursuitBehaviour.Active && !this.officer.BodySearchBehaviour.Active && (!this.officer.CheckpointBehaviour.Active || !this.officer.CheckpointBehaviour.IsSearching) && base.CanStartDialogue();
		}

		// Token: 0x04002126 RID: 8486
		private PoliceOfficer officer;
	}
}
