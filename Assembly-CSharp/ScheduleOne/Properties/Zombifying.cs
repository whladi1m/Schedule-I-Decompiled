using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200032A RID: 810
	[CreateAssetMenu(fileName = "Zombifying", menuName = "Properties/Zombifying Property")]
	public class Zombifying : Property
	{
		// Token: 0x060011CB RID: 4555 RVA: 0x0004D828 File Offset: 0x0004BA28
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZombified(true, true);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(0.5f);
			npc.VoiceOverEmitter.SetDatabase(this.zombieVODatabase, false);
			npc.PlayVO(EVOLineType.Grunt);
			npc.Movement.SpeedController.SpeedMultiplier = 0.4f;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004D886 File Offset: 0x0004BA86
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetZombified(true, true);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004D89C File Offset: 0x0004BA9C
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZombified(false, true);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1f);
			npc.VoiceOverEmitter.ResetDatabase();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0004D8EB File Offset: 0x0004BAEB
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetZombified(false, true);
		}

		// Token: 0x0400115B RID: 4443
		public VODatabase zombieVODatabase;
	}
}
