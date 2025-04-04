using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x0200066F RID: 1647
	public class SupplierStash : MonoBehaviour
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002D8B RID: 11659 RVA: 0x000BEE86 File Offset: 0x000BD086
		// (set) Token: 0x06002D8C RID: 11660 RVA: 0x000BEE8E File Offset: 0x000BD08E
		public float CashAmount { get; private set; }

		// Token: 0x06002D8D RID: 11661 RVA: 0x000BEE98 File Offset: 0x000BD098
		protected virtual void Awake()
		{
			this.IntObj.SetMessage("View " + this.Supplier.fullName + "'s stash");
			this.IntObj.enabled = this.Supplier.RelationData.Unlocked;
			NPCRelationData relationData = this.Supplier.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType type, bool b)
			{
				this.SupplierUnlocked();
			}));
			this.Storage.StorageEntityName = this.Supplier.fullName + "'s Stash";
			this.Interacted();
			this.RecalculateCash();
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.RecalculateCash));
			this.StashPoI.enabled = this.Supplier.RelationData.Unlocked;
			this.StashPoI.SetMainText(this.Supplier.fullName + "'s Stash");
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000BEF94 File Offset: 0x000BD194
		protected virtual void Start()
		{
			this.UpdateDeadDrop();
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.UpdateDeadDrop));
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000BEFD4 File Offset: 0x000BD1D4
		private void SupplierUnlocked()
		{
			Console.Log("Supplier unlocked: " + this.Supplier.fullName, null);
			this.StashPoI.enabled = true;
			this.IntObj.enabled = true;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000BF00C File Offset: 0x000BD20C
		private void RecalculateCash()
		{
			float num = 0f;
			for (int i = 0; i < this.Storage.ItemSlots.Count; i++)
			{
				if (this.Storage.ItemSlots[i] != null && this.Storage.ItemSlots[i].ItemInstance != null && this.Storage.ItemSlots[i].ItemInstance is CashInstance)
				{
					num += (this.Storage.ItemSlots[i].ItemInstance as CashInstance).Balance;
				}
			}
			this.CashAmount = num;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000BF0B0 File Offset: 0x000BD2B0
		private void Interacted()
		{
			this.Storage.StorageEntitySubtitle = string.Concat(new string[]
			{
				"You owe ",
				this.Supplier.fullName,
				" <color=#54E717>",
				MoneyManager.FormatAmount(this.Supplier.Debt, false, false),
				"</color>. Insert cash and exit stash to pay off your debt"
			});
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000BF110 File Offset: 0x000BD310
		public void RemoveCash(float amount)
		{
			float num = amount;
			int num2 = 0;
			while (num2 < this.Storage.SlotCount && num > 0f)
			{
				if (this.Storage.ItemSlots[num2].ItemInstance != null && this.Storage.ItemSlots[num2].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = this.Storage.ItemSlots[num2].ItemInstance as CashInstance;
					float num3 = Mathf.Min(num, cashInstance.Balance);
					cashInstance.ChangeBalance(-num3);
					if (cashInstance.Balance > 0f)
					{
						this.Storage.ItemSlots[num2].SetStoredItem(cashInstance, false);
					}
					num -= num3;
				}
				num2++;
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000BF1D5 File Offset: 0x000BD3D5
		private void UpdateDeadDrop()
		{
			this.Light.Enabled = (this.Storage.ItemCount > 0);
		}

		// Token: 0x04002070 RID: 8304
		public string locationDescription = "behind the X";

		// Token: 0x04002071 RID: 8305
		[Header("References")]
		public Supplier Supplier;

		// Token: 0x04002072 RID: 8306
		public StorageEntity Storage;

		// Token: 0x04002073 RID: 8307
		public InteractableObject IntObj;

		// Token: 0x04002074 RID: 8308
		public OptimizedLight Light;

		// Token: 0x04002075 RID: 8309
		public POI StashPoI;
	}
}
