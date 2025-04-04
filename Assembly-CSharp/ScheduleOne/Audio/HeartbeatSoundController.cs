using System;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000796 RID: 1942
	public class HeartbeatSoundController : MonoBehaviour
	{
		// Token: 0x060034BE RID: 13502 RVA: 0x000DDB8A File Offset: 0x000DBD8A
		private void Awake()
		{
			this.VolumeController.Initialize();
			this.VolumeController.SetDefault(0f);
			this.PitchController.Initialize();
			this.PitchController.SetDefault(1f);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000DDBC4 File Offset: 0x000DBDC4
		private void Update()
		{
			this.sound.VolumeMultiplier = this.VolumeController.CurrentValue;
			this.sound.PitchMultiplier = this.PitchController.CurrentValue;
			this.sound.ApplyPitch();
			if (this.sound.VolumeMultiplier > 0f)
			{
				if (!this.sound.isPlaying)
				{
					this.sound.Play();
					return;
				}
			}
			else if (this.sound.isPlaying)
			{
				this.sound.Stop();
			}
		}

		// Token: 0x04002603 RID: 9731
		public AudioSourceController sound;

		// Token: 0x04002604 RID: 9732
		public FloatSmoother VolumeController;

		// Token: 0x04002605 RID: 9733
		public FloatSmoother PitchController;
	}
}
