using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000561 RID: 1377
	public class PackagerConfiguration : EntityConfiguration
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x0008D66A File Offset: 0x0008B86A
		public int AssignedStationCount
		{
			get
			{
				return this.AssignedStations.Count + this.AssignedBrickPresses.Count;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x0008D683 File Offset: 0x0008B883
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x0008D68B File Offset: 0x0008B88B
		public Packager packager { get; protected set; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x0008D694 File Offset: 0x0008B894
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x0008D69C File Offset: 0x0008B89C
		public BedItem bedItem { get; private set; }

		// Token: 0x06002257 RID: 8791 RVA: 0x0008D6A8 File Offset: 0x0008B8A8
		public PackagerConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Packager _botanist) : base(replicator, configurable)
		{
			this.packager = _botanist;
			this.Bed = new ObjectField(this);
			this.Bed.TypeRequirements = new List<Type>
			{
				typeof(BedItem)
			};
			this.Bed.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.BedChanged));
			this.Bed.objectFilter = new ObjectSelector.ObjectFilter(BedItem.IsBedValid);
			this.Stations = new ObjectListField(this);
			this.Stations.MaxItems = this.packager.MaxAssignedStations;
			this.Stations.TypeRequirements = new List<Type>
			{
				typeof(PackagingStation),
				typeof(PackagingStationMk2),
				typeof(BrickPress)
			};
			this.Stations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Stations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedStationsChanged));
			this.Stations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
			this.Routes = new RouteListField(this);
			this.Routes.MaxRoutes = 5;
			this.Routes.onListChanged.AddListener(delegate(List<AdvancedTransitRoute> <p0>)
			{
				base.InvokeChanged();
			});
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x0008D820 File Offset: 0x0008BA20
		public override void Destroy()
		{
			base.Destroy();
			this.Bed.SetObject(null, false);
			foreach (PackagingStation packagingStation in this.AssignedStations)
			{
				(packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(null, false);
			}
			foreach (BrickPress brickPress in this.AssignedBrickPresses)
			{
				(brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(null, false);
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x0008D8E4 File Offset: 0x0008BAE4
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (obj is PackagingStation)
			{
				PackagingStationConfiguration packagingStationConfiguration = (obj as PackagingStation).Configuration as PackagingStationConfiguration;
				if (packagingStationConfiguration.AssignedPackager.SelectedNPC != null && packagingStationConfiguration.AssignedPackager.SelectedNPC != this.packager)
				{
					reason = "Already assigned to " + packagingStationConfiguration.AssignedPackager.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(obj is BrickPress))
				{
					return false;
				}
				BrickPressConfiguration brickPressConfiguration = (obj as BrickPress).Configuration as BrickPressConfiguration;
				if (brickPressConfiguration.AssignedPackager.SelectedNPC != null && brickPressConfiguration.AssignedPackager.SelectedNPC != this.packager)
				{
					reason = "Already assigned to " + brickPressConfiguration.AssignedPackager.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0008D9C4 File Offset: 0x0008BBC4
		public void AssignedStationsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.AssignedStations.Count; i++)
			{
				if (!objects.Contains(this.AssignedStations[i]))
				{
					PackagingStation packagingStation = this.AssignedStations[i];
					this.AssignedStations.RemoveAt(i);
					i--;
					if ((packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SelectedNPC == this.packager)
					{
						(packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < this.AssignedBrickPresses.Count; j++)
			{
				if (!objects.Contains(this.AssignedBrickPresses[j]))
				{
					BrickPress brickPress = this.AssignedBrickPresses[j];
					this.AssignedBrickPresses.RemoveAt(j);
					j--;
					if ((brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SelectedNPC == this.packager)
					{
						(brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(null, false);
					}
				}
			}
			for (int k = 0; k < objects.Count; k++)
			{
				if (objects[k] is PackagingStation)
				{
					if (!this.AssignedStations.Contains(objects[k]))
					{
						PackagingStation packagingStation2 = objects[k] as PackagingStation;
						this.AssignedStations.Add(packagingStation2);
						if ((packagingStation2.Configuration as PackagingStationConfiguration).AssignedPackager.SelectedNPC != this.packager)
						{
							(packagingStation2.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(this.packager, false);
						}
					}
				}
				else if (objects[k] is BrickPress && !this.AssignedBrickPresses.Contains(objects[k]))
				{
					BrickPress brickPress2 = objects[k] as BrickPress;
					this.AssignedBrickPresses.Add(brickPress2);
					if ((brickPress2.Configuration as BrickPressConfiguration).AssignedPackager.SelectedNPC != this.packager)
					{
						(brickPress2.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(this.packager, false);
					}
				}
			}
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x0008DBF7 File Offset: 0x0008BDF7
		public override bool ShouldSave()
		{
			return this.Bed.SelectedObject != null || this.AssignedStations.Count > 0 || base.ShouldSave();
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x0008DC24 File Offset: 0x0008BE24
		public override string GetSaveString()
		{
			return new PackagerConfigurationData(this.Bed.GetData(), this.Stations.GetData(), this.Routes.GetData()).GetJson(true);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x0008DC54 File Offset: 0x0008BE54
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
				this.bedItem.Bed.SetAssignedEmployee(this.packager);
			}
			base.InvokeChanged();
		}

		// Token: 0x040019F1 RID: 6641
		public ObjectField Bed;

		// Token: 0x040019F2 RID: 6642
		public ObjectListField Stations;

		// Token: 0x040019F3 RID: 6643
		public RouteListField Routes;

		// Token: 0x040019F4 RID: 6644
		public List<PackagingStation> AssignedStations = new List<PackagingStation>();

		// Token: 0x040019F5 RID: 6645
		public List<BrickPress> AssignedBrickPresses = new List<BrickPress>();
	}
}
