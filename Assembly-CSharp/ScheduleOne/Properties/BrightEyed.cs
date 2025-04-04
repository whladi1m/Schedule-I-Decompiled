using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000308 RID: 776
	[CreateAssetMenu(fileName = "BrightEyed", menuName = "Properties/BrightEyed Property")]
	public class BrightEyed : Property
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x0004C175 File Offset: 0x0004A375
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.OverrideEyeColor(this.EyeColor, this.Emission, true);
			npc.Avatar.Effects.SetEyeLightEmission(this.LightIntensity, this.EyeColor, true);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004C1B1 File Offset: 0x0004A3B1
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.OverrideEyeColor(this.EyeColor, this.Emission, true);
			player.Avatar.Effects.SetEyeLightEmission(this.LightIntensity, this.EyeColor, true);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004C1ED File Offset: 0x0004A3ED
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ResetEyeColor(true);
			npc.Avatar.Effects.SetEyeLightEmission(0f, this.EyeColor, true);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004C21C File Offset: 0x0004A41C
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ResetEyeColor(true);
			player.Avatar.Effects.SetEyeLightEmission(0f, this.EyeColor, true);
		}

		// Token: 0x04001131 RID: 4401
		public Color EyeColor;

		// Token: 0x04001132 RID: 4402
		public float Emission = 0.5f;

		// Token: 0x04001133 RID: 4403
		public float LightIntensity = 1f;
	}
}
