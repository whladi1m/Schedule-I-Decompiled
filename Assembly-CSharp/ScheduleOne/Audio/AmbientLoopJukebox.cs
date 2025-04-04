using System;
using System.Collections.Generic;
using GameKit.Utilities;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000783 RID: 1923
	[RequireComponent(typeof(AudioSourceController))]
	public class AmbientLoopJukebox : MonoBehaviour
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x000DC220 File Offset: 0x000DA420
		private void Start()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
			this.audioSourceController.Play();
			this.Clips.Shuffle<AudioClip>();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x000DC278 File Offset: 0x000DA478
		private void Update()
		{
			if (Singleton<MusicPlayer>.Instance.IsPlaying)
			{
				this.musicScale = Mathf.Lerp(this.musicScale, 0.3f, Time.deltaTime / 4f);
				return;
			}
			this.musicScale = Mathf.Lerp(this.musicScale, 1f, Time.deltaTime / 4f);
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000DC2D4 File Offset: 0x000DA4D4
		private void MinPass()
		{
			float num = this.VolumeCurve.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.audioSourceController.VolumeMultiplier = num * this.musicScale;
			if (!this.audioSourceController.isPlaying)
			{
				this.currentClipIndex = (this.currentClipIndex + 1) % this.Clips.Count;
				this.audioSourceController.AudioSource.clip = this.Clips[this.currentClipIndex];
				this.audioSourceController.Play();
			}
		}

		// Token: 0x0400258E RID: 9614
		public AnimationCurve VolumeCurve;

		// Token: 0x0400258F RID: 9615
		public List<AudioClip> Clips = new List<AudioClip>();

		// Token: 0x04002590 RID: 9616
		private AudioSourceController audioSourceController;

		// Token: 0x04002591 RID: 9617
		private int currentClipIndex;

		// Token: 0x04002592 RID: 9618
		private float musicScale = 1f;
	}
}
