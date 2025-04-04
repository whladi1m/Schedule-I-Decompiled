using System;
using System.Collections;
using System.Collections.Generic;
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
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x02000914 RID: 2324
	public class Constructable_GridBased : Constructable
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x0010A470 File Offset: 0x00108670
		public FootprintTile OriginFootprint
		{
			get
			{
				return this.CoordinateFootprintTilePairs[0].footprintTile;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x0010A483 File Offset: 0x00108683
		public int FootprintX
		{
			get
			{
				return this.CoordinateFootprintTilePairs[this.CoordinateFootprintTilePairs.Count - 1].coord.x + 1;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x0010A4A9 File Offset: 0x001086A9
		public int FootprintY
		{
			get
			{
				return this.CoordinateFootprintTilePairs[this.CoordinateFootprintTilePairs.Count - 1].coord.y + 1;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool hasWaterSupply
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x0010A4CF File Offset: 0x001086CF
		public PowerNode PowerNode
		{
			get
			{
				return this.powerNode;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003F26 RID: 16166 RVA: 0x0010A4D7 File Offset: 0x001086D7
		public bool isPowered
		{
			get
			{
				return this.AlwaysPowered || this.powerNode.isConnectedToPower;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x0010A4EE File Offset: 0x001086EE
		// (set) Token: 0x06003F28 RID: 16168 RVA: 0x0010A4F6 File Offset: 0x001086F6
		public Grid OwnerGrid { get; protected set; }

		// Token: 0x06003F29 RID: 16169 RVA: 0x0010A500 File Offset: 0x00108700
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ConstructableScripts.Constructable_GridBased_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0010A51F File Offset: 0x0010871F
		public override void OnStartServer()
		{
			base.OnStartServer();
			Console.Log("On start server", null);
			this.GenerateGridGUIDs();
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0010A538 File Offset: 0x00108738
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			Console.Log("On spawn server", null);
			if (!connection.IsLocalClient)
			{
				Console.Log("Sending thingys", null);
				this.SetGridGUIDs(connection, this.GetGridGUIDs());
			}
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0010A56C File Offset: 0x0010876C
		public override void OnStartNetwork()
		{
			base.OnStartNetwork();
			Console.Log("OnStartNetwork", null);
			this.ReceiveData();
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0010A585 File Offset: 0x00108785
		public virtual void InitializeConstructable_GridBased(Grid grid, Vector2 originCoordinate, float rotation)
		{
			this.SetData(grid.GUID, originCoordinate, rotation);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0010A598 File Offset: 0x00108798
		private void ReceiveData()
		{
			if (base.IsStatic)
			{
				return;
			}
			Console.Log("Constructable received data", null);
			this.OwnerGrid = GUIDManager.GetObject<Grid>(this.SyncAccessor_OwnerGridGUID);
			bool flag = false;
			if (base.NetworkObject.IsSpawned)
			{
				this.SetParent(this.OwnerGrid.Container);
				flag = true;
			}
			List<CoordinatePair> list = Coordinate.BuildCoordinateMatches(new Coordinate(this.SyncAccessor_OriginCoordinate), this.FootprintX, this.FootprintY, this.SyncAccessor_Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.OwnerGrid.GetTile(list[i].coord2) == null)
				{
					string str = "InitializeConstructable_GridBased: grid does not contain tile at ";
					Coordinate coord = list[i].coord2;
					Console.LogError(str + ((coord != null) ? coord.ToString() : null), null);
					this.DestroyConstructable(true);
					return;
				}
			}
			this.ClearPositionData();
			this.coordinatePairs.AddRange(list);
			this.RefreshTransform();
			for (int j = 0; j < this.coordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[j].coord2).AddOccupant(this, this.GetFootprintTile(this.coordinatePairs[j].coord1));
			}
			if (!flag)
			{
				base.StartCoroutine(this.<ReceiveData>g__Routine|36_0());
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0010A6E6 File Offset: 0x001088E6
		private void SetParent(Transform parent)
		{
			base.transform.SetParent(parent);
			this.ContentContainer.SetParent(parent);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0010A700 File Offset: 0x00108900
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			this.RpcWriter___Server_SetData_810381718(gridGUID, originCoordinate, rotation);
			this.RpcLogic___SetData_810381718(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x0010A731 File Offset: 0x00108931
		public virtual void RepositionConstructable(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			this.SetData(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0010A73C File Offset: 0x0010893C
		private void RefreshTransform()
		{
			base.transform.rotation = this.OwnerGrid.transform.rotation * (Quaternion.Inverse(this.buildPoint.transform.rotation) * base.transform.rotation);
			base.transform.Rotate(this.buildPoint.up, this.SyncAccessor_Rotation);
			base.transform.position = this.OwnerGrid.GetTile(this.coordinatePairs[0].coord2).transform.position - (this.OriginFootprint.transform.position - base.transform.position);
			this.ContentContainer.transform.position = base.transform.position;
			this.ContentContainer.transform.rotation = base.transform.rotation;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0010A838 File Offset: 0x00108A38
		private void ClearPositionData()
		{
			for (int i = 0; i < this.coordinatePairs.Count; i++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[i].coord2).RemoveOccupant(this, this.GetFootprintTile(this.coordinatePairs[i].coord1));
			}
			this.coordinatePairs.Clear();
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0010A8A0 File Offset: 0x00108AA0
		public override void DestroyConstructable(bool callOnServer = true)
		{
			Grid[] componentsInChildren = base.gameObject.GetComponentsInChildren<Grid>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DestroyGrid();
			}
			for (int j = 0; j < this.coordinatePairs.Count; j++)
			{
				this.OwnerGrid.GetTile(this.coordinatePairs[j].coord2).RemoveOccupant(this, this.GetFootprintTile(this.coordinatePairs[j].coord1));
			}
			base.DestroyConstructable(callOnServer);
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0010A928 File Offset: 0x00108B28
		private void GenerateGridGUIDs()
		{
			for (int i = 0; i < this.Grids.Length; i++)
			{
				((IGUIDRegisterable)this.Grids[i]).SetGUID(GUIDManager.GenerateUniqueGUID());
				Console.LogError("Generated GRID GUID: " + this.Grids[i].GUID.ToString(), null);
			}
			Console.Log("Sending GRID GUIDs", null);
			this.SetGridGUIDs(null, this.GetGridGUIDs());
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0010A9A0 File Offset: 0x00108BA0
		private string[] GetGridGUIDs()
		{
			string[] array = new string[this.Grids.Length];
			for (int i = 0; i < this.Grids.Length; i++)
			{
				array[i] = this.Grids[i].GUID.ToString();
			}
			return array;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0010A9F0 File Offset: 0x00108BF0
		[ObserversRpc]
		[TargetRpc]
		protected void SetGridGUIDs(NetworkConnection target, string[] guids)
		{
			if (target == null)
			{
				this.RpcWriter___Observers_SetGridGUIDs_2890081366(target, guids);
			}
			else
			{
				this.RpcWriter___Target_SetGridGUIDs_2890081366(target, guids);
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0010AA24 File Offset: 0x00108C24
		public override void SetInvisible()
		{
			base.SetInvisible();
			if (this.PowerNode != null)
			{
				for (int i = 0; i < this.PowerNode.connections.Count; i++)
				{
					this.PowerNode.connections[i].SetVisible(false);
				}
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0010AA78 File Offset: 0x00108C78
		public override void RestoreVisibility()
		{
			base.RestoreVisibility();
			if (this.PowerNode != null)
			{
				for (int i = 0; i < this.PowerNode.connections.Count; i++)
				{
					this.PowerNode.connections[i].SetVisible(true);
				}
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0010AACC File Offset: 0x00108CCC
		public virtual void SetRoofVisible(bool vis)
		{
			if (this.roofVisible == vis)
			{
				return;
			}
			this.roofVisible = vis;
			if (this.roofVisible)
			{
				using (List<GameObject>.Enumerator enumerator = this.roofObjectsForVisibility.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						if (this.originalRoofLayers.ContainsKey(gameObject))
						{
							gameObject.layer = this.originalRoofLayers[gameObject];
						}
						else
						{
							gameObject.layer = LayerMask.NameToLayer("Default");
						}
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.roofObjectsForVisibility)
			{
				if (gameObject2.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					if (this.originalRoofLayers.ContainsKey(gameObject2))
					{
						this.originalRoofLayers[gameObject2] = gameObject2.layer;
					}
					else
					{
						this.originalRoofLayers.Add(gameObject2, gameObject2.layer);
					}
				}
				gameObject2.layer = LayerMask.NameToLayer("Invisible");
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0010AC0C File Offset: 0x00108E0C
		public void CalculateFootprintTileIntersections()
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.CheckIntersections(true);
			}
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0010AC4C File Offset: 0x00108E4C
		public void SetFootprintTileVisiblity(bool visible)
		{
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				this.CoordinateFootprintTilePairs[i].footprintTile.tileAppearance.SetVisible(visible);
			}
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0010AC8C File Offset: 0x00108E8C
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

		// Token: 0x06003F3E RID: 16190 RVA: 0x0010ACDC File Offset: 0x00108EDC
		public List<FootprintTile> GetFootprintTiles()
		{
			List<FootprintTile> list = new List<FootprintTile>();
			for (int i = 0; i < this.CoordinateFootprintTilePairs.Count; i++)
			{
				list.Add(this.CoordinateFootprintTilePairs[i].footprintTile);
			}
			return list;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0010AD58 File Offset: 0x00108F58
		[CompilerGenerated]
		private IEnumerator <ReceiveData>g__Routine|36_0()
		{
			yield return new WaitUntil(() => base.NetworkObject.IsSpawned);
			this.SetParent(this.OwnerGrid.Container);
			yield break;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x0010AD68 File Offset: 0x00108F68
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___Rotation = new SyncVar<float>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.Rotation);
			this.syncVar___OriginCoordinate = new SyncVar<Vector2>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.OriginCoordinate);
			this.syncVar___OwnerGridGUID = new SyncVar<Guid>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.OwnerGridGUID);
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetData_810381718));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetGridGUIDs_2890081366));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetGridGUIDs_2890081366));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ConstructableScripts.Constructable_GridBased));
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x0010AE64 File Offset: 0x00109064
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.Constructable_GridBasedAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___Rotation.SetRegistered();
			this.syncVar___OriginCoordinate.SetRegistered();
			this.syncVar___OwnerGridGUID.SetRegistered();
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x0010AE9E File Offset: 0x0010909E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0010AEAC File Offset: 0x001090AC
		private void RpcWriter___Server_SetData_810381718(Guid gridGUID, Vector2 originCoordinate, float rotation)
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
			writer.WriteGuidAllocated(gridGUID);
			writer.WriteVector2(originCoordinate);
			writer.WriteSingle(rotation, AutoPackType.Unpacked);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0010AF74 File Offset: 0x00109174
		protected virtual void RpcLogic___SetData_810381718(Guid gridGUID, Vector2 originCoordinate, float rotation)
		{
			Console.Log("SetData", null);
			Grid @object = GUIDManager.GetObject<Grid>(gridGUID);
			if (@object == null)
			{
				Console.LogError("InitializeConstructable_GridBased: grid is null", null);
				this.DestroyConstructable(true);
				return;
			}
			this.sync___set_value_OwnerGridGUID(gridGUID, true);
			this.OwnerGrid = @object;
			this.sync___set_value_OriginCoordinate(originCoordinate, true);
			this.sync___set_value_Rotation(rotation, true);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0010AFD0 File Offset: 0x001091D0
		private void RpcReader___Server_SetData_810381718(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Guid gridGUID = PooledReader0.ReadGuid();
			Vector2 originCoordinate = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_810381718(gridGUID, originCoordinate, rotation);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0010B038 File Offset: 0x00109238
		private void RpcWriter___Observers_SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
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
			writer.Write___System.String[]FishNet.Serializing.Generated(guids);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0010B0F0 File Offset: 0x001092F0
		protected void RpcLogic___SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
		{
			Console.Log("Setting GRID GUIDs", null);
			for (int i = 0; i < guids.Length; i++)
			{
				((IGUIDRegisterable)this.Grids[i]).SetGUID(new Guid(guids[i]));
			}
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0010B12C File Offset: 0x0010932C
		private void RpcReader___Observers_SetGridGUIDs_2890081366(PooledReader PooledReader0, Channel channel)
		{
			string[] guids = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGridGUIDs_2890081366(null, guids);
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x0010B160 File Offset: 0x00109360
		private void RpcWriter___Target_SetGridGUIDs_2890081366(NetworkConnection target, string[] guids)
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
			writer.Write___System.String[]FishNet.Serializing.Generated(guids);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, target, false, true);
			writer.Store();
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0010B218 File Offset: 0x00109418
		private void RpcReader___Target_SetGridGUIDs_2890081366(PooledReader PooledReader0, Channel channel)
		{
			string[] guids = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.String[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGridGUIDs_2890081366(base.LocalConnection, guids);
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x0010B24F File Offset: 0x0010944F
		// (set) Token: 0x06003F4E RID: 16206 RVA: 0x0010B257 File Offset: 0x00109457
		public Guid SyncAccessor_OwnerGridGUID
		{
			get
			{
				return this.OwnerGridGUID;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.OwnerGridGUID = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___OwnerGridGUID.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0010B294 File Offset: 0x00109494
		public virtual bool Constructable_GridBased(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_Rotation(this.syncVar___Rotation.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_Rotation(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_OriginCoordinate(this.syncVar___OriginCoordinate.GetValue(true), true);
					return true;
				}
				Vector2 value2 = PooledReader0.ReadVector2();
				this.sync___set_value_OriginCoordinate(value2, Boolean2);
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
					this.sync___set_value_OwnerGridGUID(this.syncVar___OwnerGridGUID.GetValue(true), true);
					return true;
				}
				Guid value3 = PooledReader0.ReadGuid();
				this.sync___set_value_OwnerGridGUID(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003F50 RID: 16208 RVA: 0x0010B373 File Offset: 0x00109573
		// (set) Token: 0x06003F51 RID: 16209 RVA: 0x0010B37B File Offset: 0x0010957B
		public Vector2 SyncAccessor_OriginCoordinate
		{
			get
			{
				return this.OriginCoordinate;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.OriginCoordinate = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___OriginCoordinate.SetValue(value, value);
				}
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x0010B3B7 File Offset: 0x001095B7
		// (set) Token: 0x06003F53 RID: 16211 RVA: 0x0010B3BF File Offset: 0x001095BF
		public float SyncAccessor_Rotation
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

		// Token: 0x06003F54 RID: 16212 RVA: 0x0010B3FC File Offset: 0x001095FC
		protected virtual void dll()
		{
			base.Awake();
			bool isStatic = base.IsStatic;
			if (this.Grids.Length != base.GetComponentsInChildren<Grid>().Length)
			{
				Console.LogWarning(string.Concat(new string[]
				{
					base.gameObject.name,
					": Grids array length does not match number of child grids! (Grids array length: ",
					this.Grids.Length.ToString(),
					", child grids: ",
					base.GetComponentsInChildren<Grid>().Length.ToString(),
					")"
				}), null);
			}
		}

		// Token: 0x04002D88 RID: 11656
		[Header("Grid Based Constructable References")]
		public Transform buildPoint;

		// Token: 0x04002D89 RID: 11657
		public List<CoordinateFootprintTilePair> CoordinateFootprintTilePairs = new List<CoordinateFootprintTilePair>();

		// Token: 0x04002D8A RID: 11658
		public Transform ContentContainer;

		// Token: 0x04002D8B RID: 11659
		public Grid[] Grids;

		// Token: 0x04002D8C RID: 11660
		public List<GameObject> roofObjectsForVisibility = new List<GameObject>();

		// Token: 0x04002D8D RID: 11661
		[Header("Power")]
		[SerializeField]
		protected bool AlwaysPowered;

		// Token: 0x04002D8E RID: 11662
		[SerializeField]
		protected PowerNode powerNode;

		// Token: 0x04002D8F RID: 11663
		[HideInInspector]
		public bool isGhost;

		// Token: 0x04002D90 RID: 11664
		protected bool dataChangedThisFrame;

		// Token: 0x04002D92 RID: 11666
		[SyncVar]
		public Guid OwnerGridGUID;

		// Token: 0x04002D93 RID: 11667
		[SyncVar]
		public Vector2 OriginCoordinate;

		// Token: 0x04002D94 RID: 11668
		[SyncVar]
		public float Rotation;

		// Token: 0x04002D95 RID: 11669
		public List<CoordinatePair> coordinatePairs = new List<CoordinatePair>();

		// Token: 0x04002D96 RID: 11670
		private Dictionary<GameObject, LayerMask> originalRoofLayers = new Dictionary<GameObject, LayerMask>();

		// Token: 0x04002D97 RID: 11671
		protected bool roofVisible = true;

		// Token: 0x04002D98 RID: 11672
		public SyncVar<Guid> syncVar___OwnerGridGUID;

		// Token: 0x04002D99 RID: 11673
		public SyncVar<Vector2> syncVar___OriginCoordinate;

		// Token: 0x04002D9A RID: 11674
		public SyncVar<float> syncVar___Rotation;

		// Token: 0x04002D9B RID: 11675
		private bool dll_Excuted;

		// Token: 0x04002D9C RID: 11676
		private bool dll_Excuted;
	}
}
