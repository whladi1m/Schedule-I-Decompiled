using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Growing
{
	// Token: 0x02000867 RID: 2151
	public class Plant : MonoBehaviour
	{
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x000F5CBA File Offset: 0x000F3EBA
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x000F5CC2 File Offset: 0x000F3EC2
		public Pot Pot { get; protected set; }

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x000F5CCB File Offset: 0x000F3ECB
		// (set) Token: 0x06003A67 RID: 14951 RVA: 0x000F5CD3 File Offset: 0x000F3ED3
		public float NormalizedGrowthProgress { get; protected set; }

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x000F5CDC File Offset: 0x000F3EDC
		public bool IsFullyGrown
		{
			get
			{
				return this.NormalizedGrowthProgress >= 1f;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003A69 RID: 14953 RVA: 0x000F5CEE File Offset: 0x000F3EEE
		public PlantGrowthStage FinalGrowthStage
		{
			get
			{
				return this.GrowthStages[this.GrowthStages.Length - 1];
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000F5D04 File Offset: 0x000F3F04
		public virtual void Initialize(NetworkObject pot, float growthProgress = 0f, float yieldLevel = 0f, float qualityLevel = 0f)
		{
			this.Pot = pot.GetComponent<Pot>();
			if (this.Pot == null)
			{
				Console.LogWarning("Plant.Initialize: pot is null", null);
				return;
			}
			if (yieldLevel > 0f)
			{
				this.YieldLevel = yieldLevel;
			}
			else
			{
				this.YieldLevel = this.BaseYieldLevel;
			}
			if (qualityLevel > 0f)
			{
				this.QualityLevel = qualityLevel;
			}
			else
			{
				this.QualityLevel = this.BaseQualityLevel;
			}
			for (int i = 0; i < this.FinalGrowthStage.GrowthSites.Length; i++)
			{
				this.SetHarvestableActive(i, false);
			}
			this.SetNormalizedGrowthProgress(growthProgress);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x000F5D9C File Offset: 0x000F3F9C
		public virtual void Destroy(bool dropScraps = false)
		{
			this.DestroySound.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp.transform);
			this.DestroySound.PlayOneShot(false);
			UnityEngine.Object.Destroy(this.DestroySound, 1f);
			if (dropScraps && this.PlantScrapPrefab != null)
			{
				int num = UnityEngine.Random.Range(1, 2);
				for (int i = 0; i < num; i++)
				{
					Vector3 a = this.Pot.LeafDropPoint.forward;
					a += new Vector3(0f, UnityEngine.Random.Range(-0.2f, 0.2f), 0f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.PlantScrapPrefab.ID, this.Pot.LeafDropPoint.position + a * 0.2f, UnityEngine.Random.rotation, a * 0.5f, "", false);
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x000F5EA4 File Offset: 0x000F40A4
		public virtual void MinPass()
		{
			if (this.NormalizedGrowthProgress >= 1f)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsEndOfDay)
			{
				return;
			}
			float num = 1f / ((float)this.GrowthTime * 60f);
			num *= this.Pot.GetAdditiveGrowthMultiplier();
			float num2;
			num *= this.Pot.GetAverageLightExposure(out num2);
			num *= this.Pot.GrowSpeedMultiplier;
			num *= num2;
			if (GameManager.IS_TUTORIAL)
			{
				num *= 0.3f;
			}
			if (this.Pot.NormalizedWaterLevel <= 0f || this.Pot.NormalizedWaterLevel > 1f)
			{
				num *= 0f;
			}
			this.SetNormalizedGrowthProgress(this.NormalizedGrowthProgress + num);
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x000F5F5C File Offset: 0x000F415C
		public virtual void SetNormalizedGrowthProgress(float progress)
		{
			progress = Mathf.Clamp(progress, 0f, 1f);
			float normalizedGrowthProgress = this.NormalizedGrowthProgress;
			this.NormalizedGrowthProgress = progress;
			this.UpdateVisuals();
			if (this.NormalizedGrowthProgress >= 1f && normalizedGrowthProgress < 1f)
			{
				this.GrowthDone();
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000F5FAC File Offset: 0x000F41AC
		protected virtual void UpdateVisuals()
		{
			int num = Mathf.FloorToInt(this.NormalizedGrowthProgress * (float)this.GrowthStages.Length);
			for (int i = 0; i < this.GrowthStages.Length; i++)
			{
				this.GrowthStages[i].gameObject.SetActive(i + 1 == num);
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000F5FFA File Offset: 0x000F41FA
		public virtual void SetHarvestableActive(int index, bool active)
		{
			this.FinalGrowthStage.GrowthSites[index].gameObject.SetActive(active);
			this.ActiveHarvestables.Remove(index);
			if (active)
			{
				this.ActiveHarvestables.Add(index);
			}
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000F6030 File Offset: 0x000F4230
		public bool IsHarvestableActive(int index)
		{
			return this.ActiveHarvestables.Contains(index);
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000F6040 File Offset: 0x000F4240
		private void GrowthDone()
		{
			if (InstanceFinder.IsServer)
			{
				if (!this.Pot.IsSpawned)
				{
					Console.LogError("Pot not spawned!", null);
					return;
				}
				int num = Mathf.RoundToInt((float)this.FinalGrowthStage.GrowthSites.Length * this.YieldLevel * this.Pot.YieldMultiplier);
				num = Mathf.Clamp(num, 1, this.FinalGrowthStage.GrowthSites.Length);
				foreach (int harvestableIndex in this.GenerateUniqueIntegers(0, this.FinalGrowthStage.GrowthSites.Length - 1, num))
				{
					this.Pot.SendHarvestableActive(harvestableIndex, true);
				}
			}
			if (this.FullyGrownParticles != null)
			{
				this.FullyGrownParticles.Play();
			}
			if (this.onGrowthDone != null)
			{
				this.onGrowthDone.Invoke();
			}
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000F6138 File Offset: 0x000F4338
		private List<int> GenerateUniqueIntegers(int min, int max, int count)
		{
			List<int> list = new List<int>();
			if (max - min + 1 < count)
			{
				Debug.LogWarning("Range is too small to generate the requested number of unique integers.");
				return null;
			}
			List<int> list2 = new List<int>();
			for (int i = min; i <= max; i++)
			{
				list2.Add(i);
			}
			for (int j = 0; j < count; j++)
			{
				int index = UnityEngine.Random.Range(0, list2.Count);
				list.Add(list2[index]);
				list2.RemoveAt(index);
			}
			return list;
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x000F61A9 File Offset: 0x000F43A9
		public void SetVisible(bool vis)
		{
			this.VisualsContainer.gameObject.SetActive(vis);
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000F61BC File Offset: 0x000F43BC
		public virtual ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			Console.LogError("Plant.GetHarvestedProduct: This method should be overridden by a subclass.", null);
			return null;
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000F61CA File Offset: 0x000F43CA
		public PlantData GetPlantData()
		{
			return new PlantData(this.SeedDefinition.ID, this.NormalizedGrowthProgress, this.YieldLevel, this.QualityLevel, this.ActiveHarvestables.ToArray());
		}

		// Token: 0x04002A42 RID: 10818
		[Header("References")]
		public Transform VisualsContainer;

		// Token: 0x04002A43 RID: 10819
		public PlantGrowthStage[] GrowthStages;

		// Token: 0x04002A44 RID: 10820
		public Collider Collider;

		// Token: 0x04002A45 RID: 10821
		public AudioSourceController SnipSound;

		// Token: 0x04002A46 RID: 10822
		public AudioSourceController DestroySound;

		// Token: 0x04002A47 RID: 10823
		public ParticleSystem FullyGrownParticles;

		// Token: 0x04002A48 RID: 10824
		[Header("Settings")]
		public SeedDefinition SeedDefinition;

		// Token: 0x04002A49 RID: 10825
		public int GrowthTime = 48;

		// Token: 0x04002A4A RID: 10826
		public float BaseYieldLevel = 0.6f;

		// Token: 0x04002A4B RID: 10827
		public float BaseQualityLevel = 0.4f;

		// Token: 0x04002A4C RID: 10828
		public string HarvestTarget = "buds";

		// Token: 0x04002A4D RID: 10829
		[Header("Trash")]
		public TrashItem PlantScrapPrefab;

		// Token: 0x04002A4E RID: 10830
		public UnityEvent onGrowthDone;

		// Token: 0x04002A4F RID: 10831
		[Header("Plant data")]
		public float YieldLevel;

		// Token: 0x04002A50 RID: 10832
		public float QualityLevel;

		// Token: 0x04002A51 RID: 10833
		[HideInInspector]
		public List<int> ActiveHarvestables = new List<int>();
	}
}
