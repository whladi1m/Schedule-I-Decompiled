using System;
using System.Collections.Generic;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x0200077A RID: 1914
	public class BuildUpdate_ProceduralGrid : BuildUpdate_Base
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x000DACC8 File Offset: 0x000D8EC8
		protected virtual void Update()
		{
			this.CheckRotation();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition)
			{
				this.Place();
			}
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000DACE8 File Offset: 0x000D8EE8
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.GhostModel.transform.up = Vector3.up;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, true, 0f))
			{
				this.GhostModel.transform.position = raycastHit.point - this.GhostModel.transform.InverseTransformPoint(this.ItemClass.BuildPoint.transform.position);
			}
			else
			{
				this.GhostModel.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.ItemClass.HoldDistance;
				if (this.ItemClass.MidAirCenterPoint != null)
				{
					this.GhostModel.transform.position += -this.GhostModel.transform.InverseTransformPoint(this.ItemClass.MidAirCenterPoint.transform.position);
				}
			}
			this.ApplyRotation();
			this.CheckGridIntersections();
			this.UpdateMaterials();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000DAE24 File Offset: 0x000D9024
		protected void CheckRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft) && !GameInput.IsTyping)
			{
				this.currentRotation -= 90f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight) && !GameInput.IsTyping)
			{
				this.currentRotation += 90f;
			}
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000DAE78 File Offset: 0x000D9078
		protected void ApplyRotation()
		{
			this.GhostModel.transform.rotation = Quaternion.Inverse(this.ItemClass.BuildPoint.transform.rotation) * this.GhostModel.transform.rotation;
			ProceduralTile nearbyProcTile = this.GetNearbyProcTile();
			float num = this.currentRotation;
			if (nearbyProcTile != null)
			{
				num += nearbyProcTile.transform.eulerAngles.y;
			}
			this.GhostModel.transform.Rotate(this.ItemClass.BuildPoint.up, num);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000DAF10 File Offset: 0x000D9110
		protected virtual void CheckGridIntersections()
		{
			this.ItemClass.CalculateFootprintTileIntersections();
			List<BuildUpdate_ProceduralGrid.Intersection> list = new List<BuildUpdate_ProceduralGrid.Intersection>();
			for (int i = 0; i < this.ItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				for (int j = 0; j < this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedProceduralTiles.Count; j++)
				{
					list.Add(new BuildUpdate_ProceduralGrid.Intersection
					{
						footprintTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile,
						procTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.intersectedProceduralTiles[j]
					});
				}
			}
			if (list.Count == 0)
			{
				this.ItemClass.SetFootprintTileVisiblity(false);
				return;
			}
			this.ItemClass.SetFootprintTileVisiblity(true);
			float num = 100f;
			this.bestIntersection = null;
			for (int k = 0; k < list.Count; k++)
			{
				if (Vector3.Distance(list[k].footprintTile.transform.position, list[k].procTile.transform.position) < num)
				{
					num = Vector3.Distance(list[k].footprintTile.transform.position, list[k].procTile.transform.position);
					this.bestIntersection = list[k];
				}
			}
			this.validPosition = true;
			this.GhostModel.transform.position = this.bestIntersection.procTile.transform.position - (this.bestIntersection.footprintTile.transform.position - this.GhostModel.transform.position);
			this.ItemClass.CalculateFootprintTileIntersections();
			for (int l = 0; l < this.ItemClass.CoordinateFootprintTilePairs.Count; l++)
			{
				bool flag = false;
				ProceduralTile closestProceduralTile = this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileDetector.GetClosestProceduralTile();
				if (this.IsMatchValid(this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile, closestProceduralTile))
				{
					flag = true;
				}
				if (flag)
				{
					this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.White);
				}
				else
				{
					this.validPosition = false;
					this.ItemClass.CoordinateFootprintTilePairs[l].footprintTile.tileAppearance.SetColor(ETileColor.Red);
				}
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000DB1C0 File Offset: 0x000D93C0
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

		// Token: 0x06003435 RID: 13365 RVA: 0x000DB214 File Offset: 0x000D9414
		private bool IsMatchValid(FootprintTile footprintTile, ProceduralTile matchedTile)
		{
			return !(footprintTile == null) && !(matchedTile == null) && (Vector3.Distance(matchedTile.transform.position, footprintTile.transform.position) < 0.01f && matchedTile.Occupants.Count == 0 && matchedTile.TileType == this.ItemClass.ProceduralTileType);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000DB27C File Offset: 0x000D947C
		protected void Place()
		{
			List<CoordinateProceduralTilePair> list = new List<CoordinateProceduralTilePair>();
			for (int i = 0; i < this.ItemClass.CoordinateFootprintTilePairs.Count; i++)
			{
				bool flag = false;
				ProceduralTile closestProceduralTile = this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.GetClosestProceduralTile();
				if (this.IsMatchValid(this.ItemClass.CoordinateFootprintTilePairs[i].footprintTile, closestProceduralTile))
				{
					flag = true;
				}
				if (!flag)
				{
					Console.LogWarning("Invalid placement!", null);
					return;
				}
				NetworkObject networkObject = closestProceduralTile.ParentBuildableItem.NetworkObject;
				int tileIndex = (closestProceduralTile.ParentBuildableItem as IProceduralTileContainer).ProceduralTiles.IndexOf(closestProceduralTile);
				list.Add(new CoordinateProceduralTilePair
				{
					coord = this.ItemClass.CoordinateFootprintTilePairs[i].coord,
					tileParent = networkObject,
					tileIndex = tileIndex
				});
			}
			float f = Vector3.SignedAngle(list[0].tile.transform.forward, this.GhostModel.transform.forward, list[0].tile.transform.up);
			Singleton<BuildManager>.Instance.CreateProceduralGridItem(this.ItemInstance.GetCopy(1), Mathf.RoundToInt(f), list, "");
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			Singleton<BuildManager>.Instance.PlayBuildSound((this.ItemInstance.Definition as BuildableItemDefinition).BuildSoundType, this.GhostModel.transform.position);
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000DB41C File Offset: 0x000D961C
		private ProceduralTile GetNearbyProcTile()
		{
			Collider[] array = Physics.OverlapSphere(this.GhostModel.transform.position, 1f, this.detectionMask);
			for (int i = 0; i < array.Length; i++)
			{
				ProceduralTile component = array[i].GetComponent<ProceduralTile>();
				if (component != null)
				{
					return component;
				}
			}
			return null;
		}

		// Token: 0x04002562 RID: 9570
		public GameObject GhostModel;

		// Token: 0x04002563 RID: 9571
		public ProceduralGridItem ItemClass;

		// Token: 0x04002564 RID: 9572
		public ItemInstance ItemInstance;

		// Token: 0x04002565 RID: 9573
		[Header("Settings")]
		public float detectionRange = 6f;

		// Token: 0x04002566 RID: 9574
		public LayerMask detectionMask;

		// Token: 0x04002567 RID: 9575
		public float rotation_Smoothing = 5f;

		// Token: 0x04002568 RID: 9576
		protected float currentRotation;

		// Token: 0x04002569 RID: 9577
		protected bool validPosition;

		// Token: 0x0400256A RID: 9578
		protected Material currentGhostMaterial;

		// Token: 0x0400256B RID: 9579
		protected BuildUpdate_ProceduralGrid.Intersection bestIntersection;

		// Token: 0x0200077B RID: 1915
		public class Intersection
		{
			// Token: 0x0400256C RID: 9580
			public FootprintTile footprintTile;

			// Token: 0x0400256D RID: 9581
			public ProceduralTile procTile;
		}
	}
}
