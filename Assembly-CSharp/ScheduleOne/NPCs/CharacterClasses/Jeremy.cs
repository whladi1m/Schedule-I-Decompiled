using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004AB RID: 1195
	public class Jeremy : NPC
	{
		// Token: 0x06001A9B RID: 6811 RVA: 0x00070F1C File Offset: 0x0006F11C
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00070F3F File Offset: 0x0006F13F
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00070F74 File Offset: 0x0006F174
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00070FA8 File Offset: 0x0006F1A8
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0007101A File Offset: 0x0006F21A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00071033 File Offset: 0x0006F233
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0007104C File Offset: 0x0006F24C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0007105A File Offset: 0x0006F25A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001697 RID: 5783
		public Dealership Dealership;

		// Token: 0x04001698 RID: 5784
		public List<Jeremy.DealershipListing> Listings = new List<Jeremy.DealershipListing>();

		// Token: 0x04001699 RID: 5785
		public DialogueContainer GreetingDialogue;

		// Token: 0x0400169A RID: 5786
		public string GreetedVariable = "JeremyGreeted";

		// Token: 0x0400169B RID: 5787
		private bool dll_Excuted;

		// Token: 0x0400169C RID: 5788
		private bool dll_Excuted;

		// Token: 0x020004AC RID: 1196
		[Serializable]
		public class DealershipListing
		{
			// Token: 0x17000463 RID: 1123
			// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x0007106E File Offset: 0x0006F26E
			public string vehicleName
			{
				get
				{
					return NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(this.vehicleCode).VehicleName;
				}
			}

			// Token: 0x17000464 RID: 1124
			// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x00071085 File Offset: 0x0006F285
			public float price
			{
				get
				{
					return NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(this.vehicleCode).VehiclePrice;
				}
			}

			// Token: 0x0400169D RID: 5789
			public string vehicleCode = string.Empty;
		}
	}
}
