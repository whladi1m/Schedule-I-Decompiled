using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.FX
{
	// Token: 0x0200060F RID: 1551
	public class CountdownExplosion : MonoBehaviour
	{
		// Token: 0x060028DB RID: 10459 RVA: 0x000A88B1 File Offset: 0x000A6AB1
		public void Trigger()
		{
			this.countdownRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Trigger>g__Routine|5_0());
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000A88C9 File Offset: 0x000A6AC9
		public void StopCountdown()
		{
			if (this.countdownRoutine != null)
			{
				base.StopCoroutine(this.countdownRoutine);
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000A88DF File Offset: 0x000A6ADF
		[CompilerGenerated]
		private IEnumerator <Trigger>g__Routine|5_0()
		{
			float timeUntilNextTick = 1f;
			for (float i = 0f; i < 30f; i += Time.deltaTime)
			{
				timeUntilNextTick -= Time.deltaTime;
				if (timeUntilNextTick <= 0f)
				{
					timeUntilNextTick = Mathf.Lerp(1f, 0.1f, i / 30f);
					this.TickSound.PitchMultiplier = Mathf.Lerp(1f, 1.1f, i / 30f);
					this.TickSound.VolumeMultiplier = Mathf.Lerp(0.6f, 1f, i / 30f);
					this.TickSound.Play();
				}
				yield return new WaitForEndOfFrame();
			}
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<CombatManager>.Instance.CreateExplosion(base.transform.position, ExplosionData.DefaultSmall);
			}
			this.countdownRoutine = null;
			yield break;
		}

		// Token: 0x04001E0A RID: 7690
		public const float COUNTDOWN = 30f;

		// Token: 0x04001E0B RID: 7691
		public const float TICK_SPACING_MAX = 1f;

		// Token: 0x04001E0C RID: 7692
		public const float TICK_SPACING_MIN = 0.1f;

		// Token: 0x04001E0D RID: 7693
		public AudioSourceController TickSound;

		// Token: 0x04001E0E RID: 7694
		private Coroutine countdownRoutine;
	}
}
