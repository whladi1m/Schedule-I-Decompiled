using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EPOOutline;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Doors;
using ScheduleOne.Economy;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Messaging;
using ScheduleOne.NPCs.Actions;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000446 RID: 1094
	[RequireComponent(typeof(NPCHealth))]
	public class NPC : NetworkBehaviour, IGUIDRegisterable, ISaveable, IDamageable
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0006081B File Offset: 0x0005EA1B
		public string fullName
		{
			get
			{
				if (this.hasLastName)
				{
					return this.FirstName + " " + this.LastName;
				}
				return this.FirstName;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x00060842 File Offset: 0x0005EA42
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x0006084A File Offset: 0x0005EA4A
		public float Scale { get; private set; } = 1f;

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00060853 File Offset: 0x0005EA53
		public bool IsConscious
		{
			get
			{
				return this.Health.Health > 0f && !this.behaviour.UnconsciousBehaviour.Active && !this.behaviour.DeadBehaviour.Active;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0006088E File Offset: 0x0005EA8E
		public NPCMovement Movement
		{
			get
			{
				return this.movement;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x00060896 File Offset: 0x0005EA96
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x0006089E File Offset: 0x0005EA9E
		public NPCInventory Inventory { get; protected set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x000608A7 File Offset: 0x0005EAA7
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x000608AF File Offset: 0x0005EAAF
		public LandVehicle CurrentVehicle { get; protected set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x000608B8 File Offset: 0x0005EAB8
		public bool IsInVehicle
		{
			get
			{
				return this.CurrentVehicle != null;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x000608C6 File Offset: 0x0005EAC6
		public bool isInBuilding
		{
			get
			{
				return this.CurrentBuilding != null;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x000608D4 File Offset: 0x0005EAD4
		// (set) Token: 0x060015F1 RID: 5617 RVA: 0x000608DC File Offset: 0x0005EADC
		public NPCEnterableBuilding CurrentBuilding { get; protected set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x000608E5 File Offset: 0x0005EAE5
		// (set) Token: 0x060015F3 RID: 5619 RVA: 0x000608ED File Offset: 0x0005EAED
		public StaticDoor LastEnteredDoor { get; set; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x000608F6 File Offset: 0x0005EAF6
		// (set) Token: 0x060015F5 RID: 5621 RVA: 0x000608FE File Offset: 0x0005EAFE
		public MSGConversation MSGConversation { get; protected set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00060907 File Offset: 0x0005EB07
		public string SaveFolderName
		{
			get
			{
				return this.fullName;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x0006090F File Offset: 0x0005EB0F
		public string SaveFileName
		{
			get
			{
				return "NPC";
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x00060916 File Offset: 0x0005EB16
		// (set) Token: 0x060015FB RID: 5627 RVA: 0x0006091E File Offset: 0x0005EB1E
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Relationship",
			"MessageConversation"
		};

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x00060927 File Offset: 0x0005EB27
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x0006092F File Offset: 0x0005EB2F
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00060938 File Offset: 0x0005EB38
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x00060940 File Offset: 0x0005EB40
		public bool HasChanged { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x00060949 File Offset: 0x0005EB49
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x00060951 File Offset: 0x0005EB51
		public Guid GUID { get; protected set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x0006095A File Offset: 0x0005EB5A
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x00060962 File Offset: 0x0005EB62
		public bool isVisible { get; protected set; } = true;

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x0006096B File Offset: 0x0005EB6B
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x00060973 File Offset: 0x0005EB73
		public bool isUnsettled { get; protected set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x0006097C File Offset: 0x0005EB7C
		public bool IsPanicked
		{
			get
			{
				return this.timeSincePanicked < 20f;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x0006098B File Offset: 0x0005EB8B
		// (set) Token: 0x06001608 RID: 5640 RVA: 0x00060993 File Offset: 0x0005EB93
		public float timeSincePanicked { get; protected set; } = 1000f;

		// Token: 0x06001609 RID: 5641 RVA: 0x0006099C File Offset: 0x0005EB9C
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPC_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x000609BB File Offset: 0x0005EBBB
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x000609CA File Offset: 0x0005EBCA
		private void PlayerSpawned()
		{
			this.CreateMessageConversation();
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x000609D2 File Offset: 0x0005EBD2
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x000609DC File Offset: 0x0005EBDC
		protected virtual void CreateMessageConversation()
		{
			if (this.MSGConversation != null)
			{
				Console.LogWarning("Message conversation already exists for " + this.fullName, null);
				return;
			}
			this.MSGConversation = new MSGConversation(this, this.fullName);
			this.MSGConversation.SetCategories(this.ConversationCategories);
			if (this.onConversationCreated != null)
			{
				this.onConversationCreated();
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00060A3E File Offset: 0x0005EC3E
		public void SendTextMessage(string message)
		{
			this.MSGConversation.SendMessage(new Message(message, Message.ESenderType.Other, true, UnityEngine.Random.Range(int.MinValue, int.MaxValue)), true, true);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00060A64 File Offset: 0x0005EC64
		protected override void OnValidate()
		{
			base.OnValidate();
			if (base.gameObject.scene.name == null || base.gameObject.scene.name == base.gameObject.name)
			{
				return;
			}
			if (this.ID == string.Empty)
			{
				Console.LogWarning("NPC ID is empty (" + base.gameObject.name + ")", null);
			}
			this.GetHealth();
			if (this.VoiceOverEmitter == null)
			{
				this.VoiceOverEmitter = this.Avatar.HeadBone.GetComponentInChildren<VOEmitter>();
			}
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00060B0E File Offset: 0x0005ED0E
		private void GetHealth()
		{
			if (this.Health == null)
			{
				this.Health = base.GetComponent<NPCHealth>();
				if (this.Health == null)
				{
					this.Health = base.gameObject.AddComponent<NPCHealth>();
				}
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00060B4C File Offset: 0x0005ED4C
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			if (this.GUID == Guid.Empty)
			{
				if (!GUIDManager.IsGUIDValid(this.BakedGUID))
				{
					Console.LogWarning(base.gameObject.name + "'s baked GUID is not valid! Choosing random GUID", null);
					this.BakedGUID = GUIDManager.GenerateUniqueGUID().ToString();
				}
				this.GUID = new Guid(this.BakedGUID);
				GUIDManager.RegisterObject(this);
			}
			base.transform.SetParent(NetworkSingleton<NPCManager>.Instance.NPCContainer);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00060C26 File Offset: 0x0005EE26
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00060C58 File Offset: 0x0005EE58
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			if (this.RelationData.Unlocked)
			{
				this.ReceiveRelationshipData(connection, this.RelationData.RelationDelta, true);
			}
			if (this.IsInVehicle)
			{
				this.EnterVehicle(connection, this.CurrentVehicle);
			}
			if (this.isInBuilding)
			{
				this.EnterBuilding(connection, this.CurrentBuilding.GUID.ToString(), this.CurrentBuilding.Doors.IndexOf(this.LastEnteredDoor));
			}
			this.SetTransform(connection, base.transform.position, base.transform.rotation);
			if (this.Avatar.CurrentEquippable != null)
			{
				this.SetEquippable_Networked(connection, this.Avatar.CurrentEquippable.AssetPath);
			}
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00060D2F File Offset: 0x0005EF2F
		[ObserversRpc]
		private void SetTransform(NetworkConnection conn, Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Observers_SetTransform_4260003484(conn, position, rotation);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00060D44 File Offset: 0x0005EF44
		protected virtual void MinPass()
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.awareness.VisionCone.SetGeneralCrimeResponseActive(Player.PlayerList[i], this.ShouldNoticeGeneralCrime(Player.PlayerList[i]));
			}
			if (InstanceFinder.IsServer)
			{
				float timeSincePanicked = this.timeSincePanicked;
				this.timeSincePanicked += 1f;
				if (this.timeSincePanicked > 20f && timeSincePanicked <= 20f)
				{
					this.RemovePanicked();
				}
			}
			if (this.CurrentVehicle != null)
			{
				VehicleLights component = this.CurrentVehicle.GetComponent<VehicleLights>();
				if (component != null)
				{
					if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(this.headlightStartTime, this.heaedLightsEndTime))
					{
						component.headLightsOn = true;
						return;
					}
					component.headLightsOn = false;
				}
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00060E17 File Offset: 0x0005F017
		protected virtual void Update()
		{
			this.awareness.VisionCone.DisableSightUpdates = this.Avatar.Anim.IsAvatarCulled;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00060E3C File Offset: 0x0005F03C
		public virtual void SetVisible(bool visible)
		{
			this.isVisible = visible;
			this.modelContainer.gameObject.SetActive(this.isVisible);
			if (InstanceFinder.IsServer)
			{
				this.movement.Agent.enabled = this.isVisible;
			}
			if (this.onVisibilityChanged != null)
			{
				this.onVisibilityChanged(visible);
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00060E97 File Offset: 0x0005F097
		public void SetScale(float scale)
		{
			this.Scale = scale;
			this.ApplyScale();
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00060EA8 File Offset: 0x0005F0A8
		public void SetScale(float scale, float lerpTime)
		{
			NPC.<>c__DisplayClass132_0 CS$<>8__locals1 = new NPC.<>c__DisplayClass132_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (this.lerpScaleRoutine != null)
			{
				base.StopCoroutine(this.lerpScaleRoutine);
			}
			CS$<>8__locals1.startScale = this.Scale;
			this.lerpScaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__LerpScale|0());
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00060F06 File Offset: 0x0005F106
		protected virtual void ApplyScale()
		{
			base.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00060F2A File Offset: 0x0005F12A
		[ServerRpc(RequireOwnership = false)]
		public virtual void AimedAtByPlayer(NetworkObject player)
		{
			this.RpcWriter___Server_AimedAtByPlayer_3323014238(player);
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00060F36 File Offset: 0x0005F136
		public void OverrideAggression(float aggression)
		{
			this.Aggression = aggression;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00060F3F File Offset: 0x0005F13F
		public void ResetAggression()
		{
			this.Aggression = this.defaultAggression;
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00060F4D File Offset: 0x0005F14D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public virtual void SendImpact(Impact impact)
		{
			this.RpcWriter___Server_SendImpact_427288424(impact);
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00060F64 File Offset: 0x0005F164
		[ObserversRpc(RunLocally = true)]
		public virtual void ReceiveImpact(Impact impact)
		{
			this.RpcWriter___Observers_ReceiveImpact_427288424(impact);
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00060F88 File Offset: 0x0005F188
		public virtual void ProcessImpactForce(Vector3 forcePoint, Vector3 forceDirection, float force)
		{
			if (force >= 150f)
			{
				if (!this.Avatar.Ragdolled)
				{
					this.movement.ActivateRagdoll(forcePoint, forceDirection, force);
					return;
				}
			}
			else
			{
				if (force >= 100f)
				{
					this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Heavy);
					return;
				}
				if (force >= 50f)
				{
					this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Light);
				}
			}
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00060FF0 File Offset: 0x0005F1F0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void EnterVehicle(NetworkConnection connection, LandVehicle veh)
		{
			if (connection == null)
			{
				this.RpcWriter___Observers_EnterVehicle_3321926803(connection, veh);
				this.RpcLogic___EnterVehicle_3321926803(connection, veh);
			}
			else
			{
				this.RpcWriter___Target_EnterVehicle_3321926803(connection, veh);
			}
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00061034 File Offset: 0x0005F234
		[ObserversRpc(RunLocally = true)]
		public virtual void ExitVehicle()
		{
			this.RpcWriter___Observers_ExitVehicle_2166136261();
			this.RpcLogic___ExitVehicle_2166136261();
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0006104D File Offset: 0x0005F24D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendWorldspaceDialogueKey(string key, float duration)
		{
			this.RpcWriter___Server_SendWorldspaceDialogueKey_606697822(key, duration);
			this.RpcLogic___SendWorldspaceDialogueKey_606697822(key, duration);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0006106B File Offset: 0x0005F26B
		[ObserversRpc(RunLocally = true)]
		private void PlayWorldspaceDialogue(string key, float duration)
		{
			this.RpcWriter___Observers_PlayWorldspaceDialogue_606697822(key, duration);
			this.RpcLogic___PlayWorldspaceDialogue_606697822(key, duration);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00061089 File Offset: 0x0005F289
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConversant(NetworkObject player)
		{
			this.RpcWriter___Server_SetConversant_3323014238(player);
			this.RpcLogic___SetConversant_3323014238(player);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0006109F File Offset: 0x0005F29F
		private void Hovered_Internal()
		{
			this.Hovered();
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000610A7 File Offset: 0x0005F2A7
		private void Interacted_Internal()
		{
			this.Interacted();
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Hovered()
		{
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Interacted()
		{
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000610B0 File Offset: 0x0005F2B0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void EnterBuilding(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			if (connection == null)
			{
				this.RpcWriter___Observers_EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
				this.RpcLogic___EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
			}
			else
			{
				this.RpcWriter___Target_EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00061100 File Offset: 0x0005F300
		[ObserversRpc(RunLocally = true)]
		public void ExitBuilding(string buildingID = "")
		{
			this.RpcWriter___Observers_ExitBuilding_3615296227(buildingID);
			this.RpcLogic___ExitBuilding_3615296227(buildingID);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00061121 File Offset: 0x0005F321
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetEquippable_Networked(NetworkConnection conn, string assetPath)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetEquippable_Networked_2971853958(conn, assetPath);
				this.RpcLogic___SetEquippable_Networked_2971853958(conn, assetPath);
			}
			else
			{
				this.RpcWriter___Target_SetEquippable_Networked_2971853958(conn, assetPath);
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00061157 File Offset: 0x0005F357
		public AvatarEquippable SetEquippable_Networked_Return(NetworkConnection conn, string assetPath)
		{
			this.SetEquippable_Networked_ExcludeServer(conn, assetPath);
			return this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0006116D File Offset: 0x0005F36D
		public AvatarEquippable SetEquippable_Return(string assetPath)
		{
			return this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0006117B File Offset: 0x0005F37B
		[ObserversRpc(RunLocally = false, ExcludeServer = true)]
		private void SetEquippable_Networked_ExcludeServer(NetworkConnection conn, string assetPath)
		{
			this.RpcWriter___Observers_SetEquippable_Networked_ExcludeServer_2971853958(conn, assetPath);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0006118B File Offset: 0x0005F38B
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SendEquippableMessage_Networked(NetworkConnection conn, string message)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SendEquippableMessage_Networked_2971853958(conn, message);
				this.RpcLogic___SendEquippableMessage_Networked_2971853958(conn, message);
			}
			else
			{
				this.RpcWriter___Target_SendEquippableMessage_Networked_2971853958(conn, message);
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000611C4 File Offset: 0x0005F3C4
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SendEquippableMessage_Networked_Vector(NetworkConnection conn, string message, Vector3 data)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
				this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
			}
			else
			{
				this.RpcWriter___Target_SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00061211 File Offset: 0x0005F411
		[ServerRpc(RequireOwnership = false)]
		public void SendAnimationTrigger(string trigger)
		{
			this.RpcWriter___Server_SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0006121D File Offset: 0x0005F41D
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00061253 File Offset: 0x0005F453
		public void SetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.SetTrigger(trigger);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00061266 File Offset: 0x0005F466
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ResetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0006129C File Offset: 0x0005F49C
		public void ResetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.ResetTrigger(trigger);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000612AF File Offset: 0x0005F4AF
		[ObserversRpc(RunLocally = true)]
		public void SetCrouched_Networked(bool crouched)
		{
			this.RpcWriter___Observers_SetCrouched_Networked_1140765316(crouched);
			this.RpcLogic___SetCrouched_Networked_1140765316(crouched);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000612C8 File Offset: 0x0005F4C8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationBool_Networked(NetworkConnection conn, string id, bool value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationBool_Networked_619441887(conn, id, value);
				this.RpcLogic___SetAnimationBool_Networked_619441887(conn, id, value);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationBool_Networked_619441887(conn, id, value);
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00061315 File Offset: 0x0005F515
		public void SetAnimationBool(string trigger, bool val)
		{
			this.Avatar.Anim.SetBool(trigger, val);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0006132C File Offset: 0x0005F52C
		protected virtual bool ShouldNoticeGeneralCrime(Player player)
		{
			return player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && player.Health.IsAlive && !player.IsArrested && !player.IsUnconscious && !this.behaviour.CoweringBehaviour.Active && !this.behaviour.FleeBehaviour.Active && !this.isUnsettled;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0006139A File Offset: 0x0005F59A
		protected virtual void SetUnsettled_30s(Player player)
		{
			this.SetUnsettled(30f);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000613A8 File Offset: 0x0005F5A8
		protected void SetUnsettled(float duration)
		{
			NPC.<>c__DisplayClass167_0 CS$<>8__locals1 = new NPC.<>c__DisplayClass167_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			bool isUnsettled = this.isUnsettled;
			this.isUnsettled = true;
			if (!isUnsettled)
			{
				this.Avatar.EmotionManager.AddEmotionOverride("Concerned", "unsettled", 0f, 5);
			}
			if (this.resetUnsettledCoroutine != null)
			{
				base.StopCoroutine(this.resetUnsettledCoroutine);
			}
			this.resetUnsettledCoroutine = base.StartCoroutine(CS$<>8__locals1.<SetUnsettled>g__ResetUnsettled|0());
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0006141E File Offset: 0x0005F61E
		[ServerRpc(RequireOwnership = false)]
		public void SetPanicked()
		{
			this.RpcWriter___Server_SetPanicked_2166136261();
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00061428 File Offset: 0x0005F628
		[ObserversRpc]
		private void ReceivePanicked()
		{
			this.RpcWriter___Observers_ReceivePanicked_2166136261();
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0006143C File Offset: 0x0005F63C
		[ObserversRpc]
		private void RemovePanicked()
		{
			this.RpcWriter___Observers_RemovePanicked_2166136261();
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0006144F File Offset: 0x0005F64F
		public virtual string GetNameAddress()
		{
			return this.FirstName;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00061457 File Offset: 0x0005F657
		public void PlayVO(EVOLineType lineType)
		{
			this.VoiceOverEmitter.Play(lineType);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00061465 File Offset: 0x0005F665
		[TargetRpc]
		public void ReceiveRelationshipData(NetworkConnection conn, float relationship, bool unlocked)
		{
			this.RpcWriter___Target_ReceiveRelationshipData_4052192084(conn, relationship, unlocked);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00061479 File Offset: 0x0005F679
		[ServerRpc(RequireOwnership = false)]
		public void SetIsBeingPickPocketed(bool pickpocketed)
		{
			this.RpcWriter___Server_SetIsBeingPickPocketed_1140765316(pickpocketed);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00061485 File Offset: 0x0005F685
		[ServerRpc(RequireOwnership = false)]
		public void SendRelationship(float relationship)
		{
			this.RpcWriter___Server_SendRelationship_431000436(relationship);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00061491 File Offset: 0x0005F691
		[ObserversRpc]
		private void SetRelationship(float relationship)
		{
			this.RpcWriter___Observers_SetRelationship_431000436(relationship);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x000614A0 File Offset: 0x0005F6A0
		public void ShowOutline(Color color)
		{
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					SkinnedMeshRenderer[] array = new SkinnedMeshRenderer[0];
					array = new SkinnedMeshRenderer[]
					{
						gameObject.GetComponent<SkinnedMeshRenderer>()
					};
					for (int i = 0; i < array.Length; i++)
					{
						OutlineTarget target = new OutlineTarget(array[i], 0);
						this.OutlineEffect.TryAddTarget(target);
					}
				}
			}
			this.OutlineEffect.OutlineParameters.Color = color;
			Color32 c = color;
			c.a = 9;
			this.OutlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", c);
			this.OutlineEffect.enabled = true;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000615E8 File Offset: 0x0005F7E8
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			this.ShowOutline(BuildableItem.GetColorFromOutlineColorEnum(color));
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000615FB File Offset: 0x0005F7FB
		public void HideOutline()
		{
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00061617 File Offset: 0x0005F817
		public virtual bool ShouldSave()
		{
			return this.ShouldSaveRelationshipData() || this.ShouldSaveMessages() || this.ShouldSaveInventory() || this.ShouldSaveHealth() || this.HasChanged;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00061647 File Offset: 0x0005F847
		protected virtual bool ShouldSaveRelationshipData()
		{
			return this.RelationData.Unlocked || 2f != this.RelationData.RelationDelta;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0006166D File Offset: 0x0005F86D
		protected virtual bool ShouldSaveMessages()
		{
			return this.MSGConversation != null && (this.MSGConversation.messageHistory.Count > 0 && this.MSGConversation.HasChanged);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0006169C File Offset: 0x0005F89C
		protected virtual bool ShouldSaveInventory()
		{
			return ((IItemSlotOwner)this.Inventory).GetTotalItemCount() > 0;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000616AC File Offset: 0x0005F8AC
		protected virtual bool ShouldSaveHealth()
		{
			return this.Health.Health < this.Health.MaxHealth || this.Health.IsDead || this.Health.DaysPassedSinceDeath > 0;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x000616E3 File Offset: 0x0005F8E3
		public virtual string GetSaveString()
		{
			return new NPCData(this.ID).GetJson(true);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000616F8 File Offset: 0x0005F8F8
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			bool flag;
			string parentFolderPath2 = Path.Combine(parentFolderPath, ((ISaveable)this).GetLocalPath(out flag));
			if (this.ShouldSaveRelationshipData())
			{
				list.Add("Relationship.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Relationship", this.RelationData.GetSaveData().GetJson(true));
			}
			if (this.ShouldSaveMessages())
			{
				list.Add(this.MSGConversation.SaveFileName);
				new SaveRequest(this.MSGConversation, parentFolderPath2);
			}
			if (this.ShouldSaveInventory())
			{
				list.Add("Inventory.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Inventory", new ItemSet(this.Inventory.ItemSlots).GetJSON());
			}
			if (this.ShouldSaveHealth())
			{
				list.Add("Health.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Health", new NPCHealthData(this.Health.Health, this.Health.IsDead, this.Health.DaysPassedSinceDeath).GetJson(true));
			}
			Customer component = base.GetComponent<Customer>();
			if (component != null && component.HasChanged)
			{
				new SaveRequest(component, parentFolderPath2);
			}
			return list;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(NPCData data, string containerPath)
		{
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00061904 File Offset: 0x0005FB04
		[CompilerGenerated]
		private void <Awake>g__Unlocked|114_0(NPCRelationData.EUnlockType unlockType, bool notify)
		{
			if (this.NPCUnlockedVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.NPCUnlockedVariable, true.ToString(), true);
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00061940 File Offset: 0x0005FB40
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted = true;
			this.syncVar___PlayerConversant = new SyncVar<NetworkObject>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.PlayerConversant);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetTransform_4260003484));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_AimedAtByPlayer_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendImpact_427288424));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveImpact_427288424));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_EnterVehicle_3321926803));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_EnterVehicle_3321926803));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ExitVehicle_2166136261));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SendWorldspaceDialogueKey_606697822));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_PlayWorldspaceDialogue_606697822));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetConversant_3323014238));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_EnterBuilding_3905681115));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_EnterBuilding_3905681115));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ExitBuilding_3615296227));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_2971853958));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_SetEquippable_Networked_2971853958));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_ExcludeServer_2971853958));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SendEquippableMessage_Networked_2971853958));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SendEquippableMessage_Networked_2971853958));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SendEquippableMessage_Networked_Vector_4022222929));
			base.RegisterTargetRpc(19U, new ClientRpcDelegate(this.RpcReader___Target_SendEquippableMessage_Networked_Vector_4022222929));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationTrigger_3615296227));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(22U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(23U, new ClientRpcDelegate(this.RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(24U, new ClientRpcDelegate(this.RpcReader___Target_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(25U, new ClientRpcDelegate(this.RpcReader___Observers_SetCrouched_Networked_1140765316));
			base.RegisterObserversRpc(26U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationBool_Networked_619441887));
			base.RegisterTargetRpc(27U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationBool_Networked_619441887));
			base.RegisterServerRpc(28U, new ServerRpcDelegate(this.RpcReader___Server_SetPanicked_2166136261));
			base.RegisterObserversRpc(29U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePanicked_2166136261));
			base.RegisterObserversRpc(30U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePanicked_2166136261));
			base.RegisterTargetRpc(31U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveRelationshipData_4052192084));
			base.RegisterServerRpc(32U, new ServerRpcDelegate(this.RpcReader___Server_SetIsBeingPickPocketed_1140765316));
			base.RegisterServerRpc(33U, new ServerRpcDelegate(this.RpcReader___Server_SendRelationship_431000436));
			base.RegisterObserversRpc(34U, new ClientRpcDelegate(this.RpcReader___Observers_SetRelationship_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.NPCs.NPC));
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00061CC0 File Offset: 0x0005FEC0
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted = true;
			this.syncVar___PlayerConversant.SetRegistered();
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00061CDE File Offset: 0x0005FEDE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00061CEC File Offset: 0x0005FEEC
		private void RpcWriter___Observers_SetTransform_4260003484(NetworkConnection conn, Vector3 position, Quaternion rotation)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(conn);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00061DC1 File Offset: 0x0005FFC1
		private void RpcLogic___SetTransform_4260003484(NetworkConnection conn, Vector3 position, Quaternion rotation)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00061DDC File Offset: 0x0005FFDC
		private void RpcReader___Observers_SetTransform_4260003484(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetTransform_4260003484(conn, position, rotation);
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00061E34 File Offset: 0x00060034
		private void RpcWriter___Server_AimedAtByPlayer_3323014238(NetworkObject player)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(player);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00061EDB File Offset: 0x000600DB
		public virtual void RpcLogic___AimedAtByPlayer_3323014238(NetworkObject player)
		{
			this.responses.RespondToAimedAt(player.GetComponent<Player>());
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00061EF0 File Offset: 0x000600F0
		private void RpcReader___Server_AimedAtByPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___AimedAtByPlayer_3323014238(player);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00061F24 File Offset: 0x00060124
		private void RpcWriter___Server_SendImpact_427288424(Impact impact)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00061FCB File Offset: 0x000601CB
		public virtual void RpcLogic___SendImpact_427288424(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00061FD4 File Offset: 0x000601D4
		private void RpcReader___Server_SendImpact_427288424(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Impact impact = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00062014 File Offset: 0x00060214
		private void RpcWriter___Observers_ReceiveImpact_427288424(Impact impact)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000620CC File Offset: 0x000602CC
		public virtual void RpcLogic___ReceiveImpact_427288424(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			float num = 1f;
			NPCMovement.EStance stance = this.movement.Stance;
			if (stance != NPCMovement.EStance.None)
			{
				if (stance == NPCMovement.EStance.Stanced)
				{
					num = 0.5f;
				}
			}
			else
			{
				num = 1f;
			}
			this.Health.TakeDamage(impact.ImpactDamage, Impact.IsLethal(impact.ImpactType));
			this.ProcessImpactForce(impact.HitPoint, impact.ImpactForceDirection, impact.ImpactForce * num);
			this.responses.ImpactReceived(impact);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0006216C File Offset: 0x0006036C
		private void RpcReader___Observers_ReceiveImpact_427288424(PooledReader PooledReader0, Channel channel)
		{
			Impact impact = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000621A8 File Offset: 0x000603A8
		private void RpcWriter___Observers_EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated(veh);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00062260 File Offset: 0x00060460
		public virtual void RpcLogic___EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
		{
			if (veh == this.CurrentVehicle)
			{
				return;
			}
			this.CurrentVehicle = veh;
			this.SetVisible(false);
			this.movement.Agent.enabled = false;
			base.transform.SetParent(veh.transform);
			veh.AddNPCOccupant(this);
			base.transform.position = this.CurrentVehicle.Seats[this.CurrentVehicle.OccupantNPCs.ToList<NPC>().IndexOf(this)].transform.position;
			base.transform.localRotation = Quaternion.identity;
			if (this.onEnterVehicle != null)
			{
				this.onEnterVehicle(veh);
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00062310 File Offset: 0x00060510
		private void RpcReader___Observers_EnterVehicle_3321926803(PooledReader PooledReader0, Channel channel)
		{
			LandVehicle veh = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnterVehicle_3321926803(null, veh);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0006234C File Offset: 0x0006054C
		private void RpcWriter___Target_EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated(veh);
			base.SendTargetRpc(5U, writer, channel, DataOrderType.Default, connection, false, true);
			writer.Store();
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00062404 File Offset: 0x00060604
		private void RpcReader___Target_EnterVehicle_3321926803(PooledReader PooledReader0, Channel channel)
		{
			LandVehicle veh = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnterVehicle_3321926803(base.LocalConnection, veh);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0006243C File Offset: 0x0006063C
		private void RpcWriter___Observers_ExitVehicle_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000624E8 File Offset: 0x000606E8
		public virtual void RpcLogic___ExitVehicle_2166136261()
		{
			if (this.CurrentVehicle == null)
			{
				return;
			}
			int seatIndex = this.CurrentVehicle.OccupantNPCs.ToList<NPC>().IndexOf(this);
			this.CurrentVehicle.RemoveNPCOccupant(this);
			this.CurrentVehicle.Agent.Flags.ResetFlags();
			if (this.CurrentVehicle.GetComponent<VehicleLights>() != null)
			{
				this.CurrentVehicle.GetComponent<VehicleLights>().headLightsOn = false;
			}
			Transform exitPoint = this.CurrentVehicle.GetExitPoint(seatIndex);
			base.transform.SetParent(NetworkSingleton<NPCManager>.Instance.NPCContainer);
			base.transform.position = exitPoint.position - exitPoint.up * 1f;
			this.movement.FaceDirection(exitPoint.forward, 0f);
			if (InstanceFinder.IsServer)
			{
				this.movement.Agent.enabled = true;
			}
			this.SetVisible(true);
			if (this.onExitVehicle != null)
			{
				this.onExitVehicle(this.CurrentVehicle);
			}
			this.CurrentVehicle = null;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00062600 File Offset: 0x00060800
		private void RpcReader___Observers_ExitVehicle_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitVehicle_2166136261();
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0006262C File Offset: 0x0006082C
		private void RpcWriter___Server_SendWorldspaceDialogueKey_606697822(string key, float duration)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(key);
			writer.WriteSingle(duration, AutoPackType.Unpacked);
			base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000626E5 File Offset: 0x000608E5
		public void RpcLogic___SendWorldspaceDialogueKey_606697822(string key, float duration)
		{
			this.PlayWorldspaceDialogue(key, duration);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000626F0 File Offset: 0x000608F0
		private void RpcReader___Server_SendWorldspaceDialogueKey_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string key = PooledReader0.ReadString();
			float duration = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWorldspaceDialogueKey_606697822(key, duration);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00062744 File Offset: 0x00060944
		private void RpcWriter___Observers_PlayWorldspaceDialogue_606697822(string key, float duration)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(key);
			writer.WriteSingle(duration, AutoPackType.Unpacked);
			base.SendObserversRpc(8U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0006280C File Offset: 0x00060A0C
		private void RpcLogic___PlayWorldspaceDialogue_606697822(string key, float duration)
		{
			this.dialogueHandler.PlayReaction(key, duration, false);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0006281C File Offset: 0x00060A1C
		private void RpcReader___Observers_PlayWorldspaceDialogue_606697822(PooledReader PooledReader0, Channel channel)
		{
			string key = PooledReader0.ReadString();
			float duration = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlayWorldspaceDialogue_606697822(key, duration);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00062870 File Offset: 0x00060A70
		private void RpcWriter___Server_SetConversant_3323014238(NetworkObject player)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(player);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00062917 File Offset: 0x00060B17
		public void RpcLogic___SetConversant_3323014238(NetworkObject player)
		{
			this.sync___set_value_PlayerConversant(player, true);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00062924 File Offset: 0x00060B24
		private void RpcReader___Server_SetConversant_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetConversant_3323014238(player);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00062964 File Offset: 0x00060B64
		private void RpcWriter___Observers_EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, AutoPackType.Packed);
			base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00062A2C File Offset: 0x00060C2C
		public void RpcLogic___EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingGUID));
			if (@object == null)
			{
				Console.LogWarning(this.fullName + ".EnterBuilding: building not found with given GUID", null);
				return;
			}
			this.awareness.VisionCone.ClearEvents();
			if (@object == this.CurrentBuilding)
			{
				if (InstanceFinder.IsServer)
				{
					this.Movement.Warp(@object.Doors[doorIndex].AccessPoint);
					this.Movement.Stop();
				}
				this.SetVisible(false);
				return;
			}
			if (this.CurrentBuilding != null)
			{
				Console.LogWarning("NPC.EnterBuilding called but NPC is already in a building. New building will still be entered.", null);
				this.ExitBuilding("");
			}
			this.CurrentBuilding = @object;
			this.LastEnteredDoor = @object.Doors[doorIndex];
			this.awareness.SetAwarenessActive(false);
			@object.NPCEnteredBuilding(this);
			this.SetVisible(false);
			this.Movement.Stop();
			this.Movement.Warp(@object.Doors[doorIndex].AccessPoint);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00062B2C File Offset: 0x00060D2C
		private void RpcReader___Observers_EnterBuilding_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnterBuilding_3905681115(null, buildingGUID, doorIndex);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00062B80 File Offset: 0x00060D80
		private void RpcWriter___Target_EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, AutoPackType.Packed);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, connection, false, true);
			writer.Store();
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00062C48 File Offset: 0x00060E48
		private void RpcReader___Target_EnterBuilding_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnterBuilding_3905681115(base.LocalConnection, buildingGUID, doorIndex);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00062C98 File Offset: 0x00060E98
		private void RpcWriter___Observers_ExitBuilding_3615296227(string buildingID = "")
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(buildingID);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00062D50 File Offset: 0x00060F50
		public void RpcLogic___ExitBuilding_3615296227(string buildingID = "")
		{
			if (buildingID == "" && this.CurrentBuilding != null)
			{
				buildingID = this.CurrentBuilding.GUID.ToString();
			}
			if (buildingID == "")
			{
				return;
			}
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingID));
			if (@object == null)
			{
				return;
			}
			if (this.LastEnteredDoor == null)
			{
				this.LastEnteredDoor = @object.Doors[0];
			}
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			NavMeshHit navMeshHit;
			Vector3 position = NavMeshUtility.SamplePosition(this.LastEnteredDoor.AccessPoint.transform.position, out navMeshHit, 2f, -1, true) ? navMeshHit.position : this.LastEnteredDoor.AccessPoint.transform.position;
			this.Movement.Warp(position);
			this.Movement.FaceDirection(-this.LastEnteredDoor.AccessPoint.transform.forward, 0f);
			this.awareness.SetAwarenessActive(true);
			@object.NPCExitedBuilding(this);
			this.CurrentBuilding = null;
			this.SetVisible(true);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00062E98 File Offset: 0x00061098
		private void RpcReader___Observers_ExitBuilding_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string buildingID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitBuilding_3615296227(buildingID);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00062ED4 File Offset: 0x000610D4
		private void RpcWriter___Observers_SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(assetPath);
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00062F8A File Offset: 0x0006118A
		public void RpcLogic___SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00062F9C File Offset: 0x0006119C
		private void RpcReader___Observers_SetEquippable_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_2971853958(null, assetPath);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00062FD8 File Offset: 0x000611D8
		private void RpcWriter___Target_SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(assetPath);
			base.SendTargetRpc(14U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00063090 File Offset: 0x00061290
		private void RpcReader___Target_SetEquippable_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_2971853958(base.LocalConnection, assetPath);
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000630C8 File Offset: 0x000612C8
		private void RpcWriter___Observers_SetEquippable_Networked_ExcludeServer_2971853958(NetworkConnection conn, string assetPath)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(conn);
			writer.WriteString(assetPath);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, true, false);
			writer.Store();
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00062F8A File Offset: 0x0006118A
		private void RpcLogic___SetEquippable_Networked_ExcludeServer_2971853958(NetworkConnection conn, string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0006318C File Offset: 0x0006138C
		private void RpcReader___Observers_SetEquippable_Networked_ExcludeServer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_ExcludeServer_2971853958(conn, assetPath);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x000631D0 File Offset: 0x000613D0
		private void RpcWriter___Observers_SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00063286 File Offset: 0x00061486
		public void RpcLogic___SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
		{
			this.Avatar.ReceiveEquippableMessage(message, null);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00063298 File Offset: 0x00061498
		private void RpcReader___Observers_SendEquippableMessage_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_2971853958(null, message);
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000632D4 File Offset: 0x000614D4
		private void RpcWriter___Target_SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			base.SendTargetRpc(17U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0006338C File Offset: 0x0006158C
		private void RpcReader___Target_SendEquippableMessage_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_2971853958(base.LocalConnection, message);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000633C4 File Offset: 0x000615C4
		private void RpcWriter___Observers_SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteVector3(data);
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00063487 File Offset: 0x00061687
		public void RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
		{
			this.Avatar.ReceiveEquippableMessage(message, data);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0006349C File Offset: 0x0006169C
		private void RpcReader___Observers_SendEquippableMessage_Networked_Vector_4022222929(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(null, message, data);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000634EC File Offset: 0x000616EC
		private void RpcWriter___Target_SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteVector3(data);
			base.SendTargetRpc(19U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000635B0 File Offset: 0x000617B0
		private void RpcReader___Target_SendEquippableMessage_Networked_Vector_4022222929(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(base.LocalConnection, message, data);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000635F8 File Offset: 0x000617F8
		private void RpcWriter___Server_SendAnimationTrigger_3615296227(string trigger)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendServerRpc(20U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0006369F File Offset: 0x0006189F
		public void RpcLogic___SendAnimationTrigger_3615296227(string trigger)
		{
			this.SetAnimationTrigger_Networked(null, trigger);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x000636AC File Offset: 0x000618AC
		private void RpcReader___Server_SendAnimationTrigger_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000636E0 File Offset: 0x000618E0
		private void RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendObserversRpc(21U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00063796 File Offset: 0x00061996
		public void RpcLogic___SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.SetAnimationTrigger(trigger);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000637A0 File Offset: 0x000619A0
		private void RpcReader___Observers_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000637DC File Offset: 0x000619DC
		private void RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendTargetRpc(22U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00063894 File Offset: 0x00061A94
		private void RpcReader___Target_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x000638CC File Offset: 0x00061ACC
		private void RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendObserversRpc(23U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00063982 File Offset: 0x00061B82
		public void RpcLogic___ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.ResetAnimationTrigger(trigger);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x0006398C File Offset: 0x00061B8C
		private void RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x000639C8 File Offset: 0x00061BC8
		private void RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendTargetRpc(24U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00063A80 File Offset: 0x00061C80
		private void RpcReader___Target_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00063AB8 File Offset: 0x00061CB8
		private void RpcWriter___Observers_SetCrouched_Networked_1140765316(bool crouched)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(crouched);
			base.SendObserversRpc(25U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00063B6E File Offset: 0x00061D6E
		public void RpcLogic___SetCrouched_Networked_1140765316(bool crouched)
		{
			this.Avatar.Anim.SetCrouched(crouched);
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00063B84 File Offset: 0x00061D84
		private void RpcReader___Observers_SetCrouched_Networked_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCrouched_Networked_1140765316(crouched);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00063BC0 File Offset: 0x00061DC0
		private void RpcWriter___Observers_SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(id);
			writer.WriteBoolean(value);
			base.SendObserversRpc(26U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00063C83 File Offset: 0x00061E83
		public void RpcLogic___SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
		{
			this.Avatar.Anim.SetBool(id, value);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00063C98 File Offset: 0x00061E98
		private void RpcReader___Observers_SetAnimationBool_Networked_619441887(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_Networked_619441887(null, id, value);
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00063CE8 File Offset: 0x00061EE8
		private void RpcWriter___Target_SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(id);
			writer.WriteBoolean(value);
			base.SendTargetRpc(27U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00063DAC File Offset: 0x00061FAC
		private void RpcReader___Target_SetAnimationBool_Networked_619441887(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_Networked_619441887(base.LocalConnection, id, value);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00063DF4 File Offset: 0x00061FF4
		private void RpcWriter___Server_SetPanicked_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(28U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00063E8E File Offset: 0x0006208E
		public void RpcLogic___SetPanicked_2166136261()
		{
			float timeSincePanicked = this.timeSincePanicked;
			this.timeSincePanicked = 0f;
			if (timeSincePanicked > 20f)
			{
				this.ReceivePanicked();
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00063EB0 File Offset: 0x000620B0
		private void RpcReader___Server_SetPanicked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetPanicked_2166136261();
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00063ED0 File Offset: 0x000620D0
		private void RpcWriter___Observers_ReceivePanicked_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(29U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00063F7C File Offset: 0x0006217C
		private void RpcLogic___ReceivePanicked_2166136261()
		{
			this.Avatar.EmotionManager.AddEmotionOverride("Scared", "panicked", 0f, 10);
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.Agent.Flags.OverriddenSpeed = 50f;
				this.CurrentVehicle.Agent.Flags.OverriddenReverseSpeed = 20f;
				this.CurrentVehicle.Agent.Flags.OverrideSpeed = true;
				this.CurrentVehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.CurrentVehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.IgnoreOnlySquishy;
				return;
			}
			this.behaviour.CoweringBehaviour.Enable();
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00064040 File Offset: 0x00062240
		private void RpcReader___Observers_ReceivePanicked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePanicked_2166136261();
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00064060 File Offset: 0x00062260
		private void RpcWriter___Observers_RemovePanicked_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(30U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0006410C File Offset: 0x0006230C
		private void RpcLogic___RemovePanicked_2166136261()
		{
			this.Avatar.EmotionManager.RemoveEmotionOverride("panicked");
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.Agent.Flags.ResetFlags();
			}
			this.behaviour.CoweringBehaviour.Disable();
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00064164 File Offset: 0x00062364
		private void RpcReader___Observers_RemovePanicked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___RemovePanicked_2166136261();
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00064184 File Offset: 0x00062384
		private void RpcWriter___Target_ReceiveRelationshipData_4052192084(NetworkConnection conn, float relationship, bool unlocked)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(relationship, AutoPackType.Unpacked);
			writer.WriteBoolean(unlocked);
			base.SendTargetRpc(31U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0006424B File Offset: 0x0006244B
		public void RpcLogic___ReceiveRelationshipData_4052192084(NetworkConnection conn, float relationship, bool unlocked)
		{
			this.RelationData.SetRelationship(relationship);
			Console.Log("Received relationship data for " + this.fullName + " Unlocked: " + unlocked.ToString(), null);
			if (unlocked)
			{
				this.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
			}
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0006428C File Offset: 0x0006248C
		private void RpcReader___Target_ReceiveRelationshipData_4052192084(PooledReader PooledReader0, Channel channel)
		{
			float relationship = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			bool unlocked = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRelationshipData_4052192084(base.LocalConnection, relationship, unlocked);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000642DC File Offset: 0x000624DC
		private void RpcWriter___Server_SetIsBeingPickPocketed_1140765316(bool pickpocketed)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(pickpocketed);
			base.SendServerRpc(32U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00064383 File Offset: 0x00062583
		public void RpcLogic___SetIsBeingPickPocketed_1140765316(bool pickpocketed)
		{
			if (pickpocketed)
			{
				this.behaviour.StationaryBehaviour.Enable_Networked(null);
				return;
			}
			this.behaviour.StationaryBehaviour.Disable_Networked(null);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x000643AC File Offset: 0x000625AC
		private void RpcReader___Server_SetIsBeingPickPocketed_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool pickpocketed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsBeingPickPocketed_1140765316(pickpocketed);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x000643E0 File Offset: 0x000625E0
		private void RpcWriter___Server_SendRelationship_431000436(float relationship)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(relationship, AutoPackType.Unpacked);
			base.SendServerRpc(33U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0006448C File Offset: 0x0006268C
		public void RpcLogic___SendRelationship_431000436(float relationship)
		{
			this.SetRelationship(relationship);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00064498 File Offset: 0x00062698
		private void RpcReader___Server_SendRelationship_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float relationship = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRelationship_431000436(relationship);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000644D0 File Offset: 0x000626D0
		private void RpcWriter___Observers_SetRelationship_431000436(float relationship)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(relationship, AutoPackType.Unpacked);
			base.SendObserversRpc(34U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0006458B File Offset: 0x0006278B
		private void RpcLogic___SetRelationship_431000436(float relationship)
		{
			this.RelationData.SetRelationship(relationship);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0006459C File Offset: 0x0006279C
		private void RpcReader___Observers_SetRelationship_431000436(PooledReader PooledReader0, Channel channel)
		{
			float relationship = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetRelationship_431000436(relationship);
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x000645D2 File Offset: 0x000627D2
		// (set) Token: 0x060016B9 RID: 5817 RVA: 0x000645DA File Offset: 0x000627DA
		public NetworkObject SyncAccessor_PlayerConversant
		{
			get
			{
				return this.PlayerConversant;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.PlayerConversant = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___PlayerConversant.SetValue(value, value);
				}
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00064618 File Offset: 0x00062818
		public virtual bool NPC(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_PlayerConversant(this.syncVar___PlayerConversant.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_PlayerConversant(value, Boolean2);
			return true;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0006466C File Offset: 0x0006286C
		protected virtual void dll()
		{
			this.GetHealth();
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered_Internal));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted_Internal));
			this.Inventory = base.GetComponent<NPCInventory>();
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			if (!NPCManager.NPCRegistry.Contains(this))
			{
				NPCManager.NPCRegistry.Add(this);
			}
			this.awareness.onNoticedGeneralCrime.AddListener(new UnityAction<Player>(this.SetUnsettled_30s));
			this.awareness.onNoticedPettyCrime.AddListener(new UnityAction<Player>(this.SetUnsettled_30s));
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.Avatar.BodyMeshes)
			{
				this.OutlineRenderers.Add(skinnedMeshRenderer.gameObject);
			}
			if (this.VoiceOverEmitter == null)
			{
				this.VoiceOverEmitter = this.Avatar.HeadBone.GetComponentInChildren<VOEmitter>();
			}
			this.RelationData.Init(this);
			if (this.RelationData.Unlocked)
			{
				this.<Awake>g__Unlocked|114_0(NPCRelationData.EUnlockType.DirectApproach, false);
			}
			else
			{
				NPCRelationData relationData = this.RelationData;
				relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.<Awake>g__Unlocked|114_0));
			}
			foreach (NPC x in this.RelationData.Connections)
			{
				if (!(x == null) && x == this)
				{
					Console.LogWarning("NPC " + this.fullName + " has a connection to itself", null);
				}
			}
			this.headlightStartTime = 1700 + Mathf.RoundToInt(90f * Mathf.Clamp01((float)(this.fullName[0].GetHashCode() / 1000 % 10) / 10f));
			this.InitializeSaveable();
			this.defaultAggression = this.Aggression;
		}

		// Token: 0x04001482 RID: 5250
		public const float PANIC_DURATION = 20f;

		// Token: 0x04001483 RID: 5251
		public const bool RequiresRegionUnlocked = true;

		// Token: 0x04001484 RID: 5252
		[Header("Info Settings")]
		public string FirstName = string.Empty;

		// Token: 0x04001485 RID: 5253
		public bool hasLastName = true;

		// Token: 0x04001486 RID: 5254
		public string LastName = string.Empty;

		// Token: 0x04001488 RID: 5256
		public string ID = string.Empty;

		// Token: 0x04001489 RID: 5257
		public bool AutoGenerateMugshot = true;

		// Token: 0x0400148A RID: 5258
		public Sprite MugshotSprite;

		// Token: 0x0400148B RID: 5259
		public EMapRegion Region = EMapRegion.Downtown;

		// Token: 0x0400148C RID: 5260
		[Header("If true, NPC will respawn next day instead of waiting 3 days.")]
		public bool IsImportant;

		// Token: 0x0400148D RID: 5261
		[Header("Personality")]
		[Range(0f, 1f)]
		public float Aggression;

		// Token: 0x0400148E RID: 5262
		[Header("References")]
		[SerializeField]
		protected Transform modelContainer;

		// Token: 0x0400148F RID: 5263
		[SerializeField]
		protected NPCMovement movement;

		// Token: 0x04001490 RID: 5264
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04001491 RID: 5265
		public DialogueHandler dialogueHandler;

		// Token: 0x04001492 RID: 5266
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x04001493 RID: 5267
		public NPCAwareness awareness;

		// Token: 0x04001494 RID: 5268
		public NPCResponses responses;

		// Token: 0x04001495 RID: 5269
		public NPCActions actions;

		// Token: 0x04001496 RID: 5270
		public NPCBehaviour behaviour;

		// Token: 0x04001498 RID: 5272
		public VOEmitter VoiceOverEmitter;

		// Token: 0x04001499 RID: 5273
		public NPCHealth Health;

		// Token: 0x0400149B RID: 5275
		public Action<LandVehicle> onEnterVehicle;

		// Token: 0x0400149C RID: 5276
		public Action<LandVehicle> onExitVehicle;

		// Token: 0x0400149F RID: 5279
		[Header("Summoning")]
		public bool CanBeSummoned = true;

		// Token: 0x040014A0 RID: 5280
		[Header("Relationship")]
		public NPCRelationData RelationData;

		// Token: 0x040014A1 RID: 5281
		public string NPCUnlockedVariable = string.Empty;

		// Token: 0x040014A2 RID: 5282
		public bool ShowRelationshipInfo = true;

		// Token: 0x040014A3 RID: 5283
		[Header("Messaging")]
		public List<EConversationCategory> ConversationCategories;

		// Token: 0x040014A5 RID: 5285
		public bool ConversationCanBeHidden = true;

		// Token: 0x040014A6 RID: 5286
		public Action onConversationCreated;

		// Token: 0x040014A7 RID: 5287
		[Header("Other Settings")]
		public bool CanOpenDoors = true;

		// Token: 0x040014A8 RID: 5288
		[SerializeField]
		protected List<GameObject> OutlineRenderers = new List<GameObject>();

		// Token: 0x040014A9 RID: 5289
		protected Outlinable OutlineEffect;

		// Token: 0x040014AD RID: 5293
		[Header("GUID")]
		public string BakedGUID = string.Empty;

		// Token: 0x040014B0 RID: 5296
		public Action<bool> onVisibilityChanged;

		// Token: 0x040014B1 RID: 5297
		[HideInInspector]
		[SyncVar]
		public NetworkObject PlayerConversant;

		// Token: 0x040014B3 RID: 5299
		private Coroutine resetUnsettledCoroutine;

		// Token: 0x040014B5 RID: 5301
		private List<int> impactHistory = new List<int>();

		// Token: 0x040014B6 RID: 5302
		private int headlightStartTime = 1700;

		// Token: 0x040014B7 RID: 5303
		private int heaedLightsEndTime = 600;

		// Token: 0x040014B8 RID: 5304
		protected float defaultAggression;

		// Token: 0x040014B9 RID: 5305
		private Coroutine lerpScaleRoutine;

		// Token: 0x040014BA RID: 5306
		public SyncVar<NetworkObject> syncVar___PlayerConversant;

		// Token: 0x040014BB RID: 5307
		private bool dll_Excuted;

		// Token: 0x040014BC RID: 5308
		private bool dll_Excuted;
	}
}
