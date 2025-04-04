using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x02000775 RID: 1909
	public class BuildStop_Base : MonoBehaviour
	{
		// Token: 0x06003412 RID: 13330 RVA: 0x000D9AAC File Offset: 0x000D7CAC
		public virtual void Stop_Building()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
			base.GetComponent<BuildUpdate_Base>().Stop();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
