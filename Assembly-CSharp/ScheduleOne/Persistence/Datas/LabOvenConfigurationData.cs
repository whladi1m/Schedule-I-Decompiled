using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F2 RID: 1010
	[Serializable]
	public class LabOvenConfigurationData : SaveData
	{
		// Token: 0x06001551 RID: 5457 RVA: 0x0005F1C6 File Offset: 0x0005D3C6
		public LabOvenConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x040013AF RID: 5039
		public ObjectFieldData Destination;
	}
}
