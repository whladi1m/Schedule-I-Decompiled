using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000445 RID: 1093
	public class Stan : NPC
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x000606D3 File Offset: 0x0005E8D3
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x000606F6 File Offset: 0x0005E8F6
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0006072B File Offset: 0x0005E92B
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00060760 File Offset: 0x0005E960
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000607C7 File Offset: 0x0005E9C7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000607E0 File Offset: 0x0005E9E0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x000607F9 File Offset: 0x0005E9F9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00060807 File Offset: 0x0005EA07
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400147E RID: 5246
		public DialogueContainer GreetingDialogue;

		// Token: 0x0400147F RID: 5247
		public string GreetedVariable = "StanGreeted";

		// Token: 0x04001480 RID: 5248
		private bool dll_Excuted;

		// Token: 0x04001481 RID: 5249
		private bool dll_Excuted;
	}
}
