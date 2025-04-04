using System;
using ScheduleOne.Dialogue;
using ScheduleOne.UI.Shop;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049A RID: 1178
	public class Fiona : NPC
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x000707BC File Offset: 0x0006E9BC
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.ShopChoiceValid);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00070812 File Offset: 0x0006EA12
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000702CA File Offset: 0x0006E4CA
		public bool ShopChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00070840 File Offset: 0x0006EA40
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00070859 File Offset: 0x0006EA59
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00070872 File Offset: 0x0006EA72
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00070880 File Offset: 0x0006EA80
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400166D RID: 5741
		public ShopInterface ShopInterface;

		// Token: 0x0400166E RID: 5742
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x0400166F RID: 5743
		private bool dll_Excuted;

		// Token: 0x04001670 RID: 5744
		private bool dll_Excuted;
	}
}
