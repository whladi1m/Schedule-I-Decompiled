using System;
using ScheduleOne.Packaging;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008E7 RID: 2279
	public class Product_Stored : StoredItem
	{
		// Token: 0x06003E09 RID: 15881 RVA: 0x00105FE1 File Offset: 0x001041E1
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			(_item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x04002C95 RID: 11413
		public FilledPackagingVisuals Visuals;
	}
}
