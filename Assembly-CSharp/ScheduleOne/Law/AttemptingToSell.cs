using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B5 RID: 1461
	[Serializable]
	public class AttemptingToSell : Crime
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x00092CF1 File Offset: 0x00090EF1
		// (set) Token: 0x06002451 RID: 9297 RVA: 0x00092CF9 File Offset: 0x00090EF9
		public override string CrimeName { get; protected set; } = "Attempting to sell illicit items";
	}
}
