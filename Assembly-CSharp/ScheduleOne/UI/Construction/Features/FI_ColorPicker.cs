using System;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000B58 RID: 2904
	public class FI_ColorPicker : FI_Base
	{
		// Token: 0x06004D31 RID: 19761 RVA: 0x00145E60 File Offset: 0x00144060
		public override void Initialize(Feature _feature)
		{
			base.Initialize(_feature);
			this.specificFeature = (this.feature as ColorFeature);
			this.selectionIndex = this.specificFeature.SyncAccessor_ownedColorIndex;
			for (int i = 0; i < this.specificFeature.colors.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.colorButtonPrefab, this.colorButtonContainer).GetComponent<Button>();
				component.GetComponent<Image>().color = this.specificFeature.colors[i].color;
				int index = i;
				component.onClick.AddListener(delegate()
				{
					this.Select(index);
				});
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.colorButtonContainer);
			this.bar.anchoredPosition = new Vector2(this.bar.anchoredPosition.x, this.colorButtonContainer.GetChild(this.colorButtonContainer.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y - 35f);
			this.UpdateSelection();
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x00145F70 File Offset: 0x00144170
		public override void Close()
		{
			this.Select(this.specificFeature.SyncAccessor_ownedColorIndex);
			base.Close();
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x00145F8C File Offset: 0x0014418C
		public void BuyButtonClicked()
		{
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.specificFeature.colors[this.selectionIndex].price)
			{
				return;
			}
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + ": " + this.feature.featureName, -this.specificFeature.colors[this.selectionIndex].price, 1f, string.Empty);
			if (this.onSelectionPurchased != null)
			{
				this.onSelectionPurchased.Invoke(this.specificFeature.colors[this.selectionIndex]);
			}
			this.UpdateSelection();
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x00146044 File Offset: 0x00144244
		public void Select(int index)
		{
			this.selectionIndex = Mathf.Clamp(index, 0, this.specificFeature.colors.Count - 1);
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged.Invoke(this.specificFeature.colors[this.selectionIndex]);
			}
			this.UpdateSelection();
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x001460A0 File Offset: 0x001442A0
		private void UpdateSelection()
		{
			this.colorLabel.text = this.specificFeature.colors[this.selectionIndex].colorName;
			for (int i = 0; i < this.colorButtonContainer.childCount; i++)
			{
				this.colorButtonContainer.GetChild(i).Find("SelectionIndicator").gameObject.SetActive(false);
				this.colorButtonContainer.GetChild(i).Find("OwnedIndicator").gameObject.SetActive(false);
			}
			this.colorButtonContainer.GetChild(this.selectionIndex).Find("SelectionIndicator").gameObject.SetActive(true);
			this.colorButtonContainer.GetChild(this.specificFeature.SyncAccessor_ownedColorIndex).Find("OwnedIndicator").gameObject.SetActive(true);
			if (this.selectionIndex != this.specificFeature.SyncAccessor_ownedColorIndex)
			{
				this.buyButtonText.text = "Buy (" + MoneyManager.FormatAmount(this.specificFeature.colors[this.selectionIndex].price, false, false) + ")";
				this.buyButton.gameObject.SetActive(true);
				return;
			}
			this.buyButton.gameObject.SetActive(false);
		}

		// Token: 0x04003A74 RID: 14964
		[Header("References")]
		[SerializeField]
		protected RectTransform colorButtonContainer;

		// Token: 0x04003A75 RID: 14965
		[SerializeField]
		protected Button buyButton;

		// Token: 0x04003A76 RID: 14966
		[SerializeField]
		protected TextMeshProUGUI buyButtonText;

		// Token: 0x04003A77 RID: 14967
		[SerializeField]
		protected TextMeshProUGUI colorLabel;

		// Token: 0x04003A78 RID: 14968
		[SerializeField]
		protected RectTransform bar;

		// Token: 0x04003A79 RID: 14969
		[Header("Prefab")]
		[SerializeField]
		protected GameObject colorButtonPrefab;

		// Token: 0x04003A7A RID: 14970
		public UnityEvent<ColorFeature.NamedColor> onSelectionChanged;

		// Token: 0x04003A7B RID: 14971
		public UnityEvent<ColorFeature.NamedColor> onSelectionPurchased;

		// Token: 0x04003A7C RID: 14972
		private ColorFeature specificFeature;

		// Token: 0x04003A7D RID: 14973
		private int selectionIndex;
	}
}
