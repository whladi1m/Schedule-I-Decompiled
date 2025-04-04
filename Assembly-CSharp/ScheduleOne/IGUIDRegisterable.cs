using System;

namespace ScheduleOne
{
	// Token: 0x02000269 RID: 617
	public interface IGUIDRegisterable
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000CDF RID: 3295
		Guid GUID { get; }

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000394DC File Offset: 0x000376DC
		void SetGUID(string guid)
		{
			Guid guid2;
			if (Guid.TryParse(guid, out guid2))
			{
				this.SetGUID(guid2);
				return;
			}
			Console.LogWarning(guid + " is not a valid GUID.", null);
		}

		// Token: 0x06000CE1 RID: 3297
		void SetGUID(Guid guid);
	}
}
