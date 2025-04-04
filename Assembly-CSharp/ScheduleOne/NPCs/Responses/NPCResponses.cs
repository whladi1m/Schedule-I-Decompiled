using System;
using ScheduleOne.Combat;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Actions;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Responses
{
	// Token: 0x0200047C RID: 1148
	public class NPCResponses : MonoBehaviour
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0006EB9C File Offset: 0x0006CD9C
		// (set) Token: 0x06001987 RID: 6535 RVA: 0x0006EBA4 File Offset: 0x0006CDA4
		private protected NPC npc { protected get; private set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0006EBAD File Offset: 0x0006CDAD
		protected NPCActions actions
		{
			get
			{
				return this.npc.actions;
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0006EBBA File Offset: 0x0006CDBA
		protected virtual void Awake()
		{
			this.npc = base.GetComponentInParent<NPC>();
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0006EBC8 File Offset: 0x0006CDC8
		protected virtual void Update()
		{
			this.timeSinceLastImpact += Time.deltaTime;
			this.timeSinceAimedAt += Time.deltaTime;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void GunshotHeard(NoiseEvent gunshotSound)
		{
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ExplosionHeard(NoiseEvent explosionSound)
		{
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedPettyCrime(Player player)
		{
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedVandalism(Player player)
		{
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void SawPickpocketing(Player player)
		{
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticePlayerBrandishingWeapon(Player player)
		{
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticePlayerDischargingWeapon(Player player)
		{
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0006EBEE File Offset: 0x0006CDEE
		public virtual void PlayerFailedPickpocket(Player player)
		{
			if (this.npc.RelationData.Unlocked)
			{
				this.npc.RelationData.ChangeRelationship(0.25f, true);
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedDrugDeal(Player player)
		{
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedViolatingCurfew(Player player)
		{
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedWantedPlayer(Player player)
		{
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedSuspiciousPlayer(Player player)
		{
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0006EC18 File Offset: 0x0006CE18
		public virtual void HitByCar(LandVehicle vehicle)
		{
			if (vehicle.DriverPlayer != null && this.npc.Movement.timeSinceHitByCar > 2f)
			{
				if (vehicle.DriverPlayer.CrimeData.CurrentPursuitLevel > PlayerCrimeData.EPursuitLevel.None)
				{
					vehicle.DriverPlayer.CrimeData.AddCrime(new VehicularAssault(), 1);
				}
				else
				{
					vehicle.DriverPlayer.CrimeData.RecordVehicleCollision(this.npc);
				}
				this.npc.Avatar.EmotionManager.AddEmotionOverride("Angry", "hitbycar", 5f, 1);
				this.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "hitbycar1", 20f, 0);
				this.npc.PlayVO(EVOLineType.Hurt);
			}
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0006ECE8 File Offset: 0x0006CEE8
		public virtual void ImpactReceived(Impact impact)
		{
			if (!this.npc.IsConscious)
			{
				this.timeSinceLastImpact = 0f;
				return;
			}
			this.npc.VoiceOverEmitter.Play(EVOLineType.Hurt);
			Player perpetrator2;
			if (impact.ImpactForce > 50f || impact.ImpactDamage > 10f)
			{
				Player perpetrator;
				if (impact.IsPlayerImpact(out perpetrator))
				{
					if (Impact.IsLethal(impact.ImpactType))
					{
						this.RespondToLethalAttack(perpetrator, impact);
					}
					else if (this.timeSinceLastImpact < 20f)
					{
						this.RespondToRepeatedNonLethalAttack(perpetrator, impact);
					}
					else
					{
						this.RespondToFirstNonLethalAttack(perpetrator, impact);
					}
				}
			}
			else if (impact.IsPlayerImpact(out perpetrator2))
			{
				this.RespondToAnnoyingImpact(perpetrator2, impact);
			}
			this.timeSinceLastImpact = 0f;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0006ED98 File Offset: 0x0006CF98
		protected virtual void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(0.25f, true);
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0006EDBD File Offset: 0x0006CFBD
		protected virtual void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-0.25f, true);
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0006EDE2 File Offset: 0x0006CFE2
		protected virtual void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-1f, true);
			}
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void RespondToAnnoyingImpact(Player perpetrator, Impact impact)
		{
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0006EE07 File Offset: 0x0006D007
		public virtual void RespondToAimedAt(Player player)
		{
			if (this.timeSinceAimedAt > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-0.5f, true);
			}
			this.timeSinceAimedAt = 0f;
		}

		// Token: 0x0400160C RID: 5644
		public const float ASSAULT_RELATIONSHIPCHANGE = -0.25f;

		// Token: 0x0400160D RID: 5645
		public const float DEADLYASSAULT_RELATIONSHIPCHANGE = -1f;

		// Token: 0x0400160E RID: 5646
		public const float AIMED_AT_RELATIONSHIPCHANGE = -0.5f;

		// Token: 0x0400160F RID: 5647
		public const float PICKPOCKET_RELATIONSHIPCHANGE = -0.25f;

		// Token: 0x04001611 RID: 5649
		protected float timeSinceLastImpact = 100f;

		// Token: 0x04001612 RID: 5650
		protected float timeSinceAimedAt = 100f;
	}
}
