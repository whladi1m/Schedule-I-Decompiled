using System;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000277 RID: 631
	[Serializable]
	public class VODatabaseEntry
	{
		// Token: 0x06000D2A RID: 3370 RVA: 0x0003A664 File Offset: 0x00038864
		public AudioClip GetRandomClip()
		{
			if (this.Clips.Length == 0)
			{
				return null;
			}
			AudioClip audioClip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
			int num = 0;
			while (audioClip == this.lastClip && this.Clips.Length != 1 && num <= 5)
			{
				audioClip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
				num++;
			}
			this.lastClip = audioClip;
			return audioClip;
		}

		// Token: 0x04000DBD RID: 3517
		public EVOLineType LineType;

		// Token: 0x04000DBE RID: 3518
		public AudioClip[] Clips;

		// Token: 0x04000DBF RID: 3519
		private AudioClip lastClip;

		// Token: 0x04000DC0 RID: 3520
		public float VolumeMultiplier = 1f;
	}
}
