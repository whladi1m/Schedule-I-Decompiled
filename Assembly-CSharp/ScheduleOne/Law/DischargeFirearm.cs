using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005BB RID: 1467
	[Serializable]
	public class DischargeFirearm : Crime
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x00092DC9 File Offset: 0x00090FC9
		// (set) Token: 0x06002463 RID: 9315 RVA: 0x00092DD1 File Offset: 0x00090FD1
		public override string CrimeName { get; protected set; } = "Discharge of a firearm in a public place";
	}
}
