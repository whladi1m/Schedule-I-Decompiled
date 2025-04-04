using System;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.FX
{
	// Token: 0x02000612 RID: 1554
	public class FXManager : Singleton<FXManager>
	{
		// Token: 0x060028EB RID: 10475 RVA: 0x000A8DF5 File Offset: 0x000A6FF5
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000A8E00 File Offset: 0x000A7000
		public void CreateImpactFX(Impact impact)
		{
			AudioClip impactSound = this.GetImpactSound(impact);
			if (impactSound != null)
			{
				this.PlayImpact(impactSound, impact.HitPoint, Mathf.Clamp01(impact.ImpactForce / 400f));
			}
			GameObject impactParticles = this.GetImpactParticles(impact);
			if (impactParticles != null)
			{
				this.PlayParticles(impactParticles, impact.HitPoint, Quaternion.LookRotation(impact.HitPoint));
			}
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000A8E68 File Offset: 0x000A7068
		public void CreateBulletTrail(Vector3 start, Vector3 dir, float speed, float range, LayerMask mask)
		{
			FXManager.<>c__DisplayClass7_0 CS$<>8__locals1 = new FXManager.<>c__DisplayClass7_0();
			CS$<>8__locals1.start = start;
			CS$<>8__locals1.trail = UnityEngine.Object.Instantiate<TrailRenderer>(this.BulletTrail, NetworkSingleton<GameManager>.Instance.Temp);
			CS$<>8__locals1.trail.transform.position = CS$<>8__locals1.start;
			CS$<>8__locals1.trail.transform.forward = dir;
			CS$<>8__locals1.maxDistance = range;
			RaycastHit raycastHit;
			if (Physics.Raycast(CS$<>8__locals1.start, dir, out raycastHit, range, mask))
			{
				CS$<>8__locals1.maxDistance = raycastHit.distance;
			}
			Debug.DrawRay(CS$<>8__locals1.start, dir * CS$<>8__locals1.maxDistance, Color.red, 5f);
			base.StartCoroutine(CS$<>8__locals1.<CreateBulletTrail>g__Routine|0());
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000A8F20 File Offset: 0x000A7120
		private void PlayImpact(AudioClip clip, Vector3 position, float volume)
		{
			AudioSourceController source = this.GetSource();
			if (source == null)
			{
				Console.LogWarning("No available audio source controller found", null);
				return;
			}
			source.transform.position = position;
			source.AudioSource.clip = clip;
			source.VolumeMultiplier = volume;
			source.AudioSource.Play();
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000A8F73 File Offset: 0x000A7173
		private void PlayParticles(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation), 2f);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000A8F87 File Offset: 0x000A7187
		private AudioClip GetImpactSound(Impact impact)
		{
			if (!(impact.Hit.collider.GetComponentInParent<NPC>() != null))
			{
				return null;
			}
			if (impact.ImpactType == EImpactType.SharpMetal)
			{
				return FXManager.GetRandomClip(this.SlashImpactClips);
			}
			return FXManager.GetRandomClip(this.PunchImpactsClips);
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000A8FC3 File Offset: 0x000A71C3
		private GameObject GetImpactParticles(Impact impact)
		{
			if (impact.Hit.collider.GetComponentInParent<NPC>() != null)
			{
				return this.PunchParticlePrefab;
			}
			return null;
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000A8FE5 File Offset: 0x000A71E5
		private AudioSourceController GetSource()
		{
			return this.ImpactSources.FirstOrDefault((AudioSourceController x) => !x.isPlaying);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000A9011 File Offset: 0x000A7211
		private static AudioClip GetRandomClip(AudioClip[] clips)
		{
			return clips[UnityEngine.Random.Range(0, clips.Length)];
		}

		// Token: 0x04001E31 RID: 7729
		public AudioClip[] PunchImpactsClips;

		// Token: 0x04001E32 RID: 7730
		public AudioClip[] SlashImpactClips;

		// Token: 0x04001E33 RID: 7731
		[Header("References")]
		public AudioSourceController[] ImpactSources;

		// Token: 0x04001E34 RID: 7732
		[Header("Particle Prefabs")]
		public GameObject PunchParticlePrefab;

		// Token: 0x04001E35 RID: 7733
		[Header("Trails")]
		public TrailRenderer BulletTrail;
	}
}
