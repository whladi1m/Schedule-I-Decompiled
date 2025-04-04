using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE9 RID: 2793
	public class ItemSelector : ClipboardScreen
	{
		// Token: 0x06004AB6 RID: 19126 RVA: 0x00139624 File Offset: 0x00137824
		public void Initialize(string selectionTitle, List<ItemSelector.Option> _options, ItemSelector.Option _selectedOption = null, Action<ItemSelector.Option> _optionCallback = null)
		{
			this.TitleLabel.text = selectionTitle;
			this.options = new List<ItemSelector.Option>();
			this.options.AddRange(_options);
			this.selectedOption = _selectedOption;
			this.optionCallback = _optionCallback;
			this.DeleteOptions();
			this.CreateOptions(this.options);
			this.HoveredItemLabel.enabled = false;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00139681 File Offset: 0x00137881
		public override void Open()
		{
			base.Open();
			Singleton<ManagementInterface>.Instance.MainScreen.Close();
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x00139698 File Offset: 0x00137898
		public override void Close()
		{
			base.Close();
			this.HoveredItemLabel.enabled = false;
			Singleton<ManagementInterface>.Instance.MainScreen.Open();
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001396BB File Offset: 0x001378BB
		private void ButtonClicked(ItemSelector.Option option)
		{
			if (this.optionCallback != null)
			{
				this.optionCallback(option);
			}
			this.Close();
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x001396D8 File Offset: 0x001378D8
		private void ButtonHovered(ItemSelector.Option option)
		{
			this.HoveredItemLabel.text = option.Title;
			this.HoveredItemLabel.enabled = true;
			this.HoveredItemLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -140f - Mathf.Ceil((float)this.optionButtons.Count / 5f) * this.optionButtons[0].sizeDelta.y);
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x00139750 File Offset: 0x00137950
		private void ButtonHoverEnd(ItemSelector.Option option)
		{
			this.HoveredItemLabel.enabled = false;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x00139760 File Offset: 0x00137960
		private void CreateOptions(List<ItemSelector.Option> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer).GetComponent<Button>();
				if (options[i].Item != null)
				{
					component.transform.Find("None").gameObject.SetActive(false);
					component.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = options[i].Item.Icon;
					component.transform.Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					component.transform.Find("None").gameObject.SetActive(true);
					component.transform.Find("Icon").gameObject.SetActive(false);
				}
				if (options[i] == this.selectedOption)
				{
					component.transform.Find("Outline").gameObject.GetComponent<Image>().color = new Color32(90, 90, 90, byte.MaxValue);
				}
				ItemSelector.Option opt = options[i];
				component.onClick.AddListener(delegate()
				{
					this.ButtonClicked(opt);
				});
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerEnter;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.ButtonHovered(opt);
				});
				component.GetComponent<EventTrigger>().triggers.Add(entry);
				entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerExit;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.ButtonHoverEnd(opt);
				});
				component.GetComponent<EventTrigger>().triggers.Add(entry);
				this.optionButtons.Add(component.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x0013993C File Offset: 0x00137B3C
		private void DeleteOptions()
		{
			for (int i = 0; i < this.optionButtons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.optionButtons[i].gameObject);
			}
			this.optionButtons.Clear();
		}

		// Token: 0x04003836 RID: 14390
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x04003837 RID: 14391
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003838 RID: 14392
		public TextMeshProUGUI HoveredItemLabel;

		// Token: 0x04003839 RID: 14393
		public GameObject OptionPrefab;

		// Token: 0x0400383A RID: 14394
		[Header("Settings")]
		public Sprite EmptyOptionSprite;

		// Token: 0x0400383B RID: 14395
		private Coroutine lerpRoutine;

		// Token: 0x0400383C RID: 14396
		private List<ItemSelector.Option> options = new List<ItemSelector.Option>();

		// Token: 0x0400383D RID: 14397
		private ItemSelector.Option selectedOption;

		// Token: 0x0400383E RID: 14398
		private List<RectTransform> optionButtons = new List<RectTransform>();

		// Token: 0x0400383F RID: 14399
		private Action<ItemSelector.Option> optionCallback;

		// Token: 0x02000AEA RID: 2794
		[Serializable]
		public class Option
		{
			// Token: 0x06004ABF RID: 19135 RVA: 0x0013999E File Offset: 0x00137B9E
			public Option(string title, ItemDefinition item)
			{
				this.Title = title;
				this.Item = item;
			}

			// Token: 0x04003840 RID: 14400
			public string Title;

			// Token: 0x04003841 RID: 14401
			public ItemDefinition Item;
		}
	}
}
