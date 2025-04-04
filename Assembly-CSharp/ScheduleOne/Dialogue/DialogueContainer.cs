using System;
using System.Collections.Generic;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006B4 RID: 1716
	[Serializable]
	public class DialogueContainer : ScriptableObject
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002F0C RID: 12044 RVA: 0x000C4991 File Offset: 0x000C2B91
		// (set) Token: 0x06002F0D RID: 12045 RVA: 0x000C4999 File Offset: 0x000C2B99
		public bool allowExit { get; private set; } = true;

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002F0E RID: 12046 RVA: 0x000C49A2 File Offset: 0x000C2BA2
		public bool AllowExit
		{
			get
			{
				return this.allowExit || Player.Local.IsArrested || !Player.Local.Health.IsAlive;
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000C49CC File Offset: 0x000C2BCC
		public DialogueNodeData GetDialogueNodeByLabel(string dialogueNodeLabel)
		{
			return this.DialogueNodeData.Find((DialogueNodeData x) => x.DialogueNodeLabel == dialogueNodeLabel);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000C4A00 File Offset: 0x000C2C00
		public BranchNodeData GetBranchNodeByLabel(string branchLabel)
		{
			return this.BranchNodeData.Find((BranchNodeData x) => x.BranchLabel == branchLabel);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000C4A34 File Offset: 0x000C2C34
		public DialogueNodeData GetDialogueNodeByGUID(string dialogueNodeGUID)
		{
			return this.DialogueNodeData.Find((DialogueNodeData x) => x.Guid == dialogueNodeGUID);
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000C4A68 File Offset: 0x000C2C68
		public BranchNodeData GetBranchNodeByGUID(string branchGUID)
		{
			return this.BranchNodeData.Find((BranchNodeData x) => x.Guid == branchGUID);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000C4A9C File Offset: 0x000C2C9C
		public NodeLinkData GetLink(string baseChoiceOrOptionGUID)
		{
			return this.NodeLinks.Find((NodeLinkData x) => x.BaseChoiceOrOptionGUID == baseChoiceOrOptionGUID);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000C4ACD File Offset: 0x000C2CCD
		public void SetAllowExit(bool allowed)
		{
			this.allowExit = allowed;
		}

		// Token: 0x04002181 RID: 8577
		public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();

		// Token: 0x04002182 RID: 8578
		public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();

		// Token: 0x04002183 RID: 8579
		public List<BranchNodeData> BranchNodeData = new List<BranchNodeData>();
	}
}
