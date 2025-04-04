using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000ADE RID: 2782
	public class NumberFieldUI : MonoBehaviour
	{
		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06004A6C RID: 19052 RVA: 0x001380CA File Offset: 0x001362CA
		// (set) Token: 0x06004A6D RID: 19053 RVA: 0x001380D2 File Offset: 0x001362D2
		public List<NumberField> Fields { get; protected set; } = new List<NumberField>();

		// Token: 0x06004A6E RID: 19054 RVA: 0x001380DC File Offset: 0x001362DC
		public void Bind(List<NumberField> field)
		{
			this.Fields = new List<NumberField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onItemChanged.AddListener(new UnityAction<float>(this.Refresh));
			this.MinValueLabel.text = this.Fields[0].MinValue.ToString();
			this.MaxValueLabel.text = this.Fields[0].MaxValue.ToString();
			this.Slider.minValue = this.Fields[0].MinValue;
			this.Slider.maxValue = this.Fields[0].MaxValue;
			this.Slider.wholeNumbers = this.Fields[0].WholeNumbers;
			this.Slider.onValueChanged.AddListener(new UnityAction<float>(this.ValueChanged));
			this.Refresh(this.Fields[0].Value);
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x001381FD File Offset: 0x001363FD
		private void Refresh(float newVal)
		{
			if (this.AreFieldsUniform())
			{
				this.ValueLabel.text = newVal.ToString();
			}
			else
			{
				this.ValueLabel.text = "#";
			}
			this.Slider.SetValueWithoutNotify(newVal);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00138238 File Offset: 0x00136438
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

		// Token: 0x06004A71 RID: 19057 RVA: 0x00138288 File Offset: 0x00136488
		public void ValueChanged(float value)
		{
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].SetValue(value, true);
			}
		}

		// Token: 0x04003800 RID: 14336
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x04003801 RID: 14337
		public Slider Slider;

		// Token: 0x04003802 RID: 14338
		public TextMeshProUGUI ValueLabel;

		// Token: 0x04003803 RID: 14339
		public TextMeshProUGUI MinValueLabel;

		// Token: 0x04003804 RID: 14340
		public TextMeshProUGUI MaxValueLabel;
	}
}
