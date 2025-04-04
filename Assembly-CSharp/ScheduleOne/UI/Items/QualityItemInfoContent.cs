using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B4D RID: 2893
	public class QualityItemInfoContent : ItemInfoContent
	{
		// Token: 0x06004CE9 RID: 19689 RVA: 0x00144AF8 File Offset: 0x00142CF8
		public override void Initialize(ItemInstance instance)
		{
			base.Initialize(instance);
			QualityItemInstance qualityItemInstance = instance as QualityItemInstance;
			if (qualityItemInstance == null)
			{
				Console.LogError("QualityItemInfoContent can only be used with QualityItemInstance!", null);
				return;
			}
			this.QualityLabel.text = qualityItemInstance.Quality.ToString();
			this.QualityLabel.color = ItemQuality.GetColor(qualityItemInstance.Quality);
			this.Star.color = ItemQuality.GetColor(qualityItemInstance.Quality);
			this.QualityLabel.gameObject.SetActive(true);
		}

		// Token: 0x04003A36 RID: 14902
		public Image Star;

		// Token: 0x04003A37 RID: 14903
		public TextMeshProUGUI QualityLabel;
	}
}
