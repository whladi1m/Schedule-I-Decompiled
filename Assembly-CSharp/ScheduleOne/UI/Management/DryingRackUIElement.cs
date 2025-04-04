using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFE RID: 2814
	public class DryingRackUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x0013BF72 File Offset: 0x0013A172
		// (set) Token: 0x06004B3E RID: 19262 RVA: 0x0013BF7A File Offset: 0x0013A17A
		public DryingRack AssignedRack { get; protected set; }

		// Token: 0x06004B3F RID: 19263 RVA: 0x0013BF83 File Offset: 0x0013A183
		public void Initialize(DryingRack rack)
		{
			this.AssignedRack = rack;
			this.AssignedRack.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x0013BFC0 File Offset: 0x0013A1C0
		protected virtual void RefreshUI()
		{
			DryingRackConfiguration dryingRackConfiguration = this.AssignedRack.Configuration as DryingRackConfiguration;
			EQuality value = dryingRackConfiguration.TargetQuality.Value;
			this.TargetQualityIcon.color = ItemQuality.GetColor(value);
			base.SetAssignedNPC(dryingRackConfiguration.AssignedBotanist.SelectedNPC);
		}

		// Token: 0x040038A1 RID: 14497
		public Image TargetQualityIcon;
	}
}
