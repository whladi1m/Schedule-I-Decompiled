using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200030E RID: 782
	[CreateAssetMenu(fileName = "Explosive", menuName = "Properties/Explosive Property")]
	public class Explosive : Property
	{
		// Token: 0x06001148 RID: 4424 RVA: 0x0004C772 File Offset: 0x0004A972
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.TriggerCountdownExplosion(false);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0004C785 File Offset: 0x0004A985
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.TriggerCountdownExplosion(false);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0004C798 File Offset: 0x0004A998
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.StopCountdownExplosion(false);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0004C7AB File Offset: 0x0004A9AB
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.StopCountdownExplosion(false);
		}
	}
}
