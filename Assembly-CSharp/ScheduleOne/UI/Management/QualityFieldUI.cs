using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE2 RID: 2786
	public class QualityFieldUI : MonoBehaviour
	{
		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06004A8A RID: 19082 RVA: 0x00138B8C File Offset: 0x00136D8C
		// (set) Token: 0x06004A8B RID: 19083 RVA: 0x00138B94 File Offset: 0x00136D94
		public List<QualityField> Fields { get; protected set; } = new List<QualityField>();

		// Token: 0x06004A8C RID: 19084 RVA: 0x00138BA0 File Offset: 0x00136DA0
		public void Bind(List<QualityField> field)
		{
			this.Fields = new List<QualityField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onValueChanged.AddListener(new UnityAction<EQuality>(this.Refresh));
			for (int i = 0; i < this.QualityButtons.Length; i++)
			{
				EQuality quality = (EQuality)i;
				this.QualityButtons[i].onClick.AddListener(delegate()
				{
					this.ValueChanged(quality);
				});
			}
			this.Refresh(this.Fields[0].Value);
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x00138C50 File Offset: 0x00136E50
		private void Refresh(EQuality value)
		{
			if (this.AreFieldsUniform())
			{
				EQuality value2 = this.Fields[0].Value;
				for (int i = 0; i < this.QualityButtons.Length; i++)
				{
					EQuality equality = (EQuality)i;
					this.QualityButtons[i].interactable = (equality != value2);
				}
				return;
			}
			Button[] qualityButtons = this.QualityButtons;
			for (int j = 0; j < qualityButtons.Length; j++)
			{
				qualityButtons[j].interactable = true;
			}
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x00138CC4 File Offset: 0x00136EC4
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].Value != this.Fields[i + 1].Value)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x00138D14 File Offset: 0x00136F14
		public void ValueChanged(EQuality value)
		{
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].SetValue(value, true);
			}
		}

		// Token: 0x0400381C RID: 14364
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x0400381D RID: 14365
		public Button[] QualityButtons;
	}
}
