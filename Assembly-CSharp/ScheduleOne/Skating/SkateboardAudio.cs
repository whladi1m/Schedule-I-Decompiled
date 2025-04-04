using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Skating
{
	// Token: 0x020002CE RID: 718
	public class SkateboardAudio : MonoBehaviour
	{
		// Token: 0x06000F7B RID: 3963 RVA: 0x00044BD3 File Offset: 0x00042DD3
		private void Awake()
		{
			this.Board.OnJump.AddListener(new UnityAction<float>(this.PlayJump));
			this.Board.OnLand.AddListener(new UnityAction(this.PlayLand));
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00044C10 File Offset: 0x00042E10
		private void Start()
		{
			if (this.Board.IsGrounded())
			{
				this.PlayLand();
			}
			this.RollingAudio.VolumeMultiplier = 0f;
			this.RollingAudio.Play();
			this.WindAudio.VolumeMultiplier = 0f;
			this.WindAudio.Play();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00044C68 File Offset: 0x00042E68
		private void Update()
		{
			float num = Mathf.Clamp(Mathf.Abs(this.Board.CurrentSpeed_Kmh) / this.Board.TopSpeed_Kmh, 0f, 1.5f);
			float volumeMultiplier = num;
			if (this.Board.AirTime > 0.2f)
			{
				volumeMultiplier = 0f;
			}
			this.RollingAudio.VolumeMultiplier = volumeMultiplier;
			this.RollingAudio.AudioSource.pitch = Mathf.Lerp(0.75f, 1f, num);
			if (this.Board.IsOwner)
			{
				this.WindAudio.VolumeMultiplier = num;
				this.WindAudio.AudioSource.pitch = Mathf.Lerp(1.2f, 1.5f, num);
				return;
			}
			this.WindAudio.VolumeMultiplier = 0f;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00044D31 File Offset: 0x00042F31
		public void PlayJump(float force)
		{
			this.JumpAudio.VolumeMultiplier = Mathf.Lerp(0.5f, 1f, force);
			this.JumpAudio.Play();
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00044D59 File Offset: 0x00042F59
		public void PlayLand()
		{
			this.LandAudio.Play();
		}

		// Token: 0x0400101C RID: 4124
		public Skateboard Board;

		// Token: 0x0400101D RID: 4125
		[Header("References")]
		public AudioSourceController JumpAudio;

		// Token: 0x0400101E RID: 4126
		public AudioSourceController LandAudio;

		// Token: 0x0400101F RID: 4127
		public AudioSourceController RollingAudio;

		// Token: 0x04001020 RID: 4128
		public AudioSourceController WindAudio;
	}
}
