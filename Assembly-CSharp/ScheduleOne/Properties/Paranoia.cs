using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000316 RID: 790
	[CreateAssetMenu(fileName = "Paranoia", menuName = "Properties/Paranoia Property")]
	public class Paranoia : Property
	{
		// Token: 0x06001171 RID: 4465 RVA: 0x0004CC52 File Offset: 0x0004AE52
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "paranoia", 0f, 0);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0004CC74 File Offset: 0x0004AE74
		public override void ApplyToPlayer(Player player)
		{
			player.Paranoid = true;
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "paranoia", 0f, 0);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0004CC9D File Offset: 0x0004AE9D
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride("paranoia");
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0004CCB4 File Offset: 0x0004AEB4
		public override void ClearFromPlayer(Player player)
		{
			player.Paranoid = false;
			player.Avatar.EmotionManager.RemoveEmotionOverride("paranoia");
		}
	}
}
