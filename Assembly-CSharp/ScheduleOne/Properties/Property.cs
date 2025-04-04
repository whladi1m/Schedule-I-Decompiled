using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000327 RID: 807
	public abstract class Property : ScriptableObject
	{
		// Token: 0x060011C2 RID: 4546
		public abstract void ApplyToNPC(NPC npc);

		// Token: 0x060011C3 RID: 4547
		public abstract void ClearFromNPC(NPC npc);

		// Token: 0x060011C4 RID: 4548
		public abstract void ApplyToPlayer(Player player);

		// Token: 0x060011C5 RID: 4549
		public abstract void ClearFromPlayer(Player player);

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004D578 File Offset: 0x0004B778
		public void OnValidate()
		{
			if (this.Name == string.Empty)
			{
				this.Name = base.name;
			}
			if (this.ID == string.Empty)
			{
				this.ID = base.name.ToLower();
			}
		}

		// Token: 0x0400114A RID: 4426
		public string Name = string.Empty;

		// Token: 0x0400114B RID: 4427
		public string Description = string.Empty;

		// Token: 0x0400114C RID: 4428
		public string ID = string.Empty;

		// Token: 0x0400114D RID: 4429
		[Range(1f, 5f)]
		public int Tier = 1;

		// Token: 0x0400114E RID: 4430
		[Range(0f, 1f)]
		public float Addictiveness = 0.1f;

		// Token: 0x0400114F RID: 4431
		public Color ProductColor = Color.white;

		// Token: 0x04001150 RID: 4432
		public Color LabelColor = Color.white;

		// Token: 0x04001151 RID: 4433
		public bool ImplementedPriorMixingRework;

		// Token: 0x04001152 RID: 4434
		[Header("Value")]
		[Range(-100f, 100f)]
		public int ValueChange;

		// Token: 0x04001153 RID: 4435
		[Range(0f, 2f)]
		public float ValueMultiplier = 1f;

		// Token: 0x04001154 RID: 4436
		[Range(-1f, 1f)]
		public float AddBaseValueMultiple;

		// Token: 0x04001155 RID: 4437
		public Vector2 MixDirection = Vector2.zero;

		// Token: 0x04001156 RID: 4438
		public float MixMagnitude = 1f;
	}
}
