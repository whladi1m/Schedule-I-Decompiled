using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005F6 RID: 1526
	public class PlayerVisualState : NetworkBehaviour
	{
		// Token: 0x0600280A RID: 10250 RVA: 0x000A49B8 File Offset: 0x000A2BB8
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.PlayerVisualState_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000A49CC File Offset: 0x000A2BCC
		private void Update()
		{
			if (NetworkSingleton<CurfewManager>.InstanceExists && NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActiveWithTolerance && this.player.CurrentProperty == null && this.player.CurrentBusiness == null)
			{
				if (this.GetState("DisobeyingCurfew") == null)
				{
					this.ApplyState("DisobeyingCurfew", PlayerVisualState.EVisualState.DisobeyingCurfew, 0f);
				}
			}
			else if (this.GetState("DisobeyingCurfew") != null)
			{
				this.RemoveState("DisobeyingCurfew", 0f);
			}
			this.UpdateSuspiciousness();
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000A4A58 File Offset: 0x000A2C58
		[ServerRpc(RunLocally = true)]
		public void ApplyState(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
		{
			this.RpcWriter___Server_ApplyState_868472085(label, state, autoRemoveAfter);
			this.RpcLogic___ApplyState_868472085(label, state, autoRemoveAfter);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000A4A8C File Offset: 0x000A2C8C
		[ServerRpc(RunLocally = true)]
		public void RemoveState(string label, float delay = 0f)
		{
			this.RpcWriter___Server_RemoveState_606697822(label, delay);
			this.RpcLogic___RemoveState_606697822(label, delay);
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000A4AB8 File Offset: 0x000A2CB8
		public PlayerVisualState.VisualState GetState(string label)
		{
			return this.visualStates.Find((PlayerVisualState.VisualState x) => x.label == label);
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000A4AEC File Offset: 0x000A2CEC
		public void ClearStates()
		{
			PlayerVisualState.VisualState[] array = this.visualStates.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].label == "Visible"))
				{
					this.RemoveState(array[i].label, 0f);
				}
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000A4B3C File Offset: 0x000A2D3C
		private void UpdateSuspiciousness()
		{
			this.Suspiciousness = 0f;
			if (this.player.Avatar.Anim.IsCrouched)
			{
				this.Suspiciousness += 0.3f;
			}
			if (this.player.Avatar.CurrentEquippable != null)
			{
				this.Suspiciousness += this.player.Avatar.CurrentEquippable.Suspiciousness;
			}
			if (this.player.VelocityCalculator.Velocity.magnitude > PlayerMovement.WalkSpeed)
			{
				this.Suspiciousness += 0.3f * Mathf.InverseLerp(PlayerMovement.WalkSpeed, PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier, this.player.VelocityCalculator.Velocity.magnitude);
			}
			this.Suspiciousness = Mathf.Clamp01(this.Suspiciousness);
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000A4C48 File Offset: 0x000A2E48
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_ApplyState_868472085));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_RemoveState_606697822));
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000A4C94 File Offset: 0x000A2E94
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000A4CA7 File Offset: 0x000A2EA7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000A4CB8 File Offset: 0x000A2EB8
		private void RpcWriter___Server_ApplyState_868472085(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(label);
			writer.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated(state);
			writer.WriteSingle(autoRemoveAfter, AutoPackType.Unpacked);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000A4DD8 File Offset: 0x000A2FD8
		public void RpcLogic___ApplyState_868472085(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
		{
			PlayerVisualState.VisualState visualState = this.GetState(label);
			if (visualState == null)
			{
				visualState = new PlayerVisualState.VisualState();
				visualState.label = label;
				this.visualStates.Add(visualState);
			}
			visualState.state = state;
			if (this.removalRoutinesDict.ContainsKey(label))
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.removalRoutinesDict[label]);
				this.removalRoutinesDict.Remove(label);
			}
			if (autoRemoveAfter > 0f)
			{
				this.RemoveState(label, autoRemoveAfter);
			}
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000A4E54 File Offset: 0x000A3054
		private void RpcReader___Server_ApplyState_868472085(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string label = PooledReader0.ReadString();
			PlayerVisualState.EVisualState state = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds(PooledReader0);
			float autoRemoveAfter = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ApplyState_868472085(label, state, autoRemoveAfter);
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000A4ECC File Offset: 0x000A30CC
		private void RpcWriter___Server_RemoveState_606697822(string label, float delay = 0f)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(label);
			writer.WriteSingle(delay, AutoPackType.Unpacked);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000A4FE0 File Offset: 0x000A31E0
		public void RpcLogic___RemoveState_606697822(string label, float delay = 0f)
		{
			PlayerVisualState.<>c__DisplayClass9_0 CS$<>8__locals1 = new PlayerVisualState.<>c__DisplayClass9_0();
			CS$<>8__locals1.delay = delay;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.label = label;
			CS$<>8__locals1.newState = this.GetState(CS$<>8__locals1.label);
			if (CS$<>8__locals1.newState == null)
			{
				return;
			}
			if (CS$<>8__locals1.delay > 0f)
			{
				if (this.removalRoutinesDict.ContainsKey(CS$<>8__locals1.label))
				{
					Singleton<CoroutineService>.Instance.StopCoroutine(this.removalRoutinesDict[CS$<>8__locals1.label]);
					this.removalRoutinesDict.Remove(CS$<>8__locals1.label);
				}
				this.removalRoutinesDict.Add(CS$<>8__locals1.label, Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<RemoveState>g__DelayedRemove|0()));
				return;
			}
			CS$<>8__locals1.<RemoveState>g__Destroy|1();
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000A5098 File Offset: 0x000A3298
		private void RpcReader___Server_RemoveState_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string label = PooledReader0.ReadString();
			float delay = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RemoveState_606697822(label, delay);
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000A50FD File Offset: 0x000A32FD
		private void dll()
		{
			this.player = base.GetComponent<Player>();
			this.player.Health.onDie.AddListener(delegate()
			{
				this.ClearStates();
			});
			this.ApplyState("Visible", PlayerVisualState.EVisualState.Visible, 0f);
		}

		// Token: 0x04001D2B RID: 7467
		public float Suspiciousness;

		// Token: 0x04001D2C RID: 7468
		public List<PlayerVisualState.VisualState> visualStates = new List<PlayerVisualState.VisualState>();

		// Token: 0x04001D2D RID: 7469
		private Player player;

		// Token: 0x04001D2E RID: 7470
		private Dictionary<string, Coroutine> removalRoutinesDict = new Dictionary<string, Coroutine>();

		// Token: 0x04001D2F RID: 7471
		private bool dll_Excuted;

		// Token: 0x04001D30 RID: 7472
		private bool dll_Excuted;

		// Token: 0x020005F7 RID: 1527
		public enum EVisualState
		{
			// Token: 0x04001D32 RID: 7474
			Visible,
			// Token: 0x04001D33 RID: 7475
			Suspicious,
			// Token: 0x04001D34 RID: 7476
			DisobeyingCurfew,
			// Token: 0x04001D35 RID: 7477
			Vandalizing,
			// Token: 0x04001D36 RID: 7478
			PettyCrime,
			// Token: 0x04001D37 RID: 7479
			DrugDealing,
			// Token: 0x04001D38 RID: 7480
			SearchedFor,
			// Token: 0x04001D39 RID: 7481
			Wanted,
			// Token: 0x04001D3A RID: 7482
			Pickpocketing,
			// Token: 0x04001D3B RID: 7483
			DischargingWeapon,
			// Token: 0x04001D3C RID: 7484
			Brandishing
		}

		// Token: 0x020005F8 RID: 1528
		[Serializable]
		public class VisualState
		{
			// Token: 0x04001D3D RID: 7485
			public PlayerVisualState.EVisualState state;

			// Token: 0x04001D3E RID: 7486
			public string label;

			// Token: 0x04001D3F RID: 7487
			public Action stateDestroyed;
		}
	}
}
