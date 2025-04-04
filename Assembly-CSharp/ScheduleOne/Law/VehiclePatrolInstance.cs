using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005C6 RID: 1478
	[Serializable]
	public class VehiclePatrolInstance
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x0009423F File Offset: 0x0009243F
		private PoliceStation nearestStation
		{
			get
			{
				return PoliceStation.GetClosestPoliceStation(Vector3.zero);
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0009424C File Offset: 0x0009244C
		public void Evaluate()
		{
			if (this.activeOfficer != null)
			{
				this.CheckEnd();
				return;
			}
			if (this.nearestStation.OfficerPool.Count == 0)
			{
				return;
			}
			this.latestStartTime = TimeManager.AddMinutesTo24HourTime(this.StartTime, 30);
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.latestStartTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled))
			{
				if (!this.startedThisCycle && Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement)
				{
					this.StartPatrol();
					return;
				}
			}
			else
			{
				this.startedThisCycle = false;
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000942E8 File Offset: 0x000924E8
		private void CheckEnd()
		{
			if (this.activeOfficer != null && !this.activeOfficer.VehiclePatrolBehaviour.Enabled)
			{
				this.activeOfficer = null;
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00094314 File Offset: 0x00092514
		public void StartPatrol()
		{
			if (this.activeOfficer != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			this.startedThisCycle = true;
			if (this.nearestStation.OfficerPool.Count == 0)
			{
				return;
			}
			this.activeOfficer = Singleton<LawManager>.Instance.StartVehiclePatrol(this.Route);
		}

		// Token: 0x04001B62 RID: 7010
		public VehiclePatrolRoute Route;

		// Token: 0x04001B63 RID: 7011
		public int StartTime = 2000;

		// Token: 0x04001B64 RID: 7012
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001B65 RID: 7013
		public bool OnlyIfCurfewEnabled;

		// Token: 0x04001B66 RID: 7014
		private PoliceOfficer activeOfficer;

		// Token: 0x04001B67 RID: 7015
		private int latestStartTime;

		// Token: 0x04001B68 RID: 7016
		private bool startedThisCycle;
	}
}
