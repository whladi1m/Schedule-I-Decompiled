using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000386 RID: 902
	public class GenericSaveablesLoader : Loader
	{
		// Token: 0x06001470 RID: 5232 RVA: 0x0005B3E4 File Offset: 0x000595E4
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
					GenericSaveData genericSaveData = null;
					try
					{
						genericSaveData = JsonUtility.FromJson<GenericSaveData>(json);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error loading generic save data: " + ex.Message);
					}
					if (genericSaveData != null)
					{
						Singleton<GenericSaveablesManager>.Instance.LoadSaveable(genericSaveData);
					}
				}
			}
		}
	}
}
