using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x02000299 RID: 665
	public class PongBall : MonoBehaviour
	{
		// Token: 0x06000DD3 RID: 3539 RVA: 0x000045B1 File Offset: 0x000027B1
		private void FixedUpdate()
		{
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003DAD0 File Offset: 0x0003BCD0
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.gameObject.name == "LeftGoal")
			{
				this.Game.GoalHit(Pong.ESide.Left);
			}
			else if (collision.collider.gameObject.name == "RightGoal")
			{
				this.Game.GoalHit(Pong.ESide.Right);
			}
			if (this.RB.velocity.y < 0.1f && collision.collider.GetComponent<PongPaddle>() != null)
			{
				float magnitude = this.RB.velocity.magnitude;
				this.RB.AddForce(new Vector3(0f, UnityEngine.Random.Range(-this.RandomForce, this.RandomForce), 0f), ForceMode.VelocityChange);
				this.RB.velocity = this.RB.velocity.normalized * magnitude;
			}
			if (this.onHit != null)
			{
				this.onHit.Invoke();
			}
		}

		// Token: 0x04000E6B RID: 3691
		public Pong Game;

		// Token: 0x04000E6C RID: 3692
		public RectTransform Rect;

		// Token: 0x04000E6D RID: 3693
		public Rigidbody RB;

		// Token: 0x04000E6E RID: 3694
		public float RandomForce = 0.5f;

		// Token: 0x04000E6F RID: 3695
		public UnityEvent onHit;
	}
}
