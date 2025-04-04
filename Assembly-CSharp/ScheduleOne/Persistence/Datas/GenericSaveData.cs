using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003D1 RID: 977
	[Serializable]
	public class GenericSaveData : SaveData
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x0005EE04 File Offset: 0x0005D004
		public GenericSaveData(string guid)
		{
			this.GUID = guid;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0005EE55 File Offset: 0x0005D055
		public void Add(string key, bool value)
		{
			this.boolValues.Add(new GenericSaveData.BoolValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0005EE75 File Offset: 0x0005D075
		public void Add(string key, float value)
		{
			this.floatValues.Add(new GenericSaveData.FloatValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0005EE95 File Offset: 0x0005D095
		public void Add(string key, int value)
		{
			this.intValues.Add(new GenericSaveData.IntValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0005EEB5 File Offset: 0x0005D0B5
		public void Add(string key, string value)
		{
			this.stringValues.Add(new GenericSaveData.StringValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0005EED8 File Offset: 0x0005D0D8
		public bool GetBool(string key, bool defaultValue = false)
		{
			GenericSaveData.BoolValue boolValue = this.boolValues.Find((GenericSaveData.BoolValue x) => x.key == key);
			if (boolValue != null)
			{
				return boolValue.value;
			}
			return defaultValue;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0005EF18 File Offset: 0x0005D118
		public float GetFloat(string key, float defaultValue = 0f)
		{
			GenericSaveData.FloatValue floatValue = this.floatValues.Find((GenericSaveData.FloatValue x) => x.key == key);
			if (floatValue != null)
			{
				return floatValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0005EF58 File Offset: 0x0005D158
		public int GetInt(string key, int defaultValue = 0)
		{
			GenericSaveData.IntValue intValue = this.intValues.Find((GenericSaveData.IntValue x) => x.key == key);
			if (intValue != null)
			{
				return intValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0005EF98 File Offset: 0x0005D198
		public string GetString(string key, string defaultValue = "")
		{
			GenericSaveData.StringValue stringValue = this.stringValues.Find((GenericSaveData.StringValue x) => x.key == key);
			if (stringValue != null)
			{
				return stringValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0400137D RID: 4989
		public string GUID = string.Empty;

		// Token: 0x0400137E RID: 4990
		public List<GenericSaveData.BoolValue> boolValues = new List<GenericSaveData.BoolValue>();

		// Token: 0x0400137F RID: 4991
		public List<GenericSaveData.FloatValue> floatValues = new List<GenericSaveData.FloatValue>();

		// Token: 0x04001380 RID: 4992
		public List<GenericSaveData.IntValue> intValues = new List<GenericSaveData.IntValue>();

		// Token: 0x04001381 RID: 4993
		public List<GenericSaveData.StringValue> stringValues = new List<GenericSaveData.StringValue>();

		// Token: 0x020003D2 RID: 978
		[Serializable]
		public class BoolValue
		{
			// Token: 0x04001382 RID: 4994
			public string key;

			// Token: 0x04001383 RID: 4995
			public bool value;
		}

		// Token: 0x020003D3 RID: 979
		[Serializable]
		public class FloatValue
		{
			// Token: 0x04001384 RID: 4996
			public string key;

			// Token: 0x04001385 RID: 4997
			public float value;
		}

		// Token: 0x020003D4 RID: 980
		[Serializable]
		public class IntValue
		{
			// Token: 0x04001386 RID: 4998
			public string key;

			// Token: 0x04001387 RID: 4999
			public int value;
		}

		// Token: 0x020003D5 RID: 981
		[Serializable]
		public class StringValue
		{
			// Token: 0x04001388 RID: 5000
			public string key;

			// Token: 0x04001389 RID: 5001
			public string value;
		}
	}
}
