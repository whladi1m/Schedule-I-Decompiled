using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B6 RID: 1462
	[Serializable]
	public class Assault : Crime
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x00092D15 File Offset: 0x00090F15
		// (set) Token: 0x06002454 RID: 9300 RVA: 0x00092D1D File Offset: 0x00090F1D
		public override string CrimeName { get; protected set; } = "Assault";
	}
}
