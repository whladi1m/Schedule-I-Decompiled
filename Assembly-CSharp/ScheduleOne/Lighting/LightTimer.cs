using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Misc;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x0200059A RID: 1434
	[RequireComponent(typeof(ToggleableLight))]
	public class LightTimer : MonoBehaviour
	{
		// Token: 0x060023B7 RID: 9143 RVA: 0x00091151 File Offset: 0x0008F351
		protected virtual void Awake()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateState));
			this.toggleableLight = base.GetComponent<ToggleableLight>();
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x00091186 File Offset: 0x0008F386
		private void Start()
		{
			this.UpdateState();
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0009118E File Offset: 0x0008F38E
		protected virtual void UpdateState()
		{
			this.SetState(NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime + this.StartTimeOffset, this.EndTime));
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000911B3 File Offset: 0x0008F3B3
		private void SetState(bool on)
		{
			this.toggleableLight.isOn = on;
		}

		// Token: 0x04001AA4 RID: 6820
		[Header("Timing")]
		public int StartTime = 600;

		// Token: 0x04001AA5 RID: 6821
		public int EndTime = 1800;

		// Token: 0x04001AA6 RID: 6822
		public int StartTimeOffset;

		// Token: 0x04001AA7 RID: 6823
		private ToggleableLight toggleableLight;
	}
}
