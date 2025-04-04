using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF8 RID: 2808
	public class BotanistUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0013B9F5 File Offset: 0x00139BF5
		// (set) Token: 0x06004B20 RID: 19232 RVA: 0x0013B9FD File Offset: 0x00139BFD
		public Botanist AssignedBotanist { get; protected set; }

		// Token: 0x06004B21 RID: 19233 RVA: 0x0013BA08 File Offset: 0x00139C08
		public void Initialize(Botanist bot)
		{
			this.AssignedBotanist = bot;
			this.AssignedBotanist.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = bot.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x0013BA64 File Offset: 0x00139C64
		protected virtual void RefreshUI()
		{
			BotanistConfiguration botanistConfiguration = this.AssignedBotanist.Configuration as BotanistConfiguration;
			this.NoSupply.gameObject.SetActive(botanistConfiguration.Supplies.SelectedObject == null);
			if (botanistConfiguration.Supplies.SelectedObject != null)
			{
				this.SupplyIcon.sprite = botanistConfiguration.Supplies.SelectedObject.ItemInstance.Icon;
				this.SupplyIcon.gameObject.SetActive(true);
			}
			else
			{
				this.SupplyIcon.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.PotRects.Length; i++)
			{
				if (botanistConfiguration.AssignedStations.SelectedObjects.Count > i)
				{
					this.PotRects[i].Find("Icon").GetComponent<Image>().sprite = botanistConfiguration.AssignedStations.SelectedObjects[i].ItemInstance.Icon;
					this.PotRects[i].Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					this.PotRects[i].Find("Icon").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04003892 RID: 14482
		[Header("References")]
		public Image SupplyIcon;

		// Token: 0x04003893 RID: 14483
		public GameObject NoSupply;

		// Token: 0x04003894 RID: 14484
		public TextMeshProUGUI SupplyLabel;

		// Token: 0x04003895 RID: 14485
		public RectTransform[] PotRects;
	}
}
