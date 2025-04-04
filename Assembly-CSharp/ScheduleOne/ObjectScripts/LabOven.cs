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
using ScheduleOne.Audio;
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
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using TMPro;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BA6 RID: 2982
	public class LabOven : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060051F6 RID: 20982 RVA: 0x001599E2 File Offset: 0x00157BE2
		public bool isOpen
		{
			get
			{
				return Singleton<LabOvenCanvas>.Instance.isOpen && Singleton<LabOvenCanvas>.Instance.Oven == this;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x00159A02 File Offset: 0x00157C02
		// (set) Token: 0x060051F8 RID: 20984 RVA: 0x00159A0A File Offset: 0x00157C0A
		public OvenCookOperation CurrentOperation { get; private set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060051F9 RID: 20985 RVA: 0x00159A13 File Offset: 0x00157C13
		// (set) Token: 0x060051FA RID: 20986 RVA: 0x00159A1B File Offset: 0x00157C1B
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x00159A24 File Offset: 0x00157C24
		// (set) Token: 0x060051FC RID: 20988 RVA: 0x00159A2C File Offset: 0x00157C2C
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

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x00159A36 File Offset: 0x00157C36
		// (set) Token: 0x060051FE RID: 20990 RVA: 0x00159A3E File Offset: 0x00157C3E
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

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x00159A48 File Offset: 0x00157C48
		// (set) Token: 0x06005201 RID: 20993 RVA: 0x00159A50 File Offset: 0x00157C50
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x00159A59 File Offset: 0x00157C59
		// (set) Token: 0x06005203 RID: 20995 RVA: 0x00159A61 File Offset: 0x00157C61
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x00159A6A File Offset: 0x00157C6A
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x00159A72 File Offset: 0x00157C72
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06005206 RID: 20998 RVA: 0x00159A7A File Offset: 0x00157C7A
		public bool Selectable { get; } = 1;

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06005207 RID: 20999 RVA: 0x00159A82 File Offset: 0x00157C82
		// (set) Token: 0x06005208 RID: 21000 RVA: 0x00159A8A File Offset: 0x00157C8A
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x00159A93 File Offset: 0x00157C93
		public EntityConfiguration Configuration
		{
			get
			{
				return this.ovenConfiguration;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600520A RID: 21002 RVA: 0x00159A9B File Offset: 0x00157C9B
		// (set) Token: 0x0600520B RID: 21003 RVA: 0x00159AA3 File Offset: 0x00157CA3
		protected LabOvenConfiguration ovenConfiguration { get; set; }

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600520C RID: 21004 RVA: 0x00159AAC File Offset: 0x00157CAC
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600520D RID: 21005 RVA: 0x0001A87C File Offset: 0x00018A7C
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.LabOven;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600520E RID: 21006 RVA: 0x00159AB4 File Offset: 0x00157CB4
		// (set) Token: 0x0600520F RID: 21007 RVA: 0x00159ABC File Offset: 0x00157CBC
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06005210 RID: 21008 RVA: 0x00159AC5 File Offset: 0x00157CC5
		// (set) Token: 0x06005211 RID: 21009 RVA: 0x00159ACD File Offset: 0x00157CCD
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

		// Token: 0x06005212 RID: 21010 RVA: 0x00159AD7 File Offset: 0x00157CD7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06005213 RID: 21011 RVA: 0x00159AED File Offset: 0x00157CED
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06005214 RID: 21012 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06005215 RID: 21013 RVA: 0x00159AF5 File Offset: 0x00157CF5
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06005216 RID: 21014 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x00159B00 File Offset: 0x00157D00
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.LabOven_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x00159B20 File Offset: 0x00157D20
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
				this.ovenConfiguration = new LabOvenConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
				TimeManager instance3 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
			}
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x00159BD2 File Offset: 0x00157DD2
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			if (this.CurrentOperation != null)
			{
				this.SetCookOperation(connection, this.CurrentOperation, false);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x00159C00 File Offset: 0x00157E00
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			LabOven.<>c__DisplayClass125_0 CS$<>8__locals1 = new LabOven.<>c__DisplayClass125_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x00159C40 File Offset: 0x00157E40
		private void Update()
		{
			switch (this.LightMode)
			{
			case LabOven.ELightMode.Off:
				this.Light.isOn = false;
				break;
			case LabOven.ELightMode.On:
				this.Light.isOn = true;
				break;
			case LabOven.ELightMode.Flash:
				this.Light.isOn = (Mathf.Sin(Time.timeSinceLevelLoad * 4f) > 0f);
				break;
			}
			if (this.CurrentOperation != null)
			{
				this.RunLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.RunLoopSound.VolumeMultiplier, 1f, Time.deltaTime);
				if (!this.RunLoopSound.isPlaying)
				{
					this.RunLoopSound.Play();
					return;
				}
			}
			else
			{
				this.RunLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.RunLoopSound.VolumeMultiplier, 0f, Time.deltaTime);
				if (this.RunLoopSound.VolumeMultiplier <= 0f)
				{
					this.RunLoopSound.Stop();
				}
			}
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x00159D30 File Offset: 0x00157F30
		private void MinPass()
		{
			if (this.CurrentOperation != null)
			{
				bool flag = this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration();
				this.CurrentOperation.UpdateCookProgress(1);
				if (!flag && this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
				{
					this.DingSound.Play();
				}
			}
			this.UpdateOvenAppearance();
			this.UpdateLiquid();
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x00159DA0 File Offset: 0x00157FA0
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

		// Token: 0x0600521E RID: 21022 RVA: 0x00159DC8 File Offset: 0x00157FC8
		private void UpdateOvenAppearance()
		{
			if (this.CurrentOperation != null)
			{
				this.Button.SetPressed(true);
				this.TimerLabel.enabled = true;
				if (this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
				{
					this.SetOvenLit(false);
					this.LightMode = LabOven.ELightMode.Flash;
				}
				else
				{
					this.SetOvenLit(true);
					this.LightMode = LabOven.ELightMode.On;
				}
				int num = this.CurrentOperation.GetCookDuration() - this.CurrentOperation.CookProgress;
				num = Mathf.Max(0, num);
				int num2 = num / 60;
				num %= 60;
				this.TimerLabel.text = string.Format("{0:D2}:{1:D2}", num2, num);
				return;
			}
			this.TimerLabel.enabled = false;
			this.Button.SetPressed(false);
			this.SetOvenLit(false);
			this.LightMode = LabOven.ELightMode.Off;
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x00159EA0 File Offset: 0x001580A0
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

		// Token: 0x06005220 RID: 21024 RVA: 0x00159ECB File Offset: 0x001580CB
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
			if (this.CurrentOperation != null)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x00159F08 File Offset: 0x00158108
		public override void DestroyItem(bool callOnServer = true)
		{
			base.DestroyItem(callOnServer);
			if (!this.isGhost)
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
			}
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x00159FA9 File Offset: 0x001581A9
		public void SetOvenLit(bool lit)
		{
			this.OvenLight.isOn = lit;
			this.Button.SetPressed(lit);
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x00159FC3 File Offset: 0x001581C3
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x00159FD9 File Offset: 0x001581D9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x00159FF0 File Offset: 0x001581F0
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

		// Token: 0x06005226 RID: 21030 RVA: 0x0015A04A File Offset: 0x0015824A
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x0015A068 File Offset: 0x00158268
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition_Default.position, this.CameraPosition_Default.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x0015A108 File Offset: 0x00158308
		public void Close()
		{
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x0015A184 File Offset: 0x00158384
		public bool IsIngredientCookable()
		{
			if (this.IngredientSlot.ItemInstance == null)
			{
				return false;
			}
			StorableItemDefinition storableItemDefinition = this.IngredientSlot.ItemInstance.Definition as StorableItemDefinition;
			return !(storableItemDefinition.StationItem == null) && storableItemDefinition.StationItem.HasModule<CookableModule>();
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x0015A1D1 File Offset: 0x001583D1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCookOperation(OvenCookOperation operation)
		{
			this.RpcWriter___Server_SendCookOperation_3708012700(operation);
			this.RpcLogic___SendCookOperation_3708012700(operation);
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x0015A1E8 File Offset: 0x001583E8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetCookOperation(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetCookOperation_2611294368(conn, operation, playButtonPress);
				this.RpcLogic___SetCookOperation_2611294368(conn, operation, playButtonPress);
			}
			else
			{
				this.RpcWriter___Target_SetCookOperation_2611294368(conn, operation, playButtonPress);
			}
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x0015A235 File Offset: 0x00158435
		public bool IsReadyToStart()
		{
			return this.IngredientSlot.Quantity > 0 && this.IsIngredientCookable() && this.CurrentOperation == null;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x0015A258 File Offset: 0x00158458
		public bool IsReadyForHarvest()
		{
			return this.CurrentOperation != null && this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration();
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x0015A27F File Offset: 0x0015847F
		public bool CanOutputSpaceFitCurrentOperation()
		{
			return this.CurrentOperation != null && this.OutputSlot.GetCapacityForItem(this.CurrentOperation.GetProductItem(1)) >= this.CurrentOperation.Cookable.ProductQuantity;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x0015A2B7 File Offset: 0x001584B7
		public void SetLiquidColor(Color col)
		{
			this.LiquidMesh.material.color = col;
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x0015A2CC File Offset: 0x001584CC
		private void UpdateLiquid()
		{
			if (this.CurrentOperation == null)
			{
				return;
			}
			if (this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
			{
				this.LiquidMesh.gameObject.SetActive(false);
				this.CookedLiquidMesh.gameObject.SetActive(true);
				return;
			}
			this.LiquidMesh.gameObject.SetActive(true);
			this.CookedLiquidMesh.gameObject.SetActive(false);
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x0015A340 File Offset: 0x00158540
		public StationItem[] CreateStationItems(int quantity = 1)
		{
			if (this.IngredientSlot.ItemInstance == null)
			{
				return null;
			}
			StorableItemDefinition storableItemDefinition = this.IngredientSlot.ItemInstance.Definition as StorableItemDefinition;
			if (storableItemDefinition.StationItem == null)
			{
				return null;
			}
			StationItem[] array;
			if (storableItemDefinition.StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Liquid)
			{
				StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, this.PourableContainer);
				stationItem.Initialize(storableItemDefinition);
				array = new StationItem[]
				{
					stationItem
				};
			}
			else
			{
				array = new StationItem[quantity];
				for (int i = 0; i < quantity; i++)
				{
					StationItem stationItem2 = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, this.ItemContainer);
					stationItem2.Initialize(storableItemDefinition);
					stationItem2.transform.position = this.SolidIngredientSpawnPoints[i].position;
					stationItem2.transform.rotation = this.SolidIngredientSpawnPoints[i].rotation;
					stationItem2.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 360f));
					array[i] = stationItem2;
				}
			}
			return array;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x0015A445 File Offset: 0x00158645
		public void ResetPourableContainer()
		{
			this.PourableContainer.localPosition = this.pourableContainerDefaultPos;
			this.PourableContainer.localRotation = this.pourableContainerDefaultRot;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x0015A469 File Offset: 0x00158669
		public void ResetSquareTray()
		{
			this.SquareTray.SetParent(this.WireTray.transform);
			this.SquareTray.localPosition = this.squareTrayDefaultPos;
			this.SquareTray.localRotation = this.squareTrayDefaultRot;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x0015A4A4 File Offset: 0x001586A4
		public LabOvenHammer CreateHammer()
		{
			LabOvenHammer component = UnityEngine.Object.Instantiate<GameObject>(this.HammerPrefab.gameObject, this.HammerSpawnPoint.position, this.HammerSpawnPoint.rotation).GetComponent<LabOvenHammer>();
			component.Rotator.CuntAssFuckingBitch = this.OafBastard;
			component.Constraint.Container = this.HammerContainer;
			component.transform.SetParent(this.HammerContainer);
			return component;
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x0015A510 File Offset: 0x00158710
		public void CreateImpactEffects(Vector3 point, bool playSound = true)
		{
			Vector3 vector = this.DecalContainer.InverseTransformPoint(point);
			vector.y = 0f;
			vector.x = Mathf.Clamp(vector.x, this.DecalMinBounds.localPosition.x, this.DecalMaxBounds.localPosition.x);
			vector.z = Mathf.Clamp(vector.z, this.DecalMinBounds.localPosition.z, this.DecalMaxBounds.localPosition.z);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SmashDecalPrefab, this.DecalContainer);
			gameObject.transform.localPosition = vector;
			this.decals.Add(gameObject);
			if (playSound)
			{
				this.ImpactSound.transform.position = point;
				this.ImpactSound.Play();
			}
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x0015A5E4 File Offset: 0x001587E4
		public void Shatter(int shardQuantity, GameObject shardPrefab)
		{
			this.CookedLiquidMesh.gameObject.SetActive(false);
			this.ShatterParticles.Play();
			this.ShatterSound.Play();
			this.ClearDecals();
			for (int i = 0; i < shardQuantity; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(shardPrefab.gameObject, NetworkSingleton<GameManager>.Instance.Temp);
				gameObject.transform.position = this.ShardSpawnPoints[i].position;
				gameObject.transform.rotation = this.ShardSpawnPoints[i].rotation;
				gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f, ForceMode.VelocityChange);
				gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, ForceMode.VelocityChange);
				this.shards.Add(gameObject);
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x0015A6B8 File Offset: 0x001588B8
		public void ClearShards()
		{
			for (int i = 0; i < this.shards.Count; i++)
			{
				UnityEngine.Object.Destroy(this.shards[i].gameObject);
			}
			this.shards.Clear();
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x0015A6FC File Offset: 0x001588FC
		public void ClearDecals()
		{
			for (int i = 0; i < this.decals.Count; i++)
			{
				UnityEngine.Object.Destroy(this.decals[i]);
			}
			this.decals.Clear();
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x0015A73B File Offset: 0x0015893B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x0015A764 File Offset: 0x00158964
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

		// Token: 0x0600523B RID: 21051 RVA: 0x0015A7C3 File Offset: 0x001589C3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x0015A7E1 File Offset: 0x001589E1
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x0015A7FF File Offset: 0x001589FF
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x0015A838 File Offset: 0x00158A38
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

		// Token: 0x0600523F RID: 21055 RVA: 0x0015A8B8 File Offset: 0x00158AB8
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
			LabOvenUIElement component = UnityEngine.Object.Instantiate<LabOvenUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<LabOvenUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x0015A94B File Offset: 0x00158B4B
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x0015A968 File Offset: 0x00158B68
		public override string GetSaveString()
		{
			string ingredientID = string.Empty;
			int currentIngredientQuantity = 0;
			EQuality ingredientQuality = EQuality.Standard;
			string productID = string.Empty;
			int currentCookProgress = 0;
			if (this.CurrentOperation != null)
			{
				ingredientID = this.CurrentOperation.IngredientID;
				currentIngredientQuantity = this.CurrentOperation.IngredientQuantity;
				ingredientQuality = this.CurrentOperation.IngredientQuality;
				productID = this.CurrentOperation.ProductID;
				currentCookProgress = this.CurrentOperation.CookProgress;
			}
			return new LabOvenData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.IngredientSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), ingredientID, currentIngredientQuantity, ingredientQuality, productID, currentCookProgress).GetJson(true);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x0015AA2C File Offset: 0x00158C2C
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

		// Token: 0x06005244 RID: 21060 RVA: 0x0015AAD8 File Offset: 0x00158CD8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3708012700));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetCookOperation_2611294368));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_SetCookOperation_2611294368));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.LabOven));
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x0015ACD1 File Offset: 0x00158ED1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x0015AD0B File Offset: 0x00158F0B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x0015AD1C File Offset: 0x00158F1C
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

		// Token: 0x06005248 RID: 21064 RVA: 0x0015ADC3 File Offset: 0x00158FC3
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x0015ADCC File Offset: 0x00158FCC
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

		// Token: 0x0600524A RID: 21066 RVA: 0x0015AE0C File Offset: 0x0015900C
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

		// Token: 0x0600524B RID: 21067 RVA: 0x0015AEB3 File Offset: 0x001590B3
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x0015AEBC File Offset: 0x001590BC
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

		// Token: 0x0600524D RID: 21069 RVA: 0x0015AEFC File Offset: 0x001590FC
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

		// Token: 0x0600524E RID: 21070 RVA: 0x0015AFA3 File Offset: 0x001591A3
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x0015AFAC File Offset: 0x001591AC
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

		// Token: 0x06005250 RID: 21072 RVA: 0x0015AFEC File Offset: 0x001591EC
		private void RpcWriter___Server_SendCookOperation_3708012700(OvenCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			base.SendServerRpc(11U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x0015B093 File Offset: 0x00159293
		public void RpcLogic___SendCookOperation_3708012700(OvenCookOperation operation)
		{
			this.SetCookOperation(null, operation, true);
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x0015B0A0 File Offset: 0x001592A0
		private void RpcReader___Server_SendCookOperation_3708012700(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			OvenCookOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3708012700(operation);
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x0015B0E0 File Offset: 0x001592E0
		private void RpcWriter___Observers_SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(playButtonPress);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x0015B1A4 File Offset: 0x001593A4
		public void RpcLogic___SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
		{
			this.CurrentOperation = operation;
			if (this.CurrentOperation == null)
			{
				this.LiquidMesh.gameObject.SetActive(false);
				this.CookedLiquidMesh.gameObject.SetActive(false);
				return;
			}
			CookableModule module = operation.Ingredient.StationItem.GetModule<CookableModule>();
			if (module == null)
			{
				return;
			}
			this.SetLiquidColor(module.LiquidColor);
			this.CookedLiquidMesh.material.color = module.SolidColor;
			this.UpdateLiquid();
			if (playButtonPress)
			{
				this.ButtonSound.Play();
			}
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x0015B234 File Offset: 0x00159434
		private void RpcReader___Observers_SetCookOperation_2611294368(PooledReader PooledReader0, Channel channel)
		{
			OvenCookOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			bool playButtonPress = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_2611294368(null, operation, playButtonPress);
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x0015B284 File Offset: 0x00159484
		private void RpcWriter___Target_SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(playButtonPress);
			base.SendTargetRpc(13U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x0015B348 File Offset: 0x00159548
		private void RpcReader___Target_SetCookOperation_2611294368(PooledReader PooledReader0, Channel channel)
		{
			OvenCookOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			bool playButtonPress = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_2611294368(base.LocalConnection, operation, playButtonPress);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x0015B390 File Offset: 0x00159590
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
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x0015B456 File Offset: 0x00159656
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x0015B480 File Offset: 0x00159680
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

		// Token: 0x0600525B RID: 21083 RVA: 0x0015B4E8 File Offset: 0x001596E8
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
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x0015B5B0 File Offset: 0x001597B0
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x0015B5DC File Offset: 0x001597DC
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

		// Token: 0x0600525E RID: 21086 RVA: 0x0015B630 File Offset: 0x00159830
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
			base.SendTargetRpc(16U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x0015B6F8 File Offset: 0x001598F8
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

		// Token: 0x06005260 RID: 21088 RVA: 0x0015B750 File Offset: 0x00159950
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
			base.SendServerRpc(17U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x0015B80E File Offset: 0x00159A0E
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x0015B818 File Offset: 0x00159A18
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

		// Token: 0x06005263 RID: 21091 RVA: 0x0015B874 File Offset: 0x00159A74
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x0015B941 File Offset: 0x00159B41
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x0015B958 File Offset: 0x00159B58
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

		// Token: 0x06005266 RID: 21094 RVA: 0x0015B9B0 File Offset: 0x00159BB0
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
			base.SendServerRpc(19U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x0015BA90 File Offset: 0x00159C90
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x0015BAC0 File Offset: 0x00159CC0
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

		// Token: 0x06005269 RID: 21097 RVA: 0x0015BB48 File Offset: 0x00159D48
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
			base.SendTargetRpc(20U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x0015BC29 File Offset: 0x00159E29
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x0015BC58 File Offset: 0x00159E58
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

		// Token: 0x0600526C RID: 21100 RVA: 0x0015BCD4 File Offset: 0x00159ED4
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
			base.SendObserversRpc(21U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x0015BDB8 File Offset: 0x00159FB8
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

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x0015BE2C File Offset: 0x0015A02C
		// (set) Token: 0x0600526F RID: 21103 RVA: 0x0015BE34 File Offset: 0x0015A034
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

		// Token: 0x06005270 RID: 21104 RVA: 0x0015BE70 File Offset: 0x0015A070
		public virtual bool LabOven(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06005271 RID: 21105 RVA: 0x0015BF4A File Offset: 0x0015A14A
		// (set) Token: 0x06005272 RID: 21106 RVA: 0x0015BF52 File Offset: 0x0015A152
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

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x0015BF8E File Offset: 0x0015A18E
		// (set) Token: 0x06005274 RID: 21108 RVA: 0x0015BF96 File Offset: 0x0015A196
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

		// Token: 0x06005275 RID: 21109 RVA: 0x0015BFD4 File Offset: 0x0015A1D4
		protected virtual void dll()
		{
			base.Awake();
			this.pourableContainerDefaultPos = this.PourableContainer.localPosition;
			this.pourableContainerDefaultRot = this.PourableContainer.localRotation;
			this.squareTrayDefaultPos = this.SquareTray.localPosition;
			this.squareTrayDefaultRot = this.SquareTray.localRotation;
			this.TimerLabel.enabled = false;
			if (!this.isGhost)
			{
				this.IngredientSlot.SetSlotOwner(this);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.InputVisuals.AddSlot(this.IngredientSlot, false);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.Add(this.IngredientSlot);
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003D43 RID: 15683
		public const int SOLID_INGREDIENT_COOK_LIMIT = 10;

		// Token: 0x04003D44 RID: 15684
		public const float FOV_OVERRIDE = 70f;

		// Token: 0x04003D47 RID: 15687
		public LabOven.ELightMode LightMode;

		// Token: 0x04003D48 RID: 15688
		[Header("References")]
		public Transform CameraPosition_Default;

		// Token: 0x04003D49 RID: 15689
		public Transform CameraPosition_Pour;

		// Token: 0x04003D4A RID: 15690
		public Transform CameraPosition_PlaceItems;

		// Token: 0x04003D4B RID: 15691
		public Transform CameraPosition_Breaking;

		// Token: 0x04003D4C RID: 15692
		public InteractableObject IntObj;

		// Token: 0x04003D4D RID: 15693
		public LabOvenDoor Door;

		// Token: 0x04003D4E RID: 15694
		public LabOvenWireTray WireTray;

		// Token: 0x04003D4F RID: 15695
		public ToggleableLight OvenLight;

		// Token: 0x04003D50 RID: 15696
		public LabOvenButton Button;

		// Token: 0x04003D51 RID: 15697
		public TextMeshPro TimerLabel;

		// Token: 0x04003D52 RID: 15698
		public ToggleableLight Light;

		// Token: 0x04003D53 RID: 15699
		public Transform PourableContainer;

		// Token: 0x04003D54 RID: 15700
		public Transform ItemContainer;

		// Token: 0x04003D55 RID: 15701
		public Animation PourAnimation;

		// Token: 0x04003D56 RID: 15702
		public SkinnedMeshRenderer LiquidMesh;

		// Token: 0x04003D57 RID: 15703
		public StorageVisualizer InputVisuals;

		// Token: 0x04003D58 RID: 15704
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003D59 RID: 15705
		public MeshRenderer CookedLiquidMesh;

		// Token: 0x04003D5A RID: 15706
		public Animation RemoveTrayAnimation;

		// Token: 0x04003D5B RID: 15707
		public Transform SquareTray;

		// Token: 0x04003D5C RID: 15708
		public Transform HammerSpawnPoint;

		// Token: 0x04003D5D RID: 15709
		public Transform HammerContainer;

		// Token: 0x04003D5E RID: 15710
		public Transform OafBastard;

		// Token: 0x04003D5F RID: 15711
		public Transform DecalContainer;

		// Token: 0x04003D60 RID: 15712
		public Transform DecalMaxBounds;

		// Token: 0x04003D61 RID: 15713
		public Transform DecalMinBounds;

		// Token: 0x04003D62 RID: 15714
		public BoxCollider CookedLiquidCollider;

		// Token: 0x04003D63 RID: 15715
		public Transform[] ShardSpawnPoints;

		// Token: 0x04003D64 RID: 15716
		public ParticleSystem ShatterParticles;

		// Token: 0x04003D65 RID: 15717
		public Transform uiPoint;

		// Token: 0x04003D66 RID: 15718
		public Transform[] accessPoints;

		// Token: 0x04003D67 RID: 15719
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003D68 RID: 15720
		public Transform[] SolidIngredientSpawnPoints;

		// Token: 0x04003D69 RID: 15721
		public BoxCollider TrayDetectionArea;

		// Token: 0x04003D6A RID: 15722
		[Header("Sounds")]
		public AudioSourceController ButtonSound;

		// Token: 0x04003D6B RID: 15723
		public AudioSourceController DingSound;

		// Token: 0x04003D6C RID: 15724
		public AudioSourceController RunLoopSound;

		// Token: 0x04003D6D RID: 15725
		public AudioSourceController ImpactSound;

		// Token: 0x04003D6E RID: 15726
		public AudioSourceController ShatterSound;

		// Token: 0x04003D6F RID: 15727
		[Header("UI")]
		public LabOvenUIElement WorldspaceUIPrefab;

		// Token: 0x04003D70 RID: 15728
		public Sprite typeIcon;

		// Token: 0x04003D71 RID: 15729
		[Header("Prefabs")]
		public LabOvenHammer HammerPrefab;

		// Token: 0x04003D72 RID: 15730
		public GameObject SmashDecalPrefab;

		// Token: 0x04003D75 RID: 15733
		public ItemSlot IngredientSlot;

		// Token: 0x04003D76 RID: 15734
		public ItemSlot OutputSlot;

		// Token: 0x04003D7E RID: 15742
		private Vector3 pourableContainerDefaultPos;

		// Token: 0x04003D7F RID: 15743
		private Quaternion pourableContainerDefaultRot;

		// Token: 0x04003D80 RID: 15744
		private Vector3 squareTrayDefaultPos;

		// Token: 0x04003D81 RID: 15745
		private Quaternion squareTrayDefaultRot;

		// Token: 0x04003D82 RID: 15746
		private List<GameObject> decals = new List<GameObject>();

		// Token: 0x04003D83 RID: 15747
		private List<GameObject> shards = new List<GameObject>();

		// Token: 0x04003D84 RID: 15748
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003D85 RID: 15749
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003D86 RID: 15750
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003D87 RID: 15751
		private bool dll_Excuted;

		// Token: 0x04003D88 RID: 15752
		private bool dll_Excuted;

		// Token: 0x02000BA7 RID: 2983
		public enum ELightMode
		{
			// Token: 0x04003D8A RID: 15754
			Off,
			// Token: 0x04003D8B RID: 15755
			On,
			// Token: 0x04003D8C RID: 15756
			Flash
		}

		// Token: 0x02000BA8 RID: 2984
		public enum EState
		{
			// Token: 0x04003D8E RID: 15758
			CanBegin,
			// Token: 0x04003D8F RID: 15759
			MissingItems,
			// Token: 0x04003D90 RID: 15760
			InsufficentProduct,
			// Token: 0x04003D91 RID: 15761
			OutputSlotFull,
			// Token: 0x04003D92 RID: 15762
			Mismatch
		}
	}
}
