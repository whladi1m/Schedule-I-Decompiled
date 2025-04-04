using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000381 RID: 897
	public class BusinessesLoader : Loader
	{
		// Token: 0x06001465 RID: 5221 RVA: 0x0005B0E4 File Offset: 0x000592E4
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			List<DirectoryInfo> directories = base.GetDirectories(mainPath);
			BusinessLoader loader = new BusinessLoader();
			for (int i = 0; i < directories.Count; i++)
			{
				new LoadRequest(directories[i].FullName, loader);
			}
		}
	}
}
