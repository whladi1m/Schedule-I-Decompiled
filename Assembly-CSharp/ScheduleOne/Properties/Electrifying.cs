using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200030C RID: 780
	[CreateAssetMenu(fileName = "Electrifying", menuName = "Properties/Electrifying Property")]
	public class Electrifying : Property
	{
		// Token: 0x0600113E RID: 4414 RVA: 0x0004C50C File Offset: 0x0004A70C
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZapped(true, true);
			npc.Avatar.Effects.OverrideEyeColor(this.EyeColor, 0.5f, true);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0004C53C File Offset: 0x0004A73C
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetZapped(true, true);
			player.Avatar.Effects.OverrideEyeColor(this.EyeColor, 0.5f, true);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0004C56C File Offset: 0x0004A76C
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZapped(false, true);
			npc.Avatar.Effects.ResetEyeColor(true);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0004C591 File Offset: 0x0004A791
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetZapped(false, true);
			player.Avatar.Effects.ResetEyeColor(true);
		}

		// Token: 0x04001134 RID: 4404
		public Color EyeColor;
	}
}
