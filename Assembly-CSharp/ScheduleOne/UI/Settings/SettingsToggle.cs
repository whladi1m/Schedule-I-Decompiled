using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A7A RID: 2682
	public class SettingsToggle : MonoBehaviour
	{
		// Token: 0x0600483B RID: 18491 RVA: 0x0012E95D File Offset: 0x0012CB5D
		protected virtual void Awake()
		{
			this.toggle = base.GetComponent<Toggle>();
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnValueChanged(bool value)
		{
		}

		// Token: 0x040035A6 RID: 13734
		protected Toggle toggle;
	}
}
