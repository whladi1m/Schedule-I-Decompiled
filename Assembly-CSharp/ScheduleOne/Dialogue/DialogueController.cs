using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000682 RID: 1666
	public class DialogueController : MonoBehaviour
	{
		// Token: 0x06002E38 RID: 11832 RVA: 0x000C1AC4 File Offset: 0x000BFCC4
		protected virtual void Start()
		{
			this.handler = base.GetComponent<DialogueHandler>();
			this.npc = this.handler.NPC;
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000C1B26 File Offset: 0x000BFD26
		private void Update()
		{
			this.lastGreetingTime += Time.deltaTime;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000C1B3C File Offset: 0x000BFD3C
		private void Hovered()
		{
			if (this.CanStartDialogue() && (((this.GetActiveChoices().Count > 0 || this.lastGreetingTime > DialogueController.GREETING_COOLDOWN) && this.DialogueEnabled) || this.OverrideContainer != null))
			{
				this.IntObj.SetMessage("Talk to " + this.npc.GetNameAddress());
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000C1BBB File Offset: 0x000BFDBB
		public void StartGenericDialogue(bool allowExit = true)
		{
			this.Interacted();
			this.GenericDialogue.SetAllowExit(allowExit);
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000C1BD0 File Offset: 0x000BFDD0
		private void Interacted()
		{
			this.GenericDialogue.SetAllowExit(true);
			this.dialogueQueued = true;
			base.Invoke("Unqueue", 1f);
			if (this.OverrideContainer != null)
			{
				this.handler.InitializeDialogue(this.OverrideContainer, this.UseDialogueBehaviour, "ENTRY");
				return;
			}
			if (this.GetActiveChoices().Count > 0)
			{
				this.shownChoices = this.GetActiveChoices();
				bool flag;
				EVOLineType evolineType;
				this.cachedGreeting = this.GetActiveGreeting(out flag, out evolineType);
				this.handler.InitializeDialogue(this.GenericDialogue, this.UseDialogueBehaviour, "ENTRY");
				if (flag && evolineType != EVOLineType.None)
				{
					this.npc.PlayVO(evolineType);
					return;
				}
			}
			else
			{
				bool flag2;
				EVOLineType lineType;
				this.handler.ShowWorldspaceDialogue(this.GetActiveGreeting(out flag2, out lineType), 5f);
				this.lastGreetingTime = 0f;
				if (flag2)
				{
					this.npc.PlayVO(lineType);
				}
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000C1CB8 File Offset: 0x000BFEB8
		private void Unqueue()
		{
			this.dialogueQueued = false;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000C1CC4 File Offset: 0x000BFEC4
		private string GetActiveGreeting(out bool playVO, out EVOLineType voLineType)
		{
			playVO = false;
			string result;
			if (this.GetCustomGreeting(out result, out playVO, out voLineType))
			{
				return result;
			}
			playVO = true;
			voLineType = EVOLineType.Greeting;
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(400, 1200))
			{
				return this.handler.Database.GetLine(EDialogueModule.Greetings, "morning_greeting");
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(1200, 1800))
			{
				return this.handler.Database.GetLine(EDialogueModule.Greetings, "afternoon_greeting");
			}
			return this.handler.Database.GetLine(EDialogueModule.Greetings, "night_greeting");
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000C1D58 File Offset: 0x000BFF58
		private List<DialogueController.DialogueChoice> GetActiveChoices()
		{
			List<DialogueController.DialogueChoice> list = new List<DialogueController.DialogueChoice>();
			foreach (DialogueController.DialogueChoice dialogueChoice in this.Choices)
			{
				if (dialogueChoice.ShouldShow())
				{
					list.Add(dialogueChoice);
				}
			}
			list.Sort((DialogueController.DialogueChoice a, DialogueController.DialogueChoice b) => b.Priority.CompareTo(a.Priority));
			return list;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000C1DE0 File Offset: 0x000BFFE0
		protected virtual bool GetCustomGreeting(out string greeting, out bool playVO, out EVOLineType voLineType)
		{
			greeting = string.Empty;
			playVO = false;
			voLineType = EVOLineType.Greeting;
			for (int i = 0; i < this.GreetingOverrides.Count; i++)
			{
				if (this.GreetingOverrides[i].ShouldShow)
				{
					greeting = this.GreetingOverrides[i].Greeting;
					playVO = this.GreetingOverrides[i].PlayVO;
					voLineType = this.GreetingOverrides[i].VOType;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000C1E5F File Offset: 0x000C005F
		public virtual int AddDialogueChoice(DialogueController.DialogueChoice data, int priority = 0)
		{
			data.Priority = priority;
			this.Choices.Add(data);
			return this.Choices.Count - 1;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000C1E81 File Offset: 0x000C0081
		public virtual int AddGreetingOverride(DialogueController.GreetingOverride data)
		{
			this.GreetingOverrides.Add(data);
			return this.Choices.Count - 1;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000C1E9C File Offset: 0x000C009C
		public virtual bool CanStartDialogue()
		{
			return (Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None || Player.Local.CrimeData.TimeSinceSighted >= 5f) && !Singleton<ManagementClipboard>.Instance.IsEquipped && !this.npc.behaviour.CallPoliceBehaviour.Active && !this.npc.behaviour.CombatBehaviour.Active && !this.npc.behaviour.CoweringBehaviour.Active && !this.npc.behaviour.RagdollBehaviour.Active && !this.npc.behaviour.HeavyFlinchBehaviour.Active && !this.npc.behaviour.ConsumeProductBehaviour.Active && !this.npc.behaviour.FleeBehaviour.Active && !this.npc.behaviour.GenericDialogueBehaviour.Active && this.npc.IsConscious && !this.dialogueQueued;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000C1FC2 File Offset: 0x000C01C2
		public virtual string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && dialogueLabel == "ENTRY")
			{
				return this.cachedGreeting;
			}
			return dialogueText;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000C1FEB File Offset: 0x000C01EB
		public virtual string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			return choiceText;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000C1FF0 File Offset: 0x000C01F0
		public virtual void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && dialogueLabel == "ENTRY")
			{
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				for (int i = 0; i < list.Count; i++)
				{
					DialogueChoiceData dialogueChoiceData = new DialogueChoiceData();
					dialogueChoiceData.ChoiceText = list[i].ChoiceText;
					dialogueChoiceData.ChoiceLabel = "GENERIC_CHOICE_" + i.ToString();
					existingChoices.Add(dialogueChoiceData);
				}
			}
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000C206C File Offset: 0x000C026C
		public virtual void ChoiceCallback(string choiceLabel)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && choiceLabel.Contains("GENERIC_CHOICE_"))
			{
				int num = int.Parse(choiceLabel.Substring("GENERIC_CHOICE_".Length));
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				if (num >= 0 && num < list.Count)
				{
					DialogueController.DialogueChoice dialogueChoice = list[num];
					if (dialogueChoice.onChoosen != null)
					{
						dialogueChoice.onChoosen.Invoke();
					}
					if (dialogueChoice.Conversation != null)
					{
						this.handler.InitializeDialogue(dialogueChoice.Conversation);
					}
				}
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000C20FC File Offset: 0x000C02FC
		public virtual bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && choiceLabel.Contains("GENERIC_CHOICE_"))
			{
				int num = int.Parse(choiceLabel.Substring("GENERIC_CHOICE_".Length));
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				if (num >= 0 && num < list.Count)
				{
					return list[num].IsValid(out invalidReason);
				}
			}
			invalidReason = string.Empty;
			return true;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000C2168 File Offset: 0x000C0368
		public void SetOverrideContainer(DialogueContainer container)
		{
			this.OverrideContainer = container;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000C2171 File Offset: 0x000C0371
		public void ClearOverrideContainer()
		{
			this.OverrideContainer = null;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000C217A File Offset: 0x000C037A
		public virtual bool DecideBranch(string branchLabel, out int index)
		{
			index = 0;
			return false;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000C2180 File Offset: 0x000C0380
		public void SetDialogueEnabled(bool enabled)
		{
			this.DialogueEnabled = enabled;
		}

		// Token: 0x040020F1 RID: 8433
		public static float GREETING_COOLDOWN = 5f;

		// Token: 0x040020F2 RID: 8434
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x040020F3 RID: 8435
		public DialogueContainer GenericDialogue;

		// Token: 0x040020F4 RID: 8436
		[Header("Settings")]
		public bool DialogueEnabled = true;

		// Token: 0x040020F5 RID: 8437
		public bool UseDialogueBehaviour = true;

		// Token: 0x040020F6 RID: 8438
		public List<DialogueController.DialogueChoice> Choices = new List<DialogueController.DialogueChoice>();

		// Token: 0x040020F7 RID: 8439
		public List<DialogueController.GreetingOverride> GreetingOverrides = new List<DialogueController.GreetingOverride>();

		// Token: 0x040020F8 RID: 8440
		public DialogueContainer OverrideContainer;

		// Token: 0x040020F9 RID: 8441
		protected NPC npc;

		// Token: 0x040020FA RID: 8442
		protected DialogueHandler handler;

		// Token: 0x040020FB RID: 8443
		private float lastGreetingTime = 20f;

		// Token: 0x040020FC RID: 8444
		private List<DialogueController.DialogueChoice> shownChoices = new List<DialogueController.DialogueChoice>();

		// Token: 0x040020FD RID: 8445
		private bool dialogueQueued;

		// Token: 0x040020FE RID: 8446
		private string cachedGreeting = string.Empty;

		// Token: 0x02000683 RID: 1667
		[Serializable]
		public class DialogueChoice
		{
			// Token: 0x06002E4F RID: 11855 RVA: 0x000C21F0 File Offset: 0x000C03F0
			public bool ShouldShow()
			{
				if (this.shouldShowCheck != null)
				{
					return this.shouldShowCheck(this.Enabled);
				}
				return this.Enabled;
			}

			// Token: 0x06002E50 RID: 11856 RVA: 0x000C2212 File Offset: 0x000C0412
			public bool IsValid(out string invalidReason)
			{
				if (this.isValidCheck != null)
				{
					return this.isValidCheck(out invalidReason);
				}
				invalidReason = string.Empty;
				return true;
			}

			// Token: 0x040020FF RID: 8447
			public bool Enabled = true;

			// Token: 0x04002100 RID: 8448
			public string ChoiceText;

			// Token: 0x04002101 RID: 8449
			public DialogueContainer Conversation;

			// Token: 0x04002102 RID: 8450
			public UnityEvent onChoosen = new UnityEvent();

			// Token: 0x04002103 RID: 8451
			public DialogueController.DialogueChoice.ShouldShowCheck shouldShowCheck;

			// Token: 0x04002104 RID: 8452
			public DialogueController.DialogueChoice.IsChoiceValid isValidCheck;

			// Token: 0x04002105 RID: 8453
			public int Priority;

			// Token: 0x02000684 RID: 1668
			// (Invoke) Token: 0x06002E53 RID: 11859
			public delegate bool ShouldShowCheck(bool enabled);

			// Token: 0x02000685 RID: 1669
			// (Invoke) Token: 0x06002E57 RID: 11863
			public delegate bool IsChoiceValid(out string invalidReason);
		}

		// Token: 0x02000686 RID: 1670
		[Serializable]
		public class GreetingOverride
		{
			// Token: 0x04002106 RID: 8454
			public string Greeting;

			// Token: 0x04002107 RID: 8455
			public bool ShouldShow;

			// Token: 0x04002108 RID: 8456
			public bool PlayVO;

			// Token: 0x04002109 RID: 8457
			public EVOLineType VOType;
		}
	}
}
