using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039C RID: 924
	public class VehiclesLoader : Loader
	{
		// Token: 0x060014A4 RID: 5284 RVA: 0x0005C748 File Offset: 0x0005A948
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			List<DirectoryInfo> directories = base.GetDirectories(mainPath);
			VehicleLoader loader = new VehicleLoader();
			for (int i = 0; i < directories.Count; i++)
			{
				new LoadRequest(directories[i].FullName, loader);
			}
		}
	}
}
