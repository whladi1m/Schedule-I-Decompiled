using System;
using System.IO;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B10 RID: 2832
	public class MainMenuRig : MonoBehaviour
	{
		// Token: 0x06004B8D RID: 19341 RVA: 0x0013CCBF File Offset: 0x0013AEBF
		public void Awake()
		{
			Singleton<LoadManager>.Instance.onSaveInfoLoaded.AddListener(new UnityAction(this.LoadStuff));
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x0013CCDC File Offset: 0x0013AEDC
		private void LoadStuff()
		{
			bool flag = false;
			if (LoadManager.LastPlayedGame != null)
			{
				string text = Path.Combine(Path.Combine(LoadManager.LastPlayedGame.SavePath, "Players", "Player_0"), "Appearance.json");
				if (File.Exists(text))
				{
					string json = File.ReadAllText(text);
					BasicAvatarSettings basicAvatarSettings = new BasicAvatarSettings();
					JsonUtility.FromJsonOverwrite(json, basicAvatarSettings);
					this.Avatar.LoadAvatarSettings(basicAvatarSettings.GetAvatarSettings());
					flag = true;
					Console.Log("Loaded player appearance from " + text, null);
				}
				float num = LoadManager.LastPlayedGame.Networth;
				for (int i = 0; i < this.CashPiles.Length; i++)
				{
					float displayedAmount = Mathf.Clamp(num, 0f, 100000f);
					this.CashPiles[i].SetDisplayedAmount(displayedAmount);
					num -= 100000f;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				this.Avatar.gameObject.SetActive(false);
			}
		}

		// Token: 0x040038D1 RID: 14545
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x040038D2 RID: 14546
		public BasicAvatarSettings DefaultSettings;

		// Token: 0x040038D3 RID: 14547
		public CashPile[] CashPiles;
	}
}
