using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000326 RID: 806
	[CreateAssetMenu(fileName = "TropicThunder", menuName = "Properties/TropicThunder Property")]
	public class TropicThunder : Property
	{
		// Token: 0x060011BD RID: 4541 RVA: 0x0004D525 File Offset: 0x0004B725
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSkinColorInverted(true, true);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004D539 File Offset: 0x0004B739
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetSkinColorInverted(true, true);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004D54D File Offset: 0x0004B74D
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSkinColorInverted(false, true);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004D561 File Offset: 0x0004B761
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetSkinColorInverted(false, true);
		}
	}
}
