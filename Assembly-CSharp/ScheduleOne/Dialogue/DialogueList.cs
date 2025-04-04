using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000696 RID: 1686
	[Serializable]
	public class DialogueList
	{
		// Token: 0x06002E93 RID: 11923 RVA: 0x000C3218 File Offset: 0x000C1418
		public string GetRandomLine()
		{
			if (this.Lines.Length == 0)
			{
				return string.Empty;
			}
			int num = UnityEngine.Random.Range(0, this.Lines.Length);
			return this.Lines[num];
		}

		// Token: 0x04002132 RID: 8498
		public string[] Lines;
	}
}
