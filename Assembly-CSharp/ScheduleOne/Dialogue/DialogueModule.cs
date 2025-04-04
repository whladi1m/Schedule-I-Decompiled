using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006AE RID: 1710
	public class DialogueModule : MonoBehaviour
	{
		// Token: 0x06002F02 RID: 12034 RVA: 0x000C48A4 File Offset: 0x000C2AA4
		public Entry GetEntry(string key)
		{
			return this.Entries.Find((Entry x) => x.Key == key);
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000C48D8 File Offset: 0x000C2AD8
		public DialogueChain GetChain(string key)
		{
			Entry entry = this.GetEntry(key);
			if (entry.Chains == null || entry.Chains.Length == 0)
			{
				Debug.LogError("DialogueModule.Get: No lines found for key: " + key);
			}
			return entry.GetRandomChain();
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000C4915 File Offset: 0x000C2B15
		public bool HasChain(string key)
		{
			return this.GetEntry(key).Chains != null;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000C4928 File Offset: 0x000C2B28
		public string GetLine(string key)
		{
			Entry entry = this.GetEntry(key);
			if (entry.Chains == null || entry.Chains.Length == 0)
			{
				Debug.LogError("DialogueModule.Get: No lines found for key: " + key);
				return string.Empty;
			}
			return entry.GetRandomLine();
		}

		// Token: 0x0400216D RID: 8557
		public EDialogueModule ModuleType;

		// Token: 0x0400216E RID: 8558
		public List<Entry> Entries = new List<Entry>();
	}
}
