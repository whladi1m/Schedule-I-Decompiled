using System;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x02000488 RID: 1160
	public class DrinkItem : MonoBehaviour
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0006FD7B File Offset: 0x0006DF7B
		// (set) Token: 0x060019D3 RID: 6611 RVA: 0x0006FD83 File Offset: 0x0006DF83
		public bool active { get; protected set; }

		// Token: 0x060019D4 RID: 6612 RVA: 0x0006FD8C File Offset: 0x0006DF8C
		private void Awake()
		{
			if (this.Npc == null)
			{
				this.Npc = base.GetComponentInParent<NPC>();
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0006FDA8 File Offset: 0x0006DFA8
		public void Begin()
		{
			this.active = true;
			this.Npc.SetEquippable_Return(this.DrinkPrefab.AssetPath);
			this.Npc.Avatar.Anim.SetBool("Drinking", true);
			this.Npc.Avatar.LookController.OverrideIKWeight(0.3f);
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0006FE08 File Offset: 0x0006E008
		public void End()
		{
			this.active = false;
			this.Npc.Avatar.Anim.SetBool("Drinking", false);
			this.Npc.Avatar.LookController.ResetIKWeight();
			this.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x04001640 RID: 5696
		public NPC Npc;

		// Token: 0x04001641 RID: 5697
		public AvatarEquippable DrinkPrefab;
	}
}
