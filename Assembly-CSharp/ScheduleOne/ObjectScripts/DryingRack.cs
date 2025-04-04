using System;
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
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BA1 RID: 2977
	public class DryingRack : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06005161 RID: 20833 RVA: 0x001570F8 File Offset: 0x001552F8
		// (set) Token: 0x06005162 RID: 20834 RVA: 0x00157100 File Offset: 0x00155300
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

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x0015710A File Offset: 0x0015530A
		// (set) Token: 0x06005164 RID: 20836 RVA: 0x00157112 File Offset: 0x00155312
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

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06005165 RID: 20837 RVA: 0x0015711C File Offset: 0x0015531C
		// (set) Token: 0x06005166 RID: 20838 RVA: 0x00157124 File Offset: 0x00155324
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x0015712D File Offset: 0x0015532D
		// (set) Token: 0x06005169 RID: 20841 RVA: 0x00157135 File Offset: 0x00155335
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x0015713E File Offset: 0x0015533E
		// (set) Token: 0x0600516B RID: 20843 RVA: 0x00157146 File Offset: 0x00155346
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600516C RID: 20844 RVA: 0x0015714F File Offset: 0x0015534F
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x0600516D RID: 20845 RVA: 0x00157157 File Offset: 0x00155357
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x0600516E RID: 20846 RVA: 0x0015715F File Offset: 0x0015535F
		public bool Selectable { get; } = 1;

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x00157167 File Offset: 0x00155367
		// (set) Token: 0x06005170 RID: 20848 RVA: 0x0015716F File Offset: 0x0015536F
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x00157178 File Offset: 0x00155378
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x00157180 File Offset: 0x00155380
		// (set) Token: 0x06005173 RID: 20851 RVA: 0x00157188 File Offset: 0x00155388
		protected DryingRackConfiguration stationConfiguration { get; set; }

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06005174 RID: 20852 RVA: 0x00157191 File Offset: 0x00155391
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x00157199 File Offset: 0x00155399
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.DryingRack;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x0015719D File Offset: 0x0015539D
		// (set) Token: 0x06005177 RID: 20855 RVA: 0x001571A5 File Offset: 0x001553A5
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06005178 RID: 20856 RVA: 0x001571AE File Offset: 0x001553AE
		// (set) Token: 0x06005179 RID: 20857 RVA: 0x001571B6 File Offset: 0x001553B6
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

		// Token: 0x0600517A RID: 20858 RVA: 0x001571C0 File Offset: 0x001553C0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x001571D6 File Offset: 0x001553D6
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600517C RID: 20860 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600517D RID: 20861 RVA: 0x0015714F File Offset: 0x0015534F
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600517E RID: 20862 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x001571DE File Offset: 0x001553DE
		// (set) Token: 0x06005180 RID: 20864 RVA: 0x001571E6 File Offset: 0x001553E6
		public ItemSlot InputSlot { get; private set; }

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06005181 RID: 20865 RVA: 0x001571EF File Offset: 0x001553EF
		// (set) Token: 0x06005182 RID: 20866 RVA: 0x001571F7 File Offset: 0x001553F7
		public ItemSlot OutputSlot { get; private set; }

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x00157200 File Offset: 0x00155400
		// (set) Token: 0x06005184 RID: 20868 RVA: 0x00157208 File Offset: 0x00155408
		public bool IsOpen { get; private set; }

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06005185 RID: 20869 RVA: 0x00157211 File Offset: 0x00155411
		// (set) Token: 0x06005186 RID: 20870 RVA: 0x00157219 File Offset: 0x00155419
		public List<DryingOperation> DryingOperations { get; set; } = new List<DryingOperation>();

		// Token: 0x06005187 RID: 20871 RVA: 0x00157224 File Offset: 0x00155424
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.DryingRack_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x00157244 File Offset: 0x00155444
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
				base.ParentProperty.AddConfigurable(this);
				this.stationConfiguration = new DryingRackConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001572D0 File Offset: 0x001554D0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			foreach (DryingOperation op in this.DryingOperations)
			{
				this.PleaseReceiveOp(connection, op);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x0015733C File Offset: 0x0015553C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			DryingRack.<>c__DisplayClass97_0 CS$<>8__locals1 = new DryingRack.<>c__DisplayClass97_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x0015737C File Offset: 0x0015557C
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
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

		// Token: 0x0600518C RID: 20876 RVA: 0x001573A7 File Offset: 0x001555A7
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "In use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0 || this.DryingOperations.Count > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001573E4 File Offset: 0x001555E4
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x00157454 File Offset: 0x00155654
		private void MinPass()
		{
			foreach (DryingOperation dryingOperation in this.DryingOperations.ToArray())
			{
				dryingOperation.Time++;
				if (dryingOperation.Time >= 720)
				{
					if (dryingOperation.StartQuality >= EQuality.Premium)
					{
						if (InstanceFinder.IsServer && this.GetOutputCapacityForOperation(dryingOperation, EQuality.Heavenly) >= dryingOperation.Quantity)
						{
							this.TryEndOperation(this.DryingOperations.IndexOf(dryingOperation), false, EQuality.Heavenly, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
						}
					}
					else
					{
						dryingOperation.IncreaseQuality();
					}
				}
			}
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x001574E5 File Offset: 0x001556E5
		public bool CanStartOperation()
		{
			return this.GetTotalDryingItems() < this.ItemCapacity && this.InputSlot.Quantity != 0 && !this.InputSlot.IsLocked && !this.InputSlot.IsRemovalLocked;
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x00157524 File Offset: 0x00155724
		public void StartOperation()
		{
			int num = Mathf.Min(this.InputSlot.Quantity, this.ItemCapacity - this.GetTotalDryingItems());
			EQuality quality = (this.InputSlot.ItemInstance as QualityItemInstance).Quality;
			DryingOperation op = new DryingOperation(this.InputSlot.ItemInstance.ID, num, quality, 0);
			this.SendOperation(op);
			this.InputSlot.ChangeQuantity(-num, false);
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x00157594 File Offset: 0x00155794
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void TryEndOperation(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
		{
			this.RpcWriter___Server_TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
			this.RpcLogic___TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001575D0 File Offset: 0x001557D0
		public List<DryingOperation> GetOperationsAtTargetQuality()
		{
			EQuality targetQuality = (this.Configuration as DryingRackConfiguration).TargetQuality.Value;
			return (from x in this.DryingOperations
			where x.StartQuality >= targetQuality
			select x).ToList<DryingOperation>();
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x0015761C File Offset: 0x0015581C
		public int GetOutputCapacityForOperation(DryingOperation operation, EQuality quality)
		{
			QualityItemInstance qualityItemInstance = Registry.GetItem(operation.ItemID).GetDefaultInstance(1) as QualityItemInstance;
			qualityItemInstance.SetQuality(quality);
			return this.OutputSlot.GetCapacityForItem(qualityItemInstance);
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x00157653 File Offset: 0x00155853
		[ServerRpc(RequireOwnership = false)]
		private void SendOperation(DryingOperation op)
		{
			this.RpcWriter___Server_SendOperation_1307702229(op);
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x00157660 File Offset: 0x00155860
		[TargetRpc]
		[ObserversRpc]
		private void PleaseReceiveOp(NetworkConnection conn, DryingOperation op)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_PleaseReceiveOp_1575047616(conn, op);
			}
			else
			{
				this.RpcWriter___Target_PleaseReceiveOp_1575047616(conn, op);
			}
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x00157694 File Offset: 0x00155894
		[ObserversRpc(RunLocally = true, ExcludeServer = true)]
		private void RemoveOperation(int opIndex)
		{
			this.RpcWriter___Observers_RemoveOperation_3316948804(opIndex);
			this.RpcLogic___RemoveOperation_3316948804(opIndex);
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001576B8 File Offset: 0x001558B8
		[ObserversRpc]
		private void SetOperationQuantity(int opIndex, int quantity)
		{
			this.RpcWriter___Observers_SetOperationQuantity_1692629761(opIndex, quantity);
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001576D3 File Offset: 0x001558D3
		public int GetTotalDryingItems()
		{
			return this.DryingOperations.Sum((DryingOperation x) => x.Quantity);
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x00157700 File Offset: 0x00155900
		public void RefreshHangingVisuals()
		{
			for (int i = 0; i < this.hangSlots.Length; i++)
			{
				if (this.DryingOperations.Count > i)
				{
					QualityItemInstance qualityItemInstance = this.DryingOperations[i].GetQualityItemInstance();
					this.hangSlots[i].SetStoredItem(qualityItemInstance, false);
				}
				else
				{
					this.hangSlots[i].ClearStoredInstance(false);
				}
			}
			this.HangingVisuals.RefreshVisuals();
			StoredItem[] array = (from x in this.HangingVisuals.ItemContainer.GetComponentsInChildren<StoredItem>()
			where !x.Destroyed
			select x).ToArray<StoredItem>();
			int num = 0;
			while (num < array.Length && num < this.HangAlignments.Length)
			{
				Transform transform = array[num].GetComponentsInChildren<Transform>().FirstOrDefault((Transform x) => x.name == "HangingAlignment");
				if (transform == null)
				{
					Console.LogError("Missing alignment transform on stored item: " + array[num].name, null);
				}
				else
				{
					Transform transform2 = this.HangAlignments[num];
					Quaternion lhs = transform2.rotation * Quaternion.Inverse(transform.rotation);
					array[num].transform.rotation = lhs * array[num].transform.rotation;
					Vector3 b = transform2.position - transform.position;
					array[num].transform.position += b;
				}
				num++;
			}
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x00157888 File Offset: 0x00155A88
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			if (base.ParentProperty == null)
			{
				Property parentProperty = base.ParentProperty;
				Console.LogError(((parentProperty != null) ? parentProperty.ToString() : null) + " is not a child of a property!", null);
				return null;
			}
			DryingRackUIElement component = UnityEngine.Object.Instantiate<DryingRackUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<DryingRackUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x0015791B File Offset: 0x00155B1B
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00157936 File Offset: 0x00155B36
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x0015794C File Offset: 0x00155B4C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x00157964 File Offset: 0x00155B64
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

		// Token: 0x0600519F RID: 20895 RVA: 0x001579BE File Offset: 0x00155BBE
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x001579DC File Offset: 0x00155BDC
		public void Open()
		{
			this.IsOpen = true;
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Transform transform = this.CameraPositions[0];
			if (Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.CameraPositions[1].position) < Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.CameraPositions[0].position))
			{
				transform = this.CameraPositions[1];
			}
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(transform.position, transform.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<DryingRackCanvas>.Instance.SetIsOpen(this, true);
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00157AD0 File Offset: 0x00155CD0
		public void Close()
		{
			this.IsOpen = false;
			Singleton<DryingRackCanvas>.Instance.SetIsOpen(null, false);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x00157B52 File Offset: 0x00155D52
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x00157B78 File Offset: 0x00155D78
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

		// Token: 0x060051A4 RID: 20900 RVA: 0x00157BD7 File Offset: 0x00155DD7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00157BF5 File Offset: 0x00155DF5
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00157C13 File Offset: 0x00155E13
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00157C4C File Offset: 0x00155E4C
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

		// Token: 0x060051A8 RID: 20904 RVA: 0x00157CCC File Offset: 0x00155ECC
		public override string GetSaveString()
		{
			return new DryingRackData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.InputSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.DryingOperations.ToArray()).GetJson(true);
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x00157D3C File Offset: 0x00155F3C
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

		// Token: 0x060051AB RID: 20907 RVA: 0x00157DF0 File Offset: 0x00155FF0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_TryEndOperation_4146970406));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendOperation_1307702229));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_PleaseReceiveOp_1575047616));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_PleaseReceiveOp_1575047616));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_RemoveOperation_3316948804));
			base.RegisterObserversRpc(14U, new ClientRpcDelegate(this.RpcReader___Observers_SetOperationQuantity_1692629761));
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(19U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(24U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.DryingRack));
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x0015802E File Offset: 0x0015622E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x00158068 File Offset: 0x00156268
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x00158078 File Offset: 0x00156278
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

		// Token: 0x060051AF RID: 20911 RVA: 0x0015811F File Offset: 0x0015631F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00158128 File Offset: 0x00156328
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

		// Token: 0x060051B1 RID: 20913 RVA: 0x00158168 File Offset: 0x00156368
		private void RpcWriter___Server_TryEndOperation_4146970406(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
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
			writer.WriteInt32(operationIndex, AutoPackType.Packed);
			writer.WriteBoolean(allowSplitting);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			writer.WriteInt32(requestID, AutoPackType.Packed);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x00158240 File Offset: 0x00156440
		public void RpcLogic___TryEndOperation_4146970406(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
		{
			if (this.requestIDs.Contains(requestID))
			{
				return;
			}
			this.requestIDs.Add(requestID);
			if (operationIndex >= this.DryingOperations.Count)
			{
				Console.LogError("Invalid operation index: " + operationIndex.ToString(), null);
				return;
			}
			DryingOperation dryingOperation = this.DryingOperations[operationIndex];
			int outputCapacityForOperation = this.GetOutputCapacityForOperation(dryingOperation, quality);
			int num = Mathf.Min(dryingOperation.Quantity, outputCapacityForOperation);
			if (num == 0)
			{
				Console.LogWarning("No space in output slot for operation: " + operationIndex.ToString(), null);
				return;
			}
			if (!allowSplitting && num < dryingOperation.Quantity)
			{
				Console.LogWarning("Operation would be split, but splitting is not allowed", null);
				return;
			}
			QualityItemInstance qualityItemInstance = Registry.GetItem(dryingOperation.ItemID).GetDefaultInstance(num) as QualityItemInstance;
			qualityItemInstance.SetQuality(quality);
			this.OutputSlot.InsertItem(qualityItemInstance);
			if (num == dryingOperation.Quantity)
			{
				this.RemoveOperation(this.DryingOperations.IndexOf(dryingOperation));
				return;
			}
			this.SetOperationQuantity(this.DryingOperations.IndexOf(dryingOperation), dryingOperation.Quantity - num);
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x00158348 File Offset: 0x00156548
		private void RpcReader___Server_TryEndOperation_4146970406(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int operationIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool allowSplitting = PooledReader0.ReadBoolean();
			EQuality quality = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			int requestID = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001583C4 File Offset: 0x001565C4
		private void RpcWriter___Server_SendOperation_1307702229(DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x0015846B File Offset: 0x0015666B
		private void RpcLogic___SendOperation_1307702229(DryingOperation op)
		{
			this.PleaseReceiveOp(null, op);
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00158478 File Offset: 0x00156678
		private void RpcReader___Server_SendOperation_1307702229(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			DryingOperation op = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendOperation_1307702229(op);
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001584AC File Offset: 0x001566AC
		private void RpcWriter___Target_PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x00158564 File Offset: 0x00156764
		private void RpcLogic___PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
		{
			if (op.Quantity == 0)
			{
				Console.LogWarning("Operation quantity is 0. Ignoring", null);
				return;
			}
			this.DryingOperations.Add(op);
			if (this.onOperationStart != null)
			{
				this.onOperationStart(op);
			}
			if (this.onOperationsChanged != null)
			{
				this.onOperationsChanged();
			}
			this.RefreshHangingVisuals();
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x001585C0 File Offset: 0x001567C0
		private void RpcReader___Target_PleaseReceiveOp_1575047616(PooledReader PooledReader0, Channel channel)
		{
			DryingOperation op = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PleaseReceiveOp_1575047616(base.LocalConnection, op);
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001585F8 File Offset: 0x001567F8
		private void RpcWriter___Observers_PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001586B0 File Offset: 0x001568B0
		private void RpcReader___Observers_PleaseReceiveOp_1575047616(PooledReader PooledReader0, Channel channel)
		{
			DryingOperation op = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PleaseReceiveOp_1575047616(null, op);
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001586E4 File Offset: 0x001568E4
		private void RpcWriter___Observers_RemoveOperation_3316948804(int opIndex)
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
			writer.WriteInt32(opIndex, AutoPackType.Packed);
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, true, false);
			writer.Store();
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x001587A0 File Offset: 0x001569A0
		private void RpcLogic___RemoveOperation_3316948804(int opIndex)
		{
			if (opIndex < this.DryingOperations.Count)
			{
				DryingOperation dryingOperation = this.DryingOperations[opIndex];
				this.DryingOperations.Remove(dryingOperation);
				if (this.onOperationComplete != null)
				{
					this.onOperationComplete(dryingOperation);
				}
				if (this.onOperationsChanged != null)
				{
					this.onOperationsChanged();
				}
				this.RefreshHangingVisuals();
				return;
			}
			Console.LogError("Invalid operation index: " + opIndex.ToString(), null);
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x0015881C File Offset: 0x00156A1C
		private void RpcReader___Observers_RemoveOperation_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int opIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemoveOperation_3316948804(opIndex);
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x0015885C File Offset: 0x00156A5C
		private void RpcWriter___Observers_SetOperationQuantity_1692629761(int opIndex, int quantity)
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
			writer.WriteInt32(opIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendObserversRpc(14U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x0015892C File Offset: 0x00156B2C
		private void RpcLogic___SetOperationQuantity_1692629761(int opIndex, int quantity)
		{
			if (opIndex < this.DryingOperations.Count)
			{
				this.DryingOperations[opIndex].Quantity = quantity;
				if (this.onOperationsChanged != null)
				{
					this.onOperationsChanged();
				}
				this.RefreshHangingVisuals();
				return;
			}
			Console.LogError("Invalid operation index: " + opIndex.ToString(), null);
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x0015898C File Offset: 0x00156B8C
		private void RpcReader___Observers_SetOperationQuantity_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int opIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetOperationQuantity_1692629761(opIndex, quantity);
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x001589D8 File Offset: 0x00156BD8
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
			base.SendServerRpc(15U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00158A7F File Offset: 0x00156C7F
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x00158A88 File Offset: 0x00156C88
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

		// Token: 0x060051C5 RID: 20933 RVA: 0x00158AC8 File Offset: 0x00156CC8
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
			base.SendServerRpc(16U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00158B6F File Offset: 0x00156D6F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00158B78 File Offset: 0x00156D78
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

		// Token: 0x060051C8 RID: 20936 RVA: 0x00158BB8 File Offset: 0x00156DB8
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
			base.SendServerRpc(17U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x00158C7E File Offset: 0x00156E7E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00158CA8 File Offset: 0x00156EA8
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

		// Token: 0x060051CB RID: 20939 RVA: 0x00158D10 File Offset: 0x00156F10
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00158DD8 File Offset: 0x00156FD8
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00158E04 File Offset: 0x00157004
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

		// Token: 0x060051CE RID: 20942 RVA: 0x00158E58 File Offset: 0x00157058
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
			base.SendTargetRpc(19U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00158F20 File Offset: 0x00157120
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

		// Token: 0x060051D0 RID: 20944 RVA: 0x00158F78 File Offset: 0x00157178
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
			base.SendServerRpc(20U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00159036 File Offset: 0x00157236
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00159040 File Offset: 0x00157240
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

		// Token: 0x060051D3 RID: 20947 RVA: 0x0015909C File Offset: 0x0015729C
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
			base.SendObserversRpc(21U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x00159169 File Offset: 0x00157369
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x00159180 File Offset: 0x00157380
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

		// Token: 0x060051D6 RID: 20950 RVA: 0x001591D8 File Offset: 0x001573D8
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
			base.SendServerRpc(22U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001592B8 File Offset: 0x001574B8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x001592E8 File Offset: 0x001574E8
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

		// Token: 0x060051D9 RID: 20953 RVA: 0x00159370 File Offset: 0x00157570
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
			base.SendTargetRpc(23U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x00159451 File Offset: 0x00157651
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00159480 File Offset: 0x00157680
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

		// Token: 0x060051DC RID: 20956 RVA: 0x001594FC File Offset: 0x001576FC
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
			base.SendObserversRpc(24U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001595E0 File Offset: 0x001577E0
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

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x00159654 File Offset: 0x00157854
		// (set) Token: 0x060051DF RID: 20959 RVA: 0x0015965C File Offset: 0x0015785C
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

		// Token: 0x060051E0 RID: 20960 RVA: 0x00159698 File Offset: 0x00157898
		public virtual bool DryingRack(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060051E1 RID: 20961 RVA: 0x00159772 File Offset: 0x00157972
		// (set) Token: 0x060051E2 RID: 20962 RVA: 0x0015977A File Offset: 0x0015797A
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

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x001597B6 File Offset: 0x001579B6
		// (set) Token: 0x060051E4 RID: 20964 RVA: 0x001597BE File Offset: 0x001579BE
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

		// Token: 0x060051E5 RID: 20965 RVA: 0x001597FC File Offset: 0x001579FC
		protected virtual void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.InputSlot = new ItemSlot();
				this.InputSlot.SetSlotOwner(this);
				this.InputSlot.AddFilter(new ItemFilter_Dryable());
				this.InputVisuals.AddSlot(this.InputSlot, false);
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.Add(this.InputSlot);
				this.OutputSlots.Add(this.OutputSlot);
				this.HangingVisuals.BlockRefreshes = true;
				this.hangSlots = new ItemSlot[this.HangAlignments.Length];
				for (int i = 0; i < this.HangAlignments.Length; i++)
				{
					this.hangSlots[i] = new ItemSlot();
					this.HangingVisuals.AddSlot(this.hangSlots[i], false);
				}
			}
		}

		// Token: 0x04003D14 RID: 15636
		public const int DRY_MINS_PER_TIER = 720;

		// Token: 0x04003D15 RID: 15637
		[Header("Settings")]
		public int ItemCapacity = 20;

		// Token: 0x04003D16 RID: 15638
		[Header("References")]
		public Transform[] CameraPositions;

		// Token: 0x04003D17 RID: 15639
		public InteractableObject IntObj;

		// Token: 0x04003D18 RID: 15640
		public Transform uiPoint;

		// Token: 0x04003D19 RID: 15641
		public Transform[] accessPoints;

		// Token: 0x04003D1A RID: 15642
		public StorageVisualizer InputVisuals;

		// Token: 0x04003D1B RID: 15643
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003D1C RID: 15644
		public StorageVisualizer HangingVisuals;

		// Token: 0x04003D1D RID: 15645
		public Transform[] HangAlignments;

		// Token: 0x04003D1E RID: 15646
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003D1F RID: 15647
		[Header("UI")]
		public DryingRackUIElement WorldspaceUIPrefab;

		// Token: 0x04003D20 RID: 15648
		public Sprite typeIcon;

		// Token: 0x04003D2F RID: 15663
		public Action<DryingOperation> onOperationStart;

		// Token: 0x04003D30 RID: 15664
		public Action<DryingOperation> onOperationComplete;

		// Token: 0x04003D31 RID: 15665
		public Action onOperationsChanged;

		// Token: 0x04003D32 RID: 15666
		private ItemSlot[] hangSlots;

		// Token: 0x04003D33 RID: 15667
		private List<int> requestIDs = new List<int>();

		// Token: 0x04003D34 RID: 15668
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003D35 RID: 15669
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003D36 RID: 15670
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003D37 RID: 15671
		private bool dll_Excuted;

		// Token: 0x04003D38 RID: 15672
		private bool dll_Excuted;
	}
}
