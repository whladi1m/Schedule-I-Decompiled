using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFC RID: 2812
	public class ChemistUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x0013BD83 File Offset: 0x00139F83
		// (set) Token: 0x06004B34 RID: 19252 RVA: 0x0013BD8B File Offset: 0x00139F8B
		public Chemist AssignedChemist { get; protected set; }

		// Token: 0x06004B35 RID: 19253 RVA: 0x0013BD94 File Offset: 0x00139F94
		public void Initialize(Chemist chemist)
		{
			this.AssignedChemist = chemist;
			this.AssignedChemist.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = chemist.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0013BDF0 File Offset: 0x00139FF0
		protected virtual void RefreshUI()
		{
			ChemistConfiguration chemistConfiguration = this.AssignedChemist.Configuration as ChemistConfiguration;
			for (int i = 0; i < this.StationsIcons.Length; i++)
			{
				if (chemistConfiguration.Stations.SelectedObjects.Count > i)
				{
					this.StationsIcons[i].sprite = chemistConfiguration.Stations.SelectedObjects[i].ItemInstance.Icon;
					this.StationsIcons[i].enabled = true;
				}
				else
				{
					this.StationsIcons[i].enabled = false;
				}
			}
		}

		// Token: 0x0400389C RID: 14492
		[Header("References")]
		public Image[] StationsIcons;
	}
}
