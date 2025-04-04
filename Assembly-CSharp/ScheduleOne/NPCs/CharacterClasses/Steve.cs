using System;
using ScheduleOne.UI.Shop;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004CD RID: 1229
	public class Steve : NPC
	{
		// Token: 0x06001B65 RID: 7013 RVA: 0x00072255 File Offset: 0x00070455
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00072279 File Offset: 0x00070479
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000722A7 File Offset: 0x000704A7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000722C0 File Offset: 0x000704C0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000722D9 File Offset: 0x000704D9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000722E7 File Offset: 0x000704E7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016F4 RID: 5876
		public ShopInterface ShopInterface;

		// Token: 0x040016F5 RID: 5877
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x040016F6 RID: 5878
		private bool dll_Excuted;

		// Token: 0x040016F7 RID: 5879
		private bool dll_Excuted;
	}
}
