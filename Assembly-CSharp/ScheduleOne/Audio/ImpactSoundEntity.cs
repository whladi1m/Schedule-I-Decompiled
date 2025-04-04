using System;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000797 RID: 1943
	[RequireComponent(typeof(Rigidbody))]
	public class ImpactSoundEntity : MonoBehaviour
	{
		// Token: 0x060034C1 RID: 13505 RVA: 0x000DDC4C File Offset: 0x000DBE4C
		public void Awake()
		{
			PhysicsDamageable component = base.GetComponent<PhysicsDamageable>();
			if (component != null)
			{
				PhysicsDamageable physicsDamageable = component;
				physicsDamageable.onImpacted = (Action<Impact>)Delegate.Combine(physicsDamageable.onImpacted, new Action<Impact>(this.OnImpacted));
			}
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000DDC98 File Offset: 0x000DBE98
		private void OnImpacted(Impact impact)
		{
			if (Vector3.SqrMagnitude(impact.Hit.point - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 1600f)
			{
				return;
			}
			if (Time.time - this.lastImpactTime < 0.25f)
			{
				return;
			}
			float impactForce = impact.ImpactForce;
			if (impactForce < 4f)
			{
				return;
			}
			Singleton<SFXManager>.Instance.PlayImpactSound(this.Material, impact.Hit.point, impactForce);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000DDD14 File Offset: 0x000DBF14
		private void OnCollisionEnter(Collision collision)
		{
			if (Time.time - this.lastImpactTime < 0.25f)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.SqrMagnitude(collision.contacts[0].point - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 1600f)
			{
				return;
			}
			Rigidbody rigidbody = collision.rigidbody;
			float num = collision.relativeVelocity.magnitude;
			float num2 = this.rb.mass;
			if (rigidbody != null)
			{
				num2 = Mathf.Min(num2, rigidbody.mass);
			}
			num *= num2;
			if (num < 4f)
			{
				return;
			}
			this.lastImpactTime = Time.time;
			Singleton<SFXManager>.Instance.PlayImpactSound(this.Material, collision.contacts[0].point, num);
		}

		// Token: 0x04002606 RID: 9734
		public const float MIN_IMPACT_MOMENTUM = 4f;

		// Token: 0x04002607 RID: 9735
		public const float COOLDOWN = 0.25f;

		// Token: 0x04002608 RID: 9736
		public ImpactSoundEntity.EMaterial Material;

		// Token: 0x04002609 RID: 9737
		private float lastImpactTime;

		// Token: 0x0400260A RID: 9738
		private Rigidbody rb;

		// Token: 0x02000798 RID: 1944
		public enum EMaterial
		{
			// Token: 0x0400260C RID: 9740
			Wood,
			// Token: 0x0400260D RID: 9741
			HollowMetal,
			// Token: 0x0400260E RID: 9742
			Cardboard,
			// Token: 0x0400260F RID: 9743
			Glass,
			// Token: 0x04002610 RID: 9744
			Plastic,
			// Token: 0x04002611 RID: 9745
			Basketball,
			// Token: 0x04002612 RID: 9746
			SmallHollowMetal,
			// Token: 0x04002613 RID: 9747
			PlasticBag,
			// Token: 0x04002614 RID: 9748
			Punch,
			// Token: 0x04002615 RID: 9749
			BaseballBat
		}
	}
}
