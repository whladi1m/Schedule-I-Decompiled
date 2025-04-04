using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000862 RID: 2146
	[ExecuteInEditMode]
	public class WheelRotator : MonoBehaviour
	{
		// Token: 0x06003A57 RID: 14935 RVA: 0x000F5A45 File Offset: 0x000F3C45
		private void Start()
		{
			if (this.Controller != null)
			{
				this.Controller.AudioSource.time = UnityEngine.Random.Range(0f, this.Controller.AudioSource.clip.length);
			}
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x000F5A84 File Offset: 0x000F3C84
		private void LateUpdate()
		{
			Vector3 position = base.transform.position;
			float num = Vector3.Distance(position, this.lastFramePosition);
			if (num > 0f)
			{
				float num2 = num / (6.2831855f * this.Radius) * 360f;
				this.Wheel.Rotate(this.RotationAxis, num2 * (this.Flip ? -1f : 1f));
				float num3 = num2 / Time.deltaTime;
				if (this.Controller != null)
				{
					this.Controller.VolumeMultiplier = num3 / this.AudioVolumeDivisor;
				}
			}
			this.lastFramePosition = position;
		}

		// Token: 0x04002A28 RID: 10792
		public float Radius = 0.5f;

		// Token: 0x04002A29 RID: 10793
		public Transform Wheel;

		// Token: 0x04002A2A RID: 10794
		public bool Flip;

		// Token: 0x04002A2B RID: 10795
		public AudioSourceController Controller;

		// Token: 0x04002A2C RID: 10796
		public float AudioVolumeDivisor = 90f;

		// Token: 0x04002A2D RID: 10797
		public Vector3 RotationAxis = Vector3.up;

		// Token: 0x04002A2E RID: 10798
		[SerializeField]
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
