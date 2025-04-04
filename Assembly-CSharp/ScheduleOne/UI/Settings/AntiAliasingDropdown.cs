using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A68 RID: 2664
	public class AntiAliasingDropdown : SettingsDropdown
	{
		// Token: 0x060047EE RID: 18414 RVA: 0x0012DD00 File Offset: 0x0012BF00
		protected override void Awake()
		{
			base.Awake();
			GraphicsSettings.EAntiAliasingMode[] array = (GraphicsSettings.EAntiAliasingMode[])Enum.GetValues(typeof(GraphicsSettings.EAntiAliasingMode));
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToString();
				text = text.Replace("MSAAx2", "2x MSAA");
				text = text.Replace("MSAAx4", "4x MSAA");
				text = text.Replace("MSAAx8", "8x MSAA");
				base.AddOption(text);
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0012DD83 File Offset: 0x0012BF83
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.GraphicsSettings.AntiAliasingMode);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0012DD9F File Offset: 0x0012BF9F
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.AntiAliasingMode = (GraphicsSettings.EAntiAliasingMode)value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
