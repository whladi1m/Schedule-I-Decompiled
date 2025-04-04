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
	// Token: 0x02000A05 RID: 2565
	public class HospitalBillScreen : Singleton<HospitalBillScreen>
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x001223F6 File Offset: 0x001205F6
		// (set) Token: 0x06004540 RID: 17728 RVA: 0x001223FE File Offset: 0x001205FE
		public bool isOpen { get; protected set; }

		// Token: 0x06004541 RID: 17729 RVA: 0x00122408 File Offset: 0x00120608
		protected override void Awake()
		{
			base.Awake();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.CanvasGroup.alpha = 0f;
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 20);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x00122482 File Offset: 0x00120682
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x001224AC File Offset: 0x001206AC
		private void PlayerSpawned()
		{
			this.PatientNameLabel.text = Player.Local.PlayerName;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x001224C4 File Offset: 0x001206C4
		public void Open()
		{
			this.isOpen = true;
			this.arrested = Player.Local.IsArrested;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.CanvasGroup.alpha = 1f;
			this.CanvasGroup.interactable = true;
			this.BillNumberLabel.text = UnityEngine.Random.Range(10000000, 100000000).ToString();
			float amount = Mathf.Min(250f, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			this.PaidAmountLabel.text = MoneyManager.FormatAmount(amount, true, false);
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Player.Deactivate(false);
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x00122590 File Offset: 0x00120790
		public void Close()
		{
			if (!this.CanvasGroup.interactable || !this.isOpen)
			{
				return;
			}
			this.CanvasGroup.interactable = false;
			float num = Mathf.Min(250f, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-num, true, false);
			if (this.arrested)
			{
				this.CanvasGroup.alpha = 0f;
				this.Canvas.enabled = false;
				this.Container.gameObject.SetActive(false);
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				this.isOpen = false;
				Singleton<ArrestNoticeScreen>.Instance.Open();
				return;
			}
			base.StartCoroutine(this.<Close>g__CloseRoutine|16_0());
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0012264E File Offset: 0x0012084E
		[CompilerGenerated]
		private IEnumerator <Close>g__CloseRoutine|16_0()
		{
			float lerpTime = 0.3f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CanvasGroup.alpha = Mathf.Lerp(1f, 0f, i / lerpTime);
				Singleton<PostProcessingManager>.Instance.SetBlur(this.CanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			this.CanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Player.Activate();
			this.isOpen = false;
			yield break;
		}

		// Token: 0x0400331F RID: 13087
		public const float BILL_COST = 250f;

		// Token: 0x04003321 RID: 13089
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003322 RID: 13090
		public RectTransform Container;

		// Token: 0x04003323 RID: 13091
		public CanvasGroup CanvasGroup;

		// Token: 0x04003324 RID: 13092
		public TextMeshProUGUI PatientNameLabel;

		// Token: 0x04003325 RID: 13093
		public TextMeshProUGUI BillNumberLabel;

		// Token: 0x04003326 RID: 13094
		public TextMeshProUGUI PaidAmountLabel;

		// Token: 0x04003327 RID: 13095
		private bool arrested;
	}
}
