using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B02 RID: 2818
	public class PackagingStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x0013C235 File Offset: 0x0013A435
		// (set) Token: 0x06004B52 RID: 19282 RVA: 0x0013C23D File Offset: 0x0013A43D
		public PackagingStation AssignedStation { get; protected set; }

		// Token: 0x06004B53 RID: 19283 RVA: 0x0013C246 File Offset: 0x0013A446
		public void Initialize(PackagingStation pack)
		{
			this.AssignedStation = pack;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x0013C284 File Offset: 0x0013A484
		protected virtual void RefreshUI()
		{
			PackagingStationConfiguration packagingStationConfiguration = this.AssignedStation.Configuration as PackagingStationConfiguration;
			base.SetAssignedNPC(packagingStationConfiguration.AssignedPackager.SelectedNPC);
		}
	}
}
