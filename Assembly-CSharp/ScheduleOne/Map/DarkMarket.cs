using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.GameTime;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE5 RID: 3045
	public class DarkMarket : NetworkSingleton<DarkMarket>
	{
		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06005554 RID: 21844 RVA: 0x001671C2 File Offset: 0x001653C2
		// (set) Token: 0x06005555 RID: 21845 RVA: 0x001671CA File Offset: 0x001653CA
		public bool IsOpen { get; protected set; } = true;

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06005556 RID: 21846 RVA: 0x001671D3 File Offset: 0x001653D3
		// (set) Token: 0x06005557 RID: 21847 RVA: 0x001671DB File Offset: 0x001653DB
		public bool Unlocked { get; protected set; }

		// Token: 0x06005558 RID: 21848 RVA: 0x001671E4 File Offset: 0x001653E4
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoad));
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x00167207 File Offset: 0x00165407
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.Unlocked)
			{
				this.SetUnlocked(connection);
			}
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x0016721F File Offset: 0x0016541F
		private void Update()
		{
			this.IsOpen = this.ShouldBeOpen();
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x00167230 File Offset: 0x00165430
		private bool ShouldBeOpen()
		{
			if (!NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(this.AccessZone.OpenTime, this.AccessZone.CloseTime))
			{
				return false;
			}
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x00167290 File Offset: 0x00165490
		private void OnLoad()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.OnLoad));
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("WarehouseUnlocked"))
			{
				this.SendUnlocked();
				return;
			}
			this.MainDoor.SetKnockingEnabled(true);
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x001672DC File Offset: 0x001654DC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendUnlocked()
		{
			this.RpcWriter___Server_SendUnlocked_2166136261();
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x001672EC File Offset: 0x001654EC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetUnlocked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetUnlocked_328543758(conn);
				this.RpcLogic___SetUnlocked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_SetUnlocked_328543758(conn);
			}
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x00167330 File Offset: 0x00165530
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendUnlocked_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetUnlocked_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetUnlocked_328543758));
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x00167399 File Offset: 0x00165599
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x001673B2 File Offset: 0x001655B2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x001673C0 File Offset: 0x001655C0
		private void RpcWriter___Server_SendUnlocked_2166136261()
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

		// Token: 0x06005564 RID: 21860 RVA: 0x0016745A File Offset: 0x0016565A
		public void RpcLogic___SendUnlocked_2166136261()
		{
			this.SetUnlocked(null);
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x00167464 File Offset: 0x00165664
		private void RpcReader___Server_SendUnlocked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x00167494 File Offset: 0x00165694
		private void RpcWriter___Observers_SetUnlocked_328543758(NetworkConnection conn)
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

		// Token: 0x06005567 RID: 21863 RVA: 0x00167540 File Offset: 0x00165740
		private void RpcLogic___SetUnlocked_328543758(NetworkConnection conn)
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("WarehouseUnlocked", true.ToString(), true);
			this.MainDoor.SetKnockingEnabled(false);
			this.MainDoor.Igor.gameObject.SetActive(false);
			this.Unlocked = true;
			this.Oscar.EnableDeliveries();
			DoorController[] doors = this.AccessZone.Doors;
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].noAccessErrorMessage = "Only open after 6PM";
			}
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x001675C4 File Offset: 0x001657C4
		private void RpcReader___Observers_SetUnlocked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_328543758(null);
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x001675F0 File Offset: 0x001657F0
		private void RpcWriter___Target_SetUnlocked_328543758(NetworkConnection conn)
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

		// Token: 0x0600556A RID: 21866 RVA: 0x00167698 File Offset: 0x00165898
		private void RpcReader___Target_SetUnlocked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_328543758(base.LocalConnection);
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x001676BE File Offset: 0x001658BE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003F5C RID: 16220
		public DarkMarketAccessZone AccessZone;

		// Token: 0x04003F5D RID: 16221
		public DarkMarketMainDoor MainDoor;

		// Token: 0x04003F5E RID: 16222
		public Oscar Oscar;

		// Token: 0x04003F5F RID: 16223
		public FullRank UnlockRank;

		// Token: 0x04003F60 RID: 16224
		private bool dll_Excuted;

		// Token: 0x04003F61 RID: 16225
		private bool dll_Excuted;
	}
}
