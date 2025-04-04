using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A71 RID: 2673
	public class InvertYToggle : SettingsToggle
	{
		// Token: 0x06004812 RID: 18450 RVA: 0x0012E38F File Offset: 0x0012C58F
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.InputSettings.InvertMouse);
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0012E3AB File Offset: 0x0012C5AB
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.InvertMouse = value;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}
	}
}
