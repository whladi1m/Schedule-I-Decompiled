using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using Steamworks;
using Unity.AI.Navigation;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005EC RID: 1516
	public class PlayerManager : Singleton<PlayerManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000A2BC8 File Offset: 0x000A0DC8
		public string SaveFolderName
		{
			get
			{
				return "Players";
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000A2BC8 File Offset: 0x000A0DC8
		public string SaveFileName
		{
			get
			{
				return "Players";
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x000A2BCF File Offset: 0x000A0DCF
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x000A2BD7 File Offset: 0x000A0DD7
		// (set) Token: 0x060027AD RID: 10157 RVA: 0x000A2BDF File Offset: 0x000A0DDF
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x000A2BE8 File Offset: 0x000A0DE8
		// (set) Token: 0x060027AF RID: 10159 RVA: 0x000A2BF0 File Offset: 0x000A0DF0
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x000A2BF9 File Offset: 0x000A0DF9
		// (set) Token: 0x060027B1 RID: 10161 RVA: 0x000A2C01 File Offset: 0x000A0E01
		public bool HasChanged { get; set; }

		// Token: 0x060027B2 RID: 10162 RVA: 0x000A2C0A File Offset: 0x000A0E0A
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000A2C18 File Offset: 0x000A0E18
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			int i;
			int j;
			for (i = 0; i < Player.PlayerList.Count; i = j + 1)
			{
				new SaveRequest(Player.PlayerList[i], containerFolder);
				list.Add(Player.PlayerList[i].SaveFolderName);
				if (!this.loadedPlayerData.Exists((PlayerData PlayerData) => PlayerData.PlayerCode == Player.PlayerList[i].PlayerCode))
				{
					this.loadedPlayerData.Add(Player.PlayerList[i].GetPlayerData());
					this.loadedPlayerDataPaths.Add(Path.Combine(containerFolder, Player.PlayerList[i].SaveFolderName));
					this.loadedPlayerFileNames.Add(Player.PlayerList[i].SaveFolderName);
				}
				j = i;
			}
			string[] collection = Directory.GetDirectories(containerFolder).Select(new Func<string, string>(Path.GetFileName)).ToArray<string>();
			list.AddRange(collection);
			list.AddRange(this.loadedPlayerFileNames);
			return list;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000A2D50 File Offset: 0x000A0F50
		public void SavePlayer(Player player)
		{
			Console.Log("Saving player: " + player.PlayerCode, null);
			string text = Path.Combine(Singleton<LoadManager>.Instance.LoadedGameFolderPath, this.SaveFolderName);
			Singleton<SaveManager>.Instance.ClearCompletedSaveable(player);
			string saveString = player.GetSaveString();
			((ISaveable)player).WriteBaseData(text, saveString);
			player.WriteData(text);
			PlayerData playerData = this.loadedPlayerData.FirstOrDefault((PlayerData PlayerData) => PlayerData.PlayerCode == player.PlayerCode);
			if (playerData != null)
			{
				int index = this.loadedPlayerData.IndexOf(playerData);
				this.loadedPlayerData[index] = player.GetPlayerData();
				return;
			}
			this.loadedPlayerData.Add(player.GetPlayerData());
			this.loadedPlayerDataPaths.Add(Path.Combine(text, player.SaveFolderName));
			this.loadedPlayerFileNames.Add(player.SaveFolderName);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000A2E5C File Offset: 0x000A105C
		public void LoadPlayer(PlayerData data, string containerPath)
		{
			this.loadedPlayerData.Add(data);
			this.loadedPlayerDataPaths.Add(containerPath);
			this.loadedPlayerFileNames.Add(Path.GetFileName(containerPath));
			Player player = Player.PlayerList.FirstOrDefault((Player Player) => Player.PlayerCode == data.PlayerCode);
			if (player == null && InstanceFinder.IsServer)
			{
				string fileName = Path.GetFileName(containerPath);
				if (fileName == "Player_Local" || fileName == "Player_0")
				{
					player = Player.Local;
				}
			}
			if (player != null)
			{
				player.Load(data, containerPath);
			}
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000A2F08 File Offset: 0x000A1108
		public void AllPlayerFilesLoaded()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			string text = string.Empty;
			if (SteamManager.Initialized)
			{
				text = SteamUser.GetSteamID().ToString();
			}
			if (this.loadedPlayerFileNames.Contains("Player_0"))
			{
				int index = this.loadedPlayerFileNames.IndexOf("Player_0");
				Player.Local.Load(this.loadedPlayerData[index], this.loadedPlayerDataPaths[index]);
				return;
			}
			if (text != string.Empty && this.loadedPlayerFileNames.Contains("Player_" + text))
			{
				int index2 = this.loadedPlayerFileNames.IndexOf("Player_" + text);
				Player.Local.Load(this.loadedPlayerData[index2], this.loadedPlayerDataPaths[index2]);
				return;
			}
			if (this.loadedPlayerFileNames.Contains("Player_Local"))
			{
				int index3 = this.loadedPlayerFileNames.IndexOf("Player_Local");
				Player.Local.Load(this.loadedPlayerData[index3], this.loadedPlayerDataPaths[index3]);
				return;
			}
			if (this.loadedPlayerData.Count > 0)
			{
				Player.Local.Load(this.loadedPlayerData[0], this.loadedPlayerDataPaths[0]);
				return;
			}
			Console.LogWarning("Couldn't find any data for host player. This is fine if this is a new game, but not if this is a loaded game.", null);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000A3068 File Offset: 0x000A1268
		public bool TryGetPlayerData(string playerCode, out PlayerData data, out string inventoryString, out string appearanceString, out string clothingString, out VariableData[] variables)
		{
			data = this.loadedPlayerData.FirstOrDefault((PlayerData PlayerData) => PlayerData.PlayerCode == playerCode);
			inventoryString = string.Empty;
			appearanceString = string.Empty;
			clothingString = string.Empty;
			variables = null;
			List<VariableData> list = new List<VariableData>();
			if (data != null)
			{
				string text = this.loadedPlayerDataPaths[this.loadedPlayerData.IndexOf(data)];
				PlayerLoader playerLoader = new PlayerLoader();
				string text2;
				if (playerLoader.TryLoadFile(text, "Inventory", out text2))
				{
					inventoryString = text2;
				}
				else
				{
					Console.LogWarning("Failed to load player inventory under " + text, null);
				}
				string text3;
				if (playerLoader.TryLoadFile(text, "Appearance", out text3))
				{
					appearanceString = text3;
				}
				else
				{
					Console.LogWarning("Failed to load player appearance under " + text, null);
				}
				string text4;
				if (playerLoader.TryLoadFile(text, "Clothing", out text4))
				{
					clothingString = text4;
				}
				else
				{
					Console.LogWarning("Failed to load player clothing under " + text, null);
				}
				string path = Path.Combine(text, "Variables");
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					VariablesLoader variablesLoader = new VariablesLoader();
					for (int i = 0; i < files.Length; i++)
					{
						string json;
						if (variablesLoader.TryLoadFile(files[i], out json, false))
						{
							VariableData item = null;
							try
							{
								item = JsonUtility.FromJson<VariableData>(json);
							}
							catch (Exception ex)
							{
								Debug.LogError("Error loading player variable data: " + ex.Message);
							}
							if (data != null)
							{
								list.Add(item);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				variables = list.ToArray();
			}
			return data != null;
		}

		// Token: 0x04001CCB RID: 7371
		private PlayersLoader loader = new PlayersLoader();

		// Token: 0x04001CCF RID: 7375
		[SerializeField]
		protected List<PlayerData> loadedPlayerData = new List<PlayerData>();

		// Token: 0x04001CD0 RID: 7376
		protected List<string> loadedPlayerDataPaths = new List<string>();

		// Token: 0x04001CD1 RID: 7377
		protected List<string> loadedPlayerFileNames = new List<string>();

		// Token: 0x04001CD2 RID: 7378
		public NavMeshSurface PlayerRecoverySurface;
	}
}
