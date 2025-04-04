using System;
using FishNet;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200052A RID: 1322
	public class VehiclePursuitBehaviour : Behaviour
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x00084D47 File Offset: 0x00082F47
		// (set) Token: 0x06002054 RID: 8276 RVA: 0x00084D4F File Offset: 0x00082F4F
		public Player TargetPlayer { get; protected set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x00084D58 File Offset: 0x00082F58
		private bool isDriving
		{
			get
			{
				return this.vehicle.OccupantNPCs[0] == base.Npc;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00084D72 File Offset: 0x00082F72
		private VehicleAgent Agent
		{
			get
			{
				return this.vehicle.Agent;
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x00084D80 File Offset: 0x00082F80
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00084D9F File Offset: 0x00082F9F
		private void OnDestroy()
		{
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Remove(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00084DC1 File Offset: 0x00082FC1
		public void BeginAsSighted()
		{
			this.beginAsSighted = true;
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00084DCC File Offset: 0x00082FCC
		protected override void Begin()
		{
			base.Begin();
			base.Npc.awareness.VisionCone.RangeMultiplier = 1.5f;
			if (this.beginAsSighted)
			{
				this.isTargetVisible = true;
				this.initialContactMade = true;
				this.isTargetStrictlyVisible = true;
				this.SetAggressiveDriving(this.initialContactMade);
				this.DriveTo(this.GetPlayerChasePoint());
			}
			this.StartPursuit();
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00084E34 File Offset: 0x00083034
		protected override void Resume()
		{
			base.Resume();
			this.StartPursuit();
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x00084E44 File Offset: 0x00083044
		protected override void Pause()
		{
			base.Pause();
			this.initialContactMade = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00084EA8 File Offset: 0x000830A8
		protected override void End()
		{
			base.End();
			this.Disable();
			this.initialContactMade = false;
			if (this.vehicle != null)
			{
				PoliceLight componentInChildren = this.vehicle.GetComponentInChildren<PoliceLight>();
				if (componentInChildren != null)
				{
					componentInChildren.IsOn = false;
				}
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
				if (this.TargetPlayer != null)
				{
					(base.Npc as PoliceOfficer).PursuitBehaviour.AssignTarget(null, this.TargetPlayer.NetworkObject);
					(base.Npc as PoliceOfficer).PursuitBehaviour.MarkPlayerVisible();
				}
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00084F7E File Offset: 0x0008317E
		public virtual void AssignTarget(Player target)
		{
			this.TargetPlayer = target;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00084F88 File Offset: 0x00083188
		private void StartPursuit()
		{
			if (this.vehicle == null)
			{
				Console.LogError("VehiclePursuitBehaviour: Vehicle is unassigned", null);
				this.End();
				return;
			}
			if (this.TargetPlayer == null)
			{
				Console.LogError("VehiclePursuitBehaviour: TargetPlayer is unassigned", null);
				this.End();
				return;
			}
			if (InstanceFinder.IsServer && base.Npc.CurrentVehicle != this.vehicle)
			{
				if (base.Npc.CurrentVehicle != null)
				{
					base.Npc.ExitVehicle();
				}
				base.Npc.EnterVehicle(null, this.vehicle);
			}
			PoliceLight componentInChildren = this.vehicle.GetComponentInChildren<PoliceLight>();
			if (componentInChildren != null)
			{
				componentInChildren.IsOn = true;
			}
			if (!this.isDriving)
			{
				Console.Log("Disabling awareness", null);
				base.Npc.awareness.SetAwarenessActive(false);
			}
			this.UpdateDestination();
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x0008506A File Offset: 0x0008326A
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.timeSincePursuitStart += Time.deltaTime;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x0008508C File Offset: 0x0008328C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsTargetValid())
			{
				base.End_Networked(null);
				return;
			}
			this.CheckExitVehicle();
			if (!this.isDriving)
			{
				return;
			}
			this.SetAggressiveDriving(this.initialContactMade);
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000850C7 File Offset: 0x000832C7
		protected virtual void FixedUpdate()
		{
			if (!base.Active)
			{
				return;
			}
			this.CheckPlayerVisibility();
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000850D8 File Offset: 0x000832D8
		private void UpdateDestination()
		{
			if (!base.Active)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Agent.NavigationCalculationInProgress)
			{
				return;
			}
			if (!this.isDriving)
			{
				return;
			}
			if (this.Agent.GetIsStuck() && this.vehicle.speed_Kmh < 4f)
			{
				base.End_Networked(null);
				return;
			}
			if (this.vehicle.VelocityCalculator.Velocity.magnitude < 1f)
			{
				this.timeStationary += 0.2f;
				if (this.timeStationary > 3f && this.timeSincePursuitStart > 10f)
				{
					base.End_Networked(null);
					return;
				}
			}
			else
			{
				this.timeStationary = 0f;
			}
			if (this.isTargetVisible)
			{
				Vector3 b;
				if (this.IsAsCloseAsPossible(this.GetPlayerChasePoint(), out b) || this.IsAsCloseAsPossible(this.TargetPlayer.Avatar.CenterPoint, out b) || Vector3.Distance(this.vehicle.transform.position, b) < 10f)
				{
					this.vehicle.ApplyHandbrake();
					this.Agent.StopNavigating();
					if (this.vehicle.speed_Kmh < 4f)
					{
						base.End_Networked(null);
						return;
					}
				}
				else if (!this.Agent.AutoDriving || Vector3.Distance(this.vehicle.Agent.TargetLocation, this.GetPlayerChasePoint()) > 10f)
				{
					this.DriveTo(this.GetPlayerChasePoint());
				}
				float num = Vector3.Distance(this.currentDriveTarget, this.TargetPlayer.CrimeData.LastKnownPosition);
				float value = Vector3.Distance(base.transform.position, this.TargetPlayer.CrimeData.LastKnownPosition);
				if (num > this.RepathDistanceThresholdMap.Evaluate(Mathf.Clamp(value, 0f, 100f)))
				{
					this.DriveTo(this.GetPlayerChasePoint());
					return;
				}
			}
			else
			{
				if (!this.Agent.AutoDriving)
				{
					Vector3 a;
					if (this.IsAsCloseAsPossible(this.TargetPlayer.CrimeData.LastKnownPosition, out a) || Vector3.Distance(a, this.vehicle.transform.position) < 10f)
					{
						if (this.vehicle.speed_Kmh < 4f)
						{
							base.End_Networked(null);
							return;
						}
					}
					else
					{
						this.DriveTo(this.TargetPlayer.CrimeData.LastKnownPosition);
					}
				}
				float num2 = Vector3.Distance(this.currentDriveTarget, this.TargetPlayer.CrimeData.LastKnownPosition);
				float value2 = Vector3.Distance(base.transform.position, this.TargetPlayer.CrimeData.LastKnownPosition);
				if (num2 > this.RepathDistanceThresholdMap.Evaluate(Mathf.Clamp(value2, 0f, 100f)))
				{
					this.DriveTo(this.TargetPlayer.CrimeData.LastKnownPosition);
				}
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x00085398 File Offset: 0x00083598
		private bool IsTargetValid()
		{
			return !(this.TargetPlayer == null) && !this.TargetPlayer.IsArrested && !this.TargetPlayer.IsUnconscious && this.TargetPlayer.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000853E8 File Offset: 0x000835E8
		private void CheckExitVehicle()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.isDriving && this.vehicle.OccupantNPCs[0] == null)
			{
				base.End_Networked(null);
				return;
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00085418 File Offset: 0x00083618
		private Vector3 GetPlayerChasePoint()
		{
			Mathf.Min(5f, Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position));
			Mathf.Clamp01(this.TargetPlayer.VelocityCalculator.Velocity.magnitude / 8f);
			return this.TargetPlayer.Avatar.CenterPoint;
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00085484 File Offset: 0x00083684
		private void SetAggressiveDriving(bool aggressive)
		{
			bool flag = this.aggressiveDrivingEnabled;
			this.aggressiveDrivingEnabled = aggressive;
			if (aggressive)
			{
				this.vehicle.Agent.Flags.OverriddenSpeed = 80f;
				this.vehicle.Agent.Flags.OverriddenReverseSpeed = 20f;
				this.vehicle.Agent.Flags.OverrideSpeed = true;
				this.vehicle.Agent.Flags.AutoBrakeAtDestination = false;
				this.vehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.vehicle.Agent.Flags.UseRoads = false;
				this.vehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.IgnoreOnlySquishy;
			}
			else
			{
				this.vehicle.Agent.Flags.OverrideSpeed = false;
				this.vehicle.Agent.Flags.SpeedLimitMultiplier = 1.5f;
				this.vehicle.Agent.Flags.AutoBrakeAtDestination = true;
				this.vehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.vehicle.Agent.Flags.UseRoads = true;
				this.vehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.Default;
			}
			if (aggressive != flag && this.vehicle.Agent.AutoDriving)
			{
				this.vehicle.Agent.RecalculateNavigation();
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000855FA File Offset: 0x000837FA
		private void DriveTo(Vector3 location)
		{
			if (!this.Agent.IsOnVehicleGraph())
			{
				this.End();
				return;
			}
			this.targetChanges++;
			this.currentDriveTarget = location;
			this.Agent.Navigate(location, null, null);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00085633 File Offset: 0x00083833
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

		// Token: 0x0600206A RID: 8298 RVA: 0x00084BA8 File Offset: 0x00082DA8
		private bool IsAsCloseAsPossible(Vector3 pos, out Vector3 closestPosition)
		{
			closestPosition = NavigationUtility.SampleVehicleGraph(pos);
			return Vector3.Distance(closestPosition, base.transform.position) < 10f;
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0008566B File Offset: 0x0008386B
		private bool IsPlayerVisible()
		{
			return base.Npc.awareness.VisionCone.IsPlayerVisible(this.TargetPlayer);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00085688 File Offset: 0x00083888
		private void CheckPlayerVisibility()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.isTargetVisible)
			{
				this.playerSightedDuration += Time.fixedDeltaTime;
				if (this.IsPlayerVisible())
				{
					this.initialContactMade = true;
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(true);
					this.timeSinceLastSighting = 0f;
				}
				else
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
				}
			}
			if (!this.IsPlayerVisible())
			{
				this.playerSightedDuration = 0f;
				this.timeSinceLastSighting += Time.fixedDeltaTime;
				this.isTargetVisible = false;
				this.isTargetStrictlyVisible = false;
				if (this.timeSinceLastSighting < 6f)
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
					this.isTargetVisible = true;
					return;
				}
			}
			else
			{
				this.isTargetStrictlyVisible = true;
			}
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0008575C File Offset: 0x0008395C
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.isTargetVisible = true;
				this.initialContactMade = true;
				this.isTargetStrictlyVisible = true;
				this.DriveTo(this.GetPlayerChasePoint());
				if (this.TargetPlayer.IsOwner && this.TargetPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
				{
					this.TargetPlayer.CrimeData.Escalate();
				}
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000857E4 File Offset: 0x000839E4
		private void ProcessThirdPartyVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.isTargetVisible = true;
				this.isTargetStrictlyVisible = true;
				this.DriveTo(this.GetPlayerChasePoint());
			}
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00085853 File Offset: 0x00083A53
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0008586C File Offset: 0x00083A6C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00085885 File Offset: 0x00083A85
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00085894 File Offset: 0x00083A94
		protected virtual void dll()
		{
			base.Awake();
			if (InstanceFinder.IsOffline || InstanceFinder.IsServer)
			{
				VisionCone visionCone = base.Npc.awareness.VisionCone;
				visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
				base.InvokeRepeating("UpdateDestination", 0.5f, 0.2f);
			}
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Combine(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x04001902 RID: 6402
		public new const float MAX_CONSECUTIVE_PATHING_FAILURES = 5f;

		// Token: 0x04001903 RID: 6403
		public const float EXTRA_VISIBILITY_TIME = 6f;

		// Token: 0x04001904 RID: 6404
		public const float EXIT_VEHICLE_MAX_SPEED = 4f;

		// Token: 0x04001905 RID: 6405
		public const float CLOSE_ENOUGH_THRESHOLD = 10f;

		// Token: 0x04001906 RID: 6406
		public const float UPDATE_FREQUENCY = 0.2f;

		// Token: 0x04001907 RID: 6407
		public const float STATIONARY_THRESHOLD = 1f;

		// Token: 0x04001908 RID: 6408
		public const float TIME_STATIONARY_TO_EXIT = 3f;

		// Token: 0x0400190A RID: 6410
		[Header("Settings")]
		public AnimationCurve RepathDistanceThresholdMap;

		// Token: 0x0400190B RID: 6411
		public LandVehicle vehicle;

		// Token: 0x0400190C RID: 6412
		private bool initialContactMade;

		// Token: 0x0400190D RID: 6413
		private bool aggressiveDrivingEnabled;

		// Token: 0x0400190E RID: 6414
		private bool isTargetVisible;

		// Token: 0x0400190F RID: 6415
		private bool isTargetStrictlyVisible;

		// Token: 0x04001910 RID: 6416
		private float playerSightedDuration;

		// Token: 0x04001911 RID: 6417
		private float timeSinceLastSighting = 10000f;

		// Token: 0x04001912 RID: 6418
		private new int consecutivePathingFailures;

		// Token: 0x04001913 RID: 6419
		private float timeStationary;

		// Token: 0x04001914 RID: 6420
		private Vector3 currentDriveTarget = Vector3.zero;

		// Token: 0x04001915 RID: 6421
		private int targetChanges;

		// Token: 0x04001916 RID: 6422
		private float timeSincePursuitStart;

		// Token: 0x04001917 RID: 6423
		private bool beginAsSighted;

		// Token: 0x04001918 RID: 6424
		private bool dll_Excuted;

		// Token: 0x04001919 RID: 6425
		private bool dll_Excuted;
	}
}
