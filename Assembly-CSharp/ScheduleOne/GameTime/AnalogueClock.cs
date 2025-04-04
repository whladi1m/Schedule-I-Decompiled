using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002A7 RID: 679
	public class AnalogueClock : MonoBehaviour
	{
		// Token: 0x06000E3B RID: 3643 RVA: 0x0003F739 File Offset: 0x0003D939
		public void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.MinPass();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0003F767 File Offset: 0x0003D967
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0003F798 File Offset: 0x0003D998
		public void MinPass()
		{
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(currentTime);
			float num = (float)(minSumFrom24HourTime % 60);
			float num2 = (float)(minSumFrom24HourTime / 60);
			float num3 = num / 60f * 360f;
			float d = num2 / 12f * 360f + num3 / 12f;
			if (currentTime == 1200 && this.onNoon != null)
			{
				this.onNoon.Invoke();
			}
			if (currentTime == 0 && this.onMidnight != null)
			{
				this.onMidnight.Invoke();
			}
			this.MinHand.localEulerAngles = this.RotationAxis * num3;
			this.HourHand.localEulerAngles = this.RotationAxis * d;
		}

		// Token: 0x04000EE4 RID: 3812
		public Transform MinHand;

		// Token: 0x04000EE5 RID: 3813
		public Transform HourHand;

		// Token: 0x04000EE6 RID: 3814
		public Vector3 RotationAxis = Vector3.forward;

		// Token: 0x04000EE7 RID: 3815
		public UnityEvent onNoon;

		// Token: 0x04000EE8 RID: 3816
		public UnityEvent onMidnight;
	}
}
