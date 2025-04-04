using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000312 RID: 786
	[CreateAssetMenu(fileName = "Glowie", menuName = "Properties/Glowie Property")]
	public class Glowie : Property
	{
		// Token: 0x0600115D RID: 4445 RVA: 0x0004C960 File Offset: 0x0004AB60
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGlowingOn(this.GlowColor, true);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0004C979 File Offset: 0x0004AB79
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGlowingOn(this.GlowColor, true);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0004C992 File Offset: 0x0004AB92
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGlowingOff(true);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0004C9A5 File Offset: 0x0004ABA5
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGlowingOff(true);
		}

		// Token: 0x04001137 RID: 4407
		[ColorUsage(true, true)]
		[SerializeField]
		public Color GlowColor = Color.white;
	}
}
