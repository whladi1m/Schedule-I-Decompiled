using System;
using System.Text.RegularExpressions;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A8D RID: 2701
	public class CallInterface : Singleton<CallInterface>
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060048B6 RID: 18614 RVA: 0x00130528 File Offset: 0x0012E728
		// (set) Token: 0x060048B7 RID: 18615 RVA: 0x00130530 File Offset: 0x0012E730
		public PhoneCallData ActiveCallData { get; private set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060048B8 RID: 18616 RVA: 0x00130539 File Offset: 0x0012E739
		// (set) Token: 0x060048B9 RID: 18617 RVA: 0x00130541 File Offset: 0x0012E741
		public bool IsOpen { get; protected set; }

		// Token: 0x060048BA RID: 18618 RVA: 0x0013054C File Offset: 0x0012E74C
		protected override void Awake()
		{
			base.Awake();
			this.highlight1Hex = ColorUtility.ToHtmlStringRGB(this.Highlight1Color);
			this.ContinuePrompt.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x001305B0 File Offset: 0x0012E7B0
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) || GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (this.rolloutRoutine != null)
				{
					this.skipRollout = true;
					return;
				}
				this.Continue();
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x001305EA File Offset: 0x0012E7EA
		private void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (exit.exitType == ExitType.Escape)
			{
				exit.used = true;
				this.Close();
			}
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x00130614 File Offset: 0x0012E814
		public void StartCall(PhoneCallData data, CallerID caller, int startStage = 0)
		{
			if (this.IsOpen)
			{
				Debug.LogWarning("CallInterface: There is already a call in progress; existing call will be forced complete");
				for (int i = this.currentCallStage; i < this.ActiveCallData.Stages.Length; i++)
				{
					if (i > this.currentCallStage)
					{
						this.ActiveCallData.Stages[i].OnStageStart();
					}
					this.ActiveCallData.Stages[i].OnStageEnd();
				}
				this.ActiveCallData.Completed();
			}
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.ActiveCallData = data;
			this.IsOpen = true;
			this.ProfilePicture.sprite = caller.ProfilePicture;
			this.MainText.text = string.Empty;
			this.NameLabel.text = caller.Name;
			this.currentCallStage = startStage;
			this.SetIsVisible(true);
			this.ShowStage(0, 0.25f);
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x0013073C File Offset: 0x0012E93C
		public void EndCall()
		{
			if (!this.IsOpen)
			{
				Debug.LogWarning("CallInterface: Attempted to end a call while no call was in progress.");
				return;
			}
			if (this.ActiveCallData != null)
			{
				this.ActiveCallData.Completed();
			}
			if (this.CallCompleted != null)
			{
				this.CallCompleted(this.ActiveCallData);
			}
			this.Close();
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x00130794 File Offset: 0x0012E994
		private void Close()
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			if (this.rolloutRoutine != null)
			{
				base.StopCoroutine(this.rolloutRoutine);
				this.rolloutRoutine = null;
			}
			this.ActiveCallData = null;
			this.IsOpen = false;
			this.SetIsVisible(false);
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x00130848 File Offset: 0x0012EA48
		public void Continue()
		{
			if (this.currentCallStage != -1)
			{
				this.ActiveCallData.Stages[this.currentCallStage].OnStageEnd();
			}
			if (this.currentCallStage == this.ActiveCallData.Stages.Length - 1)
			{
				this.EndCall();
				return;
			}
			this.ShowStage(this.currentCallStage + 1, 0f);
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x001308A8 File Offset: 0x0012EAA8
		private void ShowStage(int stageIndex, float initialDelay = 0f)
		{
			CallInterface.<>c__DisplayClass32_0 CS$<>8__locals1 = new CallInterface.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initialDelay = initialDelay;
			CS$<>8__locals1.stageIndex = stageIndex;
			this.currentCallStage = CS$<>8__locals1.stageIndex;
			this.ActiveCallData.Stages[CS$<>8__locals1.stageIndex].OnStageStart();
			this.rolloutRoutine = base.StartCoroutine(CS$<>8__locals1.<ShowStage>g__Routine|0());
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00130908 File Offset: 0x0012EB08
		private string ProcessText(string text)
		{
			string pattern = "<Input_([a-zA-Z0-9]+)>";
			MatchEvaluator evaluator = delegate(Match match)
			{
				GameInput.ButtonCode code;
				if (Enum.TryParse<GameInput.ButtonCode>(match.Groups[1].Value, out code))
				{
					string text2;
					string controlPath;
					Singleton<GameInput>.Instance.GetAction(code).GetBindingDisplayString(0, out text2, out controlPath, (InputBinding.DisplayStringOptions)0);
					string displayNameForControlPath = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
					return string.Concat(new string[]
					{
						"<color=#",
						this.highlight1Hex,
						">",
						displayNameForControlPath,
						"</color>"
					});
				}
				return match.Value;
			};
			return Regex.Replace(text, pattern, evaluator).Replace("<h1>", "<color=#" + this.highlight1Hex + ">").Replace("</h>", "</color>");
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00130960 File Offset: 0x0012EB60
		private string GetVisibleText(int charactersShown, string fullText)
		{
			bool flag = false;
			string text = fullText.Substring(0, charactersShown);
			char[] array = text.ToCharArray();
			if ((array[charactersShown - 1] != '<' && !flag) || array[charactersShown - 1] == '>')
			{
			}
			return text;
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0013099C File Offset: 0x0012EB9C
		private void SetIsVisible(bool visible)
		{
			if (this.slideRoutine != null)
			{
				base.StopCoroutine(this.slideRoutine);
			}
			if (visible)
			{
				this.CanvasGroup.alpha = 0f;
				this.Canvas.enabled = true;
				this.Container.gameObject.SetActive(true);
				this.OpenAnim.Play();
				return;
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x04003613 RID: 13843
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003616 RID: 13846
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003617 RID: 13847
		public RectTransform Container;

		// Token: 0x04003618 RID: 13848
		public Image ProfilePicture;

		// Token: 0x04003619 RID: 13849
		public TextMeshProUGUI NameLabel;

		// Token: 0x0400361A RID: 13850
		public TextMeshProUGUI MainText;

		// Token: 0x0400361B RID: 13851
		public RectTransform ContinuePrompt;

		// Token: 0x0400361C RID: 13852
		public Animation OpenAnim;

		// Token: 0x0400361D RID: 13853
		public AudioSourceController TypewriterEffectSound;

		// Token: 0x0400361E RID: 13854
		public CanvasGroup CanvasGroup;

		// Token: 0x0400361F RID: 13855
		[Header("Settings")]
		public Color Highlight1Color;

		// Token: 0x04003620 RID: 13856
		private int currentCallStage = -1;

		// Token: 0x04003621 RID: 13857
		private Coroutine slideRoutine;

		// Token: 0x04003622 RID: 13858
		private bool skipRollout;

		// Token: 0x04003623 RID: 13859
		private Coroutine rolloutRoutine;

		// Token: 0x04003624 RID: 13860
		private string highlight1Hex;

		// Token: 0x04003625 RID: 13861
		public Action<PhoneCallData> CallCompleted;
	}
}
