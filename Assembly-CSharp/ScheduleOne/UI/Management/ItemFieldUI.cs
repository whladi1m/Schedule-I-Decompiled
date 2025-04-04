using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000ADB RID: 2779
	public class ItemFieldUI : MonoBehaviour
	{
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004A58 RID: 19032 RVA: 0x00137A90 File Offset: 0x00135C90
		// (set) Token: 0x06004A59 RID: 19033 RVA: 0x00137A98 File Offset: 0x00135C98
		public List<ItemField> Fields { get; protected set; } = new List<ItemField>();

		// Token: 0x06004A5A RID: 19034 RVA: 0x00137AA4 File Offset: 0x00135CA4
		public void Bind(List<ItemField> field)
		{
			this.Fields = new List<ItemField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onItemChanged.AddListener(new UnityAction<ItemDefinition>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedItem);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x00137B10 File Offset: 0x00135D10
		private void Refresh(ItemDefinition newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (!this.AreFieldsUniform())
			{
				this.MultipleSelected.SetActive(true);
				this.SelectionLabel.text = "Mixed";
				return;
			}
			if (newVal != null)
			{
				this.IconImg.sprite = newVal.Icon;
				this.SelectionLabel.text = newVal.Name;
				this.IconImg.gameObject.SetActive(true);
				return;
			}
			this.NoneSelected.SetActive(true);
			this.SelectionLabel.text = (this.ShowNoneAsAny ? "Any" : "None");
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x00137BE0 File Offset: 0x00135DE0
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedItem != this.Fields[i + 1].SelectedItem)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x00137C34 File Offset: 0x00135E34
		public void Clicked()
		{
			List<ItemSelector.Option> list = new List<ItemSelector.Option>();
			ItemSelector.Option selectedOption = null;
			bool flag = this.AreFieldsUniform();
			if (this.Fields[0].CanSelectNone)
			{
				list.Add(new ItemSelector.Option(this.ShowNoneAsAny ? "Any" : "None", null));
				if (flag && this.Fields[0].SelectedItem == null)
				{
					selectedOption = list[0];
				}
			}
			foreach (ItemDefinition itemDefinition in this.Fields[0].Options)
			{
				ItemSelector.Option option = new ItemSelector.Option(itemDefinition.Name, itemDefinition);
				list.Add(option);
				if (flag && this.Fields[0].SelectedItem == option.Item)
				{
					selectedOption = option;
				}
			}
			Singleton<ManagementInterface>.Instance.ItemSelectorScreen.Initialize(this.FieldLabel.text, list, selectedOption, new Action<ItemSelector.Option>(this.OptionSelected));
			Singleton<ManagementInterface>.Instance.ItemSelectorScreen.Open();
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x00137D68 File Offset: 0x00135F68
		private void OptionSelected(ItemSelector.Option option)
		{
			foreach (ItemField itemField in this.Fields)
			{
				itemField.SetItem(option.Item, true);
			}
		}

		// Token: 0x040037F0 RID: 14320
		public bool ShowNoneAsAny;

		// Token: 0x040037F1 RID: 14321
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040037F2 RID: 14322
		public Image IconImg;

		// Token: 0x040037F3 RID: 14323
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x040037F4 RID: 14324
		public GameObject NoneSelected;

		// Token: 0x040037F5 RID: 14325
		public GameObject MultipleSelected;
	}
}
