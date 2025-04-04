using System;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200079D RID: 1949
	[RequireComponent(typeof(AudioSourceController))]
	public class MusicTrack : MonoBehaviour
	{
		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060034D8 RID: 13528 RVA: 0x000DE17D File Offset: 0x000DC37D
		// (set) Token: 0x060034D9 RID: 13529 RVA: 0x000DE185 File Offset: 0x000DC385
		public bool IsPlaying { get; private set; }

		// Token: 0x060034DA RID: 13530 RVA: 0x000DE18E File Offset: 0x000DC38E
		private void OnValidate()
		{
			base.gameObject.name = this.TrackName + " (" + this.Priority.ToString() + ")";
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000DE1BB File Offset: 0x000DC3BB
		public void Enable()
		{
			this.Enabled = true;
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000DE1C4 File Offset: 0x000DC3C4
		public void Disable()
		{
			this.Enabled = false;
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000DE1CD File Offset: 0x000DC3CD
		protected virtual void Awake()
		{
			this.volumeMultiplier = 0f;
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000DE1DC File Offset: 0x000DC3DC
		public virtual void Update()
		{
			if (this.IsPlaying && this.Controller.AudioSource.time >= this.Controller.AudioSource.clip.length - this.FadeOutTime && this.AutoFadeOut)
			{
				this.Stop();
				this.Disable();
			}
			if (this.IsPlaying)
			{
				this.volumeMultiplier = Mathf.Min(this.volumeMultiplier + Time.deltaTime / this.FadeInTime, 1f);
				this.Controller.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
				return;
			}
			this.volumeMultiplier = Mathf.Max(this.volumeMultiplier - Time.deltaTime / this.FadeOutTime, 0f);
			this.Controller.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
			if (this.Controller.VolumeMultiplier == 0f)
			{
				this.Controller.AudioSource.Stop();
			}
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000DE2D4 File Offset: 0x000DC4D4
		public virtual void Play()
		{
			this.IsPlaying = true;
			this.Controller.Play();
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000DE2E8 File Offset: 0x000DC4E8
		public virtual void Stop()
		{
			this.IsPlaying = false;
		}

		// Token: 0x0400261F RID: 9759
		public bool Enabled;

		// Token: 0x04002620 RID: 9760
		public string TrackName = "Track";

		// Token: 0x04002621 RID: 9761
		public int Priority = 1;

		// Token: 0x04002622 RID: 9762
		public float FadeInTime = 1f;

		// Token: 0x04002623 RID: 9763
		public float FadeOutTime = 2f;

		// Token: 0x04002624 RID: 9764
		public AudioSourceController Controller;

		// Token: 0x04002625 RID: 9765
		public float VolumeMultiplier = 1f;

		// Token: 0x04002626 RID: 9766
		public bool AutoFadeOut = true;

		// Token: 0x04002627 RID: 9767
		protected float volumeMultiplier = 1f;
	}
}
