using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000422 RID: 1058
	[Serializable]
	public class PropertyData : SaveData
	{
		// Token: 0x06001583 RID: 5507 RVA: 0x0005F951 File Offset: 0x0005DB51
		public PropertyData(string propertyCode, bool isOwned, bool[] switchStates, bool[] toggleableStates)
		{
			this.PropertyCode = propertyCode;
			this.IsOwned = isOwned;
			this.SwitchStates = switchStates;
			this.ToggleableStates = toggleableStates;
		}

		// Token: 0x04001439 RID: 5177
		public string PropertyCode;

		// Token: 0x0400143A RID: 5178
		public bool IsOwned;

		// Token: 0x0400143B RID: 5179
		public bool[] SwitchStates;

		// Token: 0x0400143C RID: 5180
		public bool[] ToggleableStates;
	}
}
