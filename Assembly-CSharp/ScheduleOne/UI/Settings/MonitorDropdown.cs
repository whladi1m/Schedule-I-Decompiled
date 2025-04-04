using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A74 RID: 2676
	public class MonitorDropdown : SettingsDropdown
	{
		// Token: 0x06004822 RID: 18466 RVA: 0x0012E584 File Offset: 0x0012C784
		protected override void Awake()
		{
			base.Awake();
			Display[] displays = Display.displays;
			for (int i = 0; i < displays.Length; i++)
			{
				base.AddOption("Monitor " + (i + 1).ToString());
			}
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x0012E5C8 File Offset: 0x0012C7C8
		protected virtual void OnEnable()
		{
			Display[] displays = Display.displays;
			for (int i = 0; i < displays.Length; i++)
			{
				bool active = displays[i].active;
			}
			this.dropdown.SetValueWithoutNotify(Mathf.Clamp(MonitorDropdown.GetCurrentDisplayNumber(), 0, this.dropdown.options.Count - 1));
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0012E61A File Offset: 0x0012C81A
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.ActiveDisplayIndex = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0012E63E File Offset: 0x0012C83E
		public static int GetCurrentDisplayNumber()
		{
			List<DisplayInfo> list = new List<DisplayInfo>();
			Screen.GetDisplayLayout(list);
			return list.IndexOf(Screen.mainWindowDisplayInfo);
		}
	}
}
