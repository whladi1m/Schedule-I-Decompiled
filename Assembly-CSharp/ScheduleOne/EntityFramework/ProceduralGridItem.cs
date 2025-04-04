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
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000624 RID: 1572
	public class ProceduralGridItem : BuildableItem
	{
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600299F RID: 10655 RVA: 0x000ABD4B File Offset: 0x000A9F4B
		public int FootprintXSize
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.x descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.x + 1;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000ABD88 File Offset: 0x000A9F88
		public int FootprintYSize
		{
			get
			{
				return (from c in this.CoordinateFootprintTilePairs
				orderby c.coord.y descending
				select c).FirstOrDefault<CoordinateFootprintTilePair>().coord.y + 1;
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000ABDC5 File Offset: 0x000A9FC5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.ProceduralGridItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000ABDD9 File Offset: 0x000A9FD9
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.Initialized && base.LocallyBuilt)
			{
				base.StartCoroutine(this.<OnStartClient>g__WaitForDataSend|10_0());
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000ABE00 File Offset: 0x000AA000
		protected override void SendInitToClient(NetworkConnection conn)
		{
			this.InitializeProceduralGridItem(conn, base.ItemInstance, this.SyncAccessor_Rotation, this.SyncAccessor_footprintTileMatches, base.GUID.ToString());
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000ABE3A File Offset: 0x000AA03A
		[ServerRpc(RequireOwnership = false)]
		public void SendProceduralGridItemData(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.RpcWriter___Server_SendProceduralGridItemData_638911643(instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000ABE54 File Offset: 0x000AA054
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public virtual void InitializeProceduralGridItem(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
				this.RpcLogic___InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
			}
			else
			{
				this.RpcWriter___Target_InitializeProceduralGridItem_3164718044(conn, instance, _rotation, _footprintTileMatches, GUID);
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000ABEBC File Offset: 0x000AA0BC
		public virtual void InitializeProceduralGridItem(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			if (_footprintTileMatches.Count == 0)
			{
				Console.LogError(base.gameObject.name + " initialized with zero footprint tile matches!", null);
				return;
			}
			this.SetProceduralGridData(_rotation, _footprintTileMatches);
			NetworkObject tileParent = _footprintTileMatches[0].tileParent;
			if (tileParent == null)
			{
				Console.LogError("Base object is null for " + base.gameObject.name, null);
				return;
			}
			Property property = this.GetProperty(tileParent.transform);
			if (property == null)
			{
				Console.LogError("Failed to find property from base " + tileParent.gameObject.name, null);
				return;
			}
			base.InitializeBuildableItem(instance, GUID, property.PropertyCode);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000ABF68 File Offset: 0x000AA168
		protected virtual void SetProceduralGridData(int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches)
		{
			this.sync___set_value_Rotation(_rotation, true);
			this.sync___set_value_footprintTileMatches(_footprintTileMatches, true);
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				_footprintTileMatches[i].tile.AddOccupant(this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[i].coord), this);
			}
			if (base.NetworkObject.IsSpawned)
			{
				base.transform.SetParent(this.SyncAccessor_footprintTileMatches[0].tile.ParentBuildableItem.transform.parent);
				this.RefreshTransform();
				return;
			}
			base.StartCoroutine(this.<SetProceduralGridData>g__Routine|15_0());
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000AC018 File Offset: 0x000AA218
		private void RefreshTransform()
		{
			ProceduralTile tile = this.SyncAccessor_footprintTileMatches[0].tile;
			base.transform.forward = tile.transform.forward;
			base.transform.Rotate(tile.transform.up, (float)this.SyncAccessor_Rotation);
			base.transform.position = tile.transform.position - (this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[0].coord).transform.position - base.transform.position);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000AC0BC File Offset: 0x000AA2BC
		private void ClearPositionData()
		{
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				this.SyncAccessor_footprintTileMatches[i].tile.RemoveOccupant(this.GetFootprintTile(this.SyncAccessor_footprintTileMatches[i].coord), this);
			}
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000AC110 File Offset: 0x000AA310
		public override void DestroyItem(bool callOnServer = true)
		{
			this.ClearPositionData();
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000AC11F File Offset: 0x000AA31F
		protected override Property GetProperty(Transform searchTransform = null)
		{
			if (searchTransform != null && searchTransform.GetComponent<GridItem>() != null)
			{
				return searchTransform.GetComponent<GridItem>().ParentProperty;
			}
			return base.GetProperty(searchTransform);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000AC14C File Offset: 0x000AA34C
		public virtual void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000AC18C File Offset: 0x000AA38C
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000AC1CC File Offset: 0x000AA3CC
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

		// Token: 0x060029AF RID: 10671 RVA: 0x000AC21C File Offset: 0x000AA41C
		public override string GetSaveString()
		{
			FootprintMatchData[] array = new FootprintMatchData[this.SyncAccessor_footprintTileMatches.Count];
			for (int i = 0; i < this.SyncAccessor_footprintTileMatches.Count; i++)
			{
				string tileOwnerGUID = ((IGUIDRegisterable)this.SyncAccessor_footprintTileMatches[i].tileParent.GetComponent<BuildableItem>()).GUID.ToString();
				int tileIndex = this.SyncAccessor_footprintTileMatches[i].tileIndex;
				Vector2 footprintCoordinate = new Vector2((float)this.SyncAccessor_footprintTileMatches[i].coord.x, (float)this.SyncAccessor_footprintTileMatches[i].coord.y);
				array[i] = new FootprintMatchData(tileOwnerGUID, tileIndex, footprintCoordinate);
			}
			return new ProceduralGridItemData(base.GUID, base.ItemInstance, 50, this.SyncAccessor_Rotation, array).GetJson(true);
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000AC312 File Offset: 0x000AA512
		[CompilerGenerated]
		private IEnumerator <OnStartClient>g__WaitForDataSend|10_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SendProceduralGridItemData(base.ItemInstance, this.SyncAccessor_Rotation, this.SyncAccessor_footprintTileMatches, base.GUID.ToString());
			yield break;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000AC321 File Offset: 0x000AA521
		[CompilerGenerated]
		private IEnumerator <SetProceduralGridData>g__Routine|15_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			base.transform.SetParent(this.SyncAccessor_footprintTileMatches[0].tile.ParentBuildableItem.transform.parent);
			this.RefreshTransform();
			yield break;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000AC330 File Offset: 0x000AA530
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___footprintTileMatches = new SyncVar<List<CoordinateProceduralTilePair>>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.footprintTileMatches);
			this.syncVar___Rotation = new SyncVar<int>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.Rotation);
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendProceduralGridItemData_638911643));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_InitializeProceduralGridItem_3164718044));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_InitializeProceduralGridItem_3164718044));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.EntityFramework.ProceduralGridItem));
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000AC401 File Offset: 0x000AA601
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.ProceduralGridItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___footprintTileMatches.SetRegistered();
			this.syncVar___Rotation.SetRegistered();
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000AC430 File Offset: 0x000AA630
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000AC440 File Offset: 0x000AA640
		private void RpcWriter___Server_SendProceduralGridItemData_638911643(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000AC513 File Offset: 0x000AA713
		public void RpcLogic___SendProceduralGridItemData_638911643(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.InitializeProceduralGridItem(null, instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000AC524 File Offset: 0x000AA724
		private void RpcReader___Server_SendProceduralGridItemData_638911643(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			List<CoordinateProceduralTilePair> list = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendProceduralGridItemData_638911643(instance, rotation, list, guid);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000AC590 File Offset: 0x000AA790
		private void RpcWriter___Target_InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000AC671 File Offset: 0x000AA871
		public virtual void RpcLogic___InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			this.InitializeProceduralGridItem(instance, _rotation, _footprintTileMatches, GUID);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000AC680 File Offset: 0x000AA880
		private void RpcReader___Target_InitializeProceduralGridItem_3164718044(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			List<CoordinateProceduralTilePair> list = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InitializeProceduralGridItem_3164718044(base.LocalConnection, instance, rotation, list, guid);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000AC6F0 File Offset: 0x000AA8F0
		private void RpcWriter___Observers_InitializeProceduralGridItem_3164718044(NetworkConnection conn, ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
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
			writer.WriteInt32(_rotation, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generated(_footprintTileMatches);
			writer.WriteString(GUID);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000AC7D4 File Offset: 0x000AA9D4
		private void RpcReader___Observers_InitializeProceduralGridItem_3164718044(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			int rotation = PooledReader0.ReadInt32(AutoPackType.Packed);
			List<CoordinateProceduralTilePair> list = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___InitializeProceduralGridItem_3164718044(null, instance, rotation, list, guid);
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000AC848 File Offset: 0x000AAA48
		// (set) Token: 0x060029C1 RID: 10689 RVA: 0x000AC850 File Offset: 0x000AAA50
		public int SyncAccessor_Rotation
		{
			get
			{
				return this.Rotation;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.Rotation = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___Rotation.SetValue(value, value);
				}
			}
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000AC88C File Offset: 0x000AAA8C
		public virtual bool ProceduralGridItem(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_footprintTileMatches(this.syncVar___footprintTileMatches.GetValue(true), true);
					return true;
				}
				List<CoordinateProceduralTilePair> value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Tiles.CoordinateProceduralTilePair>FishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_footprintTileMatches(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_Rotation(this.syncVar___Rotation.GetValue(true), true);
					return true;
				}
				int value2 = PooledReader0.ReadInt32(AutoPackType.Packed);
				this.sync___set_value_Rotation(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x000AC927 File Offset: 0x000AAB27
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x000AC92F File Offset: 0x000AAB2F
		public List<CoordinateProceduralTilePair> SyncAccessor_footprintTileMatches
		{
			get
			{
				return this.footprintTileMatches;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.footprintTileMatches = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___footprintTileMatches.SetValue(value, value);
				}
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000AC96B File Offset: 0x000AAB6B
		protected virtual void dll()
		{
			base.Awake();
			this.SetFootprintTileVisiblity(false);
		}

		// Token: 0x04001E92 RID: 7826
		[Header("Grid item data")]
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04001E93 RID: 7827
		public ProceduralTile.EProceduralTileType ProceduralTileType;

		// Token: 0x04001E94 RID: 7828
		[SyncVar]
		[HideInInspector]
		public int Rotation;

		// Token: 0x04001E95 RID: 7829
		[SyncVar]
		[HideInInspector]
		public List<CoordinateProceduralTilePair> footprintTileMatches = new List<CoordinateProceduralTilePair>();

		// Token: 0x04001E96 RID: 7830
		public SyncVar<int> syncVar___Rotation;

		// Token: 0x04001E97 RID: 7831
		public SyncVar<List<CoordinateProceduralTilePair>> syncVar___footprintTileMatches;

		// Token: 0x04001E98 RID: 7832
		private bool dll_Excuted;

		// Token: 0x04001E99 RID: 7833
		private bool dll_Excuted;

		// Token: 0x02000625 RID: 1573
		public class FootprintTileMatch
		{
			// Token: 0x04001E9A RID: 7834
			public FootprintTile footprint;

			// Token: 0x04001E9B RID: 7835
			public ProceduralTile matchedTile;
		}
	}
}
