using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x0200077C RID: 1916
	public class BuildUpdate_StoredItem : BuildUpdate_Base
	{
		// Token: 0x0600343A RID: 13370 RVA: 0x000DB490 File Offset: 0x000D9690
		protected virtual void Update()
		{
			this.CheckRotation();
			if (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.mouseUpSinceStart = true;
				this.mouseUpSincePlace = true;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition && this.mouseUpSinceStart)
			{
				this.Place();
			}
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000DB4CC File Offset: 0x000D96CC
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.ghostModel.transform.up = Vector3.up;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0f))
			{
				this.ghostModel.transform.position = raycastHit.point - this.ghostModel.transform.InverseTransformPoint(this.storedItemClass.buildPoint.transform.position);
			}
			else
			{
				this.ghostModel.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.storedItemHoldDistance;
			}
			this.ApplyRotation();
			this.storedItemClass.CalculateFootprintTileIntersections();
			this.CheckGridIntersections();
			if (this.validPosition)
			{
				this.positionDuringLastValidPosition = this.ghostModel.transform.position;
			}
			else if (this.mouseUpSincePlace)
			{
				Vector3 position = this.ghostModel.transform.position;
				float d = 0.0625f;
				this.ghostModel.transform.position = position + this.ghostModel.transform.right * d;
				this.storedItemClass.CalculateFootprintTileIntersections();
				this.CheckGridIntersections();
				if (!this.validPosition)
				{
					this.ghostModel.transform.position = position - this.ghostModel.transform.right * d;
					this.storedItemClass.CalculateFootprintTileIntersections();
					this.CheckGridIntersections();
					if (!this.validPosition)
					{
						this.ghostModel.transform.position = position + this.ghostModel.transform.forward * d;
						this.storedItemClass.CalculateFootprintTileIntersections();
						this.CheckGridIntersections();
						if (!this.validPosition)
						{
							this.ghostModel.transform.position = position - this.ghostModel.transform.forward * d;
							this.storedItemClass.CalculateFootprintTileIntersections();
							this.CheckGridIntersections();
							if (!this.validPosition)
							{
								this.ghostModel.transform.position = position;
								this.storedItemClass.CalculateFootprintTileIntersections();
								this.CheckGridIntersections();
							}
						}
					}
				}
			}
			this.UpdateMaterials();
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000DB734 File Offset: 0x000D9934
		protected void CheckRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft))
			{
				this.currentRotation -= 90f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight))
			{
				this.currentRotation += 90f;
			}
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000DB76C File Offset: 0x000D996C
		protected void ApplyRotation()
		{
			this.ghostModel.transform.rotation = Quaternion.Inverse(this.storedItemClass.buildPoint.transform.rotation) * this.ghostModel.transform.rotation;
			this.ghostModel.transform.Rotate(this.storedItemClass.buildPoint.up, this.currentRotation);
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000DB7E0 File Offset: 0x000D99E0
		protected virtual void CheckGridIntersections()
		{
			List<BuildUpdate_StoredItem.StorageTileIntersection> list = new List<BuildUpdate_StoredItem.StorageTileIntersection>();
			for (int i = 0; i < this.storedItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				for (int j = 0; j < this.storedItemClass.CoordinateFootprintTilePairs[i].tile.tileDetector.intersectedStorageTiles.Count; j++)
				{
					list.Add(new BuildUpdate_StoredItem.StorageTileIntersection
					{
						footprintTile = this.storedItemClass.CoordinateFootprintTilePairs[i].tile,
						storageTile = this.storedItemClass.CoordinateFootprintTilePairs[i].tile.tileDetector.intersectedStorageTiles[j]
					});
				}
			}
			if (list.Count == 0)
			{
				this.storedItemClass.SetFootprintTileVisiblity(false);
				this.bestIntersection = null;
				return;
			}
			this.storedItemClass.SetFootprintTileVisiblity(true);
			float num = 100f;
			this.bestIntersection = null;
			for (int k = 0; k < list.Count; k++)
			{
				if (this.bestIntersection == null || Vector3.Distance(list[k].footprintTile.transform.position, list[k].storageTile.transform.position) < num)
				{
					num = Vector3.Distance(list[k].footprintTile.transform.position, list[k].storageTile.transform.position);
					this.bestIntersection = list[k];
				}
			}
			if (this.bestIntersection != null && this.bestIntersection.storageTile.GetComponentInParent<Pallet>())
			{
				Vector3 vector = this.bestIntersection.storageTile.transform.forward;
				if (Vector3.Angle(base.transform.forward, -this.bestIntersection.storageTile.transform.forward) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = -this.bestIntersection.storageTile.transform.forward;
				}
				if (Vector3.Angle(base.transform.forward, this.bestIntersection.storageTile.transform.right) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = this.bestIntersection.storageTile.transform.right;
				}
				if (Vector3.Angle(base.transform.forward, -this.bestIntersection.storageTile.transform.right) < Vector3.Angle(base.transform.forward, vector))
				{
					vector = -this.bestIntersection.storageTile.transform.right;
				}
				this.ghostModel.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
				this.ghostModel.transform.Rotate(this.storedItemClass.buildPoint.up, this.currentRotation);
			}
			this.ghostModel.transform.position = this.bestIntersection.storageTile.transform.position - (this.bestIntersection.footprintTile.transform.position - this.ghostModel.transform.position);
			this.validPosition = this.bestIntersection.storageTile.ownerGrid.IsItemPositionValid(this.bestIntersection.storageTile, this.bestIntersection.footprintTile, this.storedItemClass);
			for (int l = 0; l < this.storedItemClass.CoordinateFootprintTilePairs.Count; l++)
			{
				Coordinate matchedCoordinate = this.bestIntersection.storageTile.ownerGrid.GetMatchedCoordinate(this.storedItemClass.CoordinateFootprintTilePairs[l].tile);
				CoordinateStorageFootprintTilePair coordinateStorageFootprintTilePair = this.storedItemClass.CoordinateFootprintTilePairs[l];
				if (this.bestIntersection.storageTile.ownerGrid.IsGridPositionValid(matchedCoordinate, this.storedItemClass.CoordinateFootprintTilePairs[l].tile))
				{
					this.storedItemClass.CoordinateFootprintTilePairs[l].tile.tileAppearance.SetColor(ETileColor.White);
				}
				else
				{
					this.storedItemClass.CoordinateFootprintTilePairs[l].tile.tileAppearance.SetColor(ETileColor.Red);
				}
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000DBC5C File Offset: 0x000D9E5C
		protected void UpdateMaterials()
		{
			Material material = Singleton<BuildManager>.Instance.ghostMaterial_White;
			if (!this.validPosition)
			{
				material = Singleton<BuildManager>.Instance.ghostMaterial_Red;
			}
			if (this.currentGhostMaterial != material)
			{
				this.currentGhostMaterial = material;
				Singleton<BuildManager>.Instance.ApplyMaterial(this.ghostModel, material, true);
			}
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000DBCB0 File Offset: 0x000D9EB0
		protected virtual void Place()
		{
			float rotation = Vector3.SignedAngle(this.bestIntersection.storageTile.ownerGrid.transform.forward, this.storedItemClass.buildPoint.forward, this.bestIntersection.storageTile.ownerGrid.transform.up);
			StorableItemInstance item = this.itemInstance.GetCopy(1) as StorableItemInstance;
			Singleton<BuildManager>.Instance.CreateStoredItem(item, this.bestIntersection.storageTile.ownerGrid.GetComponentInParent<IStorageEntity>(), this.bestIntersection.storageTile.ownerGrid, this.GetOriginCoordinate(), rotation);
			this.mouseUpSincePlace = false;
			this.PostPlace();
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000DBD5D File Offset: 0x000D9F5D
		protected virtual void PostPlace()
		{
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000DBD70 File Offset: 0x000D9F70
		protected Vector2 GetOriginCoordinate()
		{
			this.storedItemClass.OriginFootprint.tileDetector.CheckIntersections(true);
			return new Vector2((float)this.storedItemClass.OriginFootprint.tileDetector.intersectedStorageTiles[0].x, (float)this.storedItemClass.OriginFootprint.tileDetector.intersectedStorageTiles[0].y);
		}

		// Token: 0x0400256E RID: 9582
		public StorableItemInstance itemInstance;

		// Token: 0x0400256F RID: 9583
		public GameObject ghostModel;

		// Token: 0x04002570 RID: 9584
		public StoredItem storedItemClass;

		// Token: 0x04002571 RID: 9585
		protected BuildUpdate_StoredItem.StorageTileIntersection bestIntersection;

		// Token: 0x04002572 RID: 9586
		[Header("Settings")]
		public float detectionRange = 6f;

		// Token: 0x04002573 RID: 9587
		public LayerMask detectionMask;

		// Token: 0x04002574 RID: 9588
		public float storedItemHoldDistance = 2f;

		// Token: 0x04002575 RID: 9589
		public float currentRotation;

		// Token: 0x04002576 RID: 9590
		protected bool validPosition;

		// Token: 0x04002577 RID: 9591
		protected Material currentGhostMaterial;

		// Token: 0x04002578 RID: 9592
		protected bool mouseUpSinceStart;

		// Token: 0x04002579 RID: 9593
		protected bool mouseUpSincePlace = true;

		// Token: 0x0400257A RID: 9594
		private Vector3 positionDuringLastValidPosition = Vector3.zero;

		// Token: 0x0200077D RID: 1917
		public class StorageTileIntersection
		{
			// Token: 0x0400257B RID: 9595
			public FootprintTile footprintTile;

			// Token: 0x0400257C RID: 9596
			public StorageTile storageTile;
		}
	}
}
