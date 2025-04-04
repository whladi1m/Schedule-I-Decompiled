using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000BD5 RID: 3029
	public class Equippable_Cash : Equippable_Viewmodel
	{
		// Token: 0x06005502 RID: 21762 RVA: 0x00165AD9 File Offset: 0x00163CD9
		protected override void Update()
		{
			base.Update();
			if (!this.lookingAtStorageObject && GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.amountIndex++;
			}
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x00165AFF File Offset: 0x00163CFF
		protected override void StartBuildingStoredItem()
		{
			this.isBuildingStoredItem = true;
			Singleton<BuildManager>.Instance.StartPlacingCash(this.itemInstance);
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x00165B18 File Offset: 0x00163D18
		protected override void StopBuildingStoredItem()
		{
			this.isBuildingStoredItem = false;
			Singleton<BuildManager>.Instance.StopBuilding();
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x00165B2B File Offset: 0x00163D2B
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			item.onDataChanged = (Action)Delegate.Combine(item.onDataChanged, new Action(this.UpdateCashVisuals));
			this.UpdateCashVisuals();
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x00165B5C File Offset: 0x00163D5C
		public override void Unequip()
		{
			base.Unequip();
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateCashVisuals));
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x00165B8C File Offset: 0x00163D8C
		private void UpdateCashVisuals()
		{
			CashInstance cashInstance = this.itemInstance as CashInstance;
			if (cashInstance == null)
			{
				this.Container_100_300.gameObject.SetActive(false);
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_Under100.gameObject.SetActive(false);
				return;
			}
			float num = cashInstance.Balance;
			if (num < 100f)
			{
				num = Mathf.Round(num / 10f) * 10f;
				int num2 = Mathf.Clamp(Mathf.RoundToInt(num / 10f), 0, 10);
				if (num > 0f)
				{
					num2 = Mathf.Max(1, num2);
				}
				this.Container_100_300.gameObject.SetActive(false);
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_Under100.gameObject.SetActive(true);
				for (int i = 0; i < this.SingleNotes.Count; i++)
				{
					if (i < num2)
					{
						this.SingleNotes[i].gameObject.SetActive(true);
					}
					else
					{
						this.SingleNotes[i].gameObject.SetActive(false);
					}
				}
				return;
			}
			num = Mathf.Floor(num / 100f) * 100f;
			this.Container_Under100.gameObject.SetActive(false);
			if (num < 400f)
			{
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_100_300.gameObject.SetActive(true);
				for (int j = 0; j < this.Under300Stacks.Count; j++)
				{
					if ((float)j < num / 100f)
					{
						this.Under300Stacks[j].gameObject.SetActive(true);
					}
					else
					{
						this.Under300Stacks[j].gameObject.SetActive(false);
					}
				}
				return;
			}
			this.Container_100_300.gameObject.SetActive(false);
			this.Container_300Plus.gameObject.SetActive(true);
			for (int k = 0; k < this.PlusStacks.Count; k++)
			{
				if ((float)k < num / 100f)
				{
					this.PlusStacks[k].gameObject.SetActive(true);
				}
				else
				{
					this.PlusStacks[k].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04003F03 RID: 16131
		private int amountIndex;

		// Token: 0x04003F04 RID: 16132
		[Header("References")]
		public Transform Container_Under100;

		// Token: 0x04003F05 RID: 16133
		public List<Transform> SingleNotes;

		// Token: 0x04003F06 RID: 16134
		public Transform Container_100_300;

		// Token: 0x04003F07 RID: 16135
		public List<Transform> Under300Stacks;

		// Token: 0x04003F08 RID: 16136
		public Transform Container_300Plus;

		// Token: 0x04003F09 RID: 16137
		public List<Transform> PlusStacks;
	}
}
