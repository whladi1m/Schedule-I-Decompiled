using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000931 RID: 2353
	[RequireComponent(typeof(InteractableObject))]
	public class ItemPickup : NetworkBehaviour
	{
		// Token: 0x06003FB2 RID: 16306 RVA: 0x0010BD48 File Offset: 0x00109F48
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ItemFramework.ItemPickup_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0010BD67 File Offset: 0x00109F67
		private void Start()
		{
			if (Player.Local != null)
			{
				this.Init();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Init));
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0010BD9D File Offset: 0x00109F9D
		private void Init()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Init));
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Init>g__Wait|9_0());
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0010BDD0 File Offset: 0x00109FD0
		protected virtual void Hovered()
		{
			if (this.CanPickup())
			{
				this.IntObj.SetMessage("Pick up " + this.ItemToGive.Name);
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Inventory Full");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0010BE2E File Offset: 0x0010A02E
		private void Interacted()
		{
			if (this.CanPickup())
			{
				this.Pickup();
			}
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0010BE3E File Offset: 0x0010A03E
		protected virtual bool CanPickup()
		{
			return this.ItemToGive != null && PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.ItemToGive.GetDefaultInstance(1), 1);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0010BE68 File Offset: 0x0010A068
		protected virtual void Pickup()
		{
			if (this.ItemToGive != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemToGive.GetDefaultInstance(1));
			}
			if (this.onPickup != null)
			{
				this.onPickup.Invoke();
			}
			if (this.DestroyOnPickup)
			{
				if (this.Networked)
				{
					this.Destroy();
					return;
				}
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0010BED0 File Offset: 0x0010A0D0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void Destroy()
		{
			this.RpcWriter___Server_Destroy_2166136261();
			this.RpcLogic___Destroy_2166136261();
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x0010BEFF File Offset: 0x0010A0FF
		[CompilerGenerated]
		private IEnumerator <Init>g__Wait|9_0()
		{
			yield return new WaitUntil(() => Player.Local.playerDataRetrieveReturned);
			if (this.ConditionallyActive && this.ActiveCondition != null)
			{
				base.gameObject.SetActive(this.ActiveCondition.Evaluate());
			}
			yield break;
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0010BF0E File Offset: 0x0010A10E
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_2166136261));
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0010BF38 File Offset: 0x0010A138
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0010BF4B File Offset: 0x0010A14B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0010BF5C File Offset: 0x0010A15C
		private void RpcWriter___Server_Destroy_2166136261()
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

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0010BFF8 File Offset: 0x0010A1F8
		public void RpcLogic___Destroy_2166136261()
		{
			if (base.IsServer)
			{
				base.NetworkObject.Despawn(null);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0010C02C File Offset: 0x0010A22C
		private void RpcReader___Server_Destroy_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___Destroy_2166136261();
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0010C05C File Offset: 0x0010A25C
		protected virtual void dll()
		{
			if (this.ItemToGive != null)
			{
				this.IntObj.SetMessage("Pick up " + this.ItemToGive.Name);
			}
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x04002DE4 RID: 11748
		public ItemDefinition ItemToGive;

		// Token: 0x04002DE5 RID: 11749
		public bool DestroyOnPickup = true;

		// Token: 0x04002DE6 RID: 11750
		public bool ConditionallyActive;

		// Token: 0x04002DE7 RID: 11751
		public Condition ActiveCondition;

		// Token: 0x04002DE8 RID: 11752
		public bool Networked = true;

		// Token: 0x04002DE9 RID: 11753
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04002DEA RID: 11754
		public UnityEvent onPickup;

		// Token: 0x04002DEB RID: 11755
		private bool dll_Excuted;

		// Token: 0x04002DEC RID: 11756
		private bool dll_Excuted;
	}
}
