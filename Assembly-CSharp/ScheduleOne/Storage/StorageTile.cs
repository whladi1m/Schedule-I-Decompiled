using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000895 RID: 2197
	public class StorageTile : MonoBehaviour
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x000FC096 File Offset: 0x000FA296
		public StorageGrid _ownerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x000FC09E File Offset: 0x000FA29E
		// (set) Token: 0x06003BD7 RID: 15319 RVA: 0x000FC0A6 File Offset: 0x000FA2A6
		public StoredItem occupant { get; protected set; }

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000FC0AF File Offset: 0x000FA2AF
		public void InitializeStorageTile(int _x, int _y, float _available_Offset, StorageGrid _ownerGrid)
		{
			this.x = _x;
			this.y = _y;
			this.ownerGrid = _ownerGrid;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000FC0C7 File Offset: 0x000FA2C7
		public void SetOccupant(StoredItem occ)
		{
			if (occ != null && this.occupant != null)
			{
				Console.LogWarning("SetOccupant called by there is an existing occupant. Existing occupant should be dealt with before calling this.", null);
			}
			this.occupant = occ;
			if (this.onOccupantChanged != null)
			{
				this.onOccupantChanged();
			}
		}

		// Token: 0x04002B26 RID: 11046
		public int x;

		// Token: 0x04002B27 RID: 11047
		public int y;

		// Token: 0x04002B28 RID: 11048
		[SerializeField]
		public StorageGrid ownerGrid;

		// Token: 0x04002B29 RID: 11049
		public Action onOccupantChanged;
	}
}
