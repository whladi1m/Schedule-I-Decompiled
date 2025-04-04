using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x0200063B RID: 1595
	public class Packager : Employee, IConfigurable
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000B1690 File Offset: 0x000AF890
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x000B1698 File Offset: 0x000AF898
		// (set) Token: 0x06002B10 RID: 11024 RVA: 0x000B16A0 File Offset: 0x000AF8A0
		protected PackagerConfiguration configuration { get; set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000B16A9 File Offset: 0x000AF8A9
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002B12 RID: 11026 RVA: 0x000B16B1 File Offset: 0x000AF8B1
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Packager;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000B16B4 File Offset: 0x000AF8B4
		// (set) Token: 0x06002B14 RID: 11028 RVA: 0x000B16BC File Offset: 0x000AF8BC
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000B16C5 File Offset: 0x000AF8C5
		// (set) Token: 0x06002B16 RID: 11030 RVA: 0x000B16CD File Offset: 0x000AF8CD
		public NetworkObject CurrentPlayerConfigurer
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentPlayerConfigurer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, true);
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000B16D7 File Offset: 0x000AF8D7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x000B16ED File Offset: 0x000AF8ED
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002B19 RID: 11033 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000B16F5 File Offset: 0x000AF8F5
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000AD07F File Offset: 0x000AB27F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000B16FD File Offset: 0x000AF8FD
		protected override void AssignProperty(Property prop)
		{
			base.AssignProperty(prop);
			prop.AddConfigurable(this);
			this.configuration = new PackagerConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000B1727 File Offset: 0x000AF927
		protected override void Fire()
		{
			if (this.configuration != null)
			{
				this.configuration.Destroy();
				this.DestroyWorldspaceUI();
				if (base.AssignedProperty != null)
				{
					base.AssignedProperty.RemoveConfigurable(this);
				}
			}
			base.Fire();
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000B1762 File Offset: 0x000AF962
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000B1774 File Offset: 0x000AF974
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Packager.<>c__DisplayClass40_0 CS$<>8__locals1 = new Packager.<>c__DisplayClass40_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000B17B4 File Offset: 0x000AF9B4
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (this.PackagingBehaviour.Active)
			{
				base.MarkIsWorking();
				return;
			}
			if (this.MoveItemBehaviour.Active)
			{
				base.MarkIsWorking();
				return;
			}
			if (base.Fired)
			{
				base.LeavePropertyAndDespawn();
				return;
			}
			if (!base.CanWork())
			{
				return;
			}
			if (this.configuration.AssignedStationCount + this.configuration.Routes.Routes.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned to any stations or routes.", "You can use your management clipboards to assign stations or routes to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			PackagingStation stationToAttend = this.GetStationToAttend();
			if (stationToAttend != null)
			{
				this.StartPackaging(stationToAttend);
				return;
			}
			BrickPress brickPress = this.GetBrickPress();
			if (brickPress != null)
			{
				this.StartPress(brickPress);
				return;
			}
			PackagingStation stationMoveItems = this.GetStationMoveItems();
			if (stationMoveItems != null)
			{
				this.StartMoveItem(stationMoveItems);
				return;
			}
			BrickPress brickPressMoveItems = this.GetBrickPressMoveItems();
			if (brickPressMoveItems != null)
			{
				this.StartMoveItem(brickPressMoveItems);
				return;
			}
			ItemInstance itemInstance;
			AdvancedTransitRoute transitRouteReady = this.GetTransitRouteReady(out itemInstance);
			if (transitRouteReady != null)
			{
				this.MoveItemBehaviour.Initialize(transitRouteReady, itemInstance, itemInstance.Quantity, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", "I need one of my assigned stations to have enough product and packaging to get to work.", 0);
			this.SetIdle(true);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000B18F5 File Offset: 0x000AFAF5
		private void StartPackaging(PackagingStation station)
		{
			Console.Log("Starting packaging at " + station.gameObject.name, null);
			this.PackagingBehaviour.AssignStation(station);
			this.PackagingBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000B192A File Offset: 0x000AFB2A
		private void StartPress(BrickPress press)
		{
			this.BrickPressBehaviour.AssignStation(press);
			this.BrickPressBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000B1944 File Offset: 0x000AFB44
		private void StartMoveItem(PackagingStation station)
		{
			Console.Log("Starting moving items from " + station.gameObject.name, null);
			this.MoveItemBehaviour.Initialize((station.Configuration as PackagingStationConfiguration).DestinationRoute, station.OutputSlot.ItemInstance, -1, false);
			this.MoveItemBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000B19A0 File Offset: 0x000AFBA0
		private void StartMoveItem(BrickPress press)
		{
			this.MoveItemBehaviour.Initialize((press.Configuration as BrickPressConfiguration).DestinationRoute, press.OutputSlot.ItemInstance, -1, false);
			this.MoveItemBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000B19D8 File Offset: 0x000AFBD8
		protected PackagingStation GetStationToAttend()
		{
			foreach (PackagingStation packagingStation in this.configuration.AssignedStations)
			{
				if (this.PackagingBehaviour.IsStationReady(packagingStation))
				{
					return packagingStation;
				}
			}
			return null;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000B1A40 File Offset: 0x000AFC40
		protected BrickPress GetBrickPress()
		{
			foreach (BrickPress brickPress in this.configuration.AssignedBrickPresses)
			{
				if (this.BrickPressBehaviour.IsStationReady(brickPress))
				{
					return brickPress;
				}
			}
			return null;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000B1AA8 File Offset: 0x000AFCA8
		protected PackagingStation GetStationMoveItems()
		{
			foreach (PackagingStation packagingStation in this.configuration.AssignedStations)
			{
				ItemSlot outputSlot = packagingStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((packagingStation.Configuration as PackagingStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					return packagingStation;
				}
			}
			return null;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000B1B38 File Offset: 0x000AFD38
		protected BrickPress GetBrickPressMoveItems()
		{
			foreach (BrickPress brickPress in this.configuration.AssignedBrickPresses)
			{
				ItemSlot outputSlot = brickPress.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((brickPress.Configuration as BrickPressConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					return brickPress;
				}
			}
			return null;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000B1BC8 File Offset: 0x000AFDC8
		protected AdvancedTransitRoute GetTransitRouteReady(out ItemInstance item)
		{
			item = null;
			foreach (AdvancedTransitRoute advancedTransitRoute in this.configuration.Routes.Routes)
			{
				item = advancedTransitRoute.GetItemReadyToMove();
				if (item != null && this.movement.CanGetTo(advancedTransitRoute.Source, 1f) && this.movement.CanGetTo(advancedTransitRoute.Destination, 1f))
				{
					return advancedTransitRoute;
				}
			}
			return null;
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000B1C64 File Offset: 0x000AFE64
		protected override bool ShouldIdle()
		{
			return this.configuration.AssignedStationCount == 0 || base.ShouldIdle();
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000B1C7B File Offset: 0x000AFE7B
		public override BedItem GetBed()
		{
			return this.configuration.bedItem;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000B1C88 File Offset: 0x000AFE88
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			Property assignedProperty = base.AssignedProperty;
			if (assignedProperty == null)
			{
				Property property = assignedProperty;
				Console.LogError(((property != null) ? property.ToString() : null) + " is not a child of a property!", null);
				return null;
			}
			PackagerUIElement component = UnityEngine.Object.Instantiate<PackagerUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<PackagerUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000B1D13 File Offset: 0x000AFF13
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000B1D30 File Offset: 0x000AFF30
		public override string GetSaveString()
		{
			return new PackagerData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData()).GetJson(true);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000B1DA0 File Offset: 0x000AFFA0
		public override List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			if (this.Configuration.ShouldSave())
			{
				list.Add("Configuration.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Configuration", this.Configuration.GetSaveString());
			}
			list.AddRange(base.WriteData(parentFolderPath));
			return list;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000B1E0C File Offset: 0x000B000C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Packager));
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000B1E84 File Offset: 0x000B0084
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000B1EA8 File Offset: 0x000B00A8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000B1EB8 File Offset: 0x000B00B8
		private void RpcWriter___Server_SetConfigurer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(40U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000B1F5F File Offset: 0x000B015F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000B1F68 File Offset: 0x000B0168
		private void RpcReader___Server_SetConfigurer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x000B1FA6 File Offset: 0x000B01A6
		// (set) Token: 0x06002B39 RID: 11065 RVA: 0x000B1FAE File Offset: 0x000B01AE
		public NetworkObject SyncAccessor_<CurrentPlayerConfigurer>k__BackingField
		{
			get
			{
				return this.<CurrentPlayerConfigurer>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentPlayerConfigurer>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000B1FEC File Offset: 0x000B01EC
		public virtual bool Packager(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 2U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(this.syncVar___<CurrentPlayerConfigurer>k__BackingField.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000B203E File Offset: 0x000B023E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F33 RID: 7987
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04001F34 RID: 7988
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001F35 RID: 7989
		public PackagingStationBehaviour PackagingBehaviour;

		// Token: 0x04001F36 RID: 7990
		public BrickPressBehaviour BrickPressBehaviour;

		// Token: 0x04001F37 RID: 7991
		[Header("UI")]
		public PackagerUIElement WorldspaceUIPrefab;

		// Token: 0x04001F38 RID: 7992
		public Transform uiPoint;

		// Token: 0x04001F39 RID: 7993
		[Header("Settings")]
		public int MaxAssignedStations = 3;

		// Token: 0x04001F3A RID: 7994
		[Header("Proficiency Settings")]
		public float PackagingSpeedMultiplier = 1f;

		// Token: 0x04001F3E RID: 7998
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001F3F RID: 7999
		private bool dll_Excuted;

		// Token: 0x04001F40 RID: 8000
		private bool dll_Excuted;
	}
}
