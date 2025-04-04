using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200030F RID: 783
	[CreateAssetMenu(fileName = "Foggy", menuName = "Properties/Foggy Property")]
	public class Foggy : Property
	{
		// Token: 0x0600114D RID: 4429 RVA: 0x0004C7BE File Offset: 0x0004A9BE
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFoggy(true, true);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0004C7D2 File Offset: 0x0004A9D2
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetFoggy(true, true);
			if (player.IsLocalPlayer)
			{
				Singleton<EnvironmentFX>.Instance.FogEndDistanceController.AddOverride(0.1f, this.Tier, base.name);
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0004C80E File Offset: 0x0004AA0E
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFoggy(false, true);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0004C822 File Offset: 0x0004AA22
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetFoggy(false, true);
			if (player.IsLocalPlayer)
			{
				Singleton<EnvironmentFX>.Instance.FogEndDistanceController.RemoveOverride(base.name);
			}
		}
	}
}
