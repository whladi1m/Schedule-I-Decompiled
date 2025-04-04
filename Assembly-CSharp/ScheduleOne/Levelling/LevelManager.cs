using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005A3 RID: 1443
	public class LevelManager : NetworkSingleton<LevelManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000919D2 File Offset: 0x0008FBD2
		// (set) Token: 0x060023E5 RID: 9189 RVA: 0x000919DA File Offset: 0x0008FBDA
		public ERank Rank { get; private set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000919E3 File Offset: 0x0008FBE3
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000919EB File Offset: 0x0008FBEB
		public int Tier { get; private set; } = 1;

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000919F4 File Offset: 0x0008FBF4
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000919FC File Offset: 0x0008FBFC
		public int XP { get; private set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x00091A05 File Offset: 0x0008FC05
		// (set) Token: 0x060023EB RID: 9195 RVA: 0x00091A0D File Offset: 0x0008FC0D
		public int TotalXP { get; private set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x00091A16 File Offset: 0x0008FC16
		public float XPToNextTier
		{
			get
			{
				return Mathf.Round(Mathf.Lerp(200f, 2500f, (float)this.Rank / (float)this.rankCount) / 25f) * 25f;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x00091A47 File Offset: 0x0008FC47
		public string SaveFolderName
		{
			get
			{
				return "Rank";
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x00091A47 File Offset: 0x0008FC47
		public string SaveFileName
		{
			get
			{
				return "Rank";
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x00091A4E File Offset: 0x0008FC4E
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x00091A56 File Offset: 0x0008FC56
		// (set) Token: 0x060023F2 RID: 9202 RVA: 0x00091A5E File Offset: 0x0008FC5E
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x00091A67 File Offset: 0x0008FC67
		// (set) Token: 0x060023F4 RID: 9204 RVA: 0x00091A6F File Offset: 0x0008FC6F
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x00091A78 File Offset: 0x0008FC78
		// (set) Token: 0x060023F6 RID: 9206 RVA: 0x00091A80 File Offset: 0x0008FC80
		public bool HasChanged { get; set; }

		// Token: 0x060023F7 RID: 9207 RVA: 0x00091A89 File Offset: 0x0008FC89
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Levelling.LevelManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x00091A9D File Offset: 0x0008FC9D
		protected override void Start()
		{
			base.Start();
			this.InitializeSaveable();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00091AAB File Offset: 0x0008FCAB
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetData(connection, this.Rank, this.Tier, this.XP, this.TotalXP);
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00091AD3 File Offset: 0x0008FCD3
		[ServerRpc(RequireOwnership = false)]
		public void AddXP(int xp)
		{
			this.RpcWriter___Server_AddXP_3316948804(xp);
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x00091AE0 File Offset: 0x0008FCE0
		[ObserversRpc]
		private void AddXPLocal(int xp)
		{
			this.RpcWriter___Observers_AddXPLocal_3316948804(xp);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x00091AF8 File Offset: 0x0008FCF8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetData(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetData_20965027(conn, rank, tier, xp, totalXp);
				this.RpcLogic___SetData_20965027(conn, rank, tier, xp, totalXp);
			}
			else
			{
				this.RpcWriter___Target_SetData_20965027(conn, rank, tier, xp, totalXp);
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x00091B60 File Offset: 0x0008FD60
		[ObserversRpc]
		private void IncreaseTierNetworked(FullRank before, FullRank after)
		{
			this.RpcWriter___Observers_IncreaseTierNetworked_3953286437(before, after);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x00091B7C File Offset: 0x0008FD7C
		private void IncreaseTier()
		{
			this.XP -= (int)this.XPToNextTier;
			int tier = this.Tier;
			this.Tier = tier + 1;
			if (this.Tier > 5 && this.Rank != ERank.Kingpin)
			{
				this.Tier = 1;
				ERank rank = this.Rank;
				this.Rank = rank + 1;
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x00091BD7 File Offset: 0x0008FDD7
		public virtual string GetSaveString()
		{
			return new RankData((int)this.Rank, this.Tier, this.XP, this.TotalXP).GetJson(true);
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x00091BFC File Offset: 0x0008FDFC
		public FullRank GetFullRank()
		{
			return new FullRank(this.Rank, this.Tier);
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x00091C10 File Offset: 0x0008FE10
		public void AddUnlockable(Unlockable unlockable)
		{
			if (!this.Unlockables.ContainsKey(unlockable.Rank))
			{
				this.Unlockables.Add(unlockable.Rank, new List<Unlockable>());
			}
			if (this.Unlockables[unlockable.Rank].Find((Unlockable x) => x.Title.ToLower() == unlockable.Title.ToLower() && x.Icon == unlockable.Icon) != null)
			{
				return;
			}
			this.Unlockables[unlockable.Rank].Add(unlockable);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x00091CA8 File Offset: 0x0008FEA8
		public int GetTotalXPForRank(FullRank fullrank)
		{
			int num = 0;
			foreach (ERank erank in (ERank[])Enum.GetValues(typeof(ERank)))
			{
				int xpforTier = this.GetXPForTier(erank);
				for (int j = 1; j <= 5; j++)
				{
					if (erank == fullrank.Rank && j == fullrank.Tier)
					{
						return num;
					}
					num += xpforTier;
				}
			}
			Console.LogError("Rank not found: " + fullrank.ToString(), null);
			return 0;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x00091D30 File Offset: 0x0008FF30
		public FullRank GetFullRank(int totalXp)
		{
			int num = totalXp;
			foreach (ERank rank in (ERank[])Enum.GetValues(typeof(ERank)))
			{
				int xpforTier = this.GetXPForTier(rank);
				for (int j = 1; j <= 5; j++)
				{
					if (num < xpforTier)
					{
						return new FullRank(rank, j);
					}
					num -= xpforTier;
				}
			}
			Console.LogError("Rank not found for XP: " + totalXp.ToString(), null);
			return new FullRank(ERank.Street_Rat, 1);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x00091DB0 File Offset: 0x0008FFB0
		public int GetXPForTier(ERank rank)
		{
			return Mathf.RoundToInt(Mathf.Round(Mathf.Lerp(200f, 2500f, (float)rank / (float)this.rankCount) / 25f) * 25f);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x00091DE4 File Offset: 0x0008FFE4
		public static float GetOrderLimitMultiplier(FullRank rank)
		{
			float rankOrderLimitMultiplier = LevelManager.GetRankOrderLimitMultiplier(rank.Rank);
			if (rank.Rank < ERank.Kingpin)
			{
				float rankOrderLimitMultiplier2 = LevelManager.GetRankOrderLimitMultiplier(rank.Rank + 1);
				float t = (float)(rank.Tier - 1) / 4f;
				return Mathf.Lerp(rankOrderLimitMultiplier, rankOrderLimitMultiplier2, t);
			}
			return Mathf.Clamp(LevelManager.GetRankOrderLimitMultiplier(ERank.Kingpin) + 0.1f * (float)(rank.Tier - 1), 1f, 10f);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x00091E54 File Offset: 0x00090054
		private static float GetRankOrderLimitMultiplier(ERank rank)
		{
			switch (rank)
			{
			case ERank.Street_Rat:
				return 1f;
			case ERank.Hoodlum:
				return 1.25f;
			case ERank.Peddler:
				return 1.5f;
			case ERank.Hustler:
				return 1.75f;
			case ERank.Bagman:
				return 2f;
			case ERank.Enforcer:
				return 2.25f;
			case ERank.Shot_Caller:
				return 2.5f;
			case ERank.Block_Boss:
				return 2.75f;
			case ERank.Underlord:
				return 3f;
			case ERank.Baron:
				return 3.25f;
			case ERank.Kingpin:
				return 3.5f;
			default:
				return 1f;
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x00091F18 File Offset: 0x00090118
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_AddXP_3316948804));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddXPLocal_3316948804));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_SetData_20965027));
			base.RegisterTargetRpc(3U, new ClientRpcDelegate(this.RpcReader___Target_SetData_20965027));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_IncreaseTierNetworked_3953286437));
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x00091FAF File Offset: 0x000901AF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x00091FC8 File Offset: 0x000901C8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00091FD8 File Offset: 0x000901D8
		private void RpcWriter___Server_AddXP_3316948804(int xp)
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
			writer.WriteInt32(xp, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x00092084 File Offset: 0x00090284
		public void RpcLogic___AddXP_3316948804(int xp)
		{
			this.AddXPLocal(xp);
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x00092090 File Offset: 0x00090290
		private void RpcReader___Server_AddXP_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int xp = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___AddXP_3316948804(xp);
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000920C8 File Offset: 0x000902C8
		private void RpcWriter___Observers_AddXPLocal_3316948804(int xp)
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
			writer.WriteInt32(xp, AutoPackType.Packed);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x00092184 File Offset: 0x00090384
		private void RpcLogic___AddXPLocal_3316948804(int xp)
		{
			NetworkSingleton<DailySummary>.Instance.AddXP(xp);
			this.XP += xp;
			this.TotalXP += xp;
			this.HasChanged = true;
			Console.Log(string.Concat(new string[]
			{
				"Rank progress: ",
				this.XP.ToString(),
				"/",
				this.XPToNextTier.ToString(),
				" (Total ",
				this.TotalXP.ToString(),
				")"
			}), null);
			if (InstanceFinder.IsServer)
			{
				FullRank fullRank = this.GetFullRank();
				bool flag = false;
				while ((float)this.XP >= this.XPToNextTier)
				{
					this.IncreaseTier();
					flag = true;
				}
				this.SetData(null, this.Rank, this.Tier, this.XP, this.TotalXP);
				if (flag)
				{
					this.IncreaseTierNetworked(fullRank, this.GetFullRank());
				}
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0009227C File Offset: 0x0009047C
		private void RpcReader___Observers_AddXPLocal_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int xp = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddXPLocal_3316948804(xp);
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000922B4 File Offset: 0x000904B4
		private void RpcWriter___Observers_SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
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
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(rank);
			writer.WriteInt32(tier, AutoPackType.Packed);
			writer.WriteInt32(xp, AutoPackType.Packed);
			writer.WriteInt32(totalXp, AutoPackType.Packed);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000923A0 File Offset: 0x000905A0
		public void RpcLogic___SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
		{
			this.Rank = rank;
			this.Tier = tier;
			this.XP = xp;
			this.TotalXP = totalXp;
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000923C0 File Offset: 0x000905C0
		private void RpcReader___Observers_SetData_20965027(PooledReader PooledReader0, Channel channel)
		{
			ERank rank = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(PooledReader0);
			int tier = PooledReader0.ReadInt32(AutoPackType.Packed);
			int xp = PooledReader0.ReadInt32(AutoPackType.Packed);
			int totalXp = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetData_20965027(null, rank, tier, xp, totalXp);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00092440 File Offset: 0x00090640
		private void RpcWriter___Target_SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
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
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(rank);
			writer.WriteInt32(tier, AutoPackType.Packed);
			writer.WriteInt32(xp, AutoPackType.Packed);
			writer.WriteInt32(totalXp, AutoPackType.Packed);
			base.SendTargetRpc(3U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0009252C File Offset: 0x0009072C
		private void RpcReader___Target_SetData_20965027(PooledReader PooledReader0, Channel channel)
		{
			ERank rank = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(PooledReader0);
			int tier = PooledReader0.ReadInt32(AutoPackType.Packed);
			int xp = PooledReader0.ReadInt32(AutoPackType.Packed);
			int totalXp = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetData_20965027(base.LocalConnection, rank, tier, xp, totalXp);
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000925A8 File Offset: 0x000907A8
		private void RpcWriter___Observers_IncreaseTierNetworked_3953286437(FullRank before, FullRank after)
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
			writer.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated(before);
			writer.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated(after);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0009266C File Offset: 0x0009086C
		private void RpcLogic___IncreaseTierNetworked_3953286437(FullRank before, FullRank after)
		{
			Action<FullRank, FullRank> action = this.onRankUp;
			if (action != null)
			{
				action(before, after);
			}
			this.HasChanged = true;
			Console.Log("Ranked up to " + this.Rank.ToString() + ": " + this.Tier.ToString(), null);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000926CC File Offset: 0x000908CC
		private void RpcReader___Observers_IncreaseTierNetworked_3953286437(PooledReader PooledReader0, Channel channel)
		{
			FullRank before = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds(PooledReader0);
			FullRank after = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___IncreaseTierNetworked_3953286437(before, after);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0009270E File Offset: 0x0009090E
		protected virtual void dll()
		{
			base.Awake();
			this.rankCount = Enum.GetValues(typeof(ERank)).Length;
		}

		// Token: 0x04001AD8 RID: 6872
		public const int TIERS_PER_RANK = 5;

		// Token: 0x04001AD9 RID: 6873
		public const int XP_PER_TIER_MIN = 200;

		// Token: 0x04001ADA RID: 6874
		public const int XP_PER_TIER_MAX = 2500;

		// Token: 0x04001ADC RID: 6876
		private int rankCount;

		// Token: 0x04001AE0 RID: 6880
		public Action<FullRank, FullRank> onRankUp;

		// Token: 0x04001AE1 RID: 6881
		public Dictionary<FullRank, List<Unlockable>> Unlockables = new Dictionary<FullRank, List<Unlockable>>();

		// Token: 0x04001AE2 RID: 6882
		private RankLoader loader = new RankLoader();

		// Token: 0x04001AE6 RID: 6886
		private bool dll_Excuted;

		// Token: 0x04001AE7 RID: 6887
		private bool dll_Excuted;
	}
}
