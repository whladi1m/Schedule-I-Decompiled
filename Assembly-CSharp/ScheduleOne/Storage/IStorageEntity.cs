using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000886 RID: 2182
	public interface IStorageEntity
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06003AFC RID: 15100
		Transform storedItemContainer { get; }

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003AFD RID: 15101
		Dictionary<StoredItem, Employee> reservedItems { get; }

		// Token: 0x06003AFE RID: 15102
		List<StoredItem> GetStoredItems();

		// Token: 0x06003AFF RID: 15103
		List<StorageGrid> GetStorageGrids();

		// Token: 0x06003B00 RID: 15104 RVA: 0x000F8270 File Offset: 0x000F6470
		List<StoredItem> GetStoredItemsByID(string ID)
		{
			List<StoredItem> storedItems = this.GetStoredItems();
			List<StoredItem> list = new List<StoredItem>();
			for (int i = 0; i < storedItems.Count; i++)
			{
				if (storedItems[i].item.ID == ID)
				{
					list.Add(storedItems[i]);
				}
			}
			return list;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x000F82C4 File Offset: 0x000F64C4
		void ReserveItem(StoredItem item, Employee employee)
		{
			if (this.IsItemReserved(item))
			{
				if (this.reservedItems[item] != employee)
				{
					Console.LogWarning("Item already reserved by someone else!", null);
				}
				return;
			}
			this.reservedItems.Add(item, employee);
			(this as MonoBehaviour).StartCoroutine(this.ClearReserve(item));
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000F831A File Offset: 0x000F651A
		void DereserveItem(StoredItem item)
		{
			if (this.reservedItems.ContainsKey(item))
			{
				this.reservedItems.Remove(item);
			}
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000F8337 File Offset: 0x000F6537
		bool IsItemReserved(StoredItem item)
		{
			return this.reservedItems.ContainsKey(item);
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000F8345 File Offset: 0x000F6545
		Employee WhoIsReserving(StoredItem item)
		{
			if (this.reservedItems.ContainsKey(item))
			{
				return this.reservedItems[item];
			}
			return null;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000F8364 File Offset: 0x000F6564
		List<StoredItem> GetNonReservedItemsByPrefabID(string prefabID, Employee whosAskin)
		{
			List<StoredItem> storedItemsByID = this.GetStoredItemsByID(prefabID);
			List<StoredItem> list = new List<StoredItem>();
			for (int i = 0; i < storedItemsByID.Count; i++)
			{
				Employee x = this.WhoIsReserving(storedItemsByID[i]);
				if (x == null || x == whosAskin)
				{
					list.Add(storedItemsByID[i]);
				}
			}
			return list;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000F83BE File Offset: 0x000F65BE
		IEnumerator ClearReserve(StoredItem item)
		{
			yield return new WaitForSeconds(60f);
			if (item != null)
			{
				this.DereserveItem(item);
			}
			yield break;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000F83D4 File Offset: 0x000F65D4
		bool TryFitItem(int sizeX, int sizeY, out StorageGrid grid, out Coordinate originCoordinate, out float rotation)
		{
			grid = null;
			originCoordinate = new Coordinate(0, 0);
			rotation = 0f;
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < storageGrids.Count; i++)
			{
				grid = storageGrids[i];
				if (storageGrids[i].TryFitItem(sizeX, sizeY, new List<Coordinate>(), out originCoordinate, out rotation))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000F8434 File Offset: 0x000F6634
		int HowManyCanFit(int sizeX, int sizeY, int limit = 2147483647)
		{
			int num = 0;
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < storageGrids.Count; i++)
			{
				List<Coordinate> list = new List<Coordinate>();
				Coordinate originCoord;
				float rot;
				while (storageGrids[i].TryFitItem(sizeX, sizeY, list, out originCoord, out rot) && num < limit)
				{
					num++;
					List<CoordinatePair> list2 = Coordinate.BuildCoordinateMatches(originCoord, sizeX, sizeY, rot);
					for (int j = 0; j < list2.Count; j++)
					{
						list.Add(list2[i].coord2);
					}
				}
			}
			return num;
		}
	}
}
