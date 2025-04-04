using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000427 RID: 1063
	[Serializable]
	public class SaveData
	{
		// Token: 0x06001589 RID: 5513 RVA: 0x0005FA60 File Offset: 0x0005DC60
		public SaveData()
		{
			this.DataType = base.GetType().Name;
			this.DataVersion = this.GetDataVersion();
			this.GameVersion = Application.version;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00014002 File Offset: 0x00012202
		protected virtual int GetDataVersion()
		{
			return 0;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0005FAB1 File Offset: 0x0005DCB1
		public virtual string GetJson(bool prettyPrint = true)
		{
			if (this.DataType == string.Empty)
			{
				Type type = base.GetType();
				Console.LogError(((type != null) ? type.ToString() : null) + " GetJson() called but has no data type set!", null);
			}
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x0400144F RID: 5199
		public string DataType = string.Empty;

		// Token: 0x04001450 RID: 5200
		public int DataVersion;

		// Token: 0x04001451 RID: 5201
		public string GameVersion = string.Empty;
	}
}
