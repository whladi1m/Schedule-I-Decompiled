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
using ScheduleOne.Audio;
using ScheduleOne.Decoration;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB8 RID: 3000
	public class PackagingStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x00160DF6 File Offset: 0x0015EFF6
		// (set) Token: 0x060053BA RID: 21434 RVA: 0x00160DFE File Offset: 0x0015EFFE
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x00160E07 File Offset: 0x0015F007
		// (set) Token: 0x060053BC RID: 21436 RVA: 0x00160E0F File Offset: 0x0015F00F
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

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x060053BD RID: 21437 RVA: 0x00160E19 File Offset: 0x0015F019
		// (set) Token: 0x060053BE RID: 21438 RVA: 0x00160E21 File Offset: 0x0015F021
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

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x060053BF RID: 21439 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x00160E2B File Offset: 0x0015F02B
		// (set) Token: 0x060053C1 RID: 21441 RVA: 0x00160E33 File Offset: 0x0015F033
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x00160E3C File Offset: 0x0015F03C
		// (set) Token: 0x060053C3 RID: 21443 RVA: 0x00160E44 File Offset: 0x0015F044
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x060053C4 RID: 21444 RVA: 0x00160E4D File Offset: 0x0015F04D
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x060053C5 RID: 21445 RVA: 0x00160E55 File Offset: 0x0015F055
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060053C6 RID: 21446 RVA: 0x00160E5D File Offset: 0x0015F05D
		public bool Selectable { get; } = 1;

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x00160E65 File Offset: 0x0015F065
		// (set) Token: 0x060053C8 RID: 21448 RVA: 0x00160E6D File Offset: 0x0015F06D
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x00160E76 File Offset: 0x0015F076
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x00160E7E File Offset: 0x0015F07E
		// (set) Token: 0x060053CB RID: 21451 RVA: 0x00160E86 File Offset: 0x0015F086
		protected PackagingStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x00160E8F File Offset: 0x0015F08F
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x060053CD RID: 21453 RVA: 0x000022C9 File Offset: 0x000004C9
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.PackagingStation;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x00160E97 File Offset: 0x0015F097
		// (set) Token: 0x060053CF RID: 21455 RVA: 0x00160E9F File Offset: 0x0015F09F
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x00160EA8 File Offset: 0x0015F0A8
		// (set) Token: 0x060053D1 RID: 21457 RVA: 0x00160EB0 File Offset: 0x0015F0B0
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

		// Token: 0x060053D2 RID: 21458 RVA: 0x00160EBA File Offset: 0x0015F0BA
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x00160ED0 File Offset: 0x0015F0D0
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x060053D5 RID: 21461 RVA: 0x00160ED8 File Offset: 0x0015F0D8
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x00160EE0 File Offset: 0x0015F0E0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.PackagingStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x00160F00 File Offset: 0x0015F100
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
				this.stationConfiguration = new PackagingStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x00160F63 File Offset: 0x0015F163
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x00160F7C File Offset: 0x0015F17C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			PackagingStation.<>c__DisplayClass103_0 CS$<>8__locals1 = new PackagingStation.<>c__DisplayClass103_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x00160FBC File Offset: 0x0015F1BC
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!Singleton<PackagingStationCanvas>.Instance.isOpen)
			{
				return;
			}
			if (Singleton<PackagingStationCanvas>.Instance.PackagingStation != this)
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

		// Token: 0x060053DC RID: 21468 RVA: 0x00161009 File Offset: 0x0015F209
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x00161035 File Offset: 0x0015F235
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x00161074 File Offset: 0x0015F274
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x00161095 File Offset: 0x0015F295
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x001610AC File Offset: 0x0015F2AC
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

		// Token: 0x060053E1 RID: 21473 RVA: 0x00161106 File Offset: 0x0015F306
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x00161124 File Offset: 0x0015F324
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<PackagingStationCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x001611C4 File Offset: 0x0015F3C4
		public void Close()
		{
			if (Singleton<PackagingStationCanvas>.InstanceExists)
			{
				Singleton<PackagingStationCanvas>.Instance.SetIsOpen(null, false, true);
			}
			this.SetPlayerUser(null);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			}
			if (Singleton<CompassManager>.InstanceExists)
			{
				Singleton<CompassManager>.Instance.SetVisible(true);
			}
			if (PlayerSingleton<PlayerInventory>.InstanceExists)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			}
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x00161264 File Offset: 0x0015F464
		public PackagingStation.EState GetState(PackagingStation.EMode mode)
		{
			if (mode == PackagingStation.EMode.Package)
			{
				if (this.PackagingSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				if (this.ProductSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				if (this.OutputSlot.IsAtCapacity)
				{
					return PackagingStation.EState.OutputSlotFull;
				}
				if (this.OutputSlot.Quantity > 0 && this.OutputSlot.ItemInstance.ID != this.ProductSlot.ItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.Quantity > 0 && (this.OutputSlot.ItemInstance as ProductItemInstance).AppliedPackaging.ID != this.PackagingSlot.ItemInstance.Definition.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.Quantity > 0 && (this.OutputSlot.ItemInstance as ProductItemInstance).Quality != (this.ProductSlot.ItemInstance as ProductItemInstance).Quality)
				{
					return PackagingStation.EState.Mismatch;
				}
				int quantity = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
				if (this.ProductSlot.Quantity < quantity)
				{
					return PackagingStation.EState.InsufficentProduct;
				}
			}
			else if (mode == PackagingStation.EMode.Unpackage)
			{
				if (this.OutputSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				ProductItemInstance productItemInstance = this.OutputSlot.ItemInstance.GetCopy(1) as ProductItemInstance;
				if (productItemInstance == null)
				{
					return PackagingStation.EState.MissingItems;
				}
				PackagingDefinition appliedPackaging = productItemInstance.AppliedPackaging;
				int quantity2 = appliedPackaging.Quantity;
				if (this.PackagingSlot.GetCapacityForItem(appliedPackaging.GetDefaultInstance(1)) < 1)
				{
					return PackagingStation.EState.PackageSlotFull;
				}
				productItemInstance.SetPackaging(null);
				if (this.ProductSlot.GetCapacityForItem(productItemInstance) < quantity2)
				{
					return PackagingStation.EState.ProductSlotFull;
				}
			}
			return PackagingStation.EState.CanBegin;
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x001613F8 File Offset: 0x0015F5F8
		public void Unpack()
		{
			PackagingDefinition appliedPackaging = (this.OutputSlot.ItemInstance as ProductItemInstance).AppliedPackaging;
			int quantity = appliedPackaging.Quantity;
			ProductItemInstance productItemInstance = this.OutputSlot.ItemInstance.GetCopy(quantity) as ProductItemInstance;
			productItemInstance.SetPackaging(null);
			if (appliedPackaging.ID != "brick")
			{
				this.PackagingSlot.AddItem(appliedPackaging.GetDefaultInstance(1), false);
			}
			this.ProductSlot.AddItem(productItemInstance, false);
			this.OutputSlot.ChangeQuantity(-1, false);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00161480 File Offset: 0x0015F680
		public void PackSingleInstance()
		{
			int quantity = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("PackagedProductCount");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("PackagedProductCount", (value + 1f).ToString(), true);
			if (this.OutputSlot.ItemInstance == null)
			{
				ItemInstance copy = this.ProductSlot.ItemInstance.GetCopy(1);
				(copy as ProductItemInstance).SetPackaging(this.PackagingSlot.ItemInstance.Definition as PackagingDefinition);
				this.OutputSlot.SetStoredItem(copy, false);
			}
			else
			{
				this.OutputSlot.ChangeQuantity(1, false);
			}
			this.PackagingSlot.ChangeQuantity(-1, false);
			this.ProductSlot.ChangeQuantity(-quantity, false);
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00161550 File Offset: 0x0015F750
		public void SetHatchOpen(bool open)
		{
			PackagingStation.<>c__DisplayClass116_0 CS$<>8__locals1 = new PackagingStation.<>c__DisplayClass116_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			if (CS$<>8__locals1.open == this.hatchOpen)
			{
				return;
			}
			this.hatchOpen = CS$<>8__locals1.open;
			if (this.hatchOpen)
			{
				this.HatchOpenSound.Play();
			}
			else
			{
				this.HatchCloseSound.Play();
			}
			if (this.hatchRoutine != null)
			{
				base.StopCoroutine(this.hatchRoutine);
			}
			base.StartCoroutine(CS$<>8__locals1.<SetHatchOpen>g__Routine|0());
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x001615CD File Offset: 0x0015F7CD
		public void UpdatePackagingVisuals()
		{
			this.UpdatePackagingVisuals(this.PackagingSlot.Quantity);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x001615E0 File Offset: 0x0015F7E0
		public void SetVisualsLocked(bool locked)
		{
			this.visualsLocked = locked;
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x001615EC File Offset: 0x0015F7EC
		public void UpdatePackagingVisuals(int quantity)
		{
			if (this.PackagingSlot == null)
			{
				return;
			}
			if (this.visualsLocked)
			{
				return;
			}
			string text = string.Empty;
			FunctionalPackaging functionalPackaging = null;
			if (quantity > 0 && this.PackagingSlot.ItemInstance != null)
			{
				text = this.PackagingSlot.ItemInstance.ID;
				if (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition == null)
				{
					string str = "Failed to get packaging definition for item instance: ";
					ItemInstance itemInstance = this.PackagingSlot.ItemInstance;
					Console.LogError(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
					return;
				}
				functionalPackaging = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).FunctionalPackaging;
			}
			for (int i = 0; i < this.PackagingAlignments.Length; i++)
			{
				if ((quantity <= i || this.PackagingSlotModelID[i] != text) && this.PackagingSlotModelID[i] != string.Empty)
				{
					if (this.PackagingAlignments[i].childCount > 0)
					{
						UnityEngine.Object.Destroy(this.PackagingAlignments[i].GetChild(0).gameObject);
					}
					this.PackagingSlotModelID[i] = string.Empty;
				}
				if (!(functionalPackaging == null) && quantity > i && this.PackagingSlotModelID[i] != text)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(functionalPackaging.gameObject, this.PackagingAlignments[i]).gameObject;
					gameObject.GetComponent<FunctionalPackaging>().AlignTo(this.PackagingAlignments[i]);
					this.PackagingSlotModelID[i] = text;
					UnityEngine.Object.Destroy(gameObject.GetComponent<FunctionalPackaging>());
				}
			}
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x0016177A File Offset: 0x0015F97A
		public void UpdateProductVisuals()
		{
			this.UpdateProductVisuals(this.ProductSlot.Quantity);
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x00161790 File Offset: 0x0015F990
		public void UpdateProductVisuals(int quantity)
		{
			if (this.ProductSlot == null)
			{
				return;
			}
			if (this.visualsLocked)
			{
				return;
			}
			string text = string.Empty;
			FunctionalProduct functionalProduct = null;
			if (quantity > 0)
			{
				text = this.ProductSlot.ItemInstance.ID;
				ProductDefinition productDefinition = this.ProductSlot.ItemInstance.Definition as ProductDefinition;
				if (productDefinition == null)
				{
					string str = "Failed to get product definition for item instance: ";
					ItemInstance itemInstance = this.PackagingSlot.ItemInstance;
					Console.LogError(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
					return;
				}
				functionalProduct = productDefinition.FunctionalProduct;
			}
			for (int i = 0; i < this.ProductAlignments.Length; i++)
			{
				if ((quantity <= i || this.ProductSlotModelID[i] != text) && this.ProductSlotModelID[i] != string.Empty)
				{
					UnityEngine.Object.Destroy(this.ProductAlignments[i].GetChild(0).gameObject);
					this.ProductSlotModelID[i] = string.Empty;
				}
				if (!(functionalProduct == null) && quantity > i && this.ProductSlotModelID[i] != text)
				{
					FunctionalProduct component = UnityEngine.Object.Instantiate<GameObject>(functionalProduct.gameObject, this.ProductAlignments[i]).GetComponent<FunctionalProduct>();
					component.InitializeVisuals(this.ProductSlot.ItemInstance);
					component.AlignTo(this.ProductAlignments[i]);
					if (component.Rb != null)
					{
						component.Rb.isKinematic = true;
					}
					this.ProductSlotModelID[i] = text;
					UnityEngine.Object.Destroy(component);
				}
			}
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x0016191B File Offset: 0x0015FB1B
		public virtual void StartTask()
		{
			new PackageProductTask(this);
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x00161924 File Offset: 0x0015FB24
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x0016194C File Offset: 0x0015FB4C
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

		// Token: 0x060053F0 RID: 21488 RVA: 0x001619AB File Offset: 0x0015FBAB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x001619C9 File Offset: 0x0015FBC9
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x001619E7 File Offset: 0x0015FBE7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00161A20 File Offset: 0x0015FC20
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

		// Token: 0x060053F4 RID: 21492 RVA: 0x00161AA0 File Offset: 0x0015FCA0
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
			PackagingStationUIElement component = UnityEngine.Object.Instantiate<PackagingStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<PackagingStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00161B33 File Offset: 0x0015FD33
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x00161B4E File Offset: 0x0015FD4E
		public override string GetSaveString()
		{
			return new PackagingStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.ItemSlots)).GetJson(true);
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x00161B88 File Offset: 0x0015FD88
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

		// Token: 0x060053F9 RID: 21497 RVA: 0x00161C4C File Offset: 0x0015FE4C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.PackagingStation));
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x00161E00 File Offset: 0x00160000
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00161E3A File Offset: 0x0016003A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00161E48 File Offset: 0x00160048
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

		// Token: 0x060053FD RID: 21501 RVA: 0x00161EEF File Offset: 0x001600EF
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x00161EF8 File Offset: 0x001600F8
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

		// Token: 0x060053FF RID: 21503 RVA: 0x00161F38 File Offset: 0x00160138
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
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00161FE0 File Offset: 0x001601E0
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
			if (this.OverheadLight != null)
			{
				this.OverheadLight.gameObject.SetActive(this.PlayerUserObject != null);
			}
			if (this.OverheadLightMeshRend != null)
			{
				this.OverheadLightMeshRend.material = ((this.PlayerUserObject != null) ? this.LightMeshOnMat : this.LightMeshOffMat);
			}
			if (this.Switch != null)
			{
				this.Switch.SetIsOn(this.PlayerUserObject != null);
			}
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x001620B8 File Offset: 0x001602B8
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

		// Token: 0x06005402 RID: 21506 RVA: 0x001620F8 File Offset: 0x001602F8
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
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x0016219F File Offset: 0x0016039F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x001621A8 File Offset: 0x001603A8
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

		// Token: 0x06005405 RID: 21509 RVA: 0x001621E8 File Offset: 0x001603E8
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
			base.SendServerRpc(11U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x001622AE File Offset: 0x001604AE
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x001622D8 File Offset: 0x001604D8
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

		// Token: 0x06005408 RID: 21512 RVA: 0x00162340 File Offset: 0x00160540
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
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x00162408 File Offset: 0x00160608
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x00162434 File Offset: 0x00160634
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

		// Token: 0x0600540B RID: 21515 RVA: 0x00162488 File Offset: 0x00160688
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
			base.SendTargetRpc(13U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x00162550 File Offset: 0x00160750
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

		// Token: 0x0600540D RID: 21517 RVA: 0x001625A8 File Offset: 0x001607A8
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
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x00162666 File Offset: 0x00160866
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x00162670 File Offset: 0x00160870
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

		// Token: 0x06005410 RID: 21520 RVA: 0x001626CC File Offset: 0x001608CC
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
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x00162799 File Offset: 0x00160999
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001627B0 File Offset: 0x001609B0
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

		// Token: 0x06005413 RID: 21523 RVA: 0x00162808 File Offset: 0x00160A08
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
			base.SendServerRpc(16U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x001628E8 File Offset: 0x00160AE8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x00162918 File Offset: 0x00160B18
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

		// Token: 0x06005416 RID: 21526 RVA: 0x001629A0 File Offset: 0x00160BA0
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
			base.SendTargetRpc(17U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x00162A81 File Offset: 0x00160C81
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00162AB0 File Offset: 0x00160CB0
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

		// Token: 0x06005419 RID: 21529 RVA: 0x00162B2C File Offset: 0x00160D2C
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00162C10 File Offset: 0x00160E10
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

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x00162C84 File Offset: 0x00160E84
		// (set) Token: 0x0600541C RID: 21532 RVA: 0x00162C8C File Offset: 0x00160E8C
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

		// Token: 0x0600541D RID: 21533 RVA: 0x00162CC8 File Offset: 0x00160EC8
		public virtual bool PackagingStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600541E RID: 21534 RVA: 0x00162DA2 File Offset: 0x00160FA2
		// (set) Token: 0x0600541F RID: 21535 RVA: 0x00162DAA File Offset: 0x00160FAA
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

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x00162DE6 File Offset: 0x00160FE6
		// (set) Token: 0x06005421 RID: 21537 RVA: 0x00162DEE File Offset: 0x00160FEE
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

		// Token: 0x06005422 RID: 21538 RVA: 0x00162E2C File Offset: 0x0016102C
		protected virtual void dll()
		{
			base.Awake();
			this.OverheadLight.gameObject.SetActive(false);
			this.Switch.SetIsOn(false);
			if (!this.isGhost)
			{
				for (int i = 0; i < this.PackagingAlignments.Length; i++)
				{
					this.PackagingSlotModelID.Add(string.Empty);
				}
				for (int j = 0; j < this.ProductAlignments.Length; j++)
				{
					this.ProductSlotModelID.Add(string.Empty);
				}
				this.PackagingSlot.SetSlotOwner(this);
				this.ProductSlot.SetSlotOwner(this);
				this.OutputSlot.SetSlotOwner(this);
				ItemSlot packagingSlot = this.PackagingSlot;
				packagingSlot.onItemDataChanged = (Action)Delegate.Combine(packagingSlot.onItemDataChanged, new Action(this.UpdatePackagingVisuals));
				ItemSlot productSlot = this.ProductSlot;
				productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(this.UpdateProductVisuals));
				this.PackagingSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
				{
					EItemCategory.Packaging
				}));
				this.ProductSlot.AddFilter(new ItemFilter_UnpackagedProduct());
				this.OutputSlot.AddFilter(new ItemFilter_PackagedProduct());
				this.InputSlots.Add(this.PackagingSlot);
				this.InputSlots.Add(this.ProductSlot);
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003E41 RID: 15937
		[Header("References")]
		public Light OverheadLight;

		// Token: 0x04003E42 RID: 15938
		public MeshRenderer OverheadLightMeshRend;

		// Token: 0x04003E43 RID: 15939
		public RockerSwitch Switch;

		// Token: 0x04003E44 RID: 15940
		public Transform CameraPosition;

		// Token: 0x04003E45 RID: 15941
		public Transform CameraPosition_Task;

		// Token: 0x04003E46 RID: 15942
		public InteractableObject IntObj;

		// Token: 0x04003E47 RID: 15943
		public Transform ActivePackagingAlignent;

		// Token: 0x04003E48 RID: 15944
		public Transform[] ActiveProductAlignments;

		// Token: 0x04003E49 RID: 15945
		public Transform Container;

		// Token: 0x04003E4A RID: 15946
		public Collider OutputCollider;

		// Token: 0x04003E4B RID: 15947
		public Transform Hatch;

		// Token: 0x04003E4C RID: 15948
		public Transform[] PackagingAlignments;

		// Token: 0x04003E4D RID: 15949
		public Transform[] ProductAlignments;

		// Token: 0x04003E4E RID: 15950
		public Transform uiPoint;

		// Token: 0x04003E4F RID: 15951
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04003E50 RID: 15952
		public Transform StandPoint;

		// Token: 0x04003E51 RID: 15953
		public Transform[] accessPoints;

		// Token: 0x04003E52 RID: 15954
		public AudioSourceController HatchOpenSound;

		// Token: 0x04003E53 RID: 15955
		public AudioSourceController HatchCloseSound;

		// Token: 0x04003E54 RID: 15956
		[Header("UI")]
		public PackagingStationUIElement WorldspaceUIPrefab;

		// Token: 0x04003E55 RID: 15957
		public Sprite typeIcon;

		// Token: 0x04003E56 RID: 15958
		[Header("Slot Display Points")]
		public Transform PackagingSlotPosition;

		// Token: 0x04003E57 RID: 15959
		public Transform ProductSlotPosition;

		// Token: 0x04003E58 RID: 15960
		public Transform OutputSlotPosition;

		// Token: 0x04003E59 RID: 15961
		[Header("Materials")]
		public Material LightMeshOnMat;

		// Token: 0x04003E5A RID: 15962
		public Material LightMeshOffMat;

		// Token: 0x04003E5B RID: 15963
		[Header("Settings")]
		public float PackagerEmployeeSpeedMultiplier = 1f;

		// Token: 0x04003E5C RID: 15964
		public Vector3 HatchClosedRotation;

		// Token: 0x04003E5D RID: 15965
		public Vector3 HatchOpenRotation;

		// Token: 0x04003E5E RID: 15966
		public float HatchLerpTime = 0.5f;

		// Token: 0x04003E61 RID: 15969
		public ItemSlot PackagingSlot;

		// Token: 0x04003E62 RID: 15970
		public ItemSlot ProductSlot;

		// Token: 0x04003E63 RID: 15971
		public ItemSlot OutputSlot;

		// Token: 0x04003E64 RID: 15972
		private bool hatchOpen;

		// Token: 0x04003E65 RID: 15973
		private Coroutine hatchRoutine;

		// Token: 0x04003E66 RID: 15974
		private List<string> PackagingSlotModelID = new List<string>();

		// Token: 0x04003E67 RID: 15975
		private List<string> ProductSlotModelID = new List<string>();

		// Token: 0x04003E6F RID: 15983
		private bool visualsLocked;

		// Token: 0x04003E70 RID: 15984
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003E71 RID: 15985
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003E72 RID: 15986
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003E73 RID: 15987
		private bool dll_Excuted;

		// Token: 0x04003E74 RID: 15988
		private bool dll_Excuted;

		// Token: 0x02000BB9 RID: 3001
		public enum EMode
		{
			// Token: 0x04003E76 RID: 15990
			Package,
			// Token: 0x04003E77 RID: 15991
			Unpackage
		}

		// Token: 0x02000BBA RID: 3002
		public enum EState
		{
			// Token: 0x04003E79 RID: 15993
			CanBegin,
			// Token: 0x04003E7A RID: 15994
			MissingItems,
			// Token: 0x04003E7B RID: 15995
			InsufficentProduct,
			// Token: 0x04003E7C RID: 15996
			OutputSlotFull,
			// Token: 0x04003E7D RID: 15997
			Mismatch,
			// Token: 0x04003E7E RID: 15998
			PackageSlotFull,
			// Token: 0x04003E7F RID: 15999
			ProductSlotFull
		}
	}
}
