using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000323 RID: 803
	[CreateAssetMenu(fileName = "Spicy", menuName = "Properties/Spicy Property")]
	public class Spicy : Property
	{
		// Token: 0x060011AE RID: 4526 RVA: 0x0004D33E File Offset: 0x0004B53E
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFireActive(true, true);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0004D354 File Offset: 0x0004B554
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetFireActive(true, true);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride(this.TintColor, this.Tier, base.name);
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0004D3A1 File Offset: 0x0004B5A1
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFireActive(false, true);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0004D3B5 File Offset: 0x0004B5B5
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetFireActive(false, true);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride(base.name);
			}
		}

		// Token: 0x04001148 RID: 4424
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
