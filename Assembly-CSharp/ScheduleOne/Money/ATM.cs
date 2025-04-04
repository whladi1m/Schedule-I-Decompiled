using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.ATM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Money
{
	// Token: 0x02000B5E RID: 2910
	public class ATM : NetworkBehaviour, IGUIDRegisterable, IGenericSaveable
	{
		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06004D45 RID: 19781 RVA: 0x001465CA File Offset: 0x001447CA
		// (set) Token: 0x06004D46 RID: 19782 RVA: 0x001465D2 File Offset: 0x001447D2
		public bool IsBroken { get; protected set; }

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06004D47 RID: 19783 RVA: 0x001465DB File Offset: 0x001447DB
		// (set) Token: 0x06004D48 RID: 19784 RVA: 0x001465E3 File Offset: 0x001447E3
		public int DaysUntilRepair { get; protected set; }

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x001465EC File Offset: 0x001447EC
		// (set) Token: 0x06004D4A RID: 19786 RVA: 0x001465F4 File Offset: 0x001447F4
		public bool isInUse { get; protected set; }

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06004D4B RID: 19787 RVA: 0x001465FD File Offset: 0x001447FD
		// (set) Token: 0x06004D4C RID: 19788 RVA: 0x00146605 File Offset: 0x00144805
		public Guid GUID { get; protected set; }

		// Token: 0x06004D4D RID: 19789 RVA: 0x00146610 File Offset: 0x00144810
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x00146636 File Offset: 0x00144836
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Money.ATM_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0014664C File Offset: 0x0014484C
		protected virtual void Start()
		{
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.DayPass));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onWeekPass = (Action)Delegate.Combine(instance.onWeekPass, new Action(this.WeekPass));
			((IGenericSaveable)this).InitializeSaveable();
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x001466BB File Offset: 0x001448BB
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsBroken)
			{
				this.Break(connection);
			}
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x001466D3 File Offset: 0x001448D3
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x001466E4 File Offset: 0x001448E4
		public void DayPass()
		{
			if (InstanceFinder.IsServer && this.IsBroken)
			{
				int daysUntilRepair = this.DaysUntilRepair;
				this.DaysUntilRepair = daysUntilRepair - 1;
				if (this.DaysUntilRepair <= 0)
				{
					this.Repair();
				}
			}
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0014671F File Offset: 0x0014491F
		public void WeekPass()
		{
			ATM.WeeklyDepositSum = 0f;
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0014672B File Offset: 0x0014492B
		public void Hovered()
		{
			if (this.isInUse || this.IsBroken)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.intObj.SetMessage("Use ATM");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x00146766 File Offset: 0x00144966
		public void Interacted()
		{
			if (this.isInUse || this.IsBroken)
			{
				return;
			}
			this.Enter();
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x00146780 File Offset: 0x00144980
		public void Enter()
		{
			this.isInUse = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, ATM.viewLerpTime);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.camPos.position, this.camPos.rotation, ATM.viewLerpTime, false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.interfaceATM.SetIsOpen(true);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00146800 File Offset: 0x00144A00
		public void Exit()
		{
			this.isInUse = false;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(ATM.viewLerpTime);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(ATM.viewLerpTime, true, true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00146854 File Offset: 0x00144A54
		private void Impacted(Impact impact)
		{
			if (this.IsBroken)
			{
				return;
			}
			if (impact.ImpactForce >= 165f)
			{
				this.SendBreak();
				if (impact.ImpactSource == Player.Local.NetworkObject)
				{
					Player.Local.VisualState.ApplyState("vandalism", PlayerVisualState.EVisualState.Vandalizing, 0f);
					Player.Local.VisualState.RemoveState("vandalism", 2f);
				}
				base.StartCoroutine(this.<Impacted>g__BreakRoutine|45_0());
			}
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x001468D4 File Offset: 0x00144AD4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBreak()
		{
			this.RpcWriter___Server_SendBreak_2166136261();
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x001468E2 File Offset: 0x00144AE2
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void Break(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Break_328543758(conn);
				this.RpcLogic___Break_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Break_328543758(conn);
			}
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0014690C File Offset: 0x00144B0C
		[ObserversRpc]
		private void Repair()
		{
			this.RpcWriter___Observers_Repair_2166136261();
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x00146914 File Offset: 0x00144B14
		[ServerRpc(RequireOwnership = false)]
		private void DropCash()
		{
			this.RpcWriter___Server_DropCash_2166136261();
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x00146927 File Offset: 0x00144B27
		public void Load(GenericSaveData data)
		{
			this.IsBroken = data.GetBool("broken", false);
			this.DaysUntilRepair = data.GetInt("daysUntilRepair", 0);
			if (this.IsBroken)
			{
				this.Break(null);
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0014695C File Offset: 0x00144B5C
		public GenericSaveData GetSaveData()
		{
			GenericSaveData genericSaveData = new GenericSaveData(this.GUID.ToString());
			genericSaveData.Add("broken", this.IsBroken);
			genericSaveData.Add("daysUntilRepair", this.DaysUntilRepair);
			return genericSaveData;
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x001469CD File Offset: 0x00144BCD
		[CompilerGenerated]
		private IEnumerator <Impacted>g__BreakRoutine|45_0()
		{
			int cashDrop = UnityEngine.Random.Range(2, 9);
			int num;
			for (int i = 0; i < cashDrop; i = num + 1)
			{
				this.DropCash();
				yield return new WaitForSeconds(0.2f);
				num = i;
			}
			yield break;
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x001469DC File Offset: 0x00144BDC
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBreak_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Break_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_Break_328543758));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_Repair_2166136261));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_DropCash_2166136261));
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x00146A6D File Offset: 0x00144C6D
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x00146A80 File Offset: 0x00144C80
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x00146A90 File Offset: 0x00144C90
		private void RpcWriter___Server_SendBreak_2166136261()
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

		// Token: 0x06004D66 RID: 19814 RVA: 0x00146B2A File Offset: 0x00144D2A
		private void RpcLogic___SendBreak_2166136261()
		{
			this.DaysUntilRepair = 2;
			this.Break(null);
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x00146B3C File Offset: 0x00144D3C
		private void RpcReader___Server_SendBreak_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x00146B6C File Offset: 0x00144D6C
		private void RpcWriter___Observers_Break_328543758(NetworkConnection conn)
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

		// Token: 0x06004D69 RID: 19817 RVA: 0x00146C15 File Offset: 0x00144E15
		private void RpcLogic___Break_328543758(NetworkConnection conn)
		{
			if (this.IsBroken)
			{
				return;
			}
			this.IsBroken = true;
			UnityEvent unityEvent = this.onBreak;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x00146C38 File Offset: 0x00144E38
		private void RpcReader___Observers_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Break_328543758(null);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x00146C64 File Offset: 0x00144E64
		private void RpcWriter___Target_Break_328543758(NetworkConnection conn)
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

		// Token: 0x06004D6C RID: 19820 RVA: 0x00146D0C File Offset: 0x00144F0C
		private void RpcReader___Target_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Break_328543758(base.LocalConnection);
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x00146D34 File Offset: 0x00144F34
		private void RpcWriter___Observers_Repair_2166136261()
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
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x00146DDD File Offset: 0x00144FDD
		private void RpcLogic___Repair_2166136261()
		{
			if (!this.IsBroken)
			{
				return;
			}
			this.IsBroken = false;
			UnityEvent unityEvent = this.onRepair;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x00146E00 File Offset: 0x00145000
		private void RpcReader___Observers_Repair_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Repair_2166136261();
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x00146E20 File Offset: 0x00145020
		private void RpcWriter___Server_DropCash_2166136261()
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
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00146EBC File Offset: 0x001450BC
		private void RpcLogic___DropCash_2166136261()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CashPrefab.gameObject, this.CashSpawnPoint.position, this.CashSpawnPoint.rotation);
			gameObject.GetComponent<Rigidbody>().AddForce(this.CashSpawnPoint.forward * UnityEngine.Random.Range(1.5f, 2.5f), ForceMode.VelocityChange);
			gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, ForceMode.VelocityChange);
			base.Spawn(gameObject.gameObject, null, default(Scene));
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x00146F4C File Offset: 0x0014514C
		private void RpcReader___Server_DropCash_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropCash_2166136261();
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x00146F6C File Offset: 0x0014516C
		private void dll()
		{
			PhysicsDamageable damageable = this.Damageable;
			damageable.onImpacted = (Action<Impact>)Delegate.Combine(damageable.onImpacted, new Action<Impact>(this.Impacted));
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x04003A91 RID: 14993
		public const bool DepositLimitEnabled = true;

		// Token: 0x04003A92 RID: 14994
		public const float WEEKLY_DEPOSIT_LIMIT = 10000f;

		// Token: 0x04003A93 RID: 14995
		public const float IMPACT_THRESHOLD_BREAK = 165f;

		// Token: 0x04003A94 RID: 14996
		public const int REPAIR_TIME_DAYS = 2;

		// Token: 0x04003A95 RID: 14997
		public const int MIN_CASH_DROP = 2;

		// Token: 0x04003A96 RID: 14998
		public const int MAX_CASH_DROP = 8;

		// Token: 0x04003A97 RID: 14999
		public static float WeeklyDepositSum = 0f;

		// Token: 0x04003A9A RID: 15002
		public CashPickup CashPrefab;

		// Token: 0x04003A9B RID: 15003
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003A9C RID: 15004
		[SerializeField]
		protected Transform camPos;

		// Token: 0x04003A9D RID: 15005
		[SerializeField]
		protected ATMInterface interfaceATM;

		// Token: 0x04003A9E RID: 15006
		public Transform AccessPoint;

		// Token: 0x04003A9F RID: 15007
		public Transform CashSpawnPoint;

		// Token: 0x04003AA0 RID: 15008
		public PhysicsDamageable Damageable;

		// Token: 0x04003AA1 RID: 15009
		[Header("Settings")]
		public static float viewLerpTime = 0.15f;

		// Token: 0x04003AA4 RID: 15012
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003AA5 RID: 15013
		public UnityEvent onBreak;

		// Token: 0x04003AA6 RID: 15014
		public UnityEvent onRepair;

		// Token: 0x04003AA7 RID: 15015
		private bool dll_Excuted;

		// Token: 0x04003AA8 RID: 15016
		private bool dll_Excuted;
	}
}
