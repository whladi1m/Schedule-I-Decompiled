using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Skating;
using ScheduleOne.Tools;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000453 RID: 1107
	public class NPCMovement : NetworkBehaviour
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00066D9E File Offset: 0x00064F9E
		// (set) Token: 0x06001756 RID: 5974 RVA: 0x00066DA6 File Offset: 0x00064FA6
		public bool hasDestination { get; protected set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x00066DAF File Offset: 0x00064FAF
		public bool IsMoving
		{
			get
			{
				return ((this.Agent.hasPath || this.Agent.pathPending) && this.Agent.remainingDistance > 0.25f) || this.forceIsMoving;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x00066DE5 File Offset: 0x00064FE5
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x00066DED File Offset: 0x00064FED
		public bool IsPaused { get; protected set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x00066DF6 File Offset: 0x00064FF6
		public Vector3 FootPosition
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x00066E03 File Offset: 0x00065003
		// (set) Token: 0x0600175C RID: 5980 RVA: 0x00066E0B File Offset: 0x0006500B
		public float GravityMultiplier { get; protected set; } = 1f;

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00066E14 File Offset: 0x00065014
		// (set) Token: 0x0600175E RID: 5982 RVA: 0x00066E1C File Offset: 0x0006501C
		public NPCMovement.EStance Stance { get; protected set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x00066E25 File Offset: 0x00065025
		// (set) Token: 0x06001760 RID: 5984 RVA: 0x00066E2D File Offset: 0x0006502D
		public float timeSinceHitByCar { get; protected set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x00066E36 File Offset: 0x00065036
		public bool FaceDirectionInProgress
		{
			get
			{
				return this.FaceDirectionRoutine != null;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00066E41 File Offset: 0x00065041
		// (set) Token: 0x06001763 RID: 5987 RVA: 0x00066E49 File Offset: 0x00065049
		public Vector3 CurrentDestination { get; protected set; } = Vector3.zero;

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00066E52 File Offset: 0x00065052
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x00066E5A File Offset: 0x0006505A
		public NPCPathCache PathCache { get; private set; } = new NPCPathCache();

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00066E63 File Offset: 0x00065063
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x00066E6B File Offset: 0x0006506B
		public bool Disoriented { get; set; }

		// Token: 0x06001768 RID: 5992 RVA: 0x00066E74 File Offset: 0x00065074
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCMovement_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00066E94 File Offset: 0x00065094
		private void Start()
		{
			string text = this.npc.BakedGUID;
			if (text != string.Empty)
			{
				if (text[text.Length - 1] != '1')
				{
					text = text.Substring(0, text.Length - 1) + "1";
				}
				else
				{
					text = text.Substring(0, text.Length - 1) + "2";
				}
				this.RagdollDraggable.SetGUID(new Guid(text));
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00066F13 File Offset: 0x00065113
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!InstanceFinder.IsServer)
			{
				this.Agent.enabled = false;
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00066F2E File Offset: 0x0006512E
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00066F37 File Offset: 0x00065137
		protected virtual void Update()
		{
			bool debug = this.DEBUG;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00066F40 File Offset: 0x00065140
		protected virtual void LateUpdate()
		{
			this.forceIsMoving = false;
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00066F49 File Offset: 0x00065149
		private void UpdateRagdoll()
		{
			if (!this.npc.IsConscious)
			{
				return;
			}
			if (this.anim.Avatar.Ragdolled && this.ragdollStaticTime > 1.5f)
			{
				this.DeactivateRagdoll();
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00066F80 File Offset: 0x00065180
		[Button]
		private void Stumble()
		{
			this.timeUntilNextStumble = UnityEngine.Random.Range(5f, 15f);
			if (UnityEngine.Random.Range(1f, 0f) < 0.1f)
			{
				this.ActivateRagdoll_Server();
				return;
			}
			this.timeSinceStumble = 0f;
			this.stumbleDirection = UnityEngine.Random.onUnitSphere;
			this.stumbleDirection.y = 0f;
			this.stumbleDirection.Normalize();
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00066FF0 File Offset: 0x000651F0
		private void UpdateDestination()
		{
			if (!this.hasDestination)
			{
				return;
			}
			if (this.npc.IsInVehicle)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
				return;
			}
			if (!this.IsMoving && !this.Agent.pathPending && this.CanMove())
			{
				if (this.IsAsCloseAsPossible(this.CurrentDestination, 0.5f))
				{
					if (this.Agent.hasPath)
					{
						this.Agent.ResetPath();
					}
					if (Vector3.Distance(this.CurrentDestination, this.FootPosition) < this.currentMaxDistanceForSuccess || Vector3.Distance(this.CurrentDestination, base.transform.position) < this.currentMaxDistanceForSuccess)
					{
						this.EndSetDestination(NPCMovement.WalkResult.Success);
						return;
					}
					this.EndSetDestination(NPCMovement.WalkResult.Partial);
					return;
				}
				else
				{
					this.SetDestination(this.CurrentDestination, this.walkResultCallback, false, this.currentMaxDistanceForSuccess, 1f);
				}
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000670D4 File Offset: 0x000652D4
		protected virtual void FixedUpdate()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsPaused)
			{
				this.Agent.isStopped = true;
			}
			this.timeSinceHitByCar += Time.fixedDeltaTime;
			this.capsuleCollider.transform.position = this.ragdollCentralRB.transform.position;
			this.UpdateSpeed();
			this.UpdateStumble();
			this.UpdateRagdoll();
			this.UpdateDestination();
			this.RecordVelocity();
			this.UpdateSlippery();
			this.UpdateCache();
			if (!this.anim.Avatar.Ragdolled || !this.CanRecoverFromRagdoll())
			{
				this.ragdollStaticTime = 0f;
				return;
			}
			this.ragdollTime += Time.fixedDeltaTime;
			if (this.ragdollCentralRB.velocity.magnitude < 0.25f)
			{
				this.ragdollStaticTime += Time.fixedDeltaTime;
				return;
			}
			this.ragdollStaticTime = 0f;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000671CC File Offset: 0x000653CC
		private void UpdateStumble()
		{
			if (this.Disoriented && this.IsMoving)
			{
				this.timeUntilNextStumble -= Time.fixedDeltaTime;
				if (this.timeUntilNextStumble <= 0f)
				{
					this.Stumble();
				}
			}
			this.timeSinceStumble += Time.fixedDeltaTime;
			if (this.timeSinceStumble < 0.66f)
			{
				this.Agent.Move(this.stumbleDirection * (0.66f - this.timeSinceStumble) * Time.fixedDeltaTime * 7f);
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00067264 File Offset: 0x00065464
		private void UpdateSpeed()
		{
			if ((double)this.MovementSpeedScale >= 0.0)
			{
				this.Agent.speed = Mathf.Lerp(this.WalkSpeed, this.RunSpeed, this.MovementSpeedScale) * this.MoveSpeedMultiplier;
				return;
			}
			this.Agent.speed = 0f;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000672C0 File Offset: 0x000654C0
		private void RecordVelocity()
		{
			if (this.timeSinceLastVelocityHistoryRecord > this.velocityHistorySpacing)
			{
				this.timeSinceLastVelocityHistoryRecord = 0f;
				this.desiredVelocityHistory.Add(this.Agent.velocity);
				if (this.desiredVelocityHistory.Count > this.desiredVelocityHistoryLength)
				{
					this.desiredVelocityHistory.RemoveAt(0);
					return;
				}
			}
			else
			{
				this.timeSinceLastVelocityHistoryRecord += Time.fixedDeltaTime;
			}
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00067330 File Offset: 0x00065530
		private void UpdateSlippery()
		{
			if (this.SlipperyMode)
			{
				Vector3 vector = Vector3.zero;
				foreach (Vector3 b in this.desiredVelocityHistory)
				{
					vector += b;
				}
				vector /= (float)this.desiredVelocityHistory.Count;
				if (this.Agent.enabled && this.Agent.isOnNavMesh)
				{
					float num = Vector3.Angle(vector, base.transform.forward);
					this.Agent.Move(vector * this.SlipperyModeMultiplier * Time.fixedDeltaTime * Mathf.Clamp01(num / 90f));
				}
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00067408 File Offset: 0x00065608
		private void UpdateCache()
		{
			if (this.cacheNextPath && this.Agent.path != null && this.Agent.path.corners.Length > 1)
			{
				this.cacheNextPath = false;
				this.PathCache.AddPath(this.Agent.path.corners[0], this.Agent.path.corners[this.Agent.path.corners.Length - 1], this.Agent.path);
			}
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0006749B File Offset: 0x0006569B
		public bool CanRecoverFromRagdoll()
		{
			return !this.npc.behaviour.RagdollBehaviour.Seizure;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000674B8 File Offset: 0x000656B8
		private void UpdateAvoidance()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 25f)
			{
				this.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
				return;
			}
			this.Agent.obstacleAvoidanceType = this.DefaultObstacleAvoidanceType;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000674FF File Offset: 0x000656FF
		public void OnTriggerEnter(Collider other)
		{
			this.CheckHit(other, this.capsuleCollider, false, other.transform.position);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0006751A File Offset: 0x0006571A
		public void OnCollisionEnter(Collision collision)
		{
			this.CheckHit(collision.collider, collision.contacts[0].thisCollider, true, collision.contacts[0].point);
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0006754C File Offset: 0x0006574C
		private void CheckHit(Collider other, Collider thisCollider, bool isCollision, Vector3 hitPoint)
		{
			float num;
			Player closestPlayer = Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 30f)
			{
				return;
			}
			if ((other.gameObject.layer == LayerMask.NameToLayer("Vehicle") || other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) && !this.anim.Avatar.Ragdolled)
			{
				LandVehicle landVehicle = other.GetComponentInParent<LandVehicle>();
				if (landVehicle == null)
				{
					VehicleHumanoidCollider componentInParent = other.GetComponentInParent<VehicleHumanoidCollider>();
					if (componentInParent != null)
					{
						landVehicle = componentInParent.vehicle;
					}
				}
				if (landVehicle != null && this.npc.CurrentVehicle != landVehicle && Mathf.Abs(landVehicle.speed_Kmh) > 10f)
				{
					this.ActivateRagdoll_Server();
					if (this.onHitByCar != null)
					{
						this.onHitByCar.Invoke(other.GetComponentInParent<LandVehicle>());
					}
					this.timeSinceHitByCar = 0f;
					return;
				}
			}
			else if (other.GetComponentInParent<Skateboard>() != null && !this.anim.Avatar.Ragdolled)
			{
				if (other.GetComponentInParent<Skateboard>().VelocityCalculator.Velocity.magnitude > 2.777778f)
				{
					this.ActivateRagdoll_Server();
					this.npc.PlayVO(EVOLineType.Hurt);
					return;
				}
			}
			else if (other.GetComponentInParent<PhysicsDamageable>() != null && InstanceFinder.IsServer)
			{
				PhysicsDamageable componentInParent2 = other.GetComponentInParent<PhysicsDamageable>();
				float num2 = Mathf.Sqrt(componentInParent2.Rb.mass) * componentInParent2.Rb.velocity.magnitude;
				float num3 = componentInParent2.Rb.velocity.magnitude;
				if (num3 > 40f)
				{
					return;
				}
				if (num3 > 1f)
				{
					num3 = Mathf.Pow(num3, 1.5f);
				}
				else
				{
					num3 = Mathf.Sqrt(num3);
				}
				if (num2 > 10f)
				{
					float num4 = 1f;
					NPCMovement.EStance stance = this.Stance;
					if (stance != NPCMovement.EStance.None)
					{
						if (stance == NPCMovement.EStance.Stanced)
						{
							num4 = 0.5f;
						}
					}
					else
					{
						num4 = 1f;
					}
					float num5 = num2 * 2.5f;
					float num6 = num2 * 0.3f;
					if (num2 > 20f)
					{
						this.npc.Health.TakeDamage(num6, false);
						this.npc.ProcessImpactForce(hitPoint, componentInParent2.Rb.velocity.normalized, num5 * num4);
					}
					Impact impact = new Impact(default(RaycastHit), hitPoint, componentInParent2.Rb.velocity.normalized, num5, num6, EImpactType.PhysicsProp, (num < 15f) ? closestPlayer : null, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
					this.npc.responses.ImpactReceived(impact);
				}
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0006780D File Offset: 0x00065A0D
		public void Warp(Transform target)
		{
			this.Warp(target.position);
			this.FaceDirection(target.forward, 0.5f);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0006782C File Offset: 0x00065A2C
		public void Warp(Vector3 position)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsNPCPositionValid(position))
			{
				string str = "NPCMovement.Warp called with invalid position: ";
				Vector3 vector = position;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			this.Agent.Warp(position);
			this.ReceiveWarp(position);
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00067880 File Offset: 0x00065A80
		[ObserversRpc(ExcludeServer = true)]
		private void ReceiveWarp(Vector3 position)
		{
			this.RpcWriter___Observers_ReceiveWarp_4276783012(position);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00067897 File Offset: 0x00065A97
		public void VisibilityChange(bool visible)
		{
			this.capsuleCollider.gameObject.SetActive(visible);
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000678AA File Offset: 0x00065AAA
		public bool CanMove()
		{
			return !this.anim.Avatar.Ragdolled && !this.npc.isInBuilding && !this.npc.IsInVehicle;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000678DC File Offset: 0x00065ADC
		public void SetAgentType(NPCMovement.EAgentType type)
		{
			string name = type.ToString();
			if (type == NPCMovement.EAgentType.BigHumanoid)
			{
				name = "Big Humanoid";
			}
			if (type == NPCMovement.EAgentType.IgnoreCosts)
			{
				name = "Ignore Costs";
			}
			this.Agent.agentTypeID = NavMeshUtility.GetNavMeshAgentID(name);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0006791C File Offset: 0x00065B1C
		public void SetSeat(AvatarSeat seat)
		{
			this.npc.Avatar.Anim.SetSeat(seat);
			this.Agent.enabled = (seat == null && InstanceFinder.IsServer);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00067950 File Offset: 0x00065B50
		public void SetStance(NPCMovement.EStance stance)
		{
			this.Stance = stance;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0006795C File Offset: 0x00065B5C
		public void SetGravityMultiplier(float multiplier)
		{
			this.GravityMultiplier = multiplier;
			foreach (ConstantForce constantForce in this.ragdollForceComponents)
			{
				constantForce.force = Physics.gravity * this.GravityMultiplier * constantForce.GetComponent<Rigidbody>().mass;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x000679D8 File Offset: 0x00065BD8
		public void SetRagdollDraggable(bool draggable)
		{
			this.RagdollDraggable.enabled = draggable;
			this.RagdollDraggableCollider.enabled = draggable;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x000679F2 File Offset: 0x00065BF2
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void ActivateRagdoll_Server()
		{
			this.RpcWriter___Server_ActivateRagdoll_Server_2166136261();
			this.RpcLogic___ActivateRagdoll_Server_2166136261();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00067A00 File Offset: 0x00065C00
		[ObserversRpc(RunLocally = true)]
		public void ActivateRagdoll(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.RpcWriter___Observers_ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
			this.RpcLogic___ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00067A34 File Offset: 0x00065C34
		[ObserversRpc(RunLocally = true)]
		public void ApplyRagdollForce(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.RpcWriter___Observers_ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
			this.RpcLogic___ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00067A68 File Offset: 0x00065C68
		[ObserversRpc(RunLocally = true)]
		public void DeactivateRagdoll()
		{
			this.RpcWriter___Observers_DeactivateRagdoll_2166136261();
			this.RpcLogic___DeactivateRagdoll_2166136261();
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00067A84 File Offset: 0x00065C84
		private bool SmartSampleNavMesh(Vector3 position, out NavMeshHit hit, float minRadius = 1f, float maxRadius = 10f, int steps = 3)
		{
			hit = default(NavMeshHit);
			NavMeshQueryFilter filter = default(NavMeshQueryFilter);
			filter.agentTypeID = NavMeshUtility.GetNavMeshAgentID("Humanoid");
			filter.areaMask = -1;
			for (int i = 0; i < steps; i++)
			{
				float maxDistance = Mathf.Lerp(minRadius, maxRadius, (float)(i / steps));
				if (NavMesh.SamplePosition(base.transform.position, out hit, maxDistance, filter))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00067AEC File Offset: 0x00065CEC
		public void SetDestination(Vector3 pos)
		{
			this.SetDestination(pos, null, 1f, 1f);
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00067B00 File Offset: 0x00065D00
		public void SetDestination(ITransitEntity entity)
		{
			this.SetDestination(NavMeshUtility.GetAccessPoint(entity, this.npc).position);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00067B19 File Offset: 0x00065D19
		public void SetDestination(Vector3 pos, Action<NPCMovement.WalkResult> callback = null, float maximumDistanceForSuccess = 1f, float cacheMaxDistSqr = 1f)
		{
			this.SetDestination(pos, callback, true, maximumDistanceForSuccess, cacheMaxDistSqr);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00067B28 File Offset: 0x00065D28
		private void SetDestination(Vector3 pos, Action<NPCMovement.WalkResult> callback = null, bool interruptExistingCallback = true, float successThreshold = 1f, float cacheMaxDistSqr = 1f)
		{
			if (!this.IsNPCPositionValid(pos))
			{
				string str = "NPCMovement.SetDestination called with invalid position: ";
				Vector3 vector = pos;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			if (this.npc.Avatar.Anim.IsSeated)
			{
				this.npc.Movement.SetSeat(null);
			}
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("NPCMovement.SetDestination called on client", null);
				return;
			}
			if (this.npc.isInBuilding)
			{
				this.npc.ExitBuilding("");
			}
			if (this.DEBUG)
			{
				string fullName = this.npc.fullName;
				string str2 = " SetDestination called: ";
				Vector3 vector = pos;
				Console.Log(fullName + str2 + vector.ToString(), null);
				Debug.DrawLine(this.FootPosition, pos, Color.green, 1f);
			}
			if (!this.CanMove())
			{
				Console.LogWarning("NPCMovement.SetDestination called but CanWalk == false (" + this.npc.fullName + ")", null);
				return;
			}
			if (!this.Agent.isOnNavMesh)
			{
				Console.LogWarning("NPC is not on navmesh; warping to navmesh", null);
				NavMeshHit navMeshHit;
				if (!this.SmartSampleNavMesh(base.transform.position, out navMeshHit, 1f, 10f, 3))
				{
					Console.LogWarning("NavMesh sample failed at " + base.transform.position.ToString(), null);
					return;
				}
				this.Agent.Warp(navMeshHit.position);
				this.Agent.enabled = false;
				this.Agent.enabled = true;
			}
			if (this.walkResultCallback != null && interruptExistingCallback)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
			}
			this.walkResultCallback = callback;
			this.currentMaxDistanceForSuccess = successThreshold;
			if (this.npc.IsInVehicle)
			{
				Console.LogWarning("SetDestination called but NPC is in a vehicle; returning WalkResult.Failed", null);
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			Vector3 zero = Vector3.zero;
			if (!this.GetClosestReachablePoint(pos, out zero))
			{
				string fullName2 = this.npc.fullName;
				string str3 = " failed to find closest reachable point for destination: ";
				Vector3 vector = pos;
				Console.LogWarning(fullName2 + str3 + vector.ToString(), null);
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			if (!this.IsNPCPositionValid(zero))
			{
				string fullName3 = this.npc.fullName;
				string str4 = " failed to find valid reachable point for destination: ";
				Vector3 vector = pos;
				Console.LogWarning(fullName3 + str4 + vector.ToString(), null);
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			this.hasDestination = true;
			this.CurrentDestination = pos;
			this.currentDestination_Reachable = zero;
			NavMeshPath path = this.PathCache.GetPath(this.Agent.transform.position, zero, cacheMaxDistSqr);
			bool flag = false;
			if (path != null)
			{
				try
				{
					flag = this.Agent.SetPath(path);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Agent.SetDestination error: " + ex.Message, null);
					flag = false;
				}
			}
			if (!flag)
			{
				if (this.DEBUG)
				{
					Console.Log("No cached path for " + this.npc.fullName + "; calculating new path", null);
				}
				try
				{
					this.Agent.SetDestination(zero);
					this.cacheNextPath = true;
				}
				catch (Exception ex2)
				{
					Console.LogWarning("Agent.SetDestination error: " + ex2.Message, null);
				}
			}
			if (this.IsPaused)
			{
				this.Agent.isStopped = true;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00067E68 File Offset: 0x00066068
		private bool IsNPCPositionValid(Vector3 position)
		{
			return !float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z) && !float.IsInfinity(position.x) && !float.IsInfinity(position.y) && !float.IsInfinity(position.z) && position.magnitude <= 10000f;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00067ED8 File Offset: 0x000660D8
		private void EndSetDestination(NPCMovement.WalkResult result)
		{
			if (this.DEBUG)
			{
				Console.Log(this.npc.fullName + " EndSetDestination called: " + result.ToString(), null);
			}
			if (this.walkResultCallback != null)
			{
				this.walkResultCallback(result);
				this.walkResultCallback = null;
			}
			this.hasDestination = false;
			this.CurrentDestination = Vector3.zero;
			this.currentDestination_Reachable = Vector3.zero;
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00067F50 File Offset: 0x00066150
		public void Stop()
		{
			if (this.Agent.isOnNavMesh)
			{
				this.Agent.ResetPath();
				this.Agent.velocity = Vector3.zero;
				this.Agent.isStopped = true;
				this.Agent.isStopped = false;
			}
			if (InstanceFinder.IsServer)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Stopped);
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000045B1 File Offset: 0x000027B1
		public void WarpToNavMesh()
		{
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00067FAC File Offset: 0x000661AC
		public void FacePoint(Vector3 point, float lerpTime = 0.5f)
		{
			Vector3 forward = new Vector3(point.x, base.transform.position.y, point.z) - base.transform.position;
			if (this.FaceDirectionRoutine != null)
			{
				base.StopCoroutine(this.FaceDirectionRoutine);
			}
			if (this.DEBUG)
			{
				string str = "Facing point: ";
				Vector3 vector = point;
				Debug.Log(str + vector.ToString());
			}
			this.FaceDirectionRoutine = base.StartCoroutine(this.FaceDirection_Process(forward, lerpTime));
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0006803C File Offset: 0x0006623C
		public void FaceDirection(Vector3 forward, float lerpTime = 0.5f)
		{
			if (this.FaceDirectionRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.FaceDirectionRoutine);
			}
			if (this.DEBUG)
			{
				string str = "Facing dir: ";
				Vector3 vector = forward;
				Debug.Log(str + vector.ToString());
			}
			this.FaceDirectionRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.FaceDirection_Process(forward, lerpTime));
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0006809F File Offset: 0x0006629F
		protected IEnumerator FaceDirection_Process(Vector3 forward, float lerpTime)
		{
			if (lerpTime > 0f)
			{
				Quaternion startRot = base.transform.rotation;
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					base.transform.rotation = Quaternion.Lerp(startRot, Quaternion.LookRotation(forward, Vector3.up), i / lerpTime);
					yield return new WaitForEndOfFrame();
				}
				startRot = default(Quaternion);
			}
			base.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
			this.FaceDirectionRoutine = null;
			yield break;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000680BC File Offset: 0x000662BC
		public void PauseMovement()
		{
			this.IsPaused = true;
			this.Agent.isStopped = true;
			this.Agent.velocity = Vector3.zero;
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000680E1 File Offset: 0x000662E1
		public void ResumeMovement()
		{
			this.IsPaused = false;
			if (this.Agent.isOnNavMesh)
			{
				this.Agent.isStopped = false;
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00068104 File Offset: 0x00066304
		public bool IsAsCloseAsPossible(Vector3 location, float distanceThreshold = 0.5f)
		{
			Vector3 zero = Vector3.zero;
			return this.GetClosestReachablePoint(location, out zero) && Vector3.Distance(this.FootPosition, zero) < distanceThreshold;
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00068134 File Offset: 0x00066334
		public bool GetClosestReachablePoint(Vector3 targetPosition, out Vector3 closestPoint)
		{
			closestPoint = Vector3.zero;
			bool flag = false;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < NPCMovement.cachedClosestPointKeys.Count; i++)
			{
				if (Vector3.SqrMagnitude(NPCMovement.cachedClosestPointKeys[i] - targetPosition) < 1f)
				{
					vector = NPCMovement.cachedClosestReachablePoints[NPCMovement.cachedClosestPointKeys[i]];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				closestPoint = vector;
				return true;
			}
			if (!this.Agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			float num = 3f;
			for (int j = 0; j < 3; j++)
			{
				NavMeshHit navMeshHit;
				if (NavMeshUtility.SamplePosition(targetPosition, out navMeshHit, num * (float)(j + 1), -1, true) && this.Agent.CalculatePath(navMeshHit.position, navMeshPath))
				{
					Vector3 vector2 = navMeshPath.corners[navMeshPath.corners.Length - 1];
					if (this.Agent.isActiveAndEnabled && this.Agent.isOnNavMesh && Vector3.Distance(navMeshHit.position, vector2) <= 1f)
					{
						closestPoint = vector2;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0006825C File Offset: 0x0006645C
		public bool CanGetTo(Vector3 position, float proximityReq = 1f)
		{
			NavMeshPath navMeshPath = null;
			return this.CanGetTo(position, proximityReq, out navMeshPath);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00068278 File Offset: 0x00066478
		public bool CanGetTo(ITransitEntity entity, float proximityReq = 1f)
		{
			if (entity == null)
			{
				return false;
			}
			foreach (Transform transform in entity.AccessPoints)
			{
				if (!(transform == null) && this.CanGetTo(transform.position, proximityReq))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x000682C0 File Offset: 0x000664C0
		public bool CanGetTo(Vector3 position, float proximityReq, out NavMeshPath path)
		{
			path = null;
			if (Vector3.Distance(position, base.transform.position) <= proximityReq)
			{
				return true;
			}
			if (!this.Agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshHit navMeshHit;
			if (!NavMeshUtility.SamplePosition(position, out navMeshHit, 2f, -1, true))
			{
				return false;
			}
			path = this.GetPathTo(navMeshHit.position, proximityReq);
			if (path == null)
			{
				Debug.DrawLine(this.FootPosition, navMeshHit.position, Color.red, 1f);
				return false;
			}
			if (path.corners.Length < 2)
			{
				Console.LogWarning("Path length < 2", null);
				return false;
			}
			float num = Vector3.Distance(path.corners[path.corners.Length - 1], navMeshHit.position);
			float num2 = Vector3.Distance(navMeshHit.position, position);
			return num <= proximityReq && num2 <= proximityReq;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00068390 File Offset: 0x00066590
		private NavMeshPath GetPathTo(Vector3 position, float proximityReq = 1f)
		{
			if (!this.Agent.isOnNavMesh)
			{
				Console.LogWarning("Agent not on nav mesh!", null);
				return null;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			NavMeshHit navMeshHit;
			NavMeshUtility.SamplePosition(position, out navMeshHit, 2f, -1, true);
			if (!this.Agent.CalculatePath(navMeshHit.position, navMeshPath))
			{
				return null;
			}
			float num = Vector3.Distance(navMeshPath.corners[navMeshPath.corners.Length - 1], navMeshHit.position);
			float num2 = Vector3.Distance(navMeshHit.position, position);
			if (num <= proximityReq && num2 <= proximityReq)
			{
				return navMeshPath;
			}
			return null;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000684F4 File Offset: 0x000666F4
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted = true;
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveWarp_4276783012));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_ActivateRagdoll_Server_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_ActivateRagdoll_2690242654));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyRagdollForce_2690242654));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_DeactivateRagdoll_2166136261));
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00068585 File Offset: 0x00066785
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00068598 File Offset: 0x00066798
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000685A8 File Offset: 0x000667A8
		private void RpcWriter___Observers_ReceiveWarp_4276783012(Vector3 position)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(position);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, true, false);
			writer.Store();
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00068660 File Offset: 0x00066860
		private void RpcLogic___ReceiveWarp_4276783012(Vector3 position)
		{
			if (!this.IsNPCPositionValid(position))
			{
				string str = "NPCMovement.Warp called with invalid position: ";
				Vector3 vector = position;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			this.Agent.Warp(position);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000686A4 File Offset: 0x000668A4
		private void RpcReader___Observers_ReceiveWarp_4276783012(PooledReader PooledReader0, Channel channel)
		{
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveWarp_4276783012(position);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000686D8 File Offset: 0x000668D8
		private void RpcWriter___Server_ActivateRagdoll_Server_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00068772 File Offset: 0x00066972
		public void RpcLogic___ActivateRagdoll_Server_2166136261()
		{
			this.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0006878C File Offset: 0x0006698C
		private void RpcReader___Server_ActivateRagdoll_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ActivateRagdoll_Server_2166136261();
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000687BC File Offset: 0x000669BC
		private void RpcWriter___Observers_ActivateRagdoll_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(forcePoint);
			writer.WriteVector3(forceDir);
			writer.WriteSingle(forceMagnitude, AutoPackType.Unpacked);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00068894 File Offset: 0x00066A94
		public void RpcLogic___ActivateRagdoll_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.anim.SetRagdollActive(true);
			if (this.onRagdollStart != null)
			{
				this.onRagdollStart.Invoke();
			}
			if (InstanceFinder.IsServer)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
				this.Agent.enabled = false;
			}
			this.capsuleCollider.gameObject.SetActive(false);
			if (forceMagnitude > 0f)
			{
				this.ApplyRagdollForce(forcePoint, forceDir, forceMagnitude);
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000688FC File Offset: 0x00066AFC
		private void RpcReader___Observers_ActivateRagdoll_2690242654(PooledReader PooledReader0, Channel channel)
		{
			Vector3 forcePoint = PooledReader0.ReadVector3();
			Vector3 forceDir = PooledReader0.ReadVector3();
			float forceMagnitude = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00068960 File Offset: 0x00066B60
		private void RpcWriter___Observers_ApplyRagdollForce_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(forcePoint);
			writer.WriteVector3(forceDir);
			writer.WriteSingle(forceMagnitude, AutoPackType.Unpacked);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00068A38 File Offset: 0x00066C38
		public void RpcLogic___ApplyRagdollForce_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			(from x in this.npc.Avatar.RagdollRBs
			select new
			{
				rb = x,
				dist = Vector3.Distance(x.transform.position, forcePoint)
			} into x
			orderby x.dist
			select x).First().rb.AddForceAtPosition(forceDir.normalized * forceMagnitude, forcePoint, ForceMode.Impulse);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00068ABC File Offset: 0x00066CBC
		private void RpcReader___Observers_ApplyRagdollForce_2690242654(PooledReader PooledReader0, Channel channel)
		{
			Vector3 forcePoint = PooledReader0.ReadVector3();
			Vector3 forceDir = PooledReader0.ReadVector3();
			float forceMagnitude = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00068B20 File Offset: 0x00066D20
		private void RpcWriter___Observers_DeactivateRagdoll_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00068BCC File Offset: 0x00066DCC
		public void RpcLogic___DeactivateRagdoll_2166136261()
		{
			this.capsuleCollider.gameObject.SetActive(this.npc.isVisible);
			this.anim.SetRagdollActive(false);
			base.transform.position = this.anim.Avatar.transform.position;
			base.transform.rotation = this.anim.Avatar.transform.rotation;
			this.anim.Avatar.transform.localPosition = Vector3.zero;
			this.anim.Avatar.transform.localRotation = Quaternion.identity;
			this.velocityCalculator.FlushBuffer();
			if (InstanceFinder.IsServer)
			{
				this.Agent.enabled = false;
				if (!this.Agent.isOnNavMesh)
				{
					NavMeshQueryFilter navMeshQueryFilter = default(NavMeshQueryFilter);
					navMeshQueryFilter.agentTypeID = NavMeshUtility.GetNavMeshAgentID("Humanoid");
					navMeshQueryFilter.areaMask = -1;
					NavMeshHit navMeshHit;
					if (this.SmartSampleNavMesh(base.transform.position, out navMeshHit, 1f, 10f, 3))
					{
						this.Agent.Warp(navMeshHit.position);
					}
					this.Agent.enabled = false;
					this.Agent.enabled = true;
				}
			}
			if (this.onRagdollEnd != null)
			{
				this.onRagdollEnd.Invoke();
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00068D24 File Offset: 0x00066F24
		private void RpcReader___Observers_DeactivateRagdoll_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DeactivateRagdoll_2166136261();
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00068D50 File Offset: 0x00066F50
		protected virtual void dll()
		{
			this.npc = base.GetComponent<NPC>();
			NPC npc = this.npc;
			npc.onVisibilityChanged = (Action<bool>)Delegate.Combine(npc.onVisibilityChanged, new Action<bool>(this.VisibilityChange));
			this.VisibilityChange(this.npc.isVisible);
			base.InvokeRepeating("UpdateAvoidance", 0f, 0.5f);
			for (int i = 0; i < this.npc.Avatar.RagdollRBs.Length; i++)
			{
				this.ragdollForceComponents.Add(this.npc.Avatar.RagdollRBs[i].gameObject.AddComponent<ConstantForce>());
			}
			this.SetRagdollDraggable(false);
			this.SetGravityMultiplier(1f);
		}

		// Token: 0x04001511 RID: 5393
		public const float VEHICLE_RUNOVER_THRESHOLD = 10f;

		// Token: 0x04001512 RID: 5394
		public const float SKATEBOARD_RUNOVER_THRESHOLD = 10f;

		// Token: 0x04001513 RID: 5395
		public const float LIGHT_FLINCH_THRESHOLD = 50f;

		// Token: 0x04001514 RID: 5396
		public const float HEAVY_FLINCH_THRESHOLD = 100f;

		// Token: 0x04001515 RID: 5397
		public const float RAGDOLL_THRESHOLD = 150f;

		// Token: 0x04001516 RID: 5398
		public const float MOMENTUM_ANNOYED_THRESHOLD = 10f;

		// Token: 0x04001517 RID: 5399
		public const float MOMENTUM_LIGHT_FLINCH_THRESHOLD = 20f;

		// Token: 0x04001518 RID: 5400
		public const float MOMENTUM_HEAVY_FLINCH_THRESHOLD = 40f;

		// Token: 0x04001519 RID: 5401
		public const float MOMENTUM_RAGDOLL_THRESHOLD = 60f;

		// Token: 0x0400151A RID: 5402
		public const bool USE_PATH_CACHE = true;

		// Token: 0x0400151B RID: 5403
		public const float STUMBLE_DURATION = 0.66f;

		// Token: 0x0400151C RID: 5404
		public const float STUMBLE_FORCE = 7f;

		// Token: 0x0400151D RID: 5405
		public const float OBSTACLE_AVOIDANCE_RANGE = 25f;

		// Token: 0x0400151E RID: 5406
		public const float PLAYER_DIST_IMPACT_THRESHOLD = 30f;

		// Token: 0x0400151F RID: 5407
		public static Dictionary<Vector3, Vector3> cachedClosestReachablePoints = new Dictionary<Vector3, Vector3>();

		// Token: 0x04001520 RID: 5408
		public static List<Vector3> cachedClosestPointKeys = new List<Vector3>();

		// Token: 0x04001521 RID: 5409
		public const float CLOSEST_REACHABLE_POINT_CACHE_MAX_SQR_OFFSET = 1f;

		// Token: 0x04001522 RID: 5410
		public bool DEBUG;

		// Token: 0x04001523 RID: 5411
		[Header("Settings")]
		public float WalkSpeed = 1.8f;

		// Token: 0x04001524 RID: 5412
		public float RunSpeed = 7f;

		// Token: 0x04001525 RID: 5413
		public float MoveSpeedMultiplier = 1f;

		// Token: 0x04001526 RID: 5414
		public bool SlipperyMode;

		// Token: 0x04001527 RID: 5415
		public float SlipperyModeMultiplier = 1f;

		// Token: 0x04001528 RID: 5416
		public ObstacleAvoidanceType DefaultObstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

		// Token: 0x04001529 RID: 5417
		[Header("References")]
		public NavMeshAgent Agent;

		// Token: 0x0400152A RID: 5418
		public NPCSpeedController SpeedController;

		// Token: 0x0400152B RID: 5419
		protected NPC npc;

		// Token: 0x0400152C RID: 5420
		public CapsuleCollider capsuleCollider;

		// Token: 0x0400152D RID: 5421
		[SerializeField]
		protected NPCAnimation anim;

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		protected Rigidbody ragdollCentralRB;

		// Token: 0x0400152F RID: 5423
		public SmoothedVelocityCalculator velocityCalculator;

		// Token: 0x04001530 RID: 5424
		[SerializeField]
		protected Draggable RagdollDraggable;

		// Token: 0x04001531 RID: 5425
		[SerializeField]
		protected Collider RagdollDraggableCollider;

		// Token: 0x04001532 RID: 5426
		public float MovementSpeedScale;

		// Token: 0x04001538 RID: 5432
		private float ragdollTime;

		// Token: 0x04001539 RID: 5433
		private float ragdollStaticTime;

		// Token: 0x0400153A RID: 5434
		public UnityEvent<LandVehicle> onHitByCar;

		// Token: 0x0400153B RID: 5435
		public UnityEvent onRagdollStart;

		// Token: 0x0400153C RID: 5436
		public UnityEvent onRagdollEnd;

		// Token: 0x0400153F RID: 5439
		private bool cacheNextPath;

		// Token: 0x04001540 RID: 5440
		private Vector3 currentDestination_Reachable = Vector3.zero;

		// Token: 0x04001541 RID: 5441
		private Action<NPCMovement.WalkResult> walkResultCallback;

		// Token: 0x04001542 RID: 5442
		private float currentMaxDistanceForSuccess = 0.5f;

		// Token: 0x04001543 RID: 5443
		private bool forceIsMoving;

		// Token: 0x04001544 RID: 5444
		private Coroutine FaceDirectionRoutine;

		// Token: 0x04001545 RID: 5445
		private List<ConstantForce> ragdollForceComponents = new List<ConstantForce>();

		// Token: 0x04001547 RID: 5447
		private float timeUntilNextStumble;

		// Token: 0x04001548 RID: 5448
		private float timeSinceStumble = 1000f;

		// Token: 0x04001549 RID: 5449
		private Vector3 stumbleDirection = Vector3.zero;

		// Token: 0x0400154A RID: 5450
		private List<Vector3> desiredVelocityHistory = new List<Vector3>();

		// Token: 0x0400154B RID: 5451
		private int desiredVelocityHistoryLength = 40;

		// Token: 0x0400154C RID: 5452
		private float velocityHistorySpacing = 0.05f;

		// Token: 0x0400154D RID: 5453
		private float timeSinceLastVelocityHistoryRecord;

		// Token: 0x0400154E RID: 5454
		private bool dll_Excuted;

		// Token: 0x0400154F RID: 5455
		private bool dll_Excuted;

		// Token: 0x02000454 RID: 1108
		public enum EAgentType
		{
			// Token: 0x04001551 RID: 5457
			Humanoid,
			// Token: 0x04001552 RID: 5458
			BigHumanoid,
			// Token: 0x04001553 RID: 5459
			IgnoreCosts
		}

		// Token: 0x02000455 RID: 1109
		public enum EStance
		{
			// Token: 0x04001555 RID: 5461
			None,
			// Token: 0x04001556 RID: 5462
			Stanced
		}

		// Token: 0x02000456 RID: 1110
		public enum WalkResult
		{
			// Token: 0x04001558 RID: 5464
			Failed,
			// Token: 0x04001559 RID: 5465
			Interrupted,
			// Token: 0x0400155A RID: 5466
			Stopped,
			// Token: 0x0400155B RID: 5467
			Partial,
			// Token: 0x0400155C RID: 5468
			Success
		}
	}
}
