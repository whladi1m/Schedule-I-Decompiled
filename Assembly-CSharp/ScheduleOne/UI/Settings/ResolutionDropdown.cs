using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A76 RID: 2678
	public class ResolutionDropdown : SettingsDropdown
	{
		// Token: 0x0600482B RID: 18475 RVA: 0x0012E6FC File Offset: 0x0012C8FC
		protected override void Awake()
		{
			base.Awake();
			foreach (Resolution resolution in DisplaySettings.GetResolutions().ToArray())
			{
				base.AddOption(resolution.width.ToString() + "x" + resolution.height.ToString());
			}
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0012E75E File Offset: 0x0012C95E
		protected virtual void OnEnable()
		{
			this.dropdown.SetValueWithoutNotify(Mathf.Clamp(Singleton<Settings>.Instance.DisplaySettings.ResolutionIndex, 0, this.dropdown.options.Count - 1));
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0012E792 File Offset: 0x0012C992
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.ResolutionIndex = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
