using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Audio
{
	// Token: 0x02000788 RID: 1928
	[RequireComponent(typeof(AudioSource))]
	public class AudioSourceController : MonoBehaviour
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000DCBA3 File Offset: 0x000DADA3
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x000DCBAB File Offset: 0x000DADAB
		public float Volume { get; protected set; } = 1f;

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000DCBB4 File Offset: 0x000DADB4
		public bool isPlaying
		{
			get
			{
				return this.AudioSource.isPlaying;
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000DCBC1 File Offset: 0x000DADC1
		private void Awake()
		{
			this.DoPauseStuff();
			this.basePitch = this.AudioSource.pitch;
			this.AudioSource.volume = 0f;
			if (this.AudioSource.playOnAwake)
			{
				this.isPlayingCached = true;
			}
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x000DCC00 File Offset: 0x000DAE00
		private void Start()
		{
			this.SetVolume(this.DefaultVolume);
			Singleton<AudioManager>.Instance.onSettingsChanged.AddListener(new UnityAction(this.ApplyVolume));
			if (this.AudioType == EAudioType.Music)
			{
				this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MusicMixer;
				return;
			}
			if (SceneManager.GetActiveScene().name == "Main")
			{
				this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MainGameMixer;
				return;
			}
			this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MenuMixer;
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000DCC98 File Offset: 0x000DAE98
		private void DoPauseStuff()
		{
			if (Singleton<PauseMenu>.InstanceExists)
			{
				Singleton<PauseMenu>.Instance.onPause.RemoveListener(new UnityAction(this.Pause));
				Singleton<PauseMenu>.Instance.onPause.AddListener(new UnityAction(this.Pause));
				Singleton<PauseMenu>.Instance.onResume.RemoveListener(new UnityAction(this.Unpause));
				Singleton<PauseMenu>.Instance.onResume.AddListener(new UnityAction(this.Pause));
			}
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000DCD18 File Offset: 0x000DAF18
		private void OnDestroy()
		{
			if (Singleton<AudioManager>.Instance != null)
			{
				Singleton<AudioManager>.Instance.onSettingsChanged.RemoveListener(new UnityAction(this.ApplyVolume));
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000DCD42 File Offset: 0x000DAF42
		private void OnValidate()
		{
			if (this.AudioSource == null)
			{
				this.AudioSource = base.GetComponent<AudioSource>();
			}
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x000DCD5E File Offset: 0x000DAF5E
		private void FixedUpdate()
		{
			if (this.isPlayingCached)
			{
				this.ApplyVolume();
				if (!this.AudioSource.isPlaying && !this.paused)
				{
					this.isPlayingCached = false;
				}
			}
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x000DCD8A File Offset: 0x000DAF8A
		private void Pause()
		{
			this.paused = true;
			this.AudioSource.Pause();
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000DCD9E File Offset: 0x000DAF9E
		private void Unpause()
		{
			this.paused = false;
			this.AudioSource.UnPause();
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000DCDB2 File Offset: 0x000DAFB2
		public void SetVolume(float volume)
		{
			this.Volume = volume;
			this.ApplyVolume();
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000DCDC4 File Offset: 0x000DAFC4
		public void ApplyVolume()
		{
			if (!Singleton<AudioManager>.InstanceExists)
			{
				return;
			}
			if (this.DEBUG)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Applying volume: ",
					this.Volume.ToString(),
					" * ",
					Singleton<AudioManager>.Instance.GetVolume(this.AudioType, true).ToString(),
					" * ",
					this.VolumeMultiplier.ToString()
				}));
			}
			this.AudioSource.volume = this.Volume * Singleton<AudioManager>.Instance.GetVolume(this.AudioType, true) * this.VolumeMultiplier;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000DCE70 File Offset: 0x000DB070
		public void ApplyPitch()
		{
			if (this.RandomizePitch)
			{
				this.AudioSource.pitch = UnityEngine.Random.Range(this.MinPitch, this.MaxPitch) * this.PitchMultiplier;
				return;
			}
			this.AudioSource.pitch = this.basePitch * this.PitchMultiplier;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000DCEC1 File Offset: 0x000DB0C1
		public virtual void Play()
		{
			this.ApplyPitch();
			this.ApplyVolume();
			this.isPlayingCached = true;
			this.AudioSource.Play();
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000DCEE4 File Offset: 0x000DB0E4
		public virtual void PlayOneShot(bool duplicateAudioSource = false)
		{
			if (this.RandomizePitch)
			{
				this.AudioSource.pitch = UnityEngine.Random.Range(this.MinPitch, this.MaxPitch) * this.PitchMultiplier;
			}
			this.ApplyVolume();
			if (!duplicateAudioSource)
			{
				this.AudioSource.PlayOneShot(this.AudioSource.clip, 1f);
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, NetworkSingleton<GameManager>.Instance.Temp);
			gameObject.transform.position = base.transform.position;
			gameObject.GetComponent<AudioSourceController>().PlayOneShot(false);
			if (this.AudioSource.clip != null)
			{
				UnityEngine.Object.Destroy(gameObject, this.AudioSource.clip.length + 0.1f);
				return;
			}
			UnityEngine.Object.Destroy(gameObject, 5f);
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000DCFB4 File Offset: 0x000DB1B4
		public void Stop()
		{
			this.AudioSource.Stop();
		}

		// Token: 0x040025BC RID: 9660
		public bool DEBUG;

		// Token: 0x040025BE RID: 9662
		public AudioSource AudioSource;

		// Token: 0x040025BF RID: 9663
		[Header("Settings")]
		public EAudioType AudioType;

		// Token: 0x040025C0 RID: 9664
		[Range(0f, 1f)]
		public float DefaultVolume = 1f;

		// Token: 0x040025C1 RID: 9665
		public bool RandomizePitch;

		// Token: 0x040025C2 RID: 9666
		public float MinPitch = 0.9f;

		// Token: 0x040025C3 RID: 9667
		public float MaxPitch = 1.1f;

		// Token: 0x040025C4 RID: 9668
		[Range(0f, 2f)]
		public float VolumeMultiplier = 1f;

		// Token: 0x040025C5 RID: 9669
		[Range(0f, 2f)]
		public float PitchMultiplier = 1f;

		// Token: 0x040025C6 RID: 9670
		private bool paused;

		// Token: 0x040025C7 RID: 9671
		private bool isPlayingCached;

		// Token: 0x040025C8 RID: 9672
		private float basePitch = 1f;
	}
}
