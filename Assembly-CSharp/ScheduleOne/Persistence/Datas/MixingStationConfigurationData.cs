using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F3 RID: 1011
	[Serializable]
	public class MixingStationConfigurationData : SaveData
	{
		// Token: 0x06001552 RID: 5458 RVA: 0x0005F1D5 File Offset: 0x0005D3D5
		public MixingStationConfigurationData(ObjectFieldData destination, NumberFieldData threshold)
		{
			this.Destination = destination;
			this.Threshold = threshold;
		}

		// Token: 0x040013B0 RID: 5040
		public ObjectFieldData Destination;

		// Token: 0x040013B1 RID: 5041
		public NumberFieldData Threshold;
	}
}
