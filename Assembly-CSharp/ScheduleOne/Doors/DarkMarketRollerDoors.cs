using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;

namespace ScheduleOne.Doors
{
	// Token: 0x02000673 RID: 1651
	public class DarkMarketRollerDoors : SensorRollerDoors
	{
		// Token: 0x06002DCE RID: 11726 RVA: 0x000C059F File Offset: 0x000BE79F
		protected override bool CanOpen()
		{
			return NetworkSingleton<DarkMarket>.Instance.IsOpen;
		}
	}
}
