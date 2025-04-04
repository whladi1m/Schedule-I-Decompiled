using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A24 RID: 2596
	public class RegionUnlockedCanvas : Singleton<RegionUnlockedCanvas>, IPostSleepEvent
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x001256FE File Offset: 0x001238FE
		// (set) Token: 0x06004614 RID: 17940 RVA: 0x00125706 File Offset: 0x00123906
		public bool IsRunning { get; private set; }

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x0012570F File Offset: 0x0012390F
		// (set) Token: 0x06004616 RID: 17942 RVA: 0x00125717 File Offset: 0x00123917
		public int Order { get; private set; } = 5;

		// Token: 0x06004617 RID: 17943 RVA: 0x00125720 File Offset: 0x00123920
		public void QueueUnlocked(EMapRegion _region)
		{
			this.region = _region;
			Singleton<SleepCanvas>.Instance.AddPostSleepEvent(this);
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x00125734 File Offset: 0x00123934
		public void StartEvent()
		{
			this.IsRunning = true;
			MapRegionData regionData = Singleton<Map>.Instance.GetRegionData(this.region);
			this.RegionLabel.text = regionData.Name;
			this.RegionImage.sprite = regionData.RegionSprite;
			List<NPC> npcsInRegion = NPCManager.GetNPCsInRegion(this.region);
			int num = npcsInRegion.Count((NPC x) => x.GetComponent<Customer>() != null);
			int num2 = npcsInRegion.Count((NPC x) => x is Dealer);
			int num3 = npcsInRegion.Count((NPC x) => x is Supplier);
			this.RegionDescription.text = string.Empty;
			if (num > 0)
			{
				TextMeshProUGUI regionDescription = this.RegionDescription;
				regionDescription.text = regionDescription.text + num.ToString() + " potential customer" + ((num > 1) ? "s" : "");
			}
			if (num2 > 0)
			{
				if (this.RegionDescription.text.Length > 0)
				{
					TextMeshProUGUI regionDescription2 = this.RegionDescription;
					regionDescription2.text += "\n";
				}
				TextMeshProUGUI regionDescription3 = this.RegionDescription;
				regionDescription3.text = regionDescription3.text + num2.ToString() + " dealer" + ((num2 > 1) ? "s" : "");
			}
			if (num3 > 0)
			{
				if (this.RegionDescription.text.Length > 0)
				{
					TextMeshProUGUI regionDescription4 = this.RegionDescription;
					regionDescription4.text += "\n";
				}
				TextMeshProUGUI regionDescription5 = this.RegionDescription;
				regionDescription5.text = regionDescription5.text + num3.ToString() + " supplier" + ((num3 > 1) ? "s" : "");
			}
			this.OpenCloseAnim.Play("Rank up open");
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00125916 File Offset: 0x00123B16
		public void EndEvent()
		{
			if (!this.IsRunning)
			{
				return;
			}
			this.OpenCloseAnim.Play("Rank up close");
			this.IsRunning = false;
		}

		// Token: 0x040033D1 RID: 13265
		public Animation OpenCloseAnim;

		// Token: 0x040033D2 RID: 13266
		public TextMeshProUGUI RegionLabel;

		// Token: 0x040033D3 RID: 13267
		public TextMeshProUGUI RegionDescription;

		// Token: 0x040033D4 RID: 13268
		public Image RegionImage;

		// Token: 0x040033D5 RID: 13269
		private EMapRegion region;
	}
}
