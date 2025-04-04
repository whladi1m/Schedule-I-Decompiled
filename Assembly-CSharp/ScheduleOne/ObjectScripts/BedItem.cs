using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B73 RID: 2931
	public class BedItem : PlaceableStorageEntity
	{
		// Token: 0x06004E90 RID: 20112 RVA: 0x0014B2E6 File Offset: 0x001494E6
		protected override void Start()
		{
			base.Start();
			this.Bed.onAssignedEmployeeChanged.AddListener(new UnityAction(this.UpdateBriefcase));
			this.UpdateBriefcase();
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x0014B310 File Offset: 0x00149510
		public static bool IsBedValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (!(obj is BedItem))
			{
				return false;
			}
			BedItem bedItem = obj as BedItem;
			if (bedItem.Bed.AssignedEmployee != null)
			{
				reason = "Already assigned to " + bedItem.Bed.AssignedEmployee.fullName;
				return false;
			}
			return true;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x0014B368 File Offset: 0x00149568
		private void UpdateBriefcase()
		{
			this.Briefcase.gameObject.SetActive(this.Bed.AssignedEmployee != null || this.Storage.ItemCount > 0);
			if (this.Bed.AssignedEmployee != null)
			{
				this.Storage.StorageEntityName = this.Bed.AssignedEmployee.FirstName + "'s Briefcase";
				string text = "<color=#54E717>" + MoneyManager.FormatAmount(this.Bed.AssignedEmployee.DailyWage, false, false) + "</color>";
				this.Storage.StorageEntitySubtitle = string.Concat(new string[]
				{
					this.Bed.AssignedEmployee.fullName,
					" will draw ",
					this.Bed.AssignedEmployee.IsMale ? "his" : "her",
					" daily wage of ",
					text,
					" from this briefcase."
				});
				return;
			}
			this.Storage.StorageEntityName = "Briefcase";
			this.Storage.StorageEntitySubtitle = string.Empty;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x0014B494 File Offset: 0x00149694
		public float GetCashSum()
		{
			float num = 0f;
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					num += (itemSlot.ItemInstance as CashInstance).Balance;
				}
			}
			return num;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x0014B514 File Offset: 0x00149714
		public void RemoveCash(float amount)
		{
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (amount <= 0f)
				{
					break;
				}
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					CashInstance cashInstance = itemSlot.ItemInstance as CashInstance;
					float num = Mathf.Min(amount, cashInstance.Balance);
					cashInstance.ChangeBalance(-num);
					itemSlot.ReplicateStoredInstance();
					amount -= num;
				}
			}
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x0014B5B8 File Offset: 0x001497B8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x0014B5D1 File Offset: 0x001497D1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x0014B5EA File Offset: 0x001497EA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x0014B5F8 File Offset: 0x001497F8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003B6C RID: 15212
		public Bed Bed;

		// Token: 0x04003B6D RID: 15213
		public StorageEntity Storage;

		// Token: 0x04003B6E RID: 15214
		public GameObject Briefcase;

		// Token: 0x04003B6F RID: 15215
		private bool dll_Excuted;

		// Token: 0x04003B70 RID: 15216
		private bool dll_Excuted;
	}
}
