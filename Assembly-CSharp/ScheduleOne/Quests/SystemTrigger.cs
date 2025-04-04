using System;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FF RID: 767
	[Serializable]
	public class SystemTrigger
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x0004BD3C File Offset: 0x00049F3C
		public bool Trigger()
		{
			if (this.Conditions.Evaluate())
			{
				for (int i = 0; i < this.onEvaluateTrueQuestSetters.Length; i++)
				{
					this.onEvaluateTrueQuestSetters[i].Execute();
				}
				for (int j = 0; j < this.onEvaluateTrueVariableSetters.Length; j++)
				{
					this.onEvaluateTrueVariableSetters[j].Execute();
				}
				if (this.onEvaluateTrue != null)
				{
					this.onEvaluateTrue.Invoke();
				}
				return true;
			}
			for (int k = 0; k < this.onEvaluateFalseQuestSetters.Length; k++)
			{
				this.onEvaluateFalseQuestSetters[k].Execute();
			}
			for (int l = 0; l < this.onEvaluateFalseVariableSetters.Length; l++)
			{
				this.onEvaluateFalseVariableSetters[l].Execute();
			}
			if (this.onEvaluateFalse != null)
			{
				this.onEvaluateFalse.Invoke();
			}
			return false;
		}

		// Token: 0x04001126 RID: 4390
		public Conditions Conditions;

		// Token: 0x04001127 RID: 4391
		[Header("True")]
		public VariableSetter[] onEvaluateTrueVariableSetters;

		// Token: 0x04001128 RID: 4392
		public QuestStateSetter[] onEvaluateTrueQuestSetters;

		// Token: 0x04001129 RID: 4393
		public UnityEvent onEvaluateTrue;

		// Token: 0x0400112A RID: 4394
		[Header("False")]
		public VariableSetter[] onEvaluateFalseVariableSetters;

		// Token: 0x0400112B RID: 4395
		public QuestStateSetter[] onEvaluateFalseQuestSetters;

		// Token: 0x0400112C RID: 4396
		public UnityEvent onEvaluateFalse;
	}
}
