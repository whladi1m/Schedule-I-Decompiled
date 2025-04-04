using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200010F RID: 271
	[Serializable]
	public class RaymarchingQuality
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00016DA6 File Offset: 0x00014FA6
		public int uniqueID
		{
			get
			{
				return this._UniqueID;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00016DAE File Offset: 0x00014FAE
		public bool hasValidUniqueID
		{
			get
			{
				return this._UniqueID >= 0;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00016DBC File Offset: 0x00014FBC
		public static RaymarchingQuality defaultInstance
		{
			get
			{
				return RaymarchingQuality.ms_DefaultInstance;
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00016DC3 File Offset: 0x00014FC3
		private RaymarchingQuality(int uniqueID)
		{
			this._UniqueID = uniqueID;
			this.name = "New quality";
			this.stepCount = 10;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00016DE5 File Offset: 0x00014FE5
		public static RaymarchingQuality New()
		{
			return new RaymarchingQuality(UnityEngine.Random.Range(4, int.MaxValue));
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00016DF7 File Offset: 0x00014FF7
		public static RaymarchingQuality New(string name, int forcedUniqueID, int stepCount)
		{
			return new RaymarchingQuality(forcedUniqueID)
			{
				name = name,
				stepCount = stepCount
			};
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00016E10 File Offset: 0x00015010
		private static bool HasRaymarchingQualityWithSameUniqueID(RaymarchingQuality[] values, int id)
		{
			foreach (RaymarchingQuality raymarchingQuality in values)
			{
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040005E3 RID: 1507
		public string name;

		// Token: 0x040005E4 RID: 1508
		public int stepCount;

		// Token: 0x040005E5 RID: 1509
		[SerializeField]
		private int _UniqueID;

		// Token: 0x040005E6 RID: 1510
		private static RaymarchingQuality ms_DefaultInstance = new RaymarchingQuality(-1);

		// Token: 0x040005E7 RID: 1511
		private const int kRandomUniqueIdMinRange = 4;
	}
}
