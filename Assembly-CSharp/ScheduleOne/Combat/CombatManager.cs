using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000722 RID: 1826
	public class CombatManager : NetworkSingleton<CombatManager>
	{
		// Token: 0x06003174 RID: 12660 RVA: 0x000CD7CC File Offset: 0x000CB9CC
		[Button]
		public void CreateTestExplosion()
		{
			Vector3 origin = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 10f;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(10f, out raycastHit, this.ExplosionLayerMask, true, 0f))
			{
				origin = raycastHit.point;
			}
			this.CreateExplosion(origin, ExplosionData.DefaultSmall);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000CD83C File Offset: 0x000CBA3C
		public void CreateExplosion(Vector3 origin, ExplosionData data)
		{
			int id = UnityEngine.Random.Range(0, int.MaxValue);
			this.CreateExplosion(origin, data, id);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000CD85E File Offset: 0x000CBA5E
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void CreateExplosion(Vector3 origin, ExplosionData data, int id)
		{
			this.RpcWriter___Server_CreateExplosion_2907189355(origin, data, id);
			this.RpcLogic___CreateExplosion_2907189355(origin, data, id);
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000CD884 File Offset: 0x000CBA84
		[ObserversRpc(RunLocally = true)]
		private void Explosion(Vector3 origin, ExplosionData data, int id)
		{
			this.RpcWriter___Observers_Explosion_2907189355(origin, data, id);
			this.RpcLogic___Explosion_2907189355(origin, data, id);
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x000CD8C0 File Offset: 0x000CBAC0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateExplosion_2907189355));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Explosion_2907189355));
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x000CD912 File Offset: 0x000CBB12
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000CD92B File Offset: 0x000CBB2B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x000CD93C File Offset: 0x000CBB3C
		private void RpcWriter___Server_CreateExplosion_2907189355(Vector3 origin, ExplosionData data, int id)
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
			writer.WriteVector3(origin);
			writer.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated(data);
			writer.WriteInt32(id, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000CDA02 File Offset: 0x000CBC02
		private void RpcLogic___CreateExplosion_2907189355(Vector3 origin, ExplosionData data, int id)
		{
			this.Explosion(origin, data, id);
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000CDA10 File Offset: 0x000CBC10
		private void RpcReader___Server_CreateExplosion_2907189355(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 origin = PooledReader0.ReadVector3();
			ExplosionData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds(PooledReader0);
			int id = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateExplosion_2907189355(origin, data, id);
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000CDA78 File Offset: 0x000CBC78
		private void RpcWriter___Observers_Explosion_2907189355(Vector3 origin, ExplosionData data, int id)
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
			writer.WriteVector3(origin);
			writer.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated(data);
			writer.WriteInt32(id, AutoPackType.Packed);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x000CDB4D File Offset: 0x000CBD4D
		private void RpcLogic___Explosion_2907189355(Vector3 origin, ExplosionData data, int id)
		{
			if (this.explosionIDs.Contains(id))
			{
				return;
			}
			this.explosionIDs.Add(id);
			Explosion explosion = UnityEngine.Object.Instantiate<Explosion>(this.ExplosionPrefab);
			explosion.Initialize(origin, data);
			UnityEngine.Object.Destroy(explosion.gameObject, 3f);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x000CDB8C File Offset: 0x000CBD8C
		private void RpcReader___Observers_Explosion_2907189355(PooledReader PooledReader0, Channel channel)
		{
			Vector3 origin = PooledReader0.ReadVector3();
			ExplosionData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds(PooledReader0);
			int id = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Explosion_2907189355(origin, data, id);
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x000CDBEE File Offset: 0x000CBDEE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002353 RID: 9043
		public LayerMask MeleeLayerMask;

		// Token: 0x04002354 RID: 9044
		public LayerMask ExplosionLayerMask;

		// Token: 0x04002355 RID: 9045
		public LayerMask RangedWeaponLayerMask;

		// Token: 0x04002356 RID: 9046
		public Explosion ExplosionPrefab;

		// Token: 0x04002357 RID: 9047
		private List<int> explosionIDs = new List<int>();

		// Token: 0x04002358 RID: 9048
		private bool dll_Excuted;

		// Token: 0x04002359 RID: 9049
		private bool dll_Excuted;
	}
}
