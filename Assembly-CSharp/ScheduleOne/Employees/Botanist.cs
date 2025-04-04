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
using ScheduleOne.Dialogue;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
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
	// Token: 0x0200062B RID: 1579
	public class Botanist : Employee, IConfigurable
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000AD00D File Offset: 0x000AB20D
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x000AD015 File Offset: 0x000AB215
		// (set) Token: 0x060029F0 RID: 10736 RVA: 0x000AD01D File Offset: 0x000AB21D
		protected BotanistConfiguration configuration { get; set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000AD026 File Offset: 0x000AB226
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x00051F73 File Offset: 0x00050173
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Botanist;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000AD02E File Offset: 0x000AB22E
		// (set) Token: 0x060029F4 RID: 10740 RVA: 0x000AD036 File Offset: 0x000AB236
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000AD03F File Offset: 0x000AB23F
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x000AD047 File Offset: 0x000AB247
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

		// Token: 0x060029F7 RID: 10743 RVA: 0x000AD051 File Offset: 0x000AB251
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x000AD067 File Offset: 0x000AB267
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x000AD077 File Offset: 0x000AB277
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000AD07F File Offset: 0x000AB27F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000AD087 File Offset: 0x000AB287
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000AD090 File Offset: 0x000AB290
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (this.PotActionBehaviour.Active)
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
			if (this.configuration.AssignedPots.Count + this.configuration.AssignedRacks.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any pots or drying racks", "You can use your management clipboards to assign pots/drying racks to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Pot potForWatering = this.GetPotForWatering(this.CRITICAL_WATERING_THRESHOLD, true);
			if (potForWatering != null && NavMeshUtility.GetAccessPoint(potForWatering, this) != null)
			{
				this.StartAction(potForWatering, PotActionBehaviour.EActionType.Water);
				return;
			}
			Pot potForSoilSour = this.GetPotForSoilSour();
			if (potForSoilSour != null)
			{
				if (this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForSoilSour, PotActionBehaviour.EActionType.PourSoil, -1))
				{
					this.StartAction(potForSoilSour, PotActionBehaviour.EActionType.PourSoil);
					return;
				}
				string fix = "Make sure there's soil in my supplies stash.";
				if (this.configuration.Supplies.SelectedObject == null)
				{
					fix = "Use your management clipboards to assign a supplies stash to me. Then make sure there's soil in it.";
				}
				base.SubmitNoWorkReason("There are empty pots, but I don't have any soil to pour.", fix, 0);
			}
			foreach (Pot pot in this.GetPotsReadyForSeed())
			{
				if (NavMeshUtility.GetAccessPoint(pot, this))
				{
					if (this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, pot, PotActionBehaviour.EActionType.SowSeed, -1))
					{
						this.StartAction(pot, PotActionBehaviour.EActionType.SowSeed);
						return;
					}
					string fix2 = "Make sure I have the right seeds in my supplies stash.";
					if (this.configuration.Supplies.SelectedObject == null)
					{
						fix2 = "Use your management clipboards to assign a supplies stash to me, and make sure it contains the right seeds.";
					}
					base.SubmitNoWorkReason("There is a pot ready for sowing, but I don't have any seeds for it.", fix2, 1);
				}
			}
			int additiveNumber;
			Pot potForAdditives = this.GetPotForAdditives(out additiveNumber);
			if (potForAdditives != null && this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForAdditives, PotActionBehaviour.EActionType.ApplyAdditive, additiveNumber))
			{
				this.PotActionBehaviour.AdditiveNumber = additiveNumber;
				this.StartAction(potForAdditives, PotActionBehaviour.EActionType.ApplyAdditive);
				return;
			}
			foreach (Pot pot2 in this.GetPotsForHarvest())
			{
				if (this.IsEntityAccessible(pot2))
				{
					if (this.PotActionBehaviour.DoesPotHaveValidDestination(pot2))
					{
						this.StartAction(pot2, PotActionBehaviour.EActionType.Harvest);
						return;
					}
					base.SubmitNoWorkReason("There is a plant ready for harvest, but it has no destination or it's destination is full.", "Use your management clipboard to assign a destination for each of my pots, and make sure the destination isn't full.", 0);
				}
			}
			foreach (DryingRack dryingRack in this.GetRacksToStop())
			{
				if (this.IsEntityAccessible(dryingRack))
				{
					this.StopDryingRack(dryingRack);
					return;
				}
			}
			foreach (DryingRack dryingRack2 in this.GetRacksToStart())
			{
				if (this.IsEntityAccessible(dryingRack2))
				{
					this.StartDryingRack(dryingRack2);
					return;
				}
			}
			foreach (DryingRack dryingRack3 in this.GetRacksReadyToMove())
			{
				if (this.IsEntityAccessible(dryingRack3))
				{
					this.MoveItemBehaviour.Initialize((dryingRack3.Configuration as DryingRackConfiguration).DestinationRoute, dryingRack3.OutputSlot.ItemInstance, -1, false);
					this.MoveItemBehaviour.Enable_Networked(null);
					return;
				}
			}
			Pot potForWatering2 = this.GetPotForWatering(this.WATERING_THRESHOLD, false);
			if (potForWatering2 != null)
			{
				this.StartAction(potForWatering2, PotActionBehaviour.EActionType.Water);
				return;
			}
			QualityItemInstance qualityItemInstance;
			DryingRack destination;
			int maxMoveAmount;
			if (this.CanMoveDryableToRack(out qualityItemInstance, out destination, out maxMoveAmount))
			{
				TransitRoute route = new TransitRoute(this.configuration.Supplies.SelectedObject as ITransitEntity, destination);
				this.MoveItemBehaviour.Initialize(route, qualityItemInstance, maxMoveAmount, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				Console.Log(string.Concat(new string[]
				{
					"Moving ",
					maxMoveAmount.ToString(),
					" ",
					qualityItemInstance.ID,
					" to drying rack"
				}), null);
				return;
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000AD4F4 File Offset: 0x000AB6F4
		private bool IsEntityAccessible(ITransitEntity entity)
		{
			return NavMeshUtility.GetAccessPoint(entity, this) != null;
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000AD503 File Offset: 0x000AB703
		private void StartAction(Pot pot, PotActionBehaviour.EActionType actionType)
		{
			this.SetIdle(false);
			this.PotActionBehaviour.Initialize(pot, actionType);
			this.PotActionBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000AD525 File Offset: 0x000AB725
		private void StartDryingRack(DryingRack rack)
		{
			this.StartDryingRackBehaviour.AssignRack(rack);
			this.StartDryingRackBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000AD53F File Offset: 0x000AB73F
		private void StopDryingRack(DryingRack rack)
		{
			this.StopDryingRackBehaviour.AssignRack(rack);
			this.StopDryingRackBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000AD559 File Offset: 0x000AB759
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000AD56C File Offset: 0x000AB76C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Botanist.<>c__DisplayClass58_0 CS$<>8__locals1 = new Botanist.<>c__DisplayClass58_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000AD5AC File Offset: 0x000AB7AC
		protected override void AssignProperty(Property prop)
		{
			base.AssignProperty(prop);
			prop.AddConfigurable(this);
			this.configuration = new BotanistConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000AD5D6 File Offset: 0x000AB7D6
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

		// Token: 0x06002A07 RID: 10759 RVA: 0x000AD614 File Offset: 0x000AB814
		private bool CanMoveDryableToRack(out QualityItemInstance dryable, out DryingRack destinationRack, out int moveQuantity)
		{
			moveQuantity = 0;
			destinationRack = null;
			dryable = this.GetDryableInSupplies();
			if (dryable == null)
			{
				return false;
			}
			Console.Log("Found dryable in supplies: " + dryable.ID, null);
			int b = 0;
			destinationRack = this.GetAssignedDryingRackFor(dryable, out b);
			if (destinationRack == null)
			{
				return false;
			}
			Console.Log("Found rack with capacity: " + b.ToString(), null);
			moveQuantity = Mathf.Min(dryable.Quantity, b);
			return true;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000AD690 File Offset: 0x000AB890
		public QualityItemInstance GetDryableInSupplies()
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && ItemFilter_Dryable.IsItemDryable(list[i].ItemInstance))
				{
					return list[i].ItemInstance as QualityItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000AD740 File Offset: 0x000AB940
		private DryingRack GetAssignedDryingRackFor(QualityItemInstance dryable, out int rackInputCapacity)
		{
			rackInputCapacity = 0;
			foreach (DryingRack dryingRack in this.configuration.AssignedRacks)
			{
				if ((dryingRack.Configuration as DryingRackConfiguration).TargetQuality.Value > dryable.Quality)
				{
					int inputCapacityForItem = ((ITransitEntity)dryingRack).GetInputCapacityForItem(dryable, this);
					if (inputCapacityForItem > 0)
					{
						rackInputCapacity = inputCapacityForItem;
						return dryingRack;
					}
				}
			}
			return null;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000AD7C8 File Offset: 0x000AB9C8
		public ItemInstance GetItemInSupplies(string id)
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && list[i].ItemInstance.ID.ToLower() == id.ToLower())
				{
					return list[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000AD880 File Offset: 0x000ABA80
		public ItemInstance GetSeedInSupplies()
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && list[i].ItemInstance.Definition is SeedDefinition)
				{
					return list[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000AD92D File Offset: 0x000ABB2D
		protected override bool ShouldIdle()
		{
			return this.configuration.AssignedStations.SelectedObjects.Count == 0 || base.ShouldIdle();
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000AD94E File Offset: 0x000ABB4E
		public override BedItem GetBed()
		{
			return this.configuration.bedItem;
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000AD95C File Offset: 0x000ABB5C
		private bool AreThereUnspecifiedPots()
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if ((this.configuration.AssignedPots[i].Configuration as PotConfiguration).Seed.SelectedItem == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000AD9B4 File Offset: 0x000ABBB4
		private bool AreThereNullDestinationPots()
		{
			foreach (Pot pot in this.configuration.AssignedPots)
			{
				string text;
				if (pot.IsReadyForHarvest(out text) && (pot.Configuration as PotConfiguration).Destination.SelectedObject == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000ADA34 File Offset: 0x000ABC34
		private bool IsMissingRequiredMaterials()
		{
			Pot potForSoilSour = this.GetPotForSoilSour();
			if (potForSoilSour != null && !this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForSoilSour, PotActionBehaviour.EActionType.PourSoil, -1))
			{
				return false;
			}
			List<Pot> potsReadyForSeed = this.GetPotsReadyForSeed();
			for (int i = 0; i < potsReadyForSeed.Count; i++)
			{
				if (this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potsReadyForSeed[i], PotActionBehaviour.EActionType.SowSeed, -1))
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000ADA98 File Offset: 0x000ABC98
		private Pot GetPotForWatering(float threshold, bool excludeFullyGrowm)
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotBeWatered(this.configuration.AssignedPots[i], threshold) && (!excludeFullyGrowm || this.configuration.AssignedPots[i].Plant == null || !this.configuration.AssignedPots[i].Plant.IsFullyGrown) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000ADB50 File Offset: 0x000ABD50
		private Pot GetPotForSoilSour()
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveSoilPour(this.configuration.AssignedPots[i]) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000ADBC4 File Offset: 0x000ABDC4
		private List<Pot> GetPotsReadyForSeed()
		{
			List<Pot> list = new List<Pot>();
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveSeedSown(this.configuration.AssignedPots[i]))
				{
					list.Add(this.configuration.AssignedPots[i]);
				}
			}
			return list;
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000ADC28 File Offset: 0x000ABE28
		private T GetAccessableEntity<T>(T entity) where T : ITransitEntity
		{
			if (!(NavMeshUtility.GetAccessPoint(entity, this) != null))
			{
				return default(T);
			}
			return entity;
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000ADC54 File Offset: 0x000ABE54
		private List<T> GetAccessableEntities<T>(List<T> list) where T : ITransitEntity
		{
			return (from item in list
			where NavMeshUtility.GetAccessPoint(item, this) != null
			select item).ToList<T>();
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000ADC70 File Offset: 0x000ABE70
		private List<Pot> FilterPotsForSpecifiedSeed(List<Pot> pots)
		{
			List<Pot> list = new List<Pot>();
			foreach (Pot pot in pots)
			{
				if ((pot.Configuration as PotConfiguration).Seed.SelectedItem != null)
				{
					list.Add(pot);
				}
			}
			return list;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000ADCE4 File Offset: 0x000ABEE4
		private Pot GetPotForAdditives(out int additiveNumber)
		{
			additiveNumber = -1;
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveAdditiveApplied(this.configuration.AssignedPots[i], out additiveNumber) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000ADD5C File Offset: 0x000ABF5C
		private List<Pot> GetPotsForHarvest()
		{
			List<Pot> list = new List<Pot>();
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotBeHarvested(this.configuration.AssignedPots[i]))
				{
					list.Add(this.configuration.AssignedPots[i]);
				}
			}
			return list;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000ADDC0 File Offset: 0x000ABFC0
		private List<DryingRack> GetRacksToStart()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				if (this.StartDryingRackBehaviour.IsRackReady(this.configuration.AssignedRacks[i]))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000ADE24 File Offset: 0x000AC024
		private List<DryingRack> GetRacksToStop()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				if (this.StopDryingRackBehaviour.IsRackReady(this.configuration.AssignedRacks[i]))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000ADE88 File Offset: 0x000AC088
		private List<DryingRack> GetRacksReadyToMove()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				ItemSlot outputSlot = this.configuration.AssignedRacks[i].OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((this.configuration.AssignedRacks[i].Configuration as DryingRackConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000ADF28 File Offset: 0x000AC128
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
			BotanistUIElement component = UnityEngine.Object.Instantiate<BotanistUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<BotanistUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000ADFB3 File Offset: 0x000AC1B3
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000ADFD0 File Offset: 0x000AC1D0
		public override string GetSaveString()
		{
			return new BotanistData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData()).GetJson(true);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000AE040 File Offset: 0x000AC240
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

		// Token: 0x06002A22 RID: 10786 RVA: 0x000AE128 File Offset: 0x000AC328
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Botanist));
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000AE1A0 File Offset: 0x000AC3A0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000AE1C4 File Offset: 0x000AC3C4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000AE1D4 File Offset: 0x000AC3D4
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

		// Token: 0x06002A26 RID: 10790 RVA: 0x000AE27B File Offset: 0x000AC47B
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000AE284 File Offset: 0x000AC484
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

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x000AE2C2 File Offset: 0x000AC4C2
		// (set) Token: 0x06002A29 RID: 10793 RVA: 0x000AE2CA File Offset: 0x000AC4CA
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

		// Token: 0x06002A2A RID: 10794 RVA: 0x000AE308 File Offset: 0x000AC508
		public virtual bool Botanist(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002A2B RID: 10795 RVA: 0x000AE35A File Offset: 0x000AC55A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001EB1 RID: 7857
		public float CRITICAL_WATERING_THRESHOLD = 0.1f;

		// Token: 0x04001EB2 RID: 7858
		public float WATERING_THRESHOLD = 0.3f;

		// Token: 0x04001EB3 RID: 7859
		public float TARGET_WATER_LEVEL_MIN = 0.75f;

		// Token: 0x04001EB4 RID: 7860
		public float TARGET_WATER_LEVEL_MAX = 1f;

		// Token: 0x04001EB5 RID: 7861
		public float SOIL_POUR_TIME = 10f;

		// Token: 0x04001EB6 RID: 7862
		public float WATER_POUR_TIME = 10f;

		// Token: 0x04001EB7 RID: 7863
		public float ADDITIVE_POUR_TIME = 10f;

		// Token: 0x04001EB8 RID: 7864
		public float SEED_SOW_TIME = 15f;

		// Token: 0x04001EB9 RID: 7865
		public float HARVEST_TIME = 15f;

		// Token: 0x04001EBA RID: 7866
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04001EBB RID: 7867
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001EBC RID: 7868
		public PotActionBehaviour PotActionBehaviour;

		// Token: 0x04001EBD RID: 7869
		public StartDryingRackBehaviour StartDryingRackBehaviour;

		// Token: 0x04001EBE RID: 7870
		public StopDryingRackBehaviour StopDryingRackBehaviour;

		// Token: 0x04001EBF RID: 7871
		[Header("UI")]
		public BotanistUIElement WorldspaceUIPrefab;

		// Token: 0x04001EC0 RID: 7872
		public Transform uiPoint;

		// Token: 0x04001EC1 RID: 7873
		[Header("Settings")]
		public int MaxAssignedPots = 8;

		// Token: 0x04001EC2 RID: 7874
		public DialogueContainer NoAssignedStationsDialogue;

		// Token: 0x04001EC3 RID: 7875
		public DialogueContainer UnspecifiedPotsDialogue;

		// Token: 0x04001EC4 RID: 7876
		public DialogueContainer NullDestinationPotsDialogue;

		// Token: 0x04001EC5 RID: 7877
		public DialogueContainer MissingMaterialsDialogue;

		// Token: 0x04001EC6 RID: 7878
		public DialogueContainer NoPotsRequireWorkDialogue;

		// Token: 0x04001ECA RID: 7882
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001ECB RID: 7883
		private bool dll_Excuted;

		// Token: 0x04001ECC RID: 7884
		private bool dll_Excuted;
	}
}
