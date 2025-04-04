using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004D9 RID: 1241
	public class Behaviour : NetworkBehaviour
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x00072C26 File Offset: 0x00070E26
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x00072C2E File Offset: 0x00070E2E
		public bool Enabled { get; protected set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x00072C37 File Offset: 0x00070E37
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x00072C3F File Offset: 0x00070E3F
		public bool Started { get; private set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x00072C48 File Offset: 0x00070E48
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x00072C50 File Offset: 0x00070E50
		public bool Active { get; private set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x00072C59 File Offset: 0x00070E59
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x00072C61 File Offset: 0x00070E61
		public NPCBehaviour beh { get; private set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x00072C6A File Offset: 0x00070E6A
		public NPC Npc
		{
			get
			{
				return this.beh.Npc;
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00072C77 File Offset: 0x00070E77
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.Behaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00072C8B File Offset: 0x00070E8B
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			if (this.Enabled)
			{
				this.Enable_Networked(connection);
				return;
			}
			this.Disable_Networked(connection);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00072CB4 File Offset: 0x00070EB4
		protected override void OnValidate()
		{
			base.OnValidate();
			this.UpdateGameObjectName();
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00072CC4 File Offset: 0x00070EC4
		public virtual void Enable()
		{
			if (this.Npc.behaviour.DEBUG_MODE)
			{
				Debug.Log(this.Name + " enabled");
			}
			this.Enabled = true;
			if (this.onEnable != null)
			{
				this.onEnable.Invoke();
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00072D12 File Offset: 0x00070F12
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendEnable()
		{
			this.RpcWriter___Server_SendEnable_2166136261();
			this.RpcLogic___SendEnable_2166136261();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00072D20 File Offset: 0x00070F20
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Enable_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_Networked_328543758(conn);
				this.RpcLogic___Enable_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00072D4C File Offset: 0x00070F4C
		public virtual void Disable()
		{
			if (this.Npc.behaviour.DEBUG_MODE)
			{
				Debug.Log(this.Name + " disabled");
			}
			this.Enabled = false;
			this.Started = false;
			if (this.Active)
			{
				this.End();
			}
			if (this.onDisable != null)
			{
				this.onDisable.Invoke();
			}
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00072DAF File Offset: 0x00070FAF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendDisable()
		{
			this.RpcWriter___Server_SendDisable_2166136261();
			this.RpcLogic___SendDisable_2166136261();
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00072DBD File Offset: 0x00070FBD
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Disable_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Disable_Networked_328543758(conn);
				this.RpcLogic___Disable_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Disable_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x00072DE8 File Offset: 0x00070FE8
		private void UpdateGameObjectName()
		{
			if (base.gameObject == null)
			{
				return;
			}
			base.gameObject.name = this.Name + (this.Active ? " (Active)" : " (Inactive)");
			if (!this.Active)
			{
				base.gameObject.name = base.gameObject.name + (this.Enabled ? " (Enabled)" : " (Disabled)");
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x00072E65 File Offset: 0x00071065
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Begin_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Begin_Networked_328543758(conn);
				this.RpcLogic___Begin_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Begin_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00072E90 File Offset: 0x00071090
		protected virtual void Begin()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") started", null);
			}
			this.Started = true;
			this.Active = true;
			this.beh.activeBehaviour = this;
			this.UpdateGameObjectName();
			if (this.onBegin != null)
			{
				this.onBegin.Invoke();
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x00072EF8 File Offset: 0x000710F8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendEnd()
		{
			this.RpcWriter___Server_SendEnd_2166136261();
			this.RpcLogic___SendEnd_2166136261();
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00072F06 File Offset: 0x00071106
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void End_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_End_Networked_328543758(conn);
				this.RpcLogic___End_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_End_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00072F30 File Offset: 0x00071130
		protected virtual void End()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") ended", null);
			}
			this.Active = false;
			this.beh.activeBehaviour = null;
			this.UpdateGameObjectName();
			if (this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x00072F91 File Offset: 0x00071191
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Pause_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Pause_Networked_328543758(conn);
				this.RpcLogic___Pause_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Pause_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x00072FBB File Offset: 0x000711BB
		protected virtual void Pause()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") paused", null);
			}
			this.Active = false;
			this.UpdateGameObjectName();
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x00072FF2 File Offset: 0x000711F2
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Resume_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Resume_Networked_328543758(conn);
				this.RpcLogic___Resume_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Resume_Networked_328543758(conn);
			}
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0007301C File Offset: 0x0007121C
		protected virtual void Resume()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") resumed", null);
			}
			this.Active = true;
			this.beh.activeBehaviour = this;
			this.UpdateGameObjectName();
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void BehaviourUpdate()
		{
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void BehaviourLateUpdate()
		{
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveMinPass()
		{
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0007306A File Offset: 0x0007126A
		protected void SetPriority(int p)
		{
			this.Priority = p;
			this.beh.SortBehaviourStack();
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0007307E File Offset: 0x0007127E
		protected void SetDestination(ITransitEntity transitEntity, bool teleportIfFail = true)
		{
			this.SetDestination(NavMeshUtility.GetAccessPoint(transitEntity, this.Npc).position, teleportIfFail);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00073098 File Offset: 0x00071298
		protected void SetDestination(Vector3 position, bool teleportIfFail = true)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (teleportIfFail && this.consecutivePathingFailures >= 5 && !this.Npc.Movement.CanGetTo(position, 1f))
			{
				Console.LogWarning(this.Npc.fullName + " too many pathing failures. Warping to " + position.ToString(), null);
				this.Npc.Movement.Warp(position);
				this.WalkCallback(NPCMovement.WalkResult.Success);
			}
			this.Npc.Movement.SetDestination(position, new Action<NPCMovement.WalkResult>(this.WalkCallback), 1f, 0.1f);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0007313C File Offset: 0x0007133C
		protected virtual void WalkCallback(NPCMovement.WalkResult result)
		{
			if (!this.Active)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Walk callback result: " + result.ToString(), null);
			}
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000731C0 File Offset: 0x000713C0
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendEnable_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_Networked_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_Enable_Networked_328543758));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendDisable_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_Networked_328543758));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_Disable_Networked_328543758));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_Begin_Networked_328543758));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_Begin_Networked_328543758));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendEnd_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_End_Networked_328543758));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_End_Networked_328543758));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_Pause_Networked_328543758));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_Pause_Networked_328543758));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_Resume_Networked_328543758));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_Resume_Networked_328543758));
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x00073337 File Offset: 0x00071537
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0007334A File Offset: 0x0007154A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x00073358 File Offset: 0x00071558
		private void RpcWriter___Server_SendEnable_2166136261()
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
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000733F2 File Offset: 0x000715F2
		public void RpcLogic___SendEnable_2166136261()
		{
			this.Enable_Networked(null);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000733FC File Offset: 0x000715FC
		private void RpcReader___Server_SendEnable_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEnable_2166136261();
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0007342C File Offset: 0x0007162C
		private void RpcWriter___Observers_Enable_Networked_328543758(NetworkConnection conn)
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

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000734D5 File Offset: 0x000716D5
		public void RpcLogic___Enable_Networked_328543758(NetworkConnection conn)
		{
			this.Enable();
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000734E0 File Offset: 0x000716E0
		private void RpcReader___Observers_Enable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Enable_Networked_328543758(null);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0007350C File Offset: 0x0007170C
		private void RpcWriter___Target_Enable_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000735B4 File Offset: 0x000717B4
		private void RpcReader___Target_Enable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000735DC File Offset: 0x000717DC
		private void RpcWriter___Server_SendDisable_2166136261()
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
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x00073676 File Offset: 0x00071876
		public void RpcLogic___SendDisable_2166136261()
		{
			this.Disable_Networked(null);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00073680 File Offset: 0x00071880
		private void RpcReader___Server_SendDisable_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDisable_2166136261();
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000736B0 File Offset: 0x000718B0
		private void RpcWriter___Observers_Disable_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x00073759 File Offset: 0x00071959
		public void RpcLogic___Disable_Networked_328543758(NetworkConnection conn)
		{
			this.Disable();
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x00073764 File Offset: 0x00071964
		private void RpcReader___Observers_Disable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Disable_Networked_328543758(null);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x00073790 File Offset: 0x00071990
		private void RpcWriter___Target_Disable_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(5U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00073838 File Offset: 0x00071A38
		private void RpcReader___Target_Disable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Disable_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00073860 File Offset: 0x00071A60
		private void RpcWriter___Observers_Begin_Networked_328543758(NetworkConnection conn)
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

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00073909 File Offset: 0x00071B09
		public void RpcLogic___Begin_Networked_328543758(NetworkConnection conn)
		{
			this.Begin();
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x00073914 File Offset: 0x00071B14
		private void RpcReader___Observers_Begin_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Begin_Networked_328543758(null);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00073940 File Offset: 0x00071B40
		private void RpcWriter___Target_Begin_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(7U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000739E8 File Offset: 0x00071BE8
		private void RpcReader___Target_Begin_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Begin_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00073A10 File Offset: 0x00071C10
		private void RpcWriter___Server_SendEnd_2166136261()
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
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00073AAA File Offset: 0x00071CAA
		public void RpcLogic___SendEnd_2166136261()
		{
			this.End_Networked(null);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x00073AB4 File Offset: 0x00071CB4
		private void RpcReader___Server_SendEnd_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEnd_2166136261();
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00073AE4 File Offset: 0x00071CE4
		private void RpcWriter___Observers_End_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x00073B8D File Offset: 0x00071D8D
		public void RpcLogic___End_Networked_328543758(NetworkConnection conn)
		{
			this.End();
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x00073B98 File Offset: 0x00071D98
		private void RpcReader___Observers_End_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___End_Networked_328543758(null);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x00073BC4 File Offset: 0x00071DC4
		private void RpcWriter___Target_End_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(10U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x00073C6C File Offset: 0x00071E6C
		private void RpcReader___Target_End_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___End_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x00073C94 File Offset: 0x00071E94
		private void RpcWriter___Observers_Pause_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00073D3D File Offset: 0x00071F3D
		public void RpcLogic___Pause_Networked_328543758(NetworkConnection conn)
		{
			this.Pause();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x00073D48 File Offset: 0x00071F48
		private void RpcReader___Observers_Pause_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Pause_Networked_328543758(null);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00073D74 File Offset: 0x00071F74
		private void RpcWriter___Target_Pause_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(12U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00073E1C File Offset: 0x0007201C
		private void RpcReader___Target_Pause_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Pause_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00073E44 File Offset: 0x00072044
		private void RpcWriter___Observers_Resume_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00073EED File Offset: 0x000720ED
		public void RpcLogic___Resume_Networked_328543758(NetworkConnection conn)
		{
			this.Resume();
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00073EF8 File Offset: 0x000720F8
		private void RpcReader___Observers_Resume_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Resume_Networked_328543758(null);
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x00073F24 File Offset: 0x00072124
		private void RpcWriter___Target_Resume_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(14U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00073FCC File Offset: 0x000721CC
		private void RpcReader___Target_Resume_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Resume_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00073FF2 File Offset: 0x000721F2
		protected virtual void dll()
		{
			this.beh = base.GetComponentInParent<NPCBehaviour>();
			this.Enabled = this.EnabledOnAwake;
		}

		// Token: 0x04001718 RID: 5912
		public const int MAX_CONSECUTIVE_PATHING_FAILURES = 5;

		// Token: 0x04001719 RID: 5913
		public bool EnabledOnAwake;

		// Token: 0x0400171B RID: 5915
		[Header("Settings")]
		public string Name = "Behaviour";

		// Token: 0x0400171C RID: 5916
		[Tooltip("Behaviour priority; higher = takes priority over lower number behaviour")]
		public int Priority;

		// Token: 0x04001720 RID: 5920
		public UnityEvent onEnable = new UnityEvent();

		// Token: 0x04001721 RID: 5921
		public UnityEvent onDisable = new UnityEvent();

		// Token: 0x04001722 RID: 5922
		public UnityEvent onBegin;

		// Token: 0x04001723 RID: 5923
		public UnityEvent onEnd;

		// Token: 0x04001724 RID: 5924
		protected int consecutivePathingFailures;

		// Token: 0x04001725 RID: 5925
		private bool dll_Excuted;

		// Token: 0x04001726 RID: 5926
		private bool dll_Excuted;
	}
}
