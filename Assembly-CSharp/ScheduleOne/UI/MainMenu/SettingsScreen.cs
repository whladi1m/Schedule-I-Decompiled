using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B17 RID: 2839
	public class SettingsScreen : MainMenuScreen
	{
		// Token: 0x06004BB4 RID: 19380 RVA: 0x0013D773 File Offset: 0x0013B973
		protected override void Awake()
		{
			base.Awake();
			this.ApplyDisplayButton.onClick.AddListener(new UnityAction(this.ApplyDisplaySettings));
			this.ApplyDisplayButton.gameObject.SetActive(false);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0013D7A8 File Offset: 0x0013B9A8
		protected void Start()
		{
			for (int i = 0; i < this.Categories.Length; i++)
			{
				int index = i;
				this.Categories[i].Button.onClick.AddListener(delegate()
				{
					this.ShowCategory(index);
				});
			}
			this.ShowCategory(0);
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0013D808 File Offset: 0x0013BA08
		public void ShowCategory(int index)
		{
			for (int i = 0; i < this.Categories.Length; i++)
			{
				this.Categories[i].Button.interactable = (i != index);
				this.Categories[i].Panel.SetActive(i == index);
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0013D857 File Offset: 0x0013BA57
		public void DisplayChanged()
		{
			this.ApplyDisplayButton.gameObject.SetActive(true);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x0013D86C File Offset: 0x0013BA6C
		private void ApplyDisplaySettings()
		{
			this.ApplyDisplayButton.gameObject.SetActive(false);
			DisplaySettings displaySettings = Singleton<Settings>.Instance.DisplaySettings;
			DisplaySettings unappliedDisplaySettings = Singleton<Settings>.Instance.UnappliedDisplaySettings;
			Singleton<Settings>.Instance.ApplyDisplaySettings(unappliedDisplaySettings);
			this.ConfirmDisplaySettings.Open(displaySettings, unappliedDisplaySettings);
		}

		// Token: 0x040038F2 RID: 14578
		public SettingsScreen.SettingsCategory[] Categories;

		// Token: 0x040038F3 RID: 14579
		public Button ApplyDisplayButton;

		// Token: 0x040038F4 RID: 14580
		public ConfirmDisplaySettings ConfirmDisplaySettings;

		// Token: 0x02000B18 RID: 2840
		[Serializable]
		public class SettingsCategory
		{
			// Token: 0x040038F5 RID: 14581
			public Button Button;

			// Token: 0x040038F6 RID: 14582
			public GameObject Panel;
		}
	}
}
