using System;

namespace ScheduleOne.Management
{
	// Token: 0x02000566 RID: 1382
	public abstract class ConfigField
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x0008E3CB File Offset: 0x0008C5CB
		// (set) Token: 0x06002282 RID: 8834 RVA: 0x0008E3D3 File Offset: 0x0008C5D3
		public EntityConfiguration ParentConfig { get; protected set; }

		// Token: 0x06002283 RID: 8835 RVA: 0x0008E3DC File Offset: 0x0008C5DC
		public ConfigField(EntityConfiguration parentConfig)
		{
			this.ParentConfig = parentConfig;
			this.ParentConfig.Fields.Add(this);
		}

		// Token: 0x06002284 RID: 8836
		public abstract bool IsValueDefault();
	}
}
