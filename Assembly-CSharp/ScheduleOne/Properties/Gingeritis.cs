using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000310 RID: 784
	[CreateAssetMenu(fileName = "Gingeritis", menuName = "Properties/Gingeritis Property")]
	public class Gingeritis : Property
	{
		// Token: 0x06001152 RID: 4434 RVA: 0x0004C853 File Offset: 0x0004AA53
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.OverrideHairColor(Gingeritis.Color, true);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0004C870 File Offset: 0x0004AA70
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.OverrideHairColor(Gingeritis.Color, true);
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0004C88D File Offset: 0x0004AA8D
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ResetHairColor(true);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0004C8A0 File Offset: 0x0004AAA0
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ResetHairColor(true);
		}

		// Token: 0x04001136 RID: 4406
		public static Color32 Color = new Color32(198, 113, 34, byte.MaxValue);
	}
}
