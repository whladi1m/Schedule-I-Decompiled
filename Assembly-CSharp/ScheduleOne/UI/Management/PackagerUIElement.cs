using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B01 RID: 2817
	public class PackagerUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004B4C RID: 19276 RVA: 0x0013C10B File Offset: 0x0013A30B
		// (set) Token: 0x06004B4D RID: 19277 RVA: 0x0013C113 File Offset: 0x0013A313
		public Packager AssignedPackager { get; protected set; }

		// Token: 0x06004B4E RID: 19278 RVA: 0x0013C11C File Offset: 0x0013A31C
		public void Initialize(Packager packager)
		{
			this.AssignedPackager = packager;
			this.AssignedPackager.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = packager.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x0013C178 File Offset: 0x0013A378
		protected virtual void RefreshUI()
		{
			PackagerConfiguration packagerConfiguration = this.AssignedPackager.Configuration as PackagerConfiguration;
			for (int i = 0; i < this.StationRects.Length; i++)
			{
				if (packagerConfiguration.Stations.SelectedObjects.Count > i)
				{
					this.StationRects[i].Find("Icon").GetComponent<Image>().sprite = packagerConfiguration.Stations.SelectedObjects[i].ItemInstance.Icon;
					this.StationRects[i].Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					this.StationRects[i].Find("Icon").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x040038A4 RID: 14500
		[Header("References")]
		public RectTransform[] StationRects;
	}
}
