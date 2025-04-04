using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200038D RID: 909
	public class PlayerLoader : Loader
	{
		// Token: 0x06001481 RID: 5249 RVA: 0x0005B70C File Offset: 0x0005990C
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, "Player", out json))
			{
				PlayerData playerData = null;
				try
				{
					playerData = JsonUtility.FromJson<PlayerData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (playerData != null)
				{
					Singleton<PlayerManager>.Instance.LoadPlayer(playerData, mainPath);
				}
			}
		}
	}
}
