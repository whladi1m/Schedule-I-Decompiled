using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000BF0 RID: 3056
	public class Map : Singleton<Map>
	{
		// Token: 0x06005599 RID: 21913 RVA: 0x00168058 File Offset: 0x00166258
		protected override void Awake()
		{
			base.Awake();
			if (!GameManager.IS_TUTORIAL)
			{
				using (IEnumerator enumerator = Enum.GetValues(typeof(EMapRegion)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EMapRegion region = (EMapRegion)enumerator.Current;
						if (this.Regions == null || Array.Find<MapRegionData>(this.Regions, (MapRegionData x) => x.Region == region) == null)
						{
							Console.LogError(string.Format("No region data found for {0}", region), null);
						}
					}
				}
			}
			if (this.TreeBounds != null)
			{
				this.TreeBounds.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x0016812C File Offset: 0x0016632C
		protected override void Start()
		{
			base.Start();
			LevelManager instance = NetworkSingleton<LevelManager>.Instance;
			instance.onRankUp = (Action<FullRank, FullRank>)Delegate.Combine(instance.onRankUp, new Action<FullRank, FullRank>(this.OnRankUp));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.GameLoaded));
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x00168180 File Offset: 0x00166380
		protected override void OnDestroy()
		{
			if (Singleton<LoadManager>.InstanceExists)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.GameLoaded));
			}
			base.OnDestroy();
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001681AC File Offset: 0x001663AC
		public MapRegionData GetRegionData(EMapRegion region)
		{
			return Array.Find<MapRegionData>(this.Regions, (MapRegionData x) => x.Region == region);
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001681E0 File Offset: 0x001663E0
		private void GameLoaded()
		{
			foreach (MapRegionData mapRegionData in this.Regions)
			{
				if (mapRegionData.IsUnlocked)
				{
					mapRegionData.SetUnlocked();
				}
			}
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00168214 File Offset: 0x00166414
		private void OnRankUp(FullRank oldRank, FullRank newRank)
		{
			foreach (MapRegionData mapRegionData in this.Regions)
			{
				if (oldRank < mapRegionData.RankRequirement && newRank >= mapRegionData.RankRequirement)
				{
					mapRegionData.SetUnlocked();
					if (!Singleton<LoadManager>.Instance.IsLoading)
					{
						Singleton<RegionUnlockedCanvas>.Instance.QueueUnlocked(mapRegionData.Region);
					}
				}
			}
		}

		// Token: 0x04003F99 RID: 16281
		public MapRegionData[] Regions;

		// Token: 0x04003F9A RID: 16282
		[Header("References")]
		public PoliceStation PoliceStation;

		// Token: 0x04003F9B RID: 16283
		public MedicalCentre MedicalCentre;

		// Token: 0x04003F9C RID: 16284
		public Transform TreeBounds;
	}
}
