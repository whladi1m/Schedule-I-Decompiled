using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000315 RID: 789
	[CreateAssetMenu(fileName = "Lethal", menuName = "Properties/Lethal Property")]
	public class Lethal : Property
	{
		// Token: 0x0600116C RID: 4460 RVA: 0x0004CA68 File Offset: 0x0004AC68
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSicklySkinColor(true);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "Sickly", 0f, this.Tier);
			npc.Avatar.Effects.TriggerSick(true);
			npc.Health.SetAfflictedWithLethalEffect(true);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0004CAC8 File Offset: 0x0004ACC8
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetSicklySkinColor(true);
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "Sickly", 0f, this.Tier);
			player.Avatar.Effects.TriggerSick(true);
			player.Health.SetAfflictedWithLethalEffect(true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.AddOverride(0.7f, this.Tier, "sickly");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.AddOverride(1f, this.Tier, "sickly");
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0004CB78 File Offset: 0x0004AD78
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSicklySkinColor(false);
			npc.Avatar.EmotionManager.RemoveEmotionOverride("Sickly");
			npc.Avatar.Effects.TriggerSick(true);
			npc.Health.SetAfflictedWithLethalEffect(false);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0004CBC8 File Offset: 0x0004ADC8
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetSicklySkinColor(false);
			player.Avatar.EmotionManager.RemoveEmotionOverride("Sickly");
			player.Avatar.Effects.TriggerSick(true);
			player.Health.SetAfflictedWithLethalEffect(false);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.RemoveOverride("sickly");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.RemoveOverride("sickly");
			}
		}

		// Token: 0x04001138 RID: 4408
		public const float HEALTH_DRAIN_PLAYER = 15f;

		// Token: 0x04001139 RID: 4409
		public const float HEALTH_DRAIN_NPC = 15f;
	}
}
