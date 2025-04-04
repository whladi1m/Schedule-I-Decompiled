using System;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BC9 RID: 3017
	public class StoredItem_WateringCan : StoredItem
	{
		// Token: 0x060054D1 RID: 21713 RVA: 0x00165284 File Offset: 0x00163484
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			WateringCanInstance wateringCanInstance = _item as WateringCanInstance;
			if (wateringCanInstance == null)
			{
				return;
			}
			this.Visuals.SetFillLevel(wateringCanInstance.CurrentFillAmount / 15f);
		}

		// Token: 0x04003ED3 RID: 16083
		public WateringCanVisuals Visuals;
	}
}
