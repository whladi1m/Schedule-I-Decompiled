using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000385 RID: 901
	public class GameDataLoader : Loader
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x0005B3B4 File Offset: 0x000595B4
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				GameData gameData = JsonUtility.FromJson<GameData>(json);
				if (gameData != null)
				{
					NetworkSingleton<GameManager>.Instance.Load(gameData, mainPath);
				}
			}
		}
	}
}
