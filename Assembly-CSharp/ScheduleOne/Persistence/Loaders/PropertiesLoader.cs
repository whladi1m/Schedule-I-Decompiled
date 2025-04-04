using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000392 RID: 914
	public class PropertiesLoader : Loader
	{
		// Token: 0x06001490 RID: 5264 RVA: 0x0005BC0C File Offset: 0x00059E0C
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			List<DirectoryInfo> directories = base.GetDirectories(mainPath);
			PropertyLoader loader = new PropertyLoader();
			for (int i = 0; i < directories.Count; i++)
			{
				new LoadRequest(directories[i].FullName, loader);
			}
		}
	}
}
