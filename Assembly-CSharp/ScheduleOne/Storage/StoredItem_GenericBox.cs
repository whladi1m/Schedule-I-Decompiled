using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200089D RID: 2205
	public class StoredItem_GenericBox : StoredItem
	{
		// Token: 0x06003C13 RID: 15379 RVA: 0x000FD18C File Offset: 0x000FB38C
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			this.icon1.sprite = _item.Icon;
			this.icon2.sprite = _item.Icon;
			float num = 0.025f / (_item.Icon.rect.width / 1024f) * this.IconScale;
			this.icon1.transform.localScale = new Vector3(num, num, 1f);
			this.icon2.transform.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x04002B49 RID: 11081
		private const float ReferenceIconWidth = 1024f;

		// Token: 0x04002B4A RID: 11082
		[Header("References")]
		[SerializeField]
		protected SpriteRenderer icon1;

		// Token: 0x04002B4B RID: 11083
		[SerializeField]
		protected SpriteRenderer icon2;

		// Token: 0x04002B4C RID: 11084
		[Header("Settings")]
		public float IconScale = 0.5f;
	}
}
