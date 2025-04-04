using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x0200099D RID: 2461
	public class QualitySetter : MonoBehaviour
	{
		// Token: 0x06004283 RID: 17027 RVA: 0x00116ED1 File Offset: 0x001150D1
		private void Awake()
		{
			base.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate(int x)
			{
				this.SetQuality(x);
			});
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00116EEF File Offset: 0x001150EF
		private void SetQuality(int quality)
		{
			Console.Log("Setting quality to " + quality.ToString(), null);
			QualitySettings.SetQualityLevel(quality);
		}
	}
}
