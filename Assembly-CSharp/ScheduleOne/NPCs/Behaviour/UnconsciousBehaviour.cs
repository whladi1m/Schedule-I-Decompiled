using System;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000527 RID: 1319
	public class UnconsciousBehaviour : Behaviour
	{
		// Token: 0x06002036 RID: 8246 RVA: 0x00084694 File Offset: 0x00082894
		protected override void Begin()
		{
			base.Begin();
			base.Npc.behaviour.RagdollBehaviour.Disable();
			base.Npc.Movement.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
			base.Npc.Movement.SetRagdollDraggable(true);
			base.Npc.dialogueHandler.HideWorldspaceDialogue();
			base.Npc.awareness.SetAwarenessActive(false);
			base.Npc.Avatar.EmotionManager.ClearOverrides();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Sleeping", "Dead", 0f, 20);
			this.Particles.Play();
			base.Npc.PlayVO(EVOLineType.Die);
			this.timeOnLastSnore = Time.time;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0008476C File Offset: 0x0008296C
		protected override void End()
		{
			base.End();
			base.Npc.awareness.SetAwarenessActive(true);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("Dead");
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			this.Particles.Stop();
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000847D6 File Offset: 0x000829D6
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.PlaySnoreSounds && Time.time - this.timeOnLastSnore > 6f)
			{
				base.Npc.PlayVO(EVOLineType.Snore);
				this.timeOnLastSnore = Time.time;
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00084820 File Offset: 0x00082A20
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00084839 File Offset: 0x00082A39
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.UnconsciousBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00084852 File Offset: 0x00082A52
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00084860 File Offset: 0x00082A60
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018F0 RID: 6384
		public const float SnoreInterval = 6f;

		// Token: 0x040018F1 RID: 6385
		public ParticleSystem Particles;

		// Token: 0x040018F2 RID: 6386
		public bool PlaySnoreSounds = true;

		// Token: 0x040018F3 RID: 6387
		private float timeOnLastSnore;

		// Token: 0x040018F4 RID: 6388
		private bool dll_Excuted;

		// Token: 0x040018F5 RID: 6389
		private bool dll_Excuted;
	}
}
