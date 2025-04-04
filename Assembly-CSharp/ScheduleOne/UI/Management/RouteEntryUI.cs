using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE4 RID: 2788
	public class RouteEntryUI : MonoBehaviour
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06004A93 RID: 19091 RVA: 0x00138D70 File Offset: 0x00136F70
		// (set) Token: 0x06004A94 RID: 19092 RVA: 0x00138D78 File Offset: 0x00136F78
		public AdvancedTransitRoute AssignedRoute { get; private set; }

		// Token: 0x06004A95 RID: 19093 RVA: 0x00138D81 File Offset: 0x00136F81
		public void AssignRoute(AdvancedTransitRoute route)
		{
			this.AssignedRoute = route;
			this.RefreshUI();
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x00138D90 File Offset: 0x00136F90
		public void ClearRoute()
		{
			this.AssignedRoute = null;
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x00138D9C File Offset: 0x00136F9C
		public void RefreshUI()
		{
			if (this.AssignedRoute != null && this.AssignedRoute.Source != null)
			{
				this.SourceLabel.text = this.AssignedRoute.Source.Name;
			}
			else
			{
				this.SourceLabel.text = "None";
			}
			if (this.AssignedRoute != null && this.AssignedRoute.Destination != null)
			{
				this.DestinationLabel.text = this.AssignedRoute.Destination.Name;
				return;
			}
			this.DestinationLabel.text = "None";
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x00138E2C File Offset: 0x0013702C
		public void SourceClicked()
		{
			this.settingSource = true;
			this.settingDestination = false;
			List<ITransitEntity> selectedObjects = new List<ITransitEntity>();
			List<Transform> list = new List<Transform>();
			if (this.AssignedRoute.Destination != null)
			{
				list.Add(this.AssignedRoute.Destination.LinkOrigin);
			}
			Singleton<ManagementInterface>.Instance.TransitEntitySelector.Open("Select source", "Click an entity to set it as the route source", 1, selectedObjects, new List<Type>(), new TransitEntitySelector.ObjectFilter(this.ObjectValid), new Action<List<ITransitEntity>>(this.ObjectsSelected), list, false);
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x00138EB0 File Offset: 0x001370B0
		public void DestinationClicked()
		{
			this.settingDestination = true;
			this.settingSource = false;
			List<ITransitEntity> selectedObjects = new List<ITransitEntity>();
			List<Transform> list = new List<Transform>();
			if (this.AssignedRoute.Source != null)
			{
				list.Add(this.AssignedRoute.Source.LinkOrigin);
			}
			Singleton<ManagementInterface>.Instance.TransitEntitySelector.Open("Select destination", "Click an entity to set it as the route destination", 1, selectedObjects, new List<Type>(), new TransitEntitySelector.ObjectFilter(this.ObjectValid), new Action<List<ITransitEntity>>(this.ObjectsSelected), list, true);
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x000045B1 File Offset: 0x000027B1
		public void FilterClicked()
		{
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x00138F34 File Offset: 0x00137134
		public void DeleteClicked()
		{
			if (this.onDeleteClicked != null)
			{
				this.onDeleteClicked.Invoke();
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x00138F4C File Offset: 0x0013714C
		private bool ObjectValid(ITransitEntity obj, out string reason)
		{
			reason = string.Empty;
			if (this.AssignedRoute == null)
			{
				return false;
			}
			if (obj == null)
			{
				return false;
			}
			if (this.settingDestination && obj == this.AssignedRoute.Source)
			{
				reason = "Destination cannot be the same as the source";
				return false;
			}
			if (this.settingSource && obj == this.AssignedRoute.Destination)
			{
				reason = "Source cannot be the same as the destination";
				return false;
			}
			return true;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00138FB0 File Offset: 0x001371B0
		public void ObjectsSelected(List<ITransitEntity> objs)
		{
			if (objs.Count > 1)
			{
				objs.RemoveAt(0);
			}
			if (this.settingSource)
			{
				this.AssignedRoute.SetSource((objs.Count > 0) ? objs[0] : null);
			}
			if (this.settingDestination)
			{
				this.AssignedRoute.SetDestination((objs.Count > 0) ? objs[0] : null);
			}
		}

		// Token: 0x04003821 RID: 14369
		[Header("References")]
		public TextMeshProUGUI SourceLabel;

		// Token: 0x04003822 RID: 14370
		public TextMeshProUGUI DestinationLabel;

		// Token: 0x04003823 RID: 14371
		public Image FilterIcon;

		// Token: 0x04003824 RID: 14372
		public UnityEvent onDeleteClicked = new UnityEvent();

		// Token: 0x04003825 RID: 14373
		private bool settingSource;

		// Token: 0x04003826 RID: 14374
		private bool settingDestination;
	}
}
