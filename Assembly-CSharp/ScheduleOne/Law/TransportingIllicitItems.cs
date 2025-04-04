using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B3 RID: 1459
	[Serializable]
	public class TransportingIllicitItems : Crime
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x00092CA9 File Offset: 0x00090EA9
		// (set) Token: 0x0600244B RID: 9291 RVA: 0x00092CB1 File Offset: 0x00090EB1
		public override string CrimeName { get; protected set; } = "Transporting illicit items";
	}
}
