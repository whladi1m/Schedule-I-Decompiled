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
	// Token: 0x02000ADF RID: 2783
	public class ObjectFieldUI : MonoBehaviour
	{
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06004A73 RID: 19059 RVA: 0x001382D1 File Offset: 0x001364D1
		// (set) Token: 0x06004A74 RID: 19060 RVA: 0x001382D9 File Offset: 0x001364D9
		public List<ObjectField> Fields { get; protected set; } = new List<ObjectField>();

		// Token: 0x06004A75 RID: 19061 RVA: 0x001382E4 File Offset: 0x001364E4
		public void Bind(List<ObjectField> field)
		{
			this.Fields = new List<ObjectField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedObject);
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x00138350 File Offset: 0x00136550
		private void Refresh(BuildableItem newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.IconImg.sprite = newVal.ItemInstance.Icon;
					this.SelectionLabel.text = newVal.ItemInstance.Name;
					this.IconImg.gameObject.SetActive(true);
				}
				else
				{
					this.NoneSelected.SetActive(true);
					this.SelectionLabel.text = "None";
				}
			}
			else
			{
				this.MultipleSelected.SetActive(true);
				this.SelectionLabel.text = "Mixed";
			}
			ObjectField objectField = this.Fields.FirstOrDefault((ObjectField x) => x.SelectedObject != null);
			this.ClearButton.gameObject.SetActive(objectField != null);
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0013845C File Offset: 0x0013665C
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedObject != this.Fields[i + 1].SelectedObject)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x001384B0 File Offset: 0x001366B0
		public void Clicked()
		{
			BuildableItem buildableItem = null;
			if (this.AreFieldsUniform())
			{
				buildableItem = this.Fields[0].SelectedObject;
			}
			List<BuildableItem> list = new List<BuildableItem>();
			if (buildableItem != null)
			{
				list.Add(buildableItem);
			}
			List<Transform> list2 = new List<Transform>();
			for (int i = 0; i < this.Fields.Count; i++)
			{
				if (this.Fields[i].DrawTransitLine)
				{
					list2.Add(this.Fields[i].ParentConfig.Configurable.UIPoint);
				}
			}
			Singleton<ManagementInterface>.Instance.ObjectSelector.Open(this.InstructionText, this.ExtendedInstructionText, 1, list, this.Fields[0].TypeRequirements, this.Fields[0].ParentConfig.Configurable.ParentProperty, new ObjectSelector.ObjectFilter(this.ObjectValid), new Action<List<BuildableItem>>(this.ObjectsSelected), list2);
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x001385A4 File Offset: 0x001367A4
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

		// Token: 0x06004A7A RID: 19066 RVA: 0x0013860A File Offset: 0x0013680A
		public void ObjectsSelected(List<BuildableItem> objs)
		{
			this.ObjectSelected((objs.Count > 0) ? objs[objs.Count - 1] : null);
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0013862C File Offset: 0x0013682C
		private void ObjectSelected(BuildableItem obj)
		{
			if (obj != null && this.Fields[0].TypeRequirements.Count > 0 && !this.Fields[0].TypeRequirements.Contains(obj.GetType()))
			{
				Console.LogError("Wrong Object type selection", null);
				return;
			}
			foreach (ObjectField objectField in this.Fields)
			{
				objectField.SetObject(obj, true);
			}
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x001386CC File Offset: 0x001368CC
		public void ClearClicked()
		{
			this.ObjectSelected(null);
		}

		// Token: 0x04003806 RID: 14342
		[Header("References")]
		public string InstructionText = "Select <ObjectType>";

		// Token: 0x04003807 RID: 14343
		public string ExtendedInstructionText = string.Empty;

		// Token: 0x04003808 RID: 14344
		public TextMeshProUGUI FieldLabel;

		// Token: 0x04003809 RID: 14345
		public Image IconImg;

		// Token: 0x0400380A RID: 14346
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x0400380B RID: 14347
		public GameObject NoneSelected;

		// Token: 0x0400380C RID: 14348
		public GameObject MultipleSelected;

		// Token: 0x0400380D RID: 14349
		public RectTransform ClearButton;
	}
}
