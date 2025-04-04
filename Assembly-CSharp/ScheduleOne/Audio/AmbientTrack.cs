using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000786 RID: 1926
	public class AmbientTrack : MonoBehaviour
	{
		// Token: 0x06003462 RID: 13410 RVA: 0x000DC510 File Offset: 0x000DA710
		private void Awake()
		{
			for (int i = 0; i < this.Tracks.Count; i++)
			{
				int index = UnityEngine.Random.Range(i, this.Tracks.Count);
				MusicTrack value = this.Tracks[index];
				this.Tracks[index] = this.Tracks[i];
				this.Tracks[i] = value;
			}
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000DC578 File Offset: 0x000DA778
		[Button]
		public void ForcePlay()
		{
			AmbientTrack.LastPlayedTrack = this;
			MusicPlayer.TimeSinceLastAmbientTrack = 0f;
			this.playTrack = false;
			AmbientTrack.TrackQueued = false;
			this.Tracks[0].Enable();
			this.Tracks.Add(this.Tracks[0]);
			this.Tracks.RemoveAt(0);
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000DC5D6 File Offset: 0x000DA7D6
		public void Stop()
		{
			this.Tracks[0].Disable();
			this.Tracks[0].Stop();
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000DC5FC File Offset: 0x000DA7FC
		private void Update()
		{
			if (!NetworkSingleton<TimeManager>.InstanceExists)
			{
				this.trackRandomized = false;
				AmbientTrack.TrackQueued = false;
				return;
			}
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.MinTime, this.MaxTime))
			{
				if (!this.trackRandomized)
				{
					this.playTrack = (UnityEngine.Random.value < this.Chance && MusicPlayer.TimeSinceLastAmbientTrack > 540f && AmbientTrack.LastPlayedTrack != this && !AmbientTrack.TrackQueued && this.Tracks.Count > 0 && Time.timeSinceLevelLoad > 20f && !GameManager.IS_TUTORIAL);
					this.startTime = TimeManager.AddMinutesTo24HourTime(currentTime, UnityEngine.Random.Range(0, 120));
					if (this.playTrack)
					{
						Console.Log("Will play " + this.Tracks[0].TrackName + " at " + this.startTime.ToString(), null);
						AmbientTrack.TrackQueued = true;
						MusicPlayer.TimeSinceLastAmbientTrack = 0f;
					}
					this.trackRandomized = true;
				}
				if (this.playTrack && !this.Tracks[0].Enabled && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.startTime, this.MaxTime))
				{
					AmbientTrack.LastPlayedTrack = this;
					MusicPlayer.TimeSinceLastAmbientTrack = 0f;
					this.playTrack = false;
					AmbientTrack.TrackQueued = false;
					this.Tracks[0].Enable();
					this.Tracks.Add(this.Tracks[0]);
					this.Tracks.RemoveAt(0);
					return;
				}
			}
			else
			{
				this.trackRandomized = false;
				this.playTrack = false;
				foreach (MusicTrack musicTrack in this.Tracks)
				{
					musicTrack.Disable();
				}
			}
		}

		// Token: 0x0400259F RID: 9631
		public const float MIN_TIME_BETWEEN_AMBIENT_TRACKS = 540f;

		// Token: 0x040025A0 RID: 9632
		public static AmbientTrack LastPlayedTrack;

		// Token: 0x040025A1 RID: 9633
		public static bool TrackQueued;

		// Token: 0x040025A2 RID: 9634
		public List<MusicTrack> Tracks = new List<MusicTrack>();

		// Token: 0x040025A3 RID: 9635
		public int MinTime;

		// Token: 0x040025A4 RID: 9636
		public int MaxTime;

		// Token: 0x040025A5 RID: 9637
		public float Chance = 0.3f;

		// Token: 0x040025A6 RID: 9638
		private int startTime;

		// Token: 0x040025A7 RID: 9639
		private bool playTrack;

		// Token: 0x040025A8 RID: 9640
		private bool trackRandomized;
	}
}
