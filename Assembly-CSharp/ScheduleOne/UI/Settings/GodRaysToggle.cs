using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6F RID: 2671
	public class GodRaysToggle : SettingsToggle
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x0012E291 File Offset: 0x0012C491
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.GodRays);
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0012E2AD File Offset: 0x0012C4AD
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.GodRays = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
