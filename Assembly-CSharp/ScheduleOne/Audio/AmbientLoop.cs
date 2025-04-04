using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000782 RID: 1922
	[RequireComponent(typeof(AudioSourceController))]
	public class AmbientLoop : MonoBehaviour
	{
		// Token: 0x06003456 RID: 13398 RVA: 0x000DC149 File Offset: 0x000DA349
		private void Start()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
			this.audioSourceController.Play();
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x000DC164 File Offset: 0x000DA364
		private void Update()
		{
			if (this.FadeDuringMusic)
			{
				if (Singleton<MusicPlayer>.Instance.IsPlaying)
				{
					this.musicScale = Mathf.Lerp(this.musicScale, 0.3f, Time.deltaTime / 4f);
				}
				else
				{
					this.musicScale = Mathf.Lerp(this.musicScale, 1f, Time.deltaTime / 4f);
				}
			}
			else
			{
				this.musicScale = 1f;
			}
			float num = this.VolumeCurve.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.audioSourceController.VolumeMultiplier = num * this.musicScale;
		}

		// Token: 0x04002588 RID: 9608
		public const float MUSIC_FADE_MULTIPLIER = 0.3f;

		// Token: 0x04002589 RID: 9609
		public const float MUSIC_FADE_TIME = 4f;

		// Token: 0x0400258A RID: 9610
		public AnimationCurve VolumeCurve;

		// Token: 0x0400258B RID: 9611
		public bool FadeDuringMusic = true;

		// Token: 0x0400258C RID: 9612
		private AudioSourceController audioSourceController;

		// Token: 0x0400258D RID: 9613
		private float musicScale = 1f;
	}
}
