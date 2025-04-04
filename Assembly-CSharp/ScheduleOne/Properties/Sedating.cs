using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000319 RID: 793
	[CreateAssetMenu(fileName = "Sedating", menuName = "Properties/Sedating Property")]
	public class Sedating : Property
	{
		// Token: 0x06001180 RID: 4480 RVA: 0x0004CEA4 File Offset: 0x0004B0A4
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.18f,
				topLidOpen = 0.18f
			});
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 0.6f;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0004CF08 File Offset: 0x0004B108
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.18f,
				topLidOpen = 0.18f
			});
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.7f, 6, "sedating");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(-8f, this.Tier, "sedating");
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.AddOverride(0.8f, this.Tier, "sedating");
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0004C03F File Offset: 0x0004A23F
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0004CFB8 File Offset: 0x0004B1B8
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Eyes.ResetEyeLids();
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("sedating");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("sedating");
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.RemoveOverride("sedating");
			}
		}
	}
}
