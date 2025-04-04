using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B00 RID: 2816
	public class MixingStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004B47 RID: 19271 RVA: 0x0013C08B File Offset: 0x0013A28B
		// (set) Token: 0x06004B48 RID: 19272 RVA: 0x0013C093 File Offset: 0x0013A293
		public MixingStation AssignedStation { get; protected set; }

		// Token: 0x06004B49 RID: 19273 RVA: 0x0013C09C File Offset: 0x0013A29C
		public void Initialize(MixingStation station)
		{
			this.AssignedStation = station;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x0013C0DC File Offset: 0x0013A2DC
		protected virtual void RefreshUI()
		{
			MixingStationConfiguration mixingStationConfiguration = this.AssignedStation.Configuration as MixingStationConfiguration;
			base.SetAssignedNPC(mixingStationConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
