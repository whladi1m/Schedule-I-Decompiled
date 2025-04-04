using System;
using System.IO;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000397 RID: 919
	public class StorageLoader : Loader
	{
		// Token: 0x0600149A RID: 5274 RVA: 0x0005C1CC File Offset: 0x0005A3CC
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			string[] files = Directory.GetFiles(mainPath);
			for (int i = 0; i < files.Length; i++)
			{
				string json;
				if (base.TryLoadFile(files[i], out json, false))
				{
					WorldStorageEntityData worldStorageEntityData = null;
					try
					{
						worldStorageEntityData = JsonUtility.FromJson<WorldStorageEntityData>(json);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error loading data: " + ex.Message);
					}
					if (worldStorageEntityData != null)
					{
						WorldStorageEntity @object = GUIDManager.GetObject<WorldStorageEntity>(new Guid(worldStorageEntityData.GUID));
						if (@object != null)
						{
							@object.Load(worldStorageEntityData);
						}
					}
				}
			}
		}
	}
}
