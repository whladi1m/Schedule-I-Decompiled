using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B6C RID: 2924
	public class VendingMachine : NetworkBehaviour, IGUIDRegisterable, IGenericSaveable
	{
		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06004E06 RID: 19974 RVA: 0x00149504 File Offset: 0x00147704
		// (set) Token: 0x06004E07 RID: 19975 RVA: 0x0014950C File Offset: 0x0014770C
		public bool IsBroken { get; protected set; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x00149515 File Offset: 0x00147715
		// (set) Token: 0x06004E09 RID: 19977 RVA: 0x0014951D File Offset: 0x0014771D
		public int DaysUntilRepair { get; protected set; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x00149526 File Offset: 0x00147726
		// (set) Token: 0x06004E0B RID: 19979 RVA: 0x0014952E File Offset: 0x0014772E
		public ItemPickup lastDroppedItem { get; protected set; }

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x00149537 File Offset: 0x00147737
		// (set) Token: 0x06004E0D RID: 19981 RVA: 0x0014953F File Offset: 0x0014773F
		public Guid GUID { get; protected set; }

		// Token: 0x06004E0E RID: 19982 RVA: 0x00149548 File Offset: 0x00147748
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x00149570 File Offset: 0x00147770
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.VendingMachine_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x00149590 File Offset: 0x00147790
		private void Start()
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.DayPass));
			this.SetLit(false);
			((IGenericSaveable)this).InitializeSaveable();
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x00149606 File Offset: 0x00147806
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsBroken)
			{
				this.Break(connection);
			}
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0014961E File Offset: 0x0014781E
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x0014962D File Offset: 0x0014782D
		private void OnDestroy()
		{
			if (VendingMachine.AllMachines.Contains(this))
			{
				VendingMachine.AllMachines.Remove(this);
			}
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			}
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0014966C File Offset: 0x0014786C
		private void MinPass()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(this.LitStartTime, this.LitOnEndTime) && !this.IsBroken)
			{
				if (!this.isLit)
				{
					this.SetLit(true);
					return;
				}
			}
			else if (this.isLit)
			{
				this.SetLit(false);
			}
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x001496B8 File Offset: 0x001478B8
		public void DayPass()
		{
			if (this.IsBroken)
			{
				int daysUntilRepair = this.DaysUntilRepair;
				this.DaysUntilRepair = daysUntilRepair - 1;
				if (this.DaysUntilRepair <= 0)
				{
					this.Repair();
				}
			}
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x001496EC File Offset: 0x001478EC
		public void Hovered()
		{
			if (this.purchaseInProgress)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.IsBroken)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= 2f)
			{
				this.IntObj.SetMessage("Purchase Cuke");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Not enough cash");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x0014976D File Offset: 0x0014796D
		public void Interacted()
		{
			if (this.purchaseInProgress)
			{
				return;
			}
			if (this.IsBroken)
			{
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= 2f)
			{
				this.LocalPurchase();
			}
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00149798 File Offset: 0x00147998
		private void LocalPurchase()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-2f, true, false);
			this.SendPurchase();
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x001497B1 File Offset: 0x001479B1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPurchase()
		{
			this.RpcWriter___Server_SendPurchase_2166136261();
			this.RpcLogic___SendPurchase_2166136261();
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x001497BF File Offset: 0x001479BF
		[ObserversRpc(RunLocally = true)]
		public void PurchaseRoutine()
		{
			this.RpcWriter___Observers_PurchaseRoutine_2166136261();
			this.RpcLogic___PurchaseRoutine_2166136261();
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x001497D0 File Offset: 0x001479D0
		[ServerRpc(RequireOwnership = false)]
		public void DropItem()
		{
			this.RpcWriter___Server_DropItem_2166136261();
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x001497E3 File Offset: 0x001479E3
		public void RemoveLastDropped()
		{
			if (this.lastDroppedItem != null && this.lastDroppedItem.gameObject != null)
			{
				this.lastDroppedItem.Destroy();
				this.lastDroppedItem = null;
			}
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x00149818 File Offset: 0x00147A18
		private void Impacted(Impact impact)
		{
			if (impact.ImpactForce < 50f)
			{
				return;
			}
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
				base.StartCoroutine(this.<Impacted>g__BreakRoutine|64_0());
				return;
			}
			if (UnityEngine.Random.value < 0.33f && Time.time - this.timeOnLastFreeItem > 10f)
			{
				this.timeOnLastFreeItem = Time.time;
				base.StartCoroutine(this.<Impacted>g__Drop|64_1());
			}
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x001498E0 File Offset: 0x00147AE0
		private void SetLit(bool lit)
		{
			this.isLit = lit;
			if (this.isLit)
			{
				Material[] materials = this.DoorMesh.materials;
				materials[1] = this.DoorOnMat;
				this.DoorMesh.materials = materials;
				Material[] materials2 = this.BodyMesh.materials;
				materials2[1] = this.BodyOnMat;
				this.BodyMesh.materials = materials2;
			}
			else
			{
				Material[] materials3 = this.DoorMesh.materials;
				materials3[1] = this.DoorOffMat;
				this.DoorMesh.materials = materials3;
				Material[] materials4 = this.BodyMesh.materials;
				materials4[1] = this.BodyOffMat;
				this.BodyMesh.materials = materials4;
			}
			for (int i = 0; i < this.Lights.Length; i++)
			{
				this.Lights[i].Enabled = this.isLit;
			}
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x001499AD File Offset: 0x00147BAD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBreak()
		{
			this.RpcWriter___Server_SendBreak_2166136261();
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x001499BB File Offset: 0x00147BBB
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

		// Token: 0x06004E21 RID: 20001 RVA: 0x001499E5 File Offset: 0x00147BE5
		[ObserversRpc]
		private void Repair()
		{
			this.RpcWriter___Observers_Repair_2166136261();
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x001499F0 File Offset: 0x00147BF0
		[ServerRpc(RequireOwnership = false)]
		private void DropCash()
		{
			this.RpcWriter___Server_DropCash_2166136261();
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x00149A03 File Offset: 0x00147C03
		public void Load(GenericSaveData data)
		{
			this.IsBroken = data.GetBool("broken", false);
			this.DaysUntilRepair = data.GetInt("daysUntilRepair", 0);
			if (this.IsBroken)
			{
				this.Break(null);
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x00149A38 File Offset: 0x00147C38
		public GenericSaveData GetSaveData()
		{
			GenericSaveData genericSaveData = new GenericSaveData(this.GUID.ToString());
			genericSaveData.Add("broken", this.IsBroken);
			genericSaveData.Add("daysUntilRepair", this.DaysUntilRepair);
			return genericSaveData;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x00149AB5 File Offset: 0x00147CB5
		[CompilerGenerated]
		private IEnumerator <PurchaseRoutine>g__Routine|61_0()
		{
			this.PaySound.Play();
			this.DispenseSound.Play();
			this.Anim.Play();
			yield return new WaitForSeconds(0.65f);
			if (base.IsServer)
			{
				this.DropItem();
			}
			this.purchaseInProgress = false;
			yield break;
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x00149AC4 File Offset: 0x00147CC4
		[CompilerGenerated]
		private IEnumerator <Impacted>g__BreakRoutine|64_0()
		{
			int cashDrop = UnityEngine.Random.Range(1, 5);
			int num;
			for (int i = 0; i < cashDrop; i = num + 1)
			{
				this.DropCash();
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield break;
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x00149AD3 File Offset: 0x00147CD3
		[CompilerGenerated]
		private IEnumerator <Impacted>g__Drop|64_1()
		{
			this.DispenseSound.Play();
			yield return new WaitForSeconds(0.65f);
			this.DropItem();
			yield break;
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x00149AE4 File Offset: 0x00147CE4
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendPurchase_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_PurchaseRoutine_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_DropItem_2166136261));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendBreak_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Break_328543758));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_Break_328543758));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_Repair_2166136261));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_DropCash_2166136261));
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x00149BBA File Offset: 0x00147DBA
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x00149BCD File Offset: 0x00147DCD
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x00149BDC File Offset: 0x00147DDC
		private void RpcWriter___Server_SendPurchase_2166136261()
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

		// Token: 0x06004E2E RID: 20014 RVA: 0x00149C76 File Offset: 0x00147E76
		public void RpcLogic___SendPurchase_2166136261()
		{
			this.PurchaseRoutine();
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x00149C80 File Offset: 0x00147E80
		private void RpcReader___Server_SendPurchase_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPurchase_2166136261();
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x00149CB0 File Offset: 0x00147EB0
		private void RpcWriter___Observers_PurchaseRoutine_2166136261()
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

		// Token: 0x06004E31 RID: 20017 RVA: 0x00149D59 File Offset: 0x00147F59
		public void RpcLogic___PurchaseRoutine_2166136261()
		{
			if (this.purchaseInProgress)
			{
				return;
			}
			this.purchaseInProgress = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<PurchaseRoutine>g__Routine|61_0());
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x00149D7C File Offset: 0x00147F7C
		private void RpcReader___Observers_PurchaseRoutine_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PurchaseRoutine_2166136261();
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x00149DA8 File Offset: 0x00147FA8
		private void RpcWriter___Server_DropItem_2166136261()
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
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x00149E44 File Offset: 0x00148044
		public void RpcLogic___DropItem_2166136261()
		{
			ItemPickup itemPickup = UnityEngine.Object.Instantiate<ItemPickup>(this.CukePrefab, this.ItemSpawnPoint.position, this.ItemSpawnPoint.rotation);
			base.Spawn(itemPickup.gameObject, null, default(Scene));
			this.lastDroppedItem = itemPickup;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00149E90 File Offset: 0x00148090
		private void RpcReader___Server_DropItem_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropItem_2166136261();
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x00149EB0 File Offset: 0x001480B0
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
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x00149F4A File Offset: 0x0014814A
		private void RpcLogic___SendBreak_2166136261()
		{
			this.DaysUntilRepair = 2;
			this.Break(null);
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x00149F5C File Offset: 0x0014815C
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

		// Token: 0x06004E39 RID: 20025 RVA: 0x00149F8C File Offset: 0x0014818C
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0014A035 File Offset: 0x00148235
		private void RpcLogic___Break_328543758(NetworkConnection conn)
		{
			if (this.IsBroken)
			{
				return;
			}
			this.IsBroken = true;
			this.SetLit(false);
			UnityEvent unityEvent = this.onBreak;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x0014A060 File Offset: 0x00148260
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

		// Token: 0x06004E3C RID: 20028 RVA: 0x0014A08C File Offset: 0x0014828C
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
			base.SendTargetRpc(5U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x0014A134 File Offset: 0x00148334
		private void RpcReader___Target_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Break_328543758(base.LocalConnection);
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x0014A15C File Offset: 0x0014835C
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
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x0014A205 File Offset: 0x00148405
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

		// Token: 0x06004E40 RID: 20032 RVA: 0x0014A228 File Offset: 0x00148428
		private void RpcReader___Observers_Repair_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Repair_2166136261();
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x0014A248 File Offset: 0x00148448
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
			base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x0014A2E4 File Offset: 0x001484E4
		private void RpcLogic___DropCash_2166136261()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CashPrefab.gameObject, this.CashSpawnPoint.position, this.CashSpawnPoint.rotation);
			gameObject.GetComponent<Rigidbody>().AddForce(this.CashSpawnPoint.forward * UnityEngine.Random.Range(1.5f, 2.5f), ForceMode.VelocityChange);
			gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, ForceMode.VelocityChange);
			base.Spawn(gameObject.gameObject, null, default(Scene));
			this.PaySound.Play();
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x0014A380 File Offset: 0x00148580
		private void RpcReader___Server_DropCash_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropCash_2166136261();
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x0014A3A0 File Offset: 0x001485A0
		private void dll()
		{
			if (!VendingMachine.AllMachines.Contains(this))
			{
				VendingMachine.AllMachines.Add(this);
			}
			PhysicsDamageable damageable = this.Damageable;
			damageable.onImpacted = (Action<Impact>)Delegate.Combine(damageable.onImpacted, new Action<Impact>(this.Impacted));
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x04003B0F RID: 15119
		public static List<VendingMachine> AllMachines = new List<VendingMachine>();

		// Token: 0x04003B10 RID: 15120
		public const float COST = 2f;

		// Token: 0x04003B11 RID: 15121
		public const int REPAIR_TIME_DAYS = 2;

		// Token: 0x04003B12 RID: 15122
		public const float IMPACT_THRESHOLD_FREE_ITEM = 50f;

		// Token: 0x04003B13 RID: 15123
		public const float IMPACT_THRESHOLD_FREE_ITEM_CHANCE = 0.33f;

		// Token: 0x04003B14 RID: 15124
		public const float IMPACT_THRESHOLD_BREAK = 165f;

		// Token: 0x04003B15 RID: 15125
		public const int MIN_CASH_DROP = 1;

		// Token: 0x04003B16 RID: 15126
		public const int MAX_CASH_DROP = 4;

		// Token: 0x04003B19 RID: 15129
		[Header("Settings")]
		public int LitStartTime = 1700;

		// Token: 0x04003B1A RID: 15130
		public int LitOnEndTime = 800;

		// Token: 0x04003B1B RID: 15131
		public ItemPickup CukePrefab;

		// Token: 0x04003B1C RID: 15132
		public CashPickup CashPrefab;

		// Token: 0x04003B1D RID: 15133
		[Header("References")]
		public MeshRenderer DoorMesh;

		// Token: 0x04003B1E RID: 15134
		public MeshRenderer BodyMesh;

		// Token: 0x04003B1F RID: 15135
		public Material DoorOffMat;

		// Token: 0x04003B20 RID: 15136
		public Material DoorOnMat;

		// Token: 0x04003B21 RID: 15137
		public Material BodyOffMat;

		// Token: 0x04003B22 RID: 15138
		public Material BodyOnMat;

		// Token: 0x04003B23 RID: 15139
		public OptimizedLight[] Lights;

		// Token: 0x04003B24 RID: 15140
		public AudioSourceController PaySound;

		// Token: 0x04003B25 RID: 15141
		public AudioSourceController DispenseSound;

		// Token: 0x04003B26 RID: 15142
		public Animation Anim;

		// Token: 0x04003B27 RID: 15143
		public Transform ItemSpawnPoint;

		// Token: 0x04003B28 RID: 15144
		public InteractableObject IntObj;

		// Token: 0x04003B29 RID: 15145
		public Transform AccessPoint;

		// Token: 0x04003B2A RID: 15146
		public PhysicsDamageable Damageable;

		// Token: 0x04003B2B RID: 15147
		public Transform CashSpawnPoint;

		// Token: 0x04003B2C RID: 15148
		public UnityEvent onBreak;

		// Token: 0x04003B2D RID: 15149
		public UnityEvent onRepair;

		// Token: 0x04003B2F RID: 15151
		private bool isLit;

		// Token: 0x04003B30 RID: 15152
		private bool purchaseInProgress;

		// Token: 0x04003B31 RID: 15153
		private float timeOnLastFreeItem;

		// Token: 0x04003B33 RID: 15155
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003B34 RID: 15156
		private bool dll_Excuted;

		// Token: 0x04003B35 RID: 15157
		private bool dll_Excuted;
	}
}
