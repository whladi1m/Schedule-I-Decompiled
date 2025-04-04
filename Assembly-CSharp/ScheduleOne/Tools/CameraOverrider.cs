using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200082B RID: 2091
	public class CameraOverrider : MonoBehaviour
	{
		// Token: 0x0600398F RID: 14735 RVA: 0x000F3934 File Offset: 0x000F1B34
		public void LateUpdate()
		{
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(base.transform.position, base.transform.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.FOV, 0f);
		}

		// Token: 0x04002995 RID: 10645
		public float FOV = 70f;
	}
}
