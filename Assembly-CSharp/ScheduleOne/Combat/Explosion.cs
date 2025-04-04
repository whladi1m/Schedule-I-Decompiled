using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Noise;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000723 RID: 1827
	public class Explosion : MonoBehaviour
	{
		// Token: 0x06003183 RID: 12675 RVA: 0x000CDC04 File Offset: 0x000CBE04
		public void Initialize(Vector3 origin, ExplosionData data)
		{
			base.transform.position = origin;
			this.Sound.Play();
			float num = Mathf.Max(data.DamageRadius, data.PushForceRadius);
			NoiseUtility.EmitNoise(origin, ENoiseType.Explosion, num * 4f, base.gameObject);
			List<IDamageable> list = new List<IDamageable>();
			if (InstanceFinder.IsServer)
			{
				foreach (Collider collider in Physics.OverlapSphere(origin, num))
				{
					IDamageable componentInParent = collider.GetComponentInParent<IDamageable>();
					if (componentInParent != null && !list.Contains(componentInParent))
					{
						string str = "Explosion hit ";
						IDamageable damageable = componentInParent;
						Console.Log(str + ((damageable != null) ? damageable.ToString() : null) + " at " + collider.transform.position.ToString(), null);
						RaycastHit hit = default(RaycastHit);
						if (Vector3.Distance(origin, collider.transform.position) < 1f)
						{
							hit.point = origin;
						}
						else
						{
							if (!Physics.Raycast(origin, collider.transform.position - origin, out hit, num, NetworkSingleton<CombatManager>.Instance.ExplosionLayerMask))
							{
								Debug.DrawLine(origin, collider.transform.position, Color.green, 5f);
								goto IL_20F;
							}
							Debug.DrawLine(origin, hit.point, Color.red, 5f);
							if (hit.collider != collider && hit.collider.GetComponentInParent<IDamageable>() != componentInParent)
							{
								goto IL_20F;
							}
						}
						list.Add(componentInParent);
						float num2 = Vector3.Distance(origin, collider.transform.position);
						float impactDamage = Mathf.Lerp(data.MaxDamage, 0f, Mathf.Clamp01(num2 / data.DamageRadius));
						float impactForce = Mathf.Lerp(data.MaxPushForce, 0f, Mathf.Clamp01(num2 / data.PushForceRadius));
						Impact impact = new Impact(hit, hit.point, (hit.point - origin).normalized, impactForce, impactDamage, EImpactType.Explosion, null, UnityEngine.Random.Range(0, int.MaxValue));
						componentInParent.ReceiveImpact(impact);
					}
					IL_20F:;
				}
			}
			Console.Log("Explosion hit " + list.Count.ToString() + " damageables.", null);
		}

		// Token: 0x0400235A RID: 9050
		public AudioSourceController Sound;
	}
}
