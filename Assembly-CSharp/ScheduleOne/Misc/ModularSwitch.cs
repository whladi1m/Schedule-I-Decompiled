using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BDB RID: 3035
	public class ModularSwitch : NetworkBehaviour
	{
		// Token: 0x0600551C RID: 21788 RVA: 0x0016610C File Offset: 0x0016430C
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Misc.ModularSwitch_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x0016612B File Offset: 0x0016432B
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetIsOn(connection, this.isOn);
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x00166144 File Offset: 0x00164344
		protected virtual void LateUpdate()
		{
			if (this.isOn)
			{
				this.button.localEulerAngles = new Vector3(-7f, 0f, 0f);
				return;
			}
			this.button.localEulerAngles = new Vector3(7f, 0f, 0f);
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x00166198 File Offset: 0x00164398
		public void Hovered()
		{
			if (this.isOn)
			{
				this.intObj.SetMessage("Switch off");
				return;
			}
			this.intObj.SetMessage("Switch on");
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x001661C3 File Offset: 0x001643C3
		public void Interacted()
		{
			if (this.isOn)
			{
				this.SendIsOn(false);
				return;
			}
			this.SendIsOn(true);
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x001661DC File Offset: 0x001643DC
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendIsOn(bool isOn)
		{
			this.RpcWriter___Server_SendIsOn_1140765316(isOn);
			this.RpcLogic___SendIsOn_1140765316(isOn);
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x001661F2 File Offset: 0x001643F2
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetIsOn(NetworkConnection conn, bool isOn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOn_214505783(conn, isOn);
				this.RpcLogic___SetIsOn_214505783(conn, isOn);
			}
			else
			{
				this.RpcWriter___Target_SetIsOn_214505783(conn, isOn);
			}
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x00166228 File Offset: 0x00164428
		public void SwitchOn()
		{
			if (this.isOn)
			{
				return;
			}
			this.isOn = true;
			if (this.switchedOn != null)
			{
				this.switchedOn.Invoke();
			}
			if (this.onToggled != null)
			{
				this.onToggled(this.isOn);
			}
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				this.SwitchesToSyncWith[i].SwitchOn();
			}
			this.OnAudio.Play();
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x001662A4 File Offset: 0x001644A4
		public void SwitchOff()
		{
			if (!this.isOn)
			{
				return;
			}
			this.isOn = false;
			if (this.switchedOff != null)
			{
				this.switchedOff.Invoke();
			}
			if (this.onToggled != null)
			{
				this.onToggled(this.isOn);
			}
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				this.SwitchesToSyncWith[i].SwitchOff();
			}
			this.OffAudio.Play();
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x00166334 File Offset: 0x00164534
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendIsOn_1140765316));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOn_214505783));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOn_214505783));
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x00166397 File Offset: 0x00164597
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x001663AA File Offset: 0x001645AA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x001663B8 File Offset: 0x001645B8
		private void RpcWriter___Server_SendIsOn_1140765316(bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x0016645F File Offset: 0x0016465F
		private void RpcLogic___SendIsOn_1140765316(bool isOn)
		{
			this.SetIsOn(null, isOn);
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x0016646C File Offset: 0x0016466C
		private void RpcReader___Server_SendIsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendIsOn_1140765316(flag);
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001664AC File Offset: 0x001646AC
		private void RpcWriter___Observers_SetIsOn_214505783(NetworkConnection conn, bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x00166562 File Offset: 0x00164762
		private void RpcLogic___SetIsOn_214505783(NetworkConnection conn, bool isOn)
		{
			if (isOn)
			{
				this.SwitchOn();
				return;
			}
			this.SwitchOff();
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x00166574 File Offset: 0x00164774
		private void RpcReader___Observers_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(null, flag);
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x001665B0 File Offset: 0x001647B0
		private void RpcWriter___Target_SetIsOn_214505783(NetworkConnection conn, bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x00166668 File Offset: 0x00164868
		private void RpcReader___Target_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(base.LocalConnection, flag);
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x001666A0 File Offset: 0x001648A0
		protected virtual void dll()
		{
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				if (!this.SwitchesToSyncWith[i].SwitchesToSyncWith.Contains(this))
				{
					this.SwitchesToSyncWith[i].SwitchesToSyncWith.Add(this);
				}
			}
		}

		// Token: 0x04003F1F RID: 16159
		public bool isOn;

		// Token: 0x04003F20 RID: 16160
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003F21 RID: 16161
		[SerializeField]
		protected Transform button;

		// Token: 0x04003F22 RID: 16162
		public AudioSourceController OnAudio;

		// Token: 0x04003F23 RID: 16163
		public AudioSourceController OffAudio;

		// Token: 0x04003F24 RID: 16164
		[Header("Settings")]
		[SerializeField]
		protected List<ModularSwitch> SwitchesToSyncWith = new List<ModularSwitch>();

		// Token: 0x04003F25 RID: 16165
		public ModularSwitch.ButtonChange onToggled;

		// Token: 0x04003F26 RID: 16166
		public UnityEvent switchedOn;

		// Token: 0x04003F27 RID: 16167
		public UnityEvent switchedOff;

		// Token: 0x04003F28 RID: 16168
		private bool dll_Excuted;

		// Token: 0x04003F29 RID: 16169
		private bool dll_Excuted;

		// Token: 0x02000BDC RID: 3036
		// (Invoke) Token: 0x06005533 RID: 21811
		public delegate void ButtonChange(bool isOn);
	}
}
