using System;
using FishNet;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200046E RID: 1134
	[Serializable]
	public abstract class NPCAction : NetworkBehaviour
	{
		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0006C946 File Offset: 0x0006AB46
		protected string ActionName
		{
			get
			{
				return "ActionName";
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x0006C94D File Offset: 0x0006AB4D
		public bool IsEvent
		{
			get
			{
				return this is NPCEvent;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0006C958 File Offset: 0x0006AB58
		public bool IsSignal
		{
			get
			{
				return this is NPCSignal;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x0006C963 File Offset: 0x0006AB63
		// (set) Token: 0x060018AB RID: 6315 RVA: 0x0006C96B File Offset: 0x0006AB6B
		public bool IsActive { get; protected set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x0006C974 File Offset: 0x0006AB74
		// (set) Token: 0x060018AD RID: 6317 RVA: 0x0006C97C File Offset: 0x0006AB7C
		public bool HasStarted { get; protected set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0006C985 File Offset: 0x0006AB85
		public virtual int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0006C98D File Offset: 0x0006AB8D
		protected NPCMovement movement
		{
			get
			{
				return this.npc.Movement;
			}
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0006C99A File Offset: 0x0006AB9A
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCAction_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0006C9AE File Offset: 0x0006ABAE
		protected override void OnValidate()
		{
			base.OnValidate();
			this.GetReferences();
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0006C9BC File Offset: 0x0006ABBC
		private void GetReferences()
		{
			if (this.npc == null)
			{
				this.npc = base.GetComponentInParent<NPC>();
			}
			if (this.schedule == null)
			{
				this.schedule = base.GetComponentInParent<NPCScheduleManager>();
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0006C9F2 File Offset: 0x0006ABF2
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPassed));
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0006CA1C File Offset: 0x0006AC1C
		public virtual void Started()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " started");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0006CA6C File Offset: 0x0006AC6C
		public virtual void LateStarted()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " late started");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0006CABC File Offset: 0x0006ACBC
		public virtual void JumpTo()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " jumped to");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			this.HasStarted = true;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0006CB0C File Offset: 0x0006AD0C
		public virtual void End()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " ended");
			}
			this.IsActive = false;
			this.schedule.ActiveAction = null;
			this.HasStarted = false;
			if (this.onEnded != null)
			{
				this.onEnded();
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0006CB70 File Offset: 0x0006AD70
		public virtual void Interrupt()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " interrupted");
			}
			this.IsActive = false;
			this.schedule.ActiveAction = null;
			if (!this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Add(this);
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0006CBDC File Offset: 0x0006ADDC
		public virtual void Resume()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " resumed");
			}
			this.IsActive = true;
			this.schedule.ActiveAction = this;
			if (this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Remove(this);
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0006CC4C File Offset: 0x0006AE4C
		public virtual void ResumeFailed()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(this.GetName() + " resume failed");
			}
			this.HasStarted = false;
			if (this.schedule.PendingActions.Contains(this))
			{
				this.schedule.PendingActions.Remove(this);
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0006CCAD File Offset: 0x0006AEAD
		public virtual void Skipped()
		{
			this.GetReferences();
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log(base.gameObject.name + " skipped");
			}
			this.IsActive = false;
			this.HasStarted = false;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveUpdate()
		{
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveMinPassed()
		{
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0006CCEA File Offset: 0x0006AEEA
		public virtual void PendingMinPassed()
		{
			if (this.HasStarted && !this.IsActive && !NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.GetEndTime()))
			{
				this.ResumeFailed();
			}
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void MinPassed()
		{
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0006CD1A File Offset: 0x0006AF1A
		public virtual bool ShouldStart()
		{
			return base.gameObject.activeInHierarchy;
		}

		// Token: 0x060018C1 RID: 6337
		public abstract string GetName();

		// Token: 0x060018C2 RID: 6338
		public abstract string GetTimeDescription();

		// Token: 0x060018C3 RID: 6339
		public abstract int GetEndTime();

		// Token: 0x060018C4 RID: 6340 RVA: 0x0006CD2C File Offset: 0x0006AF2C
		protected void SetDestination(Vector3 position, bool teleportIfFail = true)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (teleportIfFail && this.consecutivePathingFailures >= 5 && !this.movement.CanGetTo(position, 1f))
			{
				Console.LogWarning(this.npc.fullName + " too many pathing failures. Warping to " + position.ToString(), null);
				this.movement.Warp(position);
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.movement.SetDestination(position, new Action<NPCMovement.WalkResult>(this.WalkCallback), 1f, 1f);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0006CDC0 File Offset: 0x0006AFC0
		protected virtual void WalkCallback(NPCMovement.WalkResult result)
		{
			if (!this.IsActive)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Console.Log("Walk callback result: " + result.ToString(), null);
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0006CE1A File Offset: 0x0006B01A
		public virtual void SetStartTime(int startTime)
		{
			this.StartTime = startTime;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0006CE23 File Offset: 0x0006B023
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0006CE36 File Offset: 0x0006B036
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCActionAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0006CE49 File Offset: 0x0006B049
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0006CE57 File Offset: 0x0006B057
		protected virtual void dll()
		{
			this.GetReferences();
		}

		// Token: 0x040015C8 RID: 5576
		public const int MAX_CONSECUTIVE_PATHING_FAILURES = 5;

		// Token: 0x040015CB RID: 5579
		[SerializeField]
		protected int priority;

		// Token: 0x040015CC RID: 5580
		[Header("Timing Settings")]
		public int StartTime;

		// Token: 0x040015CD RID: 5581
		protected NPC npc;

		// Token: 0x040015CE RID: 5582
		protected NPCScheduleManager schedule;

		// Token: 0x040015CF RID: 5583
		public Action onEnded;

		// Token: 0x040015D0 RID: 5584
		protected int consecutivePathingFailures;

		// Token: 0x040015D1 RID: 5585
		private bool dll_Excuted;

		// Token: 0x040015D2 RID: 5586
		private bool dll_Excuted;
	}
}
