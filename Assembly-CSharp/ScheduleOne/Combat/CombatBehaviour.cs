using System;
using System.Collections;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vision;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Combat
{
	// Token: 0x0200072C RID: 1836
	public class CombatBehaviour : ScheduleOne.NPCs.Behaviour.Behaviour
	{
		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x000CE865 File Offset: 0x000CCA65
		// (set) Token: 0x060031B0 RID: 12720 RVA: 0x000CE86D File Offset: 0x000CCA6D
		public Player TargetPlayer { get; protected set; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x000CE876 File Offset: 0x000CCA76
		// (set) Token: 0x060031B2 RID: 12722 RVA: 0x000CE87E File Offset: 0x000CCA7E
		public bool IsSearching { get; protected set; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x000CE887 File Offset: 0x000CCA87
		// (set) Token: 0x060031B4 RID: 12724 RVA: 0x000CE88F File Offset: 0x000CCA8F
		public float TimeSinceTargetReacquired { get; protected set; }

		// Token: 0x060031B5 RID: 12725 RVA: 0x000CE898 File Offset: 0x000CCA98
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Combat.CombatBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000CE8B7 File Offset: 0x000CCAB7
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.Active && this.TargetPlayer != null)
			{
				this.SetTarget(connection, this.TargetPlayer.NetworkObject);
			}
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000CE8E8 File Offset: 0x000CCAE8
		[ObserversRpc(RunLocally = true)]
		public virtual void SetTarget(NetworkConnection conn, NetworkObject target)
		{
			this.RpcWriter___Observers_SetTarget_1824087381(conn, target);
			this.RpcLogic___SetTarget_1824087381(conn, target);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000CE906 File Offset: 0x000CCB06
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "post combat", 120f, 1);
			this.StartCombat();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000CE939 File Offset: 0x000CCB39
		protected override void Resume()
		{
			base.Resume();
			this.StartCombat();
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000CE947 File Offset: 0x000CCB47
		protected override void Pause()
		{
			base.Pause();
			this.EndCombat();
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000CE955 File Offset: 0x000CCB55
		protected override void End()
		{
			base.End();
			this.EndCombat();
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000CE963 File Offset: 0x000CCB63
		public override void Disable()
		{
			base.Disable();
			this.TargetPlayer = null;
			this.End();
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000CE978 File Offset: 0x000CCB78
		protected virtual void StartCombat()
		{
			this.CheckPlayerVisibility();
			this.isTargetRecentlyVisible = true;
			this.SetMovementSpeed(this.DefaultMovementSpeed);
			base.Npc.Movement.SetStance(NPCMovement.EStance.Stanced);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Angry", "combat", 0f, 3);
			if (InstanceFinder.IsServer && this.DefaultWeapon != null)
			{
				this.SetWeapon(this.DefaultWeapon.AssetPath);
			}
			this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			this.successfulHits = 0;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000CEA30 File Offset: 0x000CCC30
		protected void EndCombat()
		{
			this.StopSearching();
			if (InstanceFinder.IsServer && this.currentWeapon != null)
			{
				this.ClearWeapon();
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("combat");
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			base.Npc.Movement.SetStance(NPCMovement.EStance.None);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("combat");
			if (this.TargetPlayer != null)
			{
				base.Npc.awareness.VisionCone.StateSettings[this.TargetPlayer][PlayerVisualState.EVisualState.Visible].Enabled = false;
			}
			this.timeSinceLastSighting = 10000f;
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000CEAFC File Offset: 0x000CCCFC
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.UpdateLookAt();
			if (InstanceFinder.IsServer && !this.IsTargetValid())
			{
				base.Disable_Networked(null);
				return;
			}
			if (Time.time > this.nextAngryVO && this.PlayAngryVO)
			{
				EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Angry : EVOLineType.Command;
				base.Npc.PlayVO(lineType);
				this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			}
			if (this.isTargetRecentlyVisible)
			{
				this.lastKnownTargetPosition = this.TargetPlayer.Avatar.CenterPoint;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsSearching)
			{
				if (!this.isTargetImmediatelyVisible)
				{
					Console.Log("Combat action: searching", null);
					return;
				}
				this.StopSearching();
			}
			Vector3 centerPoint = base.Npc.Avatar.CenterPoint;
			if (base.Npc.Movement.IsMoving)
			{
				Vector3 currentDestination = base.Npc.Movement.CurrentDestination;
			}
			if (this.isTargetRecentlyVisible)
			{
				if (this.IsTargetInRange(base.Npc.transform.position + Vector3.up * 1f) && this.isTargetImmediatelyVisible)
				{
					if (this.ReadyToAttack(false))
					{
						Console.Log("Combat action: attack", null);
						this.Attack();
						return;
					}
				}
				else if (!this.IsTargetInRange(base.Npc.Movement.CurrentDestination) || !base.Npc.Movement.IsMoving)
				{
					Console.Log("Combat action: reposition 1", null);
					this.RepositionToTargetRange(this.lastKnownTargetPosition);
					return;
				}
			}
			else if (base.Npc.Movement.IsMoving)
			{
				if (Vector3.Distance(base.Npc.Movement.CurrentDestination, this.lastKnownTargetPosition) > 2f)
				{
					Console.Log("Combat action: reposition 2", null);
					base.Npc.Movement.SetDestination(this.lastKnownTargetPosition);
					return;
				}
			}
			else
			{
				if (Vector3.Distance(base.transform.position, this.lastKnownTargetPosition) < 2f)
				{
					this.StartSearching();
					return;
				}
				Console.Log("Combat action: reposition 3", null);
				base.Npc.Movement.SetDestination(this.lastKnownTargetPosition);
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x000CED32 File Offset: 0x000CCF32
		protected virtual void FixedUpdate()
		{
			if (!base.Active)
			{
				return;
			}
			this.CheckPlayerVisibility();
			this.UpdateTimeout();
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000CED49 File Offset: 0x000CCF49
		protected void UpdateTimeout()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.timeSinceLastSighting > this.GetSearchTime())
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000CED68 File Offset: 0x000CCF68
		protected virtual void UpdateLookAt()
		{
			if (this.isTargetImmediatelyVisible && this.TargetPlayer != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.TargetPlayer.MimicCamera.position, 10, true);
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000CEDA8 File Offset: 0x000CCFA8
		protected void SetMovementSpeed(float speed)
		{
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("combat", 5, speed));
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000CEDCC File Offset: 0x000CCFCC
		[ObserversRpc(RunLocally = true)]
		protected virtual void SetWeapon(string weaponPath)
		{
			this.RpcWriter___Observers_SetWeapon_3615296227(weaponPath);
			this.RpcLogic___SetWeapon_3615296227(weaponPath);
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x000CEDF0 File Offset: 0x000CCFF0
		[ObserversRpc(RunLocally = true)]
		protected void ClearWeapon()
		{
			this.RpcWriter___Observers_ClearWeapon_2166136261();
			this.RpcLogic___ClearWeapon_2166136261();
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000CEE09 File Offset: 0x000CD009
		protected virtual bool ReadyToAttack(bool checkTarget = true)
		{
			if (this.TimeSinceTargetReacquired < 0.5f && checkTarget)
			{
				return false;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.IsReadyToAttack();
			}
			return this.VirtualPunchWeapon.IsReadyToAttack();
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000CEE43 File Offset: 0x000CD043
		[ObserversRpc(RunLocally = true)]
		protected virtual void Attack()
		{
			this.RpcWriter___Observers_Attack_2166136261();
			this.RpcLogic___Attack_2166136261();
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000CEE51 File Offset: 0x000CD051
		protected void SucessfulHit()
		{
			this.successfulHits++;
			if (this.GiveUpAfterSuccessfulHits > 0 && this.successfulHits >= this.GiveUpAfterSuccessfulHits)
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000CEE80 File Offset: 0x000CD080
		protected void CheckPlayerVisibility()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			base.Npc.awareness.VisionCone.StateSettings[this.TargetPlayer][PlayerVisualState.EVisualState.Visible].Enabled = !this.isTargetRecentlyVisible;
			Console.Log("Target visible: " + this.IsPlayerVisible().ToString(), null);
			if (this.IsPlayerVisible())
			{
				this.playerSightedDuration += Time.fixedDeltaTime;
				this.isTargetImmediatelyVisible = true;
				this.isTargetRecentlyVisible = true;
			}
			else
			{
				this.playerSightedDuration = 0f;
				this.timeSinceLastSighting += Time.fixedDeltaTime;
				this.isTargetImmediatelyVisible = false;
				if (this.timeSinceLastSighting < 2.5f)
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
					this.isTargetRecentlyVisible = true;
				}
				else
				{
					this.isTargetRecentlyVisible = false;
				}
			}
			if (this.isTargetRecentlyVisible)
			{
				this.MarkPlayerVisible();
			}
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000CEF78 File Offset: 0x000CD178
		public void MarkPlayerVisible()
		{
			if (this.IsPlayerVisible())
			{
				this.TargetPlayer.CrimeData.RecordLastKnownPosition(true);
				this.timeSinceLastSighting = 0f;
				return;
			}
			this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000CEFB0 File Offset: 0x000CD1B0
		protected bool IsPlayerVisible()
		{
			return base.Npc.awareness.VisionCone.IsPlayerVisible(this.TargetPlayer);
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x000CEFD0 File Offset: 0x000CD1D0
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject)
			{
				if (!this.isTargetRecentlyVisible)
				{
					this.TimeSinceTargetReacquired = 0f;
				}
				this.isTargetRecentlyVisible = true;
				this.isTargetImmediatelyVisible = true;
				if (this.PlayAngryVO)
				{
					base.Npc.PlayVO(EVOLineType.Angry);
					this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
				}
			}
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000CF04F File Offset: 0x000CD24F
		protected virtual float GetSearchTime()
		{
			return this.DefaultSearchTime;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000CF058 File Offset: 0x000CD258
		private void StartSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Console.Log("Combat action: start searching", null);
			this.IsSearching = true;
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("searching", 6, 0.4f));
			this.searchRoutine = base.StartCoroutine(this.SearchRoutine());
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000CF0B8 File Offset: 0x000CD2B8
		private void StopSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Console.Log("Combat action: stop searching", null);
			this.IsSearching = false;
			base.Npc.Movement.SpeedController.RemoveSpeedControl("searching");
			this.hasSearchDestination = false;
			if (this.searchRoutine != null)
			{
				base.StopCoroutine(this.searchRoutine);
			}
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000CF114 File Offset: 0x000CD314
		private IEnumerator SearchRoutine()
		{
			while (this.IsSearching)
			{
				if (!this.hasSearchDestination)
				{
					this.currentSearchDestination = this.GetNextSearchLocation();
					base.Npc.Movement.SetDestination(this.currentSearchDestination);
					this.hasSearchDestination = true;
				}
				for (;;)
				{
					if (!base.Npc.Movement.IsMoving && base.Npc.Movement.CanMove())
					{
						base.Npc.Movement.SetDestination(this.currentSearchDestination);
					}
					if (Vector3.Distance(base.transform.position, this.currentSearchDestination) < 2f)
					{
						break;
					}
					yield return new WaitForSeconds(1f);
				}
				this.hasSearchDestination = false;
				yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 6f));
			}
			this.searchRoutine = null;
			this.StopSearching();
			yield break;
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000CF124 File Offset: 0x000CD324
		private Vector3 GetNextSearchLocation()
		{
			float num = Mathf.Lerp(25f, 60f, Mathf.Clamp(this.timeSinceLastSighting / this.TargetPlayer.CrimeData.GetSearchTime(), 0f, 1f));
			num = Mathf.Min(num, Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint));
			return this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, num, 0f);
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000CF1AC File Offset: 0x000CD3AC
		protected bool IsTargetValid()
		{
			return !(this.TargetPlayer == null) && !this.TargetPlayer.IsArrested && !this.TargetPlayer.IsUnconscious && this.TargetPlayer.Health.IsAlive && !this.TargetPlayer.CrimeData.BodySearchPending && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) <= this.GiveUpRange;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000CF23C File Offset: 0x000CD43C
		private void RepositionToTargetRange(Vector3 origin)
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			Console.Log("Repositioning to target range", null);
			Vector3 randomReachablePointNear = this.GetRandomReachablePointNear(origin, this.GetMaxTargetDistance(), this.GetMinTargetDistance());
			base.Npc.Movement.SetDestination(randomReachablePointNear);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000CF288 File Offset: 0x000CD488
		private Vector3 GetRandomReachablePointNear(Vector3 point, float randomRadius, float minDistance = 0f)
		{
			bool flag = false;
			Vector3 result = point;
			int num = 0;
			while (!flag)
			{
				Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
				Vector3 normalized = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y).normalized;
				NavMeshHit navMeshHit;
				NavMeshUtility.SamplePosition(point + normalized * randomRadius, out navMeshHit, 5f, base.Npc.Movement.Agent.areaMask, true);
				if (base.Npc.Movement.CanGetTo(navMeshHit.position, 2f) && Vector3.Distance(point, navMeshHit.position) > minDistance)
				{
					result = navMeshHit.position;
					break;
				}
				num++;
				if (num > 10)
				{
					Console.LogError("Failed to find search destination", null);
					break;
				}
			}
			return result;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000CF34F File Offset: 0x000CD54F
		protected float GetMinTargetDistance()
		{
			if (this.overrideTargetDistance)
			{
				return this.targetDistanceOverride;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.MinUseRange;
			}
			return 0f;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000CF37F File Offset: 0x000CD57F
		protected float GetMaxTargetDistance()
		{
			if (this.overrideTargetDistance)
			{
				return this.targetDistanceOverride;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.MaxUseRange;
			}
			return 1.5f;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000CF3B0 File Offset: 0x000CD5B0
		protected bool IsTargetInRange(Vector3 origin = default(Vector3))
		{
			if (origin == default(Vector3))
			{
				origin = base.transform.position;
			}
			float num = Vector3.Distance(origin, this.TargetPlayer.Avatar.CenterPoint);
			return num > this.GetMinTargetDistance() && num < this.GetMaxTargetDistance();
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000CF470 File Offset: 0x000CD670
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetTarget_1824087381));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetWeapon_3615296227));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_ClearWeapon_2166136261));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_Attack_2166136261));
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x000CF4F0 File Offset: 0x000CD6F0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000CF509 File Offset: 0x000CD709
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000CF518 File Offset: 0x000CD718
		private void RpcWriter___Observers_SetTarget_1824087381(NetworkConnection conn, NetworkObject target)
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

		// Token: 0x060031DD RID: 12765 RVA: 0x000CF5DB File Offset: 0x000CD7DB
		public virtual void RpcLogic___SetTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			this.TargetPlayer = target.GetComponent<Player>();
			this.playerSightedDuration = 0f;
			this.timeSinceLastSighting = 0f;
			this.TimeSinceTargetReacquired = 0f;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000CF60C File Offset: 0x000CD80C
		private void RpcReader___Observers_SetTarget_1824087381(PooledReader PooledReader0, Channel channel)
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
			this.RpcLogic___SetTarget_1824087381(conn, target);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000CF658 File Offset: 0x000CD858
		private void RpcWriter___Observers_SetWeapon_3615296227(string weaponPath)
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
			writer.WriteString(weaponPath);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000CF710 File Offset: 0x000CD910
		protected virtual void RpcLogic___SetWeapon_3615296227(string weaponPath)
		{
			if (this.currentWeapon != null)
			{
				if (weaponPath == this.currentWeapon.AssetPath)
				{
					return;
				}
				this.ClearWeapon();
			}
			if (weaponPath == string.Empty)
			{
				return;
			}
			this.VirtualPunchWeapon.onSuccessfulHit.RemoveListener(new UnityAction(this.SucessfulHit));
			this.currentWeapon = (base.Npc.SetEquippable_Return(weaponPath) as AvatarWeapon);
			this.currentWeapon.onSuccessfulHit.AddListener(new UnityAction(this.SucessfulHit));
			if (this.currentWeapon == null)
			{
				Console.LogError("Failed to equip weapon", null);
				return;
			}
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000CF7BC File Offset: 0x000CD9BC
		private void RpcReader___Observers_SetWeapon_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string weaponPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetWeapon_3615296227(weaponPath);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000CF7F8 File Offset: 0x000CD9F8
		private void RpcWriter___Observers_ClearWeapon_2166136261()
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

		// Token: 0x060031E3 RID: 12771 RVA: 0x000CF8A4 File Offset: 0x000CDAA4
		protected void RpcLogic___ClearWeapon_2166136261()
		{
			if (this.currentWeapon == null)
			{
				return;
			}
			this.currentWeapon.onSuccessfulHit.RemoveListener(new UnityAction(this.SucessfulHit));
			base.Npc.SetEquippable_Networked(null, string.Empty);
			this.currentWeapon = null;
			this.VirtualPunchWeapon.onSuccessfulHit.AddListener(new UnityAction(this.SucessfulHit));
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000CF910 File Offset: 0x000CDB10
		private void RpcReader___Observers_ClearWeapon_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearWeapon_2166136261();
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000CF93C File Offset: 0x000CDB3C
		private void RpcWriter___Observers_Attack_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000CF9E5 File Offset: 0x000CDBE5
		protected virtual void RpcLogic___Attack_2166136261()
		{
			if (!this.ReadyToAttack(false))
			{
				return;
			}
			if (this.currentWeapon != null)
			{
				this.currentWeapon.Attack();
				return;
			}
			this.VirtualPunchWeapon.Attack();
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000CFA18 File Offset: 0x000CDC18
		private void RpcReader___Observers_Attack_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Attack_2166136261();
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000CFA44 File Offset: 0x000CDC44
		protected virtual void dll()
		{
			base.Awake();
			VisionCone visionCone = base.Npc.awareness.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
			this.VirtualPunchWeapon.Equip(base.Npc.Avatar);
		}

		// Token: 0x04002391 RID: 9105
		public const float EXTRA_VISIBILITY_TIME = 2.5f;

		// Token: 0x04002392 RID: 9106
		public const float SEARCH_RADIUS_MIN = 25f;

		// Token: 0x04002393 RID: 9107
		public const float SEARCH_RADIUS_MAX = 60f;

		// Token: 0x04002394 RID: 9108
		public const float SEARCH_SPEED = 0.4f;

		// Token: 0x04002395 RID: 9109
		public const float CONSECUTIVE_MISS_ACCURACY_BOOST = 0.1f;

		// Token: 0x04002396 RID: 9110
		public const float REACHED_DESTINATION_DISTANCE = 2f;

		// Token: 0x0400239A RID: 9114
		[Header("General Setttings")]
		public float GiveUpRange = 20f;

		// Token: 0x0400239B RID: 9115
		public float GiveUpTime = 30f;

		// Token: 0x0400239C RID: 9116
		public int GiveUpAfterSuccessfulHits;

		// Token: 0x0400239D RID: 9117
		public bool PlayAngryVO = true;

		// Token: 0x0400239E RID: 9118
		[Header("Movement settings")]
		[Range(0f, 1f)]
		public float DefaultMovementSpeed = 0.6f;

		// Token: 0x0400239F RID: 9119
		[Header("Weapon settings")]
		public AvatarWeapon DefaultWeapon;

		// Token: 0x040023A0 RID: 9120
		public AvatarMeleeWeapon VirtualPunchWeapon;

		// Token: 0x040023A1 RID: 9121
		[Header("Search settings")]
		public float DefaultSearchTime = 30f;

		// Token: 0x040023A2 RID: 9122
		protected bool overrideTargetDistance;

		// Token: 0x040023A3 RID: 9123
		protected float targetDistanceOverride;

		// Token: 0x040023A4 RID: 9124
		protected bool isTargetRecentlyVisible;

		// Token: 0x040023A5 RID: 9125
		protected bool isTargetImmediatelyVisible;

		// Token: 0x040023A6 RID: 9126
		protected float timeSinceLastSighting = 10000f;

		// Token: 0x040023A7 RID: 9127
		protected float playerSightedDuration;

		// Token: 0x040023A8 RID: 9128
		protected Vector3 lastKnownTargetPosition = Vector3.zero;

		// Token: 0x040023A9 RID: 9129
		protected AvatarWeapon currentWeapon;

		// Token: 0x040023AA RID: 9130
		protected int successfulHits;

		// Token: 0x040023AB RID: 9131
		protected int consecutiveMissedShots;

		// Token: 0x040023AC RID: 9132
		protected Coroutine rangedWeaponRoutine;

		// Token: 0x040023AD RID: 9133
		protected Coroutine searchRoutine;

		// Token: 0x040023AE RID: 9134
		protected Vector3 currentSearchDestination = Vector3.zero;

		// Token: 0x040023AF RID: 9135
		protected bool hasSearchDestination;

		// Token: 0x040023B0 RID: 9136
		private float nextAngryVO;

		// Token: 0x040023B1 RID: 9137
		private bool dll_Excuted;

		// Token: 0x040023B2 RID: 9138
		private bool dll_Excuted;
	}
}
