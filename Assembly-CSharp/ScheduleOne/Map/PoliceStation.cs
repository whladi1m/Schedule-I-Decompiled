using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C07 RID: 3079
	public class PoliceStation : NPCEnterableBuilding
	{
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600560D RID: 22029 RVA: 0x001694AA File Offset: 0x001676AA
		// (set) Token: 0x0600560E RID: 22030 RVA: 0x001694B2 File Offset: 0x001676B2
		public float TimeSinceLastDispatch { get; private set; }

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600560F RID: 22031 RVA: 0x001694BB File Offset: 0x001676BB
		private int deployedVehicleCount
		{
			get
			{
				return (from v in this.deployedVehicles
				where v != null
				select v).Count<LandVehicle>();
			}
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x001694EC File Offset: 0x001676EC
		protected override void Awake()
		{
			base.Awake();
			if (!PoliceStation.PoliceStations.Contains(this))
			{
				PoliceStation.PoliceStations.Add(this);
			}
			base.InvokeRepeating("CleanVehicleList", 0f, 5f);
		}

		// Token: 0x06005611 RID: 22033 RVA: 0x00169521 File Offset: 0x00167721
		private void OnDestroy()
		{
			if (PoliceStation.PoliceStations.Contains(this))
			{
				PoliceStation.PoliceStations.Remove(this);
			}
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x0016953C File Offset: 0x0016773C
		private void Update()
		{
			this.TimeSinceLastDispatch += Time.deltaTime;
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x00169550 File Offset: 0x00167750
		private void CleanVehicleList()
		{
			for (int i = 0; i < this.deployedVehicles.Count; i++)
			{
				if (this.deployedVehicles[i] == null)
				{
					this.deployedVehicles.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x00169598 File Offset: 0x00167798
		public void Dispatch(int requestedOfficerCount, Player targetPlayer, PoliceStation.EDispatchType type = PoliceStation.EDispatchType.Auto, bool beginAsSighted = false)
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("Attempted to dispatch officers from a client, this is not allowed.", null);
				return;
			}
			if (requestedOfficerCount <= 0)
			{
				return;
			}
			if (requestedOfficerCount > 4)
			{
				Console.LogWarning("Attempted to dispatch more than 4 officers, this is not allowed.", null);
				return;
			}
			List<PoliceOfficer> list = new List<PoliceOfficer>();
			for (int i = 0; i < requestedOfficerCount; i++)
			{
				if (this.OfficerPool.Count > 0)
				{
					list.Add(this.PullOfficer());
				}
			}
			if (list.Count == 0)
			{
				Console.LogWarning("Attempted to dispatch officers, but there are no officers in the pool.", null);
				return;
			}
			bool flag = false;
			if (type == PoliceStation.EDispatchType.Auto)
			{
				flag = (Vector3.Distance(targetPlayer.CrimeData.LastKnownPosition, this.SpawnPoint.position) > LawManager.DISPATCH_VEHICLE_USE_THRESHOLD || targetPlayer.CurrentVehicle != null);
			}
			else if (type == PoliceStation.EDispatchType.UseVehicle)
			{
				flag = true;
			}
			if (flag && this.deployedVehicleCount < this.VehicleLimit)
			{
				LandVehicle landVehicle = this.CreateVehicle();
				list[0].AssignedVehicle = landVehicle;
				list[0].EnterVehicle(null, landVehicle);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].BeginVehiclePursuit_Networked(targetPlayer.NetworkObject, landVehicle.NetworkObject, beginAsSighted);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				list[k].BeginFootPursuit_Networked(targetPlayer.NetworkObject, true);
			}
			this.TimeSinceLastDispatch = 0f;
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x001696E4 File Offset: 0x001678E4
		public PoliceOfficer PullOfficer()
		{
			if (this.OfficerPool.Count == 0)
			{
				Console.LogWarning("Attempted to pull an officer from the station, but there are no officers in the pool.", null);
				return null;
			}
			PoliceOfficer policeOfficer = this.OfficerPool[UnityEngine.Random.Range(0, this.OfficerPool.Count)];
			this.OfficerPool.Remove(policeOfficer);
			policeOfficer.Activate();
			return policeOfficer;
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x0016973C File Offset: 0x0016793C
		public LandVehicle CreateVehicle()
		{
			Transform target = this.VehicleSpawnPoints[0];
			for (int i = 0; i < this.VehicleSpawnPoints.Length; i++)
			{
				if (PoliceStation.<CreateVehicle>g__IsSpawnPointAvailable|21_0(this.VehicleSpawnPoints[i]))
				{
					target = this.VehicleSpawnPoints[i];
					break;
				}
			}
			LandVehicle landVehicle = this.PoliceVehiclePrefabs[UnityEngine.Random.Range(0, this.PoliceVehiclePrefabs.Length)];
			Tuple<Vector3, Quaternion> alignmentTransform = landVehicle.GetAlignmentTransform(target, EParkingAlignment.RearToKerb);
			LandVehicle landVehicle2 = NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle(landVehicle.VehicleCode, alignmentTransform.Item1, alignmentTransform.Item2, false);
			this.deployedVehicles.Add(landVehicle2);
			return landVehicle2;
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x001697CF File Offset: 0x001679CF
		public override void NPCEnteredBuilding(NPC npc)
		{
			base.NPCEnteredBuilding(npc);
			if (npc is PoliceOfficer && !this.OfficerPool.Contains(npc as PoliceOfficer))
			{
				this.OfficerPool.Add(npc as PoliceOfficer);
			}
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x00169804 File Offset: 0x00167A04
		public override void NPCExitedBuilding(NPC npc)
		{
			base.NPCExitedBuilding(npc);
			if (npc is PoliceOfficer)
			{
				this.OfficerPool.Remove(npc as PoliceOfficer);
			}
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x00169827 File Offset: 0x00167A27
		public static PoliceStation GetClosestPoliceStation(Vector3 point)
		{
			return PoliceStation.PoliceStations[0];
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00169868 File Offset: 0x00167A68
		[CompilerGenerated]
		internal static bool <CreateVehicle>g__IsSpawnPointAvailable|21_0(Transform spawnPoint)
		{
			Collider[] array = Physics.OverlapSphere(spawnPoint.position, 2f, 1 << LayerMask.NameToLayer("Vehicle"));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetComponentInParent<LandVehicle>() != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003FF8 RID: 16376
		public static List<PoliceStation> PoliceStations = new List<PoliceStation>();

		// Token: 0x04003FF9 RID: 16377
		public int VehicleLimit = 5;

		// Token: 0x04003FFA RID: 16378
		[Header("References")]
		public Transform SpawnPoint;

		// Token: 0x04003FFB RID: 16379
		public Transform[] VehicleSpawnPoints;

		// Token: 0x04003FFC RID: 16380
		public Transform[] PossessedVehicleSpawnPoints;

		// Token: 0x04003FFD RID: 16381
		[Header("Prefabs")]
		public LandVehicle[] PoliceVehiclePrefabs;

		// Token: 0x04003FFE RID: 16382
		public List<PoliceOfficer> OfficerPool = new List<PoliceOfficer>();

		// Token: 0x04004000 RID: 16384
		[SerializeField]
		private List<LandVehicle> deployedVehicles = new List<LandVehicle>();

		// Token: 0x02000C08 RID: 3080
		public enum EDispatchType
		{
			// Token: 0x04004002 RID: 16386
			Auto,
			// Token: 0x04004003 RID: 16387
			UseVehicle,
			// Token: 0x04004004 RID: 16388
			OnFoot
		}
	}
}
