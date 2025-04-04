using System;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200055E RID: 1374
	public class EntityConfiguration
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x0008D0EA File Offset: 0x0008B2EA
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x0008D0F2 File Offset: 0x0008B2F2
		public ConfigurationReplicator Replicator { get; protected set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x0008D0FB File Offset: 0x0008B2FB
		// (set) Token: 0x06002229 RID: 8745 RVA: 0x0008D103 File Offset: 0x0008B303
		public IConfigurable Configurable { get; protected set; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x0008D10C File Offset: 0x0008B30C
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x0008D114 File Offset: 0x0008B314
		public bool IsSelected { get; protected set; }

		// Token: 0x0600222C RID: 8748 RVA: 0x0008D11D File Offset: 0x0008B31D
		public EntityConfiguration(ConfigurationReplicator replicator, IConfigurable configurable)
		{
			this.Replicator = replicator;
			this.Replicator.Configuration = this;
			this.Configurable = configurable;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x0008D155 File Offset: 0x0008B355
		protected void InvokeChanged()
		{
			if (this.onChanged != null)
			{
				this.onChanged.Invoke();
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x0008D16A File Offset: 0x0008B36A
		public void ReplicateField(ConfigField field, NetworkConnection conn = null)
		{
			this.Replicator.ReplicateField(field, conn);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x0008D17C File Offset: 0x0008B37C
		public void ReplicateAllFields(NetworkConnection conn = null, bool replicateDefaults = true)
		{
			foreach (ConfigField configField in this.Fields)
			{
				if (replicateDefaults || !configField.IsValueDefault())
				{
					this.ReplicateField(configField, conn);
				}
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Destroy()
		{
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x0008D1DC File Offset: 0x0008B3DC
		public virtual void Selected()
		{
			this.IsSelected = true;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0008D1E5 File Offset: 0x0008B3E5
		public virtual void Deselected()
		{
			this.IsSelected = false;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool ShouldSave()
		{
			return false;
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x040019E5 RID: 6629
		public List<ConfigField> Fields = new List<ConfigField>();

		// Token: 0x040019E6 RID: 6630
		public UnityEvent onChanged = new UnityEvent();
	}
}
