using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000784 RID: 1924
	public class AmbientOneShot : MonoBehaviour
	{
		// Token: 0x0600345D RID: 13405 RVA: 0x000DC382 File Offset: 0x000DA582
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000DC3AC File Offset: 0x000DA5AC
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position, this.MinDistance);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.MaxDistance);
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000DC3FC File Offset: 0x000DA5FC
		private void MinPass()
		{
			this.timeSinceLastPlay++;
			if (this.timeSinceLastPlay < this.CooldownTime)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
			{
				return;
			}
			if (this.PlayTime == AmbientOneShot.EPlayTime.Day && NetworkSingleton<TimeManager>.Instance.IsNight)
			{
				return;
			}
			if (this.PlayTime == AmbientOneShot.EPlayTime.Night && !NetworkSingleton<TimeManager>.Instance.IsNight)
			{
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.MinDistance)
			{
				return;
			}
			if (num > this.MaxDistance)
			{
				return;
			}
			if (UnityEngine.Random.value < this.ChancePerHour / 60f)
			{
				this.Play();
			}
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x000DC4AC File Offset: 0x000DA6AC
		private void Play()
		{
			this.timeSinceLastPlay = 0;
			this.Audio.SetVolume(this.Volume);
			this.Audio.Play();
		}

		// Token: 0x04002593 RID: 9619
		public AudioSourceController Audio;

		// Token: 0x04002594 RID: 9620
		[Header("Settings")]
		[Range(0f, 1f)]
		public float Volume = 0.2f;

		// Token: 0x04002595 RID: 9621
		[Range(0f, 1f)]
		public float ChancePerHour = 0.2f;

		// Token: 0x04002596 RID: 9622
		public int CooldownTime = 60;

		// Token: 0x04002597 RID: 9623
		public AmbientOneShot.EPlayTime PlayTime;

		// Token: 0x04002598 RID: 9624
		public float MinDistance = 20f;

		// Token: 0x04002599 RID: 9625
		public float MaxDistance = 100f;

		// Token: 0x0400259A RID: 9626
		private int timeSinceLastPlay;

		// Token: 0x02000785 RID: 1925
		public enum EPlayTime
		{
			// Token: 0x0400259C RID: 9628
			All,
			// Token: 0x0400259D RID: 9629
			Day,
			// Token: 0x0400259E RID: 9630
			Night
		}
	}
}
