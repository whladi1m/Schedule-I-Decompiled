using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006BC RID: 1724
	[Serializable]
	public class VocalReactionDatabase
	{
		// Token: 0x06002F23 RID: 12067 RVA: 0x000C4BE0 File Offset: 0x000C2DE0
		public VocalReactionDatabase.Entry GetEntry(string key)
		{
			foreach (VocalReactionDatabase.Entry entry in this.Entries)
			{
				if (entry.Key == key)
				{
					return entry;
				}
			}
			return null;
		}

		// Token: 0x04002192 RID: 8594
		public List<VocalReactionDatabase.Entry> Entries = new List<VocalReactionDatabase.Entry>();

		// Token: 0x020006BD RID: 1725
		[Serializable]
		public class Entry
		{
			// Token: 0x170006DB RID: 1755
			// (get) Token: 0x06002F25 RID: 12069 RVA: 0x000C4C57 File Offset: 0x000C2E57
			public string name
			{
				get
				{
					return this.Key;
				}
			}

			// Token: 0x06002F26 RID: 12070 RVA: 0x000C4C5F File Offset: 0x000C2E5F
			public string GetRandomReaction()
			{
				return this.Reactions[UnityEngine.Random.Range(0, this.Reactions.Length)];
			}

			// Token: 0x04002193 RID: 8595
			public string Key;

			// Token: 0x04002194 RID: 8596
			public string[] Reactions;
		}
	}
}
