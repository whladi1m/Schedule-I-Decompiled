using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF9 RID: 2809
	public class BrickPressUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x0013BBA1 File Offset: 0x00139DA1
		// (set) Token: 0x06004B25 RID: 19237 RVA: 0x0013BBA9 File Offset: 0x00139DA9
		public BrickPress AssignedPress { get; protected set; }

		// Token: 0x06004B26 RID: 19238 RVA: 0x0013BBB2 File Offset: 0x00139DB2
		public void Initialize(BrickPress press)
		{
			this.AssignedPress = press;
			this.AssignedPress.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x0013BBF0 File Offset: 0x00139DF0
		protected virtual void RefreshUI()
		{
			BrickPressConfiguration brickPressConfiguration = this.AssignedPress.Configuration as BrickPressConfiguration;
			base.SetAssignedNPC(brickPressConfiguration.AssignedPackager.SelectedNPC);
		}
	}
}
