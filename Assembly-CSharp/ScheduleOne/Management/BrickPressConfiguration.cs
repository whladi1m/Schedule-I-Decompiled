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
	// Token: 0x02000558 RID: 1368
	public class BrickPressConfiguration : EntityConfiguration
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x0008BB2F File Offset: 0x00089D2F
		// (set) Token: 0x060021D2 RID: 8658 RVA: 0x0008BB37 File Offset: 0x00089D37
		public BrickPress BrickPress { get; protected set; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0008BB40 File Offset: 0x00089D40
		// (set) Token: 0x060021D4 RID: 8660 RVA: 0x0008BB48 File Offset: 0x00089D48
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x060021D5 RID: 8661 RVA: 0x0008BB54 File Offset: 0x00089D54
		public BrickPressConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, BrickPress station) : base(replicator, configurable)
		{
			this.BrickPress = station;
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

		// Token: 0x060021D6 RID: 8662 RVA: 0x0008BC14 File Offset: 0x00089E14
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0008BC38 File Offset: 0x00089E38
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.BrickPress, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x060021D8 RID: 8664 RVA: 0x0008BCAF File Offset: 0x00089EAF
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.BrickPress;
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x0008BCDE File Offset: 0x00089EDE
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x0008BCFA File Offset: 0x00089EFA
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x0008BD16 File Offset: 0x00089F16
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x0008BD33 File Offset: 0x00089F33
		public override string GetSaveString()
		{
			return new BrickPressConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019C5 RID: 6597
		public NPCField AssignedPackager;

		// Token: 0x040019C6 RID: 6598
		public ObjectField Destination;
	}
}
