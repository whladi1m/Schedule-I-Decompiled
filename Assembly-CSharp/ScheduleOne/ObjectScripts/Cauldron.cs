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
using ScheduleOne.Misc;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.Trash;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B8F RID: 2959
	public class Cauldron : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x001516F8 File Offset: 0x0014F8F8
		public bool isOpen
		{
			get
			{
				return Singleton<CauldronCanvas>.Instance.isOpen && Singleton<CauldronCanvas>.Instance.Cauldron == this;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x00151718 File Offset: 0x0014F918
		// (set) Token: 0x06005018 RID: 20504 RVA: 0x00151720 File Offset: 0x0014F920
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x00151729 File Offset: 0x0014F929
		// (set) Token: 0x0600501A RID: 20506 RVA: 0x00151731 File Offset: 0x0014F931
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

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600501B RID: 20507 RVA: 0x0015173B File Offset: 0x0014F93B
		// (set) Token: 0x0600501C RID: 20508 RVA: 0x00151743 File Offset: 0x0014F943
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

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x0015174D File Offset: 0x0014F94D
		// (set) Token: 0x0600501F RID: 20511 RVA: 0x00151755 File Offset: 0x0014F955
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06005020 RID: 20512 RVA: 0x0015175E File Offset: 0x0014F95E
		// (set) Token: 0x06005021 RID: 20513 RVA: 0x00151766 File Offset: 0x0014F966
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06005022 RID: 20514 RVA: 0x0015176F File Offset: 0x0014F96F
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06005023 RID: 20515 RVA: 0x00151777 File Offset: 0x0014F977
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06005024 RID: 20516 RVA: 0x0015177F File Offset: 0x0014F97F
		public bool Selectable { get; } = 1;

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x00151787 File Offset: 0x0014F987
		// (set) Token: 0x06005026 RID: 20518 RVA: 0x0015178F File Offset: 0x0014F98F
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x00151798 File Offset: 0x0014F998
		public EntityConfiguration Configuration
		{
			get
			{
				return this.cauldronConfiguration;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x001517A0 File Offset: 0x0014F9A0
		// (set) Token: 0x06005029 RID: 20521 RVA: 0x001517A8 File Offset: 0x0014F9A8
		protected CauldronConfiguration cauldronConfiguration { get; set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600502A RID: 20522 RVA: 0x001517B1 File Offset: 0x0014F9B1
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x001517B9 File Offset: 0x0014F9B9
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Cauldron;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600502C RID: 20524 RVA: 0x001517BC File Offset: 0x0014F9BC
		// (set) Token: 0x0600502D RID: 20525 RVA: 0x001517C4 File Offset: 0x0014F9C4
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600502E RID: 20526 RVA: 0x001517CD File Offset: 0x0014F9CD
		// (set) Token: 0x0600502F RID: 20527 RVA: 0x001517D5 File Offset: 0x0014F9D5
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

		// Token: 0x06005030 RID: 20528 RVA: 0x001517DF File Offset: 0x0014F9DF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06005031 RID: 20529 RVA: 0x001517F5 File Offset: 0x0014F9F5
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06005032 RID: 20530 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x001517FD File Offset: 0x0014F9FD
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06005034 RID: 20532 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x00151805 File Offset: 0x0014FA05
		private bool isCooking
		{
			get
			{
				return this.RemainingCookTime > 0;
			}
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x00151810 File Offset: 0x0014FA10
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Cauldron_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x00151830 File Offset: 0x0014FA30
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
				this.cauldronConfiguration = new CauldronConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x00151894 File Offset: 0x0014FA94
		protected override void Start()
		{
			base.Start();
			if (!this.isGhost)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
				this.StartButtonClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ButtonClicked));
			}
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x00151917 File Offset: 0x0014FB17
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.RemainingCookTime > 0)
			{
				this.StartCookOperation(connection, this.RemainingCookTime, this.InputQuality);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x00151944 File Offset: 0x0014FB44
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Cauldron.<>c__DisplayClass108_0 CS$<>8__locals1 = new Cauldron.<>c__DisplayClass108_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x00151984 File Offset: 0x0014FB84
		public override void DestroyItem(bool callOnServer = true)
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x00151A1C File Offset: 0x0014FC1C
		private void MinPass()
		{
			if (this.RemainingCookTime > 0)
			{
				this.Alarm.SetScreenLit(true);
				this.Alarm.DisplayMinutes(this.RemainingCookTime);
				this.Light.isOn = true;
				this.RemainingCookTime--;
				if (this.RemainingCookTime <= 0 && InstanceFinder.IsServer)
				{
					this.FinishCookOperation();
					return;
				}
			}
			else
			{
				this.Alarm.SetScreenLit(false);
				this.Alarm.DisplayMinutes(0);
				if (this.OutputSlot.Quantity > 0)
				{
					this.Light.isOn = (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.DailyMinTotal % 2 == 0);
					return;
				}
				this.Light.isOn = false;
			}
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x00151AD0 File Offset: 0x0014FCD0
		private void TimeSkipped(int minsPassed)
		{
			for (int i = 0; i < minsPassed; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x00151AEF File Offset: 0x0014FCEF
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

		// Token: 0x0600503F RID: 20543 RVA: 0x00151B1C File Offset: 0x0014FD1C
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

		// Token: 0x06005040 RID: 20544 RVA: 0x00151B76 File Offset: 0x0014FD76
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x00151B94 File Offset: 0x0014FD94
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
			Singleton<CauldronCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x00151C34 File Offset: 0x0014FE34
		public void Close()
		{
			Singleton<CauldronCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x00151CB0 File Offset: 0x0014FEB0
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
			if (this.isCooking)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x00151CF0 File Offset: 0x0014FEF0
		private void UpdateIngredientVisuals()
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance != null)
			{
				this.PrimaryTub.Configure(CauldronDisplayTub.EContents.CocaLeaf, (float)num / 20f);
			}
			else
			{
				this.PrimaryTub.Configure(CauldronDisplayTub.EContents.None, 0f);
			}
			if (itemInstance2 != null)
			{
				this.SecondaryTub.Configure(CauldronDisplayTub.EContents.CocaLeaf, (float)num2 / 20f);
				return;
			}
			this.SecondaryTub.Configure(CauldronDisplayTub.EContents.None, 0f);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x00151D60 File Offset: 0x0014FF60
		public void GetMainInputs(out ItemInstance primaryItem, out int primaryItemQuantity, out ItemInstance secondaryItem, out int secondaryItemQuantity)
		{
			Cauldron.<>c__DisplayClass119_0 CS$<>8__locals1 = new Cauldron.<>c__DisplayClass119_0();
			CS$<>8__locals1.<>4__this = this;
			List<ItemInstance> list = new List<ItemInstance>();
			CS$<>8__locals1.itemQuantities = new Dictionary<ItemInstance, int>();
			int i;
			int k;
			for (i = 0; i < this.IngredientSlots.Length; i = k + 1)
			{
				if (this.IngredientSlots[i].ItemInstance != null)
				{
					ItemInstance itemInstance = list.Find((ItemInstance x) => x.ID == CS$<>8__locals1.<>4__this.IngredientSlots[i].ItemInstance.ID);
					if (itemInstance == null || !itemInstance.CanStackWith(this.IngredientSlots[i].ItemInstance, false))
					{
						itemInstance = this.IngredientSlots[i].ItemInstance;
						list.Add(itemInstance);
						if (!CS$<>8__locals1.itemQuantities.ContainsKey(this.IngredientSlots[i].ItemInstance))
						{
							CS$<>8__locals1.itemQuantities.Add(this.IngredientSlots[i].ItemInstance, 0);
						}
					}
					Dictionary<ItemInstance, int> itemQuantities = CS$<>8__locals1.itemQuantities;
					ItemInstance key = itemInstance;
					itemQuantities[key] += this.IngredientSlots[i].Quantity;
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

		// Token: 0x06005046 RID: 20550 RVA: 0x00151F9D File Offset: 0x0015019D
		public Cauldron.EState GetState()
		{
			if (this.isCooking)
			{
				return Cauldron.EState.Cooking;
			}
			if (!this.HasIngredients())
			{
				return Cauldron.EState.MissingIngredients;
			}
			if (!this.HasOutputSpace())
			{
				return Cauldron.EState.OutputFull;
			}
			return Cauldron.EState.Ready;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x00151FC0 File Offset: 0x001501C0
		public bool HasOutputSpace()
		{
			ItemInstance defaultInstance = this.CocaineBaseDefinition.GetDefaultInstance(1);
			return this.OutputSlot.GetCapacityForItem(defaultInstance) >= 10;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x00151FF0 File Offset: 0x001501F0
		public EQuality RemoveIngredients()
		{
			this.LiquidSlot.ChangeQuantity(-1, false);
			EQuality equality = EQuality.Heavenly;
			int num = 20;
			int num2 = this.IngredientSlots.Length - 1;
			while (num2 >= 0 && num > 0)
			{
				if (this.IngredientSlots[num2].Quantity > 0)
				{
					EQuality quality = (this.IngredientSlots[num2].ItemInstance as QualityItemInstance).Quality;
					if (quality < equality)
					{
						equality = quality;
					}
					int num3 = Mathf.Min(num, this.IngredientSlots[num2].Quantity);
					this.IngredientSlots[num2].ChangeQuantity(-num3, false);
					num -= num3;
				}
				num2--;
			}
			return equality;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x00152084 File Offset: 0x00150284
		public bool HasIngredients()
		{
			int num = 0;
			int quantity = this.LiquidSlot.Quantity;
			for (int i = 0; i < this.IngredientSlots.Length; i++)
			{
				if (this.IngredientSlots[i].ItemInstance != null)
				{
					num += this.IngredientSlots[i].Quantity;
				}
			}
			return num >= 20 && quantity > 0;
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x001520DC File Offset: 0x001502DC
		[ServerRpc(RequireOwnership = false)]
		public void SendCookOperation(int remainingCookTime, EQuality quality)
		{
			this.RpcWriter___Server_SendCookOperation_3536682170(remainingCookTime, quality);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x001520EC File Offset: 0x001502EC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void StartCookOperation(NetworkConnection conn, int remainingCookTime, EQuality quality)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartCookOperation_4210838825(conn, remainingCookTime, quality);
				this.RpcLogic___StartCookOperation_4210838825(conn, remainingCookTime, quality);
			}
			else
			{
				this.RpcWriter___Target_StartCookOperation_4210838825(conn, remainingCookTime, quality);
			}
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x0015213C File Offset: 0x0015033C
		[ObserversRpc]
		public void FinishCookOperation()
		{
			this.RpcWriter___Observers_FinishCookOperation_2166136261();
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0015214F File Offset: 0x0015034F
		private void ButtonClicked(RaycastHit hit)
		{
			if (this.onStartButtonClicked != null)
			{
				this.onStartButtonClicked.Invoke();
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x00152164 File Offset: 0x00150364
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

		// Token: 0x0600504F RID: 20559 RVA: 0x00152293 File Offset: 0x00150493
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x001522A9 File Offset: 0x001504A9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001522BF File Offset: 0x001504BF
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x001522E8 File Offset: 0x001504E8
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

		// Token: 0x06005053 RID: 20563 RVA: 0x00152347 File Offset: 0x00150547
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x00152365 File Offset: 0x00150565
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x00152383 File Offset: 0x00150583
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001523BC File Offset: 0x001505BC
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

		// Token: 0x06005057 RID: 20567 RVA: 0x0015243C File Offset: 0x0015063C
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
			CauldronUIElement component = UnityEngine.Object.Instantiate<CauldronUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<CauldronUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x001524CF File Offset: 0x001506CF
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001524EC File Offset: 0x001506EC
		public override string GetSaveString()
		{
			return new CauldronData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>(this.IngredientSlots)), new ItemSet(new List<ItemSlot>
			{
				this.LiquidSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.RemainingCookTime, this.InputQuality).GetJson(true);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0015256C File Offset: 0x0015076C
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

		// Token: 0x0600505C RID: 20572 RVA: 0x00152614 File Offset: 0x00150814
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3536682170));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_StartCookOperation_4210838825));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_StartCookOperation_4210838825));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_FinishCookOperation_2166136261));
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
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.Cauldron));
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00152824 File Offset: 0x00150A24
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0015285E File Offset: 0x00150A5E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0015286C File Offset: 0x00150A6C
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

		// Token: 0x06005060 RID: 20576 RVA: 0x00152913 File Offset: 0x00150B13
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0015291C File Offset: 0x00150B1C
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

		// Token: 0x06005062 RID: 20578 RVA: 0x0015295C File Offset: 0x00150B5C
		private void RpcWriter___Server_SendCookOperation_3536682170(int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x00152A15 File Offset: 0x00150C15
		public void RpcLogic___SendCookOperation_3536682170(int remainingCookTime, EQuality quality)
		{
			this.StartCookOperation(null, remainingCookTime, quality);
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x00152A20 File Offset: 0x00150C20
		private void RpcReader___Server_SendCookOperation_3536682170(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int remainingCookTime = PooledReader0.ReadInt32(AutoPackType.Packed);
			EQuality quality = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3536682170(remainingCookTime, quality);
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x00152A68 File Offset: 0x00150C68
		private void RpcWriter___Observers_StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x00152B30 File Offset: 0x00150D30
		public void RpcLogic___StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
		{
			this.RemainingCookTime = remainingCookTime;
			this.InputQuality = quality;
			this.CauldronFillable.AddLiquid("gasoline", 1f, Color.white);
			if (this.onCookStart != null)
			{
				this.onCookStart.Invoke();
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x00152B70 File Offset: 0x00150D70
		private void RpcReader___Observers_StartCookOperation_4210838825(PooledReader PooledReader0, Channel channel)
		{
			int remainingCookTime = PooledReader0.ReadInt32(AutoPackType.Packed);
			EQuality quality = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCookOperation_4210838825(null, remainingCookTime, quality);
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x00152BC4 File Offset: 0x00150DC4
		private void RpcWriter___Target_StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x00152C8C File Offset: 0x00150E8C
		private void RpcReader___Target_StartCookOperation_4210838825(PooledReader PooledReader0, Channel channel)
		{
			int remainingCookTime = PooledReader0.ReadInt32(AutoPackType.Packed);
			EQuality quality = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartCookOperation_4210838825(base.LocalConnection, remainingCookTime, quality);
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x00152CDC File Offset: 0x00150EDC
		private void RpcWriter___Observers_FinishCookOperation_2166136261()
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

		// Token: 0x0600506B RID: 20587 RVA: 0x00152D88 File Offset: 0x00150F88
		public void RpcLogic___FinishCookOperation_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				QualityItemInstance qualityItemInstance = this.CocaineBaseDefinition.GetDefaultInstance(10) as QualityItemInstance;
				qualityItemInstance.SetQuality(this.InputQuality);
				this.OutputSlot.InsertItem(qualityItemInstance);
			}
			this.CauldronFillable.ResetContents();
			if (this.onCookEnd != null)
			{
				this.onCookEnd.Invoke();
			}
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x00152DE8 File Offset: 0x00150FE8
		private void RpcReader___Observers_FinishCookOperation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___FinishCookOperation_2166136261();
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x00152E08 File Offset: 0x00151008
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

		// Token: 0x0600506E RID: 20590 RVA: 0x00152EAF File Offset: 0x001510AF
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x00152EB8 File Offset: 0x001510B8
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

		// Token: 0x06005070 RID: 20592 RVA: 0x00152EF8 File Offset: 0x001510F8
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

		// Token: 0x06005071 RID: 20593 RVA: 0x00152F9F File Offset: 0x0015119F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x00152FA8 File Offset: 0x001511A8
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

		// Token: 0x06005073 RID: 20595 RVA: 0x00152FE8 File Offset: 0x001511E8
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

		// Token: 0x06005074 RID: 20596 RVA: 0x001530AE File Offset: 0x001512AE
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001530D8 File Offset: 0x001512D8
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

		// Token: 0x06005076 RID: 20598 RVA: 0x00153140 File Offset: 0x00151340
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

		// Token: 0x06005077 RID: 20599 RVA: 0x00153208 File Offset: 0x00151408
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00153234 File Offset: 0x00151434
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

		// Token: 0x06005079 RID: 20601 RVA: 0x00153288 File Offset: 0x00151488
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

		// Token: 0x0600507A RID: 20602 RVA: 0x00153350 File Offset: 0x00151550
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

		// Token: 0x0600507B RID: 20603 RVA: 0x001533A8 File Offset: 0x001515A8
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

		// Token: 0x0600507C RID: 20604 RVA: 0x00153466 File Offset: 0x00151666
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x00153470 File Offset: 0x00151670
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

		// Token: 0x0600507E RID: 20606 RVA: 0x001534CC File Offset: 0x001516CC
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

		// Token: 0x0600507F RID: 20607 RVA: 0x00153599 File Offset: 0x00151799
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001535B0 File Offset: 0x001517B0
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

		// Token: 0x06005081 RID: 20609 RVA: 0x00153608 File Offset: 0x00151808
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

		// Token: 0x06005082 RID: 20610 RVA: 0x001536E8 File Offset: 0x001518E8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x00153718 File Offset: 0x00151918
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

		// Token: 0x06005084 RID: 20612 RVA: 0x001537A0 File Offset: 0x001519A0
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

		// Token: 0x06005085 RID: 20613 RVA: 0x00153881 File Offset: 0x00151A81
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001538B0 File Offset: 0x00151AB0
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

		// Token: 0x06005087 RID: 20615 RVA: 0x0015392C File Offset: 0x00151B2C
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

		// Token: 0x06005088 RID: 20616 RVA: 0x00153A10 File Offset: 0x00151C10
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

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x00153A84 File Offset: 0x00151C84
		// (set) Token: 0x0600508A RID: 20618 RVA: 0x00153A8C File Offset: 0x00151C8C
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

		// Token: 0x0600508B RID: 20619 RVA: 0x00153AC8 File Offset: 0x00151CC8
		public virtual bool Cauldron(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x00153BA2 File Offset: 0x00151DA2
		// (set) Token: 0x0600508D RID: 20621 RVA: 0x00153BAA File Offset: 0x00151DAA
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

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x00153BE6 File Offset: 0x00151DE6
		// (set) Token: 0x0600508F RID: 20623 RVA: 0x00153BEE File Offset: 0x00151DEE
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

		// Token: 0x06005090 RID: 20624 RVA: 0x00153C2C File Offset: 0x00151E2C
		protected virtual void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.IngredientSlots = new ItemSlot[4];
				for (int i = 0; i < 4; i++)
				{
					this.IngredientSlots[i] = new ItemSlot();
					this.IngredientSlots[i].SetSlotOwner(this);
					this.IngredientSlots[i].AddFilter(new ItemFilter_ID(new List<string>
					{
						"cocaleaf"
					}));
					ItemSlot itemSlot = this.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.UpdateIngredientVisuals));
				}
				this.LiquidSlot = new ItemSlot();
				this.LiquidSlot.SetSlotOwner(this);
				this.LiquidSlot.AddFilter(new ItemFilter_ID(new List<string>
				{
					"gasoline"
				}));
				this.LiquidVisuals.AddSlot(this.LiquidSlot, false);
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.AddRange(this.IngredientSlots);
				this.InputSlots.Add(this.LiquidSlot);
				this.OutputSlots.Add(this.OutputSlot);
				this.PrimaryTub.gameObject.SetActive(true);
				this.SecondaryTub.gameObject.SetActive(true);
			}
		}

		// Token: 0x04003C43 RID: 15427
		public const int INGREDIENT_SLOT_COUNT = 4;

		// Token: 0x04003C44 RID: 15428
		public const int COCA_LEAF_REQUIRED = 20;

		// Token: 0x04003C45 RID: 15429
		public ItemSlot[] IngredientSlots;

		// Token: 0x04003C46 RID: 15430
		public ItemSlot LiquidSlot;

		// Token: 0x04003C47 RID: 15431
		public ItemSlot OutputSlot;

		// Token: 0x04003C49 RID: 15433
		public int CookTime = 360;

		// Token: 0x04003C4A RID: 15434
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04003C4B RID: 15435
		public Transform CameraPosition_CombineIngredients;

		// Token: 0x04003C4C RID: 15436
		public Transform CameraPosition_StartMachine;

		// Token: 0x04003C4D RID: 15437
		public InteractableObject IntObj;

		// Token: 0x04003C4E RID: 15438
		public Transform[] accessPoints;

		// Token: 0x04003C4F RID: 15439
		public Transform StandPoint;

		// Token: 0x04003C50 RID: 15440
		public Transform uiPoint;

		// Token: 0x04003C51 RID: 15441
		public StorageVisualizer LiquidVisuals;

		// Token: 0x04003C52 RID: 15442
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003C53 RID: 15443
		public CauldronDisplayTub PrimaryTub;

		// Token: 0x04003C54 RID: 15444
		public CauldronDisplayTub SecondaryTub;

		// Token: 0x04003C55 RID: 15445
		public Transform ItemContainer;

		// Token: 0x04003C56 RID: 15446
		public Transform GasolineSpawnPoint;

		// Token: 0x04003C57 RID: 15447
		public Transform TubSpawnPoint;

		// Token: 0x04003C58 RID: 15448
		public Transform[] LeafSpawns;

		// Token: 0x04003C59 RID: 15449
		public Light OverheadLight;

		// Token: 0x04003C5A RID: 15450
		public Fillable CauldronFillable;

		// Token: 0x04003C5B RID: 15451
		public Clickable StartButtonClickable;

		// Token: 0x04003C5C RID: 15452
		public DigitalAlarm Alarm;

		// Token: 0x04003C5D RID: 15453
		public ToggleableLight Light;

		// Token: 0x04003C5E RID: 15454
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003C5F RID: 15455
		public BoxCollider TrashSpawnVolume;

		// Token: 0x04003C60 RID: 15456
		[Header("Prefabs")]
		public StationItem CocaLeafPrefab;

		// Token: 0x04003C61 RID: 15457
		public StationItem GasolinePrefab;

		// Token: 0x04003C62 RID: 15458
		public Draggable TubPrefab;

		// Token: 0x04003C63 RID: 15459
		public QualityItemDefinition CocaineBaseDefinition;

		// Token: 0x04003C64 RID: 15460
		[Header("UI")]
		public CauldronUIElement WorldspaceUIPrefab;

		// Token: 0x04003C65 RID: 15461
		public Sprite typeIcon;

		// Token: 0x04003C66 RID: 15462
		public UnityEvent onStartButtonClicked;

		// Token: 0x04003C67 RID: 15463
		public UnityEvent onCookStart;

		// Token: 0x04003C68 RID: 15464
		public UnityEvent onCookEnd;

		// Token: 0x04003C69 RID: 15465
		public int RemainingCookTime;

		// Token: 0x04003C6A RID: 15466
		public EQuality InputQuality = EQuality.Standard;

		// Token: 0x04003C74 RID: 15476
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003C75 RID: 15477
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003C76 RID: 15478
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003C77 RID: 15479
		private bool dll_Excuted;

		// Token: 0x04003C78 RID: 15480
		private bool dll_Excuted;

		// Token: 0x02000B90 RID: 2960
		public enum EState
		{
			// Token: 0x04003C7A RID: 15482
			MissingIngredients,
			// Token: 0x04003C7B RID: 15483
			Ready,
			// Token: 0x04003C7C RID: 15484
			Cooking,
			// Token: 0x04003C7D RID: 15485
			OutputFull
		}
	}
}
