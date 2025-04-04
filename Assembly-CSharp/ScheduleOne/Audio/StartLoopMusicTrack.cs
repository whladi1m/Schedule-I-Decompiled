using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007A2 RID: 1954
	public class StartLoopMusicTrack : MusicTrack
	{
		// Token: 0x060034EC RID: 13548 RVA: 0x000DE5D8 File Offset: 0x000DC7D8
		protected override void Awake()
		{
			base.Awake();
			this.AutoFadeOut = false;
			this.LoopSound.AudioSource.loop = true;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
		public override void Update()
		{
			base.Update();
			if (base.IsPlaying)
			{
				if (!this.Controller.AudioSource.isPlaying && !this.LoopSound.isPlaying)
				{
					this.LoopSound.Play();
				}
				this.LoopSound.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
				return;
			}
			this.LoopSound.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
			if (this.LoopSound.VolumeMultiplier == 0f)
			{
				this.LoopSound.AudioSource.Stop();
			}
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000DE690 File Offset: 0x000DC890
		public override void Play()
		{
			base.Play();
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Play>g__WaitForStart|3_0());
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000DE6B1 File Offset: 0x000DC8B1
		[CompilerGenerated]
		private IEnumerator <Play>g__WaitForStart|3_0()
		{
			while (base.IsPlaying)
			{
				if (this.Controller.AudioSource.clip.length - this.Controller.AudioSource.time <= Time.deltaTime)
				{
					Console.Log("Starting loop for " + this.TrackName, null);
					this.LoopSound.Play();
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x04002635 RID: 9781
		public AudioSourceController LoopSound;
	}
}
