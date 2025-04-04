using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E8 RID: 1768
	public class ValueTracker
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x000C8272 File Offset: 0x000C6472
		public ValueTracker(float HistoryDuration)
		{
			this.historyDuration = HistoryDuration;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onUpdate = (Action)Delegate.Combine(instance.onUpdate, new Action(this.Update));
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000C82B2 File Offset: 0x000C64B2
		public void Destroy()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onUpdate = (Action)Delegate.Remove(instance.onUpdate, new Action(this.Update));
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000C82DC File Offset: 0x000C64DC
		public void Update()
		{
			int num = 0;
			while (num < this.valueHistory.Count && Time.timeSinceLevelLoad - this.valueHistory[num].time > this.historyDuration)
			{
				this.valueHistory.RemoveAt(num);
				num--;
				num++;
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000C832E File Offset: 0x000C652E
		public void SubmitValue(float value)
		{
			this.valueHistory.Add(new ValueTracker.Value(value, Time.timeSinceLevelLoad));
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000C8346 File Offset: 0x000C6546
		public float RecordedHistoryLength()
		{
			if (this.valueHistory.Count == 0)
			{
				return 0f;
			}
			return Time.timeSinceLevelLoad - this.valueHistory[0].time;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000C8374 File Offset: 0x000C6574
		public float GetLowestValue()
		{
			ValueTracker.Value value = (from x in this.valueHistory
			orderby x.val
			select x).FirstOrDefault<ValueTracker.Value>();
			if (value != null)
			{
				return value.val;
			}
			return 0f;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000C83C0 File Offset: 0x000C65C0
		public float GetAverageValue()
		{
			if (this.valueHistory.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			foreach (ValueTracker.Value value in this.valueHistory)
			{
				num += value.val;
			}
			num /= (float)this.valueHistory.Count;
			return num;
		}

		// Token: 0x0400224D RID: 8781
		private float historyDuration;

		// Token: 0x0400224E RID: 8782
		private List<ValueTracker.Value> valueHistory = new List<ValueTracker.Value>();

		// Token: 0x020006E9 RID: 1769
		public class Value
		{
			// Token: 0x06003014 RID: 12308 RVA: 0x000C8440 File Offset: 0x000C6640
			public Value(float val, float time)
			{
				this.val = val;
				this.time = time;
			}

			// Token: 0x0400224F RID: 8783
			public float val;

			// Token: 0x04002250 RID: 8784
			public float time;
		}
	}
}
