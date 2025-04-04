using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F3 RID: 755
	public class Quest_SinkOrSwim : Quest
	{
		// Token: 0x060010C5 RID: 4293 RVA: 0x0004B013 File Offset: 0x00049213
		protected override void Awake()
		{
			base.Awake();
			this.LoanSharkGraves.gameObject.SetActive(false);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0004B02C File Offset: 0x0004922C
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.SleepStart));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.CheckArrival));
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.UpdateName));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.UpdateName));
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0004ADC3 File Offset: 0x00048FC3
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0004B0D1 File Offset: 0x000492D1
		private void HourPass()
		{
			if (this.Entries[0].State == EQuestState.Active)
			{
				this.UpdateName();
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0004B0F0 File Offset: 0x000492F0
		private void SleepStart()
		{
			if (this.Entries[0].State == EQuestState.Active)
			{
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
				int num = 4 - (int)value;
				if (num == -1)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage("In the midst of night gunshots ring out nearby, but you are not the target. The rest of the night is quiet.", 5f);
					return;
				}
				if (num == 0)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage("The loan sharks are arriving tonight.", 4f);
					return;
				}
				if (num == 1)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage(num.ToString() + " day until the loan sharks arrive", 3f);
					return;
				}
				Singleton<SleepCanvas>.Instance.QueueSleepMessage(num.ToString() + " days until the loan sharks arrive", 3f);
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0004B19F File Offset: 0x0004939F
		private void SpawnLoanSharkVehicle()
		{
			NetworkSingleton<VehicleManager>.Instance.SpawnLoanSharkVehicle(this.LoanSharkVehiclePosition.position, this.LoanSharkVehiclePosition.rotation);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0004B1C4 File Offset: 0x000493C4
		private void CheckArrival()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("Loan_Sharks_Arrived"))
			{
				return;
			}
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed") > 4f)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Loan_Sharks_Arrived", true.ToString(), true);
				this.SpawnLoanSharkVehicle();
				this.LoanSharkGraves.gameObject.SetActive(true);
				this.Entries[this.Entries.Count - 1].SetState(EQuestState.Completed, true);
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0004B250 File Offset: 0x00049450
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
			this.LoanSharkGraves.gameObject.SetActive(state == EQuestState.Completed);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0004B270 File Offset: 0x00049470
		private void UpdateName()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
			int num = 4 - (int)value;
			string str;
			if (num == -1)
			{
				str = string.Empty;
			}
			else if (num == 0)
			{
				str = "(arriving tonight)";
			}
			else if (num == 1)
			{
				str = "(" + num.ToString() + " day remaining)";
			}
			else
			{
				str = "(" + num.ToString() + " days remaining)";
			}
			this.Entries[0].SetEntryTitle(this.QuestName + " " + str);
		}

		// Token: 0x040010F2 RID: 4338
		public const int DAYS_TO_COMPLETE = 4;

		// Token: 0x040010F3 RID: 4339
		public string QuestName = "Make at least $1,000 to pay off the sharks";

		// Token: 0x040010F4 RID: 4340
		public int NelsonCallTime = 1215;

		// Token: 0x040010F5 RID: 4341
		public Transform LoanSharkVehiclePosition;

		// Token: 0x040010F6 RID: 4342
		public GameObject LoanSharkGraves;
	}
}
