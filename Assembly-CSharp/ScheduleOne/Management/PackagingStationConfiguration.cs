using System;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000562 RID: 1378
	public class PackagingStationConfiguration : EntityConfiguration
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x0008DCBF File Offset: 0x0008BEBF
		// (set) Token: 0x06002261 RID: 8801 RVA: 0x0008DCC7 File Offset: 0x0008BEC7
		public PackagingStation Station { get; protected set; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x0008DCD0 File Offset: 0x0008BED0
		// (set) Token: 0x06002263 RID: 8803 RVA: 0x0008DCD8 File Offset: 0x0008BED8
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002264 RID: 8804 RVA: 0x0008DCE4 File Offset: 0x0008BEE4
		public PackagingStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, PackagingStation station) : base(replicator, configurable)
		{
			this.Station = station;
			this.AssignedPackager = new NPCField(this);
			this.AssignedPackager.TypeRequirement = typeof(Packager);
			this.AssignedPackager.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x0008DDA4 File Offset: 0x0008BFA4
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0008DDC8 File Offset: 0x0008BFC8
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Station, this.Destination.SelectedObject as ITransitEntity);
				if (base.IsSelected)
				{
					this.DestinationRoute.SetVisualsActive(true);
					return;
				}
			}
			else
			{
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x0008DE3F File Offset: 0x0008C03F
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x0008DE6E File Offset: 0x0008C06E
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x0008DE8A File Offset: 0x0008C08A
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0008DEA6 File Offset: 0x0008C0A6
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0008DEC3 File Offset: 0x0008C0C3
		public override string GetSaveString()
		{
			return new PackagingStationConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019F9 RID: 6649
		public NPCField AssignedPackager;

		// Token: 0x040019FA RID: 6650
		public ObjectField Destination;
	}
}
