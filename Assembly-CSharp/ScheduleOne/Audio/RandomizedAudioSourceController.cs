using System;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200079E RID: 1950
	public class RandomizedAudioSourceController : AudioSourceController
	{
		// Token: 0x060034E2 RID: 13538 RVA: 0x000DE34C File Offset: 0x000DC54C
		public override void Play()
		{
			if (this.Clips.Length == 0)
			{
				Console.LogWarning("RandomizedAudioSourceController: No clips to play", null);
				return;
			}
			int num = UnityEngine.Random.Range(0, this.Clips.Length);
			this.AudioSource.clip = this.Clips[num];
			base.Play();
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000DE398 File Offset: 0x000DC598
		public override void PlayOneShot(bool duplicateAudioSource = false)
		{
			if (this.Clips.Length == 0)
			{
				Console.LogWarning("RandomizedAudioSourceController: No clips to play", null);
				return;
			}
			int num = UnityEngine.Random.Range(0, this.Clips.Length);
			this.AudioSource.clip = this.Clips[num];
			base.PlayOneShot(duplicateAudioSource);
		}

		// Token: 0x04002628 RID: 9768
		public AudioClip[] Clips;
	}
}
