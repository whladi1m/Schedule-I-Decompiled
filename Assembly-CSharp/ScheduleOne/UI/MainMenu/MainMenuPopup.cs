using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B0E RID: 2830
	public class MainMenuPopup : Singleton<MainMenuPopup>
	{
		// Token: 0x06004B89 RID: 19337 RVA: 0x0013CC22 File Offset: 0x0013AE22
		public void Open(MainMenuPopup.Data data)
		{
			this.Open(data.Title, data.Description, data.IsBad);
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x0013CC3C File Offset: 0x0013AE3C
		public void Open(string title, string description, bool isBad)
		{
			this.Title.color = (isBad ? new Color32(byte.MaxValue, 115, 115, byte.MaxValue) : Color.white);
			this.Title.text = title;
			this.Description.text = description;
			this.Screen.Open(false);
		}

		// Token: 0x040038CB RID: 14539
		public MainMenuScreen Screen;

		// Token: 0x040038CC RID: 14540
		public TextMeshProUGUI Title;

		// Token: 0x040038CD RID: 14541
		public TextMeshProUGUI Description;

		// Token: 0x02000B0F RID: 2831
		public class Data
		{
			// Token: 0x06004B8C RID: 19340 RVA: 0x0013CCA2 File Offset: 0x0013AEA2
			public Data(string title, string description, bool isBad)
			{
				this.Title = title;
				this.Description = description;
				this.IsBad = isBad;
			}

			// Token: 0x040038CE RID: 14542
			public string Title;

			// Token: 0x040038CF RID: 14543
			public string Description;

			// Token: 0x040038D0 RID: 14544
			public bool IsBad;
		}
	}
}
