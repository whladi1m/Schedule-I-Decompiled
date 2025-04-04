using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BF9 RID: 3065
	public class NPCPresenceAccessZone : AccessZone
	{
		// Token: 0x060055C1 RID: 21953 RVA: 0x001686F1 File Offset: 0x001668F1
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x001686F9 File Offset: 0x001668F9
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x00168724 File Offset: 0x00166924
		protected virtual void MinPass()
		{
			if (this.TargetNPC == null)
			{
				return;
			}
			this.SetIsOpen(this.DetectionZone.bounds.Contains(this.TargetNPC.Avatar.CenterPoint));
		}

		// Token: 0x04003FAF RID: 16303
		public const float CooldownTime = 0.5f;

		// Token: 0x04003FB0 RID: 16304
		public Collider DetectionZone;

		// Token: 0x04003FB1 RID: 16305
		public NPC TargetNPC;

		// Token: 0x04003FB2 RID: 16306
		private float timeSinceNPCSensed = float.MaxValue;
	}
}
