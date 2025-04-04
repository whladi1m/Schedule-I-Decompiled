using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055B RID: 1371
	public class ChemistryStationConfiguration : EntityConfiguration
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x0008C97B File Offset: 0x0008AB7B
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x0008C983 File Offset: 0x0008AB83
		public ChemistryStation Station { get; protected set; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x0008C98C File Offset: 0x0008AB8C
		// (set) Token: 0x060021FD RID: 8701 RVA: 0x0008C994 File Offset: 0x0008AB94
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x060021FE RID: 8702 RVA: 0x0008C9A0 File Offset: 0x0008ABA0
		public ChemistryStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, ChemistryStation station) : base(replicator, configurable)
		{
			this.Station = station;
			this.AssignedChemist = new NPCField(this);
			this.AssignedChemist.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.Recipe = new StationRecipeField(this);
			this.Recipe.Options = Singleton<ChemistryStationCanvas>.Instance.Recipes;
			this.Recipe.onRecipeChanged.AddListener(delegate(StationRecipe <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ScheduleOne.UI.Management.ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x0008CA88 File Offset: 0x0008AC88
		public override void Destroy()
		{
			base.Destroy();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0008CAAC File Offset: 0x0008ACAC
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

		// Token: 0x06002201 RID: 8705 RVA: 0x0008CB23 File Offset: 0x0008AD23
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0008CB52 File Offset: 0x0008AD52
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0008CB6E File Offset: 0x0008AD6E
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0008CB8A File Offset: 0x0008AD8A
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0008CBA7 File Offset: 0x0008ADA7
		public override string GetSaveString()
		{
			return new ChemistryStationConfigurationData(this.Recipe.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019D5 RID: 6613
		public NPCField AssignedChemist;

		// Token: 0x040019D6 RID: 6614
		public StationRecipeField Recipe;

		// Token: 0x040019D7 RID: 6615
		public ObjectField Destination;
	}
}
