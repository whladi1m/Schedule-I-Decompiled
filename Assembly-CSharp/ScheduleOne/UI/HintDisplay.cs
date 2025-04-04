using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Input;
using ScheduleOne.UI.Phone;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScheduleOne.UI
{
	// Token: 0x020009D3 RID: 2515
	public class HintDisplay : Singleton<HintDisplay>
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x0011CC86 File Offset: 0x0011AE86
		// (set) Token: 0x060043F2 RID: 17394 RVA: 0x0011CC8E File Offset: 0x0011AE8E
		public bool IsOpen { get; protected set; }

		// Token: 0x060043F3 RID: 17395 RVA: 0x0011CC97 File Offset: 0x0011AE97
		protected override void Start()
		{
			base.Start();
			this.Group.alpha = 0f;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0011CCC0 File Offset: 0x0011AEC0
		public void Update()
		{
			if (!this.IsOpen)
			{
				if (this.hintQueue.Count > 0 && this.Group.alpha == 0f)
				{
					this.ShowHint(this.hintQueue[0].Text, this.hintQueue[0].Duration);
					this.hintQueue.RemoveAt(0);
				}
				return;
			}
			this.timeSinceOpened += Time.deltaTime;
			if (Singleton<CallInterface>.Instance.IsOpen)
			{
				this.Hide();
			}
			this.DismissPrompt.SetLabel((this.hintQueue.Count > 0) ? "Next" : "Dismiss");
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) && !GameInput.IsTyping && this.timeSinceOpened > 0.1f)
			{
				this.Hide();
			}
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0011CD96 File Offset: 0x0011AF96
		public void ShowHint_10s(string text)
		{
			this.ShowHint(text, 10f);
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0011CDA4 File Offset: 0x0011AFA4
		public void ShowHint_20s(string text)
		{
			this.ShowHint(text, 20f);
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0011CDB2 File Offset: 0x0011AFB2
		public void ShowHint(string text)
		{
			this.ShowHint(text, 0f);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0011CDC0 File Offset: 0x0011AFC0
		public void ShowHint(string text, float autoCloseTime = 0f)
		{
			text = this.ProcessText(text);
			Console.Log("Showing hint: " + text, null);
			this.timeSinceOpened = 0f;
			this.SetAlpha(1f);
			this.FlashAnim.Play();
			this.Label.text = text;
			this.Label.ForceMeshUpdate(false, false);
			this.Container.sizeDelta = new Vector2(this.Label.renderedWidth + this.Padding.x, this.Label.renderedHeight + this.Padding.y);
			this.Container.anchoredPosition = new Vector2(-this.Container.sizeDelta.x / 2f - this.Offset.x, -this.Container.sizeDelta.y / 2f + this.Offset.y);
			if (this.autoCloseRoutine != null)
			{
				base.StopCoroutine(this.autoCloseRoutine);
			}
			if (autoCloseTime > 0f)
			{
				this.autoCloseRoutine = base.StartCoroutine(this.<ShowHint>g__AutoClose|22_0(autoCloseTime));
			}
			this.IsOpen = true;
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0011CEEC File Offset: 0x0011B0EC
		public void Hide()
		{
			this.SetAlpha(0f);
			this.IsOpen = false;
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0011CF00 File Offset: 0x0011B100
		private void SetAlpha(float alpha)
		{
			HintDisplay.<>c__DisplayClass24_0 CS$<>8__locals1 = new HintDisplay.<>c__DisplayClass24_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.alpha = alpha;
			if (CS$<>8__locals1.alpha > 0f)
			{
				this.Container.gameObject.SetActive(true);
			}
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(CS$<>8__locals1.<SetAlpha>g__Routine|0());
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0011CF65 File Offset: 0x0011B165
		public void QueueHint_10s(string message)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, 10f));
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0011CF7D File Offset: 0x0011B17D
		public void QueueHint_20s(string message)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, 20f));
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0011CF95 File Offset: 0x0011B195
		public void QueueHint(string message, float time)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, time));
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0011CFAC File Offset: 0x0011B1AC
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
					return "<color=#88CBFF>" + displayNameForControlPath + "</color>";
				}
				return match.Value;
			};
			return Regex.Replace(text, pattern, evaluator).Replace("<h1>", "<color=#88CBFF>").Replace("<h2>", "<color=#F86266>").Replace("<h3>", "<color=#46CB4F>").Replace("</h>", "</color>");
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0011D036 File Offset: 0x0011B236
		[CompilerGenerated]
		private IEnumerator <ShowHint>g__AutoClose|22_0(float time)
		{
			yield return new WaitForSeconds(time);
			this.Hide();
			this.autoCloseRoutine = null;
			yield break;
		}

		// Token: 0x040031BF RID: 12735
		public const float FadeTime = 0.3f;

		// Token: 0x040031C1 RID: 12737
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040031C2 RID: 12738
		public TextMeshProUGUI Label;

		// Token: 0x040031C3 RID: 12739
		public CanvasGroup Group;

		// Token: 0x040031C4 RID: 12740
		public InputPrompt DismissPrompt;

		// Token: 0x040031C5 RID: 12741
		public Animation FlashAnim;

		// Token: 0x040031C6 RID: 12742
		[Header("Settings")]
		public Vector2 Padding;

		// Token: 0x040031C7 RID: 12743
		public Vector2 Offset;

		// Token: 0x040031C8 RID: 12744
		private Coroutine autoCloseRoutine;

		// Token: 0x040031C9 RID: 12745
		private Coroutine fadeRoutine;

		// Token: 0x040031CA RID: 12746
		private List<HintDisplay.Hint> hintQueue = new List<HintDisplay.Hint>();

		// Token: 0x040031CB RID: 12747
		private float timeSinceOpened;

		// Token: 0x020009D4 RID: 2516
		private class Hint
		{
			// Token: 0x06004401 RID: 17409 RVA: 0x0011D04C File Offset: 0x0011B24C
			public Hint(string text, float duration)
			{
				this.Text = text;
				this.Duration = duration;
			}

			// Token: 0x040031CC RID: 12748
			public string Text;

			// Token: 0x040031CD RID: 12749
			public float Duration;
		}
	}
}
