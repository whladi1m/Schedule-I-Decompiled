using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B1 RID: 1457
	[Serializable]
	public class DrugTrafficking : Crime
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x00092C61 File Offset: 0x00090E61
		// (set) Token: 0x06002445 RID: 9285 RVA: 0x00092C69 File Offset: 0x00090E69
		public override string CrimeName { get; protected set; } = "Drug trafficking";
	}
}
