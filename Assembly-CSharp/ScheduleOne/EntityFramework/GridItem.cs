using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x0200061D RID: 1565
	public class GridItem : BuildableItem
	{
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x000AAF4F File Offset: 0x000A914F
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].footprintTile;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002965 RID: 10597 RVA: 0x000AAF62 File Offset: 0x000A9162
		public int FootprintX
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.x descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.x + 1;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x000AAF9F File Offset: 0x000A919F
		public int FootprintY
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.y descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.y + 1;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x000AAFDC File Offset: 0x000A91DC
		// (set) Token: 0x06002968 RID: 10600 RVA: 0x000AAFE4 File Offset: 0x000A91E4
		public Grid OwnerGrid { get; protected set; }

		// Token: 0x06002969 RID: 10601 RVA: 0x000AAFED File Offset: 0x000A91ED
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.GridItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000AB001 File Offset: 0x000A9201
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.Initialized && base.LocallyBuilt)
			{
				base.StartCoroutine(this.<OnStartClient>g__WaitForDataSend|18_0());
			}
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000AB028 File Offset: 0x000A9228
		protected override void SendInitToClient(NetworkConnection conn)
		{
			this.InitializeGridItem(conn, base.ItemInstance, this.OwnerGridGUID.ToString(), this.OriginCoordinate, this.Rotation, base.GUID.ToString());
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000AB073 File Offset: 0x000A9273
		[ServerRpc(RequireOwnership = false)]
		public void SendGridItemData(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.RpcWriter___Server_SendGridItemData_2821640832(instance, gridGUID, originCoordinate, rotation, GUID);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000AB090 File Offset: 0x000A9290
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public virtual void InitializeGridItem(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
				this.RpcLogic___InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
			}
			else
			{
				this.RpcWriter___Target_InitializeGridItem_1883577149(conn, instance, gridGUID, originCoordinate, rotation, GUID);
			}
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000AB101 File Offset: 0x000A9301
		public virtual void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (base.Initialized)
			{
				return;
			}
			base.InitializeBuildableItem(instance, GUID, this.GetProperty(grid.transform).PropertyCode);
			this.SetGridData(grid.GUID, originCoordinate, rotation);
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000AB138 File Offset: 0x000A9338
		protected void SetGridData(Guid gridGUID, Vector2 originCoordinate, int rotation)
		{
			Grid @object = GUIDManager.GetObject<Grid>(gridGUID);
			if (@object == null)
			{
				Console.LogError("InitializeConstructable_GridBased: grid is null", null);
				this.DestroyItem(true);
				return;
			}
			this.OwnerGridGUID = gridGUID;
			this.OwnerGrid = @object;
			this.OriginCoordinate = originCoordinate;
			this.Rotation = this.ValidateRotation(rotation);
			this.ProcessGridData();
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000AB190 File Offset: 0x000A9390
		private int ValidateRotation(int rotation)
		{
			if (float.IsNaN((float)rotation) || float.IsInfinity((float)rotation))
			{
				Console.LogWarning("Invalid rotation value: " + rotation.ToString() + " resetting to 0", null);
				return 0;
			}
			if (rotation != 0 && rotation != 90 && rotation != 180 && rotation != 270)
			{
				Console.LogWarning("Invalid rotation value: " + rotation.ToString() + ". Rounding to nearest 90 degrees", null);
				return Mathf.RoundToInt((float)(rotation / 90)) * 90;
			}
			return rotation;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000AB210 File Offset: 0x000A9410
		private void ProcessGridData()
		{
			this.OwnerGrid = GUIDManager.GetObject<Grid>(this.OwnerGridGUID);
			if (this.OwnerGrid == null)
			{
				Console.LogWarning("GridItem OwnerGrid is null", null);
				return;
			}
			base.ParentProperty = this.GetProperty(this.OwnerGrid.transform);
			if (base.NetworkObject.IsSpawned)
			{
				base.transform.SetParent(this.OwnerGrid.Container);
			}
			else
			{
				base.StartCoroutine(this.<ProcessGridData>g__Routine|25_0());
			}
			List<CoordinatePair> list = Coordinate.BuildCoordinateMatches(new Coordinate(this.OriginCoordinate), this.FootprintX, this.FootprintY, (float)this.Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.OwnerGrid.GetTile(list[i].coord2) == null)
				{
					string str = "ReceiveData: grid does not contain tile at ";
					Coordinate coord = list[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null), null);
					this.DestroyItem(true);
					return;
				}
			}
			this.ClearPositionData();
			this.CoordinatePairs.AddRange(list);
			this.RefreshTransform();
			for (int j = 0; j < this.CoordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.CoordinatePairs[j].coord2).AddOccupant(this, this.GetFootprintTile(this.CoordinatePairs[j].coord1));
				this.GetFootprintTile(this.CoordinatePairs[j].coord1).Initialize(this.OwnerGrid.GetTile(this.CoordinatePairs[j].coord2));
			}
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000AB3B8 File Offset: 0x000A95B8
		private void RefreshTransform()
		{
			base.transform.rotation = this.OwnerGrid.transform.rotation * (Quaternion.Inverse(this.BuildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.BuildPoint.up, (float)this.Rotation);
			base.transform.position = this.OwnerGrid.GetTile(this.CoordinatePairs[0].coord2).transform.position - (this.OriginFootprint.transform.position - base.transform.position);
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000AB480 File Offset: 0x000A9680
		private void ClearPositionData()
		{
			if (this.OwnerGrid != null)
			{
				for (int i = 0; i < this.CoordinatePairs.Count; i++)
				{
					this.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).RemoveOccupant(this, this.GetFootprintTile(this.CoordinatePairs[i].coord1));
				}
			}
			this.CoordinatePairs.Clear();
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000AB4F5 File Offset: 0x000A96F5
		public override void DestroyItem(bool callOnServer = true)
		{
			this.ClearPositionData();
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000AB504 File Offset: 0x000A9704
		public virtual void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000AB544 File Offset: 0x000A9744
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000AB584 File Offset: 0x000A9784
		public FootprintTile GetFootprintTile(Coordinate coord)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				if (this.CoordinateFootprintTilePairs[i].coord.Equals(coord))
				{
					return this.CoordinateFootprintTilePairs[i].footprintTile;
				}
			}
			return null;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000AB5D4 File Offset: 0x000A97D4
		public Tile GetParentTileAtFootprintCoordinate(Coordinate footprintCoord)
		{
			return this.OwnerGrid.GetTile(this.CoordinatePairs.Find((CoordinatePair x) => x.coord1 == footprintCoord).coord2);
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000AB618 File Offset: 0x000A9818
		public virtual bool CanShareTileWith(List<GridItem> obstacles)
		{
			for (int i = 0; i < obstacles.Count; i++)
			{
				if (!(obstacles[i] is FloorRack))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000AB647 File Offset: 0x000A9847
		public override string GetSaveString()
		{
			return new GridItemData(base.GUID, base.ItemInstance, 0, this.OwnerGrid, this.OriginCoordinate, this.Rotation).GetJson(true);
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000AB691 File Offset: 0x000A9891
		[CompilerGenerated]
		private IEnumerator <OnStartClient>g__WaitForDataSend|18_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SendGridItemData(base.ItemInstance, this.OwnerGridGUID.ToString(), this.OriginCoordinate, this.Rotation, base.GUID.ToString());
			yield break;
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000AB6AD File Offset: 0x000A98AD
		[CompilerGenerated]
		private IEnumerator <ProcessGridData>g__Routine|25_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			base.transform.SetParent(this.OwnerGrid.Container);
			yield break;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000AB6BC File Offset: 0x000A98BC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendGridItemData_2821640832));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_InitializeGridItem_1883577149));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_InitializeGridItem_1883577149));
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000AB725 File Offset: 0x000A9925
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.GridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000AB73E File Offset: 0x000A993E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000AB74C File Offset: 0x000A994C
		private void RpcWriter___Server_SendGridItemData_2821640832(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, AutoPackType.Packed);
			writer.WriteString(GUID);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000AB82C File Offset: 0x000A9A2C
		public void RpcLogic___SendGridItemData_2821640832(ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.InitializeGridItem(null, instance, gridGUID, originCoordinate, rotation, GUID);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000AB83C File Offset: 0x000A9A3C
		private void RpcReader___Server_SendGridItemData_2821640832(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendGridItemData_2821640832(instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000AB8B8 File Offset: 0x000A9AB8
		private void RpcWriter___Target_InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, AutoPackType.Packed);
			writer.WriteString(GUID);
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000AB9A6 File Offset: 0x000A9BA6
		public virtual void RpcLogic___InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			this.InitializeGridItem(instance, GUIDManager.GetObject<Grid>(new Guid(gridGUID)), originCoordinate, rotation, GUID);
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000AB9C0 File Offset: 0x000A9BC0
		private void RpcReader___Target_InitializeGridItem_1883577149(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InitializeGridItem_1883577149(base.LocalConnection, instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000ABA40 File Offset: 0x000A9C40
		private void RpcWriter___Observers_InitializeGridItem_1883577149(NetworkConnection conn, ItemInstance instance, string gridGUID, Vector2 originCoordinate, int rotation, string GUID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteInt32(rotation, AutoPackType.Packed);
			writer.WriteString(GUID);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000ABB30 File Offset: 0x000A9D30
		private void RpcReader___Observers_InitializeGridItem_1883577149(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string gridGUID = PooledReader0.ReadString();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___InitializeGridItem_1883577149(null, instance, gridGUID, originCoordinate, rotation, guid);
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000ABBB5 File Offset: 0x000A9DB5
		protected virtual void dll()
		{
			base.Awake();
			this.BoundingCollider.isTrigger = true;
			this.BoundingCollider.gameObject.layer = LayerMask.NameToLayer("Invisible");
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x04001E7B RID: 7803
		[Header("Grid item data")]
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04001E7C RID: 7804
		public GridItem.EGridType GridType;

		// Token: 0x04001E7E RID: 7806
		public Guid OwnerGridGUID;

		// Token: 0x04001E7F RID: 7807
		public Vector2 OriginCoordinate;

		// Token: 0x04001E80 RID: 7808
		public int Rotation;

		// Token: 0x04001E81 RID: 7809
		public List<CoordinatePair> CoordinatePairs = new List<CoordinatePair>();

		// Token: 0x04001E82 RID: 7810
		private bool dll_Excuted;

		// Token: 0x04001E83 RID: 7811
		private bool dll_Excuted;

		// Token: 0x0200061E RID: 1566
		public enum EGridType
		{
			// Token: 0x04001E85 RID: 7813
			All,
			// Token: 0x04001E86 RID: 7814
			IndoorOnly,
			// Token: 0x04001E87 RID: 7815
			OutdoorOnly
		}
	}
}
