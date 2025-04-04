using System;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x0200067A RID: 1658
	public class PivotDoor : MonoBehaviour
	{
		// Token: 0x06002E03 RID: 11779 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000C124D File Offset: 0x000BF44D
		private void LateUpdate()
		{
			this.DoorTransform.localRotation = Quaternion.Lerp(this.DoorTransform.localRotation, Quaternion.Euler(0f, this.targetDoorAngle, 0f), Time.deltaTime * this.SwingSpeed);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000C128C File Offset: 0x000BF48C
		public virtual void Opened(EDoorSide openSide)
		{
			if (openSide == EDoorSide.Interior)
			{
				this.targetDoorAngle = (this.FlipSide ? this.OpenInwardsAngle : this.OpenOutwardsAngle);
				return;
			}
			if (openSide != EDoorSide.Exterior)
			{
				return;
			}
			this.targetDoorAngle = (this.FlipSide ? this.OpenOutwardsAngle : this.OpenInwardsAngle);
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000C12DA File Offset: 0x000BF4DA
		public virtual void Closed()
		{
			this.targetDoorAngle = 0f;
		}

		// Token: 0x040020C6 RID: 8390
		[Header("Settings")]
		public Transform DoorTransform;

		// Token: 0x040020C7 RID: 8391
		public bool FlipSide;

		// Token: 0x040020C8 RID: 8392
		public float OpenInwardsAngle = -100f;

		// Token: 0x040020C9 RID: 8393
		public float OpenOutwardsAngle = 100f;

		// Token: 0x040020CA RID: 8394
		public float SwingSpeed = 5f;

		// Token: 0x040020CB RID: 8395
		private float targetDoorAngle;
	}
}
