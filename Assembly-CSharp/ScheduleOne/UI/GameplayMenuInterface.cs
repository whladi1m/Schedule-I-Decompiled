using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009CD RID: 2509
	public class GameplayMenuInterface : Singleton<GameplayMenuInterface>
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x0011C658 File Offset: 0x0011A858
		protected override void Awake()
		{
			base.Awake();
			this.PhoneButton.onClick.AddListener(new UnityAction(this.PhoneClicked));
			this.CharacterButton.onClick.AddListener(new UnityAction(this.CharacterClicked));
			this.Close();
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0011C6A9 File Offset: 0x0011A8A9
		public void Open()
		{
			this.Canvas.enabled = true;
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0011C6B7 File Offset: 0x0011A8B7
		public void Close()
		{
			this.Canvas.enabled = false;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0011C6C5 File Offset: 0x0011A8C5
		public void PhoneClicked()
		{
			Singleton<GameplayMenu>.Instance.SetScreen(GameplayMenu.EGameplayScreen.Phone);
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0011C6D2 File Offset: 0x0011A8D2
		public void CharacterClicked()
		{
			Singleton<GameplayMenu>.Instance.SetScreen(GameplayMenu.EGameplayScreen.Character);
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0011C6E0 File Offset: 0x0011A8E0
		public void SetSelected(GameplayMenu.EGameplayScreen screen)
		{
			GameplayMenuInterface.<>c__DisplayClass11_0 CS$<>8__locals1 = new GameplayMenuInterface.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pos = Vector2.zero;
			this.PhoneButton.interactable = true;
			this.CharacterButton.interactable = true;
			if (screen == GameplayMenu.EGameplayScreen.Character)
			{
				this.CharacterInterface.Open();
			}
			else
			{
				this.CharacterInterface.Close();
			}
			if (screen != GameplayMenu.EGameplayScreen.Phone)
			{
				if (screen == GameplayMenu.EGameplayScreen.Character)
				{
					CS$<>8__locals1.pos = this.CharacterButton.transform.position;
					this.CharacterButton.interactable = false;
				}
			}
			else
			{
				CS$<>8__locals1.pos = this.PhoneButton.transform.position;
				this.PhoneButton.interactable = false;
			}
			if (this.selectionLerp != null)
			{
				base.StopCoroutine(this.selectionLerp);
			}
			this.selectionLerp = base.StartCoroutine(CS$<>8__locals1.<SetSelected>g__Lerp|0());
		}

		// Token: 0x0400319E RID: 12702
		public Canvas Canvas;

		// Token: 0x0400319F RID: 12703
		public Button PhoneButton;

		// Token: 0x040031A0 RID: 12704
		public Button CharacterButton;

		// Token: 0x040031A1 RID: 12705
		public RectTransform SelectionIndicator;

		// Token: 0x040031A2 RID: 12706
		public CharacterInterface CharacterInterface;

		// Token: 0x040031A3 RID: 12707
		private Coroutine selectionLerp;
	}
}
