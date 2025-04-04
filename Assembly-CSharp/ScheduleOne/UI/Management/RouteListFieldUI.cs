using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE5 RID: 2789
	public class RouteListFieldUI : MonoBehaviour
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06004A9F RID: 19103 RVA: 0x0013902C File Offset: 0x0013722C
		// (set) Token: 0x06004AA0 RID: 19104 RVA: 0x00139034 File Offset: 0x00137234
		public List<RouteListField> Fields { get; protected set; } = new List<RouteListField>();

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00139040 File Offset: 0x00137240
		private void Start()
		{
			this.FieldLabel.text = this.FieldText;
			for (int i = 0; i < this.RouteEntries.Length; i++)
			{
				RouteEntryUI entry = this.RouteEntries[i];
				this.RouteEntries[i].onDeleteClicked.AddListener(delegate()
				{
					this.EntryDeleteClicked(entry);
				});
			}
			this.AddButton.onClick.AddListener(new UnityAction(this.AddClicked));
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x001390C8 File Offset: 0x001372C8
		public void Bind(List<RouteListField> field)
		{
			this.Fields = new List<RouteListField>();
			this.Fields.AddRange(field);
			this.Refresh(this.Fields[0].Routes);
			this.Fields[0].onListChanged.AddListener(new UnityAction<List<AdvancedTransitRoute>>(this.Refresh));
			this.MultiEditBlocker.gameObject.SetActive(this.Fields.Count > 1);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x00139144 File Offset: 0x00137344
		private void Refresh(List<AdvancedTransitRoute> newVal)
		{
			int num = 0;
			for (int i = 0; i < this.RouteEntries.Length; i++)
			{
				if (newVal.Count > i)
				{
					num++;
					this.RouteEntries[i].AssignRoute(newVal[i]);
					this.RouteEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.RouteEntries[i].ClearRoute();
					this.RouteEntries[i].gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < newVal.Count; j++)
			{
				AdvancedTransitRoute advancedTransitRoute = newVal[j];
				advancedTransitRoute.onSourceChange = (Action<ITransitEntity>)Delegate.Remove(advancedTransitRoute.onSourceChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute2 = newVal[j];
				advancedTransitRoute2.onDestinationChange = (Action<ITransitEntity>)Delegate.Remove(advancedTransitRoute2.onDestinationChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute3 = newVal[j];
				advancedTransitRoute3.onSourceChange = (Action<ITransitEntity>)Delegate.Combine(advancedTransitRoute3.onSourceChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute4 = newVal[j];
				advancedTransitRoute4.onDestinationChange = (Action<ITransitEntity>)Delegate.Combine(advancedTransitRoute4.onDestinationChange, new Action<ITransitEntity>(this.RouteChanged));
			}
			this.AddButton.gameObject.SetActive(num < this.Fields[0].MaxRoutes);
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00139297 File Offset: 0x00137497
		private void EntryDeleteClicked(RouteEntryUI entry)
		{
			this.Fields[0].RemoveItem(entry.AssignedRoute);
			entry.ClearRoute();
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x001392B6 File Offset: 0x001374B6
		private void AddClicked()
		{
			this.Fields[0].AddItem(new AdvancedTransitRoute(null, null));
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x001392D0 File Offset: 0x001374D0
		private void RouteChanged(ITransitEntity newEntity)
		{
			this.Fields[0].Replicate();
		}

		// Token: 0x04003828 RID: 14376
		[Header("References")]
		public string FieldText = "Routes";

		// Token: 0x04003829 RID: 14377
		public TextMeshProUGUI FieldLabel;

		// Token: 0x0400382A RID: 14378
		public RouteEntryUI[] RouteEntries;

		// Token: 0x0400382B RID: 14379
		public RectTransform MultiEditBlocker;

		// Token: 0x0400382C RID: 14380
		public Button AddButton;
	}
}
