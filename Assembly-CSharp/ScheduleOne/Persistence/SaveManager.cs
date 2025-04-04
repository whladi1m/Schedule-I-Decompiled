using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037C RID: 892
	public class SaveManager : PersistentSingleton<SaveManager>
	{
		// Token: 0x0600142F RID: 5167 RVA: 0x0005A1CC File Offset: 0x000583CC
		public static void ReportSaveError()
		{
			SaveManager.SaveError = true;
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0005A1D4 File Offset: 0x000583D4
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x0005A1DC File Offset: 0x000583DC
		public bool AccessPermissionIssueDetected { get; protected set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0005A1E5 File Offset: 0x000583E5
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x0005A1ED File Offset: 0x000583ED
		public bool IsSaving { get; protected set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0005A1F6 File Offset: 0x000583F6
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x0005A1FE File Offset: 0x000583FE
		public float SecondsSinceLastSave { get; protected set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0005A207 File Offset: 0x00058407
		// (set) Token: 0x06001437 RID: 5175 RVA: 0x0005A20F File Offset: 0x0005840F
		public string PlayersSavePath { get; protected set; } = string.Empty;

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0005A218 File Offset: 0x00058418
		// (set) Token: 0x06001439 RID: 5177 RVA: 0x0005A220 File Offset: 0x00058420
		public string IndividualSavesContainerPath { get; protected set; } = string.Empty;

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0005A229 File Offset: 0x00058429
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x0005A231 File Offset: 0x00058431
		public string SaveName { get; protected set; } = "DevSave";

		// Token: 0x0600143C RID: 5180 RVA: 0x0005A23C File Offset: 0x0005843C
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<SaveManager>.Instance == null || Singleton<SaveManager>.Instance != this)
			{
				return;
			}
			this.PlayersSavePath = Path.Combine(Application.persistentDataPath, "Saves");
			if (!Directory.Exists(this.PlayersSavePath))
			{
				Directory.CreateDirectory(this.PlayersSavePath);
			}
			if (Directory.GetDirectories(this.PlayersSavePath).Length == 0)
			{
				string path = Path.Combine(this.PlayersSavePath, "TempPlayer");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
			string[] directories = Directory.GetDirectories(this.PlayersSavePath);
			if (directories.Length > 1)
			{
				for (int i = 0; i < directories.Length; i++)
				{
					if (!directories[i].Contains("TempPlayer"))
					{
						this.IndividualSavesContainerPath = directories[i];
						return;
					}
				}
				return;
			}
			this.IndividualSavesContainerPath = directories[0];
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0005A309 File Offset: 0x00058509
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			this.CheckSaveFolderInitialized();
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0005A334 File Offset: 0x00058534
		public void CheckSaveFolderInitialized()
		{
			if (this.saveFolderInitialized)
			{
				return;
			}
			this.saveFolderInitialized = true;
			if (SteamManager.Initialized)
			{
				string path = SteamUser.GetSteamID().ToString();
				string text = Path.Combine(this.PlayersSavePath, path);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				this.IndividualSavesContainerPath = text;
				Console.Log("Initialized individual save folder path: " + this.IndividualSavesContainerPath, null);
			}
			else
			{
				Console.LogError("Steamworks not intialized in time for SaveManager! Using save container path: " + this.IndividualSavesContainerPath, null);
			}
			if (SaveManager.HasWritePermissionOnDir(this.IndividualSavesContainerPath))
			{
				this.AccessPermissionIssueDetected = false;
				Console.Log("Successfully verified write permission on save folder: " + this.IndividualSavesContainerPath, null);
				if (this.WriteIssueDisplay != null)
				{
					this.WriteIssueDisplay.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				this.AccessPermissionIssueDetected = true;
				Console.LogError("No write permission on save folder: " + this.IndividualSavesContainerPath, null);
				if (this.WriteIssueDisplay != null)
				{
					this.WriteIssueDisplay.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005A444 File Offset: 0x00058644
		public static bool HasWritePermissionOnDir(string path)
		{
			bool result = false;
			string path2 = Path.Combine(path, "WriteTest.txt");
			if (Directory.Exists(path))
			{
				try
				{
					File.WriteAllText(path2, "If you're reading this, it means Schedule I can write save files properly - Yay!");
					if (File.Exists(path2))
					{
						result = true;
					}
				}
				catch (Exception)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0005A494 File Offset: 0x00058694
		private void Update()
		{
			if (Singleton<LoadManager>.Instance.IsGameLoaded && Singleton<LoadManager>.Instance.LoadedGameFolderPath != string.Empty && Input.GetKeyDown(KeyCode.F5) && (Application.isEditor || Debug.isDebugBuild))
			{
				this.Save();
			}
			if (Singleton<LoadManager>.Instance.IsGameLoaded)
			{
				this.SecondsSinceLastSave += Time.unscaledDeltaTime;
				return;
			}
			this.SecondsSinceLastSave = 0f;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0005A50D File Offset: 0x0005870D
		public void DelayedSave()
		{
			base.Invoke("Save", 1f);
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0005A51F File Offset: 0x0005871F
		public void Save()
		{
			this.Save(Singleton<LoadManager>.Instance.LoadedGameFolderPath);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0005A534 File Offset: 0x00058734
		public void Save(string saveFolderPath)
		{
			SaveManager.<>c__DisplayClass51_0 CS$<>8__locals1 = new SaveManager.<>c__DisplayClass51_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.saveFolderPath = saveFolderPath;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.LoadedGameFolderPath == string.Empty)
			{
				Console.LogWarning("No game loaded to save", null);
				return;
			}
			if (this.IsSaving)
			{
				Console.LogWarning("Save called while saving is already in progress", null);
				return;
			}
			if (NetworkSingleton<GameManager>.Instance.IsTutorial && !Application.isEditor)
			{
				Console.LogWarning("Can't save during tutorial", null);
				return;
			}
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				CS$<>8__locals1.saveFolderPath = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "DevSave");
			}
			Console.Log("Saving game to " + CS$<>8__locals1.saveFolderPath, null);
			this.IsSaving = true;
			if (this.onSaveStart != null)
			{
				this.onSaveStart.Invoke();
			}
			this.CompletedSaveables.Clear();
			this.ApprovedBaseLevelPaths.Clear();
			SaveManager.SaveError = false;
			base.StartCoroutine(CS$<>8__locals1.<Save>g__SaveRoutine|0());
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005A634 File Offset: 0x00058834
		private void ClearBaseLevelOutdatedSaves(string saveFolderPath)
		{
			string[] array = null;
			string[] array2 = null;
			try
			{
				array = Directory.GetFiles(saveFolderPath);
			}
			catch (Exception ex)
			{
				string str = "Failed to get files in folder: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + saveFolderPath + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return;
			}
			try
			{
				array2 = Directory.GetDirectories(saveFolderPath);
			}
			catch (Exception ex3)
			{
				string str3 = "Failed to get folders in folder: ";
				string str4 = "\nException: ";
				Exception ex4 = ex3;
				Console.LogError(str3 + saveFolderPath + str4 + ((ex4 != null) ? ex4.ToString() : null), null);
				return;
			}
			if (array == null || array2 == null)
			{
				Console.LogError("Failed to get files or folders in folder: " + saveFolderPath, null);
				return;
			}
			foreach (string text in array)
			{
				FileInfo fileInfo = new FileInfo(text);
				if (!this.ApprovedBaseLevelPaths.Contains(fileInfo.Name))
				{
					try
					{
						Debug.Log("Deleting file: " + text);
						File.Delete(text);
					}
					catch (Exception ex5)
					{
						string str5 = "Failed to delete file: ";
						string str6 = text;
						string str7 = "\nException: ";
						Exception ex6 = ex5;
						Console.LogError(str5 + str6 + str7 + ((ex6 != null) ? ex6.ToString() : null), null);
					}
				}
			}
			foreach (string text2 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text2);
				if (!this.ApprovedBaseLevelPaths.Contains(directoryInfo.Name))
				{
					try
					{
						Debug.Log("Deleting folder: " + text2);
						Directory.Delete(text2, true);
					}
					catch (Exception ex7)
					{
						string str8 = "Failed to delete folder: ";
						string str9 = text2;
						string str10 = "\nException: ";
						Exception ex8 = ex7;
						Console.LogError(str8 + str9 + str10 + ((ex8 != null) ? ex8.ToString() : null), null);
					}
				}
			}
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0005A7F8 File Offset: 0x000589F8
		public void CompleteSaveable(ISaveable saveable)
		{
			if (this.CompletedSaveables.Contains(saveable))
			{
				Console.LogWarning("Saveable already completed", null);
				return;
			}
			this.CompletedSaveables.Add(saveable);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0005A820 File Offset: 0x00058A20
		public void ClearCompletedSaveable(ISaveable saveable)
		{
			this.CompletedSaveables.Remove(saveable);
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0005A82F File Offset: 0x00058A2F
		public void RegisterSaveable(ISaveable saveable)
		{
			if (this.Saveables.Contains(saveable))
			{
				return;
			}
			this.Saveables.Add(saveable);
			if (saveable is IBaseSaveable)
			{
				this.BaseSaveables.Add(saveable as IBaseSaveable);
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005A865 File Offset: 0x00058A65
		public void QueueSaveRequest(SaveRequest request)
		{
			this.QueuedSaveRequests.Add(request);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005A873 File Offset: 0x00058A73
		public void DequeueSaveRequest(SaveRequest request)
		{
			this.QueuedSaveRequests.Remove(request);
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0005A882 File Offset: 0x00058A82
		public static string StripExtensions(string filePath)
		{
			return filePath.Replace(".json", string.Empty);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0005A894 File Offset: 0x00058A94
		public static string MakeFileSafe(string fileName)
		{
			foreach (char oldChar in Path.GetInvalidFileNameChars())
			{
				fileName = fileName.Replace(oldChar, '-');
			}
			return fileName;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		public static float GetVersionNumber(string version)
		{
			version.ToLower().Contains("alternate");
			version = version.Replace(".", string.Empty);
			version = version.Replace("f", ".");
			version = Regex.Replace(version, "[^\\d.]", string.Empty);
			version = version.TrimStart('0');
			float result;
			if (!float.TryParse(version, out result))
			{
				Console.LogError("Failed to parse version number: " + version, null);
				return 0f;
			}
			return result;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005A948 File Offset: 0x00058B48
		private void Clean()
		{
			this.Saveables.Clear();
			this.BaseSaveables.Clear();
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005A960 File Offset: 0x00058B60
		public void DisablePlayTutorial(SaveInfo info)
		{
			string path = Path.Combine(info.SavePath, "Metadata.json");
			if (File.Exists(path))
			{
				string json = string.Empty;
				try
				{
					json = File.ReadAllText(path);
				}
				catch (Exception ex)
				{
					Console.LogError("Error reading save metadata: " + ex.Message, null);
					return;
				}
				MetaData metaData = JsonUtility.FromJson<MetaData>(json);
				metaData.PlayTutorial = false;
				try
				{
					File.WriteAllText(path, metaData.GetJson(true));
					Console.Log("Successfully disabled tutorial in metadata file", null);
				}
				catch (Exception ex2)
				{
					string str = "Failed to modify metadata file. Exception: ";
					Exception ex3 = ex2;
					Console.LogError(str + ((ex3 != null) ? ex3.ToString() : null), null);
				}
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005AA18 File Offset: 0x00058C18
		public static string SanitizeFileName(string fileName)
		{
			foreach (char oldChar in Path.GetInvalidFileNameChars())
			{
				fileName = fileName.Replace(oldChar, '_');
			}
			return fileName;
		}

		// Token: 0x0400130F RID: 4879
		public const string MAIN_SCENE_NAME = "Main";

		// Token: 0x04001310 RID: 4880
		public const string MENU_SCENE_NAME = "Menu";

		// Token: 0x04001311 RID: 4881
		public const string TUTORIAL_SCENE_NAME = "Tutorial";

		// Token: 0x04001312 RID: 4882
		public const int SAVES_PER_FRAME = 10;

		// Token: 0x04001313 RID: 4883
		public const string SAVE_FILE_EXTENSION = ".json";

		// Token: 0x04001314 RID: 4884
		public const int SAVE_SLOT_COUNT = 5;

		// Token: 0x04001315 RID: 4885
		public const string SAVE_GAME_PREFIX = "SaveGame_";

		// Token: 0x04001316 RID: 4886
		public const bool DEBUG = false;

		// Token: 0x04001317 RID: 4887
		public const bool PRETTY_PRINT = true;

		// Token: 0x04001318 RID: 4888
		public static bool SaveError;

		// Token: 0x0400131F RID: 4895
		public List<ISaveable> Saveables = new List<ISaveable>();

		// Token: 0x04001320 RID: 4896
		public List<IBaseSaveable> BaseSaveables = new List<IBaseSaveable>();

		// Token: 0x04001321 RID: 4897
		[HideInInspector]
		public List<string> ApprovedBaseLevelPaths = new List<string>();

		// Token: 0x04001322 RID: 4898
		protected List<ISaveable> CompletedSaveables = new List<ISaveable>();

		// Token: 0x04001323 RID: 4899
		protected List<SaveRequest> QueuedSaveRequests = new List<SaveRequest>();

		// Token: 0x04001324 RID: 4900
		[Header("References")]
		public RectTransform WriteIssueDisplay;

		// Token: 0x04001325 RID: 4901
		[Header("Events")]
		public UnityEvent onSaveStart;

		// Token: 0x04001326 RID: 4902
		public UnityEvent onSaveComplete;

		// Token: 0x04001327 RID: 4903
		private bool saveFolderInitialized;
	}
}
