using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000325 RID: 805
	[CreateAssetMenu(fileName = "Toxic", menuName = "Properties/Toxic Property")]
	public class Toxic : Property
	{
		// Token: 0x060011B8 RID: 4536 RVA: 0x0004D44E File Offset: 0x0004B64E
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.TriggerSick(true);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "toxic", 30f, 1);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004D484 File Offset: 0x0004B684
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.TriggerSick(true);
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "toxic", 30f, 1);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride(this.TintColor, this.Tier, "Toxic");
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000045B1 File Offset: 0x000027B1
		public override void ClearFromNPC(NPC npc)
		{
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004D4EF File Offset: 0x0004B6EF
		public override void ClearFromPlayer(Player player)
		{
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride("Toxic");
			}
		}

		// Token: 0x04001149 RID: 4425
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
