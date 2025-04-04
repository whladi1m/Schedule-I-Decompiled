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
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB2 RID: 2994
	public class MixingStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060052C2 RID: 21186 RVA: 0x0015CB4E File Offset: 0x0015AD4E
		// (set) Token: 0x060052C3 RID: 21187 RVA: 0x0015CB56 File Offset: 0x0015AD56
		public bool IsOpen { get; private set; }

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060052C4 RID: 21188 RVA: 0x0015CB5F File Offset: 0x0015AD5F
		// (set) Token: 0x060052C5 RID: 21189 RVA: 0x0015CB67 File Offset: 0x0015AD67
		public MixOperation CurrentMixOperation { get; set; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060052C6 RID: 21190 RVA: 0x0015CB70 File Offset: 0x0015AD70
		public bool IsMixingDone
		{
			get
			{
				return this.CurrentMixOperation != null && this.CurrentMixTime >= this.GetMixTimeForCurrentOperation();
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060052C7 RID: 21191 RVA: 0x0015CB8D File Offset: 0x0015AD8D
		// (set) Token: 0x060052C8 RID: 21192 RVA: 0x0015CB95 File Offset: 0x0015AD95
		public int CurrentMixTime { get; protected set; }

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060052C9 RID: 21193 RVA: 0x0015CB9E File Offset: 0x0015AD9E
		// (set) Token: 0x060052CA RID: 21194 RVA: 0x0015CBA6 File Offset: 0x0015ADA6
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060052CB RID: 21195 RVA: 0x0015CBAF File Offset: 0x0015ADAF
		// (set) Token: 0x060052CC RID: 21196 RVA: 0x0015CBB7 File Offset: 0x0015ADB7
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

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060052CD RID: 21197 RVA: 0x0015CBC1 File Offset: 0x0015ADC1
		// (set) Token: 0x060052CE RID: 21198 RVA: 0x0015CBC9 File Offset: 0x0015ADC9
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

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060052CF RID: 21199 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060052D0 RID: 21200 RVA: 0x0015CBD3 File Offset: 0x0015ADD3
		// (set) Token: 0x060052D1 RID: 21201 RVA: 0x0015CBDB File Offset: 0x0015ADDB
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060052D2 RID: 21202 RVA: 0x0015CBE4 File Offset: 0x0015ADE4
		// (set) Token: 0x060052D3 RID: 21203 RVA: 0x0015CBEC File Offset: 0x0015ADEC
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x0015CBF5 File Offset: 0x0015ADF5
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060052D5 RID: 21205 RVA: 0x0015CBFD File Offset: 0x0015ADFD
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060052D6 RID: 21206 RVA: 0x0015CC05 File Offset: 0x0015AE05
		public bool Selectable { get; } = 1;

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060052D7 RID: 21207 RVA: 0x0015CC0D File Offset: 0x0015AE0D
		// (set) Token: 0x060052D8 RID: 21208 RVA: 0x0015CC15 File Offset: 0x0015AE15
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060052D9 RID: 21209 RVA: 0x0015CC1E File Offset: 0x0015AE1E
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060052DA RID: 21210 RVA: 0x0015CC26 File Offset: 0x0015AE26
		// (set) Token: 0x060052DB RID: 21211 RVA: 0x0015CC2E File Offset: 0x0015AE2E
		protected MixingStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060052DC RID: 21212 RVA: 0x0015CC37 File Offset: 0x0015AE37
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x060052DD RID: 21213 RVA: 0x0015CC3F File Offset: 0x0015AE3F
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.MixingStation;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x060052DE RID: 21214 RVA: 0x0015CC43 File Offset: 0x0015AE43
		// (set) Token: 0x060052DF RID: 21215 RVA: 0x0015CC4B File Offset: 0x0015AE4B
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x060052E0 RID: 21216 RVA: 0x0015CC54 File Offset: 0x0015AE54
		// (set) Token: 0x060052E1 RID: 21217 RVA: 0x0015CC5C File Offset: 0x0015AE5C
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

		// Token: 0x060052E2 RID: 21218 RVA: 0x0015CC66 File Offset: 0x0015AE66
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060052E3 RID: 21219 RVA: 0x0015CC7C File Offset: 0x0015AE7C
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060052E5 RID: 21221 RVA: 0x0015CBF5 File Offset: 0x0015ADF5
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x0015CC84 File Offset: 0x0015AE84
		// (set) Token: 0x060052E8 RID: 21224 RVA: 0x0015CC8C File Offset: 0x0015AE8C
		public Vector3 DiscoveryBoxOffset { get; private set; }

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x0015CC95 File Offset: 0x0015AE95
		// (set) Token: 0x060052EA RID: 21226 RVA: 0x0015CC9D File Offset: 0x0015AE9D
		public Quaternion DiscoveryBoxRotation { get; private set; }

		// Token: 0x060052EB RID: 21227 RVA: 0x0015CCA8 File Offset: 0x0015AEA8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.MixingStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x0015CCC8 File Offset: 0x0015AEC8
		protected override void Start()
		{
			base.Start();
			if (!this.isGhost)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
				if (this.StartButton != null)
				{
					this.StartButton.onClickStart.AddListener(new UnityAction<RaycastHit>(this.StartButtonClicked));
				}
			}
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x0015CD5C File Offset: 0x0015AF5C
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
				this.stationConfiguration = new MixingStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
			}
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x0015CDAD File Offset: 0x0015AFAD
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
			if (this.CurrentMixOperation != null)
			{
				this.SetMixOperation(connection, this.CurrentMixOperation, this.CurrentMixTime);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x0015CDE0 File Offset: 0x0015AFE0
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			MixingStation.<>c__DisplayClass121_0 CS$<>8__locals1 = new MixingStation.<>c__DisplayClass121_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x0015CE20 File Offset: 0x0015B020
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			if (this.CurrentMixOperation != null && this.IsMixingDone)
			{
				reason = "Contains items";
				return false;
			}
			if (this.CurrentMixOperation != null)
			{
				reason = "Mixing in progress";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x0015CE84 File Offset: 0x0015B084
		public override void DestroyItem(bool callOnServer = true)
		{
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

		// Token: 0x060052F2 RID: 21234 RVA: 0x0015CF0C File Offset: 0x0015B10C
		private void TimeSkipped(int minsPassed)
		{
			for (int i = 0; i < minsPassed; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x0015CF2C File Offset: 0x0015B12C
		protected virtual void MinPass()
		{
			if (this.CurrentMixOperation != null || this.OutputSlot.Quantity > 0)
			{
				int num = 0;
				if (this.CurrentMixOperation != null)
				{
					int currentMixTime = this.CurrentMixTime;
					int currentMixTime2 = this.CurrentMixTime;
					this.CurrentMixTime = currentMixTime2 + 1;
					num = this.GetMixTimeForCurrentOperation();
					if (this.CurrentMixTime >= num && currentMixTime < num && InstanceFinder.IsServer)
					{
						NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Completed", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Completed") + 1f).ToString(), true);
						this.MixingDone_Networked();
					}
				}
				if (this.Clock != null)
				{
					this.Clock.SetScreenLit(true);
					this.Clock.DisplayMinutes(Mathf.Max(num - this.CurrentMixTime, 0));
				}
				if (this.Light != null)
				{
					if (this.IsMixingDone)
					{
						this.Light.isOn = (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.DailyMinTotal % 2 == 0);
						return;
					}
					this.Light.isOn = true;
					return;
				}
			}
			else
			{
				if (this.Clock != null)
				{
					this.Clock.SetScreenLit(false);
					this.Clock.DisplayText(string.Empty);
				}
				if (this.Light != null && this.IsMixingDone)
				{
					this.Light.isOn = false;
				}
			}
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x0015D07E File Offset: 0x0015B27E
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixingOperation(MixOperation operation, int mixTime)
		{
			this.RpcWriter___Server_SendMixingOperation_2669582547(operation, mixTime);
			this.RpcLogic___SendMixingOperation_2669582547(operation, mixTime);
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x0015D09C File Offset: 0x0015B29C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void SetMixOperation(NetworkConnection conn, MixOperation operation, int mixTIme)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetMixOperation_1073078804(conn, operation, mixTIme);
				this.RpcLogic___SetMixOperation_1073078804(conn, operation, mixTIme);
			}
			else
			{
				this.RpcWriter___Target_SetMixOperation_1073078804(conn, operation, mixTIme);
			}
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x0015D0E9 File Offset: 0x0015B2E9
		public virtual void MixingStart()
		{
			this.StartSound.Play();
			this.MachineSound.StartAudio();
			if (this.onMixStart != null)
			{
				this.onMixStart.Invoke();
			}
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0015D114 File Offset: 0x0015B314
		[ObserversRpc]
		public void MixingDone_Networked()
		{
			this.RpcWriter___Observers_MixingDone_Networked_2166136261();
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0015D11C File Offset: 0x0015B31C
		public virtual void MixingDone()
		{
			this.MachineSound.StopAudio();
			this.StopSound.Play();
			this.TryCreateOutputItems();
			if (this.onMixDone != null)
			{
				this.onMixDone.Invoke();
			}
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0015D150 File Offset: 0x0015B350
		public bool DoesOutputHaveSpace(StationRecipe recipe)
		{
			StorableItemInstance productInstance = recipe.GetProductInstance(this.GetIngredients());
			return this.OutputSlot.GetCapacityForItem(productInstance) >= 1;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x0015D17C File Offset: 0x0015B37C
		public List<ItemInstance> GetIngredients()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			if (this.ProductSlot.ItemInstance != null)
			{
				list.Add(this.ProductSlot.ItemInstance);
			}
			if (this.MixerSlot.ItemInstance != null)
			{
				list.Add(this.MixerSlot.ItemInstance);
			}
			return list;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0015D1CC File Offset: 0x0015B3CC
		public int GetMixQuantity()
		{
			if (this.GetProduct() == null || this.GetMixer() == null)
			{
				return 0;
			}
			return Mathf.Min(Mathf.Min(this.ProductSlot.Quantity, this.MixerSlot.Quantity), this.MaxMixQuantity);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0015D21D File Offset: 0x0015B41D
		public bool CanStartMix()
		{
			return this.GetMixQuantity() > 0 && this.OutputSlot.Quantity == 0;
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x0015D238 File Offset: 0x0015B438
		public ProductDefinition GetProduct()
		{
			if (this.ProductSlot.ItemInstance != null)
			{
				return this.ProductSlot.ItemInstance.Definition as ProductDefinition;
			}
			return null;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0015D260 File Offset: 0x0015B460
		public PropertyItemDefinition GetMixer()
		{
			if (this.MixerSlot.ItemInstance != null)
			{
				PropertyItemDefinition propertyItemDefinition = this.MixerSlot.ItemInstance.Definition as PropertyItemDefinition;
				if (propertyItemDefinition != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(propertyItemDefinition))
				{
					return propertyItemDefinition;
				}
			}
			return null;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x0015D2AE File Offset: 0x0015B4AE
		public int GetMixTimeForCurrentOperation()
		{
			if (this.CurrentMixOperation == null)
			{
				return 0;
			}
			return this.MixTimePerItem * this.CurrentMixOperation.Quantity;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x0015D2CC File Offset: 0x0015B4CC
		[ServerRpc(RequireOwnership = false)]
		public void TryCreateOutputItems()
		{
			this.RpcWriter___Server_TryCreateOutputItems_2166136261();
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0015D2DF File Offset: 0x0015B4DF
		public void SetStartButtonClickable(bool clickable)
		{
			this.StartButton.ClickableEnabled = clickable;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x0015D2F0 File Offset: 0x0015B4F0
		private void OutputChanged()
		{
			if (this.OutputSlot.Quantity == 0)
			{
				if (this.onOutputCollected != null)
				{
					this.onOutputCollected.Invoke();
				}
				if (InstanceFinder.IsServer)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Collected", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Collected") + 1f).ToString(), true);
				}
			}
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x0015D351 File Offset: 0x0015B551
		private void StartButtonClicked(RaycastHit hit)
		{
			this.SetStartButtonClickable(false);
			if (this.onStartButtonClicked != null)
			{
				this.onStartButtonClicked.Invoke();
			}
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x0015D370 File Offset: 0x0015B570
		public void Open()
		{
			this.IsOpen = true;
			if (this.CurrentMixOperation != null && this.IsMixingDone)
			{
				this.TryCreateOutputItems();
			}
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<MixingStationCanvas>.Instance.Open(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x0015D438 File Offset: 0x0015B638
		public void Close()
		{
			this.IsOpen = false;
			this.SetPlayerUser(null);
			if (this.DiscoveryBox != null)
			{
				this.DiscoveryBox.transform.SetParent(this.CameraPosition.transform);
				this.DiscoveryBox.gameObject.SetActive(false);
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				Singleton<CompassManager>.Instance.SetVisible(true);
			}
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x0015D4F0 File Offset: 0x0015B6F0
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

		// Token: 0x06005307 RID: 21255 RVA: 0x0015D54A File Offset: 0x0015B74A
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x0015D568 File Offset: 0x0015B768
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
			MixingStationUIElement component = UnityEngine.Object.Instantiate<MixingStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<MixingStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x0015D5FB File Offset: 0x0015B7FB
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x0015D616 File Offset: 0x0015B816
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x0015D63C File Offset: 0x0015B83C
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

		// Token: 0x0600530C RID: 21260 RVA: 0x0015D69B File Offset: 0x0015B89B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x0015D6B9 File Offset: 0x0015B8B9
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x0015D6D7 File Offset: 0x0015B8D7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x0015D710 File Offset: 0x0015B910
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

		// Token: 0x06005310 RID: 21264 RVA: 0x0015D790 File Offset: 0x0015B990
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x0015D7B1 File Offset: 0x0015B9B1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x0015D7C8 File Offset: 0x0015B9C8
		public override string GetSaveString()
		{
			return new MixingStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.ProductSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.MixerSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.CurrentMixOperation, this.CurrentMixTime).GetJson(true);
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x0015D850 File Offset: 0x0015BA50
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

		// Token: 0x06005318 RID: 21272 RVA: 0x0015D900 File Offset: 0x0015BB00
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendMixingOperation_2669582547));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_SetMixOperation_1073078804));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetMixOperation_1073078804));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_MixingDone_Networked_2166136261));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_TryCreateOutputItems_2166136261));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(23U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.MixingStation));
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x0015DB27 File Offset: 0x0015BD27
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x0015DB61 File Offset: 0x0015BD61
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x0015DB70 File Offset: 0x0015BD70
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

		// Token: 0x0600531C RID: 21276 RVA: 0x0015DC17 File Offset: 0x0015BE17
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x0015DC20 File Offset: 0x0015BE20
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

		// Token: 0x0600531E RID: 21278 RVA: 0x0015DC60 File Offset: 0x0015BE60
		private void RpcWriter___Server_SendMixingOperation_2669582547(MixOperation operation, int mixTime)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTime, AutoPackType.Packed);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x0015DD19 File Offset: 0x0015BF19
		public void RpcLogic___SendMixingOperation_2669582547(MixOperation operation, int mixTime)
		{
			this.SetMixOperation(null, operation, mixTime);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x0015DD24 File Offset: 0x0015BF24
		private void RpcReader___Server_SendMixingOperation_2669582547(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			MixOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTime = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixingOperation_2669582547(operation, mixTime);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x0015DD78 File Offset: 0x0015BF78
		private void RpcWriter___Observers_SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTIme, AutoPackType.Packed);
			base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x0015DE40 File Offset: 0x0015C040
		public virtual void RpcLogic___SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
		{
			if (operation != null && string.IsNullOrEmpty(operation.ProductID))
			{
				operation = null;
			}
			MixOperation currentMixOperation = this.CurrentMixOperation;
			this.CurrentMixOperation = operation;
			this.CurrentMixTime = mixTIme;
			if (operation != null)
			{
				if (currentMixOperation == null)
				{
					this.MixingStart();
					return;
				}
			}
			else if (currentMixOperation != null && this.onMixDone != null)
			{
				this.onMixDone.Invoke();
			}
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x0015DE98 File Offset: 0x0015C098
		private void RpcReader___Observers_SetMixOperation_1073078804(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTIme = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_1073078804(null, operation, mixTIme);
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x0015DEEC File Offset: 0x0015C0EC
		private void RpcWriter___Target_SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTIme, AutoPackType.Packed);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x0015DFB4 File Offset: 0x0015C1B4
		private void RpcReader___Target_SetMixOperation_1073078804(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTIme = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_1073078804(base.LocalConnection, operation, mixTIme);
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x0015E004 File Offset: 0x0015C204
		private void RpcWriter___Observers_MixingDone_Networked_2166136261()
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

		// Token: 0x06005327 RID: 21287 RVA: 0x0015E0AD File Offset: 0x0015C2AD
		public void RpcLogic___MixingDone_Networked_2166136261()
		{
			this.MixingDone();
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x0015E0B8 File Offset: 0x0015C2B8
		private void RpcReader___Observers_MixingDone_Networked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___MixingDone_Networked_2166136261();
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x0015E0D8 File Offset: 0x0015C2D8
		private void RpcWriter___Server_TryCreateOutputItems_2166136261()
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
			base.SendServerRpc(13U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x0015E174 File Offset: 0x0015C374
		public void RpcLogic___TryCreateOutputItems_2166136261()
		{
			if (this.CurrentMixOperation == null)
			{
				return;
			}
			ProductDefinition productDefinition;
			if (this.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				QualityItemInstance qualityItemInstance = productDefinition.GetDefaultInstance(this.CurrentMixOperation.Quantity) as QualityItemInstance;
				qualityItemInstance.SetQuality(this.CurrentMixOperation.ProductQuality);
				this.OutputSlot.AddItem(qualityItemInstance, false);
				if (NetworkSingleton<ProductManager>.Instance.GetRecipe(this.CurrentMixOperation.ProductID, this.CurrentMixOperation.IngredientID) == null)
				{
					NetworkSingleton<ProductManager>.Instance.SendMixRecipe(this.CurrentMixOperation.ProductID, this.CurrentMixOperation.IngredientID, qualityItemInstance.ID);
				}
				this.SetMixOperation(null, null, 0);
			}
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x0015E228 File Offset: 0x0015C428
		private void RpcReader___Server_TryCreateOutputItems_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___TryCreateOutputItems_2166136261();
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x0015E248 File Offset: 0x0015C448
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

		// Token: 0x0600532D RID: 21293 RVA: 0x0015E30E File Offset: 0x0015C50E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x0015E338 File Offset: 0x0015C538
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

		// Token: 0x0600532F RID: 21295 RVA: 0x0015E3A0 File Offset: 0x0015C5A0
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

		// Token: 0x06005330 RID: 21296 RVA: 0x0015E468 File Offset: 0x0015C668
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x0015E494 File Offset: 0x0015C694
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

		// Token: 0x06005332 RID: 21298 RVA: 0x0015E4E8 File Offset: 0x0015C6E8
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

		// Token: 0x06005333 RID: 21299 RVA: 0x0015E5B0 File Offset: 0x0015C7B0
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

		// Token: 0x06005334 RID: 21300 RVA: 0x0015E608 File Offset: 0x0015C808
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

		// Token: 0x06005335 RID: 21301 RVA: 0x0015E6C6 File Offset: 0x0015C8C6
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x0015E6D0 File Offset: 0x0015C8D0
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

		// Token: 0x06005337 RID: 21303 RVA: 0x0015E72C File Offset: 0x0015C92C
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

		// Token: 0x06005338 RID: 21304 RVA: 0x0015E7F9 File Offset: 0x0015C9F9
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x0015E810 File Offset: 0x0015CA10
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

		// Token: 0x0600533A RID: 21306 RVA: 0x0015E868 File Offset: 0x0015CA68
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

		// Token: 0x0600533B RID: 21307 RVA: 0x0015E948 File Offset: 0x0015CB48
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0015E978 File Offset: 0x0015CB78
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

		// Token: 0x0600533D RID: 21309 RVA: 0x0015EA00 File Offset: 0x0015CC00
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

		// Token: 0x0600533E RID: 21310 RVA: 0x0015EAE1 File Offset: 0x0015CCE1
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x0015EB10 File Offset: 0x0015CD10
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

		// Token: 0x06005340 RID: 21312 RVA: 0x0015EB8C File Offset: 0x0015CD8C
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

		// Token: 0x06005341 RID: 21313 RVA: 0x0015EC70 File Offset: 0x0015CE70
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

		// Token: 0x06005342 RID: 21314 RVA: 0x0015ECE4 File Offset: 0x0015CEE4
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
			base.SendServerRpc(22U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x0015ED8C File Offset: 0x0015CF8C
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x0015EDE0 File Offset: 0x0015CFE0
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

		// Token: 0x06005345 RID: 21317 RVA: 0x0015EE20 File Offset: 0x0015D020
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
			base.SendServerRpc(23U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x0015EEC7 File Offset: 0x0015D0C7
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0015EED0 File Offset: 0x0015D0D0
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

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06005348 RID: 21320 RVA: 0x0015EF0E File Offset: 0x0015D10E
		// (set) Token: 0x06005349 RID: 21321 RVA: 0x0015EF16 File Offset: 0x0015D116
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

		// Token: 0x0600534A RID: 21322 RVA: 0x0015EF54 File Offset: 0x0015D154
		public virtual bool MixingStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600534B RID: 21323 RVA: 0x0015F02E File Offset: 0x0015D22E
		// (set) Token: 0x0600534C RID: 21324 RVA: 0x0015F036 File Offset: 0x0015D236
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

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600534D RID: 21325 RVA: 0x0015F072 File Offset: 0x0015D272
		// (set) Token: 0x0600534E RID: 21326 RVA: 0x0015F07A File Offset: 0x0015D27A
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

		// Token: 0x0600534F RID: 21327 RVA: 0x0015F0B8 File Offset: 0x0015D2B8
		protected virtual void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.ProductSlot.AddFilter(new ItemFilter_UnpackagedProduct());
				this.ProductSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.ProductSlot, false);
				ItemSlot productSlot = this.ProductSlot;
				productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.MixerSlot.AddFilter(new ItemFilter_MixingIngredient());
				this.MixerSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.MixerSlot, false);
				ItemSlot mixerSlot = this.MixerSlot;
				mixerSlot.onItemDataChanged = (Action)Delegate.Combine(mixerSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				ItemSlot outputSlot = this.OutputSlot;
				outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				ItemSlot outputSlot2 = this.OutputSlot;
				outputSlot2.onItemDataChanged = (Action)Delegate.Combine(outputSlot2.onItemDataChanged, new Action(this.OutputChanged));
				this.DiscoveryBoxOffset = this.DiscoveryBox.transform.localPosition;
				this.DiscoveryBoxRotation = this.DiscoveryBox.transform.localRotation;
				this.InputSlots.AddRange(new List<ItemSlot>
				{
					this.ProductSlot,
					this.MixerSlot
				});
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003DE1 RID: 15841
		public ItemSlot ProductSlot;

		// Token: 0x04003DE2 RID: 15842
		public ItemSlot MixerSlot;

		// Token: 0x04003DE3 RID: 15843
		public ItemSlot OutputSlot;

		// Token: 0x04003DE7 RID: 15847
		public bool RequiresIngredientInsertion = true;

		// Token: 0x04003DEF RID: 15855
		[Header("Settings")]
		public int MixTimePerItem = 15;

		// Token: 0x04003DF0 RID: 15856
		public int MaxMixQuantity = 10;

		// Token: 0x04003DF1 RID: 15857
		[Header("Prefabs")]
		public GameObject JugPrefab;

		// Token: 0x04003DF2 RID: 15858
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04003DF3 RID: 15859
		public Transform CameraPosition;

		// Token: 0x04003DF4 RID: 15860
		public Transform CameraPosition_CombineIngredients;

		// Token: 0x04003DF5 RID: 15861
		public Transform CameraPosition_StartMachine;

		// Token: 0x04003DF6 RID: 15862
		public StorageVisualizer InputVisuals;

		// Token: 0x04003DF7 RID: 15863
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003DF8 RID: 15864
		public DigitalAlarm Clock;

		// Token: 0x04003DF9 RID: 15865
		public ToggleableLight Light;

		// Token: 0x04003DFA RID: 15866
		public NewMixDiscoveryBox DiscoveryBox;

		// Token: 0x04003DFB RID: 15867
		public Transform ItemContainer;

		// Token: 0x04003DFC RID: 15868
		public Transform[] IngredientTransforms;

		// Token: 0x04003DFD RID: 15869
		public Fillable BowlFillable;

		// Token: 0x04003DFE RID: 15870
		public Clickable StartButton;

		// Token: 0x04003DFF RID: 15871
		public Transform JugAlignment;

		// Token: 0x04003E00 RID: 15872
		public Rigidbody Anchor;

		// Token: 0x04003E01 RID: 15873
		public BoxCollider TrashSpawnVolume;

		// Token: 0x04003E02 RID: 15874
		public Transform uiPoint;

		// Token: 0x04003E03 RID: 15875
		public Transform[] accessPoints;

		// Token: 0x04003E04 RID: 15876
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003E05 RID: 15877
		[Header("Sounds")]
		public StartLoopStopAudio MachineSound;

		// Token: 0x04003E06 RID: 15878
		public AudioSourceController StartSound;

		// Token: 0x04003E07 RID: 15879
		public AudioSourceController StopSound;

		// Token: 0x04003E08 RID: 15880
		[Header("Mix Timing")]
		[Header("UI")]
		public MixingStationUIElement WorldspaceUIPrefab;

		// Token: 0x04003E09 RID: 15881
		public Sprite typeIcon;

		// Token: 0x04003E0A RID: 15882
		public UnityEvent onMixStart;

		// Token: 0x04003E0B RID: 15883
		public UnityEvent onMixDone;

		// Token: 0x04003E0C RID: 15884
		public UnityEvent onOutputCollected;

		// Token: 0x04003E0D RID: 15885
		public UnityEvent onStartButtonClicked;

		// Token: 0x04003E10 RID: 15888
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003E11 RID: 15889
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003E12 RID: 15890
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003E13 RID: 15891
		private bool dll_Excuted;

		// Token: 0x04003E14 RID: 15892
		private bool dll_Excuted;
	}
}
