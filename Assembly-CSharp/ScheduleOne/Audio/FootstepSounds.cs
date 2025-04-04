using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Materials;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200078F RID: 1935
	public class FootstepSounds : MonoBehaviour
	{
		// Token: 0x060034AA RID: 13482 RVA: 0x000DD780 File Offset: 0x000DB980
		private void Start()
		{
			foreach (FootstepSounds.FootstepSoundGroup footstepSoundGroup in this.soundGroups)
			{
				foreach (FootstepSounds.FootstepSoundGroup.MaterialType materialType in footstepSoundGroup.appliesTo)
				{
					if (!this.materialFootstepSounds.ContainsKey(materialType.type))
					{
						this.materialFootstepSounds.Add(materialType.type, footstepSoundGroup);
					}
				}
			}
			foreach (object obj in Enum.GetValues(typeof(EMaterialType)))
			{
				EMaterialType key = (EMaterialType)obj;
				if (!this.materialFootstepSounds.ContainsKey(key))
				{
					Console.Log("No footstep sounds for material type: " + key.ToString() + "\n Assigning to default group.", null);
					this.materialFootstepSounds.Add(key, this.soundGroups[0]);
				}
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				this.sources[i].AudioSource.enabled = false;
				this.sources[i].enabled = false;
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000DD910 File Offset: 0x000DBB10
		private void Update()
		{
			this.lastStepTime += Time.deltaTime;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000DD924 File Offset: 0x000DBB24
		public void Step(EMaterialType materialType, float hardness)
		{
			FootstepSounds.<>c__DisplayClass8_0 CS$<>8__locals1 = new FootstepSounds.<>c__DisplayClass8_0();
			if (this.lastStepTime < 0.15f)
			{
				return;
			}
			this.lastStepTime = 0f;
			CS$<>8__locals1.source = this.GetFreeSource();
			if (CS$<>8__locals1.source == null)
			{
				Console.LogWarning("No free audio sources available for footstep sound.", null);
				return;
			}
			FootstepSounds.FootstepSoundGroup footstepSoundGroup = this.materialFootstepSounds[materialType];
			CS$<>8__locals1.source.AudioSource.clip = footstepSoundGroup.clips[UnityEngine.Random.Range(0, footstepSoundGroup.clips.Count)];
			CS$<>8__locals1.source.AudioSource.pitch = UnityEngine.Random.Range(footstepSoundGroup.PitchMin, footstepSoundGroup.PitchMax);
			CS$<>8__locals1.source.SetVolume(footstepSoundGroup.Volume * hardness);
			CS$<>8__locals1.source.AudioSource.enabled = true;
			CS$<>8__locals1.source.enabled = true;
			CS$<>8__locals1.source.Play();
			base.StartCoroutine(CS$<>8__locals1.<Step>g__DisableSource|0());
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000DDA18 File Offset: 0x000DBC18
		public AudioSourceController GetFreeSource()
		{
			return this.sources.FirstOrDefault((AudioSourceController source) => !source.enabled);
		}

		// Token: 0x040025F0 RID: 9712
		public const float COOLDOWN_TIME = 0.15f;

		// Token: 0x040025F1 RID: 9713
		public List<AudioSourceController> sources = new List<AudioSourceController>();

		// Token: 0x040025F2 RID: 9714
		public List<FootstepSounds.FootstepSoundGroup> soundGroups = new List<FootstepSounds.FootstepSoundGroup>();

		// Token: 0x040025F3 RID: 9715
		private Dictionary<EMaterialType, FootstepSounds.FootstepSoundGroup> materialFootstepSounds = new Dictionary<EMaterialType, FootstepSounds.FootstepSoundGroup>();

		// Token: 0x040025F4 RID: 9716
		private float lastStepTime;

		// Token: 0x02000790 RID: 1936
		[Serializable]
		public class FootstepSoundGroup
		{
			// Token: 0x040025F5 RID: 9717
			public string name;

			// Token: 0x040025F6 RID: 9718
			public List<AudioClip> clips = new List<AudioClip>();

			// Token: 0x040025F7 RID: 9719
			public List<FootstepSounds.FootstepSoundGroup.MaterialType> appliesTo = new List<FootstepSounds.FootstepSoundGroup.MaterialType>();

			// Token: 0x040025F8 RID: 9720
			public float PitchMin = 0.9f;

			// Token: 0x040025F9 RID: 9721
			public float PitchMax = 1.1f;

			// Token: 0x040025FA RID: 9722
			public float Volume = 0.5f;

			// Token: 0x02000791 RID: 1937
			[Serializable]
			public class MaterialType
			{
				// Token: 0x040025FB RID: 9723
				public EMaterialType type;
			}
		}
	}
}
