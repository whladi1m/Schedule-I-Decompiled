using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFF RID: 2815
	public class LabOvenUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x0013C00C File Offset: 0x0013A20C
		// (set) Token: 0x06004B43 RID: 19267 RVA: 0x0013C014 File Offset: 0x0013A214
		public LabOven AssignedOven { get; protected set; }

		// Token: 0x06004B44 RID: 19268 RVA: 0x0013C01D File Offset: 0x0013A21D
		public void Initialize(LabOven oven)
		{
			this.AssignedOven = oven;
			this.AssignedOven.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0013C05C File Offset: 0x0013A25C
		protected virtual void RefreshUI()
		{
			LabOvenConfiguration labOvenConfiguration = this.AssignedOven.Configuration as LabOvenConfiguration;
			base.SetAssignedNPC(labOvenConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
