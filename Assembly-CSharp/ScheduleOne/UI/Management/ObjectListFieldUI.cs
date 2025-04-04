using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE1 RID: 2785
	public class ObjectListFieldUI : MonoBehaviour
	{
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06004A81 RID: 19073 RVA: 0x00138718 File Offset: 0x00136918
		// (set) Token: 0x06004A82 RID: 19074 RVA: 0x00138720 File Offset: 0x00136920
		public List<ObjectListField> Fields { get; protected set; } = new List<ObjectListField>();

		// Token: 0x06004A83 RID: 19075 RVA: 0x0013872C File Offset: 0x0013692C
		public void Bind(List<ObjectListField> field)
		{
			this.Fields = new List<ObjectListField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedObjects);
			if (field.Count == 1)
			{
				this.EditIcon.gameObject.SetActive(true);
				this.NoMultiEdit.gameObject.SetActive(false);
				this.Button.interactable = true;
				return;
			}
			this.EditIcon.gameObject.SetActive(false);
			this.NoMultiEdit.gameObject.SetActive(true);
			this.Button.interactable = false;
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x001387FC File Offset: 0x001369FC
		private void Refresh(List<BuildableItem> newVal)
		{
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			bool flag = this.AreFieldsUniform();
			if (flag)
			{
				if (this.Fields[0].SelectedObjects.Count == 0)
				{
					this.NoneSelected.SetActive(true);
				}
			}
			else
			{
				this.MultipleSelected.SetActive(true);
			}
			if (this.Fields.Count == 1)
			{
				this.FieldLabel.text = string.Concat(new string[]
				{
					this.FieldText,
					" (",
					newVal.Count.ToString(),
					"/",
					this.Fields[0].MaxItems.ToString(),
					")"
				});
			}
			else
			{
				this.FieldLabel.text = this.FieldText;
			}
			for (int i = 0; i < this.Entries.Length; i++)
			{
				if (flag && this.Fields[0].SelectedObjects.Count > i)
				{
					this.Entries[i].Find("Title").GetComponent<TextMeshProUGUI>().text = this.Fields[0].SelectedObjects[i].ItemInstance.Name;
					this.Entries[i].Find("Title").gameObject.SetActive(true);
				}
				else
				{
					this.Entries[i].Find("Title").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x00138998 File Offset: 0x00136B98
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (!this.Fields[i].SelectedObjects.SequenceEqual(this.Fields[i + 1].SelectedObjects))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x001389EC File Offset: 0x00136BEC
		public void Clicked()
		{
			List<BuildableItem> list = new List<BuildableItem>();
			if (this.AreFieldsUniform())
			{
				list.AddRange(this.Fields[0].SelectedObjects);
			}
			Singleton<ManagementInterface>.Instance.ObjectSelector.Open(this.InstructionText, this.ExtendedInstructionText, this.Fields[0].MaxItems, list, this.Fields[0].TypeRequirements, this.Fields[0].ParentConfig.Configurable.ParentProperty, new ObjectSelector.ObjectFilter(this.ObjectValid), new Action<List<BuildableItem>>(this.ObjectsSelected), null);
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x00138A90 File Offset: 0x00136C90
		private bool ObjectValid(BuildableItem obj, out string reason)
		{
			string text = string.Empty;
			for (int i = 0; i < this.Fields.Count; i++)
			{
				if (this.Fields[i].objectFilter == null || this.Fields[i].objectFilter(obj, out reason))
				{
					reason = string.Empty;
					return true;
				}
				text = reason;
			}
			reason = text;
			return false;
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x00138AF8 File Offset: 0x00136CF8
		public void ObjectsSelected(List<BuildableItem> objs)
		{
			foreach (ObjectListField objectListField in this.Fields)
			{
				new List<BuildableItem>().AddRange(objs);
				objectListField.SetList(objs, true);
			}
		}

		// Token: 0x04003811 RID: 14353
		[Header("References")]
		public string FieldText = "Objects";

		// Token: 0x04003812 RID: 14354
		public string InstructionText = "Select <ObjectType>";

		// Token: 0x04003813 RID: 14355
		public string ExtendedInstructionText = string.Empty;

		// Token: 0x04003814 RID: 14356
		public TextMeshProUGUI FieldLabel;

		// Token: 0x04003815 RID: 14357
		public GameObject NoneSelected;

		// Token: 0x04003816 RID: 14358
		public GameObject MultipleSelected;

		// Token: 0x04003817 RID: 14359
		public RectTransform[] Entries;

		// Token: 0x04003818 RID: 14360
		public Button Button;

		// Token: 0x04003819 RID: 14361
		public GameObject EditIcon;

		// Token: 0x0400381A RID: 14362
		public GameObject NoMultiEdit;
	}
}
