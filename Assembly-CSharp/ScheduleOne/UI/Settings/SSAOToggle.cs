using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A7C RID: 2684
	public class SSAOToggle : SettingsToggle
	{
		// Token: 0x06004842 RID: 18498 RVA: 0x0012EA2B File Offset: 0x0012CC2B
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.SSAO);
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0012EA47 File Offset: 0x0012CC47
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.SSAO = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
