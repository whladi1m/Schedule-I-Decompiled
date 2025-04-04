using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C2 RID: 1218
	public class Oscar : NPC
	{
		// Token: 0x06001B1F RID: 6943 RVA: 0x00071AD7 File Offset: 0x0006FCD7
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00071B16 File Offset: 0x0006FD16
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00071B44 File Offset: 0x0006FD44
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00071B79 File Offset: 0x0006FD79
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00071BB0 File Offset: 0x0006FDB0
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00071C04 File Offset: 0x0006FE04
		public void EnableDeliveries()
		{
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableDeliveries>g__Wait|9_0());
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00071C2A File Offset: 0x0006FE2A
		[CompilerGenerated]
		private IEnumerator <EnableDeliveries>g__Wait|9_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<DeliveryApp>.InstanceExists);
			PlayerSingleton<DeliveryApp>.Instance.GetShop(this.ShopInterface).SetIsAvailable();
			yield break;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00071C39 File Offset: 0x0006FE39
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00071C52 File Offset: 0x0006FE52
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00071C6B File Offset: 0x0006FE6B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00071C79 File Offset: 0x0006FE79
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016D1 RID: 5841
		public ShopInterface ShopInterface;

		// Token: 0x040016D2 RID: 5842
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x040016D3 RID: 5843
		public DialogueContainer GreetingDialogue;

		// Token: 0x040016D4 RID: 5844
		public string GreetedVariable = "OscarGreeted";

		// Token: 0x040016D5 RID: 5845
		private bool dll_Excuted;

		// Token: 0x040016D6 RID: 5846
		private bool dll_Excuted;
	}
}
