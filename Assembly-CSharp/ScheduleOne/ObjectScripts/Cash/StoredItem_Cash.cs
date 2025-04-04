using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000BD6 RID: 3030
	public class StoredItem_Cash : StoredItem
	{
		// Token: 0x06005509 RID: 21769 RVA: 0x00165DC8 File Offset: 0x00163FC8
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			this.cashInstance = (base.item as CashInstance);
			this.RefreshShownBills();
			CashInstance cashInstance = this.cashInstance;
			cashInstance.onDataChanged = (Action)Delegate.Combine(cashInstance.onDataChanged, new Action(this.RefreshShownBills));
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x00165E1E File Offset: 0x0016401E
		private void RefreshShownBills()
		{
			this.Visuals.ShowAmount(this.cashInstance.Balance);
		}

		// Token: 0x04003F0A RID: 16138
		protected CashInstance cashInstance;

		// Token: 0x04003F0B RID: 16139
		[Header("References")]
		public CashStackVisuals Visuals;
	}
}
