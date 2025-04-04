using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000513 RID: 1299
	public class GenericDialogueBehaviour : Behaviour
	{
		// Token: 0x06001F3A RID: 7994 RVA: 0x0007FE48 File Offset: 0x0007E048
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendTargetPlayer(NetworkObject player)
		{
			this.RpcWriter___Server_SendTargetPlayer_3323014238(player);
			this.RpcLogic___SendTargetPlayer_3323014238(player);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0007FE60 File Offset: 0x0007E060
		[ObserversRpc(RunLocally = true)]
		private void SetTargetPlayer(NetworkObject player)
		{
			this.RpcWriter___Observers_SetTargetPlayer_3323014238(player);
			this.RpcLogic___SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0007FE81 File Offset: 0x0007E081
		public override void Enable()
		{
			base.Enable();
			base.beh.Update();
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0007FE94 File Offset: 0x0007E094
		protected override void Begin()
		{
			base.Begin();
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0007FEBE File Offset: 0x0007E0BE
		protected override void Resume()
		{
			base.Resume();
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0007FEE8 File Offset: 0x0007E0E8
		protected override void End()
		{
			base.End();
			base.Npc.Movement.ResumeMovement();
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0007FF00 File Offset: 0x0007E100
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (!base.Npc.Movement.FaceDirectionInProgress && base.Npc.Avatar.Anim.TimeSinceSitEnd >= 0.5f)
			{
				float num;
				Player closestPlayer = Player.GetClosestPlayer(base.transform.position, out num, null);
				if (closestPlayer == null)
				{
					return;
				}
				Vector3 vector = closestPlayer.transform.position - base.Npc.transform.position;
				vector.y = 0f;
				if (Vector3.Angle(base.Npc.transform.forward, vector) > 10f)
				{
					base.Npc.Movement.FaceDirection(vector, 0.5f);
				}
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0007FFEC File Offset: 0x0007E1EC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SendTargetPlayer_3323014238));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetTargetPlayer_3323014238));
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0008003E File Offset: 0x0007E23E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x00080057 File Offset: 0x0007E257
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x00080068 File Offset: 0x0007E268
		private void RpcWriter___Server_SendTargetPlayer_3323014238(NetworkObject player)
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
			base.SendServerRpc(15U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0008010F File Offset: 0x0007E30F
		public void RpcLogic___SendTargetPlayer_3323014238(NetworkObject player)
		{
			this.SetTargetPlayer(player);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00080118 File Offset: 0x0007E318
		private void RpcReader___Server_SendTargetPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
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
			this.RpcLogic___SendTargetPlayer_3323014238(player);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00080158 File Offset: 0x0007E358
		private void RpcWriter___Observers_SetTargetPlayer_3323014238(NetworkObject player)
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

		// Token: 0x06001F4A RID: 8010 RVA: 0x00080210 File Offset: 0x0007E410
		private void RpcLogic___SetTargetPlayer_3323014238(NetworkObject player)
		{
			if (Singleton<DialogueCanvas>.Instance.isActive && this.targetPlayer != null && this.targetPlayer.Owner.IsLocalClient && player != null && !player.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			if (player != null)
			{
				this.targetPlayer = player.GetComponent<Player>();
				return;
			}
			this.targetPlayer = null;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00080288 File Offset: 0x0007E488
		private void RpcReader___Observers_SetTargetPlayer_3323014238(PooledReader PooledReader0, Channel channel)
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
			this.RpcLogic___SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000802C3 File Offset: 0x0007E4C3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001868 RID: 6248
		private Player targetPlayer;

		// Token: 0x04001869 RID: 6249
		private bool dll_Excuted;

		// Token: 0x0400186A RID: 6250
		private bool dll_Excuted;
	}
}
