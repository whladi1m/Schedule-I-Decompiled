using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009C2 RID: 2498
	public class EyeLidOverlaySetter : MonoBehaviour
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x0011B62D File Offset: 0x0011982D
		private void OnEnable()
		{
			Singleton<EyelidOverlay>.Instance.AutoUpdate = false;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0011B63A File Offset: 0x0011983A
		private void OnDisable()
		{
			Singleton<EyelidOverlay>.Instance.AutoUpdate = true;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0011B647 File Offset: 0x00119847
		private void Update()
		{
			Singleton<EyelidOverlay>.Instance.SetOpen(this.OpenOverride);
		}

		// Token: 0x04003167 RID: 12647
		[Range(0f, 1f)]
		public float OpenOverride = 1f;
	}
}
