using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000629 RID: 1577
	public class ToggleableItem : GridItem
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x000ACABF File Offset: 0x000AACBF
		// (set) Token: 0x060029D8 RID: 10712 RVA: 0x000ACAC7 File Offset: 0x000AACC7
		public bool IsOn { get; private set; }

		// Token: 0x060029D9 RID: 10713 RVA: 0x000ACAD0 File Offset: 0x000AACD0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.ToggleableItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000ACAEF File Offset: 0x000AACEF
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsOn)
			{
				this.SetIsOn(connection, true);
			}
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000ACB08 File Offset: 0x000AAD08
		public void Toggle()
		{
			if (this.IsOn)
			{
				this.TurnOff(true);
				return;
			}
			this.TurnOn(true);
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000ACB24 File Offset: 0x000AAD24
		public void TurnOn(bool network = true)
		{
			if (this.IsOn)
			{
				return;
			}
			if (network)
			{
				this.SendIsOn(true);
				return;
			}
			this.IsOn = true;
			if (this.onTurnedOn != null)
			{
				this.onTurnedOn.Invoke();
			}
			if (this.onTurnOnOrOff != null)
			{
				this.onTurnOnOrOff.Invoke();
			}
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000ACB74 File Offset: 0x000AAD74
		public void TurnOff(bool network = true)
		{
			if (!this.IsOn)
			{
				return;
			}
			if (network)
			{
				this.SendIsOn(false);
				return;
			}
			this.IsOn = false;
			if (this.onTurnedOff != null)
			{
				this.onTurnedOff.Invoke();
			}
			if (this.onTurnOnOrOff != null)
			{
				this.onTurnOnOrOff.Invoke();
			}
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x000ACBC2 File Offset: 0x000AADC2
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendIsOn(bool on)
		{
			this.RpcWriter___Server_SendIsOn_1140765316(on);
			this.RpcLogic___SendIsOn_1140765316(on);
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x000ACBD8 File Offset: 0x000AADD8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetIsOn(NetworkConnection conn, bool on)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOn_214505783(conn, on);
				this.RpcLogic___SetIsOn_214505783(conn, on);
			}
			else
			{
				this.RpcWriter___Target_SetIsOn_214505783(conn, on);
			}
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000ACC0E File Offset: 0x000AAE0E
		public override string GetSaveString()
		{
			return new ToggleableItemData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.IsOn).GetJson(true);
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000ACC48 File Offset: 0x000AAE48
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendIsOn_1140765316));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOn_214505783));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOn_214505783));
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000ACCB1 File Offset: 0x000AAEB1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000ACCCA File Offset: 0x000AAECA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000ACCD8 File Offset: 0x000AAED8
		private void RpcWriter___Server_SendIsOn_1140765316(bool on)
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
			writer.WriteBoolean(on);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000ACD7F File Offset: 0x000AAF7F
		private void RpcLogic___SendIsOn_1140765316(bool on)
		{
			base.HasChanged = true;
			this.SetIsOn(null, on);
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000ACD90 File Offset: 0x000AAF90
		private void RpcReader___Server_SendIsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendIsOn_1140765316(on);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000ACDD0 File Offset: 0x000AAFD0
		private void RpcWriter___Observers_SetIsOn_214505783(NetworkConnection conn, bool on)
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
			writer.WriteBoolean(on);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000ACE86 File Offset: 0x000AB086
		private void RpcLogic___SetIsOn_214505783(NetworkConnection conn, bool on)
		{
			if (on)
			{
				this.TurnOn(false);
				return;
			}
			this.TurnOff(false);
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000ACE9C File Offset: 0x000AB09C
		private void RpcReader___Observers_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(null, on);
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000ACED8 File Offset: 0x000AB0D8
		private void RpcWriter___Target_SetIsOn_214505783(NetworkConnection conn, bool on)
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
			writer.WriteBoolean(on);
			base.SendTargetRpc(10U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000ACF90 File Offset: 0x000AB190
		private void RpcReader___Target_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(base.LocalConnection, on);
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000ACFC8 File Offset: 0x000AB1C8
		protected virtual void dll()
		{
			base.Awake();
			switch (this.StartupAction)
			{
			case ToggleableItem.EStartupAction.TurnOn:
				this.TurnOn(true);
				return;
			case ToggleableItem.EStartupAction.TurnOff:
				this.TurnOff(true);
				return;
			case ToggleableItem.EStartupAction.Toggle:
				this.Toggle();
				return;
			default:
				return;
			}
		}

		// Token: 0x04001EA6 RID: 7846
		[Header("Settings")]
		public ToggleableItem.EStartupAction StartupAction;

		// Token: 0x04001EA7 RID: 7847
		public UnityEvent onTurnedOn;

		// Token: 0x04001EA8 RID: 7848
		public UnityEvent onTurnedOff;

		// Token: 0x04001EA9 RID: 7849
		public UnityEvent onTurnOnOrOff;

		// Token: 0x04001EAA RID: 7850
		private bool dll_Excuted;

		// Token: 0x04001EAB RID: 7851
		private bool dll_Excuted;

		// Token: 0x0200062A RID: 1578
		public enum EStartupAction
		{
			// Token: 0x04001EAD RID: 7853
			None,
			// Token: 0x04001EAE RID: 7854
			TurnOn,
			// Token: 0x04001EAF RID: 7855
			TurnOff,
			// Token: 0x04001EB0 RID: 7856
			Toggle
		}
	}
}
