using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000387 RID: 903
	public class LawLoader : Loader
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x0005B460 File Offset: 0x00059660
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				LawData lawData = null;
				try
				{
					lawData = JsonUtility.FromJson<LawData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (lawData != null)
				{
					Singleton<LawController>.Instance.Load(lawData);
				}
			}
		}
	}
}
