using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200056A RID: 1386
	public class ObjectField : ConfigField
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x0008E6C5 File Offset: 0x0008C8C5
		public ObjectField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0008E6E4 File Offset: 0x0008C8E4
		public void SetObject(BuildableItem obj, bool network)
		{
			if (this.SelectedObject == obj)
			{
				return;
			}
			if (this.SelectedObject != null)
			{
				this.SelectedObject.onDestroyed.RemoveListener(new UnityAction(this.SelectedObjectDestroyed));
			}
			this.SelectedObject = obj;
			if (this.SelectedObject != null)
			{
				this.SelectedObject.onDestroyed.AddListener(new UnityAction(this.SelectedObjectDestroyed));
			}
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onObjectChanged != null)
			{
				this.onObjectChanged.Invoke(obj);
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0008E77F File Offset: 0x0008C97F
		public override bool IsValueDefault()
		{
			return this.SelectedObject == null;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0008E78D File Offset: 0x0008C98D
		private void SelectedObjectDestroyed()
		{
			this.SetObject(null, false);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0008E798 File Offset: 0x0008C998
		public void Load(ObjectFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.ObjectGUID))
			{
				BuildableItem @object = GUIDManager.GetObject<BuildableItem>(new Guid(data.ObjectGUID));
				if (@object != null)
				{
					this.SetObject(@object, true);
				}
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x0008E7D8 File Offset: 0x0008C9D8
		public ObjectFieldData GetData()
		{
			return new ObjectFieldData((this.SelectedObject != null) ? this.SelectedObject.GUID.ToString() : "");
		}

		// Token: 0x04001A1E RID: 6686
		public BuildableItem SelectedObject;

		// Token: 0x04001A1F RID: 6687
		public UnityEvent<BuildableItem> onObjectChanged = new UnityEvent<BuildableItem>();

		// Token: 0x04001A20 RID: 6688
		public ObjectSelector.ObjectFilter objectFilter;

		// Token: 0x04001A21 RID: 6689
		public List<Type> TypeRequirements = new List<Type>();

		// Token: 0x04001A22 RID: 6690
		public bool DrawTransitLine;
	}
}
