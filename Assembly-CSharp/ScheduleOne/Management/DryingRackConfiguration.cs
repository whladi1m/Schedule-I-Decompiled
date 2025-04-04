using System;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055D RID: 1373
	public class DryingRackConfiguration : EntityConfiguration
	{
		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x0008CE8F File Offset: 0x0008B08F
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x0008CE97 File Offset: 0x0008B097
		public DryingRack Rack { get; protected set; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x0008CEA0 File Offset: 0x0008B0A0
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x0008CEA8 File Offset: 0x0008B0A8
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x0600221B RID: 8731 RVA: 0x0008CEB4 File Offset: 0x0008B0B4
		public DryingRackConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, DryingRack rack) : base(replicator, configurable)
		{
			this.Rack = rack;
			this.AssignedBotanist = new NPCField(this);
			this.AssignedBotanist.TypeRequirement = typeof(Botanist);
			this.AssignedBotanist.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.TargetQuality = new QualityField(this);
			this.TargetQuality.onValueChanged.AddListener(delegate(EQuality <p0>)
			{
				base.InvokeChanged();
			});
			this.TargetQuality.SetValue(EQuality.Premium, false);
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x0008CFA9 File Offset: 0x0008B1A9
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x0008CFCC File Offset: 0x0008B1CC
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Rack, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x0600221E RID: 8734 RVA: 0x0008D043 File Offset: 0x0008B243
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Rack;
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x0008D072 File Offset: 0x0008B272
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x0008D08E File Offset: 0x0008B28E
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x0008D0AA File Offset: 0x0008B2AA
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x0008D0C7 File Offset: 0x0008B2C7
		public override string GetSaveString()
		{
			return new DryingRackConfigurationData(this.TargetQuality.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019DF RID: 6623
		public NPCField AssignedBotanist;

		// Token: 0x040019E0 RID: 6624
		public QualityField TargetQuality;

		// Token: 0x040019E1 RID: 6625
		public ObjectField Destination;
	}
}
