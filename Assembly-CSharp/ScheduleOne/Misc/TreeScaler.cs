using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BDF RID: 3039
	public class TreeScaler : MonoBehaviour
	{
		// Token: 0x06005544 RID: 21828 RVA: 0x00166D73 File Offset: 0x00164F73
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateScale));
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00166D9C File Offset: 0x00164F9C
		private void UpdateScale()
		{
			float num = Mathf.Clamp(Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position), this.minScaleDistance, this.maxScaleDistance) / (this.maxScaleDistance - this.minScaleDistance);
			float num2 = this.minScale + (this.maxScale - this.minScale) * num;
			foreach (Transform transform in this.branchMeshes)
			{
				transform.localScale = new Vector3(num2, 1f, num2);
			}
		}

		// Token: 0x04003F42 RID: 16194
		[Header("References")]
		[SerializeField]
		protected List<Transform> branchMeshes = new List<Transform>();

		// Token: 0x04003F43 RID: 16195
		public float minScale = 1f;

		// Token: 0x04003F44 RID: 16196
		public float maxScale = 1.3f;

		// Token: 0x04003F45 RID: 16197
		public float minScaleDistance = 20f;

		// Token: 0x04003F46 RID: 16198
		public float maxScaleDistance = 100f;
	}
}
