using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005C2 RID: 1474
	[Serializable]
	public class PatrolInstance
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x00093BFA File Offset: 0x00091DFA
		// (set) Token: 0x060024B2 RID: 9394 RVA: 0x00093C02 File Offset: 0x00091E02
		public PatrolGroup ActiveGroup { get; protected set; }

		// Token: 0x060024B3 RID: 9395 RVA: 0x00093C0C File Offset: 0x00091E0C
		public void Evaluate()
		{
			if (this.ActiveGroup != null)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled))
			{
				this.StartPatrol();
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x00093C68 File Offset: 0x00091E68
		public void StartPatrol()
		{
			if (this.ActiveGroup != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			if (PoliceStation.GetClosestPoliceStation(Vector3.zero).OfficerPool.Count == 0)
			{
				return;
			}
			this.ActiveGroup = Singleton<LawManager>.Instance.StartFootpatrol(this.Route, this.Members);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00093CE2 File Offset: 0x00091EE2
		private void MinPass()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime))
			{
				this.EndPatrol();
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00093D04 File Offset: 0x00091F04
		public void EndPatrol()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (this.ActiveGroup == null)
			{
				return;
			}
			this.ActiveGroup.DisbandGroup();
			this.ActiveGroup = null;
		}

		// Token: 0x04001B44 RID: 6980
		public FootPatrolRoute Route;

		// Token: 0x04001B45 RID: 6981
		public int Members = 2;

		// Token: 0x04001B46 RID: 6982
		public int StartTime = 2000;

		// Token: 0x04001B47 RID: 6983
		public int EndTime = 100;

		// Token: 0x04001B48 RID: 6984
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001B49 RID: 6985
		public bool OnlyIfCurfewEnabled;
	}
}
