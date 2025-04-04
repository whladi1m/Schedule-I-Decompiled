using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000307 RID: 775
	[CreateAssetMenu(fileName = "Balding", menuName = "Properties/Balding Property")]
	public class Balding : Property
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0004C129 File Offset: 0x0004A329
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.VanishHair(true);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0004C13C File Offset: 0x0004A33C
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.VanishHair(true);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0004C14F File Offset: 0x0004A34F
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ReturnHair(true);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0004C162 File Offset: 0x0004A362
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ReturnHair(true);
		}
	}
}
