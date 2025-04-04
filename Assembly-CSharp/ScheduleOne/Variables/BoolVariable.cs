using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x02000289 RID: 649
	public class BoolVariable : Variable<bool>
	{
		// Token: 0x06000D81 RID: 3457 RVA: 0x0003C2E7 File Offset: 0x0003A4E7
		public BoolVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, bool value) : base(name, replicationMode, persistent, mode, owner, value)
		{
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003C2F8 File Offset: 0x0003A4F8
		public override bool TryDeserialize(string valueString, out bool value)
		{
			if (valueString.ToLower() == "true")
			{
				value = true;
				return true;
			}
			if (valueString.ToLower() == "false")
			{
				value = false;
				return true;
			}
			value = false;
			return false;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003C32C File Offset: 0x0003A52C
		public override bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			bool flag;
			if (!this.TryDeserialize(value, out flag))
			{
				return false;
			}
			if (operation == Condition.EConditionType.EqualTo)
			{
				return this.Value == flag;
			}
			if (operation == Condition.EConditionType.NotEqualTo)
			{
				return this.Value != flag;
			}
			Console.LogError("Invalid operation " + operation.ToString() + " for bool variable", null);
			return false;
		}
	}
}
