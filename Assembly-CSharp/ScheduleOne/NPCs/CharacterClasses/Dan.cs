using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000495 RID: 1173
	public class Dan : NPC
	{
		// Token: 0x06001A21 RID: 6689 RVA: 0x00070543 File Offset: 0x0006E743
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00070568 File Offset: 0x0006E768
		private void OrderCompleted()
		{
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("Dan_Greeting_Done"))
			{
				base.PlayVO(EVOLineType.Thanks);
				this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
				return;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Dan_Greeting_Done", true.ToString(), true);
			base.PlayVO(EVOLineType.Question);
			if (this.onGreeting != null)
			{
				this.onGreeting.Invoke();
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x000705E6 File Offset: 0x0006E7E6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x000705FF File Offset: 0x0006E7FF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00070618 File Offset: 0x0006E818
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00070626 File Offset: 0x0006E826
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001660 RID: 5728
		public ShopInterface ShopInterface;

		// Token: 0x04001661 RID: 5729
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x04001662 RID: 5730
		public UnityEvent onGreeting;

		// Token: 0x04001663 RID: 5731
		private bool dll_Excuted;

		// Token: 0x04001664 RID: 5732
		private bool dll_Excuted;
	}
}
