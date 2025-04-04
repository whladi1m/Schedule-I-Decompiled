using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000305 RID: 773
	[CreateAssetMenu(fileName = "AntiGravity", menuName = "Properties/AntiGravity Property")]
	public class AntiGravity : Property
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x0004BE65 File Offset: 0x0004A065
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SetGravityMultiplier(0.3f);
			npc.Avatar.Effects.SetAntiGrav(true, true);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0004BE89 File Offset: 0x0004A089
		public override void ApplyToPlayer(Player player)
		{
			player.SetGravityMultiplier(0.3f);
			player.Avatar.Effects.SetAntiGrav(true, true);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0004BEA8 File Offset: 0x0004A0A8
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SetGravityMultiplier(1f);
			npc.Avatar.Effects.SetAntiGrav(false, true);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0004BECC File Offset: 0x0004A0CC
		public override void ClearFromPlayer(Player player)
		{
			player.SetGravityMultiplier(1f);
			player.Avatar.Effects.SetAntiGrav(false, true);
		}

		// Token: 0x0400112E RID: 4398
		public const float GravityMultiplier = 0.3f;
	}
}
