using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x0200028E RID: 654
	public class NumberVariable : Variable<float>
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x0003C4A0 File Offset: 0x0003A6A0
		public NumberVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, float value) : base(name, replicationMode, persistent, mode, owner, value)
		{
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0003C4B4 File Offset: 0x0003A6B4
		public override bool TryDeserialize(string valueString, out float value)
		{
			float num;
			if (float.TryParse(valueString, out num))
			{
				value = num;
				return true;
			}
			value = 0f;
			return false;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0003C4D8 File Offset: 0x0003A6D8
		public override bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			float num;
			if (!this.TryDeserialize(value, out num))
			{
				return false;
			}
			if (operation == Condition.EConditionType.EqualTo)
			{
				return this.Value == num;
			}
			if (operation == Condition.EConditionType.NotEqualTo)
			{
				return this.Value != num;
			}
			if (operation == Condition.EConditionType.GreaterThan)
			{
				return this.Value > num;
			}
			if (operation == Condition.EConditionType.LessThan)
			{
				return this.Value < num;
			}
			if (operation == Condition.EConditionType.GreaterThanOrEqualTo)
			{
				return this.Value >= num;
			}
			if (operation == Condition.EConditionType.LessThanOrEqualTo)
			{
				return this.Value <= num;
			}
			Console.LogError("Invalid operation " + operation.ToString() + " for number variable", null);
			return false;
		}
	}
}
