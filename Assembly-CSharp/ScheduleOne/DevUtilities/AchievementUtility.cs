using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006BE RID: 1726
	public class AchievementUtility : MonoBehaviour
	{
		// Token: 0x06002F28 RID: 12072 RVA: 0x000C4C76 File Offset: 0x000C2E76
		public void UnlockAchievement()
		{
			Singleton<AchievementManager>.Instance.UnlockAchievement(this.Achievement);
		}

		// Token: 0x04002195 RID: 8597
		public AchievementManager.EAchievement Achievement;
	}
}
