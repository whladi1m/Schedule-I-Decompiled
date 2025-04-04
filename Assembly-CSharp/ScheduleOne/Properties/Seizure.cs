using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031A RID: 794
	[CreateAssetMenu(fileName = "Seizure", menuName = "Properties/Seizure Property")]
	public class Seizure : Property
	{
		// Token: 0x06001185 RID: 4485 RVA: 0x0004D02C File Offset: 0x0004B22C
		public override void ApplyToNPC(NPC npc)
		{
			Seizure.<>c__DisplayClass3_0 CS$<>8__locals1 = new Seizure.<>c__DisplayClass3_0();
			CS$<>8__locals1.npc = npc;
			CS$<>8__locals1.npc.PlayVO(EVOLineType.Hurt);
			CS$<>8__locals1.npc.behaviour.RagdollBehaviour.Seizure = true;
			CS$<>8__locals1.npc.Movement.ActivateRagdoll_Server();
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ApplyToNPC>g__Wait|0());
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0004D08C File Offset: 0x0004B28C
		public override void ApplyToPlayer(Player player)
		{
			Seizure.<>c__DisplayClass4_0 CS$<>8__locals1 = new Seizure.<>c__DisplayClass4_0();
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.player.Seizure = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ApplyToPlayer>g__Wait|0());
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0004D0C3 File Offset: 0x0004B2C3
		public override void ClearFromNPC(NPC npc)
		{
			npc.behaviour.RagdollBehaviour.Seizure = false;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0004D0D6 File Offset: 0x0004B2D6
		public override void ClearFromPlayer(Player player)
		{
			player.Seizure = false;
		}

		// Token: 0x0400113A RID: 4410
		public const float CAMERA_JITTER_INTENSITY = 1f;

		// Token: 0x0400113B RID: 4411
		public const float DURATION_NPC = 60f;

		// Token: 0x0400113C RID: 4412
		public const float DURATION_PLAYER = 30f;
	}
}
