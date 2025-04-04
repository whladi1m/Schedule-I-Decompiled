using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Actions
{
	// Token: 0x0200048B RID: 1163
	public class NPCActions : NetworkBehaviour
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x0006FF81 File Offset: 0x0006E181
		protected NPCBehaviour behaviour
		{
			get
			{
				return this.npc.behaviour;
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0006FF8E File Offset: 0x0006E18E
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Actions.NPCActions_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0006FFA2 File Offset: 0x0006E1A2
		public void Cower()
		{
			this.behaviour.GetBehaviour("Cowering").Enable_Networked(null);
			base.StartCoroutine(this.<Cower>g__Wait|4_0());
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0006FFC8 File Offset: 0x0006E1C8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CallPolice_Networked(Player player)
		{
			this.RpcWriter___Server_CallPolice_Networked_1385486242(player);
			this.RpcLogic___CallPolice_Networked_1385486242(player);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0006FFE9 File Offset: 0x0006E1E9
		public void SetCallPoliceBehaviourCrime(Crime crime)
		{
			this.npc.behaviour.CallPoliceBehaviour.ReportedCrime = crime;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000045B1 File Offset: 0x000027B1
		public void FacePlayer(Player player)
		{
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00070001 File Offset: 0x0006E201
		[CompilerGenerated]
		private IEnumerator <Cower>g__Wait|4_0()
		{
			yield return new WaitForSeconds(10f);
			this.behaviour.GetBehaviour("Cowering").Disable_Networked(null);
			yield break;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00070010 File Offset: 0x0006E210
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CallPolice_Networked_1385486242));
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0007003A File Offset: 0x0006E23A
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0007004D File Offset: 0x0006E24D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0007005C File Offset: 0x0006E25C
		private void RpcWriter___Server_CallPolice_Networked_1385486242(Player player)
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
			writer.Write___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generated(player);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00070104 File Offset: 0x0006E304
		public void RpcLogic___CallPolice_Networked_1385486242(Player player)
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			if (!this.npc.IsConscious)
			{
				return;
			}
			Console.Log(this.npc.fullName + " is calling the police on " + player.PlayerName, null);
			if (player.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				Console.LogWarning("Player is already being pursued, ignoring call police request.", null);
				return;
			}
			this.npc.behaviour.CallPoliceBehaviour.Target = player;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.npc.behaviour.CallPoliceBehaviour.Enable_Networked(null);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0007019C File Offset: 0x0006E39C
		private void RpcReader___Server_CallPolice_Networked_1385486242(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Player player = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CallPolice_Networked_1385486242(player);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000701DA File Offset: 0x0006E3DA
		protected virtual void dll()
		{
			this.npc = base.GetComponentInParent<NPC>();
		}

		// Token: 0x0400164A RID: 5706
		private NPC npc;

		// Token: 0x0400164B RID: 5707
		private bool dll_Excuted;

		// Token: 0x0400164C RID: 5708
		private bool dll_Excuted;
	}
}
