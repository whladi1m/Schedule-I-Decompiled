using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004DA RID: 1242
	public class BodySearchBehaviour : Behaviour
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0007400C File Offset: 0x0007220C
		public static float BODY_SEARCH_TIME
		{
			get
			{
				if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
				{
					return 2.5f;
				}
				return 4f;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x00074025 File Offset: 0x00072225
		// (set) Token: 0x06001C09 RID: 7177 RVA: 0x0007402D File Offset: 0x0007222D
		public Player TargetPlayer { get; protected set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00074036 File Offset: 0x00072236
		private DialogueDatabase dialogueDatabase
		{
			get
			{
				return this.officer.dialogueHandler.Database;
			}
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x00074048 File Offset: 0x00072248
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.BodySearchBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0007405C File Offset: 0x0007225C
		protected override void Begin()
		{
			base.Begin();
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_begin"), NetworkSingleton<GameManager>.Instance.IsTutorial ? 4f : 5f);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("bodysearching", 40, 0.15f));
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			base.Npc.PlayVO(EVOLineType.Command);
			if (this.TargetPlayer.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FocusCameraOnTarget(base.Npc.Avatar.MiddleSpineRB.transform);
			}
			this.TargetPlayer.CrimeData.ResetBodysearchCooldown();
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x00074128 File Offset: 0x00072328
		protected override void Resume()
		{
			base.Resume();
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_begin"), 5f);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("bodysearching", 40, 0.15f));
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			this.TargetPlayer.CrimeData.ResetBodysearchCooldown();
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000741A8 File Offset: 0x000723A8
		protected override void End()
		{
			base.End();
			if (this.TargetPlayer != null)
			{
				this.TargetPlayer.CrimeData.BodySearchPending = false;
			}
			this.Disable();
			base.Npc.Avatar.Anim.SetBool("PatDown", false);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			this.ClearSpeedControls();
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00074212 File Offset: 0x00072412
		protected override void Pause()
		{
			base.Pause();
			base.Npc.Avatar.Anim.SetBool("PatDown", false);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			this.ClearSpeedControls();
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0007424C File Offset: 0x0007244C
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.searchTime += Time.deltaTime;
			this.UpdateSearch();
			this.UpdateCircle();
			this.UpdateLookAt();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsTargetValid(this.TargetPlayer))
			{
				base.Disable_Networked(null);
				base.End_Networked(null);
				return;
			}
			this.UpdateMovement();
			this.UpdateEscalation();
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000742B4 File Offset: 0x000724B4
		private void UpdateSearch()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.TargetPlayer.IsOwner && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) < 2f && !Singleton<BodySearchScreen>.Instance.IsOpen)
			{
				base.Npc.dialogueHandler.HideWorldspaceDialogue();
				Singleton<BodySearchScreen>.Instance.onSearchClear.AddListener(new UnityAction(this.SearchClean));
				if (!GameManager.IS_TUTORIAL)
				{
					Singleton<BodySearchScreen>.Instance.onSearchFail.AddListener(new UnityAction(this.SearchFail));
				}
				float num = 1f;
				if (Player.Local.Sneaky)
				{
					num = 1.5f;
				}
				base.Npc.Movement.Stop();
				Singleton<BodySearchScreen>.Instance.Open(this.officer, this.officer.BodySearchDuration * num);
				PlayerSingleton<PlayerCamera>.Instance.StopFocus();
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000743B8 File Offset: 0x000725B8
		protected virtual void UpdateMovement()
		{
			if (InstanceFinder.IsServer && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) >= 2f)
			{
				bool flag = false;
				if (!base.Npc.Movement.IsMoving)
				{
					flag = true;
				}
				if (Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.Npc.Movement.CurrentDestination) > 2f)
				{
					flag = true;
				}
				if (flag)
				{
					Vector3 newDestination = this.GetNewDestination();
					if (base.Npc.Movement.CanGetTo(newDestination, 2f))
					{
						this.timeSinceCantReach = 0f;
						base.Npc.Movement.SetDestination(this.GetNewDestination());
						return;
					}
					this.timeSinceCantReach += Time.deltaTime;
					if (this.timeSinceCantReach >= 1f)
					{
						this.Escalate();
					}
				}
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000744A6 File Offset: 0x000726A6
		private void SearchClean()
		{
			Singleton<BodySearchScreen>.Instance.onSearchClear.RemoveListener(new UnityAction(this.SearchClean));
			Singleton<BodySearchScreen>.Instance.onSearchFail.RemoveListener(new UnityAction(this.SearchFail));
			this.ConcludeSearch(true);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000744E5 File Offset: 0x000726E5
		private void SearchFail()
		{
			Singleton<BodySearchScreen>.Instance.onSearchClear.RemoveListener(new UnityAction(this.SearchClean));
			Singleton<BodySearchScreen>.Instance.onSearchFail.RemoveListener(new UnityAction(this.SearchFail));
			this.ConcludeSearch(false);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00074524 File Offset: 0x00072724
		private void UpdateEscalation()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			if (this.searchTime >= 15f && this.TargetPlayer.IsOwner && !Singleton<BodySearchScreen>.Instance.IsOpen)
			{
				this.Escalate();
			}
			if (this.timeOutsideRange >= 4f)
			{
				this.Escalate();
			}
			if (this.TargetPlayer.CurrentVehicle != null)
			{
				this.Escalate();
			}
			if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) > Mathf.Max(15f, this.targetDistanceOnStart + 5f))
			{
				this.Escalate();
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000745D4 File Offset: 0x000727D4
		protected virtual void UpdateLookAt()
		{
			if (this.TargetPlayer != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.TargetPlayer.MimicCamera.position, 10, true);
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0007460C File Offset: 0x0007280C
		protected virtual void UpdateCircle()
		{
			if (this.TargetPlayer == null || this.TargetPlayer != Player.Local)
			{
				this.SetArrestCircleAlpha(0f);
				return;
			}
			float num = Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position);
			if (num < 2f)
			{
				this.SetArrestCircleAlpha(this.ArrestCircle_MaxOpacity);
				this.SetArrestCircleColor(new Color32(75, 165, byte.MaxValue, byte.MaxValue));
				return;
			}
			if (num < this.ArrestCircle_MaxVisibleDistance)
			{
				float arrestCircleAlpha = Mathf.Lerp(this.ArrestCircle_MaxOpacity, 0f, (num - 2f) / (this.ArrestCircle_MaxVisibleDistance - 2f));
				this.SetArrestCircleAlpha(arrestCircleAlpha);
				this.SetArrestCircleColor(Color.white);
				return;
			}
			this.SetArrestCircleAlpha(0f);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000746E8 File Offset: 0x000728E8
		private void SetArrestCircleAlpha(float alpha)
		{
			this.officer.ProxCircle.SetAlpha(alpha);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000746FB File Offset: 0x000728FB
		private void SetArrestCircleColor(Color col)
		{
			this.officer.ProxCircle.SetColor(col);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00074710 File Offset: 0x00072910
		private Vector3 GetNewDestination()
		{
			return this.TargetPlayer.Avatar.CenterPoint + (base.transform.position - this.TargetPlayer.Avatar.CenterPoint).normalized * 1.2f;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00074764 File Offset: 0x00072964
		private void ClearSpeedControls()
		{
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("bodysearching"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("bodysearching");
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0007479C File Offset: 0x0007299C
		private bool IsTargetValid(Player player)
		{
			return !(player == null) && !player.IsArrested && !player.IsSleeping && !player.IsUnconscious && player.Health.IsAlive && player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000747F4 File Offset: 0x000729F4
		[ObserversRpc(RunLocally = true)]
		public virtual void AssignTarget(NetworkConnection conn, NetworkObject target)
		{
			this.RpcWriter___Observers_AssignTarget_1824087381(conn, target);
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00074820 File Offset: 0x00072A20
		public virtual bool DoesPlayerContainItemsOfInterest()
		{
			foreach (ItemSlot itemSlot in PlayerSingleton<PlayerInventory>.Instance.hotbarSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= this.MaxStealthLevel)
						{
							return true;
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000748D0 File Offset: 0x00072AD0
		public virtual void ConcludeSearch(bool clear)
		{
			if (!clear)
			{
				if (this.ShowPostSearchDialogue)
				{
					base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_escalate"), 2f);
				}
				base.Npc.PlayVO(EVOLineType.Angry);
				this.TargetPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(this.TargetPlayer.NetworkObject, true);
				if (this.onSearchComplete_ItemsFound != null)
				{
					this.onSearchComplete_ItemsFound.Invoke();
				}
			}
			else
			{
				this.NoItemsOfInterestFound();
				if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
				{
					base.Npc.PlayVO(EVOLineType.Thanks);
				}
				if (this.onSearchComplete_Clear != null)
				{
					this.onSearchComplete_Clear.Invoke();
				}
				if (this.officer.CheckpointBehaviour.Enabled)
				{
					LandVehicle lastDrivenVehicle = this.TargetPlayer.LastDrivenVehicle;
					CheckpointBehaviour checkpointBehaviour = this.officer.CheckpointBehaviour;
					if (lastDrivenVehicle != null && (checkpointBehaviour.Checkpoint.SearchArea1.vehicles.Contains(lastDrivenVehicle) || checkpointBehaviour.Checkpoint.SearchArea2.vehicles.Contains(lastDrivenVehicle)))
					{
						this.officer.dialogueHandler.ShowWorldspaceDialogue("Thanks. I'll now check your vehicle.", 5f);
						checkpointBehaviour.StartSearch(lastDrivenVehicle.NetworkObject, this.TargetPlayer.NetworkObject);
					}
				}
			}
			base.SendEnd();
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00074A2C File Offset: 0x00072C2C
		public virtual void Escalate()
		{
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			Debug.Log("Escalating!");
			base.Npc.PlayVO(EVOLineType.Angry);
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_escalate"), 2f);
			this.TargetPlayer.CrimeData.AddCrime(new FailureToComply(), 1);
			this.TargetPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
			this.officer.BeginFootPursuit_Networked(this.TargetPlayer.NetworkObject, true);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x00074ABC File Offset: 0x00072CBC
		public virtual void NoItemsOfInterestFound()
		{
			if (this.ShowPostSearchDialogue)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_all_clear"), 3f);
			}
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00074B11 File Offset: 0x00072D11
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_AssignTarget_1824087381));
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00074B41 File Offset: 0x00072D41
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x00074B5A File Offset: 0x00072D5A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00074B68 File Offset: 0x00072D68
		private void RpcWriter___Observers_AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
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
			writer.WriteNetworkObject(target);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x00074C2C File Offset: 0x00072E2C
		public virtual void RpcLogic___AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			this.TargetPlayer = target.GetComponent<Player>();
			this.TargetPlayer.CrimeData.BodySearchPending = true;
			this.searchTime = 0f;
			this.timeWithinSearchRange = 0f;
			this.timeOutsideRange = 0f;
			this.hasBeenInRange = false;
			this.timeSinceCantReach = 0f;
			this.targetDistanceOnStart = Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position);
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00074CB0 File Offset: 0x00072EB0
		private void RpcReader___Observers_AssignTarget_1824087381(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00074CFC File Offset: 0x00072EFC
		protected virtual void dll()
		{
			base.Awake();
			this.officer = (base.Npc as PoliceOfficer);
		}

		// Token: 0x04001727 RID: 5927
		public const EStealthLevel MAX_STEALTH_LEVEL = EStealthLevel.None;

		// Token: 0x04001728 RID: 5928
		public const float BODY_SEARCH_RANGE = 2f;

		// Token: 0x04001729 RID: 5929
		public const float MAX_SEARCH_TIME = 15f;

		// Token: 0x0400172A RID: 5930
		public const float MAX_TIME_OUTSIDE_RANGE = 4f;

		// Token: 0x0400172B RID: 5931
		public const float RANGE_TO_ESCALATE = 15f;

		// Token: 0x0400172C RID: 5932
		public const float MOVE_SPEED = 0.15f;

		// Token: 0x0400172D RID: 5933
		public const float BODY_SEARCH_COOLDOWN = 30f;

		// Token: 0x0400172F RID: 5935
		[Header("Settings")]
		public float ArrestCircle_MaxVisibleDistance = 5f;

		// Token: 0x04001730 RID: 5936
		public float ArrestCircle_MaxOpacity = 0.25f;

		// Token: 0x04001731 RID: 5937
		public bool ShowPostSearchDialogue = true;

		// Token: 0x04001732 RID: 5938
		[Header("Item of interest settings")]
		public EStealthLevel MaxStealthLevel;

		// Token: 0x04001733 RID: 5939
		private PoliceOfficer officer;

		// Token: 0x04001734 RID: 5940
		private float targetDistanceOnStart;

		// Token: 0x04001735 RID: 5941
		private float searchTime;

		// Token: 0x04001736 RID: 5942
		private bool hasBeenInRange;

		// Token: 0x04001737 RID: 5943
		private float timeOutsideRange;

		// Token: 0x04001738 RID: 5944
		private float timeWithinSearchRange;

		// Token: 0x04001739 RID: 5945
		private float timeSinceCantReach;

		// Token: 0x0400173A RID: 5946
		[Header("Events")]
		public UnityEvent onSearchComplete_Clear;

		// Token: 0x0400173B RID: 5947
		public UnityEvent onSearchComplete_ItemsFound;

		// Token: 0x0400173C RID: 5948
		private bool dll_Excuted;

		// Token: 0x0400173D RID: 5949
		private bool dll_Excuted;
	}
}
