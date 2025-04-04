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
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.CrashReportHandler;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006CA RID: 1738
	public class GameManager : NetworkSingleton<GameManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002F56 RID: 12118 RVA: 0x000C59A0 File Offset: 0x000C3BA0
		public static bool IS_TUTORIAL
		{
			get
			{
				return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial";
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002F57 RID: 12119 RVA: 0x000C59C4 File Offset: 0x000C3BC4
		public static int Seed
		{
			get
			{
				if (NetworkSingleton<GameManager>.Instance != null)
				{
					return NetworkSingleton<GameManager>.Instance.seed;
				}
				return 0;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002F58 RID: 12120 RVA: 0x000C59DF File Offset: 0x000C3BDF
		// (set) Token: 0x06002F59 RID: 12121 RVA: 0x000C59E7 File Offset: 0x000C3BE7
		public Sprite OrganisationLogo { get; protected set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000C59F0 File Offset: 0x000C3BF0
		public bool IsTutorial
		{
			get
			{
				return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial";
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000C5A14 File Offset: 0x000C3C14
		public string SaveFolderName
		{
			get
			{
				return "Game";
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000C5A14 File Offset: 0x000C3C14
		public string SaveFileName
		{
			get
			{
				return "Game";
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000C5A1B File Offset: 0x000C3C1B
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x000C5A23 File Offset: 0x000C3C23
		// (set) Token: 0x06002F60 RID: 12128 RVA: 0x000C5A2B File Offset: 0x000C3C2B
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Logo.png"
		};

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002F61 RID: 12129 RVA: 0x000C5A34 File Offset: 0x000C3C34
		// (set) Token: 0x06002F62 RID: 12130 RVA: 0x000C5A3C File Offset: 0x000C3C3C
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000C5A45 File Offset: 0x000C3C45
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x000C5A4D File Offset: 0x000C3C4D
		public bool HasChanged { get; set; }

		// Token: 0x06002F65 RID: 12133 RVA: 0x000C5A56 File Offset: 0x000C3C56
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.DevUtilities.GameManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000C5A6A File Offset: 0x000C3C6A
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000C5A72 File Offset: 0x000C3C72
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!connection.IsHost)
			{
				this.SetGameData(connection, new GameData(this.OrganisationName, this.seed, this.Settings));
			}
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000C5AA1 File Offset: 0x000C3CA1
		[TargetRpc]
		public void SetGameData(NetworkConnection conn, GameData data)
		{
			this.RpcWriter___Target_SetGameData_3076874643(conn, data);
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000C5AB1 File Offset: 0x000C3CB1
		public virtual string GetSaveString()
		{
			return new GameData(this.OrganisationName, this.seed, this.Settings).GetJson(true);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000C5AD0 File Offset: 0x000C3CD0
		public void Load(GameData data, string path)
		{
			this.OrganisationName = data.OrganisationName;
			this.seed = data.Seed;
			this.Settings = data.Settings;
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
			this.HasChanged = true;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000C5B10 File Offset: 0x000C3D10
		[Button]
		public void EndTutorial(bool natural)
		{
			if (!this.IsTutorial)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.StoredSaveInfo != null && (!Singleton<Lobby>.Instance.IsInLobby || Singleton<Lobby>.Instance.IsHost))
			{
				Singleton<SaveManager>.Instance.DisablePlayTutorial(Singleton<LoadManager>.Instance.StoredSaveInfo);
				Singleton<LoadManager>.Instance.StoredSaveInfo.MetaData.PlayTutorial = false;
			}
			if (natural)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.COMPLETE_PROLOGUE);
			}
			Singleton<LoadManager>.Instance.ExitToMenu(Singleton<LoadManager>.Instance.StoredSaveInfo, null, true);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000C5BF8 File Offset: 0x000C3DF8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterTargetRpc(0U, new ClientRpcDelegate(this.RpcReader___Target_SetGameData_3076874643));
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000C5C28 File Offset: 0x000C3E28
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000C5C41 File Offset: 0x000C3E41
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000C5C50 File Offset: 0x000C3E50
		private void RpcWriter___Target_SetGameData_3076874643(NetworkConnection conn, GameData data)
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
			writer.Write___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(0U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000C5D05 File Offset: 0x000C3F05
		public void RpcLogic___SetGameData_3076874643(NetworkConnection conn, GameData data)
		{
			this.OrganisationName = data.OrganisationName;
			this.seed = data.Seed;
			this.Settings = data.Settings;
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000C5D40 File Offset: 0x000C3F40
		private void RpcReader___Target_SetGameData_3076874643(PooledReader PooledReader0, Channel channel)
		{
			GameData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGameData_3076874643(base.LocalConnection, data);
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000C5D77 File Offset: 0x000C3F77
		protected virtual void dll()
		{
			base.Awake();
			CrashReportHandler.logBufferSize = 50U;
			this.InitializeSaveable();
		}

		// Token: 0x040021CD RID: 8653
		public const bool IS_DEMO = false;

		// Token: 0x040021CE RID: 8654
		public static bool IS_BETA;

		// Token: 0x040021CF RID: 8655
		[SerializeField]
		private int seed;

		// Token: 0x040021D0 RID: 8656
		public string OrganisationName = "Organisation";

		// Token: 0x040021D2 RID: 8658
		public GameSettings Settings = new GameSettings();

		// Token: 0x040021D3 RID: 8659
		public Transform SpawnPoint;

		// Token: 0x040021D4 RID: 8660
		public Transform NoHomeRespawnPoint;

		// Token: 0x040021D5 RID: 8661
		public Transform Temp;

		// Token: 0x040021D6 RID: 8662
		public UnityEvent onSettingsLoaded = new UnityEvent();

		// Token: 0x040021D7 RID: 8663
		private GameDataLoader loader = new GameDataLoader();

		// Token: 0x040021DB RID: 8667
		private bool dll_Excuted;

		// Token: 0x040021DC RID: 8668
		private bool dll_Excuted;
	}
}
