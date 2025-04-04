using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004CA RID: 1226
	public class Ray : NPC
	{
		// Token: 0x06001B4D RID: 6989 RVA: 0x00071ED6 File Offset: 0x000700D6
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00071EF9 File Offset: 0x000700F9
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00071F2E File Offset: 0x0007012E
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00071F64 File Offset: 0x00070164
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00071FE9 File Offset: 0x000701E9
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00072002 File Offset: 0x00070202
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0007201B File Offset: 0x0007021B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00072029 File Offset: 0x00070229
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016E6 RID: 5862
		public DialogueContainer GreetingDialogue;

		// Token: 0x040016E7 RID: 5863
		public string GreetedVariable = "RayGreeted";

		// Token: 0x040016E8 RID: 5864
		public string IntroductionMessage;

		// Token: 0x040016E9 RID: 5865
		public string IntroSentVariable = "RayIntroSent";

		// Token: 0x040016EA RID: 5866
		[Header("Intro message conditions")]
		public FullRank IntroRank;

		// Token: 0x040016EB RID: 5867
		public int IntroDaysPlayed = 21;

		// Token: 0x040016EC RID: 5868
		public float IntroNetworth = 15000f;

		// Token: 0x040016ED RID: 5869
		private bool dll_Excuted;

		// Token: 0x040016EE RID: 5870
		private bool dll_Excuted;
	}
}
