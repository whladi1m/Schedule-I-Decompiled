using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Law
{
	// Token: 0x020005BD RID: 1469
	public class CurfewManager : NetworkSingleton<CurfewManager>
	{
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x00092F22 File Offset: 0x00091122
		// (set) Token: 0x0600246D RID: 9325 RVA: 0x00092F2A File Offset: 0x0009112A
		public bool IsEnabled { get; protected set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x00092F33 File Offset: 0x00091133
		// (set) Token: 0x0600246F RID: 9327 RVA: 0x00092F3B File Offset: 0x0009113B
		public bool IsCurrentlyActive { get; protected set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x00092F44 File Offset: 0x00091144
		public bool IsCurrentlyActiveWithTolerance
		{
			get
			{
				return this.IsCurrentlyActive && NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(2115, 500);
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x00092F64 File Offset: 0x00091164
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.Disable();
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x00092F98 File Offset: 0x00091198
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsEnabled)
			{
				this.Enable(connection);
			}
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00092FB0 File Offset: 0x000911B0
		[ObserversRpc]
		[TargetRpc]
		public void Enable(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_328543758(conn);
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00092FDC File Offset: 0x000911DC
		[ObserversRpc]
		public void Disable()
		{
			this.RpcWriter___Observers_Disable_2166136261();
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x00092FF0 File Offset: 0x000911F0
		private void MinPass()
		{
			if (!this.IsEnabled)
			{
				this.IsCurrentlyActive = false;
				return;
			}
			string text = "CURFEW TONIGHT\n9PM - 5AM";
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime == 2030 && !this.warningPlayed)
			{
				this.warningPlayed = true;
				if (this.onCurfewWarning != null)
				{
					this.onCurfewWarning.Invoke();
				}
				if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.ElapsedDays == 0 && this.onCurfewHint != null)
				{
					this.onCurfewHint.Invoke();
				}
				this.CurfewWarningSound.Play();
			}
			VMSBoard[] vmsboards;
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(2100, 500))
			{
				if (!this.IsCurrentlyActive)
				{
					if (!NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.SleepInProgress && Singleton<LoadManager>.Instance.TimeSinceGameLoaded > 3f)
					{
						this.CurfewAlarmSound.Play();
					}
					this.IsCurrentlyActive = true;
				}
				text = "CURFEW ACTIVE\n UNTIL 5AM";
				vmsboards = this.VMSBoards;
				for (int i = 0; i < vmsboards.Length; i++)
				{
					vmsboards[i].SetText(text, new Color32(byte.MaxValue, 85, 60, byte.MaxValue));
				}
				return;
			}
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(2100, -60), 2100))
			{
				this.warningPlayed = false;
				this.IsCurrentlyActive = false;
				text = "CURFEW SOON\n" + (ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(2100) - ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime)).ToString() + " MINS";
				vmsboards = this.VMSBoards;
				for (int i = 0; i < vmsboards.Length; i++)
				{
					vmsboards[i].SetText(text);
				}
				return;
			}
			this.warningPlayed = false;
			this.IsCurrentlyActive = false;
			vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].SetText(text);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000931B0 File Offset: 0x000913B0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_Enable_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_2166136261));
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00093219 File Offset: 0x00091419
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00093232 File Offset: 0x00091432
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00093240 File Offset: 0x00091440
		private void RpcWriter___Observers_Enable_328543758(NetworkConnection conn)
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

		// Token: 0x0600247B RID: 9339 RVA: 0x000932EC File Offset: 0x000914EC
		public void RpcLogic___Enable_328543758(NetworkConnection conn)
		{
			this.IsEnabled = true;
			if (this.onCurfewEnabled != null)
			{
				this.onCurfewEnabled.Invoke();
			}
			VMSBoard[] vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x00093338 File Offset: 0x00091538
		private void RpcReader___Observers_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(null);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0009335C File Offset: 0x0009155C
		private void RpcWriter___Target_Enable_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x00093404 File Offset: 0x00091604
		private void RpcReader___Target_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(base.LocalConnection);
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0009342C File Offset: 0x0009162C
		private void RpcWriter___Observers_Disable_2166136261()
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

		// Token: 0x06002480 RID: 9344 RVA: 0x000934D8 File Offset: 0x000916D8
		public void RpcLogic___Disable_2166136261()
		{
			this.IsEnabled = false;
			if (this.onCurfewDisabled != null)
			{
				this.onCurfewDisabled.Invoke();
			}
			VMSBoard[] vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00093524 File Offset: 0x00091724
		private void RpcReader___Observers_Disable_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00093544 File Offset: 0x00091744
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001B1A RID: 6938
		public const int WARNING_TIME = 2030;

		// Token: 0x04001B1B RID: 6939
		public const int CURFEW_START_TIME = 2100;

		// Token: 0x04001B1C RID: 6940
		public const int CURFEW_END_TIME = 500;

		// Token: 0x04001B1F RID: 6943
		[Header("References")]
		public VMSBoard[] VMSBoards;

		// Token: 0x04001B20 RID: 6944
		public AudioSourceController CurfewWarningSound;

		// Token: 0x04001B21 RID: 6945
		public AudioSourceController CurfewAlarmSound;

		// Token: 0x04001B22 RID: 6946
		public UnityEvent onCurfewEnabled;

		// Token: 0x04001B23 RID: 6947
		public UnityEvent onCurfewDisabled;

		// Token: 0x04001B24 RID: 6948
		public UnityEvent onCurfewHint;

		// Token: 0x04001B25 RID: 6949
		public UnityEvent onCurfewWarning;

		// Token: 0x04001B26 RID: 6950
		private bool warningPlayed;

		// Token: 0x04001B27 RID: 6951
		private bool dll_Excuted;

		// Token: 0x04001B28 RID: 6952
		private bool dll_Excuted;
	}
}
