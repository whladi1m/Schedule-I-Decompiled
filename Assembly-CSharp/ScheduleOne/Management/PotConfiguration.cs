using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
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
	// Token: 0x02000563 RID: 1379
	public class PotConfiguration : EntityConfiguration
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x0008DEDB File Offset: 0x0008C0DB
		// (set) Token: 0x0600226F RID: 8815 RVA: 0x0008DEE3 File Offset: 0x0008C0E3
		public Pot Pot { get; protected set; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x0008DEEC File Offset: 0x0008C0EC
		// (set) Token: 0x06002271 RID: 8817 RVA: 0x0008DEF4 File Offset: 0x0008C0F4
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002272 RID: 8818 RVA: 0x0008DF00 File Offset: 0x0008C100
		public PotConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Pot pot) : base(replicator, configurable)
		{
			this.Pot = pot;
			this.Seed = new ItemField(this);
			this.Seed.CanSelectNone = true;
			List<ItemDefinition> options = Singleton<Registry>.Instance.Seeds.Cast<ItemDefinition>().ToList<ItemDefinition>();
			this.Seed.Options = options;
			this.Seed.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			List<ItemDefinition> options2 = Singleton<ManagementUtilities>.Instance.AdditiveDefinitions.Cast<ItemDefinition>().ToList<ItemDefinition>();
			this.Additive1 = new ItemField(this);
			this.Additive1.CanSelectNone = true;
			this.Additive1.Options = options2;
			this.Additive1.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.Additive2 = new ItemField(this);
			this.Additive2.CanSelectNone = true;
			this.Additive2.Options = options2;
			this.Additive2.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.Additive3 = new ItemField(this);
			this.Additive3.CanSelectNone = true;
			this.Additive3.Options = options2;
			this.Additive3.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedBotanist = new NPCField(this);
			this.AssignedBotanist.TypeRequirement = typeof(Botanist);
			this.AssignedBotanist.onNPCChanged.AddListener(delegate(NPC <p0>)
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

		// Token: 0x06002273 RID: 8819 RVA: 0x0008E0EC File Offset: 0x0008C2EC
		public override void Destroy()
		{
			base.Destroy();
			if (this.AssignedBotanist.SelectedNPC != null)
			{
				((this.AssignedBotanist.SelectedNPC as Botanist).Configuration as BotanistConfiguration).AssignedStations.RemoveItem(this.Pot);
			}
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x0008E158 File Offset: 0x0008C358
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Pot, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x06002275 RID: 8821 RVA: 0x0008E1CF File Offset: 0x0008C3CF
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Pot;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0008E1FE File Offset: 0x0008C3FE
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x0008E21A File Offset: 0x0008C41A
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x0008E238 File Offset: 0x0008C438
		public override bool ShouldSave()
		{
			return this.Seed.SelectedItem != null || this.Additive1.SelectedItem != null || this.Additive2.SelectedItem != null || this.Additive3.SelectedItem != null || this.AssignedBotanist.SelectedNPC != null || this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0008E2CC File Offset: 0x0008C4CC
		public override string GetSaveString()
		{
			return new PotConfigurationData(this.Seed.GetData(), this.Additive1.GetData(), this.Additive2.GetData(), this.Additive3.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x040019FD RID: 6653
		public ItemField Seed;

		// Token: 0x040019FE RID: 6654
		public ItemField Additive1;

		// Token: 0x040019FF RID: 6655
		public ItemField Additive2;

		// Token: 0x04001A00 RID: 6656
		public ItemField Additive3;

		// Token: 0x04001A01 RID: 6657
		public NPCField AssignedBotanist;

		// Token: 0x04001A02 RID: 6658
		public ObjectField Destination;
	}
}
