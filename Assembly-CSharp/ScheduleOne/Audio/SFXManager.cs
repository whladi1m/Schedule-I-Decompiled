using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200079F RID: 1951
	public class SFXManager : Singleton<SFXManager>
	{
		// Token: 0x060034E5 RID: 13541 RVA: 0x000DE3EC File Offset: 0x000DC5EC
		public void PlayImpactSound(ImpactSoundEntity.EMaterial material, Vector3 position, float momentum)
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.Distance(position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 40f)
			{
				Console.LogWarning("Impact sound too far away", null);
				return;
			}
			SFXManager.ImpactType impactType = this.ImpactTypes.Find((SFXManager.ImpactType x) => x.Material == material);
			if (impactType == null)
			{
				Console.LogWarning("No impact type found for material: " + material.ToString(), null);
				return;
			}
			AudioSourceController source = this.GetSource();
			if (source == null)
			{
				Console.LogWarning("No source available", null);
				return;
			}
			source.transform.position = position;
			float num = Mathf.Clamp01(momentum / 100f);
			source.PitchMultiplier = Mathf.Lerp(impactType.MaxPitch, impactType.MinPitch, num);
			source.VolumeMultiplier = Mathf.Lerp(impactType.MinVolume, impactType.MaxVolume, Mathf.Sqrt(num));
			source.AudioSource.clip = impactType.Clips[UnityEngine.Random.Range(0, impactType.Clips.Length)];
			source.Play();
			this.soundsInUse.Add(source);
			this.soundPool.Remove(source);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000DE51C File Offset: 0x000DC71C
		private void FixedUpdate()
		{
			for (int i = this.soundsInUse.Count - 1; i >= 0; i--)
			{
				if (!this.soundsInUse[i].isPlaying)
				{
					this.soundPool.Add(this.soundsInUse[i]);
					this.soundsInUse.RemoveAt(i);
				}
			}
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000DE577 File Offset: 0x000DC777
		private AudioSourceController GetSource()
		{
			if (this.soundPool.Count == 0)
			{
				Console.Log("No more sources available", null);
				return null;
			}
			return this.soundPool[0];
		}

		// Token: 0x04002629 RID: 9769
		public const float MAX_PLAYER_DISTANCE = 40f;

		// Token: 0x0400262A RID: 9770
		public const float SQR_MAX_PLAYER_DISTANCE = 1600f;

		// Token: 0x0400262B RID: 9771
		public List<SFXManager.ImpactType> ImpactTypes = new List<SFXManager.ImpactType>();

		// Token: 0x0400262C RID: 9772
		[SerializeField]
		private List<AudioSourceController> soundPool = new List<AudioSourceController>();

		// Token: 0x0400262D RID: 9773
		private List<AudioSourceController> soundsInUse = new List<AudioSourceController>();

		// Token: 0x020007A0 RID: 1952
		[Serializable]
		public class ImpactType
		{
			// Token: 0x0400262E RID: 9774
			public ImpactSoundEntity.EMaterial Material;

			// Token: 0x0400262F RID: 9775
			public float MinVolume;

			// Token: 0x04002630 RID: 9776
			public float MaxVolume;

			// Token: 0x04002631 RID: 9777
			public float MinPitch;

			// Token: 0x04002632 RID: 9778
			public float MaxPitch;

			// Token: 0x04002633 RID: 9779
			public AudioClip[] Clips;
		}
	}
}
