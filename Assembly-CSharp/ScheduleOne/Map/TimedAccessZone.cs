using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C06 RID: 3078
	public class TimedAccessZone : AccessZone
	{
		// Token: 0x06005609 RID: 22025 RVA: 0x0016943D File Offset: 0x0016763D
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x00169466 File Offset: 0x00167666
		protected virtual void MinPass()
		{
			this.SetIsOpen(this.GetIsOpen());
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x00169474 File Offset: 0x00167674
		protected virtual bool GetIsOpen()
		{
			return NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.OpenTime, this.CloseTime);
		}

		// Token: 0x04003FF6 RID: 16374
		[Header("Timing Settings")]
		public int OpenTime = 600;

		// Token: 0x04003FF7 RID: 16375
		public int CloseTime = 1800;
	}
}
