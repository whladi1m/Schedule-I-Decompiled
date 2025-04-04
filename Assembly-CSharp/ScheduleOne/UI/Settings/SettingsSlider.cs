using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A79 RID: 2681
	public class SettingsSlider : MonoBehaviour
	{
		// Token: 0x06004836 RID: 18486 RVA: 0x0012E888 File Offset: 0x0012CA88
		protected virtual void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
			this.valueLabel = this.slider.handleRect.Find("Value").GetComponent<TextMeshProUGUI>();
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0012E8DE File Offset: 0x0012CADE
		protected virtual void Update()
		{
			if (this.DisplayValue && Time.time - this.timeOnValueChange > 2f)
			{
				this.valueLabel.enabled = false;
			}
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0012E907 File Offset: 0x0012CB07
		protected virtual void OnValueChanged(float value)
		{
			this.timeOnValueChange = Time.time;
			if (this.DisplayValue)
			{
				this.valueLabel.text = this.GetDisplayValue(value);
				this.valueLabel.enabled = true;
			}
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x0012E93A File Offset: 0x0012CB3A
		protected virtual string GetDisplayValue(float value)
		{
			return value.ToString();
		}

		// Token: 0x040035A1 RID: 13729
		private const float VALUE_DISPLAY_TIME = 2f;

		// Token: 0x040035A2 RID: 13730
		public bool DisplayValue = true;

		// Token: 0x040035A3 RID: 13731
		protected Slider slider;

		// Token: 0x040035A4 RID: 13732
		protected TextMeshProUGUI valueLabel;

		// Token: 0x040035A5 RID: 13733
		protected float timeOnValueChange = -100f;
	}
}
