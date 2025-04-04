using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000719 RID: 1817
	public abstract class OptionListFeature : Feature
	{
		// Token: 0x06003134 RID: 12596 RVA: 0x000CBF2D File Offset: 0x000CA12D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Construction.Features.OptionListFeature_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000CBF44 File Offset: 0x000CA144
		public override FI_Base CreateInterface(Transform parent)
		{
			FI_OptionList component = UnityEngine.Object.Instantiate<GameObject>(this.featureInterfacePrefab, parent).GetComponent<FI_OptionList>();
			component.Initialize(this, this.GetOptions());
			component.onSelectionChanged.AddListener(new UnityAction<int>(this.SelectOption));
			component.onSelectionPurchased.AddListener(new UnityAction<int>(this.PurchaseOption));
			return component;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000CBF9F File Offset: 0x000CA19F
		public override void Default()
		{
			this.PurchaseOption(this.defaultOptionIndex);
		}

		// Token: 0x06003137 RID: 12599
		protected abstract List<FI_OptionList.Option> GetOptions();

		// Token: 0x06003138 RID: 12600 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void SelectOption(int optionIndex)
		{
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000CBFAD File Offset: 0x000CA1AD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(int colorIndex)
		{
			this.RpcWriter___Server_SetData_3316948804(colorIndex);
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x000CBFC3 File Offset: 0x000CA1C3
		private void ReceiveData()
		{
			this.SelectOption(this.SyncAccessor_ownedOptionIndex);
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000CBFD1 File Offset: 0x000CA1D1
		public virtual void PurchaseOption(int optionIndex)
		{
			this.SetData(optionIndex);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000CBFE4 File Offset: 0x000CA1E4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___ownedOptionIndex = new SyncVar<int>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.ownedOptionIndex);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetData_3316948804));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Construction.Features.OptionListFeature));
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000CC05C File Offset: 0x000CA25C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.OptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___ownedOptionIndex.SetRegistered();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000CC080 File Offset: 0x000CA280
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000CC090 File Offset: 0x000CA290
		private void RpcWriter___Server_SetData_3316948804(int colorIndex)
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
			writer.WriteInt32(colorIndex, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000CC13C File Offset: 0x000CA33C
		protected virtual void RpcLogic___SetData_3316948804(int colorIndex)
		{
			if (!base.IsSpawned)
			{
				this.SelectOption(colorIndex);
				return;
			}
			this.sync___set_value_ownedOptionIndex(colorIndex, true);
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000CC158 File Offset: 0x000CA358
		private void RpcReader___Server_SetData_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int colorIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x000CC19B File Offset: 0x000CA39B
		// (set) Token: 0x06003144 RID: 12612 RVA: 0x000CC1A3 File Offset: 0x000CA3A3
		public int SyncAccessor_ownedOptionIndex
		{
			get
			{
				return this.ownedOptionIndex;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.ownedOptionIndex = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___ownedOptionIndex.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000CC1E0 File Offset: 0x000CA3E0
		public virtual bool OptionListFeature(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_ownedOptionIndex(this.syncVar___ownedOptionIndex.GetValue(true), true);
				return true;
			}
			int value = PooledReader0.ReadInt32(AutoPackType.Packed);
			this.sync___set_value_ownedOptionIndex(value, Boolean2);
			return true;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000CC237 File Offset: 0x000CA437
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x04002333 RID: 9011
		[Header("Option list feature settings")]
		public int defaultOptionIndex;

		// Token: 0x04002334 RID: 9012
		[SyncVar]
		public int ownedOptionIndex;

		// Token: 0x04002335 RID: 9013
		public SyncVar<int> syncVar___ownedOptionIndex;

		// Token: 0x04002336 RID: 9014
		private bool dll_Excuted;

		// Token: 0x04002337 RID: 9015
		private bool dll_Excuted;
	}
}
