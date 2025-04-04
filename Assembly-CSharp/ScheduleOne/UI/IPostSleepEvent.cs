using System;

namespace ScheduleOne.UI
{
	// Token: 0x02000A07 RID: 2567
	public interface IPostSleepEvent
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x0600454E RID: 17742
		bool IsRunning { get; }

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x0600454F RID: 17743
		int Order { get; }

		// Token: 0x06004550 RID: 17744
		void StartEvent();
	}
}
