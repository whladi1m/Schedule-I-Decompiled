using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200070D RID: 1805
	public class EndCutscene : Cutscene
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x000CB333 File Offset: 0x000C9533
		public override void Play()
		{
			base.Play();
			this.Avatar.LoadAvatarSettings(Player.Local.Avatar.CurrentSettings);
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000CB355 File Offset: 0x000C9555
		public void StandUp()
		{
			if (this.onStandUp != null)
			{
				Console.Log("StandUp", null);
				this.onStandUp.Invoke();
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000CB375 File Offset: 0x000C9575
		public void RunStart()
		{
			if (this.onRunStart != null)
			{
				Console.Log("RunStart", null);
				this.onRunStart.Invoke();
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000CB395 File Offset: 0x000C9595
		public void EngineStart()
		{
			if (this.onEngineStart != null)
			{
				Console.Log("EngineStart", null);
				this.onEngineStart.Invoke();
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000CB3B5 File Offset: 0x000C95B5
		public void On3rdPerson()
		{
			this.Avatar.gameObject.SetActive(true);
			this.Avatar.Anim.SetBool("Sitting", true);
		}

		// Token: 0x040022FA RID: 8954
		public UnityEvent onStandUp;

		// Token: 0x040022FB RID: 8955
		public UnityEvent onRunStart;

		// Token: 0x040022FC RID: 8956
		public UnityEvent onEngineStart;

		// Token: 0x040022FD RID: 8957
		public Avatar Avatar;
	}
}
