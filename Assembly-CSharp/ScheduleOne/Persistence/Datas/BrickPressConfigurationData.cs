using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003EB RID: 1003
	[Serializable]
	public class BrickPressConfigurationData : SaveData
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x0005F141 File Offset: 0x0005D341
		public BrickPressConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x040013A4 RID: 5028
		public ObjectFieldData Destination;
	}
}
