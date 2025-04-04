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
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Misc;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.Trash;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B9A RID: 2970
	public class ChemistryStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060050BA RID: 20666 RVA: 0x0015441C File Offset: 0x0015261C
		public bool isOpen
		{
			get
			{
				return this.PlayerUserObject == Player.Local.NetworkObject;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x00154433 File Offset: 0x00152633
		// (set) Token: 0x060050BC RID: 20668 RVA: 0x0015443B File Offset: 0x0015263B
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x00154444 File Offset: 0x00152644
		// (set) Token: 0x060050BE RID: 20670 RVA: 0x0015444C File Offset: 0x0015264C
		public NetworkObject NPCUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<NPCUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<NPCUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060050BF RID: 20671 RVA: 0x00154456 File Offset: 0x00152656
		// (set) Token: 0x060050C0 RID: 20672 RVA: 0x0015445E File Offset: 0x0015265E
		public NetworkObject PlayerUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x00154468 File Offset: 0x00152668
		// (set) Token: 0x060050C2 RID: 20674 RVA: 0x00154470 File Offset: 0x00152670
		public ChemistryCookOperation CurrentCookOperation { get; set; }

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x060050C4 RID: 20676 RVA: 0x00154479 File Offset: 0x00152679
		// (set) Token: 0x060050C5 RID: 20677 RVA: 0x00154481 File Offset: 0x00152681
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x060050C6 RID: 20678 RVA: 0x0015448A File Offset: 0x0015268A
		// (set) Token: 0x060050C7 RID: 20679 RVA: 0x00154492 File Offset: 0x00152692
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x060050C8 RID: 20680 RVA: 0x0015449B File Offset: 0x0015269B
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x001544A3 File Offset: 0x001526A3
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x060050CA RID: 20682 RVA: 0x001544AB File Offset: 0x001526AB
		public bool Selectable { get; } = 1;

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x001544B3 File Offset: 0x001526B3
		// (set) Token: 0x060050CC RID: 20684 RVA: 0x001544BB File Offset: 0x001526BB
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x001544C4 File Offset: 0x001526C4
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x001544CC File Offset: 0x001526CC
		// (set) Token: 0x060050CF RID: 20687 RVA: 0x001544D4 File Offset: 0x001526D4
		protected ChemistryStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x001544DD File Offset: 0x001526DD
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060050D1 RID: 20689 RVA: 0x000103F8 File Offset: 0x0000E5F8
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.ChemistryStation;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060050D2 RID: 20690 RVA: 0x001544E5 File Offset: 0x001526E5
		// (set) Token: 0x060050D3 RID: 20691 RVA: 0x001544ED File Offset: 0x001526ED
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x001544F6 File Offset: 0x001526F6
		// (set) Token: 0x060050D5 RID: 20693 RVA: 0x001544FE File Offset: 0x001526FE
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

		// Token: 0x060050D6 RID: 20694 RVA: 0x00154508 File Offset: 0x00152708
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0015451E File Offset: 0x0015271E
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060050D8 RID: 20696 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x00154526 File Offset: 0x00152726
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x00154530 File Offset: 0x00152730
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.ChemistryStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x00154550 File Offset: 0x00152750
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			bool initialized = base.Initialized;
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (initialized)
			{
				return;
			}
			if (!this.isGhost)
			{
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
				TimeManager instance3 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
				base.ParentProperty.AddConfigurable(this);
				this.stationConfiguration = new ChemistryStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
			}
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x00154603 File Offset: 0x00152803
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			if (this.CurrentCookOperation != null)
			{
				this.SetCookOperation(connection, this.CurrentCookOperation);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x00154630 File Offset: 0x00152830
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			ChemistryStation.<>c__DisplayClass101_0 CS$<>8__locals1 = new ChemistryStation.<>c__DisplayClass101_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x00154670 File Offset: 0x00152870
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			if (this.CurrentCookOperation != null)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x001546B0 File Offset: 0x001528B0
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x00154748 File Offset: 0x00152948
		protected virtual void MinPass()
		{
			this.Alarm.FlashScreen = false;
			if (this.CurrentCookOperation != null)
			{
				this.CurrentCookOperation.Progress(1);
				base.HasChanged = true;
				float t = Mathf.Clamp01((float)this.CurrentCookOperation.CurrentTime / (float)this.CurrentCookOperation.Recipe.CookTime_Mins);
				this.BoilingFlask.LiquidContainer.SetLiquidColor(Color.Lerp(this.CurrentCookOperation.StartLiquidColor, this.CurrentCookOperation.Recipe.FinalLiquidColor, t), true, true);
				if (InstanceFinder.IsServer && this.CurrentCookOperation.CurrentTime >= this.CurrentCookOperation.Recipe.CookTime_Mins)
				{
					this.FinalizeOperation();
				}
			}
			this.UpdateClock();
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x00154808 File Offset: 0x00152A08
		private void TimeSkipped(int minsSkippped)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < minsSkippped; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x00154830 File Offset: 0x00152A30
		private void UpdateClock()
		{
			if (this.CurrentCookOperation == null)
			{
				this.Alarm.SetScreenLit(false);
				this.Alarm.DisplayText(string.Empty);
				return;
			}
			int num = this.CurrentCookOperation.Recipe.CookTime_Mins - this.CurrentCookOperation.CurrentTime;
			num = Mathf.Max(0, num);
			this.Alarm.DisplayMinutes(num);
			if (this.CurrentCookOperation.CurrentTime >= this.CurrentCookOperation.Recipe.CookTime_Mins)
			{
				this.Alarm.FlashScreen = true;
				this.Burner.SetDialPosition(0f);
				return;
			}
			this.Alarm.SetScreenLit(true);
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001548D9 File Offset: 0x00152AD9
		protected virtual void Update()
		{
			this.StaticFunnel.gameObject.SetActive(!this.LabStand.Funnel.gameObject.activeSelf);
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x00154904 File Offset: 0x00152B04
		public Beaker CreateBeaker()
		{
			Beaker component = UnityEngine.Object.Instantiate<GameObject>(this.BeakerPrefab, this.BeakerAlignmentTransform.position, this.BeakerAlignmentTransform.rotation).GetComponent<Beaker>();
			component.Anchor = this.AnchorRb;
			component.transform.SetParent(this.ItemContainer);
			component.Constraint.Container = this.ItemContainer;
			return component;
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x00154965 File Offset: 0x00152B65
		public StirringRod CreateStirringRod()
		{
			StirringRod component = UnityEngine.Object.Instantiate<GameObject>(this.StirringRodPrefab.gameObject, this.BeakerAlignmentTransform).GetComponent<StirringRod>();
			component.transform.localPosition = Vector3.zero;
			component.transform.localRotation = Quaternion.identity;
			return component;
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x001549A2 File Offset: 0x00152BA2
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCookOperation(ChemistryCookOperation op)
		{
			this.RpcWriter___Server_SendCookOperation_3552222198(op);
			this.RpcLogic___SendCookOperation_3552222198(op);
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001549B8 File Offset: 0x00152BB8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetCookOperation(NetworkConnection conn, ChemistryCookOperation operation)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetCookOperation_1024887225(conn, operation);
				this.RpcLogic___SetCookOperation_1024887225(conn, operation);
			}
			else
			{
				this.RpcWriter___Target_SetCookOperation_1024887225(conn, operation);
			}
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x001549FC File Offset: 0x00152BFC
		[ObserversRpc]
		public void FinalizeOperation()
		{
			this.RpcWriter___Observers_FinalizeOperation_2166136261();
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x00154A10 File Offset: 0x00152C10
		public void ResetStation()
		{
			this.BoilingFlask.SetRecipe(null);
			this.BoilingFlask.ResetContents();
			this.BoilingFlask.SetTemperature(0f);
			this.BoilingFlask.LockTemperature = false;
			this.Burner.SetDialPosition(0f);
			this.Burner.LockDial = false;
			this.LabStand.SetPosition(1f);
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x00154A7C File Offset: 0x00152C7C
		public bool DoesOutputHaveSpace(StationRecipe recipe)
		{
			StorableItemInstance productInstance = recipe.GetProductInstance(this.GetIngredients());
			return this.OutputSlot.GetCapacityForItem(productInstance) >= recipe.Product.Quantity;
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x00154AB4 File Offset: 0x00152CB4
		public List<ItemInstance> GetIngredients()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			foreach (ItemSlot itemSlot in this.IngredientSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					list.Add(itemSlot.ItemInstance);
				}
			}
			return list;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x00154AF8 File Offset: 0x00152CF8
		public bool HasIngredientsForRecipe(StationRecipe recipe)
		{
			List<ItemInstance> ingredients = this.GetIngredients();
			return recipe.DoIngredientsSuffice(ingredients);
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x00154B14 File Offset: 0x00152D14
		public void CreateTrash(List<StationItem> mixerItems)
		{
			for (int i = 0; i < mixerItems.Count; i++)
			{
				if (!(mixerItems[i].TrashPrefab == null))
				{
					Vector3 posiiton = this.TrashSpawnVolume.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-this.TrashSpawnVolume.size.x / 2f, this.TrashSpawnVolume.size.x / 2f), 0f, UnityEngine.Random.Range(-this.TrashSpawnVolume.size.z / 2f, this.TrashSpawnVolume.size.z / 2f)));
					Vector3 vector = this.TrashSpawnVolume.transform.forward;
					vector = Quaternion.Euler(0f, UnityEngine.Random.Range(-45f, 45f), 0f) * vector;
					float d = UnityEngine.Random.Range(0.25f, 0.4f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(mixerItems[i].TrashPrefab.ID, posiiton, UnityEngine.Random.rotation, vector * d, "", false);
				}
			}
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x00154C44 File Offset: 0x00152E44
		public void Hovered()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage("Use " + base.ItemInstance.Name);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x00154C9E File Offset: 0x00152E9E
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x00154CBB File Offset: 0x00152EBB
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.used = true;
			this.Close();
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x00154CE8 File Offset: 0x00152EE8
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition_Default.position, this.CameraPosition_Default.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<ChemistryStationCanvas>.Instance.Open(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x00154D88 File Offset: 0x00152F88
		public void Close()
		{
			Singleton<ChemistryStationCanvas>.Instance.Close(true);
			this.LabStand.SetPosition(1f);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x00154E12 File Offset: 0x00153012
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x00154E28 File Offset: 0x00153028
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x00154E3E File Offset: 0x0015303E
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x00154E64 File Offset: 0x00153064
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x00154EC3 File Offset: 0x001530C3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x00154EE1 File Offset: 0x001530E1
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x00154EFF File Offset: 0x001530FF
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x00154F38 File Offset: 0x00153138
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x00154FB8 File Offset: 0x001531B8
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			if (base.ParentProperty == null)
			{
				Console.LogError(base.gameObject.name + " is not a child of a property!", null);
				return null;
			}
			ChemistryStationUIElement component = UnityEngine.Object.Instantiate<ChemistryStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<ChemistryStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x00155044 File Offset: 0x00153244
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x00155060 File Offset: 0x00153260
		public override string GetSaveString()
		{
			string currentRecipeID = string.Empty;
			EQuality productQuality = EQuality.Standard;
			Color startLiquidColor = Color.clear;
			float liquidLevel = 0f;
			int currentTime = 0;
			if (this.CurrentCookOperation != null)
			{
				currentRecipeID = this.CurrentCookOperation.RecipeID;
				productQuality = this.CurrentCookOperation.ProductQuality;
				startLiquidColor = this.CurrentCookOperation.StartLiquidColor;
				liquidLevel = this.CurrentCookOperation.LiquidLevel;
				currentTime = this.CurrentCookOperation.CurrentTime;
			}
			return new ChemistryStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.IngredientSlots), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), currentRecipeID, productQuality, startLiquidColor, liquidLevel, currentTime).GetJson(true);
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x0015511C File Offset: 0x0015331C
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

		// Token: 0x06005103 RID: 20739 RVA: 0x001551B0 File Offset: 0x001533B0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3552222198));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_SetCookOperation_1024887225));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetCookOperation_1024887225));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_FinalizeOperation_2166136261));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(21U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.ChemistryStation));
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x001553C0 File Offset: 0x001535C0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x001553FA File Offset: 0x001535FA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x00155408 File Offset: 0x00153608
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
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x001554AF File Offset: 0x001536AF
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x001554B8 File Offset: 0x001536B8
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

		// Token: 0x06005109 RID: 20745 RVA: 0x001554F8 File Offset: 0x001536F8
		private void RpcWriter___Server_SendCookOperation_3552222198(ChemistryCookOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(op);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x0015559F File Offset: 0x0015379F
		public void RpcLogic___SendCookOperation_3552222198(ChemistryCookOperation op)
		{
			this.SetCookOperation(null, op);
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x001555AC File Offset: 0x001537AC
		private void RpcReader___Server_SendCookOperation_3552222198(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ChemistryCookOperation op = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3552222198(op);
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001555EC File Offset: 0x001537EC
		private void RpcWriter___Observers_SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(operation);
			base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001556A4 File Offset: 0x001538A4
		public void RpcLogic___SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
		{
			this.CurrentCookOperation = operation;
			this.BoilingFlask.LiquidContainer.SetLiquidLevel(operation.LiquidLevel, false);
			this.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor1 = operation.StartLiquidColor;
			this.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor2 = operation.StartLiquidColor;
			this.BoilingFlask.SetTemperature(operation.Recipe.CookTemperature);
			this.BoilingFlask.LockTemperature = true;
			this.Burner.SetDialPosition(this.CurrentCookOperation.Recipe.CookTemperature / 500f);
			this.Burner.LockDial = true;
			base.HasChanged = true;
			this.UpdateClock();
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x00155764 File Offset: 0x00153964
		private void RpcReader___Observers_SetCookOperation_1024887225(PooledReader PooledReader0, Channel channel)
		{
			ChemistryCookOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_1024887225(null, operation);
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001557A0 File Offset: 0x001539A0
		private void RpcWriter___Target_SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(operation);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x00155858 File Offset: 0x00153A58
		private void RpcReader___Target_SetCookOperation_1024887225(PooledReader PooledReader0, Channel channel)
		{
			ChemistryCookOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_1024887225(base.LocalConnection, operation);
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00155890 File Offset: 0x00153A90
		private void RpcWriter___Observers_FinalizeOperation_2166136261()
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
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x0015593C File Offset: 0x00153B3C
		public void RpcLogic___FinalizeOperation_2166136261()
		{
			if (this.CurrentCookOperation == null)
			{
				Console.LogWarning("No cook operation to finalize", null);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				StorableItemInstance productInstance = this.CurrentCookOperation.Recipe.GetProductInstance(this.CurrentCookOperation.ProductQuality);
				this.OutputSlot.AddItem(productInstance, false);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Chemical_Operations_Completed", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Chemical_Operations_Completed") + 1f).ToString(), true);
			}
			this.CurrentCookOperation = null;
			this.ResetStation();
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001559C8 File Offset: 0x00153BC8
		private void RpcReader___Observers_FinalizeOperation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___FinalizeOperation_2166136261();
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001559E8 File Offset: 0x00153BE8
		private void RpcWriter___Server_SetPlayerUser_3323014238(NetworkObject playerObject)
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
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(13U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00155A8F File Offset: 0x00153C8F
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00155A98 File Offset: 0x00153C98
		private void RpcReader___Server_SetPlayerUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x00155AD8 File Offset: 0x00153CD8
		private void RpcWriter___Server_SetNPCUser_3323014238(NetworkObject npcObject)
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
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x00155B7F File Offset: 0x00153D7F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x00155B88 File Offset: 0x00153D88
		private void RpcReader___Server_SetNPCUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x00155BC8 File Offset: 0x00153DC8
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(15U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x00155C8E File Offset: 0x00153E8E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x00155CB8 File Offset: 0x00153EB8
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x00155D20 File Offset: 0x00153F20
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x00155DE8 File Offset: 0x00153FE8
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x00155E14 File Offset: 0x00154014
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00155E68 File Offset: 0x00154068
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(17U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x00155F30 File Offset: 0x00154130
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x00155F88 File Offset: 0x00154188
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendServerRpc(18U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x00156046 File Offset: 0x00154246
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x00156050 File Offset: 0x00154250
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x001560AC File Offset: 0x001542AC
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendObserversRpc(19U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x00156179 File Offset: 0x00154379
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x00156190 File Offset: 0x00154390
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001561E8 File Offset: 0x001543E8
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(20U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001562C8 File Offset: 0x001544C8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001562F8 File Offset: 0x001544F8
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x00156380 File Offset: 0x00154580
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(21U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00156461 File Offset: 0x00154661
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00156490 File Offset: 0x00154690
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x0015650C File Offset: 0x0015470C
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(22U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001565F0 File Offset: 0x001547F0
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x00156664 File Offset: 0x00154864
		// (set) Token: 0x06005131 RID: 20785 RVA: 0x0015666C File Offset: 0x0015486C
		public NetworkObject SyncAccessor_<NPCUserObject>k__BackingField
		{
			get
			{
				return this.<NPCUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<NPCUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<NPCUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001566A8 File Offset: 0x001548A8
		public virtual bool ChemistryStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(this.syncVar___<CurrentPlayerConfigurer>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value2, Boolean2);
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
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value3 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x00156782 File Offset: 0x00154982
		// (set) Token: 0x06005134 RID: 20788 RVA: 0x0015678A File Offset: 0x0015498A
		public NetworkObject SyncAccessor_<PlayerUserObject>k__BackingField
		{
			get
			{
				return this.<PlayerUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06005135 RID: 20789 RVA: 0x001567C6 File Offset: 0x001549C6
		// (set) Token: 0x06005136 RID: 20790 RVA: 0x001567CE File Offset: 0x001549CE
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

		// Token: 0x06005137 RID: 20791 RVA: 0x0015680C File Offset: 0x00154A0C
		protected virtual void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.IngredientSlots = new ItemSlot[3];
				for (int i = 0; i < 3; i++)
				{
					this.IngredientSlots[i] = new ItemSlot();
					this.IngredientSlots[i].SetSlotOwner(this);
					this.InputVisuals.AddSlot(this.IngredientSlots[i], false);
					ItemSlot itemSlot = this.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(delegate()
					{
						base.HasChanged = true;
					}));
				}
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				ItemSlot outputSlot = this.OutputSlot;
				outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.InputSlots.AddRange(this.IngredientSlots);
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003CB4 RID: 15540
		public const float FOV_OVERRIDE = 65f;

		// Token: 0x04003CB5 RID: 15541
		public const int INPUT_SLOT_COUNT = 3;

		// Token: 0x04003CBA RID: 15546
		public ItemSlot[] IngredientSlots;

		// Token: 0x04003CBB RID: 15547
		public ItemSlot OutputSlot;

		// Token: 0x04003CBC RID: 15548
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04003CBD RID: 15549
		public Transform CameraPosition_Default;

		// Token: 0x04003CBE RID: 15550
		public Transform CameraPosition_Stirring;

		// Token: 0x04003CBF RID: 15551
		public Transform StaticBeaker;

		// Token: 0x04003CC0 RID: 15552
		public Transform StaticFunnel;

		// Token: 0x04003CC1 RID: 15553
		public Transform StaticStirringRod;

		// Token: 0x04003CC2 RID: 15554
		public Transform ItemContainer;

		// Token: 0x04003CC3 RID: 15555
		public LabStand LabStand;

		// Token: 0x04003CC4 RID: 15556
		public StorageVisualizer InputVisuals;

		// Token: 0x04003CC5 RID: 15557
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003CC6 RID: 15558
		public Rigidbody AnchorRb;

		// Token: 0x04003CC7 RID: 15559
		public BunsenBurner Burner;

		// Token: 0x04003CC8 RID: 15560
		public BoilingFlask BoilingFlask;

		// Token: 0x04003CC9 RID: 15561
		public DigitalAlarm Alarm;

		// Token: 0x04003CCA RID: 15562
		public Transform uiPoint;

		// Token: 0x04003CCB RID: 15563
		public Transform[] accessPoints;

		// Token: 0x04003CCC RID: 15564
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003CCD RID: 15565
		public BoxCollider TrashSpawnVolume;

		// Token: 0x04003CCE RID: 15566
		public Transform ExplosionPoint;

		// Token: 0x04003CCF RID: 15567
		[Header("Slot Display Points")]
		public Transform InputSlotsPosition;

		// Token: 0x04003CD0 RID: 15568
		public Transform OutputSlotPosition;

		// Token: 0x04003CD1 RID: 15569
		[Header("Transforms")]
		public Transform[] IngredientTransforms;

		// Token: 0x04003CD2 RID: 15570
		public Transform BeakerAlignmentTransform;

		// Token: 0x04003CD3 RID: 15571
		[Header("Prefabs")]
		public GameObject BeakerPrefab;

		// Token: 0x04003CD4 RID: 15572
		public StirringRod StirringRodPrefab;

		// Token: 0x04003CD5 RID: 15573
		[Header("UI")]
		public ChemistryStationUIElement WorldspaceUIPrefab;

		// Token: 0x04003CD6 RID: 15574
		public Sprite typeIcon;

		// Token: 0x04003CDE RID: 15582
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003CDF RID: 15583
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003CE0 RID: 15584
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003CE1 RID: 15585
		private bool dll_Excuted;

		// Token: 0x04003CE2 RID: 15586
		private bool dll_Excuted;

		// Token: 0x02000B9B RID: 2971
		public enum EStep
		{
			// Token: 0x04003CE4 RID: 15588
			CombineIngredients,
			// Token: 0x04003CE5 RID: 15589
			Stir,
			// Token: 0x04003CE6 RID: 15590
			LowerBoilingFlask,
			// Token: 0x04003CE7 RID: 15591
			PourIntoBoilingFlask,
			// Token: 0x04003CE8 RID: 15592
			RaiseBoilingFlask,
			// Token: 0x04003CE9 RID: 15593
			StartHeat,
			// Token: 0x04003CEA RID: 15594
			Cook,
			// Token: 0x04003CEB RID: 15595
			LowerBoilingFlaskAgain,
			// Token: 0x04003CEC RID: 15596
			PourThroughFilter
		}
	}
}
