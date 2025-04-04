using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Variables;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E8 RID: 744
	public class Quest_DownToBusiness : Quest
	{
		// Token: 0x060010A5 RID: 4261 RVA: 0x0004AA28 File Offset: 0x00048C28
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0004AA30 File Offset: 0x00048C30
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onDayPass = (Action)Delegate.Combine(instance.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004AA60 File Offset: 0x00048C60
		private void DayPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Completed)
			{
				float num = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
				num += 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Days_Since_Tutorial_Completed", num.ToString(), true);
			}
		}
	}
}
