using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000557 RID: 1367
	public class BotanistConfiguration : EntityConfiguration
	{
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x0008B53B File Offset: 0x0008973B
		// (set) Token: 0x060021C5 RID: 8645 RVA: 0x0008B543 File Offset: 0x00089743
		public Botanist botanist { get; protected set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060021C6 RID: 8646 RVA: 0x0008B54C File Offset: 0x0008974C
		// (set) Token: 0x060021C7 RID: 8647 RVA: 0x0008B554 File Offset: 0x00089754
		public BedItem bedItem { get; private set; }

		// Token: 0x060021C8 RID: 8648 RVA: 0x0008B560 File Offset: 0x00089760
		public BotanistConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Botanist _botanist) : base(replicator, configurable)
		{
			this.botanist = _botanist;
			this.Bed = new ObjectField(this);
			this.Bed.TypeRequirements = new List<Type>
			{
				typeof(BedItem)
			};
			this.Bed.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.BedChanged));
			this.Bed.objectFilter = new ObjectSelector.ObjectFilter(BedItem.IsBedValid);
			this.Supplies = new ObjectField(this);
			this.Supplies.TypeRequirements = new List<Type>
			{
				typeof(PlaceableStorageEntity)
			};
			this.Supplies.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedStations = new ObjectListField(this);
			this.AssignedStations.MaxItems = this.botanist.MaxAssignedPots;
			this.AssignedStations.TypeRequirements = new List<Type>
			{
				typeof(Pot),
				typeof(DryingRack)
			};
			this.AssignedStations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedStations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedPotsChanged));
			this.AssignedStations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0008B6DC File Offset: 0x000898DC
		public override void Destroy()
		{
			base.Destroy();
			this.Bed.SetObject(null, false);
			foreach (Pot pot in this.AssignedPots)
			{
				(pot.Configuration as PotConfiguration).AssignedBotanist.SetNPC(null, false);
			}
			foreach (DryingRack dryingRack in this.AssignedRacks)
			{
				(dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SetNPC(null, false);
			}
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x0008B7A0 File Offset: 0x000899A0
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			Pot pot = obj as Pot;
			DryingRack dryingRack = obj as DryingRack;
			if (pot != null)
			{
				PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
				if (potConfiguration.AssignedBotanist.SelectedNPC != null && potConfiguration.AssignedBotanist.SelectedNPC != this.botanist)
				{
					reason = "Already assigned to " + potConfiguration.AssignedBotanist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(dryingRack != null))
				{
					reason = "Not a pot or drying rack";
					return false;
				}
				DryingRackConfiguration dryingRackConfiguration = dryingRack.Configuration as DryingRackConfiguration;
				if (dryingRackConfiguration.AssignedBotanist.SelectedNPC != null && dryingRackConfiguration.AssignedBotanist.SelectedNPC != this.botanist)
				{
					reason = "Already assigned to " + dryingRackConfiguration.AssignedBotanist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x0008B88C File Offset: 0x00089A8C
		public void AssignedPotsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.AssignedPots.Count; i++)
			{
				if (!objects.Contains(this.AssignedPots[i]))
				{
					Pot pot = this.AssignedPots[i];
					this.AssignedPots.RemoveAt(i);
					i--;
					if ((pot.Configuration as PotConfiguration).AssignedBotanist.SelectedNPC == this.botanist)
					{
						(pot.Configuration as PotConfiguration).AssignedBotanist.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < objects.Count; j++)
			{
				if (objects[j] is Pot)
				{
					if (!this.AssignedPots.Contains(objects[j]))
					{
						Pot pot2 = objects[j] as Pot;
						this.AssignedPots.Add(pot2);
						if ((pot2.Configuration as PotConfiguration).AssignedBotanist.SelectedNPC != this.botanist)
						{
							(pot2.Configuration as PotConfiguration).AssignedBotanist.SetNPC(this.botanist, false);
						}
					}
				}
				else if (objects[j] is DryingRack && !this.AssignedRacks.Contains(objects[j]))
				{
					DryingRack dryingRack = objects[j] as DryingRack;
					this.AssignedRacks.Add(dryingRack);
					if ((dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SelectedNPC != this.botanist)
					{
						(dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SetNPC(this.botanist, false);
					}
				}
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x0008BA30 File Offset: 0x00089C30
		public override bool ShouldSave()
		{
			return this.AssignedPots.Count > 0 || this.AssignedRacks.Count > 0 || this.Supplies.SelectedObject != null || this.Bed.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x0008BA8D File Offset: 0x00089C8D
		public override string GetSaveString()
		{
			return new BotanistConfigurationData(this.Bed.GetData(), this.Supplies.GetData(), this.AssignedStations.GetData()).GetJson(true);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0008BABC File Offset: 0x00089CBC
		private void BedChanged(BuildableItem newItem)
		{
			BedItem bedItem = this.bedItem;
			if (bedItem != null)
			{
				bedItem.Bed.SetAssignedEmployee(null);
			}
			this.bedItem = ((newItem != null) ? (newItem as BedItem) : null);
			if (this.bedItem != null)
			{
				this.bedItem.Bed.SetAssignedEmployee(this.botanist);
			}
			base.InvokeChanged();
		}

		// Token: 0x040019BD RID: 6589
		public ObjectField Bed;

		// Token: 0x040019BE RID: 6590
		public ObjectField Supplies;

		// Token: 0x040019BF RID: 6591
		public ObjectListField AssignedStations;

		// Token: 0x040019C0 RID: 6592
		public List<Pot> AssignedPots = new List<Pot>();

		// Token: 0x040019C1 RID: 6593
		public List<DryingRack> AssignedRacks = new List<DryingRack>();
	}
}
