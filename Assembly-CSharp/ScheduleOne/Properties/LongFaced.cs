using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000311 RID: 785
	[CreateAssetMenu(fileName = "LongFaced", menuName = "Properties/LongFaced Property")]
	public class LongFaced : Property
	{
		// Token: 0x06001158 RID: 4440 RVA: 0x0004C8CD File Offset: 0x0004AACD
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGiraffeActive(true, true);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0004C8E1 File Offset: 0x0004AAE1
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGiraffeActive(true, true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(15f, this.Tier, "longfaced");
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0004C91C File Offset: 0x0004AB1C
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGiraffeActive(false, true);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0004C930 File Offset: 0x0004AB30
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGiraffeActive(false, true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("longfaced");
			}
		}
	}
}
