using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFD RID: 2813
	public class CleanerUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x0013BE7A File Offset: 0x0013A07A
		// (set) Token: 0x06004B39 RID: 19257 RVA: 0x0013BE82 File Offset: 0x0013A082
		public Cleaner AssignedCleaner { get; protected set; }

		// Token: 0x06004B3A RID: 19258 RVA: 0x0013BE8C File Offset: 0x0013A08C
		public void Initialize(Cleaner cleaner)
		{
			this.AssignedCleaner = cleaner;
			this.AssignedCleaner.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = cleaner.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x0013BEE8 File Offset: 0x0013A0E8
		protected virtual void RefreshUI()
		{
			CleanerConfiguration cleanerConfiguration = this.AssignedCleaner.Configuration as CleanerConfiguration;
			for (int i = 0; i < this.StationsIcons.Length; i++)
			{
				if (cleanerConfiguration.Bins.SelectedObjects.Count > i)
				{
					this.StationsIcons[i].sprite = cleanerConfiguration.Bins.SelectedObjects[i].ItemInstance.Icon;
					this.StationsIcons[i].enabled = true;
				}
				else
				{
					this.StationsIcons[i].enabled = false;
				}
			}
		}

		// Token: 0x0400389E RID: 14494
		[Header("References")]
		public Image[] StationsIcons;
	}
}
