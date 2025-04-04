using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using Pathfinding;
using ScheduleOne.DevUtilities;
using ScheduleOne.Math;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E7 RID: 2023
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleAgent : MonoBehaviour
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x000E7B12 File Offset: 0x000E5D12
		// (set) Token: 0x060036F7 RID: 14071 RVA: 0x000E7B1A File Offset: 0x000E5D1A
		public bool KinematicMode { get; protected set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000E7B23 File Offset: 0x000E5D23
		// (set) Token: 0x060036F9 RID: 14073 RVA: 0x000E7B2B File Offset: 0x000E5D2B
		public bool AutoDriving { get; protected set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x000E7B34 File Offset: 0x000E5D34
		public bool IsReversing
		{
			get
			{
				return this.reverseCoroutine != null;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060036FB RID: 14075 RVA: 0x000E7B3F File Offset: 0x000E5D3F
		// (set) Token: 0x060036FC RID: 14076 RVA: 0x000E7B47 File Offset: 0x000E5D47
		public Vector3 TargetLocation { get; protected set; } = Vector3.zero;

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x000E7B50 File Offset: 0x000E5D50
		protected float sampleStepSize
		{
			get
			{
				return Mathf.Lerp(this.sampleStepSizeMin, this.sampleStepSizeMax, Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed));
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x000E7B7F File Offset: 0x000E5D7F
		protected float turnSpeedReductionRange
		{
			get
			{
				return Mathf.Lerp(this.turnSpeedReductionMinRange, this.turnSpeedReductionMaxRange, Mathf.Clamp(this.vehicle.speed_Kmh / this.vehicle.TopSpeed, 0f, 1f));
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x000E7BB8 File Offset: 0x000E5DB8
		protected float maxSteerAngle
		{
			get
			{
				return this.vehicle.ActualMaxSteeringAngle;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x000E7BC5 File Offset: 0x000E5DC5
		private Vector3 FrontOfVehiclePosition
		{
			get
			{
				return base.transform.position + base.transform.forward * this.vehicleLength / 2f;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06003701 RID: 14081 RVA: 0x000E7BF7 File Offset: 0x000E5DF7
		public bool NavigationCalculationInProgress
		{
			get
			{
				return this.navigationCalculationRoutine != null;
			}
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000E7C04 File Offset: 0x000E5E04
		private void Awake()
		{
			this.vehicle = base.GetComponent<LandVehicle>();
			this.throttlePID = new PID(0.08f, 0f, 0f);
			this.steerPID = new SteerPID();
			this.speedReductionTracker = new ValueTracker(10f);
			this.PositionHistoryTracker.historyDuration = this.StuckTimeThreshold;
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x000E7C64 File Offset: 0x000E5E64
		protected virtual void Start()
		{
			base.InvokeRepeating("RefreshSpeedZone", 0f, 0.25f);
			base.InvokeRepeating("UpdateStuckDetection", 1f, 1f);
			base.InvokeRepeating("InfrequentUpdate", 0f, 0.033f);
			this.InitializeVehicleData();
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x000E7CB8 File Offset: 0x000E5EB8
		private void InitializeVehicleData()
		{
			this.vehicleLength = this.vehicle.boundingBox.transform.localScale.z;
			this.vehicleWidth = this.vehicle.boundingBox.transform.localScale.x;
			Transform transform = null;
			Transform transform2 = null;
			Transform transform3 = null;
			Transform transform4 = null;
			foreach (Wheel wheel in this.vehicle.wheels)
			{
				if (transform == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).z > this.vehicle.transform.InverseTransformPoint(transform.position).z)
				{
					transform = wheel.transform;
				}
				if (transform2 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).z < this.vehicle.transform.InverseTransformPoint(transform2.position).z)
				{
					transform2 = wheel.transform;
				}
				if (transform4 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).x > this.vehicle.transform.InverseTransformPoint(transform4.position).x)
				{
					transform4 = wheel.transform;
				}
				if (transform3 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).x < this.vehicle.transform.InverseTransformPoint(transform3.position).x)
				{
					transform3 = wheel.transform;
				}
			}
			this.wheelbase = this.vehicle.transform.InverseTransformPoint(transform.position).z - this.vehicle.transform.InverseTransformPoint(transform2.position).z;
			this.wheeltrack = this.vehicle.transform.InverseTransformPoint(transform4.position).x - this.vehicle.transform.InverseTransformPoint(transform3.position).x;
			this.sweepTrack = this.sweepOrigin_FR.localPosition.x - this.sweepOrigin_FL.localPosition.x;
			this.wheelBottomOffset = -base.transform.InverseTransformPoint(this.leftWheel.transform.position).y + this.leftWheel.wheelCollider.radius;
			this.turnRadius = this.wheelbase / Mathf.Sin(this.maxSteerAngle * 0.017453292f) + 1.35f;
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x000E7FA0 File Offset: 0x000E61A0
		protected virtual void FixedUpdate()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			this.timeSinceLastNavigationCall += Time.deltaTime;
			if (!this.AutoDriving)
			{
				return;
			}
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (this.KinematicMode)
			{
				if (num < 40f * QualitySettings.lodBias)
				{
					this.KinematicMode = false;
					this.vehicle.Rb.isKinematic = (this.KinematicMode || !InstanceFinder.IsHost);
					for (int i = 0; i < this.vehicle.wheels.Count; i++)
					{
						this.vehicle.wheels[i].wheelCollider.enabled = !this.vehicle.Rb.isKinematic;
					}
					if (InstanceFinder.IsHost)
					{
						this.vehicle.Rb.velocity = base.transform.forward * this.targetSpeed / 3.6f * 0.5f;
					}
				}
			}
			else if (num > 65f * QualitySettings.lodBias)
			{
				this.KinematicMode = true;
			}
			this.vehicle.Rb.isKinematic = (this.KinematicMode || !InstanceFinder.IsHost);
			for (int j = 0; j < this.vehicle.wheels.Count; j++)
			{
				this.vehicle.wheels[j].wheelCollider.enabled = !this.vehicle.Rb.isKinematic;
			}
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x000E813C File Offset: 0x000E633C
		protected void InfrequentUpdate()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			this.UpdatePursuitMode();
			if (!this.AutoDriving)
			{
				return;
			}
			this.CheckDistanceFromPath();
			this.UpdateOvertaking();
			if (this.reverseCoroutine == null)
			{
				this.UpdateSpeed();
				this.UpdateSteering();
				this.UpdateSweep();
				this.UpdateSpeedReduction();
			}
			if (this.KinematicMode)
			{
				this.UpdateKinematic(0.033f);
			}
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x000E81A4 File Offset: 0x000E63A4
		protected void LateUpdate()
		{
			if (!this.AutoDriving)
			{
				return;
			}
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Target speed: " + this.targetSpeed.ToString());
			}
			this.throttlePID.pFactor = 0.08f;
			this.throttlePID.iFactor = 0f;
			this.throttlePID.dFactor = 0f;
			float num = this.throttlePID.Update(this.targetSpeed, this.vehicle.speed_Kmh, Time.deltaTime);
			float num2 = 0.01f;
			if (Mathf.Abs(num) < num2)
			{
				num = 0f;
			}
			this.vehicle.throttleOverride = Mathf.Clamp(num, this.throttleMin, this.throttleMax);
			this.vehicle.steerOverride = Mathf.Lerp(this.vehicle.steerOverride, this.targetSteerAngle_Normalized, Time.deltaTime * this.steerTargetFollowRate);
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x000E829C File Offset: 0x000E649C
		protected void UpdateKinematic(float deltaTime)
		{
			if (!this.AutoDriving || this.path == null)
			{
				return;
			}
			float distance = this.targetSpeed * 0.2f * deltaTime;
			Vector3 referencePoint = this.vehicle.boundingBox.transform.position - this.vehicle.boundingBox.transform.up * this.vehicle.boundingBoxDimensions.y * 0.5f;
			Vector3 aheadPoint = PathUtility.GetAheadPoint(this.path, referencePoint, distance);
			if (this.DEBUG_MODE)
			{
				Debug.DrawLine(base.transform.position, aheadPoint, Color.red, 0.5f);
			}
			if (aheadPoint == Vector3.zero)
			{
				return;
			}
			base.transform.position = aheadPoint;
			int startPointIndex;
			int num;
			float num2;
			Vector3 vector = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out num2);
			Vector3 origin = vector + Vector3.up * 2f;
			LayerMask mask = LayerMask.GetMask(new string[]
			{
				"Default"
			});
			mask |= LayerMask.GetMask(new string[]
			{
				"Terrain"
			});
			RaycastHit[] array = Physics.RaycastAll(origin, Vector3.down, 3f, mask, QueryTriggerInteraction.Ignore);
			array = (from h in array
			orderby h.distance
			select h).ToArray<RaycastHit>();
			bool flag = false;
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.transform.IsChildOf(base.transform))
				{
					raycastHit = array[i];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				vector = raycastHit.point;
			}
			base.transform.position = vector + base.transform.up * this.wheelBottomOffset;
			Vector3 a = Vector3.zero;
			int num3 = 3;
			for (int j = 0; j < num3; j++)
			{
				a += PathUtility.GetAheadPoint(this.path, base.transform.position, this.vehicleLength / 2f + 1f * (float)(j + 1), startPointIndex, (float)num);
			}
			a /= (float)num3;
			Vector3 normalized = (a - vector).normalized;
			base.transform.rotation = Quaternion.LookRotation(normalized, Vector3.up);
			Vector3 axleGroundHit = this.GetAxleGroundHit(true);
			Vector3 axleGroundHit2 = this.GetAxleGroundHit(false);
			normalized = (axleGroundHit - axleGroundHit2).normalized;
			base.transform.forward = normalized;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000E8558 File Offset: 0x000E6758
		private Vector3 GetAxleGroundHit(bool front)
		{
			Vector3 origin = this.FrontAxlePosition.position + Vector3.up * 1f;
			if (!front)
			{
				origin = this.RearAxlePosition.position + Vector3.up * 1f;
			}
			LayerMask mask = LayerMask.GetMask(new string[]
			{
				"Default"
			});
			mask |= LayerMask.GetMask(new string[]
			{
				"Terrain"
			});
			RaycastHit[] array = Physics.RaycastAll(origin, Vector3.down, 2f, mask, QueryTriggerInteraction.Ignore);
			array = (from h in array
			orderby h.distance
			select h).ToArray<RaycastHit>();
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.transform.IsChildOf(base.transform))
				{
					return array[i].point;
				}
			}
			if (front)
			{
				return this.FrontAxlePosition.position - base.transform.up * this.wheelBottomOffset;
			}
			return this.RearAxlePosition.position - base.transform.up * this.wheelBottomOffset;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000E86B0 File Offset: 0x000E68B0
		private void UpdateSweep()
		{
			if (this.KinematicMode)
			{
				return;
			}
			if (Mathf.Abs(this.vehicle.speed_Kmh) > 5f)
			{
				this.sweepTestFailedTime = 0f;
				return;
			}
			if (Mathf.Abs(this.targetSteerAngle_Normalized) * this.maxSteerAngle > 5f)
			{
				float num = 1.5f;
				float num2;
				Vector3 vector;
				bool flag = this.SweepTurn(VehicleAgent.ESweepType.FR, Mathf.Sign(this.targetSteerAngle_Normalized) * 30f, false, out num2, out vector, this.targetSteerAngle_Normalized * this.maxSteerAngle);
				float num3;
				Vector3 vector2;
				bool flag2 = this.SweepTurn(VehicleAgent.ESweepType.FL, Mathf.Sign(this.targetSteerAngle_Normalized) * 30f, false, out num3, out vector2, this.targetSteerAngle_Normalized * this.maxSteerAngle);
				if ((!flag || num2 >= num) && (!flag2 || num3 >= num))
				{
					this.sweepTestFailedTime = 0f;
					return;
				}
				this.sweepTestFailedTime += Time.deltaTime;
				if ((double)this.sweepTestFailedTime > 0.25)
				{
					this.StartReverse();
					this.sweepTestFailedTime = 0f;
					return;
				}
			}
			else
			{
				this.sweepTestFailedTime = 0f;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x000E87BC File Offset: 0x000E69BC
		private void UpdateSpeedReduction()
		{
			if (this.path == null)
			{
				return;
			}
			if (this.path != null && Vector3.Distance(base.transform.position, this.path.vectorPath[this.path.vectorPath.Count - 1]) < 3f)
			{
				this.path = null;
				this.vehicle.overrideControls = false;
				this.vehicle.steerOverride = 0f;
				this.vehicle.throttleOverride = 0f;
				this.AutoDriving = false;
				this.vehicle.ResetMaxSteerAngle();
				if (this.storedNavigationCallback != null)
				{
					this.storedNavigationCallback(VehicleAgent.ENavigationResult.Complete);
					this.storedNavigationCallback = null;
				}
				return;
			}
			if (this.KinematicMode)
			{
				return;
			}
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			float num2 = 1f;
			float num3 = 1f;
			float num4 = this.targetSpeed;
			if (this.Flags.TurnBasedSpeedReduction)
			{
				float num5 = Mathf.Max(PathUtility.CalculateAngleChangeOverPath(this.path, startPointIndex, pointLerp, this.turnSpeedReductionRange), this.targetSteerAngle_Normalized * this.maxSteerAngle);
				if (num5 > this.minTurnSpeedReductionAngleThreshold)
				{
					num4 = Mathf.Lerp(num4, this.minTurningSpeed, Mathf.Clamp(num5 / this.turnSpeedReductionDivisor, 0f, 1f));
				}
			}
			if (this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreAll)
			{
				float a;
				Vector3 vector;
				this.BetterSweepTurn(VehicleAgent.ESweepType.FL, this.vehicle.SyncAccessor_currentSteerAngle, false, this.sensor_FM.checkMask, out a, out vector);
				float b;
				Vector3 vector2;
				this.BetterSweepTurn(VehicleAgent.ESweepType.FR, this.vehicle.SyncAccessor_currentSteerAngle, false, this.sensor_FM.checkMask, out b, out vector2);
				float num6 = Mathf.Min(a, b);
				float num7 = Mathf.Lerp(1.5f, 15f, Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed));
				if (num6 < num7)
				{
					if (this.DEBUG_MODE)
					{
						Console.Log("Obstacle detected at " + num6.ToString() + "m:", null);
					}
					num2 = Mathf.Clamp((num6 - 1.5f) / (num7 - 1.5f), 0.002f, 1f);
				}
			}
			if (this.Flags.AutoBrakeAtDestination && this.path != null)
			{
				float num8 = Vector3.Distance(base.transform.position, this.path.vectorPath[this.path.vectorPath.Count - 1]);
				if (num8 < 8f)
				{
					num3 = Mathf.Clamp(num8 / 8f, 0f, 1f);
					if (num8 < 3f)
					{
						num3 = 0f;
					}
					if (num3 < 0.2f)
					{
						this.vehicle.ApplyHandbrake();
					}
				}
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Obstacle speed multiplier: " + num2.ToString());
				Debug.Log("Destination speed multiplier: " + num3.ToString());
				Debug.Log("Turn target speed: " + num4.ToString());
			}
			float num9 = num2 * num3;
			this.speedReductionTracker.SubmitValue(num9);
			this.targetSpeed *= num9;
			this.targetSpeed = Mathf.Min(this.targetSpeed, num4);
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x000E8B00 File Offset: 0x000E6D00
		private void UpdatePursuitMode()
		{
			if (!this.PursuitModeEnabled || this.PursuitTarget == null)
			{
				return;
			}
			if (Vector3.Distance(this.PursuitTarget.position, this.PursuitTargetLastPosition) > this.PursuitDistanceUpdateThreshold)
			{
				this.PursuitTargetLastPosition = this.PursuitTarget.position;
				this.Navigate(this.PursuitTarget.position, null, null);
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000E8B68 File Offset: 0x000E6D68
		private void UpdateStuckDetection()
		{
			if (!this.AutoDriving)
			{
				this.PositionHistoryTracker.ClearHistory();
				return;
			}
			if (!this.Flags.StuckDetection)
			{
				return;
			}
			if (this.speedReductionTracker.RecordedHistoryLength() < this.StuckTimeThreshold)
			{
				return;
			}
			if (this.speedReductionTracker.GetLowestValue() < 0.1f)
			{
				return;
			}
			if (this.PositionHistoryTracker.RecordedTime >= this.StuckTimeThreshold)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.StuckSamples; i++)
				{
					vector += this.PositionHistoryTracker.GetPositionXSecondsAgo(this.StuckTimeThreshold / (float)this.StuckSamples * (float)(i + 1));
				}
				vector /= (float)this.StuckSamples;
				if (Vector3.Distance(base.transform.position, vector) < this.StuckDistanceThreshold)
				{
					if (this.DEBUG_MODE)
					{
						Console.LogWarning("Vehicle stuck", null);
					}
					if (this.IsOnVehicleGraph())
					{
						this.Teleporter.MoveToRoadNetwork(true);
					}
					else
					{
						this.Teleporter.MoveToGraph(true);
					}
					this.PositionHistoryTracker.ClearHistory();
				}
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000E8C78 File Offset: 0x000E6E78
		private void CheckDistanceFromPath()
		{
			if (this.timeSinceLastNavigationCall < VehicleAgent.MinRenavigationRate)
			{
				return;
			}
			if (this.KinematicMode)
			{
				return;
			}
			if (this.path != null)
			{
				int num;
				int num2;
				float num3;
				Vector3 vector = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out num, out num2, out num3);
				vector += this.GetPathLateralDirection() * this.lateralOffset;
				if (Vector3.Distance(base.transform.position, vector) > (this.IsReversing ? 8f : 6f))
				{
					if (this.DEBUG_MODE)
					{
						Console.Log("Too far from path! Re-navigating.", null);
						Debug.DrawLine(base.transform.position, vector, Color.red, 2f);
					}
					this.Navigate(this.TargetLocation, this.currentNavigationSettings, this.storedNavigationCallback);
				}
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000E8D4C File Offset: 0x000E6F4C
		private void UpdateOvertaking()
		{
			this.lateralOffset = 0f;
			if (this.sensor_FM.obstruction != null && this.sensor_FM.obstruction.GetComponentInParent<LandVehicle>() != null && this.sensor_FM.obstructionDistance < 8f)
			{
				float num = this.sensor_FM.obstructionDistance / 8f;
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000E8DB4 File Offset: 0x000E6FB4
		protected virtual void RefreshSpeedZone()
		{
			List<SpeedZone> speedZones = SpeedZone.GetSpeedZones(base.transform.position);
			if (speedZones.Count > 0)
			{
				this.currentSpeedZone = speedZones[0];
				return;
			}
			this.currentSpeedZone = null;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x000E8DF0 File Offset: 0x000E6FF0
		protected virtual void UpdateSpeed()
		{
			if (this.path == null)
			{
				this.targetSpeed = 0f;
				return;
			}
			if (this.currentSpeedZone != null)
			{
				this.targetSpeed = this.currentSpeedZone.speed * this.Flags.SpeedLimitMultiplier;
			}
			else
			{
				this.targetSpeed = VehicleAgent.UnmarkedSpeed * this.Flags.SpeedLimitMultiplier;
			}
			if (this.Flags.OverrideSpeed)
			{
				this.targetSpeed = this.Flags.OverriddenSpeed;
			}
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000E8E74 File Offset: 0x000E7074
		protected void UpdateSteering()
		{
			if (this.path == null || this.path.vectorPath.Count < 2 || this.KinematicMode)
			{
				this.targetSteerAngle_Normalized = 0f;
				return;
			}
			int num;
			int num2;
			float num3;
			Vector3 closestPointOnPath = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out num, out num2, out num3);
			Vector3 vector = PathUtility.GetAheadPoint(this.path, base.transform.position, this.vehicleLength / 2f + this.sampleStepSize);
			vector = closestPointOnPath;
			Vector3 vector2 = PathUtility.GetAverageAheadPoint(this.path, base.transform.position, this.aheadPointSamples, this.sampleStepSize);
			vector2 = PathUtility.GetAheadPoint(this.path, base.transform.position, 0.5f);
			Vector3 normalized = (vector2 - vector).normalized;
			Debug.DrawLine(base.transform.position, vector, Color.yellow, 0.5f);
			Debug.DrawLine(base.transform.position, vector2, Color.magenta, 0.5f);
			float error = PathUtility.CalculateCTE(this.CTE_Origin.position + base.transform.forward * Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed) * (this.vehicle.TopSpeed * 0.2778f * 0.3f), base.transform, vector, vector2, this.path);
			float num4 = Mathf.Clamp(this.steerPID.GetNewValue(error, new PID_Parameters(40f, 5f, 10f)) / this.maxSteerAngle, -1f, 1f);
			float num5 = Vector3.SignedAngle(base.transform.forward, normalized, Vector3.up);
			float num6 = 45f;
			if (Mathf.Abs(num5) > 45f)
			{
				num4 += Mathf.Clamp01(Mathf.Abs(num5 - num6) / (180f - num6)) * Mathf.Sign(num5);
			}
			this.targetSteerAngle_Normalized = Mathf.Clamp(num4, -1f, 1f);
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000E9094 File Offset: 0x000E7294
		public void Navigate(Vector3 location, NavigationSettings settings = null, VehicleAgent.NavigationCallback callback = null)
		{
			if (this.navigationCalculationRoutine != null)
			{
				Console.LogWarning("Navigate called before previous navigation calculation was complete!", null);
				base.StopCoroutine(this.navigationCalculationRoutine);
			}
			if (this.GetIsStuck())
			{
				Console.LogWarning("Navigate called but vehilc is stuck! Navigation will still be attemped", null);
			}
			if (this.reverseCoroutine != null)
			{
				this.StopReversing();
			}
			if (!InstanceFinder.IsHost)
			{
				return;
			}
			this.path = null;
			this.timeSinceLastNavigationCall = 0f;
			if (settings == null)
			{
				settings = new NavigationSettings();
			}
			if (this.GetDistanceFromVehicleGraph() > 6f)
			{
				if (settings.ensureProximityToGraph)
				{
					this.Teleporter.MoveToGraph(true);
				}
				else if (callback != null)
				{
					callback(VehicleAgent.ENavigationResult.Failed);
					return;
				}
			}
			this.vehicle.Rb.isKinematic = this.KinematicMode;
			this.vehicle.Rb.interpolation = RigidbodyInterpolation.Interpolate;
			if (this.DEBUG_MODE)
			{
				Console.Log("Navigate called...", null);
			}
			this.TargetLocation = location;
			this.AutoDriving = true;
			this.storedNavigationCallback = callback;
			this.vehicle.OverrideMaxSteerAngle(35f);
			this.vehicle.overrideControls = true;
			this.currentNavigationSettings = settings;
			this.navigationCalculationRoutine = NavigationUtility.CalculatePath(this.FrontOfVehiclePosition, this.TargetLocation, this.currentNavigationSettings, this.Flags, this.generalSeeker, this.roadSeeker, new NavigationUtility.NavigationCalculationCallback(this.NavigationCalculationCallback));
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000E91E0 File Offset: 0x000E73E0
		private void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath _path)
		{
			this.navigationCalculationRoutine = null;
			if (result == NavigationUtility.ENavigationCalculationResult.Failed)
			{
				if (this.storedNavigationCallback != null)
				{
					this.storedNavigationCallback(VehicleAgent.ENavigationResult.Failed);
				}
				this.EndDriving();
				return;
			}
			this.path = _path;
			this.path.InitializePath();
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000E921A File Offset: 0x000E741A
		private void EndDriving()
		{
			this.AutoDriving = false;
			this.vehicle.ResetMaxSteerAngle();
			this.path = null;
			this.storedNavigationCallback = null;
			this.vehicle.overrideControls = false;
			this.currentNavigationSettings = null;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000E924F File Offset: 0x000E744F
		public void StopNavigating()
		{
			if (this.navigationCalculationRoutine != null)
			{
				base.StopCoroutine(this.navigationCalculationRoutine);
			}
			if (this.storedNavigationCallback != null)
			{
				this.storedNavigationCallback(VehicleAgent.ENavigationResult.Stopped);
			}
			this.EndDriving();
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000E927F File Offset: 0x000E747F
		public void RecalculateNavigation()
		{
			if (!this.AutoDriving)
			{
				return;
			}
			this.Navigate(this.TargetLocation, this.currentNavigationSettings, this.storedNavigationCallback);
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000E92A4 File Offset: 0x000E74A4
		public bool SweepTurn(VehicleAgent.ESweepType sweep, float sweepAngle, bool reverse, out float hitDistance, out Vector3 hitPoint, float steerAngle = 0f)
		{
			hitDistance = float.MaxValue;
			hitPoint = Vector3.zero;
			if (steerAngle == 0f)
			{
				steerAngle = this.maxSteerAngle;
			}
			steerAngle = Mathf.Abs(steerAngle);
			float num = Mathf.Sign(sweepAngle);
			this.FrontAxlePosition.localEulerAngles = new Vector3(0f, num * steerAngle, 0f);
			float num2 = this.turnRadius;
			Vector3 vector = Vector3.zero;
			Vector3 castStart = Vector3.zero;
			if (sweepAngle > 0f)
			{
				vector = this.sweepOrigin_FL.position + this.FrontAxlePosition.right * this.turnRadius;
			}
			else
			{
				vector = this.sweepOrigin_FR.position - this.FrontAxlePosition.right * this.turnRadius;
			}
			switch (sweep)
			{
			case VehicleAgent.ESweepType.FL:
				castStart = this.sweepOrigin_FL.position;
				break;
			case VehicleAgent.ESweepType.FR:
				castStart = this.sweepOrigin_FR.position;
				break;
			case VehicleAgent.ESweepType.RL:
				castStart = this.sweepOrigin_RL.position;
				break;
			case VehicleAgent.ESweepType.RR:
				castStart = this.sweepOrigin_RR.position;
				break;
			}
			Vector3 normalized = (castStart - vector).normalized;
			Vector3 a = Quaternion.AngleAxis(90f * num, base.transform.up) * normalized;
			num2 = Vector3.Distance(vector, castStart);
			float num3 = 0f;
			float num4 = 0f;
			Func<RaycastHit, float> <>9__0;
			RaycastHit raycastHit;
			for (;;)
			{
				float num5 = num3;
				float num6 = Mathf.Clamp(num5 + Mathf.Abs(15f), 0f, Mathf.Abs(sweepAngle));
				num3 += num6 - num5;
				float d = num2 * Mathf.Cos(num6 * 0.017453292f);
				float d2 = num2 * Mathf.Sin(num6 * 0.017453292f);
				Vector3 vector2 = vector;
				vector2 += a * d2 * (reverse ? -1f : 1f);
				vector2 += normalized * d;
				RaycastHit[] array = Physics.SphereCastAll(castStart, 0.1f, (vector2 - castStart).normalized, Vector3.Distance(castStart, vector2), this.sweepMask, QueryTriggerInteraction.Ignore);
				if (array.Length != 0)
				{
					IEnumerable<RaycastHit> source = array;
					Func<RaycastHit, float> keySelector;
					if ((keySelector = <>9__0) == null)
					{
						keySelector = (<>9__0 = ((RaycastHit x) => Vector3.Distance(castStart, x.point)));
					}
					array = source.OrderBy(keySelector).ToArray<RaycastHit>();
				}
				raycastHit = default(RaycastHit);
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].collider.transform.IsChildOf(base.transform))
					{
						raycastHit = array[i];
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num4 += Vector3.Distance(castStart, vector2);
				castStart = vector2;
				if (num3 >= Mathf.Abs(sweepAngle))
				{
					return false;
				}
			}
			if (raycastHit.point == Vector3.zero)
			{
				raycastHit.point = castStart;
			}
			num4 += Vector3.Distance(castStart, raycastHit.point);
			hitDistance = num4;
			hitPoint = raycastHit.point;
			return true;
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000E95F4 File Offset: 0x000E77F4
		public void BetterSweepTurn(VehicleAgent.ESweepType sweep, float steerAngle, bool reverse, LayerMask mask, out float hitDistance, out Vector3 hitPoint)
		{
			hitDistance = float.MaxValue;
			hitPoint = Vector3.zero;
			float num = Mathf.Sign(steerAngle);
			this.FrontAxlePosition.localEulerAngles = new Vector3(0f, steerAngle, 0f);
			Vector3 vector = Vector3.zero;
			Vector3 castStart = Vector3.zero;
			float num2 = Mathf.Clamp(this.wheelbase / Mathf.Sin(steerAngle * 0.017453292f), -100f, 100f);
			vector = this.sweepOrigin_FL.position + this.FrontAxlePosition.right * num2;
			switch (sweep)
			{
			case VehicleAgent.ESweepType.FL:
				castStart = this.sweepOrigin_FL.position;
				break;
			case VehicleAgent.ESweepType.FR:
				castStart = this.sweepOrigin_FR.position;
				break;
			case VehicleAgent.ESweepType.RL:
				castStart = this.sweepOrigin_RL.position;
				break;
			case VehicleAgent.ESweepType.RR:
				castStart = this.sweepOrigin_RR.position;
				break;
			default:
				Console.LogWarning("Invalid sweep type: " + sweep.ToString(), null);
				break;
			}
			Debug.DrawLine(castStart, vector, Color.white);
			Vector3 normalized = (castStart - vector).normalized;
			Vector3 a = Quaternion.AngleAxis(90f * num, base.transform.up) * normalized;
			num2 = Vector3.Distance(vector, castStart);
			float num3 = 0f;
			int num4 = 6;
			float num5 = Mathf.Clamp(Mathf.Abs(steerAngle), 5f, 30f);
			Func<RaycastHit, float> <>9__0;
			for (float num6 = 0f; num6 < (float)num4; num6 += 1f)
			{
				float num7 = num5 * (num6 + 1f);
				float d = num2 * Mathf.Cos(num7 * 0.017453292f);
				float d2 = num2 * Mathf.Sin(num7 * 0.017453292f);
				Vector3 vector2 = vector;
				vector2 += a * d2 * (reverse ? -1f : 1f);
				vector2 += normalized * d;
				RaycastHit[] array = Physics.SphereCastAll(castStart, this.sensor_FM.checkRadius, (vector2 - castStart).normalized, Vector3.Distance(castStart, vector2), mask);
				if (array.Length != 0)
				{
					IEnumerable<RaycastHit> source = array;
					Func<RaycastHit, float> keySelector;
					if ((keySelector = <>9__0) == null)
					{
						keySelector = (<>9__0 = ((RaycastHit x) => Vector3.Distance(castStart, x.point)));
					}
					array = source.OrderBy(keySelector).ToArray<RaycastHit>();
				}
				RaycastHit raycastHit = default(RaycastHit);
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].collider.transform.IsChildOf(base.transform) && !array[i].collider.transform.IsChildOf(this.vehicle.HumanoidColliderContainer.transform.transform))
					{
						if (this.Flags.IgnoreTrafficLights)
						{
							VehicleObstacle componentInParent = array[i].transform.GetComponentInParent<VehicleObstacle>();
							if (componentInParent != null && componentInParent.type == VehicleObstacle.EObstacleType.TrafficLight)
							{
								goto IL_3EF;
							}
						}
						VehicleObstacle componentInParent2 = array[i].collider.transform.GetComponentInParent<VehicleObstacle>();
						if (componentInParent2 != null)
						{
							if (!componentInParent2.twoSided && Vector3.Angle(-componentInParent2.transform.forward, base.transform.forward) > 90f)
							{
								goto IL_3EF;
							}
						}
						else if (array[i].collider.isTrigger)
						{
							goto IL_3EF;
						}
						if (this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreOnlySquishy || (!(array[i].transform.GetComponentInParent<LandVehicle>() != null) && !(array[i].transform.GetComponentInParent<Player>() != null) && !(array[i].transform.GetComponentInParent<NPC>() != null)))
						{
							raycastHit = array[i];
							flag = true;
							break;
						}
					}
					IL_3EF:;
				}
				if (flag)
				{
					if (raycastHit.point == Vector3.zero)
					{
						raycastHit.point = castStart;
					}
					num3 += Vector3.Distance(castStart, raycastHit.point);
					hitDistance = num3;
					hitPoint = raycastHit.point;
					Debug.DrawLine(castStart, raycastHit.point, Color.red);
					return;
				}
				num3 += Vector3.Distance(castStart, vector2);
				Debug.DrawLine(castStart, vector2, (num6 % 2f == 0f) ? Color.green : Color.cyan);
				castStart = vector2;
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000E9ABD File Offset: 0x000E7CBD
		public void StartReverse()
		{
			if (this.reverseCoroutine != null)
			{
				this.StopReversing();
			}
			this.reverseCoroutine = base.StartCoroutine(this.Reverse());
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000E9ADF File Offset: 0x000E7CDF
		public IEnumerator Reverse()
		{
			if (this.DEBUG_MODE)
			{
				Console.Log("Starting reverse operation", null);
			}
			this.targetSpeed = 0f;
			this.targetSteerAngle_Normalized = 0f;
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			float num2 = 3f;
			Vector3 futureTarget = Vector3.zero;
			Vector3 zero = Vector3.zero;
			int num3 = 0;
			Vector3 vector;
			Vector3 a;
			for (;;)
			{
				vector = Vector3.zero;
				a = Vector3.zero;
				for (int i = 1; i <= this.aheadPointSamples; i++)
				{
					vector += PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, num2 + (float)i * this.sampleStepSizeMin);
					if (i == this.aheadPointSamples)
					{
						a = PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, num2 + (float)i * this.sampleStepSizeMin + 1f);
					}
				}
				vector /= (float)this.aheadPointSamples;
				if (Mathf.Abs(base.transform.InverseTransformPoint(vector).x) > 1f)
				{
					break;
				}
				if (num3 >= 25)
				{
					goto Block_5;
				}
				num3++;
				num2 += 1f;
			}
			futureTarget = vector;
			a - futureTarget;
			float steerAngleNormal = -Mathf.Sign(base.transform.InverseTransformPoint(futureTarget).x);
			yield return new WaitForSeconds(1f);
			VehicleAgent.ESweepType frontWheel = VehicleAgent.ESweepType.FL;
			if (steerAngleNormal < 0f)
			{
				frontWheel = VehicleAgent.ESweepType.FR;
			}
			float num4 = 10f;
			float num5 = 90f;
			Vector3 vector2 = futureTarget - base.transform.position;
			vector2.y = 0f;
			vector2.Normalize();
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			float sweepAngle = num4 + (num5 - num4) * Mathf.Clamp(Vector3.Angle(forward, vector2) / 90f, 0f, 1f);
			if (this.DEBUG_MODE)
			{
				Console.Log("Beginning straight reverse...", null);
			}
			float reverseSweepDistanceMin = 1.25f;
			this.targetSpeed = (this.Flags.OverrideSpeed ? (-this.Flags.OverriddenReverseSpeed) : (-VehicleAgent.ReverseSpeed));
			bool canBeginSwing = false;
			while (!canBeginSwing)
			{
				yield return new WaitForEndOfFrame();
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				Vector3 zero2 = Vector3.zero;
				Vector3 zero3 = Vector3.zero;
				if (this.SweepTurn(frontWheel, sweepAngle * steerAngleNormal, true, out num6, out zero2, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RL, sweepAngle * steerAngleNormal, true, out num7, out zero3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RR, sweepAngle * steerAngleNormal, true, out num8, out zero3, 0f))
				{
					float num9 = 2f;
					if (this.sensor_RR.obstructionDistance < num9 || this.sensor_RL.obstructionDistance < num9)
					{
						if (this.DEBUG_MODE)
						{
							Console.Log("Continued straight reversing will result in collision; starting turn", null);
						}
						canBeginSwing = true;
					}
				}
				else if (base.transform.InverseTransformPoint(futureTarget).z > -this.vehicleLength)
				{
					canBeginSwing = true;
				}
			}
			if (this.DEBUG_MODE)
			{
				Console.Log("Beginning swing...", null);
			}
			this.targetSteerAngle_Normalized = steerAngleNormal;
			Vector3 faceTarget = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			Vector3 normalized = (PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, 0.5f) - faceTarget).normalized;
			faceTarget += normalized * this.vehicleLength / 2f;
			bool continueReversing = true;
			while (continueReversing)
			{
				yield return new WaitForEndOfFrame();
				if (this.path == null)
				{
					continueReversing = false;
				}
				else
				{
					vector2 = faceTarget - base.transform.position;
					vector2.y = 0f;
					vector2.Normalize();
					forward = base.transform.forward;
					forward.y = 0f;
					Debug.DrawLine(base.transform.position, faceTarget, Color.magenta);
					Debug.DrawLine(base.transform.position, base.transform.position + forward * 5f, Color.cyan);
					if (Vector3.Angle(vector2, forward) < 20f)
					{
						continueReversing = false;
					}
					float maxValue = float.MaxValue;
					float maxValue2 = float.MaxValue;
					float maxValue3 = float.MaxValue;
					Vector3 vector3;
					if ((this.SweepTurn(frontWheel, 30f * steerAngleNormal, true, out maxValue, out vector3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RL, 30f * steerAngleNormal, true, out maxValue2, out vector3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RR, 30f * steerAngleNormal, true, out maxValue3, out vector3, 0f)) && (maxValue < reverseSweepDistanceMin || maxValue2 < reverseSweepDistanceMin || maxValue3 < reverseSweepDistanceMin))
					{
						continueReversing = false;
						if (this.DEBUG_MODE)
						{
							Console.Log("Reverse sweep obstructed", null);
						}
					}
				}
			}
			this.targetSpeed = 0f;
			yield return new WaitUntil(() => this.vehicle.speed_Kmh >= -1f);
			if (this.DEBUG_MODE)
			{
				Console.Log("Reverse finished", null);
			}
			this.reverseCoroutine = null;
			yield break;
			Block_5:
			this.reverseCoroutine = null;
			Console.LogWarning("Can't calculate average ahead point!", null);
			yield break;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000E9AEE File Offset: 0x000E7CEE
		private void StopReversing()
		{
			if (this.DEBUG_MODE)
			{
				Console.Log("Reverse stop", null);
			}
			if (this.reverseCoroutine != null)
			{
				base.StopCoroutine(this.reverseCoroutine);
				this.reverseCoroutine = null;
				this.targetSpeed = 0f;
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000E9B2C File Offset: 0x000E7D2C
		private Collider GetClosestForwardObstruction(out float obstructionDist)
		{
			Collider result = null;
			obstructionDist = float.MaxValue;
			foreach (Sensor sensor in new List<Sensor>
			{
				this.sensor_FL,
				this.sensor_FM,
				this.sensor_FR
			})
			{
				if (sensor.obstruction != null)
				{
					if (this.Flags.IgnoreTrafficLights)
					{
						VehicleObstacle componentInParent = sensor.obstruction.GetComponentInParent<VehicleObstacle>();
						if (componentInParent != null && componentInParent.type == VehicleObstacle.EObstacleType.TrafficLight)
						{
							continue;
						}
					}
					if ((this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreOnlySquishy || (!(sensor.obstruction.GetComponentInParent<LandVehicle>() != null) && !(sensor.obstruction.GetComponentInParent<Player>() != null) && !(sensor.obstruction.GetComponentInParent<NPC>() != null))) && sensor.obstructionDistance < obstructionDist)
					{
						result = sensor.obstruction;
						obstructionDist = sensor.obstructionDistance;
					}
				}
			}
			return result;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000E9C48 File Offset: 0x000E7E48
		public bool IsOnVehicleGraph()
		{
			return this.GetDistanceFromVehicleGraph() < 2.5f;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000E9C58 File Offset: 0x000E7E58
		private float GetDistanceFromVehicleGraph()
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			return Vector3.Distance(AstarPath.active.GetNearest(base.transform.position, nnconstraint).position, base.transform.position - base.transform.up * this.vehicle.boundingBoxDimensions.y / 2f);
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000E9CD8 File Offset: 0x000E7ED8
		private Vector3 GetPathLateralDirection()
		{
			if (this.path == null)
			{
				Console.LogWarning("Path is null!", null);
				return Vector3.zero;
			}
			int startPointIndex;
			int num;
			float pointLerp;
			Vector3 closestPointOnPath = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			Vector3 pointAheadOfPathPoint = PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, 0.01f);
			return Quaternion.AngleAxis(90f, base.transform.up) * (pointAheadOfPathPoint - closestPointOnPath).normalized;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000E9D5C File Offset: 0x000E7F5C
		public bool GetIsStuck()
		{
			if (this.speedReductionTracker.RecordedHistoryLength() < this.StuckTimeThreshold)
			{
				return false;
			}
			if (this.speedReductionTracker.GetLowestValue() < 0.1f)
			{
				return false;
			}
			if (this.PositionHistoryTracker.RecordedTime >= this.StuckTimeThreshold)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.StuckSamples; i++)
				{
					vector += this.PositionHistoryTracker.GetPositionXSecondsAgo(this.StuckTimeThreshold / (float)this.StuckSamples * (float)(i + 1));
				}
				vector /= (float)this.StuckSamples;
				if (Vector3.Distance(base.transform.position, vector) < this.StuckDistanceThreshold)
				{
					if (this.DEBUG_MODE)
					{
						Console.LogWarning("Vehicle stuck", null);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x040027F3 RID: 10227
		public const string VehicleGraphName = "General Vehicle Graph";

		// Token: 0x040027F4 RID: 10228
		public const string RoadGraphName = "Road Nodes";

		// Token: 0x040027F5 RID: 10229
		public const float MaxDistanceFromPath = 6f;

		// Token: 0x040027F6 RID: 10230
		public const float MaxDistanceFromPathWhenReversing = 8f;

		// Token: 0x040027F7 RID: 10231
		public static Vector3 MainGraphSamplePoint = new Vector3(31.5f, 0f, 51f);

		// Token: 0x040027F8 RID: 10232
		public static float MinRenavigationRate = 2f;

		// Token: 0x040027F9 RID: 10233
		public const float Steer_P = 40f;

		// Token: 0x040027FA RID: 10234
		public const float Steer_I = 5f;

		// Token: 0x040027FB RID: 10235
		public const float Steer_D = 10f;

		// Token: 0x040027FC RID: 10236
		public const float Throttle_P = 0.08f;

		// Token: 0x040027FD RID: 10237
		public const float Throttle_I = 0f;

		// Token: 0x040027FE RID: 10238
		public const float Throttle_D = 0f;

		// Token: 0x040027FF RID: 10239
		public const float Steer_Rate = 135f;

		// Token: 0x04002800 RID: 10240
		public const float MaxAxlePositionShift = 3f;

		// Token: 0x04002801 RID: 10241
		public const float OBSTACLE_MIN_RANGE = 1.5f;

		// Token: 0x04002802 RID: 10242
		public const float OBSTACLE_MAX_RANGE = 15f;

		// Token: 0x04002803 RID: 10243
		public const float MAX_STEER_ANGLE_OVERRIDE = 35f;

		// Token: 0x04002804 RID: 10244
		public const float KINEMATIC_MODE_MIN_DISTANCE = 40f;

		// Token: 0x04002805 RID: 10245
		public const float INFREQUENT_UPDATE_RATE = 0.033f;

		// Token: 0x04002806 RID: 10246
		public bool DEBUG_MODE;

		// Token: 0x0400280A RID: 10250
		public DriveFlags Flags;

		// Token: 0x0400280B RID: 10251
		[Header("Seekers")]
		[SerializeField]
		protected Seeker roadSeeker;

		// Token: 0x0400280C RID: 10252
		[SerializeField]
		protected Seeker generalSeeker;

		// Token: 0x0400280D RID: 10253
		[Header("References")]
		[SerializeField]
		protected Transform CTE_Origin;

		// Token: 0x0400280E RID: 10254
		[SerializeField]
		protected Transform FrontAxlePosition;

		// Token: 0x0400280F RID: 10255
		[SerializeField]
		protected Transform RearAxlePosition;

		// Token: 0x04002810 RID: 10256
		[Header("Sensors")]
		[SerializeField]
		protected Sensor sensor_FL;

		// Token: 0x04002811 RID: 10257
		[SerializeField]
		protected Sensor sensor_FM;

		// Token: 0x04002812 RID: 10258
		[SerializeField]
		protected Sensor sensor_FR;

		// Token: 0x04002813 RID: 10259
		[SerializeField]
		protected Sensor sensor_RR;

		// Token: 0x04002814 RID: 10260
		[SerializeField]
		protected Sensor sensor_RL;

		// Token: 0x04002815 RID: 10261
		[Header("Sweeping")]
		[SerializeField]
		protected LayerMask sweepMask;

		// Token: 0x04002816 RID: 10262
		[SerializeField]
		protected Transform sweepOrigin_FL;

		// Token: 0x04002817 RID: 10263
		[SerializeField]
		protected Transform sweepOrigin_FR;

		// Token: 0x04002818 RID: 10264
		[SerializeField]
		protected Transform sweepOrigin_RL;

		// Token: 0x04002819 RID: 10265
		[SerializeField]
		protected Transform sweepOrigin_RR;

		// Token: 0x0400281A RID: 10266
		[SerializeField]
		protected Wheel leftWheel;

		// Token: 0x0400281B RID: 10267
		[SerializeField]
		protected Wheel rightWheel;

		// Token: 0x0400281C RID: 10268
		protected const float sweepSegment = 15f;

		// Token: 0x0400281D RID: 10269
		[Header("Path following")]
		[SerializeField]
		[Range(0.1f, 5f)]
		protected float sampleStepSizeMin = 2.5f;

		// Token: 0x0400281E RID: 10270
		[SerializeField]
		[Range(0.1f, 5f)]
		protected float sampleStepSizeMax = 5f;

		// Token: 0x0400281F RID: 10271
		protected int aheadPointSamples = 4;

		// Token: 0x04002820 RID: 10272
		protected const float DestinationDistanceSlowThreshold = 8f;

		// Token: 0x04002821 RID: 10273
		protected const float DestinationArrivalThreshold = 3f;

		// Token: 0x04002822 RID: 10274
		[Header("Steer settings")]
		[SerializeField]
		protected float steerTargetFollowRate = 2f;

		// Token: 0x04002823 RID: 10275
		private SteerPID steerPID;

		// Token: 0x04002824 RID: 10276
		[Header("Turning speed reduction")]
		protected float turnSpeedReductionMinRange = 2f;

		// Token: 0x04002825 RID: 10277
		protected float turnSpeedReductionMaxRange = 10f;

		// Token: 0x04002826 RID: 10278
		protected float turnSpeedReductionDivisor = 90f;

		// Token: 0x04002827 RID: 10279
		private float minTurnSpeedReductionAngleThreshold = 15f;

		// Token: 0x04002828 RID: 10280
		private float minTurningSpeed = 10f;

		// Token: 0x04002829 RID: 10281
		[Header("Throttle")]
		[SerializeField]
		protected float throttleMin = -1f;

		// Token: 0x0400282A RID: 10282
		[SerializeField]
		protected float throttleMax = 1f;

		// Token: 0x0400282B RID: 10283
		private PID throttlePID;

		// Token: 0x0400282C RID: 10284
		public static float UnmarkedSpeed = 25f;

		// Token: 0x0400282D RID: 10285
		public static float ReverseSpeed = 5f;

		// Token: 0x0400282E RID: 10286
		private ValueTracker speedReductionTracker;

		// Token: 0x0400282F RID: 10287
		[Header("Pursuit Mode")]
		public bool PursuitModeEnabled;

		// Token: 0x04002830 RID: 10288
		public Transform PursuitTarget;

		// Token: 0x04002831 RID: 10289
		public float PursuitDistanceUpdateThreshold = 5f;

		// Token: 0x04002832 RID: 10290
		private Vector3 PursuitTargetLastPosition = Vector3.zero;

		// Token: 0x04002833 RID: 10291
		[Header("Stuck Detection")]
		public VehicleTeleporter Teleporter;

		// Token: 0x04002834 RID: 10292
		public PositionHistoryTracker PositionHistoryTracker;

		// Token: 0x04002835 RID: 10293
		public float StuckTimeThreshold = 10f;

		// Token: 0x04002836 RID: 10294
		public int StuckSamples = 4;

		// Token: 0x04002837 RID: 10295
		public float StuckDistanceThreshold = 1f;

		// Token: 0x04002838 RID: 10296
		protected VehicleAgent.NavigationCallback storedNavigationCallback;

		// Token: 0x04002839 RID: 10297
		protected SpeedZone currentSpeedZone;

		// Token: 0x0400283A RID: 10298
		protected LandVehicle vehicle;

		// Token: 0x0400283B RID: 10299
		protected float wheelbase;

		// Token: 0x0400283C RID: 10300
		protected float wheeltrack;

		// Token: 0x0400283D RID: 10301
		protected float vehicleLength;

		// Token: 0x0400283E RID: 10302
		protected float vehicleWidth;

		// Token: 0x0400283F RID: 10303
		protected float turnRadius;

		// Token: 0x04002840 RID: 10304
		protected float sweepTrack;

		// Token: 0x04002841 RID: 10305
		private float wheelBottomOffset;

		// Token: 0x04002842 RID: 10306
		[Header("Control info - READONLY")]
		[SerializeField]
		protected float targetSpeed;

		// Token: 0x04002843 RID: 10307
		[SerializeField]
		protected float targetSteerAngle_Normalized;

		// Token: 0x04002844 RID: 10308
		protected float lateralOffset;

		// Token: 0x04002845 RID: 10309
		protected PathSmoothingUtility.SmoothedPath path;

		// Token: 0x04002846 RID: 10310
		private float timeSinceLastNavigationCall;

		// Token: 0x04002847 RID: 10311
		private float sweepTestFailedTime;

		// Token: 0x04002848 RID: 10312
		private NavigationSettings currentNavigationSettings;

		// Token: 0x04002849 RID: 10313
		private Coroutine navigationCalculationRoutine;

		// Token: 0x0400284A RID: 10314
		private Coroutine reverseCoroutine;

		// Token: 0x020007E8 RID: 2024
		public enum ENavigationResult
		{
			// Token: 0x0400284C RID: 10316
			Failed,
			// Token: 0x0400284D RID: 10317
			Complete,
			// Token: 0x0400284E RID: 10318
			Stopped
		}

		// Token: 0x020007E9 RID: 2025
		public enum EAgentStatus
		{
			// Token: 0x04002850 RID: 10320
			Inactive,
			// Token: 0x04002851 RID: 10321
			MovingToRoad,
			// Token: 0x04002852 RID: 10322
			OnRoad
		}

		// Token: 0x020007EA RID: 2026
		public enum EPathGroupStatus
		{
			// Token: 0x04002854 RID: 10324
			Inactive,
			// Token: 0x04002855 RID: 10325
			Calculating
		}

		// Token: 0x020007EB RID: 2027
		public enum ESweepType
		{
			// Token: 0x04002857 RID: 10327
			FL,
			// Token: 0x04002858 RID: 10328
			FR,
			// Token: 0x04002859 RID: 10329
			RL,
			// Token: 0x0400285A RID: 10330
			RR
		}

		// Token: 0x020007EC RID: 2028
		// (Invoke) Token: 0x06003726 RID: 14118
		public delegate void NavigationCallback(VehicleAgent.ENavigationResult status);
	}
}
