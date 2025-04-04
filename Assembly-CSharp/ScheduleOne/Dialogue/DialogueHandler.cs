using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200069D RID: 1693
	public class DialogueHandler : MonoBehaviour
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x000C3412 File Offset: 0x000C1612
		// (set) Token: 0x06002EA5 RID: 11941 RVA: 0x000C341A File Offset: 0x000C161A
		public bool IsPlaying { get; private set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x000C3423 File Offset: 0x000C1623
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x000C342B File Offset: 0x000C162B
		public NPC NPC { get; protected set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x000C3434 File Offset: 0x000C1634
		private DialogueCanvas canvas
		{
			get
			{
				return Singleton<DialogueCanvas>.Instance;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x000C343B File Offset: 0x000C163B
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000C3443 File Offset: 0x000C1643
		public List<DialogueModule> runtimeModules { get; private set; } = new List<DialogueModule>();

		// Token: 0x06002EAB RID: 11947 RVA: 0x000C344C File Offset: 0x000C164C
		protected virtual void Awake()
		{
			if (this.NPC == null)
			{
				this.NPC = base.GetComponentInParent<NPC>();
			}
			if (this.Database == null)
			{
				Console.LogWarning(this.NPC.fullName + " dialogue database isn't assigned! Using default database.", null);
				this.Database = Singleton<DialogueManager>.Instance.DefaultDatabase;
			}
			if (this.VOEmitter == null && this.NPC != null)
			{
				this.VOEmitter = this.NPC.VoiceOverEmitter;
			}
			DialogueModule dialogueModule = base.gameObject.AddComponent<DialogueModule>();
			dialogueModule.ModuleType = EDialogueModule.Generic;
			dialogueModule.Entries = this.Database.GenericEntries;
			this.runtimeModules.Add(dialogueModule);
			this.runtimeModules.AddRange(this.Database.Modules);
			this.Database.Initialize(this);
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000C352B File Offset: 0x000C172B
		public void InitializeDialogue(DialogueContainer container)
		{
			this.InitializeDialogue(container, true, "ENTRY");
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000C353C File Offset: 0x000C173C
		public void InitializeDialogue(DialogueContainer dialogueContainer, bool enableDialogueBehaviour = true, string entryNodeLabel = "ENTRY")
		{
			DialogueHandler.<>c__DisplayClass34_0 CS$<>8__locals1 = new DialogueHandler.<>c__DisplayClass34_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.dialogueContainer = dialogueContainer;
			CS$<>8__locals1.entryNodeLabel = entryNodeLabel;
			if (CS$<>8__locals1.dialogueContainer == null)
			{
				Console.LogWarning("InitializeDialogue: provided dialogueContainer is null", null);
				return;
			}
			if (enableDialogueBehaviour)
			{
				this.NPC.behaviour.GenericDialogueBehaviour.SendTargetPlayer(Player.Local.NetworkObject);
				this.NPC.behaviour.GenericDialogueBehaviour.SendEnable();
				this.NPC.behaviour.Update();
			}
			if (this.WorldspaceRend.ShownText != null)
			{
				this.WorldspaceRend.HideText();
			}
			if (this.onConversationStart != null)
			{
				this.onConversationStart.Invoke();
			}
			CS$<>8__locals1.npc = base.GetComponentInParent<NPC>();
			if (CS$<>8__locals1.npc != null && CS$<>8__locals1.npc.Avatar.Anim.TimeSinceSitEnd < 0.5f && enableDialogueBehaviour)
			{
				base.StartCoroutine(CS$<>8__locals1.<InitializeDialogue>g__Wait|0());
				return;
			}
			CS$<>8__locals1.<InitializeDialogue>g__Open|1();
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000C3644 File Offset: 0x000C1844
		public void InitializeDialogue(string dialogueContainerName, bool enableDialogueBehaviour = true, string entryNodeLabel = "ENTRY")
		{
			DialogueContainer dialogueContainer = this.dialogueContainers.Find((DialogueContainer x) => x.name.ToLower() == dialogueContainerName.ToLower());
			if (dialogueContainer == null)
			{
				Console.LogWarning("InitializeDialogue: Could not find DialogueContainer with name '" + dialogueContainerName + "'", null);
				return;
			}
			this.InitializeDialogue(dialogueContainer, enableDialogueBehaviour, entryNodeLabel);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000C36A4 File Offset: 0x000C18A4
		public virtual bool CanBeginConversation()
		{
			return this.NPC.SyncAccessor_PlayerConversant == null;
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000C36B7 File Offset: 0x000C18B7
		public void OverrideShownDialogue(string _overrideText)
		{
			this.overrideText = _overrideText;
			this.canvas.OverrideText(this.overrideText);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000C36D1 File Offset: 0x000C18D1
		public void StopOverride()
		{
			this.overrideText = string.Empty;
			this.canvas.StopTextOverride();
			if (DialogueHandler.activeDialogueNode != null)
			{
				this.ShowNode(DialogueHandler.activeDialogueNode);
			}
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000C36FC File Offset: 0x000C18FC
		public virtual void EndDialogue()
		{
			if (this.skipNextDialogueBehaviourEnd)
			{
				this.skipNextDialogueBehaviourEnd = false;
			}
			else
			{
				this.NPC.behaviour.GenericDialogueBehaviour.SendDisable();
			}
			foreach (DialogueEvent dialogueEvent in this.DialogueEvents)
			{
				if (!(dialogueEvent.Dialogue != DialogueHandler.activeDialogue) && dialogueEvent.onDialogueEnded != null)
				{
					dialogueEvent.onDialogueEnded.Invoke();
				}
			}
			this.canvas.EndDialogue();
			this.IsPlaying = false;
			DialogueHandler.activeDialogue = null;
			DialogueHandler.activeDialogueNode = null;
			this.NPC.SetConversant(null);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000C3797 File Offset: 0x000C1997
		public void SkipNextDialogueBehaviourEnd()
		{
			this.skipNextDialogueBehaviourEnd = true;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000C37A0 File Offset: 0x000C19A0
		protected virtual DialogueNodeData FinalizeDialogueNode(DialogueNodeData data)
		{
			return data;
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000C37A4 File Offset: 0x000C19A4
		public void ShowNode(DialogueNodeData node)
		{
			node = this.FinalizeDialogueNode(node);
			DialogueHandler.activeDialogueNode = node;
			if (this.overrideText != string.Empty)
			{
				return;
			}
			string dialogueText = this.ModifyDialogueText(node.DialogueNodeLabel, node.DialogueText);
			this.CurrentChoices = new List<DialogueChoiceData>();
			foreach (DialogueChoiceData dialogueChoiceData in node.choices)
			{
				if (this.ShouldChoiceBeShown(dialogueChoiceData.ChoiceLabel))
				{
					this.CurrentChoices.Add(dialogueChoiceData);
				}
			}
			this.TempLinks.Clear();
			this.ModifyChoiceList(node.DialogueNodeLabel, ref this.CurrentChoices);
			List<string> list = new List<string>();
			foreach (DialogueChoiceData dialogueChoiceData2 in this.CurrentChoices)
			{
				list.Add(this.ModifyChoiceText(dialogueChoiceData2.ChoiceLabel, dialogueChoiceData2.ChoiceText));
			}
			this.DialogueCallback(node.DialogueNodeLabel);
			if (this.VOEmitter != null && node.VoiceLine != EVOLineType.None)
			{
				this.VOEmitter.Play(node.VoiceLine);
			}
			this.canvas.DisplayDialogueNode(this, DialogueHandler.activeDialogueNode, dialogueText, list);
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000C38EC File Offset: 0x000C1AEC
		private void EvaluateBranch(BranchNodeData node)
		{
			int num = this.CheckBranch(node.BranchLabel);
			if (node.options.Length > num)
			{
				NodeLinkData link = this.GetLink(node.options[num].Guid);
				if (link != null)
				{
					if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
					}
					else if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
					}
				}
				else
				{
					this.EndDialogue();
				}
			}
			else
			{
				Console.LogWarning("EvaluateBranch: optionIndex is out of range", null);
				this.EndDialogue();
			}
			this.TempLinks.Clear();
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000C39A0 File Offset: 0x000C1BA0
		public void ChoiceSelected(int choiceIndex)
		{
			DialogueNodeData dialogueNodeData = DialogueHandler.activeDialogueNode;
			this.ChoiceCallback(this.CurrentChoices[choiceIndex].ChoiceLabel);
			if (DialogueHandler.activeDialogueNode == dialogueNodeData && DialogueHandler.activeDialogueNode != null)
			{
				NodeLinkData link = this.GetLink(this.CurrentChoices[choiceIndex].Guid);
				if (link != null)
				{
					if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
						return;
					}
					if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
						return;
					}
				}
				else
				{
					this.EndDialogue();
				}
			}
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000C3A4C File Offset: 0x000C1C4C
		public void ContinueSubmitted()
		{
			if (DialogueHandler.activeDialogueNode.choices.Length == 0)
			{
				this.EndDialogue();
				return;
			}
			NodeLinkData link = this.GetLink(DialogueHandler.activeDialogueNode.choices[0].Guid);
			if (link != null)
			{
				if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
				{
					this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
					return;
				}
				if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
				{
					this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
					return;
				}
			}
			else
			{
				this.EndDialogue();
			}
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000C3AE0 File Offset: 0x000C1CE0
		public virtual bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CHOICE_TEST")
			{
				invalidReason = "IT JUST CAN'T BE DONE";
				return false;
			}
			invalidReason = string.Empty;
			return true;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool ShouldChoiceBeShown(string choiceLabel)
		{
			return true;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000C3B00 File Offset: 0x000C1D00
		protected virtual int CheckBranch(string branchLabel)
		{
			if (branchLabel == "BRANCH_REJECTION")
			{
				return UnityEngine.Random.Range(0, 2);
			}
			if (!(branchLabel == "BRANCH_CHECKPASS"))
			{
				if (branchLabel != string.Empty)
				{
					Console.LogWarning("CheckBranch: branch label '" + branchLabel + "' not accounted for!", null);
				}
				return 0;
			}
			if (this.passChecked)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000C1FEB File Offset: 0x000C01EB
		protected virtual string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			return dialogueText;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000C1FEB File Offset: 0x000C01EB
		protected virtual string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			return choiceText;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000C3B5F File Offset: 0x000C1D5F
		protected virtual void ChoiceCallback(string choiceLabel)
		{
			if (this.onDialogueChoiceChosen != null)
			{
				this.onDialogueChoiceChosen.Invoke(choiceLabel);
			}
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000C3B78 File Offset: 0x000C1D78
		protected virtual void DialogueCallback(string dialogueLabel)
		{
			if (this.onDialogueNodeDisplayed != null)
			{
				this.onDialogueNodeDisplayed.Invoke(dialogueLabel);
			}
			foreach (DialogueEvent dialogueEvent in this.DialogueEvents)
			{
				if (!(dialogueEvent.Dialogue != DialogueHandler.activeDialogue))
				{
					foreach (DialogueNodeEvent dialogueNodeEvent in dialogueEvent.NodeEvents)
					{
						if (dialogueNodeEvent.NodeLabel == dialogueLabel)
						{
							dialogueNodeEvent.onNodeDisplayed.Invoke();
						}
					}
				}
			}
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000C3C00 File Offset: 0x000C1E00
		protected void CreateTempLink(string baseNodeGUID, string baseOptionGUID, string targetNodeGUID)
		{
			NodeLinkData nodeLinkData = new NodeLinkData();
			nodeLinkData.BaseDialogueOrBranchNodeGuid = baseNodeGUID;
			nodeLinkData.BaseChoiceOrOptionGUID = baseOptionGUID;
			nodeLinkData.TargetNodeGuid = targetNodeGUID;
			this.TempLinks.Add(nodeLinkData);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000C3C34 File Offset: 0x000C1E34
		private NodeLinkData GetLink(string baseChoiceOrOptionGUID)
		{
			NodeLinkData nodeLinkData = DialogueHandler.activeDialogue.GetLink(baseChoiceOrOptionGUID);
			if (nodeLinkData == null)
			{
				nodeLinkData = this.TempLinks.Find((NodeLinkData x) => x.BaseChoiceOrOptionGUID == baseChoiceOrOptionGUID);
			}
			return nodeLinkData;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Hovered()
		{
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Interacted()
		{
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000C3C7B File Offset: 0x000C1E7B
		public virtual void PlayReaction_Local(string key)
		{
			this.PlayReaction(key, -1f, false);
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000C3C8A File Offset: 0x000C1E8A
		public virtual void PlayReaction_Networked(string key)
		{
			this.PlayReaction(key, -1f, true);
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000C3C9C File Offset: 0x000C1E9C
		public virtual void PlayReaction(string key, float duration, bool network)
		{
			if (!this.NPC.IsConscious)
			{
				return;
			}
			if (network)
			{
				this.NPC.SendWorldspaceDialogueKey(key, duration);
				return;
			}
			if (key == string.Empty)
			{
				this.HideWorldspaceDialogue();
				return;
			}
			string line = this.Database.GetLine(EDialogueModule.Reactions, key);
			if (duration == -1f)
			{
				duration = Mathf.Clamp((float)line.Length * 0.2f, 1.5f, 5f);
			}
			this.WorldspaceRend.ShowText(line, duration);
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000C3D1D File Offset: 0x000C1F1D
		public virtual void HideWorldspaceDialogue()
		{
			this.WorldspaceRend.HideText();
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000C3D2A File Offset: 0x000C1F2A
		public virtual void ShowWorldspaceDialogue(string text, float duration)
		{
			if (!this.NPC.IsConscious)
			{
				return;
			}
			this.WorldspaceRend.ShowText(text, duration);
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000C3D47 File Offset: 0x000C1F47
		public virtual void ShowWorldspaceDialogue_5s(string text)
		{
			this.ShowWorldspaceDialogue(text, 5f);
		}

		// Token: 0x0400213F RID: 8511
		public const float TimePerChar = 0.2f;

		// Token: 0x04002140 RID: 8512
		public const float WorldspaceDialogueMinDuration = 1.5f;

		// Token: 0x04002141 RID: 8513
		public const float WorldspaceDialogueMaxDuration = 5f;

		// Token: 0x04002142 RID: 8514
		public static DialogueContainer activeDialogue;

		// Token: 0x04002143 RID: 8515
		public static DialogueNodeData activeDialogueNode;

		// Token: 0x04002145 RID: 8517
		public DialogueDatabase Database;

		// Token: 0x04002146 RID: 8518
		[Header("References")]
		public Transform LookPosition;

		// Token: 0x04002147 RID: 8519
		public WorldspaceDialogueRenderer WorldspaceRend;

		// Token: 0x04002149 RID: 8521
		public VOEmitter VOEmitter;

		// Token: 0x0400214A RID: 8522
		[HideInInspector]
		public List<DialogueChoiceData> CurrentChoices = new List<DialogueChoiceData>();

		// Token: 0x0400214B RID: 8523
		[Header("Events")]
		public DialogueEvent[] DialogueEvents;

		// Token: 0x0400214C RID: 8524
		public UnityEvent onConversationStart;

		// Token: 0x0400214D RID: 8525
		public UnityEvent<string> onDialogueNodeDisplayed;

		// Token: 0x0400214E RID: 8526
		public UnityEvent<string> onDialogueChoiceChosen;

		// Token: 0x0400214F RID: 8527
		protected string overrideText = string.Empty;

		// Token: 0x04002150 RID: 8528
		[SerializeField]
		private List<DialogueContainer> dialogueContainers = new List<DialogueContainer>();

		// Token: 0x04002151 RID: 8529
		private List<NodeLinkData> TempLinks = new List<NodeLinkData>();

		// Token: 0x04002152 RID: 8530
		private bool skipNextDialogueBehaviourEnd;

		// Token: 0x04002154 RID: 8532
		private bool passChecked;
	}
}
