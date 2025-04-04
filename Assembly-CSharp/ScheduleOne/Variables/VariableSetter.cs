using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Variables
{
	// Token: 0x02000294 RID: 660
	[Serializable]
	public class VariableSetter
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0003D3C0 File Offset: 0x0003B5C0
		public void Execute()
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.VariableName, this.NewValue, true);
		}

		// Token: 0x04000E44 RID: 3652
		public string VariableName;

		// Token: 0x04000E45 RID: 3653
		public string NewValue;
	}
}
