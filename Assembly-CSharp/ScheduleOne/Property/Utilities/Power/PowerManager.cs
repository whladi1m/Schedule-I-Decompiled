using System;
using System.Collections.Generic;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Power
{
	// Token: 0x0200080D RID: 2061
	public class PowerManager : Singleton<PowerManager>
	{
		// Token: 0x06003873 RID: 14451 RVA: 0x000EECD0 File Offset: 0x000ECED0
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000EED2F File Offset: 0x000ECF2F
		private void MinPass()
		{
			this.usageThisMinute = 0f;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x000EED3C File Offset: 0x000ECF3C
		private void DayPass()
		{
			this.usageAtTime.Clear();
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000EED4C File Offset: 0x000ECF4C
		public float GetTotalUsage()
		{
			float num = 0f;
			foreach (int key in this.usageAtTime.Keys)
			{
				num += this.usageAtTime[key];
			}
			return num;
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000EEDB4 File Offset: 0x000ECFB4
		public void ConsumePower(float kwh)
		{
			this.usageThisMinute += kwh;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000EEDC4 File Offset: 0x000ECFC4
		public PowerLine CreatePowerLine(PowerNode nodeA, PowerNode nodeB, Property p)
		{
			if (!PowerLine.CanNodesBeConnected(nodeA, nodeB))
			{
				Console.LogWarning("Nodes can't be connected!", null);
				return null;
			}
			PowerLine component = Singleton<ConstructionManager>.Instance.CreateConstructable("Utilities/PowerLine/PowerLine").GetComponent<PowerLine>();
			component.transform.SetParent(p.Container.transform);
			component.InitializePowerLine(nodeA, nodeB);
			return component;
		}

		// Token: 0x04002905 RID: 10501
		[Header("Prefabs")]
		public GameObject powerLineSegmentPrefab;

		// Token: 0x04002906 RID: 10502
		public static float pricePerkWh = 0.25f;

		// Token: 0x04002907 RID: 10503
		private Dictionary<int, float> usageAtTime = new Dictionary<int, float>();

		// Token: 0x04002908 RID: 10504
		private float usageThisMinute;
	}
}
