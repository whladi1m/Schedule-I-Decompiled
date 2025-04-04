using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Product;
using ScheduleOne.Variables;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D4 RID: 1236
	public class Benji : Dealer
	{
		// Token: 0x06001B99 RID: 7065 RVA: 0x00072890 File Offset: 0x00070A90
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_Recommended", base.HasBeenRecommended.ToString(), true);
			int num = 0;
			for (int i = 0; i < base.Inventory.ItemSlots.Count; i++)
			{
				if (base.Inventory.ItemSlots[i].Quantity != 0 && base.Inventory.ItemSlots[i].ItemInstance is WeedInstance)
				{
					num += (base.Inventory.ItemSlots[i].ItemInstance as WeedInstance).Amount * base.Inventory.ItemSlots[i].Quantity;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_WeedCount", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CashAmount", base.Cash.ToString(), true);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00072990 File Offset: 0x00070B90
		protected override void AddCustomer(Customer customer)
		{
			base.AddCustomer(customer);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CustomerCount", this.AssignedCustomers.Count.ToString(), true);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000729C8 File Offset: 0x00070BC8
		public override void RemoveCustomer(Customer customer)
		{
			base.RemoveCustomer(customer);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CustomerCount", this.AssignedCustomers.Count.ToString(), true);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000729FF File Offset: 0x00070BFF
		protected override void RecruitmentRequested()
		{
			base.RecruitmentRequested();
			if (this.onRecruitmentRequested != null)
			{
				this.onRecruitmentRequested.Invoke();
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00072A1A File Offset: 0x00070C1A
		protected override void UpdatePotentialDealerPoI()
		{
			base.UpdatePotentialDealerPoI();
			base.potentialDealerPoI.enabled = false;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00072A2E File Offset: 0x00070C2E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00072A47 File Offset: 0x00070C47
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00072A60 File Offset: 0x00070C60
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00072A6E File Offset: 0x00070C6E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400170D RID: 5901
		public UnityEvent onRecruitmentRequested;

		// Token: 0x0400170E RID: 5902
		private bool dll_Excuted;

		// Token: 0x0400170F RID: 5903
		private bool dll_Excuted;
	}
}
