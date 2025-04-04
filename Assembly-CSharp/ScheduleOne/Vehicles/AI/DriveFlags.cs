using System;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007D2 RID: 2002
	[Serializable]
	public class DriveFlags
	{
		// Token: 0x060036AD RID: 13997 RVA: 0x000E6164 File Offset: 0x000E4364
		public void ResetFlags()
		{
			this.OverrideSpeed = false;
			this.OverriddenSpeed = 50f;
			this.OverriddenReverseSpeed = 10f;
			this.SpeedLimitMultiplier = 1f;
			this.IgnoreTrafficLights = false;
			this.UseRoads = true;
			this.StuckDetection = true;
			this.ObstacleMode = DriveFlags.EObstacleMode.Default;
			this.AutoBrakeAtDestination = true;
			this.TurnBasedSpeedReduction = true;
		}

		// Token: 0x04002799 RID: 10137
		public bool OverrideSpeed;

		// Token: 0x0400279A RID: 10138
		public float OverriddenSpeed = 50f;

		// Token: 0x0400279B RID: 10139
		public float OverriddenReverseSpeed = 10f;

		// Token: 0x0400279C RID: 10140
		public float SpeedLimitMultiplier = 1f;

		// Token: 0x0400279D RID: 10141
		public bool IgnoreTrafficLights;

		// Token: 0x0400279E RID: 10142
		public bool UseRoads = true;

		// Token: 0x0400279F RID: 10143
		public bool StuckDetection = true;

		// Token: 0x040027A0 RID: 10144
		public DriveFlags.EObstacleMode ObstacleMode;

		// Token: 0x040027A1 RID: 10145
		public bool AutoBrakeAtDestination = true;

		// Token: 0x040027A2 RID: 10146
		public bool TurnBasedSpeedReduction = true;

		// Token: 0x020007D3 RID: 2003
		public enum EObstacleMode
		{
			// Token: 0x040027A4 RID: 10148
			Default,
			// Token: 0x040027A5 RID: 10149
			IgnoreAll,
			// Token: 0x040027A6 RID: 10150
			IgnoreOnlySquishy
		}
	}
}
