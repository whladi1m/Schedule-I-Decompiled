using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000470 RID: 1136
	public class NPCEvent : NPCAction
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x0006CE72 File Offset: 0x0006B072
		public new string ActionName
		{
			get
			{
				return "Event";
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0006CE79 File Offset: 0x0006B079
		[Button]
		public void ApplyDuration()
		{
			Debug.Log("Applying duration");
			this.EndTime = ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(this.StartTime, this.Duration);
			base.GetComponentInParent<NPCScheduleManager>().InitializeActions();
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0006CEA8 File Offset: 0x0006B0A8
		[Button]
		public void ApplyEndTime()
		{
			if (this.EndTime > this.StartTime)
			{
				Debug.Log("Set duration");
				this.Duration = ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(this.EndTime) - ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(this.StartTime);
			}
			else
			{
				Debug.Log("Set duration");
				this.Duration = 1440 - ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(this.StartTime) + ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(this.EndTime);
			}
			base.GetComponentInParent<NPCScheduleManager>().InitializeActions();
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0006CF24 File Offset: 0x0006B124
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime == this.GetEndTime())
			{
				this.End();
			}
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0006CF44 File Offset: 0x0006B144
		public override void PendingMinPassed()
		{
			base.PendingMinPassed();
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0006CF4C File Offset: 0x0006B14C
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0006CF54 File Offset: 0x0006B154
		public override string GetTimeDescription()
		{
			return ScheduleOne.GameTime.TimeManager.Get12HourTime((float)this.StartTime, true) + " - " + ScheduleOne.GameTime.TimeManager.Get12HourTime((float)this.GetEndTime(), true);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0006CF7A File Offset: 0x0006B17A
		public override int GetEndTime()
		{
			return ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(this.StartTime, this.Duration);
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0006CF9D File Offset: 0x0006B19D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0006CFB6 File Offset: 0x0006B1B6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0006CFCF File Offset: 0x0006B1CF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0006CFDD File Offset: 0x0006B1DD
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015D4 RID: 5588
		public int Duration = 60;

		// Token: 0x040015D5 RID: 5589
		public int EndTime;

		// Token: 0x040015D6 RID: 5590
		private bool dll_Excuted;

		// Token: 0x040015D7 RID: 5591
		private bool dll_Excuted;
	}
}
