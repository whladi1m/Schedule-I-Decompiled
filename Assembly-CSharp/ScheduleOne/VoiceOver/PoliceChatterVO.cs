using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000274 RID: 628
	public class PoliceChatterVO : VOEmitter
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x0003A463 File Offset: 0x00038663
		public override void Play(EVOLineType lineType)
		{
			if (lineType == EVOLineType.PoliceChatter)
			{
				this.PlayChatter();
				return;
			}
			base.Play(lineType);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0003A478 File Offset: 0x00038678
		private void PlayChatter()
		{
			if (this.chatterRoutine != null)
			{
				base.StopCoroutine(this.chatterRoutine);
			}
			this.chatterRoutine = base.StartCoroutine(this.<PlayChatter>g__Play|5_0());
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0003A4A8 File Offset: 0x000386A8
		[CompilerGenerated]
		private IEnumerator <PlayChatter>g__Play|5_0()
		{
			this.StartBeep.Play();
			this.Static.Play();
			yield return new WaitForSeconds(0.25f);
			base.Play(EVOLineType.PoliceChatter);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => !this.audioSourceController.isPlaying);
			this.StartEndBeep.Play();
			this.Static.Stop();
			this.chatterRoutine = null;
			yield break;
		}

		// Token: 0x04000DB4 RID: 3508
		public AudioSourceController StartBeep;

		// Token: 0x04000DB5 RID: 3509
		public AudioSourceController StartEndBeep;

		// Token: 0x04000DB6 RID: 3510
		public AudioSourceController Static;

		// Token: 0x04000DB7 RID: 3511
		private Coroutine chatterRoutine;
	}
}
