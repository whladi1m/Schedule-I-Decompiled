using System;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BEE RID: 3054
	public class Gate : MonoBehaviour
	{
		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600558B RID: 21899 RVA: 0x00167CCF File Offset: 0x00165ECF
		// (set) Token: 0x0600558C RID: 21900 RVA: 0x00167CD7 File Offset: 0x00165ED7
		public bool IsOpen { get; protected set; }

		// Token: 0x0600558D RID: 21901 RVA: 0x00167CE0 File Offset: 0x00165EE0
		private void Update()
		{
			this.Momentum = Mathf.MoveTowards(this.Momentum, 1f, Time.deltaTime * this.Acceleration);
			if (this.IsOpen)
			{
				this.openDelta += Time.deltaTime * this.OpenSpeed * this.Momentum;
			}
			else
			{
				this.openDelta -= Time.deltaTime * this.OpenSpeed * this.Momentum;
			}
			this.openDelta = Mathf.Clamp01(this.openDelta);
			if (this.openDelta <= 0.01f || this.openDelta >= 0.99f)
			{
				if (this.LoopSounds[0].isPlaying)
				{
					AudioSourceController[] array = this.LoopSounds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Stop();
					}
					array = this.StopSounds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Play();
					}
				}
			}
			else if (!this.LoopSounds[0].isPlaying && this.StartSounds[0].AudioSource.time >= this.StartSounds[0].AudioSource.clip.length * 0.5f)
			{
				AudioSourceController[] array = this.LoopSounds;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Play();
				}
			}
			this.Gate1.localPosition = Vector3.Lerp(this.Gate1Closed, this.Gate1Open, this.openDelta);
			this.Gate2.localPosition = Vector3.Lerp(this.Gate2Closed, this.Gate2Open, this.openDelta);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x00167E74 File Offset: 0x00166074
		[Button]
		public void Open()
		{
			this.Momentum *= -1f;
			if (this.openDelta == 0f)
			{
				this.Momentum = 0f;
			}
			AudioSourceController[] startSounds = this.StartSounds;
			for (int i = 0; i < startSounds.Length; i++)
			{
				startSounds[i].Play();
			}
			this.IsOpen = true;
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x00167ED0 File Offset: 0x001660D0
		[Button]
		public void Close()
		{
			this.Momentum *= -1f;
			if (this.openDelta == 1f)
			{
				this.Momentum = 0f;
			}
			AudioSourceController[] startSounds = this.StartSounds;
			for (int i = 0; i < startSounds.Length; i++)
			{
				startSounds[i].Play();
			}
			this.IsOpen = false;
		}

		// Token: 0x04003F85 RID: 16261
		public Transform Gate1;

		// Token: 0x04003F86 RID: 16262
		public Vector3 Gate1Open;

		// Token: 0x04003F87 RID: 16263
		public Vector3 Gate1Closed;

		// Token: 0x04003F88 RID: 16264
		public Transform Gate2;

		// Token: 0x04003F89 RID: 16265
		public Vector3 Gate2Open;

		// Token: 0x04003F8A RID: 16266
		public Vector3 Gate2Closed;

		// Token: 0x04003F8B RID: 16267
		public float OpenSpeed;

		// Token: 0x04003F8C RID: 16268
		public float Acceleration = 2f;

		// Token: 0x04003F8D RID: 16269
		[Header("Sound")]
		public AudioSourceController[] StartSounds;

		// Token: 0x04003F8E RID: 16270
		public AudioSourceController[] LoopSounds;

		// Token: 0x04003F8F RID: 16271
		public AudioSourceController[] StopSounds;

		// Token: 0x04003F90 RID: 16272
		private float Momentum;

		// Token: 0x04003F91 RID: 16273
		private float openDelta;
	}
}
