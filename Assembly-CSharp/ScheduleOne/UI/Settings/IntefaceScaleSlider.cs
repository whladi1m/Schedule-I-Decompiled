using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A70 RID: 2672
	public class IntefaceScaleSlider : SettingsSlider
	{
		// Token: 0x0600480E RID: 18446 RVA: 0x0012E2EC File Offset: 0x0012C4EC
		protected virtual void OnEnable()
		{
			this.slider.minValue = 7f;
			this.slider.maxValue = 14f;
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.UIScale / 0.1f);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0012E339 File Offset: 0x0012C539
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.UIScale = value * 0.1f;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0012E364 File Offset: 0x0012C564
		protected override string GetDisplayValue(float value)
		{
			return Mathf.Round(value * 10f).ToString() + "%";
		}

		// Token: 0x04003598 RID: 13720
		public const float MULTIPLIER = 0.1f;

		// Token: 0x04003599 RID: 13721
		public const float MinScale = 0.7f;

		// Token: 0x0400359A RID: 13722
		public const float MaxScale = 1.4f;
	}
}
