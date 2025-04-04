using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200085F RID: 2143
	public class SmoothRotate : MonoBehaviour
	{
		// Token: 0x06003A4F RID: 14927 RVA: 0x000F58F0 File Offset: 0x000F3AF0
		private void Update()
		{
			if (this.Active)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.Speed, this.Aceleration * Time.deltaTime);
			}
			else
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, this.Aceleration * Time.deltaTime);
			}
			base.transform.Rotate(this.Axis, this.currentSpeed * Time.deltaTime, Space.Self);
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x000F596A File Offset: 0x000F3B6A
		public void SetActive(bool active)
		{
			this.Active = active;
		}

		// Token: 0x04002A1F RID: 10783
		public bool Active = true;

		// Token: 0x04002A20 RID: 10784
		public float Speed = 5f;

		// Token: 0x04002A21 RID: 10785
		public float Aceleration = 2f;

		// Token: 0x04002A22 RID: 10786
		public Vector3 Axis = Vector3.up;

		// Token: 0x04002A23 RID: 10787
		private float currentSpeed;
	}
}
