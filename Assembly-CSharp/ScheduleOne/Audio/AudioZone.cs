using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200078A RID: 1930
	public class AudioZone : Zone
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x000DD024 File Offset: 0x000DB224
		// (set) Token: 0x06003492 RID: 13458 RVA: 0x000DD02C File Offset: 0x000DB22C
		public float VolumeModifier { get; set; }

		// Token: 0x06003493 RID: 13459 RVA: 0x000DD038 File Offset: 0x000DB238
		private void Start()
		{
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.Init();
			}
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000DD0B0 File Offset: 0x000DB2B0
		private void Update()
		{
			this.VolumeModifier = Mathf.MoveTowards(this.VolumeModifier, this.GetFalloffFactor(base.LocalPlayerDistance), 1f * Time.deltaTime);
			this.CurrentVolumeMultiplier = Mathf.MoveTowards(this.CurrentVolumeMultiplier, this.GetTotalVolumeMultiplier(), 1f * Time.deltaTime);
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.Update(this.VolumeModifier * this.CurrentVolumeMultiplier);
			}
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000DD158 File Offset: 0x000DB358
		private float GetTotalVolumeMultiplier()
		{
			float num = 1f;
			foreach (KeyValuePair<AudioZoneModifierVolume, float> keyValuePair in this.Modifiers)
			{
				num *= keyValuePair.Value;
			}
			return num;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000DD1B8 File Offset: 0x000DB3B8
		private void MinPass()
		{
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.UpdateTimeMultiplier(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			}
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000DD214 File Offset: 0x000DB414
		public void AddModifier(AudioZoneModifierVolume modifier, float value)
		{
			if (!this.Modifiers.ContainsKey(modifier))
			{
				this.Modifiers.Add(modifier, value);
			}
			this.Modifiers[modifier] = value;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000DD23E File Offset: 0x000DB43E
		public void RemoveModifier(AudioZoneModifierVolume modifier)
		{
			if (this.Modifiers.ContainsKey(modifier))
			{
				this.Modifiers.Remove(modifier);
			}
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000DD25B File Offset: 0x000DB45B
		private float GetFalloffFactor(float distance)
		{
			if (distance > this.MaxDistance)
			{
				return 0f;
			}
			return 1f / (1f + 0.5f * distance);
		}

		// Token: 0x040025D0 RID: 9680
		public const float VOLUME_CHANGE_RATE = 1f;

		// Token: 0x040025D1 RID: 9681
		public const float ROLLOFF_SCALE = 0.5f;

		// Token: 0x040025D2 RID: 9682
		[Header("Settings")]
		[Range(1f, 200f)]
		public float MaxDistance = 100f;

		// Token: 0x040025D3 RID: 9683
		public List<AudioZone.Track> Tracks = new List<AudioZone.Track>();

		// Token: 0x040025D4 RID: 9684
		public Dictionary<AudioZoneModifierVolume, float> Modifiers = new Dictionary<AudioZoneModifierVolume, float>();

		// Token: 0x040025D6 RID: 9686
		protected float CurrentVolumeMultiplier = 1f;

		// Token: 0x0200078B RID: 1931
		[Serializable]
		public class Track
		{
			// Token: 0x0600349B RID: 13467 RVA: 0x000DD2B4 File Offset: 0x000DB4B4
			public void Init()
			{
				this.fadeInStart = TimeManager.AddMinutesTo24HourTime(this.StartTime, -this.FadeTime / 2);
				this.fadeInEnd = TimeManager.AddMinutesTo24HourTime(this.StartTime, this.FadeTime / 2);
				this.fadeOutStart = TimeManager.AddMinutesTo24HourTime(this.EndTime, -this.FadeTime / 2);
				this.fadeOutEnd = TimeManager.AddMinutesTo24HourTime(this.EndTime, this.FadeTime / 2);
				this.fadeInStartMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeInStart);
				this.fadeInEndMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeInEnd);
				this.fadeOutStartMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeOutStart);
				this.fadeOutEndMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeOutEnd);
			}

			// Token: 0x0600349C RID: 13468 RVA: 0x000DD36C File Offset: 0x000DB56C
			public void Update(float multiplier)
			{
				float num = this.Volume * multiplier * this.timeVolMultiplier;
				this.Source.SetVolume(num);
				if (num > 0f)
				{
					if (!this.Source.isPlaying)
					{
						this.Source.Play();
						return;
					}
				}
				else if (this.Source.isPlaying)
				{
					this.Source.Stop();
				}
			}

			// Token: 0x0600349D RID: 13469 RVA: 0x000DD3D0 File Offset: 0x000DB5D0
			public void UpdateTimeMultiplier(int time)
			{
				int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(time);
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeInEnd, this.fadeOutStart))
				{
					this.timeVolMultiplier = 1f;
					return;
				}
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeInStart, this.fadeInEnd))
				{
					this.timeVolMultiplier = (float)(minSumFrom24HourTime - this.fadeInStartMinSum) / (float)(this.fadeInEndMinSum - this.fadeInStartMinSum);
					return;
				}
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeOutStart, this.fadeOutEnd))
				{
					this.timeVolMultiplier = 1f - (float)(minSumFrom24HourTime - this.fadeOutStartMinSum) / (float)(this.fadeOutEndMinSum - this.fadeOutStartMinSum);
					return;
				}
				this.timeVolMultiplier = 0f;
			}

			// Token: 0x040025D7 RID: 9687
			public AudioSourceController Source;

			// Token: 0x040025D8 RID: 9688
			[Range(0.01f, 2f)]
			public float Volume = 1f;

			// Token: 0x040025D9 RID: 9689
			public int StartTime;

			// Token: 0x040025DA RID: 9690
			public int EndTime;

			// Token: 0x040025DB RID: 9691
			public int FadeTime = 60;

			// Token: 0x040025DC RID: 9692
			private float timeVolMultiplier;

			// Token: 0x040025DD RID: 9693
			private int fadeInStart;

			// Token: 0x040025DE RID: 9694
			private int fadeInEnd;

			// Token: 0x040025DF RID: 9695
			private int fadeOutStart;

			// Token: 0x040025E0 RID: 9696
			private int fadeOutEnd;

			// Token: 0x040025E1 RID: 9697
			private int fadeInStartMinSum;

			// Token: 0x040025E2 RID: 9698
			private int fadeInEndMinSum;

			// Token: 0x040025E3 RID: 9699
			private int fadeOutStartMinSum;

			// Token: 0x040025E4 RID: 9700
			private int fadeOutEndMinSum;
		}
	}
}
