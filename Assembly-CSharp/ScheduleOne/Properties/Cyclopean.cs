using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000301 RID: 769
	[CreateAssetMenu(fileName = "Cyclopean", menuName = "Properties/Cyclopean Property")]
	public class Cyclopean : Property
	{
		// Token: 0x06001107 RID: 4359 RVA: 0x0004BE0D File Offset: 0x0004A00D
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetCyclopean(true, true);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0004BE21 File Offset: 0x0004A021
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetCyclopean(true, true);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0004BE35 File Offset: 0x0004A035
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetCyclopean(false, true);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0004BE49 File Offset: 0x0004A049
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetCyclopean(false, true);
		}
	}
}
