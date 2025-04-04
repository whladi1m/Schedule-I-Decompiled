using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Audio;

namespace ScheduleOne.Audio
{
	// Token: 0x02000799 RID: 1945
	public class MusicPlayer : PersistentSingleton<MusicPlayer>
	{
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000DDDE1 File Offset: 0x000DBFE1
		public bool IsPlaying
		{
			get
			{
				return this._currentTrack != null && this._currentTrack.IsPlaying;
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000DDE00 File Offset: 0x000DC000
		public void OnValidate()
		{
			this.Tracks = new List<MusicTrack>(base.GetComponentsInChildren<MusicTrack>());
			for (int i = 0; i < this.Tracks.Count - 1; i++)
			{
				if (this.Tracks[i].Priority > this.Tracks[i + 1].Priority)
				{
					this.Tracks[i].transform.SetSiblingIndex(i + 1);
				}
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000DDE78 File Offset: 0x000DC078
		protected override void Start()
		{
			base.Start();
			if (Singleton<MusicPlayer>.Instance == null || Singleton<MusicPlayer>.Instance != this)
			{
				return;
			}
			base.InvokeRepeating("UpdateTracks", 0f, 0.2f);
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				this.SetMusicDistorted(false, 0.5f);
			});
			this.DefaultSnapshot.TransitionTo(0.1f);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000DDEE6 File Offset: 0x000DC0E6
		private void Update()
		{
			MusicPlayer.TimeSinceLastAmbientTrack += Time.unscaledDeltaTime;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000DDEF8 File Offset: 0x000DC0F8
		public void SetMusicDistorted(bool distorted, float transition = 5f)
		{
			if (distorted)
			{
				this.DistortedSnapshot.TransitionTo(transition);
				return;
			}
			this.DefaultSnapshot.TransitionTo(transition);
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000DDF18 File Offset: 0x000DC118
		public void SetTrackEnabled(string trackName, bool enabled)
		{
			MusicTrack musicTrack = this.Tracks.Find((MusicTrack t) => t.TrackName == trackName);
			if (musicTrack == null)
			{
				Console.LogWarning("Music track not found: " + trackName, null);
				return;
			}
			if (enabled)
			{
				musicTrack.Enable();
				return;
			}
			musicTrack.Disable();
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000DDF7C File Offset: 0x000DC17C
		public void StopTrack(string trackName)
		{
			MusicTrack musicTrack = this.Tracks.Find((MusicTrack t) => t.TrackName == trackName);
			if (musicTrack == null)
			{
				Console.LogWarning("Music track not found: " + trackName, null);
				return;
			}
			musicTrack.Stop();
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000DDFD4 File Offset: 0x000DC1D4
		public void StopAndDisableTracks()
		{
			foreach (MusicTrack musicTrack in this.Tracks)
			{
				musicTrack.Disable();
				musicTrack.Stop();
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000DE02C File Offset: 0x000DC22C
		private void UpdateTracks()
		{
			if (this._currentTrack != null && !this._currentTrack.IsPlaying)
			{
				this._currentTrack = null;
			}
			MusicTrack musicTrack = null;
			foreach (MusicTrack musicTrack2 in this.Tracks)
			{
				if (musicTrack2.Enabled && (musicTrack == null || musicTrack2.Priority > musicTrack.Priority))
				{
					musicTrack = musicTrack2;
				}
			}
			if (this._currentTrack != musicTrack && musicTrack != null)
			{
				if (this._currentTrack != null)
				{
					this._currentTrack.Stop();
				}
				this._currentTrack = musicTrack;
				if (this._currentTrack != null)
				{
					this._currentTrack.Play();
				}
			}
		}

		// Token: 0x04002616 RID: 9750
		public static float TimeSinceLastAmbientTrack = 100000f;

		// Token: 0x04002617 RID: 9751
		public List<MusicTrack> Tracks = new List<MusicTrack>();

		// Token: 0x04002618 RID: 9752
		public AudioMixerGroup MusicMixer;

		// Token: 0x04002619 RID: 9753
		public AudioMixerSnapshot DefaultSnapshot;

		// Token: 0x0400261A RID: 9754
		public AudioMixerSnapshot DistortedSnapshot;

		// Token: 0x0400261B RID: 9755
		private MusicTrack _currentTrack;
	}
}
