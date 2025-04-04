using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.EntityFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B70 RID: 2928
	public class FloorRack : GridItem, IProceduralTileContainer
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0014A5B3 File Offset: 0x001487B3
		public List<ProceduralTile> ProceduralTiles
		{
			get
			{
				return this.procTiles;
			}
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x0014A5BC File Offset: 0x001487BC
		public virtual void UpdateLegVisibility()
		{
			this.CockAndBalls(this.leg_BottomLeft.gameObject, this.obs_BottomLeft, -1, -1);
			this.CockAndBalls(this.leg_BottomRight.gameObject, this.obs_BottomRight, 1, -1);
			this.CockAndBalls(this.leg_TopLeft.gameObject, this.obs_TopLeft, -1, 1);
			this.CockAndBalls(this.leg_TopRight.gameObject, this.obs_TopRight, 1, 1);
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x0014A630 File Offset: 0x00148830
		protected void CockAndBalls(GameObject leg, CornerObstacle obs, int xOffset, int yOffset)
		{
			FloorRack x = null;
			FloorRack x2 = null;
			FloorRack x3 = null;
			Coordinate coord = new Coordinate(this.CoordinatePairs[0].coord2.x + xOffset, this.CoordinatePairs[0].coord2.y + yOffset);
			if (base.OwnerGrid.GetTile(coord) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants) != null)
			{
				x = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants);
			}
			Coordinate coord2 = new Coordinate(this.CoordinatePairs[0].coord2.x + xOffset, this.CoordinatePairs[0].coord2.y);
			if (base.OwnerGrid.GetTile(coord2) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord2).BuildableOccupants) != null)
			{
				x2 = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord2).BuildableOccupants);
			}
			Coordinate coord3 = new Coordinate(this.CoordinatePairs[0].coord2.x, this.CoordinatePairs[0].coord2.y + yOffset);
			if (base.OwnerGrid.GetTile(coord3) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord3).BuildableOccupants) != null)
			{
				x3 = this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord3).BuildableOccupants);
			}
			bool flag = true;
			if ((!(x2 != null) || !(x3 != null) || !(x != null)) && x == null && (!(x2 != null) || !(x3 == null)) && x2 == null)
			{
				x3 != null;
			}
			leg.gameObject.SetActive(flag);
			obs.obstacleEnabled = flag;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0014A82C File Offset: 0x00148A2C
		private FloorRack GetFloorRackFromOccupants(List<GridItem> occs)
		{
			for (int i = 0; i < occs.Count; i++)
			{
				if (occs[i] is FloorRack)
				{
					return occs[i] as FloorRack;
				}
			}
			return null;
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x0014A868 File Offset: 0x00148A68
		public List<FloorRack> GetSurroundingRacks()
		{
			List<FloorRack> list = new List<FloorRack>();
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					if (i != 0 || j != 0)
					{
						Coordinate coord = new Coordinate(this.CoordinatePairs[0].coord2.x + i, this.CoordinatePairs[0].coord2.y + j);
						if (base.OwnerGrid.GetTile(coord) != null && this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants) != null)
						{
							list.Add(this.GetFloorRackFromOccupants(base.OwnerGrid.GetTile(coord).BuildableOccupants));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x0014A930 File Offset: 0x00148B30
		public override bool CanShareTileWith(List<GridItem> obstacles)
		{
			for (int i = 0; i < obstacles.Count; i++)
			{
				if (obstacles[i] is FloorRack)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0014A960 File Offset: 0x00148B60
		public override bool CanBeDestroyed(out string reason)
		{
			bool flag = false;
			foreach (ProceduralTile proceduralTile in this.procTiles)
			{
				if (proceduralTile.Occupants.Count > 0 || proceduralTile.OccupantTiles.Count > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				reason = base.ItemInstance.Name + " is supporting another item";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0014A9F4 File Offset: 0x00148BF4
		public override void DestroyItem(bool callOnServer = true)
		{
			for (int i = 0; i < this.CoordinatePairs.Count; i++)
			{
				base.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).RemoveOccupant(this, base.GetFootprintTile(this.CoordinatePairs[i].coord1));
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x0014AA57 File Offset: 0x00148C57
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x0014AA70 File Offset: 0x00148C70
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.FloorRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0014AA89 File Offset: 0x00148C89
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x0014AA97 File Offset: 0x00148C97
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003B41 RID: 15169
		[Header("References")]
		public Transform leg_BottomLeft;

		// Token: 0x04003B42 RID: 15170
		public Transform leg_BottomRight;

		// Token: 0x04003B43 RID: 15171
		public Transform leg_TopLeft;

		// Token: 0x04003B44 RID: 15172
		public Transform leg_TopRight;

		// Token: 0x04003B45 RID: 15173
		public CornerObstacle obs_BottomLeft;

		// Token: 0x04003B46 RID: 15174
		public CornerObstacle obs_BottomRight;

		// Token: 0x04003B47 RID: 15175
		public CornerObstacle obs_TopLeft;

		// Token: 0x04003B48 RID: 15176
		public CornerObstacle obs_TopRight;

		// Token: 0x04003B49 RID: 15177
		public List<ProceduralTile> procTiles;

		// Token: 0x04003B4A RID: 15178
		private bool dll_Excuted;

		// Token: 0x04003B4B RID: 15179
		private bool dll_Excuted;
	}
}
