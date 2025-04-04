using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFA RID: 2810
	public class CauldronUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x0013BC1F File Offset: 0x00139E1F
		// (set) Token: 0x06004B2A RID: 19242 RVA: 0x0013BC27 File Offset: 0x00139E27
		public Cauldron AssignedCauldron { get; protected set; }

		// Token: 0x06004B2B RID: 19243 RVA: 0x0013BC30 File Offset: 0x00139E30
		public void Initialize(Cauldron cauldron)
		{
			this.AssignedCauldron = cauldron;
			this.AssignedCauldron.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x0013BC70 File Offset: 0x00139E70
		protected virtual void RefreshUI()
		{
			CauldronConfiguration cauldronConfiguration = this.AssignedCauldron.Configuration as CauldronConfiguration;
			base.SetAssignedNPC(cauldronConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
