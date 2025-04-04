using System;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055C RID: 1372
	public class CleanerConfiguration : EntityConfiguration
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x0008CBCA File Offset: 0x0008ADCA
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x0008CBD2 File Offset: 0x0008ADD2
		public Cleaner cleaner { get; protected set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x0008CBDB File Offset: 0x0008ADDB
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x0008CBE3 File Offset: 0x0008ADE3
		public List<TrashContainerItem> binItems { get; private set; } = new List<TrashContainerItem>();

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x0008CBEC File Offset: 0x0008ADEC
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x0008CBF4 File Offset: 0x0008ADF4
		public BedItem bedItem { get; private set; }

		// Token: 0x0600220F RID: 8719 RVA: 0x0008CC00 File Offset: 0x0008AE00
		public CleanerConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Cleaner _cleaner) : base(replicator, configurable)
		{
			this.cleaner = _cleaner;
			this.Bed = new ObjectField(this);
			this.Bed.TypeRequirements = new List<Type>
			{
				typeof(BedItem)
			};
			this.Bed.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.BedChanged));
			this.Bed.objectFilter = new ObjectSelector.ObjectFilter(BedItem.IsBedValid);
			this.Bins = new ObjectListField(this);
			this.Bins.MaxItems = 3;
			this.Bins.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Bins.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedBinsChanged));
			this.Bins.objectFilter = new ObjectSelector.ObjectFilter(this.IsObjValid);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0008CCED File Offset: 0x0008AEED
		public override void Destroy()
		{
			base.Destroy();
			this.Bed.SetObject(null, false);
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0008CD04 File Offset: 0x0008AF04
		private bool IsObjValid(BuildableItem obj, out string reason)
		{
			TrashContainerItem trashContainerItem = obj as TrashContainerItem;
			if (trashContainerItem == null)
			{
				reason = string.Empty;
				return false;
			}
			if (!trashContainerItem.UsableByCleaners)
			{
				reason = "This trash can is not usable by cleaners.";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0008CD44 File Offset: 0x0008AF44
		public void AssignedBinsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.binItems.Count; i++)
			{
				if (!objects.Contains(this.binItems[i]))
				{
					this.binItems.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < objects.Count; j++)
			{
				if (!this.binItems.Contains(objects[j] as TrashContainerItem))
				{
					this.binItems.Add(objects[j] as TrashContainerItem);
				}
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0008CDCC File Offset: 0x0008AFCC
		public override bool ShouldSave()
		{
			return this.Bed.SelectedObject != null || this.Bins.SelectedObjects.Count > 0 || base.ShouldSave();
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0008CDFE File Offset: 0x0008AFFE
		public override string GetSaveString()
		{
			return new CleanerConfigurationData(this.Bed.GetData(), this.Bins.GetData()).GetJson(true);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0008CE24 File Offset: 0x0008B024
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
				this.bedItem.Bed.SetAssignedEmployee(this.cleaner);
			}
			base.InvokeChanged();
		}

		// Token: 0x040019D9 RID: 6617
		public ObjectField Bed;

		// Token: 0x040019DA RID: 6618
		public ObjectListField Bins;
	}
}
