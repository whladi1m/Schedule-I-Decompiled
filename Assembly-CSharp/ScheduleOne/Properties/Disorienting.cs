using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200030B RID: 779
	[CreateAssetMenu(fileName = "Disorienting", menuName = "Properties/Disorienting Property")]
	public class Disorienting : Property
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x0004C384 File Offset: 0x0004A584
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.Disoriented = true;
			npc.Avatar.Eyes.leftEye.AngleOffset = new Vector2(20f, 10f);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "disoriented", 0f, 0);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
		public override void ApplyToPlayer(Player player)
		{
			player.Disoriented = true;
			player.Avatar.Eyes.leftEye.AngleOffset = new Vector2(20f, 10f);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.AddOverride(0.8f, this.Tier, "disoriented");
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0004C444 File Offset: 0x0004A644
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.Disoriented = false;
			npc.Avatar.Eyes.leftEye.AngleOffset = Vector2.zero;
			npc.Avatar.Eyes.rightEye.AngleOffset = Vector2.zero;
			npc.Avatar.EmotionManager.RemoveEmotionOverride("disoriented");
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0004C4A8 File Offset: 0x0004A6A8
		public override void ClearFromPlayer(Player player)
		{
			player.Disoriented = false;
			player.Avatar.Eyes.leftEye.AngleOffset = Vector2.zero;
			player.Avatar.Eyes.rightEye.AngleOffset = Vector2.zero;
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.RemoveOverride("disoriented");
			}
		}
	}
}
