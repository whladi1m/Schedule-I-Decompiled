using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CF RID: 975
	public class GameData : SaveData
	{
		// Token: 0x0600151F RID: 5407 RVA: 0x0005ED94 File Offset: 0x0005CF94
		public GameData(string organisationName, int seed, GameSettings settings)
		{
			this.OrganisationName = organisationName;
			this.Seed = seed;
			this.Settings = settings;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0005EDB1 File Offset: 0x0005CFB1
		public GameData()
		{
			this.OrganisationName = "Organisation";
			this.Seed = 0;
		}

		// Token: 0x04001378 RID: 4984
		public string OrganisationName;

		// Token: 0x04001379 RID: 4985
		public int Seed;

		// Token: 0x0400137A RID: 4986
		public GameSettings Settings;
	}
}
