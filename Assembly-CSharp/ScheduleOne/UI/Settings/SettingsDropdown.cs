using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A78 RID: 2680
	public class SettingsDropdown : MonoBehaviour
	{
		// Token: 0x06004832 RID: 18482 RVA: 0x0012E818 File Offset: 0x0012CA18
		protected virtual void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
			foreach (string option in this.DefaultOptions)
			{
				this.AddOption(option);
			}
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnValueChanged(int value)
		{
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x0012E86E File Offset: 0x0012CA6E
		protected void AddOption(string option)
		{
			this.dropdown.options.Add(new TMP_Dropdown.OptionData(option));
		}

		// Token: 0x0400359F RID: 13727
		public string[] DefaultOptions;

		// Token: 0x040035A0 RID: 13728
		protected TMP_Dropdown dropdown;
	}
}
