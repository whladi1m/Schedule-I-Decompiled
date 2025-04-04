using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F7 RID: 759
	public class Quest_UnfavourableAgreements : Quest
	{
		// Token: 0x060010E2 RID: 4322 RVA: 0x0004B690 File Offset: 0x00049890
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			this.Thomas.onCartelContractReceived.AddListener(new UnityAction(this.HandoverCompleted));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.CheckHandoverExpiry));
			this.UpdateName();
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004B706 File Offset: 0x00049906
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			this.ResetTimer(false);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004B718 File Offset: 0x00049918
		private void HourPass()
		{
			float num = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Since_Cartel_Handover");
			float num2 = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due");
			if (this.Entries[0].State == EQuestState.Active)
			{
				num += 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_Cartel_Handover", num.ToString(), true);
				num2 -= 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Until_CartelContract_Due", num2.ToString(), true);
				this.UpdateName();
			}
			if (!this.handoverSetup && num >= 12f)
			{
				this.SetupHandover();
			}
			if (!this.Thomas.HandoverReminderSent && num2 <= 24f)
			{
				this.Thomas.SendHandoverReminder();
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0004B7D3 File Offset: 0x000499D3
		private void SetupHandover()
		{
			this.handoverSetup = true;
			Debug.Log("Setting up handover");
			this.Gate.ActivateIntercom();
			this.Switch.SwitchOn();
			this.Thomas.SetHandoverEventActive(true);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0004B808 File Offset: 0x00049A08
		private void CheckHandoverExpiry()
		{
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due") <= 0f)
			{
				Singleton<SleepCanvas>.Instance.QueueSleepMessage("You have failed to make the weekly delivery. Benzies family goons break in during the night, taking your stock and leaving you nearly dead.", 5f);
				this.RV.Ransack();
				this.ResetTimer(false);
				Player.Local.Health.SetHealth(65f);
			}
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0004B868 File Offset: 0x00049A68
		private void UpdateName()
		{
			int num = Mathf.FloorToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due") / 24f);
			string str;
			if (num == -1)
			{
				str = string.Empty;
			}
			else if (num == 0)
			{
				str = "(due today)";
			}
			else if (num == 1)
			{
				str = "(" + num.ToString() + " day)";
			}
			else
			{
				str = "(" + num.ToString() + " days)";
			}
			this.Entries[0].SetEntryTitle(this.QuestEntryTitle + " " + str);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0004B8FD File Offset: 0x00049AFD
		private void HandoverCompleted()
		{
			this.ResetTimer(true);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0004B908 File Offset: 0x00049B08
		public void ResetTimer(bool allowBuildup)
		{
			float num = Mathf.Floor((float)TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<TimeManager>.Instance.CurrentTime) / 60f);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_Cartel_Handover", num.ToString(), true);
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due");
			float num2 = 168f;
			if (allowBuildup)
			{
				num2 += value;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Until_CartelContract_Due", num2.ToString(), true);
			this.UpdateName();
		}

		// Token: 0x04001106 RID: 4358
		public const float WEEKLY_DELIVERY_HOURS = 168f;

		// Token: 0x04001107 RID: 4359
		public const float REMINDER_THRESHOLD = 144f;

		// Token: 0x04001108 RID: 4360
		public Thomas Thomas;

		// Token: 0x04001109 RID: 4361
		public ManorGate Gate;

		// Token: 0x0400110A RID: 4362
		public ModularSwitch Switch;

		// Token: 0x0400110B RID: 4363
		public RV RV;

		// Token: 0x0400110C RID: 4364
		public string QuestEntryTitle;

		// Token: 0x0400110D RID: 4365
		private bool handoverSetup;
	}
}
