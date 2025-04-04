using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using Steamworks;

namespace ScheduleOne
{
	// Token: 0x02000270 RID: 624
	public class AchievementManager : PersistentSingleton<AchievementManager>
	{
		// Token: 0x06000D14 RID: 3348 RVA: 0x0003A1C8 File Offset: 0x000383C8
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<AchievementManager>.Instance == null || Singleton<AchievementManager>.Instance != this)
			{
				return;
			}
			this.achievements = (AchievementManager.EAchievement[])Enum.GetValues(typeof(AchievementManager.EAchievement));
			foreach (AchievementManager.EAchievement key in this.achievements)
			{
				this.achievementUnlocked.Add(key, false);
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0003A236 File Offset: 0x00038436
		protected override void Start()
		{
			base.Start();
			if (Singleton<AchievementManager>.Instance == null || Singleton<AchievementManager>.Instance != this)
			{
				return;
			}
			this.PullAchievements();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003A260 File Offset: 0x00038460
		private void PullAchievements()
		{
			if (!SteamManager.Initialized)
			{
				Console.LogWarning("Steamworks not initialized, cannot pull achievement stats", null);
				return;
			}
			foreach (AchievementManager.EAchievement key in this.achievements)
			{
				bool value;
				SteamUserStats.GetAchievement(key.ToString(), out value);
				this.achievementUnlocked[key] = value;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0003A2BC File Offset: 0x000384BC
		public void UnlockAchievement(AchievementManager.EAchievement achievement)
		{
			if (!SteamManager.Initialized)
			{
				Console.LogWarning("Steamworks not initialized, cannot unlock achievement", null);
				return;
			}
			if (this.achievementUnlocked[achievement])
			{
				return;
			}
			Console.Log(string.Format("Unlocking achievement: {0}", achievement), null);
			SteamUserStats.SetAchievement(achievement.ToString());
			SteamUserStats.StoreStats();
			this.achievementUnlocked[achievement] = true;
		}

		// Token: 0x04000D8E RID: 3470
		private AchievementManager.EAchievement[] achievements;

		// Token: 0x04000D8F RID: 3471
		private Dictionary<AchievementManager.EAchievement, bool> achievementUnlocked = new Dictionary<AchievementManager.EAchievement, bool>();

		// Token: 0x02000271 RID: 625
		public enum EAchievement
		{
			// Token: 0x04000D91 RID: 3473
			COMPLETE_PROLOGUE,
			// Token: 0x04000D92 RID: 3474
			RV_DESTROYED,
			// Token: 0x04000D93 RID: 3475
			DEALER_RECRUITED,
			// Token: 0x04000D94 RID: 3476
			MASTER_CHEF,
			// Token: 0x04000D95 RID: 3477
			BUSINESSMAN,
			// Token: 0x04000D96 RID: 3478
			BIGWIG,
			// Token: 0x04000D97 RID: 3479
			MAGNATE,
			// Token: 0x04000D98 RID: 3480
			UPSTANDING_CITIZEN,
			// Token: 0x04000D99 RID: 3481
			ROLLING_IN_STYLE,
			// Token: 0x04000D9A RID: 3482
			LONG_ARM_OF_THE_LAW,
			// Token: 0x04000D9B RID: 3483
			INDIAN_DEALER
		}
	}
}
