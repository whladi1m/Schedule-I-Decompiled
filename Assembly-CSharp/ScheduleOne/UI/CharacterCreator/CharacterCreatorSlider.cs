using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B3C RID: 2876
	public class CharacterCreatorSlider : CharacterCreatorField<float>
	{
		// Token: 0x06004C78 RID: 19576 RVA: 0x00141F1D File Offset: 0x0014011D
		protected override void Awake()
		{
			base.Awake();
			this.Slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderChanged));
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00141F41 File Offset: 0x00140141
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.Slider.SetValueWithoutNotify(base.value);
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00141F5A File Offset: 0x0014015A
		public void OnSliderChanged(float newValue)
		{
			base.value = newValue;
			this.WriteValue(false);
		}

		// Token: 0x040039D1 RID: 14801
		[Header("References")]
		public Slider Slider;
	}
}
