using System;

namespace ScheduleOne.Variables
{
	// Token: 0x0200028C RID: 652
	[Serializable]
	public class Conditions
	{
		// Token: 0x06000D86 RID: 3462 RVA: 0x0003C408 File Offset: 0x0003A608
		public bool Evaluate()
		{
			bool flag = false;
			for (int i = 0; i < this.ConditionList.Length; i++)
			{
				if (this.ConditionList[i].Evaluate())
				{
					flag = true;
					if (this.EvaluationType != Conditions.EEvaluationType.And)
					{
						return true;
					}
				}
				else if (this.EvaluationType == Conditions.EEvaluationType.And)
				{
					return false;
				}
			}
			for (int j = 0; j < this.QuestConditionList.Length; j++)
			{
				if (this.QuestConditionList[j].Evaluate())
				{
					flag = true;
					if (this.EvaluationType != Conditions.EEvaluationType.And)
					{
						return true;
					}
				}
				else if (this.EvaluationType == Conditions.EEvaluationType.And)
				{
					return false;
				}
			}
			return flag || this.ConditionList.Length + this.QuestConditionList.Length == 0;
		}

		// Token: 0x04000E23 RID: 3619
		public Conditions.EEvaluationType EvaluationType;

		// Token: 0x04000E24 RID: 3620
		public Condition[] ConditionList;

		// Token: 0x04000E25 RID: 3621
		public QuestCondition[] QuestConditionList;

		// Token: 0x0200028D RID: 653
		public enum EEvaluationType
		{
			// Token: 0x04000E27 RID: 3623
			And,
			// Token: 0x04000E28 RID: 3624
			Or
		}
	}
}
