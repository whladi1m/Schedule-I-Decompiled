using System;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x02000489 RID: 1161
	public class HoldItem : MonoBehaviour
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x0006FE5D File Offset: 0x0006E05D
		// (set) Token: 0x060019D9 RID: 6617 RVA: 0x0006FE65 File Offset: 0x0006E065
		public bool active { get; protected set; }

		// Token: 0x060019DA RID: 6618 RVA: 0x0006FE6E File Offset: 0x0006E06E
		public void Begin()
		{
			this.active = true;
			this.Npc.SetEquippable_Return(this.Equippable.AssetPath);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0006FE8E File Offset: 0x0006E08E
		private void Update()
		{
			bool active = this.active;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0006FE97 File Offset: 0x0006E097
		public void End()
		{
			this.active = false;
			this.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x04001643 RID: 5699
		public NPC Npc;

		// Token: 0x04001644 RID: 5700
		public AvatarEquippable Equippable;
	}
}
