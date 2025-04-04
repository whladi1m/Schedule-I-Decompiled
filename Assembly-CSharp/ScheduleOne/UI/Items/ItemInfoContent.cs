using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B46 RID: 2886
	public class ItemInfoContent : MonoBehaviour
	{
		// Token: 0x06004CB2 RID: 19634 RVA: 0x00142DBB File Offset: 0x00140FBB
		public virtual void Initialize(ItemInstance instance)
		{
			this.NameLabel.text = instance.Name;
			this.DescriptionLabel.text = instance.Description;
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x00142DDF File Offset: 0x00140FDF
		public virtual void Initialize(ItemDefinition definition)
		{
			this.NameLabel.text = definition.Name;
			this.DescriptionLabel.text = definition.Description;
		}

		// Token: 0x04003A01 RID: 14849
		[Header("Settings")]
		public float Height = 90f;

		// Token: 0x04003A02 RID: 14850
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003A03 RID: 14851
		public TextMeshProUGUI DescriptionLabel;
	}
}
