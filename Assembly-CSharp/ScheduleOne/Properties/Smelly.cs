using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000321 RID: 801
	[CreateAssetMenu(fileName = "Smelly", menuName = "Properties/Smelly Property")]
	public class Smelly : Property
	{
		// Token: 0x060011A4 RID: 4516 RVA: 0x0004D27E File Offset: 0x0004B47E
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetStinkParticlesActive(true, true);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004D292 File Offset: 0x0004B492
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetStinkParticlesActive(true, true);
			if (player.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerCamera>.Instance.Flies.Play();
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004D2C2 File Offset: 0x0004B4C2
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetStinkParticlesActive(false, true);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004D2D6 File Offset: 0x0004B4D6
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetStinkParticlesActive(false, true);
			if (player.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerCamera>.Instance.Flies.Stop();
			}
		}
	}
}
