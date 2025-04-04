using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6A RID: 2666
	public class CameraBobSlider : SettingsSlider
	{
		// Token: 0x060047F5 RID: 18421 RVA: 0x0012DF50 File Offset: 0x0012C150
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.CameraBobbing * 10f);
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0012DF72 File Offset: 0x0012C172
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.CameraBobbing = value / 10f;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
