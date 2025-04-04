using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C1A RID: 3098
	[Serializable]
	public class VersionInfo
	{
		// Token: 0x060056AC RID: 22188 RVA: 0x0016BB00 File Offset: 0x00169D00
		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 1, 9, 0) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x0016BB2E File Offset: 0x00169D2E
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", this.m_major, this.m_minor, this.m_release) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060056AE RID: 22190 RVA: 0x0016BB6A File Offset: 0x00169D6A
		public static int FullNumber
		{
			get
			{
				return 190;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x0016BB71 File Offset: 0x00169D71
		public int Number
		{
			get
			{
				return this.m_major * 100 + this.m_minor * 10 + this.m_release;
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x0016BB8D File Offset: 0x00169D8D
		private VersionInfo()
		{
			this.m_major = 1;
			this.m_minor = 9;
			this.m_release = 0;
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x0016BBAB File Offset: 0x00169DAB
		private VersionInfo(byte major, byte minor, byte release)
		{
			this.m_major = (int)major;
			this.m_minor = (int)minor;
			this.m_release = (int)release;
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0016BBC8 File Offset: 0x00169DC8
		public static VersionInfo Current()
		{
			return new VersionInfo(1, 9, 0);
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0016BBD3 File Offset: 0x00169DD3
		public static bool Matches(VersionInfo version)
		{
			return 1 == version.m_major && 9 == version.m_minor && version.m_release == 0;
		}

		// Token: 0x0400406C RID: 16492
		public const byte Major = 1;

		// Token: 0x0400406D RID: 16493
		public const byte Minor = 9;

		// Token: 0x0400406E RID: 16494
		public const byte Release = 0;

		// Token: 0x0400406F RID: 16495
		private static string StageSuffix = "";

		// Token: 0x04004070 RID: 16496
		private static string TrialSuffix = "";

		// Token: 0x04004071 RID: 16497
		[SerializeField]
		private int m_major;

		// Token: 0x04004072 RID: 16498
		[SerializeField]
		private int m_minor;

		// Token: 0x04004073 RID: 16499
		[SerializeField]
		private int m_release;
	}
}
