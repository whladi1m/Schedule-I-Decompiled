using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C3 RID: 707
	public class TileDetector : MonoBehaviour
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x00042454 File Offset: 0x00040654
		public virtual void CheckIntersections(bool sort = true)
		{
			this.intersectedTiles.Clear();
			this.intersectedOutdoorTiles.Clear();
			this.intersectedIndoorTiles.Clear();
			this.intersectedStorageTiles.Clear();
			this.intersectedProceduralTiles.Clear();
			LayerMask mask = default(LayerMask) | 1 << LayerMask.NameToLayer("Tile");
			Collider[] array = Physics.OverlapSphere(base.transform.position, this.detectionRadius, mask);
			for (int i = 0; i < array.Length; i++)
			{
				if (this.tileDetectionMode == ETileDetectionMode.Tile)
				{
					Tile componentInParent = array[i].GetComponentInParent<Tile>();
					if (componentInParent != null && !this.intersectedTiles.Contains(componentInParent))
					{
						this.intersectedTiles.Add(componentInParent);
					}
				}
				if (this.tileDetectionMode == ETileDetectionMode.OutdoorTile)
				{
					Tile componentInParent2 = array[i].GetComponentInParent<Tile>();
					if (componentInParent2 != null && !(componentInParent2 is IndoorTile) && !this.intersectedOutdoorTiles.Contains(componentInParent2))
					{
						this.intersectedOutdoorTiles.Add(componentInParent2);
					}
				}
				if (this.tileDetectionMode == ETileDetectionMode.IndoorTile)
				{
					IndoorTile componentInParent3 = array[i].GetComponentInParent<IndoorTile>();
					if (componentInParent3 != null && !this.intersectedIndoorTiles.Contains(componentInParent3))
					{
						this.intersectedIndoorTiles.Add(componentInParent3);
					}
				}
				if (this.tileDetectionMode == ETileDetectionMode.StorageTile)
				{
					StorageTile componentInParent4 = array[i].GetComponentInParent<StorageTile>();
					if (componentInParent4 != null && !this.intersectedStorageTiles.Contains(componentInParent4))
					{
						this.intersectedStorageTiles.Add(componentInParent4);
					}
				}
				if (this.tileDetectionMode == ETileDetectionMode.ProceduralTile)
				{
					ProceduralTile componentInParent5 = array[i].GetComponentInParent<ProceduralTile>();
					if (componentInParent5 != null && !this.intersectedProceduralTiles.Contains(componentInParent5))
					{
						this.intersectedProceduralTiles.Add(componentInParent5);
					}
				}
			}
			if (sort)
			{
				this.intersectedTiles = this.OrderList<Tile>(this.intersectedTiles);
				this.intersectedOutdoorTiles = this.OrderList<Tile>(this.intersectedOutdoorTiles);
				this.intersectedIndoorTiles = this.OrderList<Tile>(this.intersectedIndoorTiles);
				this.intersectedStorageTiles = this.OrderList<StorageTile>(this.intersectedStorageTiles);
				this.intersectedProceduralTiles = this.OrderList<ProceduralTile>(this.intersectedProceduralTiles);
			}
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0004266B File Offset: 0x0004086B
		public List<T> OrderList<T>(List<T> list) where T : MonoBehaviour
		{
			return (from x in list
			orderby Vector3.Distance(x.transform.position, base.transform.position)
			select x).ToList<T>();
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00042684 File Offset: 0x00040884
		public Tile GetClosestTile()
		{
			Tile result = null;
			float num = 100f;
			for (int i = 0; i < this.intersectedTiles.Count; i++)
			{
				if (Vector3.Distance(this.intersectedTiles[i].transform.position, base.transform.position) < num)
				{
					result = this.intersectedTiles[i];
					num = Vector3.Distance(this.intersectedTiles[i].transform.position, base.transform.position);
				}
			}
			return result;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00042710 File Offset: 0x00040910
		public ProceduralTile GetClosestProceduralTile()
		{
			ProceduralTile result = null;
			float num = 100f;
			for (int i = 0; i < this.intersectedProceduralTiles.Count; i++)
			{
				if (Vector3.Distance(this.intersectedProceduralTiles[i].transform.position, base.transform.position) < num)
				{
					result = this.intersectedProceduralTiles[i];
					num = Vector3.Distance(this.intersectedProceduralTiles[i].transform.position, base.transform.position);
				}
			}
			return result;
		}

		// Token: 0x04000F7B RID: 3963
		public float detectionRadius = 0.25f;

		// Token: 0x04000F7C RID: 3964
		public ETileDetectionMode tileDetectionMode;

		// Token: 0x04000F7D RID: 3965
		public List<Tile> intersectedTiles = new List<Tile>();

		// Token: 0x04000F7E RID: 3966
		public List<Tile> intersectedOutdoorTiles = new List<Tile>();

		// Token: 0x04000F7F RID: 3967
		public List<Tile> intersectedIndoorTiles = new List<Tile>();

		// Token: 0x04000F80 RID: 3968
		public List<StorageTile> intersectedStorageTiles = new List<StorageTile>();

		// Token: 0x04000F81 RID: 3969
		public List<ProceduralTile> intersectedProceduralTiles = new List<ProceduralTile>();
	}
}
