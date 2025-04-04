using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005F1 RID: 1521
	public class PlayerMovement : PlayerSingleton<PlayerMovement>
	{
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000A32B6 File Offset: 0x000A14B6
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x000A32BD File Offset: 0x000A14BD
		public static float GravityMultiplier { get; set; } = 1f;

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000A32C5 File Offset: 0x000A14C5
		// (set) Token: 0x060027C6 RID: 10182 RVA: 0x000A32CD File Offset: 0x000A14CD
		public float playerHeight { get; protected set; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000A32D6 File Offset: 0x000A14D6
		public Vector3 Movement
		{
			get
			{
				return this.movement;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x000A32DE File Offset: 0x000A14DE
		// (set) Token: 0x060027C9 RID: 10185 RVA: 0x000A32E6 File Offset: 0x000A14E6
		public LandVehicle currentVehicle { get; protected set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x000A32EF File Offset: 0x000A14EF
		// (set) Token: 0x060027CB RID: 10187 RVA: 0x000A32F7 File Offset: 0x000A14F7
		public float airTime { get; protected set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060027CC RID: 10188 RVA: 0x000A3300 File Offset: 0x000A1500
		// (set) Token: 0x060027CD RID: 10189 RVA: 0x000A3308 File Offset: 0x000A1508
		public bool isCrouched { get; protected set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060027CE RID: 10190 RVA: 0x000A3311 File Offset: 0x000A1511
		// (set) Token: 0x060027CF RID: 10191 RVA: 0x000A3319 File Offset: 0x000A1519
		public float standingScale { get; protected set; } = 1f;

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060027D0 RID: 10192 RVA: 0x000A3322 File Offset: 0x000A1522
		// (set) Token: 0x060027D1 RID: 10193 RVA: 0x000A332A File Offset: 0x000A152A
		public bool isRagdolled { get; protected set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060027D2 RID: 10194 RVA: 0x000A3333 File Offset: 0x000A1533
		// (set) Token: 0x060027D3 RID: 10195 RVA: 0x000A333B File Offset: 0x000A153B
		public bool isSprinting { get; protected set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060027D4 RID: 10196 RVA: 0x000A3344 File Offset: 0x000A1544
		// (set) Token: 0x060027D5 RID: 10197 RVA: 0x000A334C File Offset: 0x000A154C
		public float CurrentSprintMultiplier { get; protected set; } = 1f;

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060027D6 RID: 10198 RVA: 0x000A3355 File Offset: 0x000A1555
		// (set) Token: 0x060027D7 RID: 10199 RVA: 0x000A335D File Offset: 0x000A155D
		public bool IsGrounded { get; private set; } = true;

		// Token: 0x060027D8 RID: 10200 RVA: 0x000A3368 File Offset: 0x000A1568
		protected override void Awake()
		{
			base.Awake();
			this.playerHeight = this.Controller.height;
			this.Controller.detectCollisions = false;
			for (int i = 0; i < this.visibilityPointsToScale.Count; i++)
			{
				this.originalVisibilityPointOffsets.Add(this.visibilityPointsToScale[i], this.visibilityPointsToScale[i].localPosition.y);
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000A33DC File Offset: 0x000A15DC
		protected override void Start()
		{
			base.Start();
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.EnterVehicle));
			Player local2 = Player.Local;
			local2.onExitVehicle = (Player.VehicleTransformEvent)Delegate.Combine(local2.onExitVehicle, new Player.VehicleTransformEvent(this.ExitVehicle));
			Player.Local.Health.onRevive.AddListener(delegate()
			{
				this.SetStamina(PlayerMovement.StaminaReserveMax, false);
			});
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000A345C File Offset: 0x000A165C
		protected virtual void Update()
		{
			this.UpdateHorizontalAxis();
			this.UpdateVerticalAxis();
			if (this.isCrouched)
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 0f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			else
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 1f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			this.UpdatePlayerHeight();
			if (this.residualVelocityTimeRemaining > 0f)
			{
				this.residualVelocityTimeRemaining -= Time.deltaTime;
			}
			this.timeSinceStaminaDrain += Time.deltaTime;
			if (this.timeSinceStaminaDrain > 1f && this.CurrentStaminaReserve < PlayerMovement.StaminaReserveMax)
			{
				this.ChangeStamina(25f * Time.deltaTime, true);
			}
			this.Move();
			this.UpdateCrouchVignetteEffect();
			this.UpdateMovementEvents();
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000A3536 File Offset: 0x000A1736
		private void FixedUpdate()
		{
			this.IsGrounded = this.isGrounded();
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000A3544 File Offset: 0x000A1744
		private void LateUpdate()
		{
			if (this.teleport)
			{
				this.Controller.enabled = false;
				this.Controller.transform.position = this.teleportPosition;
				this.Controller.enabled = true;
				this.teleport = false;
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000A3584 File Offset: 0x000A1784
		protected virtual void Move()
		{
			this.isSprinting = false;
			if (!this.Controller.enabled)
			{
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
				return;
			}
			if (this.currentVehicle != null)
			{
				return;
			}
			if (this.IsGrounded)
			{
				this.timeGrounded += Time.deltaTime;
			}
			else
			{
				this.timeGrounded = 0f;
			}
			if (this.canMove && this.canJump && this.IsGrounded && !this.isJumping && !GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (!this.isCrouched)
				{
					this.isJumping = true;
					if (this.onJump != null)
					{
						this.onJump();
					}
					Player.Local.PlayJumpAnimation();
					base.StartCoroutine(this.Jump());
				}
				else
				{
					this.TryToggleCrouch();
				}
			}
			if (this.canMove && !GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && GameInput.GetButtonDown(GameInput.ButtonCode.Crouch))
			{
				this.TryToggleCrouch();
			}
			if (!this.IsGrounded)
			{
				this.airTime += Time.deltaTime;
			}
			else
			{
				this.isJumping = false;
				if (this.airTime > 0.1f && this.onLand != null)
				{
					this.onLand();
				}
				this.airTime = 0f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Sprint) && !this.sprintActive)
			{
				this.sprintActive = true;
				this.sprintReleased = false;
			}
			else if (GameInput.GetButton(GameInput.ButtonCode.Sprint) && Singleton<Settings>.Instance.SprintMode == InputSettings.EActionMode.Hold)
			{
				this.sprintActive = true;
			}
			else if (Singleton<Settings>.Instance.SprintMode == InputSettings.EActionMode.Hold)
			{
				this.sprintActive = false;
			}
			if (!GameInput.GetButton(GameInput.ButtonCode.Sprint))
			{
				this.sprintReleased = true;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Sprint) && this.sprintReleased)
			{
				this.sprintActive = !this.sprintActive;
			}
			this.isSprinting = false;
			if (this.sprintActive && this.canMove && !this.isCrouched && !Player.Local.IsTased && (this.horizontalAxis != 0f || this.verticalAxis != 0f) && this.sprintBlockers.Count == 0)
			{
				if (this.CurrentStaminaReserve > 0f || !this.SprintingRequiresStamina)
				{
					this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, PlayerMovement.SprintMultiplier, Time.deltaTime * 4f);
					if (this.SprintingRequiresStamina)
					{
						this.ChangeStamina(-12.5f * Time.deltaTime, true);
					}
					this.isSprinting = true;
				}
				else
				{
					this.sprintActive = false;
					this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
				}
			}
			else
			{
				this.sprintActive = false;
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
			}
			if (!this.isSprinting && this.timeSinceStaminaDrain > 1f)
			{
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
			}
			float num = 1f;
			if (this.isCrouched)
			{
				num = 1f - (1f - this.crouchSpeedMultipler) * (1f - this.standingScale);
			}
			float num2 = PlayerMovement.WalkSpeed * this.CurrentSprintMultiplier * num * PlayerMovement.StaticMoveSpeedMultiplier * this.MoveSpeedMultiplier;
			if (Player.Local.IsTased)
			{
				num2 *= 0.5f;
			}
			if ((Application.isEditor || Debug.isDebugBuild) && this.isSprinting)
			{
				num2 *= 1f;
			}
			if (this.Controller.isGrounded)
			{
				if (this.canMove)
				{
					Vector3 vector = this.movement;
					this.movement = new Vector3(this.horizontalAxis, -this.Controller.stepOffset, this.verticalAxis);
					this.movement = base.transform.TransformDirection(this.movement);
					this.ClampMovement();
					this.movement.x = this.movement.x * num2;
					this.movement.z = this.movement.z * num2;
				}
				else
				{
					this.movement = new Vector3(0f, -this.Controller.stepOffset, 0f);
				}
			}
			else if (this.canMove)
			{
				Vector3 vector2 = this.movement;
				this.movement = new Vector3(this.horizontalAxis, this.movement.y, this.verticalAxis);
				this.movement = base.transform.TransformDirection(this.movement);
				this.ClampMovement();
				this.movement.x = this.movement.x * num2;
				this.movement.z = this.movement.z * num2;
			}
			else
			{
				this.movement = new Vector3(0f, this.movement.y, 0f);
			}
			if (!this.canMove)
			{
				this.movement.x = Mathf.MoveTowards(this.movement.x, 0f, this.sensitivity * Time.deltaTime);
				this.movement.z = Mathf.MoveTowards(this.movement.z, 0f, this.sensitivity * Time.deltaTime);
			}
			this.movement.y = this.movement.y + Physics.gravity.y * this.gravityMultiplier * Time.deltaTime * PlayerMovement.GravityMultiplier;
			this.movement.y = this.movement.y + this.movementY;
			this.movementY = 0f;
			if (this.residualVelocityTimeRemaining > 0f)
			{
				this.movement += this.residualVelocityDirection * this.residualVelocityForce * Mathf.Clamp01(this.residualVelocityTimeRemaining / this.residualVelocityDuration) * Time.deltaTime;
			}
			if (Player.Local.Slippery)
			{
				this.movement = Vector3.Lerp(this.movement, new Vector3(this.lastFrameMovement.x, this.movement.y, this.lastFrameMovement.z), this.SlipperyMovementMultiplier);
			}
			float surfaceAngle = this.GetSurfaceAngle();
			if ((this.horizontalAxis != 0f || this.verticalAxis != 0f) && surfaceAngle > 5f)
			{
				float d = Mathf.Clamp01(surfaceAngle / this.Controller.slopeLimit);
				Vector3 b = Vector3.down * Time.deltaTime * this.slopeForce * d;
				this.Controller.Move(this.movement * Time.deltaTime + b);
			}
			else
			{
				this.Controller.Move(this.movement * Time.deltaTime);
			}
			this.lastFrameMovement = this.movement;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000A3C60 File Offset: 0x000A1E60
		private void ClampMovement()
		{
			float y = this.movement.y;
			this.movement = Vector3.ClampMagnitude(new Vector3(this.movement.x, 0f, this.movement.z), 1f);
			this.movement.y = y;
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000A3CB8 File Offset: 0x000A1EB8
		protected float GetSurfaceAngle()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, this.slopeForceRayLength, this.groundDetectionMask))
			{
				return Vector3.Angle(raycastHit.normal, Vector3.up);
			}
			return 0f;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000A3D06 File Offset: 0x000A1F06
		private bool isGrounded()
		{
			return Player.Local.GetIsGrounded();
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000A3D14 File Offset: 0x000A1F14
		protected void UpdateHorizontalAxis()
		{
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.horizontalAxis = 0f;
				return;
			}
			int num = (!GameInput.IsTyping) ? Mathf.RoundToInt(GameInput.MotionAxis.x) : 0;
			if (this.Player.Disoriented)
			{
				num = -num;
			}
			if (this.Player.Schizophrenic && Time.timeSinceLevelLoad % 20f < 1f)
			{
				num = -num;
			}
			float num2 = Mathf.MoveTowards(this.horizontalAxis, (float)num, this.sensitivity * Time.deltaTime);
			this.horizontalAxis = ((Mathf.Abs(num2) < this.dead) ? 0f : num2);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000A3DBC File Offset: 0x000A1FBC
		protected void UpdateVerticalAxis()
		{
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.verticalAxis = 0f;
				return;
			}
			int num = (!GameInput.IsTyping) ? Mathf.RoundToInt(GameInput.MotionAxis.y) : 0;
			if (this.Player.Schizophrenic && (Time.timeSinceLevelLoad + 5f) % 25f < 1f)
			{
				num = -num;
			}
			float num2 = Mathf.MoveTowards(this.verticalAxis, (float)num, this.sensitivity * Time.deltaTime);
			this.verticalAxis = ((Mathf.Abs(num2) < this.dead) ? 0f : num2);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000A3E59 File Offset: 0x000A2059
		private IEnumerator Jump()
		{
			float savedSlopeLimit = this.Controller.slopeLimit;
			this.Controller.velocity.Set(this.Controller.velocity.x, 0f, this.Controller.velocity.y);
			this.movementY += this.jumpForce * PlayerMovement.JumpMultiplier;
			this.timeGrounded = 0f;
			do
			{
				yield return new WaitForEndOfFrame();
			}
			while (this.timeGrounded < 0.05f && this.Controller.collisionFlags != CollisionFlags.Above && this.currentVehicle == null);
			this.Controller.slopeLimit = savedSlopeLimit;
			yield break;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000A3E68 File Offset: 0x000A2068
		private void TryToggleCrouch()
		{
			if (this.isCrouched)
			{
				if (this.CanStand())
				{
					this.SetCrouched(false);
					return;
				}
			}
			else
			{
				this.SetCrouched(true);
			}
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000A3E8C File Offset: 0x000A208C
		public bool CanStand()
		{
			float num = this.Controller.radius * 0.75f;
			float num2 = 0.1f;
			Vector3 origin = base.transform.position - Vector3.up * this.Controller.height * 0.5f + Vector3.up * num + Vector3.up * num2;
			float maxDistance = this.playerHeight - num * 2f - num2;
			RaycastHit raycastHit;
			return !Physics.SphereCast(origin, num, Vector3.up, out raycastHit, maxDistance, this.groundDetectionMask);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000A3F30 File Offset: 0x000A2130
		public void SetCrouched(bool c)
		{
			this.isCrouched = c;
			this.Player.SendCrouched(this.isCrouched);
			this.Player.SetCrouchedLocal(this.isCrouched);
			VisibilityAttribute visibilityAttribute = Player.Local.Visibility.GetAttribute("Crouched");
			if (this.isCrouched)
			{
				if (visibilityAttribute == null)
				{
					visibilityAttribute = new VisibilityAttribute("Crouched", 0f, 0.8f, 1);
					return;
				}
			}
			else if (visibilityAttribute != null)
			{
				visibilityAttribute.Delete();
			}
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000A3FA8 File Offset: 0x000A21A8
		private void UpdateCrouchVignetteEffect()
		{
			float intensity = Mathf.Lerp(this.Crouched_VigIntensity, Singleton<PostProcessingManager>.Instance.Vig_DefaultIntensity, this.standingScale);
			float smoothness = Mathf.Lerp(this.Crouched_VigSmoothness, Singleton<PostProcessingManager>.Instance.Vig_DefaultSmoothness, this.standingScale);
			Singleton<PostProcessingManager>.Instance.OverrideVignette(intensity, smoothness);
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000A3FFC File Offset: 0x000A21FC
		private void UpdatePlayerHeight()
		{
			float height = this.Controller.height;
			this.Controller.height = this.playerHeight - this.playerHeight * (1f - PlayerMovement.CrouchHeightMultiplier) * (1f - this.standingScale);
			float num = this.Controller.height - height;
			if (this.IsGrounded && Mathf.Abs(num) > 1E-05f)
			{
				this.movementY += num * 0.5f;
			}
			if (Mathf.Abs(num) > 0.0001f)
			{
				for (int i = 0; i < this.visibilityPointsToScale.Count; i++)
				{
					this.visibilityPointsToScale[i].localPosition = new Vector3(this.visibilityPointsToScale[i].localPosition.x, this.originalVisibilityPointOffsets[this.visibilityPointsToScale[i]] * (this.Controller.height / this.playerHeight), this.visibilityPointsToScale[i].localPosition.z);
				}
			}
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000A410F File Offset: 0x000A230F
		public void LerpPlayerRotation(Quaternion rotation, float lerpTime)
		{
			if (this.playerRotCoroutine != null)
			{
				base.StopCoroutine(this.playerRotCoroutine);
			}
			this.playerRotCoroutine = base.StartCoroutine(this.LerpPlayerRotation_Process(rotation, lerpTime));
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000A4139 File Offset: 0x000A2339
		private IEnumerator LerpPlayerRotation_Process(Quaternion endRotation, float lerpTime)
		{
			Quaternion startRot = this.Player.transform.rotation;
			this.Controller.enabled = false;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.Player.transform.rotation = Quaternion.Lerp(startRot, endRotation, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.Player.transform.rotation = endRotation;
			this.Controller.enabled = true;
			this.playerRotCoroutine = null;
			yield break;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000A4158 File Offset: 0x000A2358
		private void EnterVehicle(LandVehicle vehicle)
		{
			this.currentVehicle = vehicle;
			this.canMove = false;
			this.Controller.enabled = false;
			if (this.recentlyDrivenVehicles.Contains(vehicle))
			{
				this.recentlyDrivenVehicles.Remove(vehicle);
			}
			this.recentlyDrivenVehicles.Insert(0, vehicle);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A41A7 File Offset: 0x000A23A7
		private void ExitVehicle(LandVehicle veh, Transform exitPoint)
		{
			this.currentVehicle = null;
			this.canMove = true;
			this.Controller.enabled = true;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000A41C4 File Offset: 0x000A23C4
		public void Teleport(Vector3 position)
		{
			string str = "Player teleported: ";
			Vector3 vector = position;
			Console.Log(str + vector.ToString(), null);
			if (this.Player.ActiveSkateboard != null)
			{
				this.Player.ActiveSkateboard.Equippable.Dismount();
			}
			this.Controller.enabled = false;
			this.Controller.transform.position = position;
			this.Controller.enabled = true;
			this.teleport = true;
			this.teleportPosition = position;
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000A424F File Offset: 0x000A244F
		public void SetResidualVelocity(Vector3 dir, float force, float time)
		{
			this.residualVelocityDirection = dir.normalized;
			this.residualVelocityForce = force;
			this.residualVelocityDuration = time;
			this.residualVelocityTimeRemaining = time;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A4274 File Offset: 0x000A2474
		public void WarpToNavMesh()
		{
			NavMeshQueryFilter filter = default(NavMeshQueryFilter);
			filter.agentTypeID = Singleton<PlayerManager>.Instance.PlayerRecoverySurface.agentTypeID;
			filter.areaMask = -1;
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(PlayerSingleton<PlayerMovement>.Instance.transform.position, out navMeshHit, 100f, filter))
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(navMeshHit.position + Vector3.up * 1f);
				return;
			}
			Console.LogError("Failed to find recovery point!", null);
			PlayerSingleton<PlayerMovement>.Instance.Teleport(Vector3.up * 5f);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A4310 File Offset: 0x000A2510
		public void RegisterMovementEvent(int threshold, Action action)
		{
			if (threshold < 1)
			{
				Console.LogWarning("Movement events min. threshold is 1m!", null);
				return;
			}
			if (!this.movementEvents.ContainsKey(threshold))
			{
				this.movementEvents.Add(threshold, new PlayerMovement.MovementEvent());
			}
			this.movementEvents[threshold].actions.Add(action);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A4364 File Offset: 0x000A2564
		public void DeregisterMovementEvent(Action action)
		{
			foreach (int key in this.movementEvents.Keys)
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[key];
				if (movementEvent.actions.Contains(action))
				{
					movementEvent.actions.Remove(action);
					break;
				}
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A43E0 File Offset: 0x000A25E0
		private void UpdateMovementEvents()
		{
			foreach (int num in this.movementEvents.Keys.ToList<int>())
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[num];
				if (Vector3.Distance(this.Player.Avatar.CenterPoint, movementEvent.LastUpdatedDistance) > (float)num)
				{
					movementEvent.Update(this.Player.Avatar.CenterPoint);
				}
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000A4478 File Offset: 0x000A2678
		public void ChangeStamina(float change, bool notify = true)
		{
			if (change < 0f)
			{
				this.timeSinceStaminaDrain = 0f;
			}
			this.SetStamina(this.CurrentStaminaReserve + change, notify);
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A449C File Offset: 0x000A269C
		public void SetStamina(float value, bool notify = true)
		{
			if (this.CurrentStaminaReserve == value)
			{
				return;
			}
			float currentStaminaReserve = this.CurrentStaminaReserve;
			this.CurrentStaminaReserve = Mathf.Clamp(value, 0f, PlayerMovement.StaminaReserveMax);
			if (notify && this.onStaminaReserveChanged != null)
			{
				this.onStaminaReserveChanged(this.CurrentStaminaReserve - currentStaminaReserve);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000A44EE File Offset: 0x000A26EE
		public void AddSprintBlocker(string tag)
		{
			if (!this.sprintBlockers.Contains(tag))
			{
				this.sprintBlockers.Add(tag);
			}
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000A450A File Offset: 0x000A270A
		public void RemoveSprintBlocker(string tag)
		{
			if (this.sprintBlockers.Contains(tag))
			{
				this.sprintBlockers.Remove(tag);
			}
		}

		// Token: 0x04001CD7 RID: 7383
		public const float DEV_SPRINT_MULTIPLIER = 1f;

		// Token: 0x04001CD8 RID: 7384
		public const float GROUNDED_THRESHOLD = 0.05f;

		// Token: 0x04001CD9 RID: 7385
		public const float SLOPE_THRESHOLD = 5f;

		// Token: 0x04001CDA RID: 7386
		public static float WalkSpeed = 3.25f;

		// Token: 0x04001CDB RID: 7387
		public static float SprintMultiplier = 1.9f;

		// Token: 0x04001CDC RID: 7388
		public static float StaticMoveSpeedMultiplier = 1f;

		// Token: 0x04001CDD RID: 7389
		public const float StaminaRestoreDelay = 1f;

		// Token: 0x04001CDE RID: 7390
		public static float JumpMultiplier = 1f;

		// Token: 0x04001CDF RID: 7391
		public static float ControllerRadius = 0.35f;

		// Token: 0x04001CE0 RID: 7392
		public static float StandingControllerHeight = 1.85f;

		// Token: 0x04001CE1 RID: 7393
		public static float CrouchHeightMultiplier = 0.65f;

		// Token: 0x04001CE2 RID: 7394
		public static float CrouchTime = 0.2f;

		// Token: 0x04001CE4 RID: 7396
		public const float StaminaDrainRate = 12.5f;

		// Token: 0x04001CE5 RID: 7397
		public const float StaminaRestoreRate = 25f;

		// Token: 0x04001CE6 RID: 7398
		public static float StaminaReserveMax = 100f;

		// Token: 0x04001CE7 RID: 7399
		public const float SprintChangeRate = 4f;

		// Token: 0x04001CE8 RID: 7400
		[Header("References")]
		public Player Player;

		// Token: 0x04001CE9 RID: 7401
		public CharacterController Controller;

		// Token: 0x04001CEA RID: 7402
		[Header("Move settings")]
		[SerializeField]
		protected float sensitivity = 6f;

		// Token: 0x04001CEB RID: 7403
		[SerializeField]
		protected float dead = 0.001f;

		// Token: 0x04001CEC RID: 7404
		public bool canMove = true;

		// Token: 0x04001CED RID: 7405
		public bool canJump = true;

		// Token: 0x04001CEE RID: 7406
		public bool SprintingRequiresStamina = true;

		// Token: 0x04001CEF RID: 7407
		public float MoveSpeedMultiplier = 1f;

		// Token: 0x04001CF0 RID: 7408
		public float SlipperyMovementMultiplier = 1f;

		// Token: 0x04001CF1 RID: 7409
		[Header("Jump/fall settings")]
		[SerializeField]
		protected float jumpForce = 4.5f;

		// Token: 0x04001CF2 RID: 7410
		[SerializeField]
		protected float gravityMultiplier = 1f;

		// Token: 0x04001CF3 RID: 7411
		[SerializeField]
		protected LayerMask groundDetectionMask;

		// Token: 0x04001CF4 RID: 7412
		[Header("Slope Settings")]
		[SerializeField]
		protected float slopeForce;

		// Token: 0x04001CF5 RID: 7413
		[SerializeField]
		protected float slopeForceRayLength;

		// Token: 0x04001CF6 RID: 7414
		[Header("Crouch Settings")]
		[SerializeField]
		protected float crouchSpeedMultipler = 0.5f;

		// Token: 0x04001CF7 RID: 7415
		[SerializeField]
		protected float Crouched_VigIntensity = 0.8f;

		// Token: 0x04001CF8 RID: 7416
		[SerializeField]
		protected float Crouched_VigSmoothness = 1f;

		// Token: 0x04001CF9 RID: 7417
		[Header("Visibility Points")]
		[SerializeField]
		protected List<Transform> visibilityPointsToScale = new List<Transform>();

		// Token: 0x04001CFA RID: 7418
		private Dictionary<Transform, float> originalVisibilityPointOffsets = new Dictionary<Transform, float>();

		// Token: 0x04001CFC RID: 7420
		protected Vector3 movement = Vector3.zero;

		// Token: 0x04001CFD RID: 7421
		protected float movementY;

		// Token: 0x04001CFF RID: 7423
		public List<LandVehicle> recentlyDrivenVehicles = new List<LandVehicle>();

		// Token: 0x04001D00 RID: 7424
		private bool isJumping;

		// Token: 0x04001D07 RID: 7431
		public float CurrentStaminaReserve = PlayerMovement.StaminaReserveMax;

		// Token: 0x04001D09 RID: 7433
		public Action<float> onStaminaReserveChanged;

		// Token: 0x04001D0A RID: 7434
		public Action onJump;

		// Token: 0x04001D0B RID: 7435
		public Action onLand;

		// Token: 0x04001D0C RID: 7436
		public UnityEvent onCrouch;

		// Token: 0x04001D0D RID: 7437
		public UnityEvent onUncrouch;

		// Token: 0x04001D0E RID: 7438
		protected float horizontalAxis;

		// Token: 0x04001D0F RID: 7439
		protected float verticalAxis;

		// Token: 0x04001D10 RID: 7440
		protected float timeGrounded;

		// Token: 0x04001D11 RID: 7441
		private Dictionary<int, PlayerMovement.MovementEvent> movementEvents = new Dictionary<int, PlayerMovement.MovementEvent>();

		// Token: 0x04001D12 RID: 7442
		private float timeSinceStaminaDrain = 10000f;

		// Token: 0x04001D13 RID: 7443
		private bool sprintActive;

		// Token: 0x04001D14 RID: 7444
		private bool sprintReleased;

		// Token: 0x04001D15 RID: 7445
		private Vector3 residualVelocityDirection = Vector3.zero;

		// Token: 0x04001D16 RID: 7446
		private float residualVelocityForce;

		// Token: 0x04001D17 RID: 7447
		private float residualVelocityDuration;

		// Token: 0x04001D18 RID: 7448
		private float residualVelocityTimeRemaining;

		// Token: 0x04001D19 RID: 7449
		private bool teleport;

		// Token: 0x04001D1A RID: 7450
		private Vector3 teleportPosition = Vector3.zero;

		// Token: 0x04001D1B RID: 7451
		private List<string> sprintBlockers = new List<string>();

		// Token: 0x04001D1C RID: 7452
		private Vector3 lastFrameMovement = Vector3.zero;

		// Token: 0x04001D1D RID: 7453
		private Coroutine playerRotCoroutine;

		// Token: 0x020005F2 RID: 1522
		public class MovementEvent
		{
			// Token: 0x060027FA RID: 10234 RVA: 0x000A46CC File Offset: 0x000A28CC
			public void Update(Vector3 newPosition)
			{
				this.LastUpdatedDistance = newPosition;
				foreach (Action action in this.actions)
				{
					action();
				}
			}

			// Token: 0x04001D1E RID: 7454
			public List<Action> actions = new List<Action>();

			// Token: 0x04001D1F RID: 7455
			public Vector3 LastUpdatedDistance = Vector3.zero;
		}
	}
}
