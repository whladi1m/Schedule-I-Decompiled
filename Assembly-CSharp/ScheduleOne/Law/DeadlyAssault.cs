using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B7 RID: 1463
	[Serializable]
	public class DeadlyAssault : Crime
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x00092D39 File Offset: 0x00090F39
		// (set) Token: 0x06002457 RID: 9303 RVA: 0x00092D41 File Offset: 0x00090F41
		public override string CrimeName { get; protected set; } = "Assault with a deadly weapon";
	}
}
