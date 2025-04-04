using System;

namespace ScheduleOne.Management.Presets.Options
{
	// Token: 0x02000592 RID: 1426
	public abstract class Option
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x00090CBD File Offset: 0x0008EEBD
		// (set) Token: 0x06002394 RID: 9108 RVA: 0x00090CC5 File Offset: 0x0008EEC5
		public string Name { get; protected set; } = "OptionName";

		// Token: 0x06002395 RID: 9109 RVA: 0x00090CCE File Offset: 0x0008EECE
		public Option(string name)
		{
			this.Name = name;
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x00090CE8 File Offset: 0x0008EEE8
		public virtual void CopyTo(Option other)
		{
			other.Name = this.Name;
		}

		// Token: 0x06002397 RID: 9111
		public abstract string GetDisplayString();
	}
}
