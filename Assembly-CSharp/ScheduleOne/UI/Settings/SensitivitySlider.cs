using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A77 RID: 2679
	public class SensitivitySlider : SettingsSlider
	{
		// Token: 0x0600482F RID: 18479 RVA: 0x0012E7B6 File Offset: 0x0012C9B6
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.InputSettings.MouseSensitivity / 0.033333335f);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0012E7D8 File Offset: 0x0012C9D8
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.MouseSensitivity = value * 0.033333335f;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}

		// Token: 0x0400359E RID: 13726
		public const float MULTIPLIER = 0.033333335f;
	}
}
