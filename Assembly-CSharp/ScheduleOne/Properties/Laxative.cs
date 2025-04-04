using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000314 RID: 788
	[CreateAssetMenu(fileName = "Laxative", menuName = "Properties/Laxative Property")]
	public class Laxative : Property
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x0004CA1B File Offset: 0x0004AC1B
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.EnableLaxative(true);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004CA2E File Offset: 0x0004AC2E
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.EnableLaxative(true);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004CA41 File Offset: 0x0004AC41
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.DisableLaxative(true);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0004CA54 File Offset: 0x0004AC54
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.DisableLaxative(true);
		}
	}
}
