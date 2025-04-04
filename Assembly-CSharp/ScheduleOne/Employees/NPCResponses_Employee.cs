using System;
using FishNet;
using ScheduleOne.Combat;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Employees
{
	// Token: 0x0200063E RID: 1598
	public class NPCResponses_Employee : NPCResponses
	{
		// Token: 0x06002B45 RID: 11077 RVA: 0x000B20F2 File Offset: 0x000B02F2
		protected override void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToFirstNonLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000B2103 File Offset: 0x000B0303
		protected override void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000B2114 File Offset: 0x000B0314
		protected override void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToRepeatedNonLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000B2128 File Offset: 0x000B0328
		private void Ow(Player perpetrator)
		{
			base.npc.dialogueHandler.PlayReaction("hurt", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "hurt", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.Enable_Networked(null);
			}
		}
	}
}
