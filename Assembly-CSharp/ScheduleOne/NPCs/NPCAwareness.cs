using System;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200044C RID: 1100
	public class NPCAwareness : MonoBehaviour
	{
		// Token: 0x060016D9 RID: 5849 RVA: 0x00064BF8 File Offset: 0x00062DF8
		protected virtual void Awake()
		{
			this.npc = base.GetComponentInParent<NPC>();
			if (this.Responses == null)
			{
				Console.LogError("NPCAwareness doesn't have a reference to NPCResponses - responses won't be automatically connected.", null);
			}
			VisionCone visionCone = this.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.VisionEvent));
			Listener listener = this.Listener;
			listener.onNoiseHeard = (Listener.HearingEvent)Delegate.Combine(listener.onNoiseHeard, new Listener.HearingEvent(this.NoiseEvent));
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00064C78 File Offset: 0x00062E78
		public void SetAwarenessActive(bool active)
		{
			this.Listener.enabled = active;
			this.VisionCone.enabled = active;
			base.enabled = active;
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00064C9C File Offset: 0x00062E9C
		public void VisionEvent(VisionEventReceipt vEvent)
		{
			if (!base.enabled)
			{
				return;
			}
			switch (vEvent.State)
			{
			case PlayerVisualState.EVisualState.Visible:
			case PlayerVisualState.EVisualState.SearchedFor:
				break;
			case PlayerVisualState.EVisualState.Suspicious:
				if (this.onNoticedSuspiciousPlayer != null)
				{
					this.onNoticedSuspiciousPlayer.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedSuspiciousPlayer(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DisobeyingCurfew:
				if (this.onNoticedPlayerViolatingCurfew != null)
				{
					this.onNoticedPlayerViolatingCurfew.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedViolatingCurfew(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Vandalizing:
				if (this.Responses != null)
				{
					this.Responses.NoticedVandalism(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.PettyCrime:
				if (this.onNoticedPettyCrime != null)
				{
					this.onNoticedPettyCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.onNoticedGeneralCrime != null)
				{
					this.onNoticedGeneralCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedPettyCrime(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DrugDealing:
				if (this.onNoticedDrugDealing != null)
				{
					this.onNoticedDrugDealing.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.onNoticedGeneralCrime != null)
				{
					this.onNoticedGeneralCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedDrugDeal(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Wanted:
				if (this.Responses != null)
				{
					this.Responses.NoticedWantedPlayer(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Pickpocketing:
				if (this.Responses != null)
				{
					this.Responses.SawPickpocketing(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DischargingWeapon:
				if (this.Responses != null)
				{
					this.Responses.NoticePlayerDischargingWeapon(vEvent.TargetPlayer.GetComponent<Player>());
				}
				break;
			case PlayerVisualState.EVisualState.Brandishing:
				if (this.Responses != null)
				{
					this.Responses.NoticePlayerBrandishingWeapon(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00064F00 File Offset: 0x00063100
		public void NoiseEvent(NoiseEvent nEvent)
		{
			if (!base.enabled)
			{
				return;
			}
			if (nEvent.type == ENoiseType.Gunshot)
			{
				if (this.onGunshotHeard != null)
				{
					this.onGunshotHeard.Invoke(nEvent);
				}
				if (this.Responses != null)
				{
					this.Responses.GunshotHeard(nEvent);
				}
			}
			if (nEvent.type == ENoiseType.Explosion)
			{
				if (this.onExplosionHeard != null)
				{
					this.onExplosionHeard.Invoke(nEvent);
				}
				if (this.Responses != null)
				{
					this.Responses.ExplosionHeard(nEvent);
				}
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00064F84 File Offset: 0x00063184
		public void HitByCar(LandVehicle vehicle)
		{
			if (this.onHitByCar != null)
			{
				this.onHitByCar.Invoke(vehicle);
			}
			if (this.Responses != null)
			{
				this.Responses.HitByCar(vehicle);
			}
		}

		// Token: 0x040014D2 RID: 5330
		public const float PLAYER_AIM_DETECTION_RANGE = 15f;

		// Token: 0x040014D3 RID: 5331
		[Header("References")]
		public VisionCone VisionCone;

		// Token: 0x040014D4 RID: 5332
		public Listener Listener;

		// Token: 0x040014D5 RID: 5333
		public NPCResponses Responses;

		// Token: 0x040014D6 RID: 5334
		public UnityEvent<Player> onNoticedGeneralCrime;

		// Token: 0x040014D7 RID: 5335
		public UnityEvent<Player> onNoticedPettyCrime;

		// Token: 0x040014D8 RID: 5336
		public UnityEvent<Player> onNoticedDrugDealing;

		// Token: 0x040014D9 RID: 5337
		public UnityEvent<Player> onNoticedPlayerViolatingCurfew;

		// Token: 0x040014DA RID: 5338
		public UnityEvent<Player> onNoticedSuspiciousPlayer;

		// Token: 0x040014DB RID: 5339
		public UnityEvent<NoiseEvent> onGunshotHeard;

		// Token: 0x040014DC RID: 5340
		public UnityEvent<NoiseEvent> onExplosionHeard;

		// Token: 0x040014DD RID: 5341
		public UnityEvent<LandVehicle> onHitByCar;

		// Token: 0x040014DE RID: 5342
		private NPC npc;
	}
}
