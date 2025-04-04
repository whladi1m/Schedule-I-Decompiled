using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200050D RID: 1293
	public class FacePlayerBehaviour : Behaviour
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x0007EB53 File Offset: 0x0007CD53
		// (set) Token: 0x06001EEE RID: 7918 RVA: 0x0007EB5B File Offset: 0x0007CD5B
		public Player Player { get; private set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x0007EB64 File Offset: 0x0007CD64
		// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x0007EB6C File Offset: 0x0007CD6C
		public float Countdown { get; private set; }

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0007EB78 File Offset: 0x0007CD78
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetTarget(NetworkObject player, float countDown = 5f)
		{
			this.RpcWriter___Server_SetTarget_244313061(player, countDown);
			this.RpcLogic___SetTarget_244313061(player, countDown);
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0007EBA1 File Offset: 0x0007CDA1
		[ObserversRpc(RunLocally = true)]
		private void SetTargetLocal(NetworkObject player)
		{
			this.RpcWriter___Observers_SetTargetLocal_3323014238(player);
			this.RpcLogic___SetTargetLocal_3323014238(player);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0007EBB7 File Offset: 0x0007CDB7
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Movement.Stop();
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0007EBD0 File Offset: 0x0007CDD0
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (base.Active)
			{
				if (this.Player != null)
				{
					base.Npc.Avatar.LookController.OverrideLookTarget(this.Player.EyePosition, 1, true);
				}
				if (InstanceFinder.IsServer)
				{
					this.Countdown -= Time.deltaTime;
					if (this.Countdown <= 0f)
					{
						base.Disable_Networked(null);
					}
				}
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0007EC48 File Offset: 0x0007CE48
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetTarget_244313061));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetTargetLocal_3323014238));
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0007EC9A File Offset: 0x0007CE9A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0007ECB3 File Offset: 0x0007CEB3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0007ECC4 File Offset: 0x0007CEC4
		private void RpcWriter___Server_SetTarget_244313061(NetworkObject player, float countDown = 5f)
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
			writer.WriteSingle(countDown, AutoPackType.Unpacked);
			base.SendServerRpc(15U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0007ED80 File Offset: 0x0007CF80
		public void RpcLogic___SetTarget_244313061(NetworkObject player, float countDown = 5f)
		{
			Console.Log("SetTarget: " + ((player != null) ? player.ToString() : null), null);
			this.Countdown = countDown;
			this.Player = ((player != null) ? player.GetComponent<Player>() : null);
			this.SetTargetLocal(player);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0007EDD0 File Offset: 0x0007CFD0
		private void RpcReader___Server_SetTarget_244313061(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			float countDown = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetTarget_244313061(player, countDown);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0007EE24 File Offset: 0x0007D024
		private void RpcWriter___Observers_SetTargetLocal_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0007EEDA File Offset: 0x0007D0DA
		private void RpcLogic___SetTargetLocal_3323014238(NetworkObject player)
		{
			this.Player = ((player != null) ? player.GetComponent<Player>() : null);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0007EEF4 File Offset: 0x0007D0F4
		private void RpcReader___Observers_SetTargetLocal_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetTargetLocal_3323014238(player);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0007EF2F File Offset: 0x0007D12F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001849 RID: 6217
		private bool dll_Excuted;

		// Token: 0x0400184A RID: 6218
		private bool dll_Excuted;
	}
}
