using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200038B RID: 907
	public class MoneyLoader : Loader
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0005B658 File Offset: 0x00059858
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, true))
			{
				MoneyData moneyData = null;
				try
				{
					moneyData = JsonUtility.FromJson<MoneyData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (moneyData != null)
				{
					NetworkSingleton<MoneyManager>.Instance.Load(moneyData);
				}
			}
		}
	}
}
