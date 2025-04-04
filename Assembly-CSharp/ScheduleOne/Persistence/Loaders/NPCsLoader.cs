using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200038C RID: 908
	public class NPCsLoader : Loader
	{
		// Token: 0x0600147F RID: 5247 RVA: 0x0005B6CC File Offset: 0x000598CC
		public override void Load(string mainPath)
		{
			List<DirectoryInfo> directories = base.GetDirectories(mainPath);
			NPCLoader loader = new NPCLoader();
			for (int i = 0; i < directories.Count; i++)
			{
				new LoadRequest(directories[i].FullName, loader);
			}
		}
	}
}
