using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Skating
{
	// Token: 0x020002D2 RID: 722
	public class Skateboard_Equippable : Equippable_Viewmodel
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00045684 File Offset: 0x00043884
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x0004568C File Offset: 0x0004388C
		public bool IsRiding { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x00045695 File Offset: 0x00043895
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0004569D File Offset: 0x0004389D
		public Skateboard ActiveSkateboard { get; private set; }

		// Token: 0x06000F9C RID: 3996 RVA: 0x000456A6 File Offset: 0x000438A6
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("heldskateboard");
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x000456D0 File Offset: 0x000438D0
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (!this.IsRiding)
			{
				return;
			}
			action.used = true;
			this.Dismount();
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000456FC File Offset: 0x000438FC
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && !this.blockDismount && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				if (this.IsRiding)
				{
					if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						this.Dismount();
					}
				}
				else if (this.CanMountHere() && !PlayerSingleton<PlayerMovement>.Instance.isCrouched && (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || this.mountTime > 0f))
				{
					this.mountTime += Time.deltaTime;
					Singleton<HUD>.Instance.ShowRadialIndicator(this.mountTime / 0.33f);
					if (this.mountTime >= 0.33f)
					{
						this.Mount();
					}
				}
				else
				{
					this.mountTime = 0f;
				}
			}
			else
			{
				this.mountTime = 0f;
			}
			if (this.IsRiding && Vector3.Angle(this.ActiveSkateboard.transform.up, Vector3.up) > 80f)
			{
				this.Dismount();
			}
			this.UpdateModel();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00045804 File Offset: 0x00043A04
		private void UpdateModel()
		{
			Vector3 b = this.IsRiding ? this.ModelPosition_Lowered.localPosition : this.ModelPosition_Raised.localPosition;
			this.ModelContainer.localPosition = Vector3.Lerp(this.ModelContainer.localPosition, b, Time.deltaTime * 8f);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00045859 File Offset: 0x00043A59
		public override void Unequip()
		{
			base.Unequip();
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.IsRiding)
			{
				this.Dismount();
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0004588C File Offset: 0x00043A8C
		public void Mount()
		{
			this.IsRiding = true;
			this.mountTime = 0f;
			this.ActiveSkateboard = UnityEngine.Object.Instantiate<GameObject>(this.SkateboardPrefab.gameObject, null).GetComponent<Skateboard>();
			this.ActiveSkateboard.Equippable = this;
			Pose skateboardSpawnPose = this.GetSkateboardSpawnPose();
			this.ActiveSkateboard.transform.position = skateboardSpawnPose.position;
			this.ActiveSkateboard.transform.rotation = skateboardSpawnPose.rotation;
			Player.Local.Spawn(this.ActiveSkateboard.NetworkObject, Player.Local.Connection, default(Scene));
			Vector3 velocity = Player.Local.VelocityCalculator.Velocity;
			this.ActiveSkateboard.SetVelocity(velocity * 1.2f);
			Player.Local.MountSkateboard(this.ActiveSkateboard);
			Player.Local.Avatar.SetEquippable(string.Empty);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0004597C File Offset: 0x00043B7C
		public void Dismount()
		{
			this.IsRiding = false;
			this.mountTime = 0f;
			Vector3 velocity = this.ActiveSkateboard.Rb.velocity;
			float num = 50f;
			float time = 0.7f * Mathf.Clamp01(velocity.magnitude / 9f);
			Vector3 normalized = Vector3.ProjectOnPlane(velocity, Vector3.up).normalized;
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(normalized, velocity.magnitude * num, time);
			Player.Local.DismountSkateboard();
			Player.Local.Despawn(this.ActiveSkateboard.NetworkObject, null);
			UnityEngine.Object.Destroy(this.ActiveSkateboard.gameObject);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("heldskateboard");
			this.ActiveSkateboard = null;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00045A44 File Offset: 0x00043C44
		private bool CanMountHere()
		{
			return Vector3.Angle(this.GetSkateboardSpawnPose().rotation * Vector3.up, Vector3.up) <= 30f;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00045A70 File Offset: 0x00043C70
		private Pose GetSkateboardSpawnPose()
		{
			Vector3 vector = Player.Local.PlayerBasePosition + Player.Local.transform.forward * 0.4f + Vector3.up * 0.4f;
			Vector3 vector2 = Player.Local.PlayerBasePosition - Player.Local.transform.forward * 0.4f + Vector3.up * 0.4f;
			Debug.DrawRay(vector, Vector3.down * 0.7f, Color.cyan, 10f);
			Debug.DrawRay(vector2, Vector3.down * 0.7f, Color.cyan, 10f);
			RaycastHit raycastHit;
			if (!Physics.Raycast(vector, Vector3.down, out raycastHit, 0.7f, this.SkateboardPrefab.GroundDetectionMask, QueryTriggerInteraction.Ignore))
			{
				raycastHit.point = vector + Vector3.down * 0.7f;
			}
			RaycastHit raycastHit2;
			if (!Physics.Raycast(vector2, Vector3.down, out raycastHit2, 0.7f, this.SkateboardPrefab.GroundDetectionMask, QueryTriggerInteraction.Ignore))
			{
				raycastHit2.point = vector2 + Vector3.down * 0.7f;
			}
			Vector3 position = (raycastHit.point + raycastHit2.point) / 2f + Vector3.up * 0.1f;
			Vector3 normalized = (raycastHit.point - raycastHit2.point).normalized;
			Vector3 normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
			Vector3 normalized3 = Vector3.Cross(normalized, normalized2).normalized;
			Quaternion rotation = Quaternion.LookRotation(normalized, normalized3);
			return new Pose
			{
				position = position,
				rotation = rotation
			};
		}

		// Token: 0x04001045 RID: 4165
		public const float ModelLerpSpeed = 8f;

		// Token: 0x04001046 RID: 4166
		public const float SurfaceSampleDistance = 0.4f;

		// Token: 0x04001047 RID: 4167
		public const float SurfaceSampleRayLength = 0.7f;

		// Token: 0x04001048 RID: 4168
		public const float BoardSpawnUpwardsShift = 0.1f;

		// Token: 0x04001049 RID: 4169
		public const float BoardSpawnAngleLimit = 30f;

		// Token: 0x0400104A RID: 4170
		public const float MountTime = 0.33f;

		// Token: 0x0400104B RID: 4171
		public const float BoardMomentumTransfer = 1.2f;

		// Token: 0x0400104C RID: 4172
		public const float DismountAngle = 80f;

		// Token: 0x0400104F RID: 4175
		public Skateboard SkateboardPrefab;

		// Token: 0x04001050 RID: 4176
		public bool blockDismount;

		// Token: 0x04001051 RID: 4177
		[Header("References")]
		public Transform ModelContainer;

		// Token: 0x04001052 RID: 4178
		public Transform ModelPosition_Raised;

		// Token: 0x04001053 RID: 4179
		public Transform ModelPosition_Lowered;

		// Token: 0x04001054 RID: 4180
		private float mountTime;
	}
}
