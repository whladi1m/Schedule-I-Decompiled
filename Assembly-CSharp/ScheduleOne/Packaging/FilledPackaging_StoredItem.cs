using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000870 RID: 2160
	public class FilledPackaging_StoredItem : StoredItem
	{
		// Token: 0x06003A8F RID: 14991 RVA: 0x000F6628 File Offset: 0x000F4828
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			(base.item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x000F664C File Offset: 0x000F484C
		public override GameObject CreateGhostModel(ItemInstance _item, Transform parent)
		{
			GameObject gameObject = base.CreateGhostModel(_item, parent);
			(_item as ProductItemInstance).SetupPackagingVisuals(gameObject.GetComponent<FilledPackaging_StoredItem>().Visuals);
			return gameObject;
		}

		// Token: 0x04002A68 RID: 10856
		public FilledPackagingVisuals Visuals;
	}
}
