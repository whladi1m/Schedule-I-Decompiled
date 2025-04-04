using System;
using ScheduleOne.Audio;
using ScheduleOne.Stealth;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x0200099A RID: 2458
	public class SpottedTremolo : MonoBehaviour
	{
		// Token: 0x0600427F RID: 17023 RVA: 0x00116D3C File Offset: 0x00114F3C
		public void Update()
		{
			this.Intensity = ((this.PlayerVisibility.HighestVisionEvent != null) ? this.PlayerVisibility.HighestVisionEvent.NormalizedNoticeLevel : 0f);
			if (this.Intensity > this.smoothedIntensity)
			{
				this.smoothedIntensity = Mathf.MoveTowards(this.smoothedIntensity, this.Intensity, Time.deltaTime / this.SmoothTime);
			}
			else
			{
				this.smoothedIntensity = Mathf.MoveTowards(this.smoothedIntensity, this.Intensity, Time.deltaTime / 3f);
			}
			float num = Mathf.Lerp(this.MinVolume, this.MaxVolume, this.smoothedIntensity);
			this.Loop.VolumeMultiplier = num;
			this.Loop.PitchMultiplier = Mathf.Lerp(this.MinPitch, this.MaxPitch, this.smoothedIntensity);
			this.Loop.ApplyPitch();
			if (num > 0f && !this.Loop.isPlaying)
			{
				this.Loop.Play();
				return;
			}
			if (num <= 0f && this.Loop.isPlaying)
			{
				this.Loop.Stop();
			}
		}

		// Token: 0x0400306F RID: 12399
		[Range(0f, 1f)]
		public float Intensity;

		// Token: 0x04003070 RID: 12400
		public AudioSourceController Loop;

		// Token: 0x04003071 RID: 12401
		public PlayerVisibility PlayerVisibility;

		// Token: 0x04003072 RID: 12402
		[Header("Settings")]
		public float MinVolume;

		// Token: 0x04003073 RID: 12403
		public float MaxVolume = 1f;

		// Token: 0x04003074 RID: 12404
		public float MinPitch = 0.9f;

		// Token: 0x04003075 RID: 12405
		public float MaxPitch = 1.2f;

		// Token: 0x04003076 RID: 12406
		public float SmoothTime = 0.5f;

		// Token: 0x04003077 RID: 12407
		[Range(0f, 1f)]
		[SerializeField]
		private float smoothedIntensity;
	}
}
