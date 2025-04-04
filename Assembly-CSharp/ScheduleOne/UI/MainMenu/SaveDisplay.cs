using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B15 RID: 2837
	public class SaveDisplay : MonoBehaviour
	{
		// Token: 0x06004BA3 RID: 19363 RVA: 0x0013D21E File Offset: 0x0013B41E
		public void Awake()
		{
			Singleton<LoadManager>.Instance.onSaveInfoLoaded.AddListener(new UnityAction(this.Refresh));
			this.Refresh();
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x0013D244 File Offset: 0x0013B444
		public void Refresh()
		{
			for (int i = 0; i < LoadManager.SaveGames.Length; i++)
			{
				this.SetDisplayedSave(i, LoadManager.SaveGames[i]);
			}
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x0013D274 File Offset: 0x0013B474
		public void SetDisplayedSave(int index, SaveInfo info)
		{
			Transform transform = this.Slots[index].Find("Container");
			if (info == null)
			{
				transform.gameObject.SetActive(false);
				return;
			}
			transform.Find("Organisation").GetComponent<TextMeshProUGUI>().text = info.OrganisationName;
			transform.Find("Version").GetComponent<TextMeshProUGUI>().text = "v" + info.SaveVersion;
			float num = info.Networth;
			string text = string.Empty;
			Color color = new Color32(75, byte.MaxValue, 10, byte.MaxValue);
			if (num > 1000000f)
			{
				num /= 1000000f;
				text = "$" + this.RoundToDecimalPlaces(num, 1).ToString() + "M";
				color = new Color32(byte.MaxValue, 225, 10, byte.MaxValue);
			}
			else if (num > 1000f)
			{
				num /= 1000f;
				text = "$" + this.RoundToDecimalPlaces(num, 1).ToString() + "K";
			}
			else
			{
				text = MoneyManager.FormatAmount(num, false, false);
			}
			transform.Find("NetWorth/Text").GetComponent<TextMeshProUGUI>().text = text;
			transform.Find("NetWorth/Text").GetComponent<TextMeshProUGUI>().color = color;
			int hours = Mathf.RoundToInt((float)(DateTime.Now - info.DateCreated).TotalHours);
			transform.Find("Created/Text").GetComponent<TextMeshProUGUI>().text = this.GetTimeLabel(hours);
			int hours2 = Mathf.RoundToInt((float)(DateTime.Now - info.DateLastPlayed).TotalHours);
			transform.Find("LastPlayed/Text").GetComponent<TextMeshProUGUI>().text = this.GetTimeLabel(hours2);
			transform.gameObject.SetActive(true);
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x0013D449 File Offset: 0x0013B649
		private float RoundToDecimalPlaces(float value, int decimalPlaces)
		{
			return SaveDisplay.ToSingle(Math.Floor((double)value * Math.Pow(10.0, (double)decimalPlaces)) / Math.Pow(10.0, (double)decimalPlaces));
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x0013D479 File Offset: 0x0013B679
		public static float ToSingle(double value)
		{
			return (float)value;
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x0013D480 File Offset: 0x0013B680
		private string GetTimeLabel(int hours)
		{
			int num = hours / 24;
			if (num == 0)
			{
				return "Today";
			}
			if (num == 1)
			{
				return "Yesterday";
			}
			if (num > 365)
			{
				return "More than a year ago";
			}
			return num.ToString() + " days ago";
		}

		// Token: 0x040038EA RID: 14570
		[Header("References")]
		public RectTransform[] Slots;
	}
}
