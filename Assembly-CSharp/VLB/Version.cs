using System;

namespace VLB
{
	// Token: 0x02000156 RID: 342
	public static class Version
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001D885 File Offset: 0x0001BA85
		public static string CurrentAsString
		{
			get
			{
				return Version.GetVersionAsString(20100);
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001D894 File Offset: 0x0001BA94
		private static string GetVersionAsString(int version)
		{
			int num = version / 10000;
			int num2 = (version - num * 10000) / 100;
			int num3 = (version - num * 10000 - num2 * 100) / 1;
			return string.Format("{0}.{1}.{2}", num, num2, num3);
		}

		// Token: 0x04000755 RID: 1877
		public const int Current = 20100;
	}
}
