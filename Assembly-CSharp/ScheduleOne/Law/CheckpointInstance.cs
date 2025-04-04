using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005A7 RID: 1447
	[Serializable]
	public class CheckpointInstance
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000927AE File Offset: 0x000909AE
		// (set) Token: 0x06002420 RID: 9248 RVA: 0x000927B6 File Offset: 0x000909B6
		public RoadCheckpoint activeCheckpoint { get; protected set; }

		// Token: 0x06002421 RID: 9249 RVA: 0x000927C0 File Offset: 0x000909C0
		public void Evaluate()
		{
			if (this.checkPoint == null)
			{
				this.checkPoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location);
			}
			if (this.activeCheckpoint != null)
			{
				return;
			}
			if (this.checkPoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled) && this.DistanceRequirementsMet())
			{
				this.EnableCheckpoint();
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0009285C File Offset: 0x00090A5C
		public void EnableCheckpoint()
		{
			if (this.activeCheckpoint != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			if (PoliceStation.GetClosestPoliceStation(Vector3.zero).OfficerPool.Count == 0)
			{
				return;
			}
			this.activeCheckpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location);
			NetworkSingleton<CheckpointManager>.Instance.SetCheckpointEnabled(this.Location, true, this.Members);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000928F0 File Offset: 0x00090AF0
		private bool DistanceRequirementsMet()
		{
			float num;
			Player closestPlayer = Player.GetClosestPlayer(NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location).transform.position, out num, null);
			return NetworkSingleton<TimeManager>.Instance.SleepInProgress || closestPlayer == null || num >= 50f;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x00092940 File Offset: 0x00090B40
		private void MinPass()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && this.DistanceRequirementsMet())
			{
				this.DisableCheckpoint();
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x00092968 File Offset: 0x00090B68
		public void DisableCheckpoint()
		{
			if (this.activeCheckpoint == null)
			{
				return;
			}
			NetworkSingleton<CheckpointManager>.Instance.SetCheckpointEnabled(this.Location, false, this.Members);
			this.activeCheckpoint = null;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x04001AF0 RID: 6896
		public const float MIN_ACTIVATION_DISTANCE = 50f;

		// Token: 0x04001AF1 RID: 6897
		public CheckpointManager.ECheckpointLocation Location;

		// Token: 0x04001AF2 RID: 6898
		public int Members = 2;

		// Token: 0x04001AF3 RID: 6899
		public int StartTime = 800;

		// Token: 0x04001AF4 RID: 6900
		public int EndTime = 2000;

		// Token: 0x04001AF5 RID: 6901
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001AF6 RID: 6902
		public bool OnlyIfCurfewEnabled;

		// Token: 0x04001AF7 RID: 6903
		private RoadCheckpoint checkPoint;
	}
}
