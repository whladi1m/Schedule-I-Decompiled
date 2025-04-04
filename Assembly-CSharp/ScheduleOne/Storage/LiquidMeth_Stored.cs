using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000884 RID: 2180
	public class LiquidMeth_Stored : StoredItem
	{
		// Token: 0x06003AF5 RID: 15093 RVA: 0x000F81D4 File Offset: 0x000F63D4
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			LiquidMethDefinition def = _item.Definition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(def);
			}
		}

		// Token: 0x04002ADF RID: 10975
		public LiquidMethVisuals Visuals;
	}
}
