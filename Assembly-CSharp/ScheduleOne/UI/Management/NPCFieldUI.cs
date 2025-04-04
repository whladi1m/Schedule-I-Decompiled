using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000ADC RID: 2780
	public class NPCFieldUI : MonoBehaviour
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x00137DD3 File Offset: 0x00135FD3
		// (set) Token: 0x06004A61 RID: 19041 RVA: 0x00137DDB File Offset: 0x00135FDB
		public List<NPCField> Fields { get; protected set; } = new List<NPCField>();

		// Token: 0x06004A62 RID: 19042 RVA: 0x00137DE4 File Offset: 0x00135FE4
		public void Bind(List<NPCField> field)
		{
			this.Fields = new List<NPCField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onNPCChanged.AddListener(new UnityAction<NPC>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedNPC);
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x00137E50 File Offset: 0x00136050
		private void Refresh(NPC newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.IconImg.sprite = newVal.MugshotSprite;
					this.SelectionLabel.text = newVal.FirstName + "\n" + newVal.LastName;
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
			NPCField npcfield = this.Fields.FirstOrDefault((NPCField x) => x.SelectedNPC != null);
			this.ClearButton.gameObject.SetActive(npcfield != null);
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x00137F60 File Offset: 0x00136160
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedNPC != this.Fields[i + 1].SelectedNPC)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x00137FB4 File Offset: 0x001361B4
		public void Clicked()
		{
			this.AreFieldsUniform();
			Singleton<ManagementInterface>.Instance.NPCSelector.Open("Select " + this.FieldLabel.text, this.Fields[0].TypeRequirement, new Action<NPC>(this.NPCSelected));
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x0013800C File Offset: 0x0013620C
		public void NPCSelected(NPC npc)
		{
			if (npc != null && npc.GetType() != this.Fields[0].TypeRequirement)
			{
				Console.LogError("Wrong NPC type selection", null);
				return;
			}
			foreach (NPCField npcfield in this.Fields)
			{
				npcfield.SetNPC(npc, true);
			}
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x00138094 File Offset: 0x00136294
		public void ClearClicked()
		{
			this.NPCSelected(null);
		}

		// Token: 0x040037F7 RID: 14327
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040037F8 RID: 14328
		public Image IconImg;

		// Token: 0x040037F9 RID: 14329
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x040037FA RID: 14330
		public GameObject NoneSelected;

		// Token: 0x040037FB RID: 14331
		public GameObject MultipleSelected;

		// Token: 0x040037FC RID: 14332
		public RectTransform ClearButton;
	}
}
