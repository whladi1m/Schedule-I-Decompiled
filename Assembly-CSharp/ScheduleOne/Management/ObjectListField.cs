using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200056B RID: 1387
	public class ObjectListField : ConfigField
	{
		// Token: 0x060022A8 RID: 8872 RVA: 0x0008E818 File Offset: 0x0008CA18
		public ObjectListField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0008E84C File Offset: 0x0008CA4C
		public void SetList(List<BuildableItem> list, bool network)
		{
			if (this.SelectedObjects.SequenceEqual(list))
			{
				return;
			}
			for (int i = 0; i < this.SelectedObjects.Count; i++)
			{
				if (!(this.SelectedObjects[i] == null))
				{
					BuildableItem buildableItem = this.SelectedObjects[i];
					buildableItem.onDestroyedWithParameter = (Action<BuildableItem>)Delegate.Remove(buildableItem.onDestroyedWithParameter, new Action<BuildableItem>(this.SelectedObjectDestroyed));
				}
			}
			this.SelectedObjects = new List<BuildableItem>();
			this.SelectedObjects.AddRange(list);
			for (int j = 0; j < this.SelectedObjects.Count; j++)
			{
				if (!(this.SelectedObjects[j] == null))
				{
					BuildableItem buildableItem2 = this.SelectedObjects[j];
					buildableItem2.onDestroyedWithParameter = (Action<BuildableItem>)Delegate.Combine(buildableItem2.onDestroyedWithParameter, new Action<BuildableItem>(this.SelectedObjectDestroyed));
				}
			}
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onListChanged != null)
			{
				this.onListChanged.Invoke(list);
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0008E954 File Offset: 0x0008CB54
		public void AddItem(BuildableItem item)
		{
			if (this.SelectedObjects.Contains(item))
			{
				return;
			}
			if (this.SelectedObjects.Count >= this.MaxItems)
			{
				Console.LogWarning(item.ItemInstance.Name + " cannot be added to " + base.ParentConfig.GetType().Name + " because the maximum number of items has been reached", null);
				return;
			}
			this.SetList(new List<BuildableItem>(this.SelectedObjects)
			{
				item
			}, true);
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x0008E9D0 File Offset: 0x0008CBD0
		public void RemoveItem(BuildableItem item)
		{
			if (!this.SelectedObjects.Contains(item))
			{
				return;
			}
			List<BuildableItem> list = new List<BuildableItem>(this.SelectedObjects);
			list.Remove(item);
			this.SetList(list, true);
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x0008EA08 File Offset: 0x0008CC08
		private void SelectedObjectDestroyed(BuildableItem item)
		{
			if (item == null)
			{
				return;
			}
			Console.Log("Removing destroyed object from " + base.ParentConfig.GetType().Name, null);
			this.RemoveItem(item);
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0008EA3B File Offset: 0x0008CC3B
		public override bool IsValueDefault()
		{
			return this.SelectedObjects.Count == 0;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x0008EA4C File Offset: 0x0008CC4C
		public ObjectListFieldData GetData()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.SelectedObjects.Count; i++)
			{
				list.Add(this.SelectedObjects[i].GUID.ToString());
			}
			return new ObjectListFieldData(list);
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x0008EAA0 File Offset: 0x0008CCA0
		public void Load(ObjectListFieldData data)
		{
			if (data != null)
			{
				List<BuildableItem> list = new List<BuildableItem>();
				for (int i = 0; i < data.ObjectGUIDs.Count; i++)
				{
					if (!string.IsNullOrEmpty(data.ObjectGUIDs[i]))
					{
						BuildableItem @object = GUIDManager.GetObject<BuildableItem>(new Guid(data.ObjectGUIDs[i]));
						if (@object != null)
						{
							list.Add(@object);
						}
					}
				}
				this.SetList(list, true);
			}
		}

		// Token: 0x04001A23 RID: 6691
		public List<BuildableItem> SelectedObjects = new List<BuildableItem>();

		// Token: 0x04001A24 RID: 6692
		public int MaxItems = 1;

		// Token: 0x04001A25 RID: 6693
		public ObjectSelector.ObjectFilter objectFilter;

		// Token: 0x04001A26 RID: 6694
		public List<Type> TypeRequirements = new List<Type>();

		// Token: 0x04001A27 RID: 6695
		public UnityEvent<List<BuildableItem>> onListChanged = new UnityEvent<List<BuildableItem>>();
	}
}
