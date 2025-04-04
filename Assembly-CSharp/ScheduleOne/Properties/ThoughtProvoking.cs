using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000324 RID: 804
	[CreateAssetMenu(fileName = "ThoughtProvoking", menuName = "Properties/ThoughtProvoking Property")]
	public class ThoughtProvoking : Property
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x0004D3FE File Offset: 0x0004B5FE
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetBigHeadActive(true, true);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0004D412 File Offset: 0x0004B612
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetBigHeadActive(true, true);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004D426 File Offset: 0x0004B626
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetBigHeadActive(false, true);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004D43A File Offset: 0x0004B63A
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetBigHeadActive(false, true);
		}
	}
}
