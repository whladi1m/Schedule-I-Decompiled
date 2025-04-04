using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6D RID: 2669
	public class FOVSLider : SettingsSlider
	{
		// Token: 0x06004803 RID: 18435 RVA: 0x0012E1D6 File Offset: 0x0012C3D6
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.FOV);
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0012E1F2 File Offset: 0x0012C3F2
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.FOV = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
