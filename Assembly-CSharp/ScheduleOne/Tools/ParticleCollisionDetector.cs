using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084F RID: 2127
	public class ParticleCollisionDetector : MonoBehaviour
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x000F4A1D File Offset: 0x000F2C1D
		private void Awake()
		{
			this.ps = base.GetComponent<ParticleSystem>();
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x000F4A2B File Offset: 0x000F2C2B
		public void OnParticleCollision(GameObject other)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(other);
			}
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x000F4A44 File Offset: 0x000F2C44
		private void OnParticleTrigger()
		{
			Component collider = this.ps.trigger.GetCollider(0);
			if (collider != null && this.onCollision != null)
			{
				this.onCollision.Invoke(collider.gameObject);
			}
		}

		// Token: 0x040029EE RID: 10734
		public UnityEvent<GameObject> onCollision = new UnityEvent<GameObject>();

		// Token: 0x040029EF RID: 10735
		private ParticleSystem ps;
	}
}
