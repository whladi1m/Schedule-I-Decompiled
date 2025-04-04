using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CA RID: 970
	[Serializable]
	public class BusinessData : PropertyData
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x0005EC47 File Offset: 0x0005CE47
		public BusinessData(string propertyCode, bool isOwned, bool[] switchStates, LaunderOperationData[] launderingOperations, bool[] toggleableStates) : base(propertyCode, isOwned, switchStates, toggleableStates)
		{
			this.LaunderingOperations = launderingOperations;
		}

		// Token: 0x04001360 RID: 4960
		public LaunderOperationData[] LaunderingOperations;
	}
}
