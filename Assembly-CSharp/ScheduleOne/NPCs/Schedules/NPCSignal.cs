using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000471 RID: 1137
	public class NPCSignal : NPCAction
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x0006CFF1 File Offset: 0x0006B1F1
		public new string ActionName
		{
			get
			{
				return "Signal";
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x0006CFF8 File Offset: 0x0006B1F8
		// (set) Token: 0x060018DC RID: 6364 RVA: 0x0006D000 File Offset: 0x0006B200
		public bool StartedThisCycle { get; protected set; }

		// Token: 0x060018DD RID: 6365 RVA: 0x0006D009 File Offset: 0x0006B209
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0006D011 File Offset: 0x0006B211
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0006D019 File Offset: 0x0006B219
		public override string GetTimeDescription()
		{
			return ScheduleOne.GameTime.TimeManager.Get12HourTime((float)this.StartTime, true);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0006D028 File Offset: 0x0006B228
		public override int GetEndTime()
		{
			return ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(this.StartTime, this.MaxDuration);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0006D03B File Offset: 0x0006B23B
		public override void Started()
		{
			base.Started();
			this.StartedThisCycle = true;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0006D04A File Offset: 0x0006B24A
		public override void LateStarted()
		{
			base.LateStarted();
			this.StartedThisCycle = true;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0006D059 File Offset: 0x0006B259
		public override bool ShouldStart()
		{
			return !this.StartedThisCycle && base.ShouldStart();
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0006D06B File Offset: 0x0006B26B
		public override void Interrupt()
		{
			this.StartedThisCycle = false;
			base.Interrupt();
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x0006D07A File Offset: 0x0006B27A
		public override void MinPassed()
		{
			base.MinPassed();
			if (this.StartedThisCycle && !NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.GetEndTime()))
			{
				this.StartedThisCycle = false;
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0006D0B9 File Offset: 0x0006B2B9
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0006D0D2 File Offset: 0x0006B2D2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0006D0EB File Offset: 0x0006B2EB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0006D0F9 File Offset: 0x0006B2F9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015D8 RID: 5592
		public int MaxDuration = 60;

		// Token: 0x040015DA RID: 5594
		private bool dll_Excuted;

		// Token: 0x040015DB RID: 5595
		private bool dll_Excuted;
	}
}
