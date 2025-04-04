using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000430 RID: 1072
	[Serializable]
	public class VariableData : SaveData
	{
		// Token: 0x06001597 RID: 5527 RVA: 0x0005FC10 File Offset: 0x0005DE10
		public VariableData(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0005FC26 File Offset: 0x0005DE26
		public VariableData()
		{
			this.Name = "";
			this.Value = "";
		}

		// Token: 0x04001467 RID: 5223
		public string Name;

		// Token: 0x04001468 RID: 5224
		public string Value;
	}
}
