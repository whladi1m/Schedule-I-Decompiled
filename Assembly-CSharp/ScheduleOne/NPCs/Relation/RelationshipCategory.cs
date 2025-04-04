using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x02000481 RID: 1153
	public class RelationshipCategory
	{
		// Token: 0x060019AC RID: 6572 RVA: 0x0006F77A File Offset: 0x0006D97A
		public static ERelationshipCategory GetCategory(float delta)
		{
			if (delta >= 4f)
			{
				return ERelationshipCategory.Loyal;
			}
			if (delta >= 3f)
			{
				return ERelationshipCategory.Friendly;
			}
			if (delta >= 2f)
			{
				return ERelationshipCategory.Neutral;
			}
			if (delta >= 1f)
			{
				return ERelationshipCategory.Unfriendly;
			}
			return ERelationshipCategory.Hostile;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0006F7A8 File Offset: 0x0006D9A8
		public static Color32 GetColor(ERelationshipCategory category)
		{
			switch (category)
			{
			case ERelationshipCategory.Hostile:
				return RelationshipCategory.Hostile_Color;
			case ERelationshipCategory.Unfriendly:
				return RelationshipCategory.Unfriendly_Color;
			case ERelationshipCategory.Neutral:
				return RelationshipCategory.Neutral_Color;
			case ERelationshipCategory.Friendly:
				return RelationshipCategory.Friendly_Color;
			case ERelationshipCategory.Loyal:
				return RelationshipCategory.Loyal_Color;
			default:
				Console.LogError("Failed to find relationship category color", null);
				return Color.white;
			}
		}

		// Token: 0x04001627 RID: 5671
		public static Color32 Hostile_Color = new Color32(173, 63, 63, byte.MaxValue);

		// Token: 0x04001628 RID: 5672
		public static Color32 Unfriendly_Color = new Color32(227, 136, 55, byte.MaxValue);

		// Token: 0x04001629 RID: 5673
		public static Color32 Neutral_Color = new Color32(208, 208, 208, byte.MaxValue);

		// Token: 0x0400162A RID: 5674
		public static Color32 Friendly_Color = new Color32(61, 181, 243, byte.MaxValue);

		// Token: 0x0400162B RID: 5675
		public static Color32 Loyal_Color = new Color32(63, 211, 63, byte.MaxValue);
	}
}
