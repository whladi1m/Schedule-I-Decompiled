using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000474 RID: 1140
	public class NPCSignal_HandleDeal : NPCSignal
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x0006D6F2 File Offset: 0x0006B8F2
		public new string ActionName
		{
			get
			{
				return "Handle deal";
			}
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0006D6F9 File Offset: 0x0006B8F9
		public void AssignContract(Contract c)
		{
			this.contract = c;
			if (this.contract != null)
			{
				this.customer = c.Customer.GetComponent<Customer>();
			}
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0006D721 File Offset: 0x0006B921
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCSignal_HandleDeal_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0006D735 File Offset: 0x0006B935
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 10;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0006D745 File Offset: 0x0006B945
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0006D74D File Offset: 0x0006B94D
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0006D764 File Offset: 0x0006B964
		public override void MinPassed()
		{
			base.MinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (this.contract == null || this.contract.QuestState != EQuestState.Active)
			{
				this.End();
				base.gameObject.SetActive(false);
				this.contract = null;
				base.StartedThisCycle = false;
				return;
			}
			if (this.handoverRoutine != null)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.IsAtDestination())
				{
					if (this.IsCustomerReady())
					{
						this.BeginHandover();
						return;
					}
				}
				else
				{
					base.SetDestination(this.GetStandPos(), true);
				}
			}
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0006D7FC File Offset: 0x0006B9FC
		public override void LateStarted()
		{
			base.LateStarted();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0006D811 File Offset: 0x0006BA11
		public override void JumpTo()
		{
			base.JumpTo();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0006D826 File Offset: 0x0006BA26
		public override void Interrupt()
		{
			base.Interrupt();
			this.npc.Movement.Stop();
			this.StopHandover();
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0006D844 File Offset: 0x0006BA44
		public override void End()
		{
			base.End();
			this.StopHandover();
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0006C448 File Offset: 0x0006A648
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0006D852 File Offset: 0x0006BA52
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.GetStandPos()) < 2f;
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0006D876 File Offset: 0x0006BA76
		private bool IsCustomerReady()
		{
			return this.customer.IsAtDealLocation();
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0006D883 File Offset: 0x0006BA83
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				Debug.LogWarning(this.npc.fullName + ": walk to location not successful");
				return;
			}
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0006D8B4 File Offset: 0x0006BAB4
		private void BeginHandover()
		{
			if (this.handoverRoutine != null)
			{
				return;
			}
			this.handoverRoutine = base.StartCoroutine(this.<BeginHandover>g__Routine|20_0());
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0006D8D1 File Offset: 0x0006BAD1
		private void StopHandover()
		{
			if (this.handoverRoutine != null)
			{
				base.StopCoroutine(this.handoverRoutine);
				this.handoverRoutine = null;
			}
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0006D8F0 File Offset: 0x0006BAF0
		private Vector3 GetStandPos()
		{
			if (this.contract == null)
			{
				return Vector3.zero;
			}
			return this.contract.DeliveryLocation.CustomerStandPoint.position + this.contract.DeliveryLocation.CustomerStandPoint.forward * 1.2f;
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0006D94A File Offset: 0x0006BB4A
		private Vector3 GetStandDir()
		{
			return -this.contract.DeliveryLocation.CustomerStandPoint.forward;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0006D966 File Offset: 0x0006BB66
		[CompilerGenerated]
		private IEnumerator <BeginHandover>g__Routine|20_0()
		{
			this.npc.Movement.FaceDirection(this.GetStandDir(), 0.5f);
			yield return new WaitForSeconds(2f);
			yield return new WaitUntil(() => this.customer.IsAtDealLocation());
			List<ItemInstance> items;
			if (!this.dealer.RemoveContractItems(this.contract, this.customer.CustomerData.Standards.GetCorrespondingQuality(), out items))
			{
				Console.LogWarning("Dealer does not have items for contract. Contract will still be marked as complete.", null);
			}
			bool flag;
			this.customer.OfferDealItems(items, false, out flag);
			this.npc.SetAnimationTrigger("GrabItem");
			this.End();
			base.gameObject.SetActive(false);
			this.contract = null;
			base.StartedThisCycle = false;
			this.handoverRoutine = null;
			yield break;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0006D975 File Offset: 0x0006BB75
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0006D98E File Offset: 0x0006BB8E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0006D9A7 File Offset: 0x0006BBA7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0006D9B5 File Offset: 0x0006BBB5
		protected virtual void dll()
		{
			base.Awake();
			this.priority = 100;
			this.MaxDuration = 720;
			this.dealer = (this.npc as Dealer);
		}

		// Token: 0x040015E8 RID: 5608
		private Dealer dealer;

		// Token: 0x040015E9 RID: 5609
		private Contract contract;

		// Token: 0x040015EA RID: 5610
		private Customer customer;

		// Token: 0x040015EB RID: 5611
		private Coroutine handoverRoutine;

		// Token: 0x040015EC RID: 5612
		private bool dll_Excuted;

		// Token: 0x040015ED RID: 5613
		private bool dll_Excuted;
	}
}
