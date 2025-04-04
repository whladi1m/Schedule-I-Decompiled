using System;
using FishNet;
using ScheduleOne.Combat;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;

namespace ScheduleOne.Police
{
	// Token: 0x02000331 RID: 817
	public class NPCResponses_Police : NPCResponses
	{
		// Token: 0x060011E4 RID: 4580 RVA: 0x0004DB9F File Offset: 0x0004BD9F
		protected override void Awake()
		{
			base.Awake();
			this.officer = (base.npc as PoliceOfficer);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004DBB8 File Offset: 0x0004BDB8
		public override void HitByCar(LandVehicle vehicle)
		{
			base.HitByCar(vehicle);
			base.npc.PlayVO(EVOLineType.Angry);
			if (vehicle.DriverPlayer != null && vehicle.DriverPlayer.IsOwner)
			{
				vehicle.DriverPlayer.CrimeData.AddCrime(new VehicularAssault(), 1);
				if (vehicle.DriverPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					vehicle.DriverPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
					return;
				}
				vehicle.DriverPlayer.CrimeData.Escalate();
			}
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0004DC40 File Offset: 0x0004BE40
		public override void NoticedDrugDeal(Player player)
		{
			base.NoticedDrugDeal(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new DrugTrafficking(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004DC9C File Offset: 0x0004BE9C
		public override void NoticedPettyCrime(Player player)
		{
			base.NoticedPettyCrime(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004DCDC File Offset: 0x0004BEDC
		public override void NoticedVandalism(Player player)
		{
			base.NoticedVandalism(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new Vandalism(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004DD38 File Offset: 0x0004BF38
		public override void SawPickpocketing(Player player)
		{
			base.SawPickpocketing(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new Theft(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004DD94 File Offset: 0x0004BF94
		public override void NoticePlayerBrandishingWeapon(Player player)
		{
			base.NoticePlayerBrandishingWeapon(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new BrandishingWeapon(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004DDF0 File Offset: 0x0004BFF0
		public override void NoticePlayerDischargingWeapon(Player player)
		{
			base.NoticePlayerDischargingWeapon(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new DischargeFirearm(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004DE4C File Offset: 0x0004C04C
		public override void NoticedWantedPlayer(Player player)
		{
			base.NoticedWantedPlayer(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.RecordLastKnownPosition(true);
				if (base.npc.CurrentVehicle != null)
				{
					(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, false);
					(base.npc as PoliceOfficer).BeginVehiclePursuit_Networked(player.NetworkObject, base.npc.CurrentVehicle.NetworkObject, true);
					return;
				}
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004DEE9 File Offset: 0x0004C0E9
		public override void NoticedSuspiciousPlayer(Player player)
		{
			base.NoticedSuspiciousPlayer(player);
			if (player.IsOwner)
			{
				(base.npc as PoliceOfficer).BeginBodySearch_Networked(player.NetworkObject);
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0004DF10 File Offset: 0x0004C110
		public override void NoticedViolatingCurfew(Player player)
		{
			base.NoticedViolatingCurfew(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new ViolatingCurfew(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				if (base.npc.CurrentVehicle != null)
				{
					(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, false);
					(base.npc as PoliceOfficer).BeginVehiclePursuit_Networked(player.NetworkObject, base.npc.CurrentVehicle.NetworkObject, true);
					return;
				}
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0004DFC4 File Offset: 0x0004C1C4
		protected override void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToFirstNonLethalAttack(perpetrator, impact);
			perpetrator.CrimeData.AddCrime(new Assault(), 1);
			if (perpetrator.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
				return;
			}
			perpetrator.CrimeData.Escalate();
			this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004E034 File Offset: 0x0004C234
		protected override void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToLethalAttack(perpetrator, impact);
			perpetrator.CrimeData.AddCrime(new DeadlyAssault(), 1);
			if (perpetrator.CrimeData.CurrentPursuitLevel < PlayerCrimeData.EPursuitLevel.Lethal)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0004E088 File Offset: 0x0004C288
		protected override void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToRepeatedNonLethalAttack(perpetrator, impact);
			if (!perpetrator.CrimeData.IsCrimeOnRecord(typeof(Assault)))
			{
				perpetrator.CrimeData.AddCrime(new Assault(), 1);
			}
			if (perpetrator.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
				return;
			}
			perpetrator.CrimeData.Escalate();
			this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004E110 File Offset: 0x0004C310
		protected override void RespondToAnnoyingImpact(Player perpetrator, Impact impact)
		{
			base.RespondToAnnoyingImpact(perpetrator, impact);
			base.npc.VoiceOverEmitter.Play(EVOLineType.Annoyed);
			base.npc.dialogueHandler.PlayReaction("annoyed", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "annoyed", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.Enable_Networked(null);
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004E1B4 File Offset: 0x0004C3B4
		public override void RespondToAimedAt(Player player)
		{
			base.RespondToAimedAt(player);
			if (player.CrimeData.CurrentPursuitLevel < PlayerCrimeData.EPursuitLevel.Lethal)
			{
				player.CrimeData.AddCrime(new Assault(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004E1E8 File Offset: 0x0004C3E8
		public override void ImpactReceived(Impact impact)
		{
			base.ImpactReceived(impact);
			if (this.officer.PursuitBehaviour.Active)
			{
				this.officer.PursuitBehaviour.ResetArrestProgress();
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004E214 File Offset: 0x0004C414
		public override void GunshotHeard(NoiseEvent gunshotSound)
		{
			base.GunshotHeard(gunshotSound);
			if (gunshotSound.source != null && gunshotSound.source.GetComponent<Player>() != null)
			{
				this.officer.behaviour.FacePlayerBehaviour.SetTarget(gunshotSound.source.GetComponent<Player>().NetworkObject, 5f);
				this.officer.behaviour.FacePlayerBehaviour.SendEnable();
			}
		}

		// Token: 0x0400116A RID: 4458
		private PoliceOfficer officer;
	}
}
