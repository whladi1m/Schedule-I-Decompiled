using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Storage
{
	// Token: 0x02000891 RID: 2193
	[RequireComponent(typeof(StorageEntity))]
	public class StorageEntityVisualizer : StorageVisualizer
	{
		// Token: 0x06003BB0 RID: 15280 RVA: 0x000FB938 File Offset: 0x000F9B38
		protected virtual void Start()
		{
			this.storageEntity = base.GetComponent<StorageEntity>();
			this.storageEntity.onContentsChanged.AddListener(new UnityAction(base.QueueRefresh));
			for (int i = 0; i < this.storageEntity.ItemSlots.Count; i++)
			{
				base.AddSlot(this.storageEntity.ItemSlots[i], false);
			}
			if (this.storageEntity.ItemCount > 0)
			{
				base.QueueRefresh();
			}
		}

		// Token: 0x04002B17 RID: 11031
		private StorageEntity storageEntity;
	}
}
