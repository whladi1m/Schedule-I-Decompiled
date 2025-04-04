using System;
using System.Collections.Generic;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020F RID: 527
	[Serializable]
	public class GeneratorExclude
	{
		// Token: 0x04000C6F RID: 3183
		public ExcludeItem ExcludeItem;

		// Token: 0x04000C70 RID: 3184
		public int targetIndex;

		// Token: 0x04000C71 RID: 3185
		public List<ExcludeIndexes> exclude = new List<ExcludeIndexes>();
	}
}
