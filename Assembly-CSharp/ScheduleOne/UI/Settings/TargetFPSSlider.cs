using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A7D RID: 2685
	public class TargetFPSSlider : SettingsSlider
	{
		// Token: 0x06004845 RID: 18501 RVA: 0x0012EA7E File Offset: 0x0012CC7E
		protected virtual void OnEnable()
		{
			this.slider.SetValueWithoutNotify((float)Singleton<Settings>.Instance.DisplaySettings.TargetFPS);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x0012EA9B File Offset: 0x0012CC9B
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.TargetFPS = Mathf.RoundToInt(value);
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
