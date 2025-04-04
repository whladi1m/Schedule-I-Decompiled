using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Water
{
	// Token: 0x0200080B RID: 2059
	public class WaterManager : Singleton<WaterManager>
	{
		// Token: 0x06003859 RID: 14425 RVA: 0x000EE2BC File Offset: 0x000EC4BC
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000EE31B File Offset: 0x000EC51B
		private void MinPass()
		{
			this.usageThisMinute = 0f;
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000EE328 File Offset: 0x000EC528
		private void DayPass()
		{
			this.usageAtTime.Clear();
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000EE338 File Offset: 0x000EC538
		public float GetTotalUsage()
		{
			float num = 0f;
			foreach (int key in this.usageAtTime.Keys)
			{
				num += this.usageAtTime[key];
			}
			return num;
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000EE3A0 File Offset: 0x000EC5A0
		public void ConsumeWater(float litres)
		{
			this.usageThisMinute += litres;
		}

		// Token: 0x040028F5 RID: 10485
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject waterPipePrefab;

		// Token: 0x040028F6 RID: 10486
		public static float pricePerL = 0.1f;

		// Token: 0x040028F7 RID: 10487
		private Dictionary<int, float> usageAtTime = new Dictionary<int, float>();

		// Token: 0x040028F8 RID: 10488
		private float usageThisMinute;
	}
}
