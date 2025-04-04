using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000309 RID: 777
	[CreateAssetMenu(fileName = "CalmingProperty", menuName = "Properties/Calming Property")]
	public class Calming : Property
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x0004C269 File Offset: 0x0004A469
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SpeedController.SpeedMultiplier = 0.8f;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x0004C280 File Offset: 0x0004A480
		public override void ApplyToPlayer(Player player)
		{
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.9f, 6, "calming");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(-4f, this.Tier, "calming");
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0004C2CE File Offset: 0x0004A4CE
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0004C2E5 File Offset: 0x0004A4E5
		public override void ClearFromPlayer(Player player)
		{
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("calming");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("calming");
			}
		}
	}
}
