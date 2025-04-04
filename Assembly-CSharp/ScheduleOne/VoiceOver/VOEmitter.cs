using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000278 RID: 632
	[RequireComponent(typeof(AudioSourceController))]
	public class VOEmitter : MonoBehaviour
	{
		// Token: 0x06000D2C RID: 3372 RVA: 0x0003A6E8 File Offset: 0x000388E8
		protected virtual void Awake()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0003A6F8 File Offset: 0x000388F8
		public virtual void Play(EVOLineType lineType)
		{
			if (!this.audioSourceController.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.Database == null)
			{
				Console.LogError("Database is not set on VOEmitter.", null);
				return;
			}
			AudioClip randomClip = this.Database.GetRandomClip(lineType);
			if (randomClip == null)
			{
				Console.LogError("No clip found for line type: " + lineType.ToString(), null);
				return;
			}
			this.audioSourceController.AudioSource.clip = randomClip;
			this.audioSourceController.VolumeMultiplier = this.Database.VolumeMultiplier * this.Database.GetEntry(lineType).VolumeMultiplier;
			this.audioSourceController.PitchMultiplier = (this.PitchMultiplier + UnityEngine.Random.Range(-0.05f, 0.05f)) * this.runtimePitchMultiplier;
			this.audioSourceController.Play();
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003A7D2 File Offset: 0x000389D2
		public void SetRuntimePitchMultiplier(float pitchMultiplier)
		{
			this.runtimePitchMultiplier = pitchMultiplier;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003A7DB File Offset: 0x000389DB
		public void SetDatabase(VODatabase database, bool writeDefault = true)
		{
			this.Database = database;
			if (writeDefault)
			{
				this.defaultVODatabase = database;
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0003A7EE File Offset: 0x000389EE
		public void ResetDatabase()
		{
			this.SetDatabase(this.defaultVODatabase, false);
		}

		// Token: 0x04000DC1 RID: 3521
		public const float PitchVariation = 0.05f;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private VODatabase Database;

		// Token: 0x04000DC3 RID: 3523
		[Range(0.5f, 2f)]
		public float PitchMultiplier = 1f;

		// Token: 0x04000DC4 RID: 3524
		private float runtimePitchMultiplier = 1f;

		// Token: 0x04000DC5 RID: 3525
		protected AudioSourceController audioSourceController;

		// Token: 0x04000DC6 RID: 3526
		private VODatabase defaultVODatabase;
	}
}
