using System;
using System.Linq;
using System.Text;
using EasyButtons;
using FishNet.Managing;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using ScheduleOne.UI.MainMenu;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x02000530 RID: 1328
	public class Lobby : PersistentSingleton<Lobby>
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x00085A63 File Offset: 0x00083C63
		public bool IsHost
		{
			get
			{
				return !this.IsInLobby || (this.Players.Length != 0 && this.Players[0] == this.LocalPlayerID);
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x00085A91 File Offset: 0x00083C91
		// (set) Token: 0x06002084 RID: 8324 RVA: 0x00085A99 File Offset: 0x00083C99
		public ulong LobbyID { get; private set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x00085AA2 File Offset: 0x00083CA2
		public CSteamID LobbySteamID
		{
			get
			{
				return new CSteamID(this.LobbyID);
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x00085AAF File Offset: 0x00083CAF
		public bool IsInLobby
		{
			get
			{
				return this.LobbyID > 0UL;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x00085ABB File Offset: 0x00083CBB
		public int PlayerCount
		{
			get
			{
				if (!this.IsInLobby)
				{
					return 1;
				}
				return this.Players.Count((CSteamID p) => p != CSteamID.Nil);
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x00085AF1 File Offset: 0x00083CF1
		// (set) Token: 0x06002089 RID: 8329 RVA: 0x00085AF9 File Offset: 0x00083CF9
		public CSteamID LocalPlayerID { get; private set; } = CSteamID.Nil;

		// Token: 0x0600208A RID: 8330 RVA: 0x00085B02 File Offset: 0x00083D02
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Lobby>.Instance == null || Singleton<Lobby>.Instance != this)
			{
				return;
			}
			bool destroyed = this.Destroyed;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00085B2C File Offset: 0x00083D2C
		protected override void Start()
		{
			base.Start();
			if (Singleton<Lobby>.Instance == null || Singleton<Lobby>.Instance != this)
			{
				return;
			}
			if (this.Destroyed)
			{
				return;
			}
			if (!SteamManager.Initialized)
			{
				Debug.LogError("Steamworks not initialized");
				return;
			}
			this.LocalPlayerID = SteamUser.GetSteamID();
			this.InitializeCallbacks();
			string launchLobby = this.GetLaunchLobby();
			if (launchLobby != null && launchLobby != string.Empty && SteamManager.Initialized)
			{
				try
				{
					SteamMatchmaking.JoinLobby(new CSteamID(ulong.Parse(launchLobby)));
				}
				catch
				{
					Console.LogWarning("There is an issue with launch commands.", null);
				}
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x00085BD8 File Offset: 0x00083DD8
		private void InitializeCallbacks()
		{
			this.LobbyCreatedCallback = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.OnLobbyCreated));
			this.LobbyEnteredCallback = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.OnLobbyEntered));
			this.ChatUpdateCallback = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this.PlayerEnterOrLeave));
			this.GameLobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.LobbyJoinRequested));
			this.LobbyChatMessageCallback = Callback<LobbyChatMsg_t>.Create(new Callback<LobbyChatMsg_t>.DispatchDelegate(this.OnLobbyChatMessage));
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00085C58 File Offset: 0x00083E58
		public void TryOpenInviteInterface()
		{
			if (!this.IsInLobby)
			{
				Console.Log("Not currently in a lobby, creating one...", null);
				this.CreateLobby();
			}
			if (SteamMatchmaking.GetNumLobbyMembers(this.LobbySteamID) >= 4)
			{
				Debug.LogWarning("Lobby already at max capacity!");
				return;
			}
			SteamFriends.ActivateGameOverlayInviteDialog(this.LobbySteamID);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x00085C98 File Offset: 0x00083E98
		public void LeaveLobby()
		{
			if (this.IsInLobby)
			{
				SteamMatchmaking.LeaveLobby(this.LobbySteamID);
				Console.Log("Leaving lobby: " + this.LobbyID.ToString(), null);
			}
			this.LobbyID = 0UL;
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00085CF7 File Offset: 0x00083EF7
		private void CreateLobby()
		{
			SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00085D04 File Offset: 0x00083F04
		private string GetLaunchLobby()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].ToLower() == "+connect_lobby" && commandLineArgs.Length > i + 1)
				{
					return commandLineArgs[i + 1];
				}
			}
			return string.Empty;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00085D4C File Offset: 0x00083F4C
		private void UpdateLobbyMembers()
		{
			for (int i = 0; i < this.Players.Length; i++)
			{
				this.Players[i] = CSteamID.Nil;
			}
			int num = this.IsInLobby ? SteamMatchmaking.GetNumLobbyMembers(this.LobbySteamID) : 0;
			for (int j = 0; j < num; j++)
			{
				this.Players[j] = SteamMatchmaking.GetLobbyMemberByIndex(this.LobbySteamID, j);
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00085DB8 File Offset: 0x00083FB8
		[Button]
		public void DebugJoin()
		{
			this.JoinAsClient(this.DebugSteamId64);
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00085DC6 File Offset: 0x00083FC6
		public void JoinAsClient(string steamId64)
		{
			Singleton<LoadManager>.Instance.LoadAsClient(steamId64);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00085DD4 File Offset: 0x00083FD4
		public void SendLobbyMessage(string message)
		{
			if (!this.IsInLobby)
			{
				Console.LogWarning("Not in a lobby, cannot send message.", null);
				return;
			}
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			SteamMatchmaking.SendLobbyChatMsg(this.LobbySteamID, bytes, bytes.Length);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00085E11 File Offset: 0x00084011
		public void SetLobbyData(string key, string value)
		{
			if (!this.IsInLobby)
			{
				Console.LogWarning("Not in a lobby, cannot set data.", null);
				return;
			}
			SteamMatchmaking.SetLobbyData(this.LobbySteamID, key, value);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00085E38 File Offset: 0x00084038
		private void OnLobbyCreated(LobbyCreated_t result)
		{
			if (result.m_eResult == EResult.k_EResultOK)
			{
				Console.Log("Lobby created: " + result.m_ulSteamIDLobby.ToString(), null);
			}
			else
			{
				Console.LogWarning("Lobby creation failed: " + result.m_eResult.ToString(), null);
			}
			this.LobbyID = result.m_ulSteamIDLobby;
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "owner", SteamUser.GetSteamID().ToString());
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "version", Application.version);
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "host_loading", "false");
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ready", "false");
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00085F30 File Offset: 0x00084130
		private void OnLobbyEntered(LobbyEnter_t result)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(new CSteamID(result.m_ulSteamIDLobby), "version");
			Console.Log("Lobby version: " + lobbyData + ", client version: " + Application.version, null);
			if (lobbyData != Application.version)
			{
				Console.LogWarning("Lobby version mismatch, cannot join.", null);
				if (Singleton<MainMenuPopup>.InstanceExists)
				{
					Singleton<MainMenuPopup>.Instance.Open("Version Mismatch", "Host version: " + lobbyData + "\nYour version: " + Application.version, true);
				}
				this.LeaveLobby();
				return;
			}
			Console.Log("Entered lobby: " + result.m_ulSteamIDLobby.ToString(), null);
			this.LobbyID = result.m_ulSteamIDLobby;
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
			string lobbyData2 = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "ready");
			bool flag = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "load_tutorial") == "true";
			bool flag2 = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "host_loading") == "true";
			if (lobbyData2 == "true" && !this.IsHost)
			{
				this.JoinAsClient(SteamMatchmaking.GetLobbyOwner(this.LobbySteamID).m_SteamID.ToString());
				return;
			}
			if (flag && !this.IsHost)
			{
				Singleton<LoadManager>.Instance.LoadTutorialAsClient();
				return;
			}
			if (flag2 && !this.IsHost)
			{
				Singleton<LoadManager>.Instance.SetWaitingForHostLoad();
				Singleton<LoadingScreen>.Instance.Open(false);
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000860AC File Offset: 0x000842AC
		private void PlayerEnterOrLeave(LobbyChatUpdate_t result)
		{
			Console.Log("Player join/leave: " + SteamFriends.GetFriendPersonaName(new CSteamID(result.m_ulSteamIDUserChanged)), null);
			this.UpdateLobbyMembers();
			if (result.m_ulSteamIDMakingChange == this.LobbySteamID.m_SteamID && result.m_ulSteamIDUserChanged != this.LocalPlayerID.m_SteamID)
			{
				Console.Log("Lobby owner left, leaving lobby.", null);
				this.LeaveLobby();
			}
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0008612C File Offset: 0x0008432C
		private void LobbyJoinRequested(GameLobbyJoinRequested_t result)
		{
			string str = "Join requested: ";
			CSteamID steamIDLobby = result.m_steamIDLobby;
			Console.Log(str + steamIDLobby.ToString(), null);
			if (this.LobbyID != 0UL)
			{
				this.LeaveLobby();
			}
			SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00086178 File Offset: 0x00084378
		private void OnLobbyChatMessage(LobbyChatMsg_t result)
		{
			byte[] array = new byte[128];
			int cubData = 128;
			CSteamID csteamID;
			EChatEntryType echatEntryType;
			SteamMatchmaking.GetLobbyChatEntry(new CSteamID(this.LobbyID), (int)result.m_iChatID, out csteamID, array, cubData, out echatEntryType);
			string text = Encoding.ASCII.GetString(array);
			text = text.TrimEnd(new char[1]);
			Console.Log("Lobby chat message received: " + text, null);
			if (!this.IsHost && !Singleton<LoadManager>.Instance.IsGameLoaded)
			{
				if (text == "ready")
				{
					this.JoinAsClient(csteamID.m_SteamID.ToString());
					return;
				}
				if (text == "load_tutorial")
				{
					Singleton<LoadManager>.Instance.LoadTutorialAsClient();
					return;
				}
				if (text == "host_loading")
				{
					Singleton<LoadManager>.Instance.SetWaitingForHostLoad();
					Singleton<LoadingScreen>.Instance.Open(false);
				}
			}
		}

		// Token: 0x04001927 RID: 6439
		public const bool ENABLED = true;

		// Token: 0x04001928 RID: 6440
		public const int PLAYER_LIMIT = 4;

		// Token: 0x04001929 RID: 6441
		public const string JOIN_READY = "ready";

		// Token: 0x0400192A RID: 6442
		public const string LOAD_TUTORIAL = "load_tutorial";

		// Token: 0x0400192B RID: 6443
		public const string HOST_LOADING = "host_loading";

		// Token: 0x0400192C RID: 6444
		public NetworkManager NetworkManager;

		// Token: 0x0400192F RID: 6447
		public CSteamID[] Players = new CSteamID[4];

		// Token: 0x04001930 RID: 6448
		public Action onLobbyChange;

		// Token: 0x04001931 RID: 6449
		private Callback<LobbyCreated_t> LobbyCreatedCallback;

		// Token: 0x04001932 RID: 6450
		private Callback<LobbyEnter_t> LobbyEnteredCallback;

		// Token: 0x04001933 RID: 6451
		private Callback<LobbyChatUpdate_t> ChatUpdateCallback;

		// Token: 0x04001934 RID: 6452
		private Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequestedCallback;

		// Token: 0x04001935 RID: 6453
		private Callback<LobbyChatMsg_t> LobbyChatMessageCallback;

		// Token: 0x04001936 RID: 6454
		public string DebugSteamId64 = string.Empty;
	}
}
