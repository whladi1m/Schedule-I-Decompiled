using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000510 RID: 1296
	public class FootPatrolBehaviour : Behaviour
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x0007F5FF File Offset: 0x0007D7FF
		// (set) Token: 0x06001F20 RID: 7968 RVA: 0x0007F607 File Offset: 0x0007D807
		public PatrolGroup Group { get; protected set; }

		// Token: 0x06001F21 RID: 7969 RVA: 0x0007F610 File Offset: 0x0007D810
		protected override void Begin()
		{
			base.Begin();
			if (InstanceFinder.IsServer && this.Group == null)
			{
				Console.LogError("Foot patrol behaviour started without a group!", null);
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("footpatrol", 1, 0.08f));
			(base.Npc as PoliceOfficer).BodySearchChance = 0.4f;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0007F678 File Offset: 0x0007D878
		protected override void Resume()
		{
			base.Resume();
			if (InstanceFinder.IsServer && this.Group == null)
			{
				Console.LogError("Foot patrol behaviour resumed without a group!", null);
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("footpatrol", 1, 0.08f));
			(base.Npc as PoliceOfficer).BodySearchChance = 0.25f;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0007F6E0 File Offset: 0x0007D8E0
		protected override void Pause()
		{
			base.Pause();
			base.Npc.Movement.SpeedController.RemoveSpeedControl("footpatrol");
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0007F734 File Offset: 0x0007D934
		protected override void End()
		{
			base.End();
			if (this.Group != null)
			{
				this.Group.Members.Remove(base.Npc);
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("footpatrol");
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0007F7A4 File Offset: 0x0007D9A4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(1930, this.FLASHLIGHT_MAX_TIME))
			{
				if (this.UseFlashlight && !this.flashlightEquipped && this.Group.Members.Count > 0 && this.Group.Members[0] == base.Npc)
				{
					this.SetFlashlightEquipped(true);
				}
			}
			else if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			if (this.Group == null)
			{
				return;
			}
			if (!this.Group.Members.Contains(base.Npc))
			{
				Console.LogWarning("Foot patrol behaviour is not in group members list! Adding now", null);
				this.SetGroup(this.Group);
			}
			if (this.Group.IsPaused())
			{
				if (base.Npc.Movement.IsMoving)
				{
					base.Npc.Movement.Stop();
				}
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				return;
			}
			if (this.IsReadyToAdvance())
			{
				if (this.Group.Members.Count > 0 && this.Group.Members[0] == base.Npc && this.Group.IsGroupReadyToAdvance())
				{
					this.Group.AdvanceGroup();
					return;
				}
			}
			else if (!this.IsAtDestination())
			{
				base.Npc.Movement.SetDestination(this.Group.GetDestination(base.Npc));
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0007F925 File Offset: 0x0007DB25
		private void SetFlashlightEquipped(bool equipped)
		{
			this.flashlightEquipped = equipped;
			if (equipped)
			{
				base.Npc.SetEquippable_Networked(null, "Tools/Flashlight/Flashlight_AvatarEquippable");
				return;
			}
			base.Npc.SetEquippable_Networked(null, string.Empty);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0007F954 File Offset: 0x0007DB54
		public void SetGroup(PatrolGroup group)
		{
			this.Group = group;
			this.Group.Members.Add(base.Npc);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0007F974 File Offset: 0x0007DB74
		public bool IsReadyToAdvance()
		{
			Vector3 destination = this.Group.GetDestination(base.Npc);
			return Vector3.Distance(base.transform.position, destination) < 2f || (!base.Npc.Movement.IsMoving && base.Npc.Movement.IsAsCloseAsPossible(this.Group.GetDestination(base.Npc), 3f));
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0007F9EC File Offset: 0x0007DBEC
		private bool IsAtDestination()
		{
			return this.Group != null && Vector3.Distance(base.Npc.Movement.FootPosition, this.Group.GetDestination(base.Npc)) < 2f;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0007FA3F File Offset: 0x0007DC3F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0007FA58 File Offset: 0x0007DC58
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FootPatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0007FA71 File Offset: 0x0007DC71
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0007FA7F File Offset: 0x0007DC7F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001858 RID: 6232
		public const float MOVE_SPEED = 0.08f;

		// Token: 0x04001859 RID: 6233
		public const int FLASHLIGHT_MIN_TIME = 1930;

		// Token: 0x0400185A RID: 6234
		public int FLASHLIGHT_MAX_TIME = 500;

		// Token: 0x0400185B RID: 6235
		public const string FLASHLIGHT_ASSET_PATH = "Tools/Flashlight/Flashlight_AvatarEquippable";

		// Token: 0x0400185C RID: 6236
		public bool UseFlashlight = true;

		// Token: 0x0400185D RID: 6237
		private bool flashlightEquipped;

		// Token: 0x0400185F RID: 6239
		private bool dll_Excuted;

		// Token: 0x04001860 RID: 6240
		private bool dll_Excuted;
	}
}
