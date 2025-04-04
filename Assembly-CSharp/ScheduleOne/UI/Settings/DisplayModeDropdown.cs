using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6C RID: 2668
	public class DisplayModeDropdown : SettingsDropdown
	{
		// Token: 0x060047FF RID: 18431 RVA: 0x0012E124 File Offset: 0x0012C324
		protected override void Awake()
		{
			base.Awake();
			DisplaySettings.EDisplayMode[] array = (DisplaySettings.EDisplayMode[])Enum.GetValues(typeof(DisplaySettings.EDisplayMode));
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToString();
				text = text.Replace("ExclusiveFullscreen", "Exclusive Fullscreen");
				text = text.Replace("FullscreenWindow", "Fullscreen Window");
				base.AddOption(text);
			}
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0012E196 File Offset: 0x0012C396
		protected virtual void OnEnable()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.DisplaySettings.DisplayMode);
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0012E1B2 File Offset: 0x0012C3B2
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.DisplayMode = (DisplaySettings.EDisplayMode)value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
