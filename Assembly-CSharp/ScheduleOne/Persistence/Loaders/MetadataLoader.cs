using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000389 RID: 905
	public class MetadataLoader : Loader
	{
		// Token: 0x06001479 RID: 5241 RVA: 0x0005B5A8 File Offset: 0x000597A8
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				MetaData metaData = JsonUtility.FromJson<MetaData>(json);
				if (metaData != null)
				{
					Singleton<MetadataManager>.Instance.Load(metaData);
				}
			}
		}
	}
}
