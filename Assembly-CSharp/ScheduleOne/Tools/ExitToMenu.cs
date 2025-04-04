using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200083C RID: 2108
	public class ExitToMenu : MonoBehaviour
	{
		// Token: 0x060039CA RID: 14794 RVA: 0x000F4148 File Offset: 0x000F2348
		public void Exit()
		{
			Singleton<LoadManager>.Instance.ExitToMenu(null, null, false);
		}
	}
}
