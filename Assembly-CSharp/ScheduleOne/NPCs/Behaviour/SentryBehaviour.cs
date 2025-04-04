using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000525 RID: 1317
	public class SentryBehaviour : Behaviour
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00084391 File Offset: 0x00082591
		// (set) Token: 0x0600201E RID: 8222 RVA: 0x00084399 File Offset: 0x00082599
		public SentryLocation AssignedLocation { get; private set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x000843A2 File Offset: 0x000825A2
		private Transform standPoint
		{
			get
			{
				return this.AssignedLocation.StandPoints[this.AssignedLocation.AssignedOfficers.IndexOf(this.officer)];
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000843CA File Offset: 0x000825CA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.SentryBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000803AD File Offset: 0x0007E5AD
		protected override void Begin()
		{
			base.Begin();
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000803B5 File Offset: 0x0007E5B5
		protected override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000843DE File Offset: 0x000825DE
		protected override void End()
		{
			base.End();
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000843F5 File Offset: 0x000825F5
		protected override void Pause()
		{
			base.Pause();
			if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0008440C File Offset: 0x0008260C
		public void AssignLocation(SentryLocation loc)
		{
			if (this.AssignedLocation != null)
			{
				this.UnassignLocation();
			}
			this.AssignedLocation = loc;
			this.AssignedLocation.AssignedOfficers.Add(this.officer);
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0008443F File Offset: 0x0008263F
		public void UnassignLocation()
		{
			if (this.AssignedLocation != null)
			{
				this.AssignedLocation.AssignedOfficers.Remove(this.officer);
				this.AssignedLocation = null;
			}
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00084470 File Offset: 0x00082670
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(1930, this.FLASHLIGHT_MAX_TIME))
			{
				if (this.UseFlashlight && !this.flashlightEquipped)
				{
					this.SetFlashlightEquipped(true);
				}
			}
			else if (this.flashlightEquipped)
			{
				this.SetFlashlightEquipped(false);
			}
			this.officer.BodySearchChance = 0.1f;
			if (!base.Npc.Movement.IsMoving)
			{
				if (Vector3.Distance(base.Npc.transform.position, this.standPoint.position) < 2f)
				{
					this.officer.BodySearchChance = 0.75f;
					if (!base.Npc.Movement.FaceDirectionInProgress)
					{
						base.Npc.Movement.FaceDirection(this.standPoint.forward, 0.5f);
						return;
					}
				}
				else if (base.Npc.Movement.CanMove())
				{
					base.Npc.Movement.SetDestination(this.standPoint.position);
				}
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00084586 File Offset: 0x00082786
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

		// Token: 0x0600202B RID: 8235 RVA: 0x000845CF File Offset: 0x000827CF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000845E8 File Offset: 0x000827E8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.SentryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00084601 File Offset: 0x00082801
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0008460F File Offset: 0x0008280F
		protected virtual void dll()
		{
			base.Awake();
			this.officer = (base.Npc as PoliceOfficer);
		}

		// Token: 0x040018E4 RID: 6372
		public const float BODY_SEARCH_CHANCE = 0.75f;

		// Token: 0x040018E5 RID: 6373
		public const int FLASHLIGHT_MIN_TIME = 1930;

		// Token: 0x040018E6 RID: 6374
		public int FLASHLIGHT_MAX_TIME = 500;

		// Token: 0x040018E7 RID: 6375
		public const string FLASHLIGHT_ASSET_PATH = "Tools/Flashlight/Flashlight_AvatarEquippable";

		// Token: 0x040018E8 RID: 6376
		public bool UseFlashlight = true;

		// Token: 0x040018E9 RID: 6377
		private bool flashlightEquipped;

		// Token: 0x040018EB RID: 6379
		private PoliceOfficer officer;

		// Token: 0x040018EC RID: 6380
		private bool dll_Excuted;

		// Token: 0x040018ED RID: 6381
		private bool dll_Excuted;
	}
}
