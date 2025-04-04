using System;
using FishNet.Object;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.Tools;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200044B RID: 1099
	public class NPCAnimation : NetworkBehaviour
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x00064A6B File Offset: 0x00062C6B
		private void Start()
		{
			this.npc = base.GetComponent<NPC>();
			NPC npc = this.npc;
			npc.onExitVehicle = (Action<LandVehicle>)Delegate.Combine(npc.onExitVehicle, new Action<LandVehicle>(delegate(LandVehicle <p0>)
			{
				this.ResetVelocityCalculations();
			}));
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00064AA0 File Offset: 0x00062CA0
		protected virtual void LateUpdate()
		{
			if (this.anim.enabled && !this.anim.IsAvatarCulled && this.npc.isVisible)
			{
				this.UpdateMovementAnimation();
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00064AD0 File Offset: 0x00062CD0
		protected virtual void UpdateMovementAnimation()
		{
			Vector3 vector = this.Avatar.transform.InverseTransformVector(this.velocityCalculator.Velocity) / 8f;
			this.anim.SetDirection(this.WalkMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
			this.anim.SetStrafe(this.WalkMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00064B5D File Offset: 0x00062D5D
		public virtual void SetRagdollActive(bool active)
		{
			this.Avatar.SetRagdollPhysicsEnabled(active, true);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00064B6C File Offset: 0x00062D6C
		public void ResetVelocityCalculations()
		{
			this.velocityCalculator.FlushBuffer();
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00064B79 File Offset: 0x00062D79
		public void StandupStart()
		{
			this.movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("ragdollstandup", 100, 0f));
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00064B9C File Offset: 0x00062D9C
		public void StandupDone()
		{
			this.movement.SpeedController.RemoveSpeedControl("ragdollstandup");
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00064BC3 File Offset: 0x00062DC3
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00064BD6 File Offset: 0x00062DD6
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCAnimationAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00064BE9 File Offset: 0x00062DE9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00064BE9 File Offset: 0x00062DE9
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040014CA RID: 5322
		[Header("References")]
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x040014CB RID: 5323
		[SerializeField]
		protected AvatarAnimation anim;

		// Token: 0x040014CC RID: 5324
		[SerializeField]
		protected NPCMovement movement;

		// Token: 0x040014CD RID: 5325
		protected NPC npc;

		// Token: 0x040014CE RID: 5326
		[SerializeField]
		protected SmoothedVelocityCalculator velocityCalculator;

		// Token: 0x040014CF RID: 5327
		[Header("Settings")]
		public AnimationCurve WalkMapCurve;

		// Token: 0x040014D0 RID: 5328
		private bool dll_Excuted;

		// Token: 0x040014D1 RID: 5329
		private bool dll_Excuted;
	}
}
