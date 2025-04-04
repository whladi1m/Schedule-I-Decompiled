using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041B RID: 1051
	[Serializable]
	public class PlantData : SaveData
	{
		// Token: 0x0600157B RID: 5499 RVA: 0x0005F840 File Offset: 0x0005DA40
		public PlantData(string seedID, float growthProgress, float yieldLevel, float qualityLevel, int[] activeBuds)
		{
			this.SeedID = seedID;
			this.GrowthProgress = growthProgress;
			this.YieldLevel = yieldLevel;
			this.QualityLevel = qualityLevel;
			this.ActiveBuds = activeBuds;
		}

		// Token: 0x04001422 RID: 5154
		public string SeedID;

		// Token: 0x04001423 RID: 5155
		public float GrowthProgress;

		// Token: 0x04001424 RID: 5156
		public float YieldLevel;

		// Token: 0x04001425 RID: 5157
		public float QualityLevel;

		// Token: 0x04001426 RID: 5158
		public int[] ActiveBuds;
	}
}
