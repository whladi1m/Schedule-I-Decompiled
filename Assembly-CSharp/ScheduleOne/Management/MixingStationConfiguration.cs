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
	// Token: 0x02000560 RID: 1376
	public class MixingStationConfiguration : EntityConfiguration
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x0008D3F3 File Offset: 0x0008B5F3
		// (set) Token: 0x06002244 RID: 8772 RVA: 0x0008D3FB File Offset: 0x0008B5FB
		public MixingStation station { get; protected set; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x0008D404 File Offset: 0x0008B604
		// (set) Token: 0x06002246 RID: 8774 RVA: 0x0008D40C File Offset: 0x0008B60C
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002247 RID: 8775 RVA: 0x0008D418 File Offset: 0x0008B618
		public MixingStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, MixingStation station) : base(replicator, configurable)
		{
			this.station = station;
			this.AssignedChemist = new NPCField(this);
			this.AssignedChemist.TypeRequirement = typeof(Chemist);
			this.AssignedChemist.onNPCChanged.AddListener(delegate(NPC <p0>)
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
			this.StartThrehold = new NumberField(this);
			this.StartThrehold.Configure(1f, 10f, true);
			this.StartThrehold.SetValue(1f, false);
			this.StartThrehold.onItemChanged.AddListener(delegate(float <p0>)
			{
				base.InvokeChanged();
			});
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x0008D527 File Offset: 0x0008B727
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0008D54C File Offset: 0x0008B74C
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.station, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x0600224A RID: 8778 RVA: 0x0008D5C3 File Offset: 0x0008B7C3
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.station;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0008D5F2 File Offset: 0x0008B7F2
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x0008D60E File Offset: 0x0008B80E
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x0008D62A File Offset: 0x0008B82A
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x0008D647 File Offset: 0x0008B847
		public override string GetSaveString()
		{
			return new MixingStationConfigurationData(this.Destination.GetData(), this.StartThrehold.GetData()).GetJson(true);
		}

		// Token: 0x040019ED RID: 6637
		public NPCField AssignedChemist;

		// Token: 0x040019EE RID: 6638
		public ObjectField Destination;

		// Token: 0x040019EF RID: 6639
		public NumberField StartThrehold;
	}
}
