using System;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B86 RID: 2950
	public class Meth_Stored : StoredItem
	{
		// Token: 0x06004F80 RID: 20352 RVA: 0x0014F23C File Offset: 0x0014D43C
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			MethInstance methInstance = _item as MethInstance;
			if (methInstance != null)
			{
				this.Visuals.Setup(methInstance.Definition as MethDefinition);
			}
		}

		// Token: 0x04003BFC RID: 15356
		public MethVisuals Visuals;
	}
}
