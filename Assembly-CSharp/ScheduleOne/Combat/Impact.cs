using System;
using FishNet.Object;
using FishNet.Serializing.Helping;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000727 RID: 1831
	[Serializable]
	public class Impact
	{
		// Token: 0x06003189 RID: 12681 RVA: 0x000CDE90 File Offset: 0x000CC090
		public Impact(RaycastHit hit, Vector3 hitPoint, Vector3 impactForceDirection, float impactForce, float impactDamage, EImpactType impactType, Player impactSource, int impactID)
		{
			this.Hit = hit;
			this.HitPoint = hitPoint;
			this.ImpactForceDirection = impactForceDirection;
			this.ImpactForce = impactForce;
			this.ImpactDamage = impactDamage;
			this.ImpactType = impactType;
			if (impactSource != null)
			{
				this.ImpactSource = impactSource.NetworkObject;
			}
			this.ImpactID = impactID;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0000494F File Offset: 0x00002B4F
		public Impact()
		{
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000CDEEF File Offset: 0x000CC0EF
		public static bool IsLethal(EImpactType impactType)
		{
			return impactType == EImpactType.SharpMetal || impactType == EImpactType.Bullet || impactType == EImpactType.Explosion;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000CDF00 File Offset: 0x000CC100
		public bool IsPlayerImpact(out Player player)
		{
			if (this.ImpactSource == null)
			{
				player = null;
				return false;
			}
			player = this.ImpactSource.GetComponent<Player>();
			return player != null;
		}

		// Token: 0x04002367 RID: 9063
		[CodegenExclude]
		public RaycastHit Hit;

		// Token: 0x04002368 RID: 9064
		public Vector3 HitPoint;

		// Token: 0x04002369 RID: 9065
		public Vector3 ImpactForceDirection;

		// Token: 0x0400236A RID: 9066
		public float ImpactForce;

		// Token: 0x0400236B RID: 9067
		public float ImpactDamage;

		// Token: 0x0400236C RID: 9068
		public EImpactType ImpactType;

		// Token: 0x0400236D RID: 9069
		public NetworkObject ImpactSource;

		// Token: 0x0400236E RID: 9070
		public int ImpactID;
	}
}
