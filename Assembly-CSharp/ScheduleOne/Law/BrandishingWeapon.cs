using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005BA RID: 1466
	[Serializable]
	public class BrandishingWeapon : Crime
	{
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x00092DA5 File Offset: 0x00090FA5
		// (set) Token: 0x06002460 RID: 9312 RVA: 0x00092DAD File Offset: 0x00090FAD
		public override string CrimeName { get; protected set; } = "Brandishing a weapon";
	}
}
