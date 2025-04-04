using System;
using System.Collections.Generic;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000B5B RID: 2907
	public class FI_OptionList : FI_Base
	{
		// Token: 0x06004D3C RID: 19772 RVA: 0x00146234 File Offset: 0x00144434
		public virtual void Initialize(OptionListFeature _feature, List<FI_OptionList.Option> _options)
		{
			base.Initialize(_feature);
			this.specificFeature = _feature;
			this.options.AddRange(_options);
			this.selectionIndex = this.specificFeature.SyncAccessor_ownedOptionIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, this.buttonContainer).GetComponent<Button>();
				component.GetComponent<Image>().color = this.options[i].optionColor;
				component.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = this.options[i].optionLabel;
				int index = i;
				component.onClick.AddListener(delegate()
				{
					this.Select(index);
				});
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.buttonContainer);
			this.bar.anchoredPosition = new Vector2(this.bar.anchoredPosition.x, this.buttonContainer.GetChild(this.buttonContainer.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y - 35f);
			this.UpdateSelection();
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0014636D File Offset: 0x0014456D
		public override void Close()
		{
			this.Select(this.specificFeature.SyncAccessor_ownedOptionIndex);
			base.Close();
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x00146388 File Offset: 0x00144588
		public void BuyButtonClicked()
		{
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.options[this.selectionIndex].optionPrice)
			{
				return;
			}
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + ": " + this.feature.featureName, -this.options[this.selectionIndex].optionPrice, 1f, string.Empty);
			if (this.onSelectionPurchased != null)
			{
				this.onSelectionPurchased.Invoke(this.selectionIndex);
			}
			this.UpdateSelection();
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x00146426 File Offset: 0x00144626
		public void Select(int index)
		{
			this.selectionIndex = Mathf.Clamp(index, 0, this.options.Count - 1);
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged.Invoke(this.selectionIndex);
			}
			this.UpdateSelection();
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x00146464 File Offset: 0x00144664
		private void UpdateSelection()
		{
			for (int i = 0; i < this.buttonContainer.childCount; i++)
			{
				this.buttonContainer.GetChild(i).Find("SelectionIndicator").gameObject.SetActive(false);
				this.buttonContainer.GetChild(i).Find("OwnedIndicator").gameObject.SetActive(false);
			}
			this.buttonContainer.GetChild(this.selectionIndex).Find("SelectionIndicator").gameObject.SetActive(true);
			this.buttonContainer.GetChild(this.specificFeature.SyncAccessor_ownedOptionIndex).Find("OwnedIndicator").gameObject.SetActive(true);
			if (this.selectionIndex != this.specificFeature.SyncAccessor_ownedOptionIndex)
			{
				this.buyButtonText.text = "Buy (" + MoneyManager.FormatAmount(this.options[this.selectionIndex].optionPrice, false, false) + ")";
				this.buyButton.gameObject.SetActive(true);
				return;
			}
			this.buyButton.gameObject.SetActive(false);
		}

		// Token: 0x04003A82 RID: 14978
		[Header("References")]
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x04003A83 RID: 14979
		[SerializeField]
		protected Button buyButton;

		// Token: 0x04003A84 RID: 14980
		[SerializeField]
		protected TextMeshProUGUI buyButtonText;

		// Token: 0x04003A85 RID: 14981
		[SerializeField]
		protected RectTransform bar;

		// Token: 0x04003A86 RID: 14982
		[Header("Prefab")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x04003A87 RID: 14983
		public UnityEvent<int> onSelectionChanged;

		// Token: 0x04003A88 RID: 14984
		public UnityEvent<int> onSelectionPurchased;

		// Token: 0x04003A89 RID: 14985
		private List<FI_OptionList.Option> options = new List<FI_OptionList.Option>();

		// Token: 0x04003A8A RID: 14986
		public OptionListFeature specificFeature;

		// Token: 0x04003A8B RID: 14987
		private int selectionIndex;

		// Token: 0x02000B5C RID: 2908
		public class Option
		{
			// Token: 0x06004D42 RID: 19778 RVA: 0x0014659A File Offset: 0x0014479A
			public Option(string _optionLabel, Color _optionColor, float _optionPrice)
			{
				this.optionLabel = _optionLabel;
				this.optionColor = _optionColor;
				this.optionPrice = _optionPrice;
			}

			// Token: 0x04003A8C RID: 14988
			public string optionLabel;

			// Token: 0x04003A8D RID: 14989
			public Color optionColor;

			// Token: 0x04003A8E RID: 14990
			public float optionPrice;
		}
	}
}
