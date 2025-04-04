using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;

namespace ScheduleOne.Law
{
	// Token: 0x020005C0 RID: 1472
	public class LawManager : Singleton<LawManager>
	{
		// Token: 0x060024A8 RID: 9384 RVA: 0x00093A2C File Offset: 0x00091C2C
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				PoliceOfficer.Officers.Clear();
			});
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00093A64 File Offset: 0x00091C64
		public void PoliceCalled(Player target, Crime crime)
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			Console.Log("Police called on " + target.PlayerName, null);
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(target.CrimeData.LastKnownPosition);
			target.CrimeData.RecordLastKnownPosition(false);
			closestPoliceStation.Dispatch(2, target, PoliceStation.EDispatchType.Auto, false);
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00093ABC File Offset: 0x00091CBC
		public PatrolGroup StartFootpatrol(FootPatrolRoute route, int requestedMembers)
		{
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(route.Waypoints[route.StartWaypointIndex].position);
			if (closestPoliceStation.OfficerPool.Count == 0)
			{
				Console.LogWarning(closestPoliceStation.name + " has no officers in its pool!", null);
				return null;
			}
			PatrolGroup patrolGroup = new PatrolGroup(route);
			List<PoliceOfficer> list = new List<PoliceOfficer>();
			int num = 0;
			while (num < requestedMembers && closestPoliceStation.OfficerPool.Count != 0)
			{
				list.Add(closestPoliceStation.PullOfficer());
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].StartFootPatrol(patrolGroup, false);
			}
			return patrolGroup;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00093B60 File Offset: 0x00091D60
		public PoliceOfficer StartVehiclePatrol(VehiclePatrolRoute route)
		{
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(route.Waypoints[route.StartWaypointIndex].position);
			if (closestPoliceStation.OfficerPool.Count == 0)
			{
				Console.LogWarning(closestPoliceStation.name + " has no officers in its pool!", null);
				return null;
			}
			LandVehicle landVehicle = closestPoliceStation.CreateVehicle();
			PoliceOfficer policeOfficer = closestPoliceStation.PullOfficer();
			policeOfficer.AssignedVehicle = landVehicle;
			policeOfficer.EnterVehicle(null, landVehicle);
			policeOfficer.StartVehiclePatrol(route, landVehicle);
			return policeOfficer;
		}

		// Token: 0x04001B40 RID: 6976
		public const int DISPATCH_OFFICER_COUNT = 2;

		// Token: 0x04001B41 RID: 6977
		public static float DISPATCH_VEHICLE_USE_THRESHOLD = 25f;
	}
}
