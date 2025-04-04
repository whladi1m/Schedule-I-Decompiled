using System;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;

namespace ScheduleOne.Tools
{
	// Token: 0x02000853 RID: 2131
	public class PlayerSmoothedVelocityCalculator : SmoothedVelocityCalculator
	{
		// Token: 0x06003A2A RID: 14890 RVA: 0x000F4F28 File Offset: 0x000F3128
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.Player.CurrentVehicle != null)
			{
				this.Velocity = this.Player.CurrentVehicle.GetComponent<LandVehicle>().VelocityCalculator.Velocity;
			}
		}

		// Token: 0x040029FB RID: 10747
		public Player Player;
	}
}
