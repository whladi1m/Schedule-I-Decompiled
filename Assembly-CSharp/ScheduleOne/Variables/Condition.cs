using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Variables
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	public class Condition
	{
		// Token: 0x06000D84 RID: 3460 RVA: 0x0003C388 File Offset: 0x0003A588
		public bool Evaluate()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return false;
			}
			BaseVariable variable = NetworkSingleton<VariableDatabase>.Instance.GetVariable(this.VariableName);
			if (variable == null)
			{
				Debug.LogError("Variable " + this.VariableName + " not found");
				return false;
			}
			return variable.EvaluateCondition(this.Operator, this.Value);
		}

		// Token: 0x04000E19 RID: 3609
		public string VariableName = "Variable Name";

		// Token: 0x04000E1A RID: 3610
		public Condition.EConditionType Operator = Condition.EConditionType.EqualTo;

		// Token: 0x04000E1B RID: 3611
		public string Value = "true";

		// Token: 0x0200028B RID: 651
		public enum EConditionType
		{
			// Token: 0x04000E1D RID: 3613
			GreaterThan,
			// Token: 0x04000E1E RID: 3614
			LessThan,
			// Token: 0x04000E1F RID: 3615
			EqualTo,
			// Token: 0x04000E20 RID: 3616
			NotEqualTo,
			// Token: 0x04000E21 RID: 3617
			GreaterThanOrEqualTo,
			// Token: 0x04000E22 RID: 3618
			LessThanOrEqualTo
		}
	}
}
