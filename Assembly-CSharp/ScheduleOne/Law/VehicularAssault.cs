using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B0 RID: 1456
	[Serializable]
	public class VehicularAssault : Crime
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x00092C3D File Offset: 0x00090E3D
		// (set) Token: 0x06002442 RID: 9282 RVA: 0x00092C45 File Offset: 0x00090E45
		public override string CrimeName { get; protected set; } = "Vehicular assault";
	}
}
