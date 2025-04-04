using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE6 RID: 3046
	public class DarkMarketAccessZone : TimedAccessZone
	{
		// Token: 0x0600556C RID: 21868 RVA: 0x001676D2 File Offset: 0x001658D2
		protected override bool GetIsOpen()
		{
			return NetworkSingleton<DarkMarket>.Instance.IsOpen && NetworkSingleton<DarkMarket>.Instance.Unlocked && base.GetIsOpen();
		}
	}
}
