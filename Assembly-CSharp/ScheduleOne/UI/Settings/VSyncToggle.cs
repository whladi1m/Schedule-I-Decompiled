using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A7E RID: 2686
	public class VSyncToggle : SettingsToggle
	{
		// Token: 0x06004848 RID: 18504 RVA: 0x0012EAC4 File Offset: 0x0012CCC4
		protected virtual void OnEnable()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.VSync);
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0012EAE0 File Offset: 0x0012CCE0
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.VSync = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
