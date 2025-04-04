using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000467 RID: 1127
	public class NPCEvent_Conversate : NPCEvent
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x00069E3F File Offset: 0x0006803F
		public new string ActionName
		{
			get
			{
				return "Conversate";
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001806 RID: 6150 RVA: 0x00069E46 File Offset: 0x00068046
		private Transform StandPoint
		{
			get
			{
				return this.Location.GetStandPoint(this.npc);
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00069E5C File Offset: 0x0006805C
		public override string GetName()
		{
			if (this.Location == null)
			{
				return this.ActionName + " (No destination set)";
			}
			return this.ActionName + " (" + this.Location.gameObject.name + ")";
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00069EAD File Offset: 0x000680AD
		protected override void Start()
		{
			base.Start();
			this.Location.NPCs.Add(this.npc);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00069ECB File Offset: 0x000680CB
		public override void Started()
		{
			base.Started();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00069EF8 File Offset: 0x000680F8
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
			if (this.npc.Movement.IsMoving)
			{
				this.Location.SetNPCReady(this.npc, false);
				this.timeAtDestination = 0f;
				return;
			}
			if (this.IsAtDestination())
			{
				this.Location.SetNPCReady(this.npc, true);
				this.timeAtDestination += Time.deltaTime;
				return;
			}
			this.Location.SetNPCReady(this.npc, false);
			this.timeAtDestination = 0f;
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00069F98 File Offset: 0x00068198
		public override void MinPassed()
		{
			base.MinPassed();
			if (InstanceFinder.IsServer)
			{
				if (!this.IsConversating && this.timeAtDestination >= 0.1f && this.CanConversationStart())
				{
					this.StartConversate();
				}
				if (!this.IsConversating && !this.IsWaiting && this.timeAtDestination >= 3f && !this.CanConversationStart())
				{
					this.StartWait();
				}
				if (this.IsConversating && !this.CanConversationStart())
				{
					this.EndConversate();
				}
			}
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0006A016 File Offset: 0x00068216
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0006A040 File Offset: 0x00068240
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.StandPoint.position);
				}
				this.npc.Movement.FaceDirection(this.StandPoint.forward, 0.5f);
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0006A0BF File Offset: 0x000682BF
		public override void End()
		{
			base.End();
			this.Location.SetNPCReady(this.npc, false);
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			if (this.IsConversating)
			{
				this.EndConversate();
			}
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0006A0F8 File Offset: 0x000682F8
		public override void Interrupt()
		{
			base.Interrupt();
			this.Location.SetNPCReady(this.npc, false);
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			if (this.IsConversating)
			{
				this.EndConversate();
			}
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0006A15B File Offset: 0x0006835B
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0006A185 File Offset: 0x00068385
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.StandPoint.position) < 1f;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0006A1AE File Offset: 0x000683AE
		private bool CanConversationStart()
		{
			return this.Location.NPCsReady;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0006A1BB File Offset: 0x000683BB
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			this.npc.Movement.FaceDirection(this.StandPoint.forward, 0.5f);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0006A1F2 File Offset: 0x000683F2
		[ObserversRpc(RunLocally = true)]
		protected virtual void StartWait()
		{
			this.RpcWriter___Observers_StartWait_2166136261();
			this.RpcLogic___StartWait_2166136261();
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0006A200 File Offset: 0x00068400
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndWait()
		{
			this.RpcWriter___Observers_EndWait_2166136261();
			this.RpcLogic___EndWait_2166136261();
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0006A20E File Offset: 0x0006840E
		[ObserversRpc(RunLocally = true)]
		protected virtual void StartConversate()
		{
			this.RpcWriter___Observers_StartConversate_2166136261();
			this.RpcLogic___StartConversate_2166136261();
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0006A21C File Offset: 0x0006841C
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndConversate()
		{
			this.RpcWriter___Observers_EndConversate_2166136261();
			this.RpcLogic___EndConversate_2166136261();
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0006A282 File Offset: 0x00068482
		[CompilerGenerated]
		private IEnumerator <StartConversate>g__Routine|30_0()
		{
			while (this.IsConversating)
			{
				UnityEngine.Random.InitState(this.npc.fullName.GetHashCode() + (int)Time.time);
				float wait = UnityEngine.Random.Range(2f, 8f);
				NPC otherNPC = this.Location.GetOtherNPC(this.npc);
				for (float t = 0f; t < wait; t += Time.deltaTime)
				{
					if (!this.IsConversating)
					{
						yield break;
					}
					this.npc.Avatar.LookController.OverrideLookTarget(otherNPC.Avatar.LookController.HeadBone.position, 1, false);
					yield return new WaitForEndOfFrame();
				}
				this.npc.VoiceOverEmitter.Play(this.ConversationLines[UnityEngine.Random.Range(0, this.ConversationLines.Length)]);
				this.npc.Avatar.Anim.SetTrigger(this.AnimationTriggers[UnityEngine.Random.Range(0, this.AnimationTriggers.Length)]);
				otherNPC = null;
			}
			yield break;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0006A294 File Offset: 0x00068494
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartWait_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_EndWait_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_StartConversate_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_EndConversate_2166136261));
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0006A314 File Offset: 0x00068514
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0006A32D File Offset: 0x0006852D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0006A33C File Offset: 0x0006853C
		private void RpcWriter___Observers_StartWait_2166136261()
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
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0006A3E5 File Offset: 0x000685E5
		protected virtual void RpcLogic___StartWait_2166136261()
		{
			if (this.IsWaiting)
			{
				return;
			}
			this.IsWaiting = true;
			if (this.OnWaitStart != null)
			{
				this.OnWaitStart.Invoke();
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0006A40C File Offset: 0x0006860C
		private void RpcReader___Observers_StartWait_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartWait_2166136261();
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0006A438 File Offset: 0x00068638
		private void RpcWriter___Observers_EndWait_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0006A4E1 File Offset: 0x000686E1
		protected virtual void RpcLogic___EndWait_2166136261()
		{
			if (!this.IsWaiting)
			{
				return;
			}
			this.IsWaiting = false;
			if (this.OnWaitEnd != null)
			{
				this.OnWaitEnd.Invoke();
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0006A508 File Offset: 0x00068708
		private void RpcReader___Observers_EndWait_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndWait_2166136261();
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0006A534 File Offset: 0x00068734
		private void RpcWriter___Observers_StartConversate_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0006A5DD File Offset: 0x000687DD
		protected virtual void RpcLogic___StartConversate_2166136261()
		{
			if (this.IsConversating)
			{
				return;
			}
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			this.IsConversating = true;
			this.conversateRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<StartConversate>g__Routine|30_0());
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0006A614 File Offset: 0x00068814
		private void RpcReader___Observers_StartConversate_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartConversate_2166136261();
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0006A640 File Offset: 0x00068840
		private void RpcWriter___Observers_EndConversate_2166136261()
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
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0006A6E9 File Offset: 0x000688E9
		protected virtual void RpcLogic___EndConversate_2166136261()
		{
			if (!this.IsConversating)
			{
				return;
			}
			this.IsConversating = false;
			this.timeAtDestination = 0f;
			if (this.conversateRoutine != null)
			{
				base.StopCoroutine(this.conversateRoutine);
			}
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0006A71C File Offset: 0x0006891C
		private void RpcReader___Observers_EndConversate_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndConversate_2166136261();
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0006A746 File Offset: 0x00068946
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400158D RID: 5517
		private EVOLineType[] ConversationLines = new EVOLineType[]
		{
			EVOLineType.Greeting,
			EVOLineType.Question,
			EVOLineType.Surprised,
			EVOLineType.Alerted,
			EVOLineType.Annoyed,
			EVOLineType.Acknowledge,
			EVOLineType.Think,
			EVOLineType.No
		};

		// Token: 0x0400158E RID: 5518
		private string[] AnimationTriggers = new string[]
		{
			"ThumbsUp",
			"DisagreeWave",
			"Nod",
			"ConversationGesture1"
		};

		// Token: 0x0400158F RID: 5519
		public const float DESTINATION_THRESHOLD = 1f;

		// Token: 0x04001590 RID: 5520
		public const float TIME_BEFORE_WAIT_START = 3f;

		// Token: 0x04001591 RID: 5521
		public ConversationLocation Location;

		// Token: 0x04001592 RID: 5522
		private bool IsConversating;

		// Token: 0x04001593 RID: 5523
		private Coroutine conversateRoutine;

		// Token: 0x04001594 RID: 5524
		private bool IsWaiting;

		// Token: 0x04001595 RID: 5525
		public UnityEvent OnWaitStart;

		// Token: 0x04001596 RID: 5526
		public UnityEvent OnWaitEnd;

		// Token: 0x04001597 RID: 5527
		private float timeAtDestination;

		// Token: 0x04001598 RID: 5528
		private bool dll_Excuted;

		// Token: 0x04001599 RID: 5529
		private bool dll_Excuted;
	}
}
