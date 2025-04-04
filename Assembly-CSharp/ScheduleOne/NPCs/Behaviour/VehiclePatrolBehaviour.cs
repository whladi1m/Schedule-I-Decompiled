using System;
using FishNet;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000528 RID: 1320
	public class VehiclePatrolBehaviour : Behaviour
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00084874 File Offset: 0x00082A74
		private bool isDriving
		{
			get
			{
				return this.Vehicle.OccupantNPCs[0] == base.Npc;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x0008488E File Offset: 0x00082A8E
		private VehicleAgent Agent
		{
			get
			{
				return this.Vehicle.Agent;
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0008489B File Offset: 0x00082A9B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000848AF File Offset: 0x00082AAF
		protected override void Begin()
		{
			base.Begin();
			this.StartPatrol();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000848BD File Offset: 0x00082ABD
		protected override void Resume()
		{
			base.Resume();
			this.StartPatrol();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000848CC File Offset: 0x00082ACC
		protected override void Pause()
		{
			base.Pause();
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0008493C File Offset: 0x00082B3C
		protected override void End()
		{
			base.End();
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000849AC File Offset: 0x00082BAC
		public void SetRoute(VehiclePatrolRoute route)
		{
			this.Route = route;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000849B8 File Offset: 0x00082BB8
		private void StartPatrol()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Vehicle == null)
			{
				Console.LogError("VehiclePursuitBehaviour: Vehicle is unassigned", null);
				base.Disable_Networked(null);
				base.End_Networked(null);
				return;
			}
			if (InstanceFinder.IsServer && base.Npc.CurrentVehicle != this.Vehicle)
			{
				if (base.Npc.CurrentVehicle != null)
				{
					base.Npc.ExitVehicle();
				}
				base.Npc.EnterVehicle(null, this.Vehicle);
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00084A44 File Offset: 0x00082C44
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.isDriving)
			{
				return;
			}
			if (this.Agent.AutoDriving)
			{
				if (!this.Agent.NavigationCalculationInProgress && Vector3.Distance(this.Vehicle.transform.position, this.Route.Waypoints[this.CurrentWaypoint].position) < 10f)
				{
					this.CurrentWaypoint++;
					if (this.CurrentWaypoint >= this.Route.Waypoints.Length)
					{
						base.Disable_Networked(null);
						return;
					}
					this.DriveTo(this.Route.Waypoints[this.CurrentWaypoint].position);
					return;
				}
			}
			else
			{
				if (this.CurrentWaypoint >= this.Route.Waypoints.Length)
				{
					base.Disable_Networked(null);
					return;
				}
				this.DriveTo(this.Route.Waypoints[this.CurrentWaypoint].position);
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00084B41 File Offset: 0x00082D41
		private void DriveTo(Vector3 location)
		{
			if (!this.Agent.IsOnVehicleGraph())
			{
				this.End();
				return;
			}
			this.Agent.Navigate(location, null, new VehicleAgent.NavigationCallback(this.NavigationCallback));
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00084B70 File Offset: 0x00082D70
		private void NavigationCallback(VehicleAgent.ENavigationResult status)
		{
			if (status == VehicleAgent.ENavigationResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if ((float)this.consecutivePathingFailures > 5f && InstanceFinder.IsServer)
			{
				base.End_Networked(null);
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00084BA8 File Offset: 0x00082DA8
		private bool IsAsCloseAsPossible(Vector3 pos, out Vector3 closestPosition)
		{
			closestPosition = NavigationUtility.SampleVehicleGraph(pos);
			return Vector3.Distance(closestPosition, base.transform.position) < 10f;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00084BE2 File Offset: 0x00082DE2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00084BFB File Offset: 0x00082DFB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00084C14 File Offset: 0x00082E14
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00084389 File Offset: 0x00082589
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x040018F6 RID: 6390
		public new const float MAX_CONSECUTIVE_PATHING_FAILURES = 5f;

		// Token: 0x040018F7 RID: 6391
		public const float PROGRESSION_THRESHOLD = 10f;

		// Token: 0x040018F8 RID: 6392
		public int CurrentWaypoint;

		// Token: 0x040018F9 RID: 6393
		[Header("Settings")]
		public VehiclePatrolRoute Route;

		// Token: 0x040018FA RID: 6394
		public LandVehicle Vehicle;

		// Token: 0x040018FB RID: 6395
		private bool aggressiveDrivingEnabled = true;

		// Token: 0x040018FC RID: 6396
		private new int consecutivePathingFailures;

		// Token: 0x040018FD RID: 6397
		private bool dll_Excuted;

		// Token: 0x040018FE RID: 6398
		private bool dll_Excuted;
	}
}
