using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004E3 RID: 1251
	public class DeadBehaviour : Behaviour
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x00076EC3 File Offset: 0x000750C3
		public bool IsInMedicalCenter
		{
			get
			{
				return base.Npc.CurrentBuilding == Singleton<Map>.Instance.MedicalCentre;
			}
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x00076EDF File Offset: 0x000750DF
		private void Start()
		{
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x00076F01 File Offset: 0x00075101
		private void OnDestroy()
		{
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x00076F24 File Offset: 0x00075124
		protected override void Begin()
		{
			base.Begin();
			base.Npc.behaviour.RagdollBehaviour.Disable();
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				this.EnterMedicalCentre();
			}
			else
			{
				base.Npc.Movement.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
				base.Npc.Movement.SetRagdollDraggable(true);
			}
			base.Npc.dialogueHandler.HideWorldspaceDialogue();
			base.Npc.awareness.SetAwarenessActive(false);
			base.Npc.Avatar.EmotionManager.ClearOverrides();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Sleeping", "Dead", 0f, 20);
			base.Npc.PlayVO(EVOLineType.Die);
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x00076FFC File Offset: 0x000751FC
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!this.IsInMedicalCenter && !base.Npc.Avatar.Ragdolled)
			{
				if (base.Npc.Movement.IsMoving)
				{
					base.Npc.Movement.Stop();
				}
				this.EnterMedicalCentre();
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00077051 File Offset: 0x00075251
		private void SleepStart()
		{
			if (base.Active && !this.IsInMedicalCenter)
			{
				this.EnterMedicalCentre();
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x0007706C File Offset: 0x0007526C
		private void EnterMedicalCentre()
		{
			Console.Log(base.Npc.fullName + " entering medical center", null);
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			base.Npc.EnterBuilding(null, Singleton<Map>.Instance.MedicalCentre.GUID.ToString(), 0);
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000770E0 File Offset: 0x000752E0
		protected override void End()
		{
			base.End();
			base.Npc.awareness.SetAwarenessActive(true);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("Dead");
			base.Npc.Movement.DeactivateRagdoll();
			base.Npc.Movement.SetRagdollDraggable(false);
			if (this.IsInMedicalCenter)
			{
				base.Npc.ExitBuilding("");
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x00077157 File Offset: 0x00075357
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x00077170 File Offset: 0x00075370
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DeadBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x00077189 File Offset: 0x00075389
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00077197 File Offset: 0x00075397
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400176B RID: 5995
		private bool dll_Excuted;

		// Token: 0x0400176C RID: 5996
		private bool dll_Excuted;
	}
}
