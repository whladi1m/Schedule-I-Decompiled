using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007A4 RID: 1956
	public class StartLoopStopAudio : MonoBehaviour
	{
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060034F7 RID: 13559 RVA: 0x000DE77B File Offset: 0x000DC97B
		// (set) Token: 0x060034F8 RID: 13560 RVA: 0x000DE783 File Offset: 0x000DC983
		public bool Runnning { get; private set; }

		// Token: 0x060034F9 RID: 13561 RVA: 0x000DE78C File Offset: 0x000DC98C
		private void Update()
		{
			if (!this.Runnning)
			{
				this.timeSinceStop += Time.deltaTime;
				if (this.FadeLoopOut)
				{
					this.LoopSound.VolumeMultiplier = Mathf.Lerp(1f, 0f, this.timeSinceStop / this.StopSound.AudioSource.clip.length);
				}
				else
				{
					this.LoopSound.VolumeMultiplier = 0f;
				}
				if (this.LoopSound.isPlaying && this.LoopSound.VolumeMultiplier == 0f)
				{
					this.LoopSound.Stop();
				}
				return;
			}
			this.timeSinceStart += Time.deltaTime;
			if (this.FadeLoopIn)
			{
				this.LoopSound.VolumeMultiplier = Mathf.Lerp(0f, 1f, this.timeSinceStart / this.StartSound.AudioSource.clip.length);
				return;
			}
			this.LoopSound.VolumeMultiplier = 1f;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000DE890 File Offset: 0x000DCA90
		[Button]
		public void StartAudio()
		{
			if (this.Runnning)
			{
				return;
			}
			this.Runnning = true;
			this.timeSinceStart = 0f;
			this.LoopSound.Play();
			this.LoopSound.AudioSource.loop = true;
			this.StartSound.Play();
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000DE8DF File Offset: 0x000DCADF
		[Button]
		public void StopAudio()
		{
			if (!this.Runnning)
			{
				return;
			}
			this.Runnning = false;
			this.timeSinceStop = 0f;
			this.StartSound.Stop();
			this.StopSound.Play();
		}

		// Token: 0x0400263A RID: 9786
		public AudioSourceController StartSound;

		// Token: 0x0400263B RID: 9787
		public AudioSourceController LoopSound;

		// Token: 0x0400263C RID: 9788
		public AudioSourceController StopSound;

		// Token: 0x0400263D RID: 9789
		public bool FadeLoopIn;

		// Token: 0x0400263E RID: 9790
		public bool FadeLoopOut;

		// Token: 0x0400263F RID: 9791
		private float timeSinceStart;

		// Token: 0x04002640 RID: 9792
		private float timeSinceStop;
	}
}
