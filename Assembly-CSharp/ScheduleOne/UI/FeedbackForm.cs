using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using AeLa.EasyFeedback;
using AeLa.EasyFeedback.FormElements;
using AeLa.EasyFeedback.Utility;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009C3 RID: 2499
	public class FeedbackForm : FeedbackForm
	{
		// Token: 0x0600438F RID: 17295 RVA: 0x0011B66C File Offset: 0x0011986C
		public override void Awake()
		{
			base.Awake();
			this.ScreenshotToggle.SetIsOnWithoutNotify(this.IncludeScreenshot);
			this.ScreenshotToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnScreenshotToggle));
			this.SaveFileToggle.SetIsOnWithoutNotify(this.IncludeSaveFile);
			this.SaveFileToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnSaveFileToggle));
			this.OnSubmissionSucceeded.AddListener(new UnityAction(this.Clear));
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0011B6F0 File Offset: 0x001198F0
		private void Update()
		{
			this.Cog.localEulerAngles += new Vector3(0f, 0f, -180f * Time.unscaledDeltaTime);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0011B722 File Offset: 0x00119922
		public void PrepScreenshot()
		{
			this.CurrentReport = new Report();
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0011B72F File Offset: 0x0011992F
		private void OnScreenshotToggle(bool value)
		{
			this.IncludeScreenshot = value;
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0011B738 File Offset: 0x00119938
		private void OnSaveFileToggle(bool value)
		{
			this.IncludeSaveFile = value;
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0011B741 File Offset: 0x00119941
		public void SetFormData(string title)
		{
			if (this.CurrentReport == null)
			{
				this.CurrentReport = new Report();
			}
			this.CurrentReport.Title = title;
			base.GetComponentInChildren<ReportTitle>().GetComponent<TMP_InputField>().SetTextWithoutNotify(title);
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0011B774 File Offset: 0x00119974
		public void SetCategory(string categoryName)
		{
			for (int i = 0; i < this.Config.Board.CategoryNames.Length; i++)
			{
				if (this.Config.Board.CategoryNames[i].Contains(categoryName))
				{
					this.CategoryDropdown.SetValueWithoutNotify(i + 1);
					return;
				}
			}
			Console.LogWarning("Category not found: " + categoryName, null);
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0011B7D8 File Offset: 0x001199D8
		public override void Submit()
		{
			if (this.IncludeScreenshot)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
				this.CanvasGroup.alpha = 0f;
				this.ssCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.ScreenshotAndOpenForm());
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<Submit>g__Wait|15_0());
			}
			if (File.Exists(Application.persistentDataPath + "/Player-prev.log"))
			{
				try
				{
					byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/Player-prev.log");
					this.CurrentReport.AttachFile("Player-prev.txt", data);
				}
				catch (Exception ex)
				{
					Console.LogError("Failed to attach Player-prev.txt: " + ex.Message, null);
				}
			}
			if (this.IncludeSaveFile)
			{
				string loadedGameFolderPath = Singleton<LoadManager>.Instance.LoadedGameFolderPath;
				string text = loadedGameFolderPath + ".zip";
				try
				{
					if (File.Exists(text))
					{
						Console.Log("Deleting prior zip file: " + text, null);
						File.Delete(text);
					}
					ZipFile.CreateFromDirectory(loadedGameFolderPath, text, System.IO.Compression.CompressionLevel.Optimal, true);
					byte[] data2 = File.ReadAllBytes(text);
					this.CurrentReport.AttachFile("SaveGame.zip", data2);
				}
				catch (Exception ex2)
				{
					Console.LogError("Failed to attach save file: " + ex2.Message, null);
				}
				finally
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
			}
			if (Player.Local != null)
			{
				Report currentReport = this.CurrentReport;
				currentReport.Title = currentReport.Title + " (" + Player.Local.PlayerName + ")";
			}
			this.CurrentReport.AddSection("Game Info", 2);
			string text2 = "Singleplayer";
			if (Singleton<Lobby>.InstanceExists && Singleton<Lobby>.Instance.IsInLobby)
			{
				text2 = "Multiplayer";
				if (Singleton<Lobby>.Instance.IsHost)
				{
					text2 += " (Host)";
				}
				else
				{
					text2 += " (Client)";
				}
			}
			this.CurrentReport["Game Info"].AppendLine("Network Mode: " + text2);
			this.CurrentReport["Game Info"].AppendLine("Player Count: " + Player.PlayerList.Count.ToString());
			this.CurrentReport["Game Info"].AppendLine("Beta Branch: " + GameManager.IS_BETA.ToString());
			this.CurrentReport["Game Info"].AppendLine("Is Demo: " + false.ToString());
			this.CurrentReport["Game Info"].AppendLine("Load History: " + string.Join(", ", LoadManager.LoadHistory));
			Singleton<CoroutineService>.Instance.StartCoroutine(base.SubmitAsync());
			base.Submit();
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0011BAC4 File Offset: 0x00119CC4
		protected override string GetTextToAppendToTitle()
		{
			string text = base.GetTextToAppendToTitle();
			text = text + " (" + Application.version + ")";
			if (Player.Local != null)
			{
				text = text + " (" + Player.Local.PlayerName + ")";
			}
			return text;
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0011BB17 File Offset: 0x00119D17
		private void Clear()
		{
			this.SummaryField.SetTextWithoutNotify(string.Empty);
			this.DescriptionField.SetTextWithoutNotify(string.Empty);
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0011BB39 File Offset: 0x00119D39
		private IEnumerator ScreenshotAndOpenForm()
		{
			if (this.IncludeScreenshot)
			{
				yield return ScreenshotUtil.CaptureScreenshot(this.ScreenshotCaptureMode, this.ResizeLargeScreenshots, delegate(byte[] ss)
				{
					this.CurrentReport.AttachFile("screenshot.png", ss);
				}, delegate(string err)
				{
					this.OnSubmissionError.Invoke(err);
				});
			}
			base.EnableForm();
			this.Form.gameObject.SetActive(true);
			this.OnFormOpened.Invoke();
			this.ssCoroutine = null;
			yield break;
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0011BB50 File Offset: 0x00119D50
		[CompilerGenerated]
		private IEnumerator <Submit>g__Wait|15_0()
		{
			yield return new WaitForEndOfFrame();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			this.CanvasGroup.alpha = 1f;
			yield break;
		}

		// Token: 0x04003168 RID: 12648
		private Coroutine ssCoroutine;

		// Token: 0x04003169 RID: 12649
		public CanvasGroup CanvasGroup;

		// Token: 0x0400316A RID: 12650
		public Toggle ScreenshotToggle;

		// Token: 0x0400316B RID: 12651
		public Toggle SaveFileToggle;

		// Token: 0x0400316C RID: 12652
		public TMP_InputField SummaryField;

		// Token: 0x0400316D RID: 12653
		public TMP_InputField DescriptionField;

		// Token: 0x0400316E RID: 12654
		public RectTransform Cog;

		// Token: 0x0400316F RID: 12655
		public TMP_Dropdown CategoryDropdown;
	}
}
