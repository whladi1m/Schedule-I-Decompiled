using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005BC RID: 1468
	[Serializable]
	public class CurfewInstance
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x00092DED File Offset: 0x00090FED
		// (set) Token: 0x06002466 RID: 9318 RVA: 0x00092DF5 File Offset: 0x00090FF5
		public bool Enabled { get; protected set; }

		// Token: 0x06002467 RID: 9319 RVA: 0x00092DFE File Offset: 0x00090FFE
		public void Evaluate(bool ignoreSleepReq = false)
		{
			if (this.Enabled)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && (NetworkSingleton<TimeManager>.Instance.SleepInProgress || ignoreSleepReq))
			{
				this.Enable();
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00092E2F File Offset: 0x0009102F
		private void MinPass()
		{
			if (this.Enabled)
			{
				if (Singleton<LawController>.Instance.LE_Intensity < this.IntensityRequirement)
				{
					this.shouldDisable = true;
				}
				if (this.shouldDisable && NetworkSingleton<TimeManager>.Instance.SleepInProgress)
				{
					this.Disable();
				}
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x00092E6C File Offset: 0x0009106C
		public void Enable()
		{
			CurfewInstance.ActiveInstance = this;
			this.Enabled = true;
			this.shouldDisable = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			NetworkSingleton<CurfewManager>.Instance.Enable(null);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x00092EC0 File Offset: 0x000910C0
		public void Disable()
		{
			this.Enabled = false;
			this.shouldDisable = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (CurfewInstance.ActiveInstance == this)
			{
				NetworkSingleton<CurfewManager>.Instance.Disable();
			}
		}

		// Token: 0x04001B16 RID: 6934
		public static CurfewInstance ActiveInstance;

		// Token: 0x04001B17 RID: 6935
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001B19 RID: 6937
		[HideInInspector]
		public bool shouldDisable;
	}
}
