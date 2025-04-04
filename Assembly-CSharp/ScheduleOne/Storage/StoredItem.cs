using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ScheduleOne.Storage
{
	// Token: 0x02000899 RID: 2201
	public class StoredItem : MonoBehaviour
	{
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x000FC895 File Offset: 0x000FAA95
		// (set) Token: 0x06003BE8 RID: 15336 RVA: 0x000FC89D File Offset: 0x000FAA9D
		public StorableItemInstance item { get; protected set; }

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x000FC8A6 File Offset: 0x000FAAA6
		// (set) Token: 0x06003BEA RID: 15338 RVA: 0x000FC8AE File Offset: 0x000FAAAE
		public bool Destroyed { get; private set; }

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x000FC8B7 File Offset: 0x000FAAB7
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].tile;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003BEC RID: 15340 RVA: 0x000FC8CC File Offset: 0x000FAACC
		public int FootprintX
		{
			get
			{
				if (this.footprintX == -1)
				{
					this.footprintX = (from c in this.CoordinateFootprintTilePairs
					orderby c.coord.x descending
					select c).FirstOrDefault<CoordinateStorageFootprintTilePair>().coord.x + 1;
				}
				return this.footprintX;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x000FC92C File Offset: 0x000FAB2C
		public int FootprintY
		{
			get
			{
				if (this.footprintY == -1)
				{
					this.footprintY = (from c in this.CoordinateFootprintTilePairs
					orderby c.coord.y descending
					select c).FirstOrDefault<CoordinateStorageFootprintTilePair>().coord.y + 1;
				}
				return this.footprintY;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003BEE RID: 15342 RVA: 0x000FC989 File Offset: 0x000FAB89
		// (set) Token: 0x06003BEF RID: 15343 RVA: 0x000FC991 File Offset: 0x000FAB91
		public IStorageEntity parentStorageEntity { get; protected set; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x000FC99A File Offset: 0x000FAB9A
		// (set) Token: 0x06003BF1 RID: 15345 RVA: 0x000FC9A2 File Offset: 0x000FABA2
		public StorageGrid parentGrid { get; protected set; }

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x000FC9AB File Offset: 0x000FABAB
		public List<CoordinatePair> CoordinatePairs
		{
			get
			{
				return this.coordinatePairs;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x000FC9B3 File Offset: 0x000FABB3
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003BF4 RID: 15348 RVA: 0x000FC9BB File Offset: 0x000FABBB
		public int totalArea
		{
			get
			{
				return this.CoordinateFootprintTilePairs.Count;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x000FC9C8 File Offset: 0x000FABC8
		// (set) Token: 0x06003BF6 RID: 15350 RVA: 0x000FC9D0 File Offset: 0x000FABD0
		public bool canBePickedUp { get; protected set; } = true;

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x000FC9D9 File Offset: 0x000FABD9
		// (set) Token: 0x06003BF8 RID: 15352 RVA: 0x000FC9E1 File Offset: 0x000FABE1
		public string noPickupReason { get; protected set; } = string.Empty;

		// Token: 0x06003BF9 RID: 15353 RVA: 0x000FC9EC File Offset: 0x000FABEC
		protected virtual void Awake()
		{
			MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					componentsInChildren[i].enabled = false;
				}
				else
				{
					componentsInChildren[i].shadowCastingMode = ShadowCastingMode.Off;
				}
			}
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x000FCA30 File Offset: 0x000FAC30
		protected virtual void OnValidate()
		{
			if (base.gameObject.layer != LayerMask.NameToLayer("StoredItem"))
			{
				StoredItem.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("StoredItem"));
			}
			if (this.CoordinateFootprintTilePairs.Count == 0)
			{
				Debug.LogWarning("StoredItem (" + base.gameObject.name + ") has no CoordinateFootprintTilePairs!");
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x000FCA98 File Offset: 0x000FAC98
		public virtual void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			if (grid == null)
			{
				Console.LogError("InitializeStoredItem: grid is null!", null);
				this.DestroyStoredItem();
				return;
			}
			this.item = _item;
			this.parentGrid = grid;
			this.rotation = _rotation;
			StoredItem.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("StoredItem"));
			this.coordinatePairs = Coordinate.BuildCoordinateMatches(new Coordinate(_originCoordinate), this.FootprintX, this.FootprintY, this.Rotation);
			this.RefreshTransform();
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				StorageTile tile = this.parentGrid.GetTile(this.coordinatePairs[i].coord2);
				if (tile == null)
				{
					string str = "Failed to find tile at ";
					Coordinate coord = this.coordinatePairs[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null) + " when initializing stored item!", null);
					this.DestroyStoredItem();
					return;
				}
				if (tile.occupant != null)
				{
					this.DestroyStoredItem();
					return;
				}
				tile.SetOccupant(this);
				grid.freeTiles.Remove(tile);
			}
			this.intObj = base.GetComponentInChildren<InteractableObject>();
			if (this.intObj != null)
			{
				UnityEngine.Object.Destroy(this.intObj);
			}
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x000FCBE4 File Offset: 0x000FADE4
		private void RefreshTransform()
		{
			FootprintTile tile = this.GetTile(this.coordinatePairs[0].coord1);
			StorageTile tile2 = this.parentGrid.GetTile(this.coordinatePairs[0].coord2);
			base.transform.rotation = this.parentGrid.transform.rotation * (Quaternion.Inverse(this.buildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.buildPoint.up, this.rotation);
			base.transform.position = tile2.transform.position - (tile.transform.position - base.transform.position);
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x000FCCC0 File Offset: 0x000FAEC0
		protected virtual void InitializeIntObj()
		{
			this.intObj = base.GetComponentInChildren<InteractableObject>();
			if (this.intObj == null)
			{
				this.intObj = base.gameObject.AddComponent<InteractableObject>();
			}
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x000FCD34 File Offset: 0x000FAF34
		public virtual void Destroy_Internal()
		{
			this.Destroyed = true;
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				this.parentGrid.GetTile(this.coordinatePairs[i].coord2).SetOccupant(null);
			}
			if (base.GetComponentInParent<IStorageEntity>() != null)
			{
				base.GetComponentInParent<IStorageEntity>().DereserveItem(this);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x000FCD9F File Offset: 0x000FAF9F
		public void DestroyStoredItem()
		{
			this.Destroyed = true;
			this.ClearFootprintOccupancy();
			if (this != null && base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x000FCDD0 File Offset: 0x000FAFD0
		public void ClearFootprintOccupancy()
		{
			if (this.parentGrid == null)
			{
				return;
			}
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				StorageTile tile = this.parentGrid.GetTile(this.coordinatePairs[i].coord2);
				if (!(tile == null))
				{
					tile.SetOccupant(null);
					this.parentGrid.freeTiles.Add(tile);
				}
			}
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x000FCE40 File Offset: 0x000FB040
		public void SetCanBePickedUp(bool _canBePickedUp, string _noPickupReason = "")
		{
			this.canBePickedUp = _canBePickedUp;
			this.noPickupReason = _noPickupReason;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x000FCE50 File Offset: 0x000FB050
		public static void SetLayerRecursively(GameObject go, int layerNumber)
		{
			foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					transform.gameObject.layer = layerNumber;
				}
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000FCE9C File Offset: 0x000FB09C
		public static List<StoredItem> RemoveReservedItems(List<StoredItem> itemList, Employee allowedReservant)
		{
			return (from x in itemList
			where x.parentStorageEntity.WhoIsReserving(x) == null || x.parentStorageEntity.WhoIsReserving(x) == allowedReservant
			select x).ToList<StoredItem>();
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x000FCECD File Offset: 0x000FB0CD
		public virtual GameObject CreateGhostModel(ItemInstance _item, Transform parent)
		{
			return UnityEngine.Object.Instantiate<GameObject>(base.gameObject, parent);
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x000FCEDC File Offset: 0x000FB0DC
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].tile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000FCF1C File Offset: 0x000FB11C
		public void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].tile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x000FCF5C File Offset: 0x000FB15C
		public FootprintTile GetTile(Coordinate coord)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				if (this.CoordinateFootprintTilePairs[i].coord.Equals(coord))
				{
					return this.CoordinateFootprintTilePairs[i].tile;
				}
			}
			return null;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x000FCFAC File Offset: 0x000FB1AC
		public virtual void Hovered()
		{
			if (this.canBePickedUp)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.item, 1))
				{
					this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.intObj.SetMessage(string.Concat(new string[]
					{
						"Pick up <color=#",
						ColorUtility.ToHtmlStringRGBA(this.item.LabelDisplayColor),
						">",
						this.item.Name,
						"</color>"
					}));
					return;
				}
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.intObj.SetMessage("Inventory full");
				return;
			}
			else
			{
				if (this.noPickupReason != "")
				{
					this.intObj.SetMessage(this.noPickupReason);
					this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
					return;
				}
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000FD08C File Offset: 0x000FB28C
		public virtual void Interacted()
		{
			if (!this.canBePickedUp)
			{
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.item);
			this.DestroyStoredItem();
		}

		// Token: 0x04002B37 RID: 11063
		[Header("References")]
		public Transform buildPoint;

		// Token: 0x04002B38 RID: 11064
		public List<CoordinateStorageFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateStorageFootprintTilePair>();

		// Token: 0x04002B39 RID: 11065
		private int footprintX = -1;

		// Token: 0x04002B3A RID: 11066
		private int footprintY = -1;

		// Token: 0x04002B3D RID: 11069
		protected InteractableObject intObj;

		// Token: 0x04002B3E RID: 11070
		protected List<CoordinatePair> coordinatePairs = new List<CoordinatePair>();

		// Token: 0x04002B3F RID: 11071
		protected float rotation;

		// Token: 0x04002B40 RID: 11072
		public int xSize;

		// Token: 0x04002B41 RID: 11073
		public int ySize;
	}
}
