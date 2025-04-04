using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000313 RID: 787
	[CreateAssetMenu(fileName = "Jennerising", menuName = "Properties/Jennerising Property")]
	public class Jennerising : Property
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x0004C9CB File Offset: 0x0004ABCB
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGenderInverted(true, true);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004C9DF File Offset: 0x0004ABDF
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGenderInverted(true, true);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0004C9F3 File Offset: 0x0004ABF3
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGenderInverted(false, true);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004CA07 File Offset: 0x0004AC07
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGenderInverted(false, true);
		}
	}
}
