using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x0200060D RID: 1549
	public class NetworkedInteractableToggleable : NetworkBehaviour
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000A81C5 File Offset: 0x000A63C5
		// (set) Token: 0x060028C3 RID: 10435 RVA: 0x000A81CD File Offset: 0x000A63CD
		public bool IsActivated { get; private set; }

		// Token: 0x060028C4 RID: 10436 RVA: 0x000A81D6 File Offset: 0x000A63D6
		public void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000A8210 File Offset: 0x000A6410
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsActivated)
			{
				this.SetState(connection, true);
			}
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000A822C File Offset: 0x000A642C
		public void Hovered()
		{
			if (Time.time - this.lastActivated < this.CoolDown)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage(this.IsActivated ? this.DeactivateMessage : this.ActivateMessage);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000A8287 File Offset: 0x000A6487
		public void Interacted()
		{
			this.SendToggle();
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000A828F File Offset: 0x000A648F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendToggle()
		{
			this.RpcWriter___Server_SendToggle_2166136261();
			this.RpcLogic___SendToggle_2166136261();
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000A82A0 File Offset: 0x000A64A0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetState(NetworkConnection conn, bool activated)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetState_214505783(conn, activated);
				this.RpcLogic___SetState_214505783(conn, activated);
			}
			else
			{
				this.RpcWriter___Target_SetState_214505783(conn, activated);
			}
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000A82E1 File Offset: 0x000A64E1
		public void PoliceDetected()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsActivated)
			{
				this.SendToggle();
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000A8338 File Offset: 0x000A6538
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendToggle_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetState_214505783));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetState_214505783));
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000A839B File Offset: 0x000A659B
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000A83AE File Offset: 0x000A65AE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000A83BC File Offset: 0x000A65BC
		private void RpcWriter___Server_SendToggle_2166136261()
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

		// Token: 0x060028D0 RID: 10448 RVA: 0x000A8456 File Offset: 0x000A6656
		public void RpcLogic___SendToggle_2166136261()
		{
			this.SetState(null, !this.IsActivated);
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000A8468 File Offset: 0x000A6668
		private void RpcReader___Server_SendToggle_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendToggle_2166136261();
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000A8498 File Offset: 0x000A6698
		private void RpcWriter___Observers_SetState_214505783(NetworkConnection conn, bool activated)
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
			writer.WriteBoolean(activated);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000A8550 File Offset: 0x000A6750
		public void RpcLogic___SetState_214505783(NetworkConnection conn, bool activated)
		{
			if (this.IsActivated == activated)
			{
				return;
			}
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.onToggle != null)
			{
				this.onToggle.Invoke();
			}
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000A85B4 File Offset: 0x000A67B4
		private void RpcReader___Observers_SetState_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool activated = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetState_214505783(null, activated);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000A85F0 File Offset: 0x000A67F0
		private void RpcWriter___Target_SetState_214505783(NetworkConnection conn, bool activated)
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
			writer.WriteBoolean(activated);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000A86A8 File Offset: 0x000A68A8
		private void RpcReader___Target_SetState_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool activated = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetState_214505783(base.LocalConnection, activated);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000A83AE File Offset: 0x000A65AE
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001DF9 RID: 7673
		public string ActivateMessage = "Activate";

		// Token: 0x04001DFA RID: 7674
		public string DeactivateMessage = "Deactivate";

		// Token: 0x04001DFB RID: 7675
		public float CoolDown;

		// Token: 0x04001DFC RID: 7676
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04001DFD RID: 7677
		public UnityEvent onToggle = new UnityEvent();

		// Token: 0x04001DFE RID: 7678
		public UnityEvent onActivate = new UnityEvent();

		// Token: 0x04001DFF RID: 7679
		public UnityEvent onDeactivate = new UnityEvent();

		// Token: 0x04001E00 RID: 7680
		private float lastActivated;

		// Token: 0x04001E01 RID: 7681
		private bool dll_Excuted;

		// Token: 0x04001E02 RID: 7682
		private bool dll_Excuted;
	}
}
