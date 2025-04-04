using System;
using ScheduleOne.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x02000769 RID: 1897
	[CreateAssetMenu(fileName = "PhoneCallData", menuName = "ScriptableObjects/PhoneCallData", order = 1)]
	[Serializable]
	public class PhoneCallData : ScriptableObject
	{
		// Token: 0x060033E5 RID: 13285 RVA: 0x000D9052 File Offset: 0x000D7252
		public void Completed()
		{
			if (this.onCallCompleted != null)
			{
				this.onCallCompleted.Invoke();
			}
		}

		// Token: 0x0400253E RID: 9534
		public CallerID CallerID;

		// Token: 0x0400253F RID: 9535
		public PhoneCallData.Stage[] Stages;

		// Token: 0x04002540 RID: 9536
		public UnityEvent onCallCompleted;

		// Token: 0x0200076A RID: 1898
		[Serializable]
		public class Stage
		{
			// Token: 0x060033E7 RID: 13287 RVA: 0x000D9068 File Offset: 0x000D7268
			public void OnStageStart()
			{
				if (this.OnStartTriggers != null)
				{
					for (int i = 0; i < this.OnStartTriggers.Length; i++)
					{
						this.OnStartTriggers[i].Trigger();
					}
				}
			}

			// Token: 0x060033E8 RID: 13288 RVA: 0x000D90A0 File Offset: 0x000D72A0
			public void OnStageEnd()
			{
				if (this.OnDoneTriggers != null)
				{
					for (int i = 0; i < this.OnDoneTriggers.Length; i++)
					{
						this.OnDoneTriggers[i].Trigger();
					}
				}
			}

			// Token: 0x04002541 RID: 9537
			[TextArea(3, 10)]
			public string Text;

			// Token: 0x04002542 RID: 9538
			public SystemTrigger[] OnStartTriggers;

			// Token: 0x04002543 RID: 9539
			public SystemTrigger[] OnDoneTriggers;
		}
	}
}
