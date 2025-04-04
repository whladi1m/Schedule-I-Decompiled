using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200069A RID: 1690
	[Serializable]
	public struct Entry
	{
		// Token: 0x06002EA0 RID: 11936 RVA: 0x000C33D4 File Offset: 0x000C15D4
		public DialogueChain GetRandomChain()
		{
			if (this.Chains.Length == 0)
			{
				return null;
			}
			int num = UnityEngine.Random.Range(0, this.Chains.Length);
			return this.Chains[num];
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000C3403 File Offset: 0x000C1603
		public string GetRandomLine()
		{
			return this.GetRandomChain().Lines[0];
		}

		// Token: 0x04002138 RID: 8504
		public string Key;

		// Token: 0x04002139 RID: 8505
		public DialogueChain[] Chains;
	}
}
