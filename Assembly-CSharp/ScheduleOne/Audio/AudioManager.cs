using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Audio
{
	// Token: 0x02000787 RID: 1927
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x000DC80A File Offset: 0x000DAA0A
		public float MasterVolume
		{
			get
			{
				return this.masterVolume;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06003468 RID: 13416 RVA: 0x000DC812 File Offset: 0x000DAA12
		public float AmbientVolume
		{
			get
			{
				return this.ambientVolume * this.masterVolume;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x000DC821 File Offset: 0x000DAA21
		public float UnscaledAmbientVolume
		{
			get
			{
				return this.ambientVolume;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600346A RID: 13418 RVA: 0x000DC829 File Offset: 0x000DAA29
		public float FootstepsVolume
		{
			get
			{
				return this.footstepsVolume * this.masterVolume;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x000DC838 File Offset: 0x000DAA38
		public float UnscaledFootstepsVolume
		{
			get
			{
				return this.footstepsVolume;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x000DC840 File Offset: 0x000DAA40
		public float FXVolume
		{
			get
			{
				return this.fxVolume * this.masterVolume;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x000DC84F File Offset: 0x000DAA4F
		public float UnscaledFXVolume
		{
			get
			{
				return this.fxVolume;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600346E RID: 13422 RVA: 0x000DC857 File Offset: 0x000DAA57
		public float UIVolume
		{
			get
			{
				return this.uiVolume * this.masterVolume;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x000DC866 File Offset: 0x000DAA66
		public float UnscaledUIVolume
		{
			get
			{
				return this.uiVolume;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06003470 RID: 13424 RVA: 0x000DC86E File Offset: 0x000DAA6E
		public float MusicVolume
		{
			get
			{
				return this.musicVolume * this.masterVolume * 0.7f;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x000DC883 File Offset: 0x000DAA83
		public float UnscaledMusicVolume
		{
			get
			{
				return this.musicVolume;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x000DC88B File Offset: 0x000DAA8B
		public float VoiceVolume
		{
			get
			{
				return this.voiceVolume * this.masterVolume * 0.5f;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x000DC8A0 File Offset: 0x000DAAA0
		public float UnscaledVoiceVolume
		{
			get
			{
				return this.voiceVolume;
			}
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000DC8A8 File Offset: 0x000DAAA8
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<AudioManager>.Instance == null || Singleton<AudioManager>.Instance != this)
			{
				return;
			}
			this.SetGameVolume(0f);
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000DC8D6 File Offset: 0x000DAAD6
		protected override void Start()
		{
			base.Start();
			if (Singleton<AudioManager>.Instance == null || Singleton<AudioManager>.Instance != this)
			{
				return;
			}
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				this.SetDistorted(false, 0.5f);
			});
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000DC914 File Offset: 0x000DAB14
		protected void Update()
		{
			if (SceneManager.GetActiveScene().name == "Main" && !Singleton<LoadingScreen>.Instance.IsOpen)
			{
				if (this.currentGameVolume < 1f)
				{
					this.SetGameVolume(this.currentGameVolume + Time.deltaTime * 1f);
					return;
				}
			}
			else if (this.currentGameVolume > 0f)
			{
				this.SetGameVolume(this.currentGameVolume - Time.deltaTime * 1f);
			}
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x000DC991 File Offset: 0x000DAB91
		public void SetGameVolumeMultipler(float value)
		{
			this.gameVolumeMultiplier = value;
			this.SetGameVolume(this.currentGameVolume);
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000DC9A6 File Offset: 0x000DABA6
		public void SetDistorted(bool distorted, float transition = 5f)
		{
			if (distorted)
			{
				this.DistortedSnapshot.TransitionTo(transition);
				return;
			}
			this.DefaultSnapshot.TransitionTo(transition);
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000DC9C4 File Offset: 0x000DABC4
		private void SetGameVolume(float value)
		{
			this.currentGameVolume = value;
			value = Mathf.Lerp(value * this.gameVolumeMultiplier, 0.0001f, 0.0001f);
			this.MainGameMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000DCA14 File Offset: 0x000DAC14
		public float GetVolume(EAudioType audioType, bool scaled = true)
		{
			switch (audioType)
			{
			case EAudioType.Ambient:
				if (!scaled)
				{
					return this.UnscaledAmbientVolume;
				}
				return this.AmbientVolume;
			case EAudioType.Footsteps:
				if (!scaled)
				{
					return this.UnscaledFootstepsVolume;
				}
				return this.FootstepsVolume;
			case EAudioType.FX:
				if (!scaled)
				{
					return this.UnscaledFXVolume;
				}
				return this.FXVolume;
			case EAudioType.UI:
				if (!scaled)
				{
					return this.UnscaledUIVolume;
				}
				return this.UIVolume;
			case EAudioType.Music:
				if (!scaled)
				{
					return this.UnscaledMusicVolume;
				}
				return this.MusicVolume;
			case EAudioType.Voice:
				if (!scaled)
				{
					return this.UnscaledVoiceVolume;
				}
				return this.VoiceVolume;
			default:
				return 1f;
			}
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000DCAAC File Offset: 0x000DACAC
		public void SetMasterVolume(float volume)
		{
			this.masterVolume = volume;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000DCAB8 File Offset: 0x000DACB8
		public void SetVolume(EAudioType type, float volume)
		{
			switch (type)
			{
			case EAudioType.Ambient:
				this.ambientVolume = volume;
				return;
			case EAudioType.Footsteps:
				this.footstepsVolume = volume;
				return;
			case EAudioType.FX:
				this.fxVolume = volume;
				return;
			case EAudioType.UI:
				this.uiVolume = volume;
				return;
			case EAudioType.Music:
				this.musicVolume = volume;
				return;
			case EAudioType.Voice:
				this.voiceVolume = volume;
				return;
			default:
				return;
			}
		}

		// Token: 0x040025A9 RID: 9641
		[Range(0f, 2f)]
		[SerializeField]
		protected float masterVolume = 1f;

		// Token: 0x040025AA RID: 9642
		[Range(0f, 2f)]
		[SerializeField]
		protected float ambientVolume = 1f;

		// Token: 0x040025AB RID: 9643
		[Range(0f, 2f)]
		[SerializeField]
		protected float footstepsVolume = 1f;

		// Token: 0x040025AC RID: 9644
		[Range(0f, 2f)]
		[SerializeField]
		protected float fxVolume = 1f;

		// Token: 0x040025AD RID: 9645
		[Range(0f, 2f)]
		[SerializeField]
		protected float uiVolume = 1f;

		// Token: 0x040025AE RID: 9646
		[Range(0f, 2f)]
		[SerializeField]
		protected float musicVolume = 1f;

		// Token: 0x040025AF RID: 9647
		[Range(0f, 2f)]
		[SerializeField]
		protected float voiceVolume = 1f;

		// Token: 0x040025B0 RID: 9648
		public UnityEvent onSettingsChanged = new UnityEvent();

		// Token: 0x040025B1 RID: 9649
		[Header("Generic Door Sounds")]
		public AudioSourceController DoorOpen;

		// Token: 0x040025B2 RID: 9650
		public AudioSourceController DoorClose;

		// Token: 0x040025B3 RID: 9651
		[Header("Mixers")]
		public AudioMixerGroup MainGameMixer;

		// Token: 0x040025B4 RID: 9652
		public AudioMixerGroup MenuMixer;

		// Token: 0x040025B5 RID: 9653
		public AudioMixerGroup MusicMixer;

		// Token: 0x040025B6 RID: 9654
		private float currentGameVolume = 1f;

		// Token: 0x040025B7 RID: 9655
		private const float minGameVolume = 0.0001f;

		// Token: 0x040025B8 RID: 9656
		private const float maxGameVolume = 0.0001f;

		// Token: 0x040025B9 RID: 9657
		private float gameVolumeMultiplier = 1f;

		// Token: 0x040025BA RID: 9658
		public AudioMixerSnapshot DefaultSnapshot;

		// Token: 0x040025BB RID: 9659
		public AudioMixerSnapshot DistortedSnapshot;
	}
}
