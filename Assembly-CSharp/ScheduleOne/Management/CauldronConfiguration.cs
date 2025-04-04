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
	// Token: 0x02000559 RID: 1369
	public class CauldronConfiguration : EntityConfiguration
	{
		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x0008BD4B File Offset: 0x00089F4B
		// (set) Token: 0x060021E0 RID: 8672 RVA: 0x0008BD53 File Offset: 0x00089F53
		public Cauldron Station { get; protected set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x0008BD5C File Offset: 0x00089F5C
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x0008BD64 File Offset: 0x00089F64
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x060021E3 RID: 8675 RVA: 0x0008BD70 File Offset: 0x00089F70
		public CauldronConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Cauldron cauldron) : base(replicator, configurable)
		{
			this.Station = cauldron;
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
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0008BE30 File Offset: 0x0008A030
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0008BE54 File Offset: 0x0008A054
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

		// Token: 0x060021E6 RID: 8678 RVA: 0x0008BECB File Offset: 0x0008A0CB
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0008BEFA File Offset: 0x0008A0FA
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0008BF16 File Offset: 0x0008A116
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x0008BF32 File Offset: 0x0008A132
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0008BF4F File Offset: 0x0008A14F
		public override string GetSaveString()
		{
			return new CauldronConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019C9 RID: 6601
		public NPCField AssignedChemist;

		// Token: 0x040019CA RID: 6602
		public ObjectField Destination;
	}
}
