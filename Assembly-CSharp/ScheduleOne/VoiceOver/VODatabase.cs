using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000276 RID: 630
	[CreateAssetMenu(fileName = "VODatabase", menuName = "ScriptableObjects/VODatabase")]
	[Serializable]
	public class VODatabase : ScriptableObject
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x0003A5C8 File Offset: 0x000387C8
		public VODatabaseEntry GetEntry(EVOLineType lineType)
		{
			foreach (VODatabaseEntry vodatabaseEntry in this.Entries)
			{
				if (vodatabaseEntry.LineType == lineType)
				{
					return vodatabaseEntry;
				}
			}
			return null;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003A624 File Offset: 0x00038824
		public AudioClip GetRandomClip(EVOLineType lineType)
		{
			VODatabaseEntry entry = this.GetEntry(lineType);
			if (entry != null)
			{
				return entry.GetRandomClip();
			}
			return null;
		}

		// Token: 0x04000DBB RID: 3515
		[Range(0f, 2f)]
		public float VolumeMultiplier = 1f;

		// Token: 0x04000DBC RID: 3516
		public List<VODatabaseEntry> Entries = new List<VODatabaseEntry>();
	}
}
