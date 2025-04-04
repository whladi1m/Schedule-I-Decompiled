using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B2A RID: 2858
	public class CharacterCustomizationCategory : MonoBehaviour
	{
		// Token: 0x06004C14 RID: 19476 RVA: 0x00140AA8 File Offset: 0x0013ECA8
		private void Awake()
		{
			this.ui = base.GetComponentInParent<CharacterCustomizationUI>();
			this.options = base.GetComponentsInChildren<CharacterCustomizationOption>(true);
			this.TitleText.text = this.CategoryName;
			this.BackButton.onClick.AddListener(new UnityAction(this.Back));
			for (int i = 0; i < this.options.Length; i++)
			{
				CharacterCustomizationOption option = this.options[i];
				this.options[i].onSelect.AddListener(delegate()
				{
					this.OptionSelected(option);
				});
				this.options[i].onDeselect.AddListener(delegate()
				{
					this.OptionDeselected(option);
				});
				this.options[i].onPurchase.AddListener(delegate()
				{
					this.OptionPurchased(option);
				});
			}
			for (int j = 0; j < this.options.Length; j++)
			{
				for (int k = j + 1; k < this.options.Length; k++)
				{
					if (this.options[k].Price < this.options[j].Price)
					{
						Transform transform = this.options[j].transform;
						this.options[j].transform.SetSiblingIndex(k);
						this.options[k].transform.SetSiblingIndex(j);
					}
				}
			}
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x00140C00 File Offset: 0x0013EE00
		public void Open()
		{
			bool flag = false;
			for (int i = 0; i < this.options.Length; i++)
			{
				if (this.ui.IsOptionCurrentlyApplied(this.options[i]))
				{
					flag = true;
					this.options[i].SetPurchased(true);
				}
				else
				{
					this.options[i].SetPurchased(false);
					this.options[i].SetSelected(false);
				}
			}
			if (!flag && this.options.Length != 0)
			{
				this.options[0].SetPurchased(true);
			}
			this.ScrollRect.verticalScrollbar.value = 1f;
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x00140CA8 File Offset: 0x0013EEA8
		public void Back()
		{
			this.ui.SetActiveCategory(null);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].ParentCategoryClosed();
			}
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x00140CF4 File Offset: 0x0013EEF4
		private void OptionSelected(CharacterCustomizationOption option)
		{
			this.ui.OptionSelected(option);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].SiblingOptionSelected(option);
			}
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x00140D2E File Offset: 0x0013EF2E
		private void OptionDeselected(CharacterCustomizationOption option)
		{
			this.ui.OptionDeselected(option);
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00140D3C File Offset: 0x0013EF3C
		private void OptionPurchased(CharacterCustomizationOption option)
		{
			this.ui.OptionPurchased(option);
			for (int i = 0; i < this.options.Length; i++)
			{
				this.options[i].SiblingOptionPurchased(option);
			}
		}

		// Token: 0x04003978 RID: 14712
		public string CategoryName;

		// Token: 0x04003979 RID: 14713
		[Header("References")]
		public TextMeshProUGUI TitleText;

		// Token: 0x0400397A RID: 14714
		public Button BackButton;

		// Token: 0x0400397B RID: 14715
		public ScrollRect ScrollRect;

		// Token: 0x0400397C RID: 14716
		private CharacterCustomizationUI ui;

		// Token: 0x0400397D RID: 14717
		private CharacterCustomizationOption[] options;

		// Token: 0x0400397E RID: 14718
		public UnityEvent onOpen;

		// Token: 0x0400397F RID: 14719
		public UnityEvent onClose;
	}
}
