using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005C4 RID: 1476
	[Serializable]
	public class SentryInstance
	{
		// Token: 0x060024B9 RID: 9401 RVA: 0x00094038 File Offset: 0x00092238
		public void Evaluate()
		{
			if (this.Location.AssignedOfficers.Count > 0)
			{
				return;
			}
			if (this.officers.Count > 0)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled))
			{
				this.StartEntry();
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000940AC File Offset: 0x000922AC
		public void StartEntry()
		{
			if (this.Location.AssignedOfficers.Count > 0)
			{
				Console.LogWarning("StartEntry called but location already has active officers", null);
				return;
			}
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(this.Location.transform.position);
			if (closestPoliceStation.OfficerPool.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.Members; i++)
			{
				PoliceOfficer policeOfficer = closestPoliceStation.PullOfficer();
				if (policeOfficer == null)
				{
					Console.LogWarning("Failed to pull officer from station", null);
					break;
				}
				policeOfficer.AssignToSentryLocation(this.Location);
				this.officers.Add(policeOfficer);
			}
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00094168 File Offset: 0x00092368
		private void MinPass()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime))
			{
				this.EndSentry();
			}
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00094188 File Offset: 0x00092388
		public void EndSentry()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			for (int i = 0; i < this.officers.Count; i++)
			{
				this.officers[i].UnassignFromSentryLocation();
			}
			this.officers.Clear();
		}

		// Token: 0x04001B59 RID: 7001
		public SentryLocation Location;

		// Token: 0x04001B5A RID: 7002
		public int Members = 2;

		// Token: 0x04001B5B RID: 7003
		[Header("Timing")]
		public int StartTime = 2000;

		// Token: 0x04001B5C RID: 7004
		public int EndTime = 100;

		// Token: 0x04001B5D RID: 7005
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001B5E RID: 7006
		public bool OnlyIfCurfewEnabled;

		// Token: 0x04001B5F RID: 7007
		private List<PoliceOfficer> officers = new List<PoliceOfficer>();
	}
}
