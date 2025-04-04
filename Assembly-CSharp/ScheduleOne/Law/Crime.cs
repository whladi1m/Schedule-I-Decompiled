using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AA RID: 1450
	[Serializable]
	public class Crime
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x00092B65 File Offset: 0x00090D65
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x00092B6D File Offset: 0x00090D6D
		public virtual string CrimeName { get; protected set; } = "Crime";
	}
}
