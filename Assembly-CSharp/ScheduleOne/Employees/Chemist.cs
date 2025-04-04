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
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x0200062E RID: 1582
	public class Chemist : Employee, IConfigurable
	{
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000AE40E File Offset: 0x000AC60E
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002A36 RID: 10806 RVA: 0x000AE416 File Offset: 0x000AC616
		// (set) Token: 0x06002A37 RID: 10807 RVA: 0x000AE41E File Offset: 0x000AC61E
		protected ChemistConfiguration configuration { get; set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000AE427 File Offset: 0x000AC627
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000AE42F File Offset: 0x000AC62F
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Chemist;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x000AE432 File Offset: 0x000AC632
		// (set) Token: 0x06002A3B RID: 10811 RVA: 0x000AE43A File Offset: 0x000AC63A
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x000AE443 File Offset: 0x000AC643
		// (set) Token: 0x06002A3D RID: 10813 RVA: 0x000AE44B File Offset: 0x000AC64B
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

		// Token: 0x06002A3E RID: 10814 RVA: 0x000AE455 File Offset: 0x000AC655
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x000AE46B File Offset: 0x000AC66B
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002A41 RID: 10817 RVA: 0x000AE473 File Offset: 0x000AC673
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002A43 RID: 10819 RVA: 0x000AD07F File Offset: 0x000AB27F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000AE47B File Offset: 0x000AC67B
		protected override void AssignProperty(Property prop)
		{
			base.AssignProperty(prop);
			prop.AddConfigurable(this);
			this.configuration = new ChemistConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000AE4A5 File Offset: 0x000AC6A5
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

		// Token: 0x06002A46 RID: 10822 RVA: 0x000AE4E0 File Offset: 0x000AC6E0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000AE4F0 File Offset: 0x000AC6F0
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Chemist.<>c__DisplayClass42_0 CS$<>8__locals1 = new Chemist.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000AE530 File Offset: 0x000AC730
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.AnyWorkInProgress())
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
			if (this.configuration.TotalStations == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any stations", "You can use your management clipboards to assign stations to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				this.TryStartNewTask();
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000AE5A8 File Offset: 0x000AC7A8
		private void TryStartNewTask()
		{
			List<LabOven> labOvensReadyToFinish = this.GetLabOvensReadyToFinish();
			if (labOvensReadyToFinish.Count > 0)
			{
				this.FinishLabOven(labOvensReadyToFinish[0]);
				return;
			}
			List<LabOven> labOvensReadyToStart = this.GetLabOvensReadyToStart();
			if (labOvensReadyToStart.Count > 0)
			{
				this.StartLabOven(labOvensReadyToStart[0]);
				return;
			}
			List<ChemistryStation> chemistryStationsReadyToStart = this.GetChemistryStationsReadyToStart();
			if (chemistryStationsReadyToStart.Count > 0)
			{
				this.StartChemistryStation(chemistryStationsReadyToStart[0]);
				return;
			}
			List<Cauldron> cauldronsReadyToStart = this.GetCauldronsReadyToStart();
			if (cauldronsReadyToStart.Count > 0)
			{
				this.StartCauldron(cauldronsReadyToStart[0]);
				return;
			}
			List<MixingStation> mixingStationsReadyToStart = this.GetMixingStationsReadyToStart();
			if (mixingStationsReadyToStart.Count > 0)
			{
				this.StartMixingStation(mixingStationsReadyToStart[0]);
				return;
			}
			List<LabOven> labOvensReadyToMove = this.GetLabOvensReadyToMove();
			if (labOvensReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((labOvensReadyToMove[0].Configuration as LabOvenConfiguration).DestinationRoute, labOvensReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<ChemistryStation> chemStationsReadyToMove = this.GetChemStationsReadyToMove();
			if (chemStationsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((chemStationsReadyToMove[0].Configuration as ChemistryStationConfiguration).DestinationRoute, chemStationsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<Cauldron> cauldronsReadyToMove = this.GetCauldronsReadyToMove();
			if (cauldronsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((cauldronsReadyToMove[0].Configuration as CauldronConfiguration).DestinationRoute, cauldronsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<MixingStation> mixStationsReadyToMove = this.GetMixStationsReadyToMove();
			if (mixStationsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((mixStationsReadyToMove[0].Configuration as MixingStationConfiguration).DestinationRoute, mixStationsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000AE7BC File Offset: 0x000AC9BC
		private bool AnyWorkInProgress()
		{
			return this.StartChemistryStationBehaviour.Active || this.StartLabOvenBehaviour.Active || this.FinishLabOvenBehaviour.Active || this.MoveItemBehaviour.Active || this.StartMixingStationBehaviour.Active;
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000AE815 File Offset: 0x000ACA15
		protected override bool ShouldIdle()
		{
			return this.configuration.Stations.SelectedObjects.Count == 0 || base.ShouldIdle();
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000AE836 File Offset: 0x000ACA36
		private void StartChemistryStation(ChemistryStation station)
		{
			this.StartChemistryStationBehaviour.SetTargetStation(station);
			this.StartChemistryStationBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000AE850 File Offset: 0x000ACA50
		private void StartCauldron(Cauldron cauldron)
		{
			this.StartCauldronBehaviour.AssignStation(cauldron);
			this.StartCauldronBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000AE86A File Offset: 0x000ACA6A
		private void StartLabOven(LabOven oven)
		{
			this.StartLabOvenBehaviour.SetTargetOven(oven);
			this.StartLabOvenBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000AE884 File Offset: 0x000ACA84
		private void FinishLabOven(LabOven oven)
		{
			this.FinishLabOvenBehaviour.SetTargetOven(oven);
			this.FinishLabOvenBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000AE89E File Offset: 0x000ACA9E
		private void StartMixingStation(MixingStation station)
		{
			this.StartMixingStationBehaviour.AssignStation(station);
			this.StartMixingStationBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000AE8B8 File Offset: 0x000ACAB8
		public override BedItem GetBed()
		{
			return this.configuration.bedItem;
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000AE8C8 File Offset: 0x000ACAC8
		public List<LabOven> GetLabOvensReadyToFinish()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				if (!((IUsable)labOven).IsInUse && labOven.CurrentOperation != null && labOven.IsReadyForHarvest() && labOven.CanOutputSpaceFitCurrentOperation())
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000AE948 File Offset: 0x000ACB48
		public List<LabOven> GetLabOvensReadyToStart()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				if (!((IUsable)labOven).IsInUse && labOven.CurrentOperation == null && labOven.IsReadyToStart())
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000AE9C0 File Offset: 0x000ACBC0
		public List<ChemistryStation> GetChemistryStationsReadyToStart()
		{
			List<ChemistryStation> list = new List<ChemistryStation>();
			foreach (ChemistryStation chemistryStation in this.configuration.ChemStations)
			{
				if (!((IUsable)chemistryStation).IsInUse && chemistryStation.CurrentCookOperation == null)
				{
					StationRecipe selectedRecipe = (chemistryStation.Configuration as ChemistryStationConfiguration).Recipe.SelectedRecipe;
					if (!(selectedRecipe == null) && chemistryStation.HasIngredientsForRecipe(selectedRecipe))
					{
						list.Add(chemistryStation);
					}
				}
			}
			return list;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000AEA58 File Offset: 0x000ACC58
		public List<Cauldron> GetCauldronsReadyToStart()
		{
			List<Cauldron> list = new List<Cauldron>();
			foreach (Cauldron cauldron in this.configuration.Cauldrons)
			{
				if (!((IUsable)cauldron).IsInUse && cauldron.RemainingCookTime <= 0 && cauldron.GetState() == Cauldron.EState.Ready)
				{
					list.Add(cauldron);
				}
			}
			return list;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000AEAD4 File Offset: 0x000ACCD4
		public List<MixingStation> GetMixingStationsReadyToStart()
		{
			List<MixingStation> list = new List<MixingStation>();
			foreach (MixingStation mixingStation in this.configuration.MixStations)
			{
				if (!((IUsable)mixingStation).IsInUse && mixingStation.CanStartMix() && mixingStation.CurrentMixOperation == null && (float)mixingStation.GetMixQuantity() >= (mixingStation.Configuration as MixingStationConfiguration).StartThrehold.Value)
				{
					list.Add(mixingStation);
				}
			}
			return list;
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000AEB6C File Offset: 0x000ACD6C
		protected List<LabOven> GetLabOvensReadyToMove()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				ItemSlot outputSlot = labOven.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((labOven.Configuration as LabOvenConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000AEC04 File Offset: 0x000ACE04
		protected List<ChemistryStation> GetChemStationsReadyToMove()
		{
			List<ChemistryStation> list = new List<ChemistryStation>();
			foreach (ChemistryStation chemistryStation in this.configuration.ChemStations)
			{
				ItemSlot outputSlot = chemistryStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((chemistryStation.Configuration as ChemistryStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(chemistryStation);
				}
			}
			return list;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000AEC9C File Offset: 0x000ACE9C
		protected List<Cauldron> GetCauldronsReadyToMove()
		{
			List<Cauldron> list = new List<Cauldron>();
			foreach (Cauldron cauldron in this.configuration.Cauldrons)
			{
				ItemSlot outputSlot = cauldron.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((cauldron.Configuration as CauldronConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(cauldron);
				}
			}
			return list;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000AED34 File Offset: 0x000ACF34
		protected List<MixingStation> GetMixStationsReadyToMove()
		{
			List<MixingStation> list = new List<MixingStation>();
			foreach (MixingStation mixingStation in this.configuration.MixStations)
			{
				ItemSlot outputSlot = mixingStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((mixingStation.Configuration as MixingStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(mixingStation);
				}
			}
			return list;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000AEDCC File Offset: 0x000ACFCC
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
			ChemistUIElement component = UnityEngine.Object.Instantiate<ChemistUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<ChemistUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000AEE57 File Offset: 0x000AD057
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000AEE74 File Offset: 0x000AD074
		public override string GetSaveString()
		{
			return new ChemistData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData()).GetJson(true);
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000AEEE4 File Offset: 0x000AD0E4
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

		// Token: 0x06002A60 RID: 10848 RVA: 0x000AEF40 File Offset: 0x000AD140
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Chemist));
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000AEFB8 File Offset: 0x000AD1B8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000AEFDC File Offset: 0x000AD1DC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000AEFEC File Offset: 0x000AD1EC
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

		// Token: 0x06002A64 RID: 10852 RVA: 0x000AF093 File Offset: 0x000AD293
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000AF09C File Offset: 0x000AD29C
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

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x000AF0DA File Offset: 0x000AD2DA
		// (set) Token: 0x06002A67 RID: 10855 RVA: 0x000AF0E2 File Offset: 0x000AD2E2
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

		// Token: 0x06002A68 RID: 10856 RVA: 0x000AF120 File Offset: 0x000AD320
		public virtual bool Chemist(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002A69 RID: 10857 RVA: 0x000AF172 File Offset: 0x000AD372
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001ED2 RID: 7890
		public const int MAX_ASSIGNED_STATIONS = 4;

		// Token: 0x04001ED3 RID: 7891
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04001ED4 RID: 7892
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001ED5 RID: 7893
		[Header("Behaviours")]
		public StartChemistryStationBehaviour StartChemistryStationBehaviour;

		// Token: 0x04001ED6 RID: 7894
		public StartLabOvenBehaviour StartLabOvenBehaviour;

		// Token: 0x04001ED7 RID: 7895
		public FinishLabOvenBehaviour FinishLabOvenBehaviour;

		// Token: 0x04001ED8 RID: 7896
		public StartCauldronBehaviour StartCauldronBehaviour;

		// Token: 0x04001ED9 RID: 7897
		public StartMixingStationBehaviour StartMixingStationBehaviour;

		// Token: 0x04001EDA RID: 7898
		[Header("UI")]
		public ChemistUIElement WorldspaceUIPrefab;

		// Token: 0x04001EDB RID: 7899
		public Transform uiPoint;

		// Token: 0x04001EDF RID: 7903
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001EE0 RID: 7904
		private bool dll_Excuted;

		// Token: 0x04001EE1 RID: 7905
		private bool dll_Excuted;
	}
}
