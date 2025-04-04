using System;

namespace ScheduleOne.Combat
{
	// Token: 0x02000724 RID: 1828
	public struct ExplosionData
	{
		// Token: 0x06003185 RID: 12677 RVA: 0x000CDE50 File Offset: 0x000CC050
		public ExplosionData(float damageRadius, float maxDamage, float maxPushForce)
		{
			this.DamageRadius = damageRadius;
			this.MaxDamage = maxDamage;
			this.PushForceRadius = damageRadius * 2f;
			this.MaxPushForce = maxPushForce;
		}

		// Token: 0x0400235B RID: 9051
		public float DamageRadius;

		// Token: 0x0400235C RID: 9052
		public float MaxDamage;

		// Token: 0x0400235D RID: 9053
		public float PushForceRadius;

		// Token: 0x0400235E RID: 9054
		public float MaxPushForce;

		// Token: 0x0400235F RID: 9055
		public static readonly ExplosionData DefaultSmall = new ExplosionData(6f, 200f, 500f);
	}
}
