using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE4 RID: 3044
	public class AutoshopAccessZone : NPCPresenceAccessZone
	{
		// Token: 0x06005551 RID: 21841 RVA: 0x0016711E File Offset: 0x0016531E
		public override void SetIsOpen(bool open)
		{
			base.SetIsOpen(open);
			if (this.rollerDoorOpen != open)
			{
				this.rollerDoorOpen = open;
				this.RollerDoorAnim.Play(this.rollerDoorOpen ? "Roller door open" : "Roller door close");
			}
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x00167158 File Offset: 0x00165358
		protected override void MinPass()
		{
			if (this.TargetNPC == null)
			{
				return;
			}
			this.SetIsOpen(this.DetectionZone.bounds.Contains(this.TargetNPC.Avatar.CenterPoint) || this.VehicleDetection.closestVehicle != null);
		}

		// Token: 0x04003F57 RID: 16215
		public Animation RollerDoorAnim;

		// Token: 0x04003F58 RID: 16216
		public VehicleDetector VehicleDetection;

		// Token: 0x04003F59 RID: 16217
		private bool rollerDoorOpen = true;
	}
}
