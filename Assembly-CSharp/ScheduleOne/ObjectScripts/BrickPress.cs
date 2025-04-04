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
using FishNet.Transporting;
using ScheduleOne.Audio;
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
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B87 RID: 2951
	public class BrickPress : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x0014F274 File Offset: 0x0014D474
		public bool isOpen
		{
			get
			{
				return this.PlayerUserObject == Player.Local.NetworkObject;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x0014F28B File Offset: 0x0014D48B
		// (set) Token: 0x06004F84 RID: 20356 RVA: 0x0014F293 File Offset: 0x0014D493
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06004F85 RID: 20357 RVA: 0x0014F29C File Offset: 0x0014D49C
		// (set) Token: 0x06004F86 RID: 20358 RVA: 0x0014F2A4 File Offset: 0x0014D4A4
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

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x0014F2AE File Offset: 0x0014D4AE
		// (set) Token: 0x06004F88 RID: 20360 RVA: 0x0014F2B6 File Offset: 0x0014D4B6
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

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x0014F2C0 File Offset: 0x0014D4C0
		// (set) Token: 0x06004F8A RID: 20362 RVA: 0x0014F2C8 File Offset: 0x0014D4C8
		public ItemSlot[] ProductSlots { get; private set; }

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x0014F2D1 File Offset: 0x0014D4D1
		// (set) Token: 0x06004F8C RID: 20364 RVA: 0x0014F2D9 File Offset: 0x0014D4D9
		public ItemSlot OutputSlot { get; private set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x0014F2E2 File Offset: 0x0014D4E2
		// (set) Token: 0x06004F8F RID: 20367 RVA: 0x0014F2EA File Offset: 0x0014D4EA
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x0014F2F3 File Offset: 0x0014D4F3
		// (set) Token: 0x06004F91 RID: 20369 RVA: 0x0014F2FB File Offset: 0x0014D4FB
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06004F92 RID: 20370 RVA: 0x0014F304 File Offset: 0x0014D504
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06004F93 RID: 20371 RVA: 0x0014F30C File Offset: 0x0014D50C
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004F94 RID: 20372 RVA: 0x0014F314 File Offset: 0x0014D514
		public bool Selectable { get; } = 1;

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06004F95 RID: 20373 RVA: 0x0014F31C File Offset: 0x0014D51C
		// (set) Token: 0x06004F96 RID: 20374 RVA: 0x0014F324 File Offset: 0x0014D524
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004F97 RID: 20375 RVA: 0x0014F32D File Offset: 0x0014D52D
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06004F98 RID: 20376 RVA: 0x0014F335 File Offset: 0x0014D535
		// (set) Token: 0x06004F99 RID: 20377 RVA: 0x0014F33D File Offset: 0x0014D53D
		protected BrickPressConfiguration stationConfiguration { get; set; }

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06004F9A RID: 20378 RVA: 0x0014F346 File Offset: 0x0014D546
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x00010FEA File Offset: 0x0000F1EA
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.BrickPress;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x0014F34E File Offset: 0x0014D54E
		// (set) Token: 0x06004F9D RID: 20381 RVA: 0x0014F356 File Offset: 0x0014D556
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004F9E RID: 20382 RVA: 0x0014F35F File Offset: 0x0014D55F
		// (set) Token: 0x06004F9F RID: 20383 RVA: 0x0014F367 File Offset: 0x0014D567
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

		// Token: 0x06004FA0 RID: 20384 RVA: 0x0014F371 File Offset: 0x0014D571
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06004FA1 RID: 20385 RVA: 0x0014F387 File Offset: 0x0014D587
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06004FA2 RID: 20386 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06004FA3 RID: 20387 RVA: 0x0014F304 File Offset: 0x0014D504
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06004FA4 RID: 20388 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x0014F390 File Offset: 0x0014D590
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.BrickPress_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x0014F3B0 File Offset: 0x0014D5B0
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
				this.stationConfiguration = new BrickPressConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x0014F413 File Offset: 0x0014D613
		protected virtual void LateUpdate()
		{
			this.PressTransform.localPosition = Vector3.Lerp(this.PressTransform_Raised.localPosition, this.PressTransform_Lowered.localPosition, this.Handle.CurrentPosition);
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x0014F446 File Offset: 0x0014D646
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x0014F460 File Offset: 0x0014D660
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			BrickPress.<>c__DisplayClass98_0 CS$<>8__locals1 = new BrickPress.<>c__DisplayClass98_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x0014F4A0 File Offset: 0x0014D6A0
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

		// Token: 0x06004FAB RID: 20395 RVA: 0x0014F4CB File Offset: 0x0014D6CB
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "In use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x0014F4F7 File Offset: 0x0014D6F7
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

		// Token: 0x06004FAD RID: 20397 RVA: 0x0014F536 File Offset: 0x0014D736
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x0014F54C File Offset: 0x0014D74C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x0014F564 File Offset: 0x0014D764
		public PackagingStation.EState GetState()
		{
			ProductItemInstance productItemInstance;
			if (!this.HasSufficientProduct(out productItemInstance))
			{
				return PackagingStation.EState.InsufficentProduct;
			}
			if (this.OutputSlot.ItemInstance != null)
			{
				if ((this.OutputSlot.ItemInstance as QualityItemInstance).Quality != productItemInstance.Quality)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.ID != productItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.ID != productItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.Quantity >= this.OutputSlot.ItemInstance.StackLimit)
				{
					return PackagingStation.EState.OutputSlotFull;
				}
			}
			return PackagingStation.EState.CanBegin;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x0014F610 File Offset: 0x0014D810
		private void UpdateInputVisuals()
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance != null)
			{
				this.Container1.SetContents(itemInstance as ProductItemInstance, (float)num / 20f);
			}
			else
			{
				this.Container1.SetContents(null, 0f);
			}
			if (itemInstance2 != null)
			{
				this.Container2.SetContents(itemInstance2 as ProductItemInstance, (float)num2 / 20f);
				return;
			}
			this.Container2.SetContents(null, 0f);
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x0014F688 File Offset: 0x0014D888
		public bool HasSufficientProduct(out ProductItemInstance product)
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance == null)
			{
				product = null;
				return false;
			}
			product = (itemInstance as ProductItemInstance);
			return num >= 20;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x0014F6BC File Offset: 0x0014D8BC
		public void GetMainInputs(out ItemInstance primaryItem, out int primaryItemQuantity, out ItemInstance secondaryItem, out int secondaryItemQuantity)
		{
			BrickPress.<>c__DisplayClass107_0 CS$<>8__locals1 = new BrickPress.<>c__DisplayClass107_0();
			CS$<>8__locals1.<>4__this = this;
			List<ItemInstance> list = new List<ItemInstance>();
			CS$<>8__locals1.itemQuantities = new Dictionary<ItemInstance, int>();
			int i;
			int k;
			for (i = 0; i < this.InputSlots.Count; i = k + 1)
			{
				if (this.InputSlots[i].ItemInstance != null)
				{
					ItemInstance itemInstance = list.Find((ItemInstance x) => x.ID == CS$<>8__locals1.<>4__this.InputSlots[i].ItemInstance.ID);
					if (itemInstance == null || !itemInstance.CanStackWith(this.InputSlots[i].ItemInstance, false))
					{
						itemInstance = this.InputSlots[i].ItemInstance;
						list.Add(itemInstance);
						if (!CS$<>8__locals1.itemQuantities.ContainsKey(this.InputSlots[i].ItemInstance))
						{
							CS$<>8__locals1.itemQuantities.Add(this.InputSlots[i].ItemInstance, 0);
						}
					}
					Dictionary<ItemInstance, int> itemQuantities = CS$<>8__locals1.itemQuantities;
					ItemInstance key = itemInstance;
					itemQuantities[key] += this.InputSlots[i].Quantity;
				}
				k = i;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (CS$<>8__locals1.itemQuantities[list[j]] > 20)
				{
					int num = CS$<>8__locals1.itemQuantities[list[j]] - 20;
					CS$<>8__locals1.itemQuantities[list[j]] = 20;
					ItemInstance copy = list[j].GetCopy(num);
					list.Add(copy);
					CS$<>8__locals1.itemQuantities.Add(copy, num);
				}
			}
			list = (from x in list
			orderby CS$<>8__locals1.itemQuantities[x] descending
			select x).ToList<ItemInstance>();
			if (list.Count > 0)
			{
				primaryItem = list[0];
				primaryItemQuantity = CS$<>8__locals1.itemQuantities[list[0]];
			}
			else
			{
				primaryItem = null;
				primaryItemQuantity = 0;
			}
			if (list.Count > 1)
			{
				secondaryItem = list[1];
				secondaryItemQuantity = CS$<>8__locals1.itemQuantities[list[1]];
				return;
			}
			secondaryItem = null;
			secondaryItemQuantity = 0;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x0014F914 File Offset: 0x0014DB14
		public Draggable CreateFunctionalContainer(ProductItemInstance instance, float productScale, out List<FunctionalProduct> products)
		{
			Draggable draggable = UnityEngine.Object.Instantiate<Draggable>(this.FunctionalContainerPrefab, NetworkSingleton<GameManager>.Instance.Temp);
			draggable.transform.position = this.ContainerSpawnPoint.position;
			draggable.transform.rotation = this.ContainerSpawnPoint.rotation;
			draggable.GetComponent<DraggableConstraint>().SetContainer(base.transform);
			Transform transform = draggable.transform.Find("ProductSpawnPoints");
			ProductDefinition productDefinition = instance.Definition as ProductDefinition;
			products = new List<FunctionalProduct>();
			for (int i = 0; i < 20; i++)
			{
				Transform child = transform.GetChild(i);
				FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>(productDefinition.FunctionalProduct, NetworkSingleton<GameManager>.Instance.Temp);
				functionalProduct.transform.position = child.position;
				functionalProduct.transform.rotation = child.rotation;
				functionalProduct.transform.localScale = Vector3.one * productScale;
				functionalProduct.Initialize(instance);
				products.Add(functionalProduct);
			}
			return draggable;
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x0014FA14 File Offset: 0x0014DC14
		public void PlayPressAnim()
		{
			base.StartCoroutine(this.<PlayPressAnim>g__Routine|109_0());
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x0014FA24 File Offset: 0x0014DC24
		public void CompletePress(ProductItemInstance product)
		{
			ProductItemInstance productItemInstance = product.GetCopy(1) as ProductItemInstance;
			productItemInstance.SetPackaging(this.BrickPackaging);
			this.OutputSlot.AddItem(productItemInstance, false);
			int num = 20;
			int num2 = 0;
			while (num2 < this.InputSlots.Count && num > 0)
			{
				if (this.InputSlots[num2].ItemInstance != null && this.InputSlots[num2].ItemInstance.CanStackWith(product, false))
				{
					int num3 = Mathf.Min(num, this.InputSlots[num2].Quantity);
					this.InputSlots[num2].ChangeQuantity(-num3, false);
					num -= num3;
				}
				num2++;
			}
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x0014FAD4 File Offset: 0x0014DCD4
		public List<FunctionalProduct> GetProductInMould()
		{
			Collider[] array = Physics.OverlapBox(this.MouldDetection.bounds.center, this.MouldDetection.bounds.extents, this.MouldDetection.transform.rotation, LayerMask.GetMask(new string[]
			{
				"Task"
			}));
			List<FunctionalProduct> list = new List<FunctionalProduct>();
			for (int i = 0; i < array.Length; i++)
			{
				FunctionalProduct componentInParent = array[i].GetComponentInParent<FunctionalProduct>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
				}
			}
			return list;
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x0014FB6C File Offset: 0x0014DD6C
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
			BrickPressUIElement component = UnityEngine.Object.Instantiate<BrickPressUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<BrickPressUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x0014FBFF File Offset: 0x0014DDFF
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x0014FC1C File Offset: 0x0014DE1C
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

		// Token: 0x06004FBA RID: 20410 RVA: 0x0014FC76 File Offset: 0x0014DE76
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x0014FC94 File Offset: 0x0014DE94
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
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x0014FD34 File Offset: 0x0014DF34
		public void Close()
		{
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x0014FDB0 File Offset: 0x0014DFB0
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x0014FDD8 File Offset: 0x0014DFD8
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

		// Token: 0x06004FBF RID: 20415 RVA: 0x0014FE37 File Offset: 0x0014E037
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x0014FE55 File Offset: 0x0014E055
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x0014FE73 File Offset: 0x0014E073
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x0014FEAC File Offset: 0x0014E0AC
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

		// Token: 0x06004FC3 RID: 20419 RVA: 0x0014FF2B File Offset: 0x0014E12B
		public override string GetSaveString()
		{
			return new BrickPressData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.ItemSlots)).GetJson(true);
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x0014FF64 File Offset: 0x0014E164
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

		// Token: 0x06004FC6 RID: 20422 RVA: 0x0014FFEC File Offset: 0x0014E1EC
		[CompilerGenerated]
		private IEnumerator <PlayPressAnim>g__Routine|109_0()
		{
			this.Handle.Locked = true;
			this.Handle.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.SlamSound.Play();
			yield return new WaitForSeconds(0.5f);
			this.Handle.Locked = false;
			yield break;
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x0014FFFC File Offset: 0x0014E1FC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted = true;
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
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.BrickPress));
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x001501B0 File Offset: 0x0014E3B0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001501EA File Offset: 0x0014E3EA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x001501F8 File Offset: 0x0014E3F8
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

		// Token: 0x06004FCB RID: 20427 RVA: 0x0015029F File Offset: 0x0014E49F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x001502A8 File Offset: 0x0014E4A8
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

		// Token: 0x06004FCD RID: 20429 RVA: 0x001502E8 File Offset: 0x0014E4E8
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

		// Token: 0x06004FCE RID: 20430 RVA: 0x0015038F File Offset: 0x0014E58F
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x00150398 File Offset: 0x0014E598
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

		// Token: 0x06004FD0 RID: 20432 RVA: 0x001503D8 File Offset: 0x0014E5D8
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

		// Token: 0x06004FD1 RID: 20433 RVA: 0x0015047F File Offset: 0x0014E67F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00150488 File Offset: 0x0014E688
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

		// Token: 0x06004FD3 RID: 20435 RVA: 0x001504C8 File Offset: 0x0014E6C8
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

		// Token: 0x06004FD4 RID: 20436 RVA: 0x0015058E File Offset: 0x0014E78E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001505B8 File Offset: 0x0014E7B8
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

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00150620 File Offset: 0x0014E820
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

		// Token: 0x06004FD7 RID: 20439 RVA: 0x001506E8 File Offset: 0x0014E8E8
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x00150714 File Offset: 0x0014E914
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

		// Token: 0x06004FD9 RID: 20441 RVA: 0x00150768 File Offset: 0x0014E968
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

		// Token: 0x06004FDA RID: 20442 RVA: 0x00150830 File Offset: 0x0014EA30
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

		// Token: 0x06004FDB RID: 20443 RVA: 0x00150888 File Offset: 0x0014EA88
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

		// Token: 0x06004FDC RID: 20444 RVA: 0x00150946 File Offset: 0x0014EB46
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x00150950 File Offset: 0x0014EB50
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

		// Token: 0x06004FDE RID: 20446 RVA: 0x001509AC File Offset: 0x0014EBAC
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

		// Token: 0x06004FDF RID: 20447 RVA: 0x00150A79 File Offset: 0x0014EC79
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x00150A90 File Offset: 0x0014EC90
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

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00150AE8 File Offset: 0x0014ECE8
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

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00150BC8 File Offset: 0x0014EDC8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00150BF8 File Offset: 0x0014EDF8
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

		// Token: 0x06004FE4 RID: 20452 RVA: 0x00150C80 File Offset: 0x0014EE80
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

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00150D61 File Offset: 0x0014EF61
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x00150D90 File Offset: 0x0014EF90
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

		// Token: 0x06004FE7 RID: 20455 RVA: 0x00150E0C File Offset: 0x0014F00C
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

		// Token: 0x06004FE8 RID: 20456 RVA: 0x00150EF0 File Offset: 0x0014F0F0
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

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x00150F64 File Offset: 0x0014F164
		// (set) Token: 0x06004FEA RID: 20458 RVA: 0x00150F6C File Offset: 0x0014F16C
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

		// Token: 0x06004FEB RID: 20459 RVA: 0x00150FA8 File Offset: 0x0014F1A8
		public virtual bool BrickPress(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x00151082 File Offset: 0x0014F282
		// (set) Token: 0x06004FED RID: 20461 RVA: 0x0015108A File Offset: 0x0014F28A
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

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06004FEE RID: 20462 RVA: 0x001510C6 File Offset: 0x0014F2C6
		// (set) Token: 0x06004FEF RID: 20463 RVA: 0x001510CE File Offset: 0x0014F2CE
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

		// Token: 0x06004FF0 RID: 20464 RVA: 0x0015110C File Offset: 0x0014F30C
		protected virtual void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.ProductSlots = new ItemSlot[2];
				for (int i = 0; i < 2; i++)
				{
					this.ProductSlots[i] = new ItemSlot();
					this.ProductSlots[i].SetSlotOwner(this);
					this.ProductSlots[i].AddFilter(new ItemFilter_UnpackagedProduct());
					ItemSlot itemSlot = this.ProductSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.UpdateInputVisuals));
				}
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.AddRange(this.ProductSlots);
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003BFD RID: 15357
		public const int INPUT_SLOT_COUNT = 2;

		// Token: 0x04003C01 RID: 15361
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04003C02 RID: 15362
		public Transform CameraPosition_Pouring;

		// Token: 0x04003C03 RID: 15363
		public Transform CameraPosition_Raising;

		// Token: 0x04003C04 RID: 15364
		public InteractableObject IntObj;

		// Token: 0x04003C05 RID: 15365
		public Transform uiPoint;

		// Token: 0x04003C06 RID: 15366
		public Transform StandPoint;

		// Token: 0x04003C07 RID: 15367
		public Transform[] accessPoints;

		// Token: 0x04003C08 RID: 15368
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003C09 RID: 15369
		public BrickPressContainer Container1;

		// Token: 0x04003C0A RID: 15370
		public BrickPressContainer Container2;

		// Token: 0x04003C0B RID: 15371
		public Transform ContainerSpawnPoint;

		// Token: 0x04003C0C RID: 15372
		public PackagingDefinition BrickPackaging;

		// Token: 0x04003C0D RID: 15373
		public BoxCollider MouldDetection;

		// Token: 0x04003C0E RID: 15374
		public BrickPressHandle Handle;

		// Token: 0x04003C0F RID: 15375
		public Transform PressTransform;

		// Token: 0x04003C10 RID: 15376
		public Transform PressTransform_Raised;

		// Token: 0x04003C11 RID: 15377
		public Transform PressTransform_Lowered;

		// Token: 0x04003C12 RID: 15378
		public Transform PressTransform_Compressed;

		// Token: 0x04003C13 RID: 15379
		public AudioSourceController SlamSound;

		// Token: 0x04003C14 RID: 15380
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003C15 RID: 15381
		[Header("Prefabs")]
		public Draggable FunctionalContainerPrefab;

		// Token: 0x04003C16 RID: 15382
		[Header("UI")]
		public BrickPressUIElement WorldspaceUIPrefab;

		// Token: 0x04003C17 RID: 15383
		public Sprite typeIcon;

		// Token: 0x04003C21 RID: 15393
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003C22 RID: 15394
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003C23 RID: 15395
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003C24 RID: 15396
		private bool dll_Excuted;

		// Token: 0x04003C25 RID: 15397
		private bool dll_Excuted;
	}
}
