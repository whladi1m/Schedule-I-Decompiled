using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006CE RID: 1742
	public static class JsonHelper
	{
		// Token: 0x06002F7D RID: 12157 RVA: 0x000C60B2 File Offset: 0x000C42B2
		public static T[] FromJson<T>(string json)
		{
			return JsonUtility.FromJson<JsonHelper.Wrapper<T>>(json).Items;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000C60BF File Offset: 0x000C42BF
		public static string ToJson<T>(T[] array)
		{
			return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
			{
				Items = array
			});
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000C60D2 File Offset: 0x000C42D2
		public static string ToJson<T>(T[] array, bool prettyPrint)
		{
			return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
			{
				Items = array
			}, prettyPrint);
		}

		// Token: 0x020006CF RID: 1743
		[Serializable]
		private class Wrapper<T>
		{
			// Token: 0x040021E9 RID: 8681
			public T[] Items;
		}
	}
}
