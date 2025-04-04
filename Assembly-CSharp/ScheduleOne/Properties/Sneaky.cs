using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000322 RID: 802
	[CreateAssetMenu(fileName = "Sneaky", menuName = "Properties/Sneaky Property")]
	public class Sneaky : Property
	{
		// Token: 0x060011A9 RID: 4521 RVA: 0x000045B1 File Offset: 0x000027B1
		public override void ApplyToNPC(NPC npc)
		{
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004D306 File Offset: 0x0004B506
		public override void ApplyToPlayer(Player player)
		{
			player.Sneaky = true;
			this.visibilityAttribute = new VisibilityAttribute("sneaky", 0f, 0.6f, -1);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000045B1 File Offset: 0x000027B1
		public override void ClearFromNPC(NPC npc)
		{
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004D32A File Offset: 0x0004B52A
		public override void ClearFromPlayer(Player player)
		{
			player.Sneaky = true;
			this.visibilityAttribute.Delete();
		}

		// Token: 0x04001147 RID: 4423
		private VisibilityAttribute visibilityAttribute;
	}
}
