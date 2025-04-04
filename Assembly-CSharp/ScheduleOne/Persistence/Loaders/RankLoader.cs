using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000396 RID: 918
	public class RankLoader : Loader
	{
		// Token: 0x06001498 RID: 5272 RVA: 0x0005C158 File Offset: 0x0005A358
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				RankData rankData = null;
				try
				{
					rankData = JsonUtility.FromJson<RankData>(json);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to load rank data: " + ex.Message);
				}
				if (rankData != null)
				{
					NetworkSingleton<LevelManager>.Instance.SetData(null, (ERank)rankData.Rank, rankData.Tier, rankData.XP, rankData.TotalXP);
				}
			}
		}
	}
}
