using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A72 RID: 2674
	public class Keybinder : MonoBehaviour
	{
		// Token: 0x06004815 RID: 18453 RVA: 0x0012E3E2 File Offset: 0x0012C5E2
		private void Awake()
		{
			RebindActionUI rebindActionUI = this.rebindActionUI;
			rebindActionUI.onRebind = (Action)Delegate.Combine(rebindActionUI.onRebind, new Action(this.OnRebind));
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x0012E40C File Offset: 0x0012C60C
		private void Start()
		{
			Settings instance = Singleton<Settings>.Instance;
			instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.OnSettingsApplied));
			Settings instance2 = Singleton<Settings>.Instance;
			instance2.onInputsApplied = (Action)Delegate.Combine(instance2.onInputsApplied, new Action(this.OnSettingsApplied));
			this.rebindActionUI.UpdateBindingDisplay();
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0012E470 File Offset: 0x0012C670
		private void OnDestroy()
		{
			if (this.rebindActionUI != null)
			{
				RebindActionUI rebindActionUI = this.rebindActionUI;
				rebindActionUI.onRebind = (Action)Delegate.Remove(rebindActionUI.onRebind, new Action(this.OnRebind));
			}
			if (Singleton<Settings>.InstanceExists)
			{
				Settings instance = Singleton<Settings>.Instance;
				instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.OnSettingsApplied));
			}
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0012E4DF File Offset: 0x0012C6DF
		private void OnRebind()
		{
			base.StartCoroutine(Keybinder.<OnRebind>g__ApplySettings|4_0());
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0012E4ED File Offset: 0x0012C6ED
		private void OnSettingsApplied()
		{
			this.rebindActionUI.UpdateBindingDisplay();
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0012E4FA File Offset: 0x0012C6FA
		[CompilerGenerated]
		internal static IEnumerator <OnRebind>g__ApplySettings|4_0()
		{
			yield return new WaitForEndOfFrame();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
			Singleton<Settings>.Instance.ApplyInputSettings(Singleton<Settings>.Instance.ReadInputSettings());
			yield break;
		}

		// Token: 0x0400359B RID: 13723
		public RebindActionUI rebindActionUI;
	}
}
