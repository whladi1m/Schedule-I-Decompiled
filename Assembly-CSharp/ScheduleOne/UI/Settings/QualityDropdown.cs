using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A75 RID: 2677
	public class QualityDropdown : SettingsDropdown
	{
		// Token: 0x06004827 RID: 18471 RVA: 0x0012E658 File Offset: 0x0012C858
		protected override void Awake()
		{
			base.Awake();
			GraphicsSettings.EGraphicsQuality[] array = (GraphicsSettings.EGraphicsQuality[])Enum.GetValues(typeof(GraphicsSettings.EGraphicsQuality));
			for (int i = 0; i < array.Length; i++)
			{
				string option = array[i].ToString();
				base.AddOption(option);
			}
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x0012E6A8 File Offset: 0x0012C8A8
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.GraphicsSettings.GraphicsQuality);
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0012E6C4 File Offset: 0x0012C8C4
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.GraphicsQuality = (GraphicsSettings.EGraphicsQuality)value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
