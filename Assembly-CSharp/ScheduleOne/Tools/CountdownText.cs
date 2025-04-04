using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000834 RID: 2100
	public class CountdownText : MonoBehaviour
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x000F3E5C File Offset: 0x000F205C
		private void Start()
		{
			TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
			DateTime dateTime = new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, DateTimeKind.Unspecified);
			this.targetPDTDate = TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000F3EAD File Offset: 0x000F20AD
		private void Update()
		{
			this.UpdateCountdown();
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000F3EB8 File Offset: 0x000F20B8
		private void UpdateCountdown()
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = this.targetPDTDate - utcNow;
			if (timeSpan.TotalSeconds > 0.0)
			{
				this.TimeLabel.text = this.FormatTime(timeSpan);
				return;
			}
			this.TimeLabel.text = "Now available!";
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x000F3F10 File Offset: 0x000F2110
		private string FormatTime(TimeSpan timeSpan)
		{
			return string.Concat(new string[]
			{
				timeSpan.Days.ToString(),
				" days, ",
				timeSpan.Hours.ToString(),
				" hours, ",
				timeSpan.Minutes.ToString(),
				" minutes"
			});
		}

		// Token: 0x040029AB RID: 10667
		public TextMeshProUGUI TimeLabel;

		// Token: 0x040029AC RID: 10668
		[Header("Date Setting")]
		public int Year = 2025;

		// Token: 0x040029AD RID: 10669
		public int Month = 3;

		// Token: 0x040029AE RID: 10670
		public int Day = 24;

		// Token: 0x040029AF RID: 10671
		public int Hour = 16;

		// Token: 0x040029B0 RID: 10672
		public int Minute;

		// Token: 0x040029B1 RID: 10673
		public int Second;

		// Token: 0x040029B2 RID: 10674
		private DateTime targetPDTDate;
	}
}
