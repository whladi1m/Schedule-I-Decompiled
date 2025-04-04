using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039A RID: 922
	public class VariablesLoader : Loader
	{
		// Token: 0x060014A0 RID: 5280 RVA: 0x0005C644 File Offset: 0x0005A844
		public override void Load(string mainPath)
		{
			if (!Directory.Exists(mainPath))
			{
				return;
			}
			Console.Log("Loading variables", null);
			string[] files = Directory.GetFiles(mainPath);
			for (int i = 0; i < files.Length; i++)
			{
				string json;
				if (base.TryLoadFile(files[i], out json, false))
				{
					VariableData variableData = null;
					try
					{
						variableData = JsonUtility.FromJson<VariableData>(json);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error loading quest data: " + ex.Message);
					}
					if (variableData != null)
					{
						NetworkSingleton<VariableDatabase>.Instance.Load(variableData);
					}
				}
			}
		}
	}
}
