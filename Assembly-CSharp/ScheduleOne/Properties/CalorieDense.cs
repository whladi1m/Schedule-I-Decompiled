using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200030A RID: 778
	[CreateAssetMenu(fileName = "CalorieDense", menuName = "Properties/CalorieDense Property")]
	public class CalorieDense : Property
	{
		// Token: 0x06001134 RID: 4404 RVA: 0x0004C317 File Offset: 0x0004A517
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.AddAdditionalWeightOverride(1f, 6, "calorie dense", true);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0004C335 File Offset: 0x0004A535
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.AddAdditionalWeightOverride(1f, 6, "calorie dense", true);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0004C353 File Offset: 0x0004A553
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.RemoveAdditionalWeightOverride("calorie dense", true);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0004C36B File Offset: 0x0004A56B
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.RemoveAdditionalWeightOverride("calorie dense", true);
		}
	}
}
