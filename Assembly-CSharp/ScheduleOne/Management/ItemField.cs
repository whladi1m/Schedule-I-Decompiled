using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000567 RID: 1383
	public class ItemField : ConfigField
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x0008E3FC File Offset: 0x0008C5FC
		// (set) Token: 0x06002286 RID: 8838 RVA: 0x0008E404 File Offset: 0x0008C604
		public ItemDefinition SelectedItem { get; protected set; }

		// Token: 0x06002287 RID: 8839 RVA: 0x0008E40D File Offset: 0x0008C60D
		public ItemField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0008E433 File Offset: 0x0008C633
		public void SetItem(ItemDefinition item, bool network)
		{
			this.SelectedItem = item;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onItemChanged != null)
			{
				this.onItemChanged.Invoke(this.SelectedItem);
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0008E465 File Offset: 0x0008C665
		public override bool IsValueDefault()
		{
			return this.SelectedItem == null;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0008E473 File Offset: 0x0008C673
		public ItemFieldData GetData()
		{
			return new ItemFieldData((this.SelectedItem != null) ? this.SelectedItem.ID.ToString() : "");
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0008E4A0 File Offset: 0x0008C6A0
		public void Load(ItemFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.ItemID))
			{
				ItemDefinition item = Registry.GetItem(data.ItemID);
				if (item != null)
				{
					this.SetItem(item, true);
				}
			}
		}

		// Token: 0x04001A13 RID: 6675
		public bool CanSelectNone = true;

		// Token: 0x04001A14 RID: 6676
		public List<ItemDefinition> Options = new List<ItemDefinition>();

		// Token: 0x04001A15 RID: 6677
		public UnityEvent<ItemDefinition> onItemChanged = new UnityEvent<ItemDefinition>();
	}
}
