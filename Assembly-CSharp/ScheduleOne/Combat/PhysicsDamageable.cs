using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000728 RID: 1832
	public class PhysicsDamageable : MonoBehaviour, IDamageable
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x000CDF2A File Offset: 0x000CC12A
		// (set) Token: 0x0600318E RID: 12686 RVA: 0x000CDF32 File Offset: 0x000CC132
		public Vector3 averageVelocity { get; private set; } = Vector3.zero;

		// Token: 0x0600318F RID: 12687 RVA: 0x000CDF3B File Offset: 0x000CC13B
		public void OnValidate()
		{
			if (this.Rb == null)
			{
				this.Rb = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000CDF57 File Offset: 0x000CC157
		public virtual void SendImpact(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000CDF60 File Offset: 0x000CC160
		public virtual void ReceiveImpact(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			if (this.onImpacted != null)
			{
				this.onImpacted(impact);
			}
			if (this.Rb != null)
			{
				this.Rb.AddForceAtPosition(-impact.Hit.normal * impact.ImpactForce * this.ForceMultiplier, impact.Hit.point, ForceMode.Impulse);
			}
		}

		// Token: 0x0400236F RID: 9071
		public const int VELOCITY_HISTORY_LENGTH = 4;

		// Token: 0x04002370 RID: 9072
		public Rigidbody Rb;

		// Token: 0x04002371 RID: 9073
		public float ForceMultiplier = 1f;

		// Token: 0x04002372 RID: 9074
		private List<int> impactHistory = new List<int>();

		// Token: 0x04002373 RID: 9075
		public Action<Impact> onImpacted;

		// Token: 0x04002375 RID: 9077
		private List<Vector3> velocityHistory = new List<Vector3>();
	}
}
