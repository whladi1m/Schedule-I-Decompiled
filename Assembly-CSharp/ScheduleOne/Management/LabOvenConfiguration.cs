using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055F RID: 1375
	public class LabOvenConfiguration : EntityConfiguration
	{
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x0008D1EE File Offset: 0x0008B3EE
		// (set) Token: 0x06002236 RID: 8758 RVA: 0x0008D1F6 File Offset: 0x0008B3F6
		public LabOven Oven { get; protected set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002237 RID: 8759 RVA: 0x0008D1FF File Offset: 0x0008B3FF
		// (set) Token: 0x06002238 RID: 8760 RVA: 0x0008D207 File Offset: 0x0008B407
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002239 RID: 8761 RVA: 0x0008D210 File Offset: 0x0008B410
		public LabOvenConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, LabOven oven) : base(replicator, configurable)
		{
			this.Oven = oven;
			this.AssignedChemist = new NPCField(this);
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

		// Token: 0x0600223A RID: 8762 RVA: 0x0008D2BB File Offset: 0x0008B4BB
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0008D2E0 File Offset: 0x0008B4E0
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Oven, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x0600223C RID: 8764 RVA: 0x0008D357 File Offset: 0x0008B557
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Oven;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x0008D386 File Offset: 0x0008B586
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0008D3A2 File Offset: 0x0008B5A2
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0008D3BE File Offset: 0x0008B5BE
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0008D3DB File Offset: 0x0008B5DB
		public override string GetSaveString()
		{
			return new LabOvenConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019E9 RID: 6633
		public NPCField AssignedChemist;

		// Token: 0x040019EA RID: 6634
		public ObjectField Destination;
	}
}
