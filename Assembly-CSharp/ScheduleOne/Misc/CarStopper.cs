using System;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BDA RID: 3034
	public class CarStopper : MonoBehaviour
	{
		// Token: 0x0600551A RID: 21786 RVA: 0x0016603C File Offset: 0x0016423C
		protected virtual void Update()
		{
			float num = 70f;
			if (this.isActive)
			{
				this.Obstacle.enabled = true;
				this.blocker.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(this.blocker.localEulerAngles.z + Time.deltaTime * num / this.moveTime, 0f, num));
				return;
			}
			this.Obstacle.enabled = false;
			this.blocker.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(this.blocker.localEulerAngles.z - Time.deltaTime * num / this.moveTime, 0f, num));
		}

		// Token: 0x04003F1B RID: 16155
		public bool isActive;

		// Token: 0x04003F1C RID: 16156
		[Header("References")]
		[SerializeField]
		protected Transform blocker;

		// Token: 0x04003F1D RID: 16157
		public NavMeshObstacle Obstacle;

		// Token: 0x04003F1E RID: 16158
		private float moveTime = 0.5f;
	}
}
