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
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004DE RID: 1246
	public class ConsumeProductBehaviour : Behaviour
	{
		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x00076346 File Offset: 0x00074546
		// (set) Token: 0x06001C70 RID: 7280 RVA: 0x0007634E File Offset: 0x0007454E
		public ProductItemInstance ConsumedProduct { get; private set; }

		// Token: 0x06001C71 RID: 7281 RVA: 0x00076358 File Offset: 0x00074558
		protected virtual void Start()
		{
			ScheduleOne.GameTime.TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepEnd, new Action<int>(this.DayPass));
			ScheduleOne.GameTime.TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepEnd, new Action<int>(this.DayPass));
			if (this.TestProduct != null && Application.isEditor)
			{
				this.product = (this.TestProduct.GetDefaultInstance(1) as ProductItemInstance);
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000763D1 File Offset: 0x000745D1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendProduct(ProductItemInstance _product)
		{
			this.RpcWriter___Server_SendProduct_2622925554(_product);
			this.RpcLogic___SendProduct_2622925554(_product);
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000763E7 File Offset: 0x000745E7
		[ObserversRpc(RunLocally = true)]
		public void SetProduct(ProductItemInstance _product)
		{
			this.RpcWriter___Observers_SetProduct_2622925554(_product);
			this.RpcLogic___SetProduct_2622925554(_product);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000763FD File Offset: 0x000745FD
		[ObserversRpc(RunLocally = true)]
		public void ClearEffects()
		{
			this.RpcWriter___Observers_ClearEffects_2166136261();
			this.RpcLogic___ClearEffects_2166136261();
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0007640B File Offset: 0x0007460B
		protected override void Begin()
		{
			base.Begin();
			this.TryConsume();
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00076419 File Offset: 0x00074619
		protected override void Resume()
		{
			base.Resume();
			this.TryConsume();
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00076428 File Offset: 0x00074628
		private void TryConsume()
		{
			if (this.product == null)
			{
				Console.LogError("No product to consume", null);
				this.Disable();
				return;
			}
			switch ((this.product.Definition as ProductDefinition).DrugType)
			{
			case EDrugType.Marijuana:
				this.ConsumeWeed();
				return;
			case EDrugType.Methamphetamine:
				this.ConsumeMeth();
				return;
			case EDrugType.Cocaine:
				this.ConsumeCocaine();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0007648C File Offset: 0x0007468C
		public override void Disable()
		{
			base.Disable();
			this.Clear();
			this.End();
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000764A0 File Offset: 0x000746A0
		protected override void End()
		{
			base.End();
			if (this.consumeRoutine != null)
			{
				base.StopCoroutine(this.consumeRoutine);
				this.consumeRoutine = null;
			}
			base.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000764D4 File Offset: 0x000746D4
		private void ConsumeWeed()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeWeed>g__ConsumeWeedRoutine|23_0());
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000764E8 File Offset: 0x000746E8
		private void ConsumeMeth()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeMeth>g__ConsumeWeedRoutine|24_0());
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000764FC File Offset: 0x000746FC
		private void ConsumeCocaine()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeCocaine>g__ConsumeWeedRoutine|25_0());
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00076510 File Offset: 0x00074710
		[ObserversRpc]
		private void ApplyEffects()
		{
			this.RpcWriter___Observers_ApplyEffects_2166136261();
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00076518 File Offset: 0x00074718
		private void Clear()
		{
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00076535 File Offset: 0x00074735
		private void DayPass(int minsSlept)
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearEffects();
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00076545 File Offset: 0x00074745
		[CompilerGenerated]
		private IEnumerator <ConsumeWeed>g__ConsumeWeedRoutine|23_0()
		{
			base.Npc.SetEquippable_Return(this.JointPrefab.AssetPath);
			base.Npc.Avatar.Anim.SetBool("Smoking", true);
			this.WeedConsumeSound.Play();
			yield return new WaitForSeconds(3f);
			this.SmokeExhaleParticles.Play();
			yield return new WaitForSeconds(1.5f);
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00076554 File Offset: 0x00074754
		[CompilerGenerated]
		private IEnumerator <ConsumeMeth>g__ConsumeWeedRoutine|24_0()
		{
			base.Npc.SetEquippable_Return(this.PipePrefab.AssetPath);
			base.Npc.Avatar.Anim.SetBool("Smoking", true);
			this.MethConsumeSound.Play();
			yield return new WaitForSeconds(3f);
			this.SmokeExhaleParticles.Play();
			yield return new WaitForSeconds(1.5f);
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00076563 File Offset: 0x00074763
		[CompilerGenerated]
		private IEnumerator <ConsumeCocaine>g__ConsumeWeedRoutine|25_0()
		{
			base.Npc.Avatar.Anim.SetTrigger("Snort");
			yield return new WaitForSeconds(0.8f);
			this.SnortSound.Play();
			yield return new WaitForSeconds(1f);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00076574 File Offset: 0x00074774
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SendProduct_2622925554));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetProduct_2622925554));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_ClearEffects_2166136261));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyEffects_2166136261));
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000765F4 File Offset: 0x000747F4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x0007660D File Offset: 0x0007480D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x0007661C File Offset: 0x0007481C
		private void RpcWriter___Server_SendProduct_2622925554(ProductItemInstance _product)
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
			writer.WriteProductItemInstance(_product);
			base.SendServerRpc(15U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000766C3 File Offset: 0x000748C3
		public void RpcLogic___SendProduct_2622925554(ProductItemInstance _product)
		{
			this.SetProduct(_product);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000766CC File Offset: 0x000748CC
		private void RpcReader___Server_SendProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance productItemInstance = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendProduct_2622925554(productItemInstance);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0007670C File Offset: 0x0007490C
		private void RpcWriter___Observers_SetProduct_2622925554(ProductItemInstance _product)
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
			writer.WriteProductItemInstance(_product);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000767C2 File Offset: 0x000749C2
		public void RpcLogic___SetProduct_2622925554(ProductItemInstance _product)
		{
			this.product = _product;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000767CC File Offset: 0x000749CC
		private void RpcReader___Observers_SetProduct_2622925554(PooledReader PooledReader0, Channel channel)
		{
			ProductItemInstance productItemInstance = PooledReader0.ReadProductItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProduct_2622925554(productItemInstance);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00076808 File Offset: 0x00074A08
		private void RpcWriter___Observers_ClearEffects_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000768B1 File Offset: 0x00074AB1
		public void RpcLogic___ClearEffects_2166136261()
		{
			if (this.ConsumedProduct == null)
			{
				return;
			}
			this.ConsumedProduct.ClearEffectsFromNPC(base.Npc);
			this.ConsumedProduct = null;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000768D4 File Offset: 0x00074AD4
		private void RpcReader___Observers_ClearEffects_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearEffects_2166136261();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00076900 File Offset: 0x00074B00
		private void RpcWriter___Observers_ApplyEffects_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000769A9 File Offset: 0x00074BA9
		private void RpcLogic___ApplyEffects_2166136261()
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearEffects();
			}
			this.ConsumedProduct = this.product;
			if (this.product != null)
			{
				this.product.ApplyEffectsToNPC(base.Npc);
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000769E0 File Offset: 0x00074BE0
		private void RpcReader___Observers_ApplyEffects_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ApplyEffects_2166136261();
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00076A00 File Offset: 0x00074C00
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001753 RID: 5971
		public AvatarEquippable JointPrefab;

		// Token: 0x04001754 RID: 5972
		public AvatarEquippable PipePrefab;

		// Token: 0x04001756 RID: 5974
		private ProductItemInstance product;

		// Token: 0x04001757 RID: 5975
		private Coroutine consumeRoutine;

		// Token: 0x04001758 RID: 5976
		public AudioSourceController WeedConsumeSound;

		// Token: 0x04001759 RID: 5977
		public AudioSourceController MethConsumeSound;

		// Token: 0x0400175A RID: 5978
		public AudioSourceController SnortSound;

		// Token: 0x0400175B RID: 5979
		public ParticleSystem SmokeExhaleParticles;

		// Token: 0x0400175C RID: 5980
		[Header("Debug")]
		public ProductDefinition TestProduct;

		// Token: 0x0400175D RID: 5981
		public UnityEvent onConsumeDone;

		// Token: 0x0400175E RID: 5982
		private bool dll_Excuted;

		// Token: 0x0400175F RID: 5983
		private bool dll_Excuted;
	}
}
