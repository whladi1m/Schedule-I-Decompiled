using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200082A RID: 2090
	public class BetaEnabledGameObject : MonoBehaviour
	{
		// Token: 0x0600398D RID: 14733 RVA: 0x000F391F File Offset: 0x000F1B1F
		private void Start()
		{
			if (!GameManager.IS_BETA)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
