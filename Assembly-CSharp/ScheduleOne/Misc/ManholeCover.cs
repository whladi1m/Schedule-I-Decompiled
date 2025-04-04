using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BD9 RID: 3033
	public class ManholeCover : MonoBehaviour
	{
		// Token: 0x06005517 RID: 21783 RVA: 0x00165FB8 File Offset: 0x001641B8
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x00165FE0 File Offset: 0x001641E0
		private void MinPass()
		{
			Color startColor = this.SteamColor.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			startColor.a = this.SteamAlpha.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.SteamParticles.startColor = startColor;
		}

		// Token: 0x04003F18 RID: 16152
		public ParticleSystem SteamParticles;

		// Token: 0x04003F19 RID: 16153
		public Gradient SteamColor;

		// Token: 0x04003F1A RID: 16154
		public AnimationCurve SteamAlpha;
	}
}
