using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.UI.Handover;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000521 RID: 1313
	public class RequestProductBehaviour : Behaviour
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x000832A4 File Offset: 0x000814A4
		// (set) Token: 0x06001FE0 RID: 8160 RVA: 0x000832AC File Offset: 0x000814AC
		public Player TargetPlayer { get; private set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x000832B5 File Offset: 0x000814B5
		// (set) Token: 0x06001FE2 RID: 8162 RVA: 0x000832BD File Offset: 0x000814BD
		public RequestProductBehaviour.EState State { get; private set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x000832C6 File Offset: 0x000814C6
		private Customer customer
		{
			get
			{
				return base.Npc.GetComponent<Customer>();
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000832D3 File Offset: 0x000814D3
		[ObserversRpc(RunLocally = true)]
		public void AssignTarget(NetworkObject plr)
		{
			this.RpcWriter___Observers_AssignTarget_3323014238(plr);
			this.RpcLogic___AssignTarget_3323014238(plr);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000832E9 File Offset: 0x000814E9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.RequestProductBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00083300 File Offset: 0x00081500
		protected override void Begin()
		{
			base.Begin();
			this.State = RequestProductBehaviour.EState.InitialApproach;
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_initial");
			if (InstanceFinder.IsServer)
			{
				Transform target = NetworkSingleton<NPCManager>.Instance.GetOrderedDistanceWarpPoints(this.TargetPlayer.transform.position)[1];
				base.Npc.Movement.Warp(target);
				if (base.Npc.isInBuilding)
				{
					base.Npc.ExitBuilding("");
				}
				base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
				base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("requestproduct", 5, 0.4f));
			}
			this.requestGreeting.ShouldShow = (this.TargetPlayer != null && this.TargetPlayer.Owner.IsLocalClient);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000833FC File Offset: 0x000815FC
		protected override void End()
		{
			base.End();
			if (this.requestGreeting != null)
			{
				this.requestGreeting.ShouldShow = false;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
				base.Npc.Movement.SpeedController.RemoveSpeedControl("requestproduct");
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00083458 File Offset: 0x00081658
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (base.Npc.dialogueHandler.IsPlaying)
			{
				this.minsSinceLastDialogue = 0;
			}
			this.minsSinceLastDialogue++;
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.TargetPlayer.Owner.IsLocalClient)
			{
				if (this.State == RequestProductBehaviour.EState.InitialApproach && this.CanStartDialogue())
				{
					this.SendStartInitialDialogue();
				}
				if (this.State == RequestProductBehaviour.EState.FollowPlayer && this.minsSinceLastDialogue >= 90 && this.CanStartDialogue())
				{
					this.minsSinceLastDialogue = 0;
					this.SendStartFollowUpDialogue();
				}
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<HandoverScreen>.Instance.CurrentCustomer == this.customer)
			{
				return;
			}
			if (!RequestProductBehaviour.IsTargetValid(this.TargetPlayer))
			{
				base.SendDisable();
				return;
			}
			if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				if (!this.IsTargetDestinationValid())
				{
					Vector3 destination;
					if (this.GetNewDestination(out destination))
					{
						base.Npc.Movement.SetDestination(destination);
						return;
					}
					base.SendDisable();
					return;
				}
			}
			else if (this.State == RequestProductBehaviour.EState.FollowPlayer && !this.IsTargetDestinationValid())
			{
				Vector3 destination2;
				if (this.GetNewDestination(out destination2))
				{
					base.Npc.Movement.SetDestination(destination2);
					return;
				}
				base.SendDisable();
				return;
			}
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0008358C File Offset: 0x0008178C
		private bool IsTargetDestinationValid()
		{
			return base.Npc.Movement.IsMoving && Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.transform.position) <= ((this.State == RequestProductBehaviour.EState.InitialApproach) ? 2.5f : 5f) && base.Npc.Movement.Agent.path != null;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x00083604 File Offset: 0x00081804
		private bool GetNewDestination(out Vector3 dest)
		{
			dest = this.TargetPlayer.transform.position;
			if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				dest += this.TargetPlayer.transform.forward * 1.5f;
			}
			else if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				dest += (base.Npc.transform.position - this.TargetPlayer.transform.position).normalized * 2.5f;
			}
			NavMeshHit navMeshHit;
			if (NavMeshUtility.SamplePosition(dest, out navMeshHit, 15f, -1, true))
			{
				dest = navMeshHit.position;
				return true;
			}
			Console.LogError("Failed to find valid destination for RequestProductBehaviour: stopping", null);
			return false;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000836E0 File Offset: 0x000818E0
		public static bool IsTargetValid(Player player)
		{
			return !(player == null) && !player.IsArrested && player.Health.IsAlive && player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && !player.CrimeData.BodySearchPending && !player.IsSleeping;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x0008373C File Offset: 0x0008193C
		public bool CanStartDialogue()
		{
			return RequestProductBehaviour.IsTargetValid(this.TargetPlayer) && this.TargetPlayer.Owner.IsLocalClient && !Singleton<DialogueCanvas>.Instance.isActive && Vector3.Distance(base.Npc.transform.position, this.TargetPlayer.transform.position) <= 2.5f && !Singleton<HandoverScreen>.Instance.IsOpen && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000837C8 File Offset: 0x000819C8
		private void SetUpDialogue()
		{
			if (this.requestGreeting != null)
			{
				return;
			}
			this.acceptRequestChoice = new DialogueController.DialogueChoice();
			this.acceptRequestChoice.ChoiceText = "[Make an offer]";
			this.acceptRequestChoice.Enabled = true;
			this.acceptRequestChoice.Conversation = null;
			this.acceptRequestChoice.onChoosen = new UnityEvent();
			this.acceptRequestChoice.onChoosen.AddListener(new UnityAction(this.RequestAccepted));
			this.acceptRequestChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.acceptRequestChoice, 0);
			this.followChoice = new DialogueController.DialogueChoice();
			this.followChoice.ChoiceText = "Follow me, I need to grab it first";
			this.followChoice.Enabled = true;
			this.followChoice.Conversation = null;
			this.followChoice.onChoosen = new UnityEvent();
			this.followChoice.onChoosen.AddListener(new UnityAction(this.Follow));
			this.followChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.followChoice, 0);
			this.rejectChoice = new DialogueController.DialogueChoice();
			this.rejectChoice.ChoiceText = "Get out of here";
			this.rejectChoice.Enabled = true;
			this.rejectChoice.Conversation = null;
			this.rejectChoice.onChoosen = new UnityEvent();
			this.rejectChoice.onChoosen.AddListener(new UnityAction(this.RequestRejected));
			this.rejectChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.rejectChoice, 0);
			this.requestGreeting = new DialogueController.GreetingOverride();
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_initial");
			this.requestGreeting.ShouldShow = false;
			this.requestGreeting.PlayVO = true;
			this.requestGreeting.VOType = EVOLineType.Question;
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddGreetingOverride(this.requestGreeting);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00083A08 File Offset: 0x00081C08
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendStartInitialDialogue()
		{
			this.RpcWriter___Server_SendStartInitialDialogue_2166136261();
			this.RpcLogic___SendStartInitialDialogue_2166136261();
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00083A18 File Offset: 0x00081C18
		[ObserversRpc(RunLocally = true)]
		private void StartInitialDialogue()
		{
			this.RpcWriter___Observers_StartInitialDialogue_2166136261();
			this.RpcLogic___StartInitialDialogue_2166136261();
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00083A31 File Offset: 0x00081C31
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendStartFollowUpDialogue()
		{
			this.RpcWriter___Server_SendStartFollowUpDialogue_2166136261();
			this.RpcLogic___SendStartFollowUpDialogue_2166136261();
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00083A40 File Offset: 0x00081C40
		[ObserversRpc(RunLocally = true)]
		private void StartFollowUpDialogue()
		{
			this.RpcWriter___Observers_StartFollowUpDialogue_2166136261();
			this.RpcLogic___StartFollowUpDialogue_2166136261();
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00083A59 File Offset: 0x00081C59
		private bool DialogueActive(bool enabled)
		{
			return base.Active && this.TargetPlayer.Owner.IsLocalClient;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00083A75 File Offset: 0x00081C75
		private void RequestAccepted()
		{
			this.minsSinceLastDialogue = 0;
			Singleton<HandoverScreen>.Instance.Open(null, this.customer, HandoverScreen.EMode.Offer, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.HandoverClosed), new Func<List<ItemInstance>, float, float>(this.customer.GetOfferSuccessChance));
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00083AB0 File Offset: 0x00081CB0
		private void HandoverClosed(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float askingPrice)
		{
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				Singleton<DialogueCanvas>.Instance.SkipNextRollout = true;
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<HandoverClosed>g__Wait|36_0());
				return;
			}
			float offerSuccessChance = this.customer.GetOfferSuccessChance(items, askingPrice);
			if (UnityEngine.Random.value < offerSuccessChance)
			{
				Contract contract = new Contract();
				ProductList productList = new ProductList();
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i] is ProductItemInstance)
					{
						productList.entries.Add(new ProductList.Entry
						{
							ProductID = items[i].ID,
							Quantity = items[i].Quantity,
							Quality = this.customer.CustomerData.Standards.GetCorrespondingQuality()
						});
					}
				}
				contract.SilentlyInitializeContract("Offer", string.Empty, null, string.Empty, base.Npc.NetworkObject, askingPrice, productList, string.Empty, new QuestWindowConfig(), 0, NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.GetDateTime());
				this.customer.ProcessHandover(HandoverScreen.EHandoverOutcome.Finalize, contract, items, true, false);
			}
			else
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(true);
				this.customer.RejectProductRequestOffer();
			}
			base.SendDisable();
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00083BD8 File Offset: 0x00081DD8
		private void Follow()
		{
			this.minsSinceLastDialogue = 0;
			this.State = RequestProductBehaviour.EState.FollowPlayer;
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("requestproduct", 5, 0.6f));
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_after_follow");
			base.Npc.dialogueHandler.ShowWorldspaceDialogue("Ok...", 3f);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00083C58 File Offset: 0x00081E58
		private void RequestRejected()
		{
			this.minsSinceLastDialogue = 0;
			this.customer.PlayerRejectedProductRequest();
			base.SendDisable();
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00083C72 File Offset: 0x00081E72
		[CompilerGenerated]
		private IEnumerator <HandoverClosed>g__Wait|36_0()
		{
			yield return new WaitForEndOfFrame();
			this.StartInitialDialogue();
			yield break;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00083C84 File Offset: 0x00081E84
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_AssignTarget_3323014238));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SendStartInitialDialogue_2166136261));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_StartInitialDialogue_2166136261));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SendStartFollowUpDialogue_2166136261));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_StartFollowUpDialogue_2166136261));
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00083D1B File Offset: 0x00081F1B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00083D34 File Offset: 0x00081F34
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00083D44 File Offset: 0x00081F44
		private void RpcWriter___Observers_AssignTarget_3323014238(NetworkObject plr)
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
			writer.WriteNetworkObject(plr);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00083DFA File Offset: 0x00081FFA
		public void RpcLogic___AssignTarget_3323014238(NetworkObject plr)
		{
			this.TargetPlayer = ((plr != null) ? plr.GetComponent<Player>() : null);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00083E14 File Offset: 0x00082014
		private void RpcReader___Observers_AssignTarget_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject plr = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignTarget_3323014238(plr);
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x00083E50 File Offset: 0x00082050
		private void RpcWriter___Server_SendStartInitialDialogue_2166136261()
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
			base.SendServerRpc(16U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00083EEA File Offset: 0x000820EA
		private void RpcLogic___SendStartInitialDialogue_2166136261()
		{
			this.StartInitialDialogue();
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00083EF4 File Offset: 0x000820F4
		private void RpcReader___Server_SendStartInitialDialogue_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartInitialDialogue_2166136261();
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00083F24 File Offset: 0x00082124
		private void RpcWriter___Observers_StartInitialDialogue_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x00083FD0 File Offset: 0x000821D0
		private void RpcLogic___StartInitialDialogue_2166136261()
		{
			if (this.TargetPlayer != null && this.TargetPlayer.IsOwner && !base.Npc.dialogueHandler.IsPlaying)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
				{
					Singleton<GameInput>.Instance.ExitAll();
				}
				base.Npc.dialogueHandler.GetComponent<DialogueController>().StartGenericDialogue(false);
			}
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00084038 File Offset: 0x00082238
		private void RpcReader___Observers_StartInitialDialogue_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartInitialDialogue_2166136261();
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00084064 File Offset: 0x00082264
		private void RpcWriter___Server_SendStartFollowUpDialogue_2166136261()
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
			base.SendServerRpc(18U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000840FE File Offset: 0x000822FE
		private void RpcLogic___SendStartFollowUpDialogue_2166136261()
		{
			this.StartFollowUpDialogue();
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00084108 File Offset: 0x00082308
		private void RpcReader___Server_SendStartFollowUpDialogue_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartFollowUpDialogue_2166136261();
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00084138 File Offset: 0x00082338
		private void RpcWriter___Observers_StartFollowUpDialogue_2166136261()
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
			base.SendObserversRpc(19U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000841E4 File Offset: 0x000823E4
		private void RpcLogic___StartFollowUpDialogue_2166136261()
		{
			if (this.TargetPlayer != null && this.TargetPlayer.IsOwner && !base.Npc.dialogueHandler.IsPlaying)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
				{
					Singleton<GameInput>.Instance.ExitAll();
				}
				base.Npc.dialogueHandler.GetComponent<DialogueController>().StartGenericDialogue(false);
			}
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0008424C File Offset: 0x0008244C
		private void RpcReader___Observers_StartFollowUpDialogue_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartFollowUpDialogue_2166136261();
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00084276 File Offset: 0x00082476
		protected virtual void dll()
		{
			base.Awake();
			this.SetUpDialogue();
		}

		// Token: 0x040018CF RID: 6351
		public const float CONVERSATION_RANGE = 2.5f;

		// Token: 0x040018D0 RID: 6352
		public const float FOLLOW_MAX_RANGE = 5f;

		// Token: 0x040018D1 RID: 6353
		public const int MINS_TO_ASK_AGAIN = 90;

		// Token: 0x040018D4 RID: 6356
		private int minsSinceLastDialogue;

		// Token: 0x040018D5 RID: 6357
		private DialogueController.GreetingOverride requestGreeting;

		// Token: 0x040018D6 RID: 6358
		private DialogueController.DialogueChoice acceptRequestChoice;

		// Token: 0x040018D7 RID: 6359
		private DialogueController.DialogueChoice followChoice;

		// Token: 0x040018D8 RID: 6360
		private DialogueController.DialogueChoice rejectChoice;

		// Token: 0x040018D9 RID: 6361
		private bool dll_Excuted;

		// Token: 0x040018DA RID: 6362
		private bool dll_Excuted;

		// Token: 0x02000522 RID: 1314
		public enum EState
		{
			// Token: 0x040018DC RID: 6364
			InitialApproach,
			// Token: 0x040018DD RID: 6365
			FollowPlayer
		}
	}
}
