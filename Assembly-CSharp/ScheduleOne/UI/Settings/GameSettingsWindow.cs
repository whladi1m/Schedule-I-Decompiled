using System;
using FishNet;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6E RID: 2670
	public class GameSettingsWindow : MonoBehaviour
	{
		// Token: 0x06004806 RID: 18438 RVA: 0x0012E229 File Offset: 0x0012C429
		private void Awake()
		{
			this.ConsoleToggle.onValueChanged.AddListener(new UnityAction<bool>(this.ConsoleToggled));
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0012E247 File Offset: 0x0012C447
		public void Start()
		{
			this.ApplySettings(NetworkSingleton<GameManager>.Instance.Settings);
			this.Blocker.SetActive(!InstanceFinder.IsServer);
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0012E26C File Offset: 0x0012C46C
		public void ApplySettings(GameSettings settings)
		{
			this.ConsoleToggle.SetIsOnWithoutNotify(settings.ConsoleEnabled);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0012E27F File Offset: 0x0012C47F
		private void ConsoleToggled(bool value)
		{
			NetworkSingleton<GameManager>.Instance.Settings.ConsoleEnabled = value;
		}

		// Token: 0x04003596 RID: 13718
		public Toggle ConsoleToggle;

		// Token: 0x04003597 RID: 13719
		public GameObject Blocker;
	}
}
