using System;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B23 RID: 2851
	public class HandoverScreenDetailPanel : MonoBehaviour
	{
		// Token: 0x06004BEB RID: 19435 RVA: 0x0013FEFC File Offset: 0x0013E0FC
		public void Open(Customer customer)
		{
			this.NameLabel.text = customer.NPC.fullName;
			if (customer.NPC.RelationData.Unlocked)
			{
				this.RelationshipContainer.gameObject.SetActive(true);
				this.RelationshipScrollbar.value = customer.NPC.RelationData.NormalizedRelationDelta;
				this.AddictionContainer.gameObject.SetActive(true);
				this.AdditionScrollbar.value = customer.CurrentAddiction;
			}
			else
			{
				this.RelationshipContainer.gameObject.SetActive(false);
				this.AddictionContainer.gameObject.SetActive(false);
			}
			this.StandardsStar.color = ItemQuality.GetColor(customer.CustomerData.Standards.GetCorrespondingQuality());
			this.StandardsLabel.text = customer.CustomerData.Standards.GetName();
			this.StandardsLabel.color = this.StandardsStar.color;
			this.EffectsLabel.text = string.Empty;
			for (int i = 0; i < customer.CustomerData.PreferredProperties.Count; i++)
			{
				if (i > 0)
				{
					TextMeshProUGUI effectsLabel = this.EffectsLabel;
					effectsLabel.text += "\n";
				}
				string str = string.Concat(new string[]
				{
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(customer.CustomerData.PreferredProperties[i].LabelColor),
					">•  ",
					customer.CustomerData.PreferredProperties[i].Name,
					"</color>"
				});
				TextMeshProUGUI effectsLabel2 = this.EffectsLabel;
				effectsLabel2.text += str;
			}
			base.gameObject.SetActive(true);
			this.LayoutGroup.CalculateLayoutInputHorizontal();
			this.LayoutGroup.CalculateLayoutInputVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.LayoutGroup.GetComponent<RectTransform>());
			this.LayoutGroup.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			this.Container.anchoredPosition = new Vector2(0f, -this.Container.sizeDelta.y / 2f);
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003943 RID: 14659
		public LayoutGroup LayoutGroup;

		// Token: 0x04003944 RID: 14660
		public RectTransform Container;

		// Token: 0x04003945 RID: 14661
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003946 RID: 14662
		public RectTransform RelationshipContainer;

		// Token: 0x04003947 RID: 14663
		public Scrollbar RelationshipScrollbar;

		// Token: 0x04003948 RID: 14664
		public RectTransform AddictionContainer;

		// Token: 0x04003949 RID: 14665
		public Scrollbar AdditionScrollbar;

		// Token: 0x0400394A RID: 14666
		public Image StandardsStar;

		// Token: 0x0400394B RID: 14667
		public TextMeshProUGUI StandardsLabel;

		// Token: 0x0400394C RID: 14668
		public TextMeshProUGUI EffectsLabel;
	}
}
