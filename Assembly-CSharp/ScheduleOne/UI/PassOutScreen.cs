using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A09 RID: 2569
	public class PassOutScreen : Singleton<PassOutScreen>
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x00122982 File Offset: 0x00120B82
		// (set) Token: 0x06004555 RID: 17749 RVA: 0x0012298A File Offset: 0x00120B8A
		public bool isOpen { get; protected set; }

		// Token: 0x06004556 RID: 17750 RVA: 0x00122993 File Offset: 0x00120B93
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Group.alpha = 0f;
			this.Group.interactable = false;
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x001229C3 File Offset: 0x00120BC3
		private void Continue()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<Continue>g__Routine|14_0());
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x001229E2 File Offset: 0x00120BE2
		private void LoadSaveClicked()
		{
			this.Close();
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x001229EC File Offset: 0x00120BEC
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			Singleton<EyelidOverlay>.Instance.Canvas.sortingOrder = 5;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.cashLoss = Mathf.Min(UnityEngine.Random.Range(50f, 500f), NetworkSingleton<MoneyManager>.Instance.cashBalance);
			base.StartCoroutine(this.<Open>g__Routine|16_0());
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x00122A5C File Offset: 0x00120C5C
		public void Close()
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			Singleton<EyelidOverlay>.Instance.Canvas.sortingOrder = -1;
			Singleton<EyelidOverlay>.Instance.AutoUpdate = true;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (!Singleton<ArrestNoticeScreen>.Instance.isOpen)
			{
				Player.Activate();
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00122AC0 File Offset: 0x00120CC0
		[CompilerGenerated]
		private IEnumerator <Continue>g__Routine|14_0()
		{
			float fadeTime = 1f;
			for (float i = 0f; i < fadeTime; i += Time.deltaTime)
			{
				this.Group.alpha = Mathf.Lerp(1f, 0f, i / fadeTime);
				yield return new WaitForEndOfFrame();
			}
			this.MainLabel.gameObject.SetActive(false);
			Player.Local.SendPassOutRecovery();
			Player.Local.Health.RecoverHealth(100f);
			Transform child = this.RecoveryPointsContainer.GetChild(UnityEngine.Random.Range(0, this.RecoveryPointsContainer.childCount));
			PlayerSingleton<PlayerMovement>.Instance.Teleport(child.position);
			Player.Local.transform.forward = child.forward;
			yield return new WaitForSeconds(0.5f);
			bool fadeBlur = false;
			if (Player.Local.IsArrested)
			{
				Singleton<ArrestNoticeScreen>.Instance.RecordCrimes();
				Player.Local.Free();
				Singleton<ArrestNoticeScreen>.Instance.Open();
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				yield return new WaitForSeconds(1f);
			}
			else
			{
				this.ContextLabel.text = "You awaken in a new location, unsure of how you got there.";
				if (this.cashLoss > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.cashLoss, true, false);
					this.ContextLabel.text = this.ContextLabel.text + "\n\n<color=#54E717>" + MoneyManager.FormatAmount(this.cashLoss, false, false) + "</color> is missing from your wallet.";
				}
				this.ContextLabel.gameObject.SetActive(true);
				for (float i = 0f; i < fadeTime; i += Time.deltaTime)
				{
					this.Group.alpha = Mathf.Lerp(0f, 1f, i / fadeTime);
					yield return new WaitForEndOfFrame();
				}
				fadeBlur = true;
				yield return new WaitForSeconds(4f);
				for (float i = 0f; i < fadeTime; i += Time.deltaTime)
				{
					this.Group.alpha = Mathf.Lerp(1f, 0f, i / fadeTime);
					yield return new WaitForEndOfFrame();
				}
				this.Group.alpha = 0f;
			}
			yield return new WaitForSeconds(1f);
			float lerpTime = 2f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<EyelidOverlay>.Instance.SetOpen(Mathf.Lerp(0f, 1f, i / lerpTime));
				if (fadeBlur)
				{
					Singleton<PostProcessingManager>.Instance.SetBlur(1f - i / lerpTime);
				}
				yield return new WaitForEndOfFrame();
			}
			Singleton<EyelidOverlay>.Instance.SetOpen(1f);
			if (fadeBlur)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			}
			this.Close();
			yield break;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00122ACF File Offset: 0x00120CCF
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|16_0()
		{
			this.MainLabel.gameObject.SetActive(true);
			this.ContextLabel.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.5f);
			Singleton<EyelidOverlay>.Instance.AutoUpdate = false;
			float lerpTime = 2f;
			float startOpenness = Singleton<EyelidOverlay>.Instance.CurrentOpen;
			float endOpenness = 0f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<EyelidOverlay>.Instance.SetOpen(Mathf.Lerp(startOpenness, endOpenness, i / lerpTime));
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<EyelidOverlay>.Instance.SetOpen(0f);
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			yield return new WaitForSeconds(0.5f);
			this.Anim.Play();
			this.Canvas.enabled = true;
			yield return new WaitForSeconds(3f);
			this.Continue();
			yield break;
		}

		// Token: 0x04003331 RID: 13105
		public const float CASH_LOSS_MIN = 50f;

		// Token: 0x04003332 RID: 13106
		public const float CASH_LOSS_MAX = 500f;

		// Token: 0x04003333 RID: 13107
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003334 RID: 13108
		public CanvasGroup Group;

		// Token: 0x04003335 RID: 13109
		public Transform RecoveryPointsContainer;

		// Token: 0x04003336 RID: 13110
		public TextMeshProUGUI MainLabel;

		// Token: 0x04003337 RID: 13111
		public TextMeshProUGUI ContextLabel;

		// Token: 0x04003338 RID: 13112
		public Animation Anim;

		// Token: 0x04003339 RID: 13113
		private float cashLoss;
	}
}
