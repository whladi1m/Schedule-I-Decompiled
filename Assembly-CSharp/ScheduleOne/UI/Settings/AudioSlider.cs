using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A69 RID: 2665
	public class AudioSlider : SettingsSlider
	{
		// Token: 0x060047F2 RID: 18418 RVA: 0x0012DDE0 File Offset: 0x0012BFE0
		protected virtual void Start()
		{
			if (this.Master)
			{
				this.slider.SetValueWithoutNotify(Singleton<AudioManager>.Instance.MasterVolume / 0.01f);
				return;
			}
			this.slider.SetValueWithoutNotify(Singleton<AudioManager>.Instance.GetVolume(this.AudioType, false) / 0.01f);
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0012DE34 File Offset: 0x0012C034
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			if (this.Master)
			{
				Singleton<Settings>.Instance.AudioSettings.MasterVolume = value * 0.01f;
			}
			else
			{
				switch (this.AudioType)
				{
				case EAudioType.Ambient:
					Singleton<Settings>.Instance.AudioSettings.AmbientVolume = value * 0.01f;
					break;
				case EAudioType.Footsteps:
					Singleton<Settings>.Instance.AudioSettings.FootstepsVolume = value * 0.01f;
					break;
				case EAudioType.FX:
					Singleton<Settings>.Instance.AudioSettings.SFXVolume = value * 0.01f;
					break;
				case EAudioType.UI:
					Singleton<Settings>.Instance.AudioSettings.UIVolume = value * 0.01f;
					break;
				case EAudioType.Music:
					Singleton<Settings>.Instance.AudioSettings.MusicVolume = value * 0.01f;
					break;
				case EAudioType.Voice:
					Singleton<Settings>.Instance.AudioSettings.DialogueVolume = value * 0.01f;
					break;
				}
			}
			Singleton<Settings>.Instance.ReloadAudioSettings();
			Singleton<Settings>.Instance.WriteAudioSettings(Singleton<Settings>.Instance.AudioSettings);
		}

		// Token: 0x0400358E RID: 13710
		public const float MULTIPLIER = 0.01f;

		// Token: 0x0400358F RID: 13711
		public bool Master;

		// Token: 0x04003590 RID: 13712
		public EAudioType AudioType = EAudioType.FX;
	}
}
