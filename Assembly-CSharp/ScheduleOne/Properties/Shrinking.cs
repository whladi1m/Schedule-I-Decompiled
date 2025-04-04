using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031F RID: 799
	[CreateAssetMenu(fileName = "Shrinking", menuName = "Properties/Shrinking Property")]
	public class Shrinking : Property
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x0004D1E8 File Offset: 0x0004B3E8
		public override void ApplyToNPC(NPC npc)
		{
			npc.SetScale(0.8f, 1f);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1.5f);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004D20A File Offset: 0x0004B40A
		public override void ApplyToPlayer(Player player)
		{
			player.SetScale(0.8f, 1f);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004D21C File Offset: 0x0004B41C
		public override void ClearFromNPC(NPC npc)
		{
			npc.SetScale(1f, 1f);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1f);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004D23E File Offset: 0x0004B43E
		public override void ClearFromPlayer(Player player)
		{
			player.SetScale(1f, 1f);
		}

		// Token: 0x04001145 RID: 4421
		public const float Scale = 0.8f;

		// Token: 0x04001146 RID: 4422
		public const float LerpTime = 1f;
	}
}
