using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x02000795 RID: 1941
	public class GameVolumeSetter : MonoBehaviour
	{
		// Token: 0x060034BC RID: 13500 RVA: 0x000DDB65 File Offset: 0x000DBD65
		private void Update()
		{
			Singleton<AudioManager>.Instance.SetGameVolumeMultipler(this.VolumeMultiplier);
		}

		// Token: 0x04002602 RID: 9730
		[Range(0f, 1f)]
		public float VolumeMultiplier = 1f;
	}
}
