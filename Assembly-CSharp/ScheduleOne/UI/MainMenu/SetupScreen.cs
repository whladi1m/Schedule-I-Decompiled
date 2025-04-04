using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.ExtendedComponents;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B16 RID: 2838
	public class SetupScreen : MainMenuScreen
	{
		// Token: 0x06004BAA RID: 19370 RVA: 0x0013D4C4 File Offset: 0x0013B6C4
		protected virtual void Start()
		{
			this.InputField.onSubmit.AddListener(delegate(string <p0>)
			{
				this.StartGame();
			});
			this.SkipIntroContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x0013D4F3 File Offset: 0x0013B6F3
		public void Initialize(int index)
		{
			this.slotIndex = index;
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x0013D4FC File Offset: 0x0013B6FC
		private void Update()
		{
			if (base.IsOpen)
			{
				this.StartButton.interactable = (this.IsInputValid() && Singleton<Lobby>.Instance.IsHost);
				this.NotHostWarning.gameObject.SetActive(!Singleton<Lobby>.Instance.IsHost);
			}
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x0013D550 File Offset: 0x0013B750
		public void StartGame()
		{
			if (!this.IsInputValid())
			{
				return;
			}
			if (!Singleton<Lobby>.Instance.IsHost)
			{
				Console.LogWarning("Only the host can start the game.", null);
				return;
			}
			string text = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "SaveGame_" + (this.slotIndex + 1).ToString());
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			this.ClearFolderContents(text);
			this.CopyDefaultSaveToFolder(text);
			string path = Path.Combine(text, "Game.json");
			int seed = UnityEngine.Random.Range(0, int.MaxValue);
			string json = new GameData(this.InputField.text, seed, new GameSettings()).GetJson(true);
			File.WriteAllText(path, json);
			bool isOn = this.SkipIntroToggle.isOn;
			string path2 = Path.Combine(text, "Metadata.json");
			string json2 = new MetaData(new DateTimeData(DateTime.Now), new DateTimeData(DateTime.Now), Application.version, Application.version, !isOn).GetJson(true);
			File.WriteAllText(path2, json2);
			Singleton<LoadManager>.Instance.RefreshSaveInfo();
			Singleton<LoadManager>.Instance.StartGame(LoadManager.SaveGames[this.slotIndex], false);
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0013D66E File Offset: 0x0013B86E
		private bool IsInputValid()
		{
			return !string.IsNullOrEmpty(this.InputField.text);
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0013D684 File Offset: 0x0013B884
		private void ClearFolderContents(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(true);
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x0013D6D4 File Offset: 0x0013B8D4
		private void CopyDefaultSaveToFolder(string folderPath)
		{
			SetupScreen.CopyFilesRecursively(Path.Combine(Application.streamingAssetsPath, "DefaultSave"), folderPath);
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x0013D6F8 File Offset: 0x0013B8F8
		private static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			string[] array = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < array.Length; i++)
			{
				Directory.CreateDirectory(array[i].Replace(sourcePath, targetPath));
			}
			foreach (string text in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				if (!text.EndsWith(".meta"))
				{
					File.Copy(text, text.Replace(sourcePath, targetPath), true);
				}
			}
		}

		// Token: 0x040038EB RID: 14571
		public const string DEFAULT_SAVE_PATH = "DefaultSave";

		// Token: 0x040038EC RID: 14572
		[Header("References")]
		public GameInputField InputField;

		// Token: 0x040038ED RID: 14573
		public Button StartButton;

		// Token: 0x040038EE RID: 14574
		public RectTransform SkipIntroContainer;

		// Token: 0x040038EF RID: 14575
		public Toggle SkipIntroToggle;

		// Token: 0x040038F0 RID: 14576
		public RectTransform NotHostWarning;

		// Token: 0x040038F1 RID: 14577
		private int slotIndex;
	}
}
