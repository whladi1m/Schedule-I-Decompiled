using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041A RID: 1050
	[Serializable]
	public class OrganisationData : SaveData
	{
		// Token: 0x0600157A RID: 5498 RVA: 0x0005F82A File Offset: 0x0005DA2A
		public OrganisationData(string name, float netWorth)
		{
			this.Name = name;
			this.NetWorth = netWorth;
		}

		// Token: 0x04001420 RID: 5152
		public string Name;

		// Token: 0x04001421 RID: 5153
		public float NetWorth;
	}
}
