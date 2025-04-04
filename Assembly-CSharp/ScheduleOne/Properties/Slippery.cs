using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000320 RID: 800
	[CreateAssetMenu(fileName = "Slippery", menuName = "Properties/Slippery Property")]
	public class Slippery : Property
	{
		// Token: 0x0600119F RID: 4511 RVA: 0x0004D250 File Offset: 0x0004B450
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SlipperyMode = true;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0004D25E File Offset: 0x0004B45E
		public override void ApplyToPlayer(Player player)
		{
			player.Slippery = true;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004D267 File Offset: 0x0004B467
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SlipperyMode = false;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004D275 File Offset: 0x0004B475
		public override void ClearFromPlayer(Player player)
		{
			player.Slippery = false;
		}
	}
}
