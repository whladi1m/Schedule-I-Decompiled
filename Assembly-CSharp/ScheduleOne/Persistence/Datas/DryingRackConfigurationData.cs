using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F0 RID: 1008
	[Serializable]
	public class DryingRackConfigurationData : SaveData
	{
		// Token: 0x0600154F RID: 5455 RVA: 0x0005F1A1 File Offset: 0x0005D3A1
		public DryingRackConfigurationData(QualityFieldData targetquality, ObjectFieldData destination)
		{
			this.TargetQuality = targetquality;
			this.Destination = destination;
		}

		// Token: 0x040013AC RID: 5036
		public QualityFieldData TargetQuality;

		// Token: 0x040013AD RID: 5037
		public ObjectFieldData Destination;
	}
}
