using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A7B RID: 2683
	public class SprintModeDropdown : SettingsDropdown
	{
		// Token: 0x0600483E RID: 18494 RVA: 0x0012E988 File Offset: 0x0012CB88
		protected override void Awake()
		{
			base.Awake();
			InputSettings.EActionMode[] array = (InputSettings.EActionMode[])Enum.GetValues(typeof(InputSettings.EActionMode));
			for (int i = 0; i < array.Length; i++)
			{
				string option = array[i].ToString();
				base.AddOption(option);
			}
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0012E9D8 File Offset: 0x0012CBD8
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.InputSettings.SprintMode);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0012E9F4 File Offset: 0x0012CBF4
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.SprintMode = (InputSettings.EActionMode)value;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}
	}
}
