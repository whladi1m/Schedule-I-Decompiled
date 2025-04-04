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
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.Trash;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x02000631 RID: 1585
	public class Cleaner : Employee, IConfigurable
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000AF226 File Offset: 0x000AD426
		// (set) Token: 0x06002A74 RID: 10868 RVA: 0x000AF22E File Offset: 0x000AD42E
		public TrashGrabberInstance trashGrabberInstance { get; private set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002A75 RID: 10869 RVA: 0x000AF237 File Offset: 0x000AD437
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000AF23F File Offset: 0x000AD43F
		// (set) Token: 0x06002A77 RID: 10871 RVA: 0x000AF247 File Offset: 0x000AD447
		protected CleanerConfiguration configuration { get; set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000AF250 File Offset: 0x000AD450
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002A79 RID: 10873 RVA: 0x000AF258 File Offset: 0x000AD458
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Cleaner;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000AF25B File Offset: 0x000AD45B
		// (set) Token: 0x06002A7B RID: 10875 RVA: 0x000AF263 File Offset: 0x000AD463
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000AF26C File Offset: 0x000AD46C
		// (set) Token: 0x06002A7D RID: 10877 RVA: 0x000AF274 File Offset: 0x000AD474
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

		// Token: 0x06002A7E RID: 10878 RVA: 0x000AF27E File Offset: 0x000AD47E
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000AF294 File Offset: 0x000AD494
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000AF29C File Offset: 0x000AD49C
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000AD07F File Offset: 0x000AB27F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000AF2A4 File Offset: 0x000AD4A4
		protected override void AssignProperty(Property prop)
		{
			base.AssignProperty(prop);
			prop.AddConfigurable(this);
			this.configuration = new CleanerConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000AF2CE File Offset: 0x000AD4CE
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

		// Token: 0x06002A86 RID: 10886 RVA: 0x000AF309 File Offset: 0x000AD509
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000AF31C File Offset: 0x000AD51C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Cleaner.<>c__DisplayClass46_0 CS$<>8__locals1 = new Cleaner.<>c__DisplayClass46_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000AF35C File Offset: 0x000AD55C
		protected override void MinPass()
		{
			base.MinPass();
			if (Singleton<LoadManager>.Instance.IsLoading)
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
			if (this.configuration.binItems.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any trash cans", "You can use your management clipboards to assign trash cans to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.TryStartNewTask();
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000AF3DC File Offset: 0x000AD5DC
		private void TryStartNewTask()
		{
			TrashContainerItem[] trashContainersOrderedByDistance = this.GetTrashContainersOrderedByDistance();
			this.EnsureTrashGrabberInInventory();
			foreach (TrashContainerItem trashContainerItem in trashContainersOrderedByDistance)
			{
				if (trashContainerItem.TrashBagsInRadius.Count > 0)
				{
					if (base.AssignedProperty.DisposalArea != null)
					{
						TrashBag targetBag = trashContainerItem.TrashBagsInRadius[0];
						this.DisposeTrashBagBehaviour.SetTargetBag(targetBag);
						this.DisposeTrashBagBehaviour.Enable_Networked(null);
						return;
					}
					Console.LogError("No disposal area assigned to property " + base.AssignedProperty.PropertyCode, null);
				}
			}
			if (this.GetTrashGrabberAmount() < 20)
			{
				foreach (TrashContainerItem trashContainerItem2 in trashContainersOrderedByDistance)
				{
					if (trashContainerItem2.TrashItemsInRadius.Count > 0)
					{
						int num = 0;
						TrashItem trashItem = trashContainerItem2.TrashItemsInRadius[num];
						while (trashItem == null || !this.movement.CanGetTo(trashItem.transform.position, 1f))
						{
							num++;
							if (num >= trashContainerItem2.TrashItemsInRadius.Count)
							{
								trashItem = null;
								break;
							}
							trashItem = trashContainerItem2.TrashItemsInRadius[num];
						}
						if (trashItem != null)
						{
							this.PickUpTrashBehaviour.SetTargetTrash(trashItem);
							this.PickUpTrashBehaviour.Enable_Networked(null);
							return;
						}
					}
				}
			}
			if (this.GetTrashGrabberAmount() >= 20 && this.GetFirstNonFullBin(trashContainersOrderedByDistance) != null)
			{
				this.EmptyTrashGrabberBehaviour.SetTargetTrashCan(this.GetFirstNonFullBin(trashContainersOrderedByDistance));
				this.EmptyTrashGrabberBehaviour.Enable_Networked(null);
				return;
			}
			foreach (TrashContainerItem trashContainerItem3 in trashContainersOrderedByDistance)
			{
				if (trashContainerItem3.Container.NormalizedTrashLevel >= 0.75f)
				{
					this.BagTrashCanBehaviour.SetTargetTrashCan(trashContainerItem3);
					this.BagTrashCanBehaviour.Enable_Networked(null);
					return;
				}
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000AF5C6 File Offset: 0x000AD7C6
		private TrashContainerItem GetFirstNonFullBin(TrashContainerItem[] bins)
		{
			return bins.FirstOrDefault((TrashContainerItem bin) => bin.Container.NormalizedTrashLevel < 1f);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000AF5ED File Offset: 0x000AD7ED
		public override void SetIdle(bool idle)
		{
			base.SetIdle(idle);
			if (idle && this.Avatar.CurrentEquippable != null)
			{
				base.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000AF618 File Offset: 0x000AD818
		private TrashContainerItem[] GetTrashContainersOrderedByDistance()
		{
			TrashContainerItem[] array = this.configuration.binItems.ToArray();
			Array.Sort<TrashContainerItem>(array, delegate(TrashContainerItem x, TrashContainerItem y)
			{
				float num = Vector3.Distance(x.transform.position, base.transform.position);
				float value = Vector3.Distance(y.transform.position, base.transform.position);
				return num.CompareTo(value);
			});
			return array;
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000AF63C File Offset: 0x000AD83C
		public override BedItem GetBed()
		{
			return this.configuration.bedItem;
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000AF64C File Offset: 0x000AD84C
		private void EnsureTrashGrabberInInventory()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Inventory._GetItemAmount(this.TrashGrabberDef.ID) == 0)
			{
				base.Inventory.InsertItem(this.TrashGrabberDef.GetDefaultInstance(1), true);
			}
			this.trashGrabberInstance = (base.Inventory.GetFirstItem(this.TrashGrabberDef.ID, null) as TrashGrabberInstance);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000AF6B4 File Offset: 0x000AD8B4
		private bool AnyWorkInProgress()
		{
			return this.PickUpTrashBehaviour.Active || this.EmptyTrashGrabberBehaviour.Active || this.BagTrashCanBehaviour.Active || this.DisposeTrashBagBehaviour.Active || this.MoveItemBehaviour.Active;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000AF70D File Offset: 0x000AD90D
		private int GetTrashGrabberAmount()
		{
			return this.trashGrabberInstance.GetTotalSize();
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000AF71C File Offset: 0x000AD91C
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
			CleanerUIElement component = UnityEngine.Object.Instantiate<CleanerUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<CleanerUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000AF7A7 File Offset: 0x000AD9A7
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000AF7C4 File Offset: 0x000AD9C4
		public override string GetSaveString()
		{
			return new CleanerData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData()).GetJson(true);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000AF834 File Offset: 0x000ADA34
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

		// Token: 0x06002A97 RID: 10903 RVA: 0x000AF8D8 File Offset: 0x000ADAD8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Cleaner));
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000AF950 File Offset: 0x000ADB50
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000AF974 File Offset: 0x000ADB74
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000AF984 File Offset: 0x000ADB84
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

		// Token: 0x06002A9B RID: 10907 RVA: 0x000AFA2B File Offset: 0x000ADC2B
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000AFA34 File Offset: 0x000ADC34
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

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x000AFA72 File Offset: 0x000ADC72
		// (set) Token: 0x06002A9E RID: 10910 RVA: 0x000AFA7A File Offset: 0x000ADC7A
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

		// Token: 0x06002A9F RID: 10911 RVA: 0x000AFAB8 File Offset: 0x000ADCB8
		public virtual bool Cleaner(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000AFB0A File Offset: 0x000ADD0A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001EE7 RID: 7911
		public const int MAX_ASSIGNED_BINS = 3;

		// Token: 0x04001EE8 RID: 7912
		public TrashGrabberDefinition TrashGrabberDef;

		// Token: 0x04001EE9 RID: 7913
		[Header("References")]
		public PickUpTrashBehaviour PickUpTrashBehaviour;

		// Token: 0x04001EEA RID: 7914
		public EmptyTrashGrabberBehaviour EmptyTrashGrabberBehaviour;

		// Token: 0x04001EEB RID: 7915
		public BagTrashCanBehaviour BagTrashCanBehaviour;

		// Token: 0x04001EEC RID: 7916
		public DisposeTrashBagBehaviour DisposeTrashBagBehaviour;

		// Token: 0x04001EED RID: 7917
		public Sprite typeIcon;

		// Token: 0x04001EEE RID: 7918
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001EEF RID: 7919
		[Header("UI")]
		public CleanerUIElement WorldspaceUIPrefab;

		// Token: 0x04001EF0 RID: 7920
		public Transform uiPoint;

		// Token: 0x04001EF5 RID: 7925
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001EF6 RID: 7926
		private bool dll_Excuted;

		// Token: 0x04001EF7 RID: 7927
		private bool dll_Excuted;
	}
}
