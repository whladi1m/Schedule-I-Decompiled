using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F4 RID: 756
	public class Quest_TheDeepEnd : Quest
	{
		// Token: 0x060010CF RID: 4303 RVA: 0x0004B320 File Offset: 0x00049520
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.BeforeSleep));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.SleepFadeOut));
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0004B394 File Offset: 0x00049594
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			this.SetupFirstMeeting();
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0004B3A4 File Offset: 0x000495A4
		public void SetupFirstMeeting()
		{
			this.meetingSetup = true;
			this.Gate.ActivateIntercom();
			this.Switch.SwitchOn();
			this.Thomas.SetFirstMeetingEventActive(true);
			this.Thomas.dialogueHandler.onDialogueNodeDisplayed.AddListener(new UnityAction<string>(this.ThomasDialogueNodeDisplayed));
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0004B3FC File Offset: 0x000495FC
		private void ThomasDialogueNodeDisplayed(string nodeLabel)
		{
			if (nodeLabel == "THOMAS_INTRO_DONE")
			{
				Debug.Log("Intro meeting done!");
				this.Gate.SetEnterable(false);
				this.Thomas.InitialMeetingComplete();
				this.Entries[0].SetState(EQuestState.Completed, true);
				this.Entries[1].SetState(EQuestState.Active, true);
				this.PostMeetingTrigger.Trigger();
				base.StartCoroutine(this.<ThomasDialogueNodeDisplayed>g__Wait|13_0());
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0004B478 File Offset: 0x00049678
		private void HourPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Quest.GetQuest("Sink or Swim").QuestState != EQuestState.Completed)
			{
				return;
			}
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Since_LoanSharks_Arrived");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_LoanSharks_Arrived", (value + 1f).ToString(), true);
			if (this.Entries[0].State != EQuestState.Completed && value >= 36f && !this.Thomas.MeetingReminderSent)
			{
				this.Thomas.SendMeetingReminder();
				if (base.QuestState == EQuestState.Inactive)
				{
					this.Begin(true);
				}
			}
			if (this.Entries[0].State == EQuestState.Active && value >= 82f && !this.kidnapQueued)
			{
				this.kidnapQueued = true;
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0004B53D File Offset: 0x0004973D
		private void BeforeSleep()
		{
			if (this.kidnapQueued)
			{
				Singleton<SleepCanvas>.Instance.QueueSleepMessage("In the middle of the night, the door is kicked in and you are dragged into a vehicle trunk...", 3f);
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0004B55B File Offset: 0x0004975B
		private void SleepFadeOut()
		{
			if (this.kidnapQueued)
			{
				this.kidnapQueued = false;
				PlayerSingleton<PlayerMovement>.Instance.Teleport(this.MeetingTeleportPoint.position);
				Player.Local.transform.forward = this.MeetingTeleportPoint.forward;
			}
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0004B59B File Offset: 0x0004979B
		public override void SetQuestEntryState(int entryIndex, EQuestState state, bool network = true)
		{
			base.SetQuestEntryState(entryIndex, state, network);
			if (this.Entries[0].State == EQuestState.Active && !this.meetingSetup)
			{
				this.SetupFirstMeeting();
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0004B5C8 File Offset: 0x000497C8
		[CompilerGenerated]
		private IEnumerator <ThomasDialogueNodeDisplayed>g__Wait|13_0()
		{
			yield return new WaitUntil(() => Player.Local.CurrentProperty == null);
			Singleton<CallInterface>.Instance.StartCall(this.PostMeetingCall, this.PostMeetingCall.CallerID, 0);
			yield break;
		}

		// Token: 0x040010F7 RID: 4343
		public const float MEETING_REMINDER_TIME = 36f;

		// Token: 0x040010F8 RID: 4344
		public const float KIDNAP_TIME = 82f;

		// Token: 0x040010F9 RID: 4345
		private bool kidnapQueued;

		// Token: 0x040010FA RID: 4346
		private bool meetingSetup;

		// Token: 0x040010FB RID: 4347
		public Thomas Thomas;

		// Token: 0x040010FC RID: 4348
		public ManorGate Gate;

		// Token: 0x040010FD RID: 4349
		public ModularSwitch Switch;

		// Token: 0x040010FE RID: 4350
		public Transform MeetingTeleportPoint;

		// Token: 0x040010FF RID: 4351
		public PhoneCallData PostMeetingCall;

		// Token: 0x04001100 RID: 4352
		public SystemTriggerObject PostMeetingTrigger;
	}
}
