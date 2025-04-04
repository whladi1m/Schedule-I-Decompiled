using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049B RID: 1179
	public class Fixer : NPC
	{
		// Token: 0x06001A46 RID: 6726 RVA: 0x00070894 File Offset: 0x0006EA94
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x000708B7 File Offset: 0x0006EAB7
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000708EC File Offset: 0x0006EAEC
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x00070920 File Offset: 0x0006EB20
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00070974 File Offset: 0x0006EB74
		public static float GetAdditionalSigningFee()
		{
			int num = Mathf.RoundToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LifetimeEmployeesRecruited"));
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				if (i <= 5)
				{
					num2 += 100f;
				}
				else
				{
					num2 += 250f;
				}
			}
			return Mathf.Min(num2, 500f);
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000709DC File Offset: 0x0006EBDC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000709F5 File Offset: 0x0006EBF5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00070A0E File Offset: 0x0006EC0E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00070A1C File Offset: 0x0006EC1C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001671 RID: 5745
		public const int ADDITIONAL_SIGNING_FEE_1 = 100;

		// Token: 0x04001672 RID: 5746
		public const int ADDITIONAL_SIGNING_FEE_2 = 250;

		// Token: 0x04001673 RID: 5747
		public const int MAX_SIGNING_FEE = 500;

		// Token: 0x04001674 RID: 5748
		public const int ADDITIONAL_FEE_THRESHOLD = 5;

		// Token: 0x04001675 RID: 5749
		public DialogueContainer GreetingDialogue;

		// Token: 0x04001676 RID: 5750
		public string GreetedVariable = "FixerGreeted";

		// Token: 0x04001677 RID: 5751
		private bool dll_Excuted;

		// Token: 0x04001678 RID: 5752
		private bool dll_Excuted;
	}
}
