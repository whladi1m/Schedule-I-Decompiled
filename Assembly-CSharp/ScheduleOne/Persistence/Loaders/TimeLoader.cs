using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000398 RID: 920
	public class TimeLoader : Loader
	{
		// Token: 0x0600149C RID: 5276 RVA: 0x0005C264 File Offset: 0x0005A464
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				TimeData timeData = JsonUtility.FromJson<TimeData>(json);
				if (timeData != null)
				{
					NetworkSingleton<TimeManager>.Instance.SetTime(timeData.TimeOfDay, false);
					NetworkSingleton<TimeManager>.Instance.SetElapsedDays(timeData.ElapsedDays);
					NetworkSingleton<TimeManager>.Instance.SetPlaytime((float)timeData.Playtime);
				}
			}
		}
	}
}
