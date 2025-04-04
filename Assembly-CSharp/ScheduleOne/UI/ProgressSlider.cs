using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A15 RID: 2581
	public class ProgressSlider : Singleton<ProgressSlider>
	{
		// Token: 0x060045A8 RID: 17832 RVA: 0x0012419F File Offset: 0x0012239F
		private void LateUpdate()
		{
			if (this.progressSetThisFrame)
			{
				this.Container.SetActive(true);
				this.progressSetThisFrame = false;
				return;
			}
			this.Container.SetActive(false);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x001241C9 File Offset: 0x001223C9
		public void ShowProgress(float progress)
		{
			this.progressSetThisFrame = true;
			this.Slider.value = progress;
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x001241DE File Offset: 0x001223DE
		public void Configure(string label, Color sliderFillColor)
		{
			this.Label.text = label;
			this.Label.color = sliderFillColor;
			this.SliderFill.color = sliderFillColor;
		}

		// Token: 0x04003384 RID: 13188
		[Header("References")]
		public GameObject Container;

		// Token: 0x04003385 RID: 13189
		public TextMeshProUGUI Label;

		// Token: 0x04003386 RID: 13190
		public Slider Slider;

		// Token: 0x04003387 RID: 13191
		public Image SliderFill;

		// Token: 0x04003388 RID: 13192
		private bool progressSetThisFrame;
	}
}
