using System;

namespace ScheduleOne.Variables
{
	// Token: 0x02000291 RID: 657
	[Serializable]
	public class VariableCreator
	{
		// Token: 0x04000E31 RID: 3633
		public string Name;

		// Token: 0x04000E32 RID: 3634
		public VariableDatabase.EVariableType Type;

		// Token: 0x04000E33 RID: 3635
		public string InitialValue = string.Empty;

		// Token: 0x04000E34 RID: 3636
		public bool Persistent = true;

		// Token: 0x04000E35 RID: 3637
		public EVariableMode Mode;
	}
}
