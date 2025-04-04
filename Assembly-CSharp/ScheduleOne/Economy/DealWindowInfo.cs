using System;

namespace ScheduleOne.Economy
{
	// Token: 0x02000663 RID: 1635
	public struct DealWindowInfo
	{
		// Token: 0x06002D1F RID: 11551 RVA: 0x000BCDEF File Offset: 0x000BAFEF
		public DealWindowInfo(int startTime, int endTime)
		{
			this.StartTime = startTime;
			this.EndTime = endTime;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000BCDFF File Offset: 0x000BAFFF
		public static DealWindowInfo GetWindowInfo(EDealWindow window)
		{
			switch (window)
			{
			case EDealWindow.Morning:
				return DealWindowInfo.Morning;
			case EDealWindow.Afternoon:
				return DealWindowInfo.Afternoon;
			case EDealWindow.Night:
				return DealWindowInfo.Night;
			case EDealWindow.LateNight:
				return DealWindowInfo.LateNight;
			default:
				return DealWindowInfo.Morning;
			}
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000BCE38 File Offset: 0x000BB038
		public static EDealWindow GetWindow(int time)
		{
			if (time >= DealWindowInfo.Morning.StartTime && time < DealWindowInfo.Morning.EndTime)
			{
				return EDealWindow.Morning;
			}
			if (time >= DealWindowInfo.Afternoon.StartTime && time < DealWindowInfo.Afternoon.EndTime)
			{
				return EDealWindow.Afternoon;
			}
			if (time >= DealWindowInfo.Night.StartTime && time < DealWindowInfo.Night.EndTime)
			{
				return EDealWindow.Night;
			}
			return EDealWindow.LateNight;
		}

		// Token: 0x04002023 RID: 8227
		public const int WINDOW_DURATION_MINS = 360;

		// Token: 0x04002024 RID: 8228
		public const int WINDOW_COUNT = 4;

		// Token: 0x04002025 RID: 8229
		public int StartTime;

		// Token: 0x04002026 RID: 8230
		public int EndTime;

		// Token: 0x04002027 RID: 8231
		public static readonly DealWindowInfo Morning = new DealWindowInfo(600, 1200);

		// Token: 0x04002028 RID: 8232
		public static readonly DealWindowInfo Afternoon = new DealWindowInfo(1200, 1800);

		// Token: 0x04002029 RID: 8233
		public static readonly DealWindowInfo Night = new DealWindowInfo(1800, 2400);

		// Token: 0x0400202A RID: 8234
		public static readonly DealWindowInfo LateNight = new DealWindowInfo(0, 600);
	}
}
