using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x02000779 RID: 1913
	public class BuildUpdate_Grid : BuildUpdate_Base
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x000D9E60 File Offset: 0x000D8060
		protected virtual void Start()
		{
			this.LateUpdate();
			if (this.closestIntersection != null)
			{
				Vector3 forward = this.closestIntersection.tile.OwnerGrid.transform.forward;
				Vector3 normalized = (PlayerSingleton<PlayerCamera>.Instance.transform.position - this.BuildableItemClass.BuildPoint.transform.position).normalized;
				normalized.y = 0f;
				float num = Vector3.SignedAngle(forward, normalized, Vector3.up);
				Debug.DrawRay(this.BuildableItemClass.BuildPoint.transform.position, forward, Color.red, 5f);
				Debug.DrawRay(this.BuildableItemClass.BuildPoint.transform.position, normalized, Color.green, 5f);
				float num2 = 90f;
				float currentRotation = (float)((int)Mathf.Round(num / num2)) * num2;
				this.CurrentRotation = currentRotation;
			}
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000D9F47 File Offset: 0x000D8147
		protected virtual void Update()
		{
			this.CheckRotation();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition)
			{
				this.Place();
			}
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000D9F68 File Offset: 0x000D8168
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.GhostModel.transform.up = Vector3.up;
			float holdDistance = this.BuildableItemClass.HoldDistance;
			float num = (Mathf.Clamp(Vector3.Angle(Vector3.down, PlayerSingleton<PlayerCamera>.Instance.transform.forward), 45f, 90f) - 45f) / 45f;
			float num2 = holdDistance * (1f + num);
			this.PositionObjectInFrontOfPlayer(num2, true, true);
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast_ExcludeBuildables(num2, out raycastHit, this.detectionMask, true))
			{
				this.ApplyRotation();
			}
			if (PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(this.BuildableItemClass.transform.position + Vector3.up * 0.1f, Vector3.down, 3f, out raycastHit, this.detectionMask, false, 0f, 45f))
			{
				this.GhostModel.transform.position = raycastHit.point - this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.BuildPoint.transform.position);
			}
			this.ApplyRotation();
			float d;
			float d2;
			float d3;
			if ((!Application.isEditor || !Input.GetKey(KeyCode.LeftAlt)) && this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
			{
				if (Vector3.Distance(this.GhostModel.transform.position - this.GhostModel.transform.right * d, PlayerSingleton<PlayerCamera>.Instance.transform.position) < Vector3.Distance(this.GhostModel.transform.position - this.GhostModel.transform.forward * d2, PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					this.GhostModel.transform.position -= this.GhostModel.transform.right * d;
					if (this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
					{
						this.GhostModel.transform.position -= this.GhostModel.transform.forward * d2;
					}
				}
				else
				{
					this.GhostModel.transform.position -= this.GhostModel.transform.forward * d2;
					if (this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
					{
						this.GhostModel.transform.position -= this.GhostModel.transform.right * d;
					}
				}
				this.GhostModel.transform.position -= this.GhostModel.transform.up * d3;
			}
			this.BuildableItemClass.CalculateFootprintTileIntersections();
			this.CheckIntersections();
			if (this.validPosition)
			{
				this.verticalOffset = Mathf.MoveTowards(this.verticalOffset, 0f, Time.deltaTime * 1f);
			}
			else
			{
				this.verticalOffset = Mathf.MoveTowards(this.verticalOffset, 0.1f, Time.deltaTime * 1f);
			}
			this.BuildableItemClass.transform.position += Vector3.up * this.verticalOffset;
			this.UpdateMaterials();
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000DA2F4 File Offset: 0x000D84F4
		protected void PositionObjectInFrontOfPlayer(float dist, bool sanitizeForward, bool buildPointAsOrigin)
		{
			Vector3 forward = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			if (sanitizeForward)
			{
				forward.y = 0f;
			}
			Vector3 position = PlayerSingleton<PlayerCamera>.Instance.transform.position + forward * dist;
			this.GhostModel.transform.position = position;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast_ExcludeBuildables(dist, out raycastHit, this.detectionMask, true))
			{
				this.GhostModel.transform.position = raycastHit.point;
				if (buildPointAsOrigin && Vector3.Angle(raycastHit.normal, Vector3.up) < 1f)
				{
					this.GhostModel.transform.position += -this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.BuildPoint.transform.position);
					return;
				}
				if (this.BuildableItemClass.MidAirCenterPoint != null)
				{
					this.GhostModel.transform.position += -this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.MidAirCenterPoint.transform.position);
					return;
				}
			}
			else if (this.BuildableItemClass.MidAirCenterPoint != null)
			{
				this.GhostModel.transform.position += -this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.MidAirCenterPoint.transform.position);
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000DA494 File Offset: 0x000D8694
		protected void CheckRotation()
		{
			if (!this.AllowRotation)
			{
				this.CurrentRotation = 0f;
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft) && !GameInput.IsTyping)
			{
				this.CurrentRotation -= 90f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight) && !GameInput.IsTyping)
			{
				this.CurrentRotation += 90f;
			}
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000DA4FC File Offset: 0x000D86FC
		protected void ApplyRotation()
		{
			this.GhostModel.transform.rotation = Quaternion.Inverse(this.BuildableItemClass.BuildPoint.transform.rotation) * this.GhostModel.transform.rotation;
			Grid hoveredGrid = this.GetHoveredGrid();
			float num = this.CurrentRotation;
			if (hoveredGrid != null)
			{
				num += hoveredGrid.transform.eulerAngles.y;
			}
			this.GhostModel.transform.Rotate(this.BuildableItemClass.BuildPoint.up, num);
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000DA594 File Offset: 0x000D8794
		private List<TileIntersection> GetRelevantIntersections(FootprintTile tile)
		{
			List<TileIntersection> list = new List<TileIntersection>();
			List<Tile> list2 = new List<Tile>();
			switch (this.BuildableItemClass.GridType)
			{
			case GridItem.EGridType.All:
				list2 = tile.tileDetector.intersectedTiles;
				break;
			case GridItem.EGridType.IndoorOnly:
				list2 = tile.tileDetector.intersectedIndoorTiles;
				break;
			case GridItem.EGridType.OutdoorOnly:
				list2 = tile.tileDetector.intersectedOutdoorTiles;
				break;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(new TileIntersection
				{
					footprint = tile,
					tile = list2[i]
				});
			}
			return list;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000DA62C File Offset: 0x000D882C
		protected virtual void CheckIntersections()
		{
			List<TileIntersection> list = new List<TileIntersection>();
			for (int i = 0; i < this.BuildableItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				list.AddRange(this.GetRelevantIntersections(this.BuildableItemClass.CoordinateFootprintTilePairs[i].footprintTile));
			}
			if (list.Count == 0 || (Application.isEditor && Input.GetKey(KeyCode.LeftControl)))
			{
				this.BuildableItemClass.SetFootprintTileVisiblity(false);
				this.closestIntersection = null;
				return;
			}
			this.BuildableItemClass.SetFootprintTileVisiblity(true);
			float num = 100f;
			this.closestIntersection = null;
			for (int j = 0; j < list.Count; j++)
			{
				if (Vector3.Distance(list[j].tile.transform.position, list[j].footprint.transform.position) < num)
				{
					num = Vector3.Distance(list[j].tile.transform.position, list[j].footprint.transform.position);
					this.closestIntersection = list[j];
				}
			}
			List<Vector2> list2 = new List<Vector2>();
			this.GhostModel.transform.position = this.closestIntersection.tile.transform.position + (this.GhostModel.transform.position - this.closestIntersection.footprint.transform.position);
			this.validPosition = true;
			for (int k = 0; k < this.BuildableItemClass.CoordinateFootprintTilePairs.Count; k++)
			{
				Coordinate matchedCoordinate = this.closestIntersection.tile.OwnerGrid.GetMatchedCoordinate(this.BuildableItemClass.CoordinateFootprintTilePairs[k].footprintTile);
				this.BuildableItemClass.CoordinateFootprintTilePairs[k].footprintTile.tileAppearance.SetColor(ETileColor.Red);
				if (this.closestIntersection.tile.OwnerGrid.GetTile(matchedCoordinate) == null)
				{
					this.validPosition = false;
				}
				else
				{
					list2.Add(new Vector2((float)matchedCoordinate.x, (float)matchedCoordinate.y));
					if (this.BuildableItemClass.CoordinateFootprintTilePairs[k].footprintTile.AreCornerObstaclesBlocked(this.closestIntersection.tile.OwnerGrid.GetTile(matchedCoordinate)))
					{
						this.validPosition = false;
					}
					else if (this.closestIntersection.tile.OwnerGrid.IsTileValidAtCoordinate(matchedCoordinate, this.BuildableItemClass.CoordinateFootprintTilePairs[k].footprintTile, this.BuildableItemClass))
					{
						this.BuildableItemClass.CoordinateFootprintTilePairs[k].footprintTile.tileAppearance.SetColor(ETileColor.White);
					}
					else
					{
						this.validPosition = false;
					}
				}
			}
			for (int l = 0; l < this.BuildableItemClass.CoordinateFootprintTilePairs.Count; l++)
			{
				Coordinate matchedCoordinate2 = this.closestIntersection.tile.OwnerGrid.GetMatchedCoordinate(this.BuildableItemClass.CoordinateFootprintTilePairs[l].footprintTile);
				Tile tile = this.closestIntersection.tile.OwnerGrid.GetTile(matchedCoordinate2);
				if (tile != null)
				{
					for (int m = 0; m < tile.OccupantTiles.Count; m++)
					{
						for (int n = 0; n < tile.OccupantTiles[m].Corners.Count; n++)
						{
							if (tile.OccupantTiles[m].Corners[n].obstacleEnabled)
							{
								List<Tile> neighbourTiles = tile.OccupantTiles[m].Corners[n].GetNeighbourTiles(tile);
								int num2 = 0;
								foreach (Tile tile2 in neighbourTiles)
								{
									if (list2.Contains(new Vector2((float)tile2.x, (float)tile2.y)))
									{
										num2++;
									}
								}
								if (num2 == 4)
								{
									this.validPosition = false;
									for (int num3 = 0; num3 < this.BuildableItemClass.CoordinateFootprintTilePairs.Count; num3++)
									{
										this.BuildableItemClass.CoordinateFootprintTilePairs[num3].footprintTile.tileAppearance.SetColor(ETileColor.Red);
									}
									return;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000DAACC File Offset: 0x000D8CCC
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
				Singleton<BuildManager>.Instance.ApplyMaterial(this.GhostModel, material, true);
			}
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000DAB20 File Offset: 0x000D8D20
		protected virtual void Place()
		{
			int rotation = Mathf.RoundToInt(Vector3.SignedAngle(this.closestIntersection.tile.OwnerGrid.transform.forward, this.BuildableItemClass.BuildPoint.forward, this.closestIntersection.tile.OwnerGrid.transform.up));
			Singleton<BuildManager>.Instance.CreateGridItem(this.ItemInstance.GetCopy(1), this.closestIntersection.tile.OwnerGrid, this.GetOriginCoordinate(), rotation, "");
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			Singleton<BuildManager>.Instance.PlayBuildSound((this.ItemInstance.Definition as BuildableItemDefinition).BuildSoundType, this.GhostModel.transform.position);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000DABF0 File Offset: 0x000D8DF0
		private Vector2 GetOriginCoordinate()
		{
			this.BuildableItemClass.OriginFootprint.tileDetector.CheckIntersections(true);
			TileIntersection tileIntersection = this.GetRelevantIntersections(this.BuildableItemClass.OriginFootprint)[0];
			return new Vector2((float)tileIntersection.tile.x, (float)tileIntersection.tile.y);
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000DAC48 File Offset: 0x000D8E48
		private Grid GetHoveredGrid()
		{
			Collider[] array = Physics.OverlapSphere(this.GhostModel.transform.position, 1.5f, this.detectionMask);
			for (int i = 0; i < array.Length; i++)
			{
				Tile component = array[i].GetComponent<Tile>();
				if (component != null)
				{
					return component.OwnerGrid;
				}
			}
			return null;
		}

		// Token: 0x04002556 RID: 9558
		public GameObject GhostModel;

		// Token: 0x04002557 RID: 9559
		public GridItem BuildableItemClass;

		// Token: 0x04002558 RID: 9560
		public ItemInstance ItemInstance;

		// Token: 0x04002559 RID: 9561
		public float CurrentRotation;

		// Token: 0x0400255A RID: 9562
		[Header("Settings")]
		public float detectionRange = 6f;

		// Token: 0x0400255B RID: 9563
		public LayerMask detectionMask;

		// Token: 0x0400255C RID: 9564
		public float rotation_Smoothing = 5f;

		// Token: 0x0400255D RID: 9565
		public bool AllowRotation = true;

		// Token: 0x0400255E RID: 9566
		protected bool validPosition;

		// Token: 0x0400255F RID: 9567
		protected Material currentGhostMaterial;

		// Token: 0x04002560 RID: 9568
		protected TileIntersection closestIntersection;

		// Token: 0x04002561 RID: 9569
		private float verticalOffset;
	}
}
