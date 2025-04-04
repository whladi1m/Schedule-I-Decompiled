using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x0200029C RID: 668
	public class RunnerGameCharacter : MonoBehaviour
	{
		// Token: 0x06000DE9 RID: 3561 RVA: 0x0003E400 File Offset: 0x0003C600
		public void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "2DObstacle")
			{
				this.Game.PlayerCollided();
				if (this.onHit != null)
				{
					this.onHit.Invoke();
				}
			}
		}

		// Token: 0x04000E94 RID: 3732
		public RunnerGame Game;

		// Token: 0x04000E95 RID: 3733
		public UnityEvent onHit;
	}
}
