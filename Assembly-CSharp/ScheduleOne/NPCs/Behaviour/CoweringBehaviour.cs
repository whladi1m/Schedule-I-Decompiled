using System;
using ScheduleOne.VoiceOver;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004E2 RID: 1250
	public class CoweringBehaviour : Behaviour
	{
		// Token: 0x06001CA6 RID: 7334 RVA: 0x00076D21 File Offset: 0x00074F21
		protected override void Begin()
		{
			base.Begin();
			this.SetCowering(true);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00076D30 File Offset: 0x00074F30
		public override void Enable()
		{
			base.Enable();
			Console.Log("CoweringBehaviour Enabled", null);
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00076D43 File Offset: 0x00074F43
		protected override void End()
		{
			base.End();
			this.SetCowering(false);
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00076D52 File Offset: 0x00074F52
		protected override void Resume()
		{
			base.Resume();
			this.SetCowering(true);
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x00076D61 File Offset: 0x00074F61
		protected override void Pause()
		{
			base.Pause();
			this.SetCowering(false);
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x00076D80 File Offset: 0x00074F80
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			base.Npc.Avatar.LookController.OverrideLookTarget(base.Npc.Movement.FootPosition + base.Npc.Avatar.transform.forward * 2f, 5, false);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00076DE0 File Offset: 0x00074FE0
		private void SetCowering(bool cowering)
		{
			base.Npc.Avatar.Anim.SetCrouched(cowering);
			base.Npc.Avatar.Anim.SetBool("HandsUp", cowering);
			if (cowering)
			{
				base.Npc.PlayVO(EVOLineType.Scared);
				base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("cowering", 80, 0f));
				return;
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("cowering");
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x00076E6F File Offset: 0x0007506F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x00076E88 File Offset: 0x00075088
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CoweringBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x00076EA1 File Offset: 0x000750A1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00076EAF File Offset: 0x000750AF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001769 RID: 5993
		private bool dll_Excuted;

		// Token: 0x0400176A RID: 5994
		private bool dll_Excuted;
	}
}
