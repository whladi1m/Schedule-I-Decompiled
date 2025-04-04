using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009D9 RID: 2521
	public class HUD : Singleton<HUD>
	{
		// Token: 0x06004413 RID: 17427 RVA: 0x0011D267 File Offset: 0x0011B467
		protected override void Awake()
		{
			base.Awake();
			this.eventSystem = EventSystem.current;
			this.managementSlotContainer.gameObject.SetActive(true);
			this.HideTopScreenText();
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0011D291 File Offset: 0x0011B491
		public void SetCrosshairVisible(bool vis)
		{
			this.crosshair.gameObject.SetActive(vis);
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0011D2A4 File Offset: 0x0011B4A4
		public void SetBlackOverlayVisible(bool vis, float fadeTime)
		{
			if (this.blackOverlayFade != null)
			{
				base.StopCoroutine(this.blackOverlayFade);
			}
			this.blackOverlayFade = base.StartCoroutine(this.FadeBlackOverlay(vis, fadeTime));
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0011D2CE File Offset: 0x0011B4CE
		protected virtual void Update()
		{
			this.RefreshFPS();
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0011D2D8 File Offset: 0x0011B4D8
		private void FixedUpdate()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.SleepPrompt.gameObject.SetActive(NetworkSingleton<TimeManager>.Instance.CurrentTime == 400);
			if (NetworkSingleton<CurfewManager>.InstanceExists)
			{
				if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
				{
					this.CurfewPrompt.text = "Police curfew in effect until 5AM";
					this.CurfewPrompt.color = new Color32(byte.MaxValue, 108, 88, 60);
					this.CurfewPrompt.gameObject.SetActive(true);
				}
				else if (NetworkSingleton<CurfewManager>.Instance.IsEnabled && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(2030, 500))
				{
					this.CurfewPrompt.text = "Police curfew starting soon";
					this.CurfewPrompt.color = new Color32(byte.MaxValue, 182, 88, 60);
					this.CurfewPrompt.gameObject.SetActive(true);
				}
				else
				{
					this.CurfewPrompt.gameObject.SetActive(false);
				}
			}
			this.UpdateQuestEntryTitle();
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0011D3E8 File Offset: 0x0011B5E8
		private void UpdateQuestEntryTitle()
		{
			int num = 0;
			for (int i = 0; i < this.QuestEntryContainer.childCount; i++)
			{
				if (this.QuestEntryContainer.GetChild(i).gameObject.activeSelf)
				{
					num++;
				}
			}
			this.QuestEntryTitle.enabled = (num > 1);
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0011D438 File Offset: 0x0011B638
		private void RefreshFPS()
		{
			this._previousFPS.Add(1f / Time.unscaledDeltaTime);
			if (this._previousFPS.Count > this.SampleSize)
			{
				this._previousFPS.RemoveAt(0);
			}
			this.fpsLabel.text = Mathf.Floor(this.GetAverageFPS()).ToString() + " FPS";
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0011D4A4 File Offset: 0x0011B6A4
		private float GetAverageFPS()
		{
			float num = 0f;
			for (int i = 0; i < this._previousFPS.Count; i++)
			{
				num += this._previousFPS[i];
			}
			return num / (float)this._previousFPS.Count;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0011D4EA File Offset: 0x0011B6EA
		protected virtual void LateUpdate()
		{
			if (!this.radialIndicatorSetThisFrame)
			{
				this.radialIndicator.enabled = false;
			}
			this.radialIndicatorSetThisFrame = false;
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x0011D507 File Offset: 0x0011B707
		protected IEnumerator FadeBlackOverlay(bool visible, float fadeTime)
		{
			if (visible)
			{
				this.blackOverlay.enabled = true;
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Blackout");
			}
			float startAlpha = this.blackOverlay.color.a;
			float endAlpha = 1f;
			if (!visible)
			{
				endAlpha = 0f;
			}
			for (float i = 0f; i < fadeTime; i += Time.unscaledDeltaTime)
			{
				this.blackOverlay.color = new Color(this.blackOverlay.color.r, this.blackOverlay.color.g, this.blackOverlay.color.b, Mathf.Lerp(startAlpha, endAlpha, i / fadeTime));
				yield return new WaitForEndOfFrame();
			}
			this.blackOverlay.color = new Color(this.blackOverlay.color.r, this.blackOverlay.color.g, this.blackOverlay.color.b, endAlpha);
			this.blackOverlayFade = null;
			if (!visible)
			{
				this.blackOverlay.enabled = false;
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Blackout");
			}
			yield break;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0011D524 File Offset: 0x0011B724
		public void ShowRadialIndicator(float fill)
		{
			this.radialIndicatorSetThisFrame = true;
			this.radialIndicator.fillAmount = fill;
			this.radialIndicator.enabled = true;
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0011D548 File Offset: 0x0011B748
		public void ShowTopScreenText(string t)
		{
			this.topScreenText.text = t;
			this.topScreenText_Background.sizeDelta = new Vector2(this.topScreenText.preferredWidth + 30f, this.topScreenText_Background.sizeDelta.y);
			this.topScreenText_Background.gameObject.SetActive(true);
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0011D5A3 File Offset: 0x0011B7A3
		public void HideTopScreenText()
		{
			this.topScreenText_Background.gameObject.SetActive(false);
		}

		// Token: 0x040031DB RID: 12763
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040031DC RID: 12764
		public RectTransform canvasRect;

		// Token: 0x040031DD RID: 12765
		public Image crosshair;

		// Token: 0x040031DE RID: 12766
		[SerializeField]
		protected Image blackOverlay;

		// Token: 0x040031DF RID: 12767
		[SerializeField]
		protected Image radialIndicator;

		// Token: 0x040031E0 RID: 12768
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x040031E1 RID: 12769
		[SerializeField]
		protected TextMeshProUGUI topScreenText;

		// Token: 0x040031E2 RID: 12770
		[SerializeField]
		protected RectTransform topScreenText_Background;

		// Token: 0x040031E3 RID: 12771
		public Text fpsLabel;

		// Token: 0x040031E4 RID: 12772
		public RectTransform cashSlotContainer;

		// Token: 0x040031E5 RID: 12773
		public RectTransform cashSlotUI;

		// Token: 0x040031E6 RID: 12774
		public RectTransform onlineBalanceContainer;

		// Token: 0x040031E7 RID: 12775
		public RectTransform onlineBalanceSlotUI;

		// Token: 0x040031E8 RID: 12776
		public RectTransform managementSlotContainer;

		// Token: 0x040031E9 RID: 12777
		public ItemSlotUI managementSlotUI;

		// Token: 0x040031EA RID: 12778
		public RectTransform HotbarContainer;

		// Token: 0x040031EB RID: 12779
		public RectTransform SlotContainer;

		// Token: 0x040031EC RID: 12780
		public ItemSlotUI discardSlot;

		// Token: 0x040031ED RID: 12781
		public Image discardSlotFill;

		// Token: 0x040031EE RID: 12782
		public TextMeshProUGUI selectedItemLabel;

		// Token: 0x040031EF RID: 12783
		public RectTransform QuestEntryContainer;

		// Token: 0x040031F0 RID: 12784
		public TextMeshProUGUI QuestEntryTitle;

		// Token: 0x040031F1 RID: 12785
		public CrimeStatusUI CrimeStatusUI;

		// Token: 0x040031F2 RID: 12786
		public BalanceDisplay OnlineBalanceDisplay;

		// Token: 0x040031F3 RID: 12787
		public BalanceDisplay SafeBalanceDisplay;

		// Token: 0x040031F4 RID: 12788
		public CrosshairText CrosshairText;

		// Token: 0x040031F5 RID: 12789
		public RectTransform UnreadMessagesPrompt;

		// Token: 0x040031F6 RID: 12790
		public TextMeshProUGUI SleepPrompt;

		// Token: 0x040031F7 RID: 12791
		public TextMeshProUGUI CurfewPrompt;

		// Token: 0x040031F8 RID: 12792
		[Header("Settings")]
		public Gradient RedGreenGradient;

		// Token: 0x040031F9 RID: 12793
		private int SampleSize = 60;

		// Token: 0x040031FA RID: 12794
		private List<float> _previousFPS = new List<float>();

		// Token: 0x040031FB RID: 12795
		private EventSystem eventSystem;

		// Token: 0x040031FC RID: 12796
		private Coroutine blackOverlayFade;

		// Token: 0x040031FD RID: 12797
		private bool radialIndicatorSetThisFrame;
	}
}
