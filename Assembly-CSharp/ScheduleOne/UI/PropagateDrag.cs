using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A16 RID: 2582
	[RequireComponent(typeof(EventTrigger))]
	public class PropagateDrag : MonoBehaviour
	{
		// Token: 0x060045AC RID: 17836 RVA: 0x0012420C File Offset: 0x0012240C
		private void Start()
		{
			if (this.ScrollView == null)
			{
				this.ScrollView = base.GetComponentInParent<ScrollRect>();
			}
			if (this.ScrollView == null)
			{
				return;
			}
			EventTrigger component = base.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			EventTrigger.Entry entry4 = new EventTrigger.Entry();
			EventTrigger.Entry entry5 = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.BeginDrag;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.ScrollView.OnBeginDrag((PointerEventData)data);
			});
			component.triggers.Add(entry);
			entry2.eventID = EventTriggerType.Drag;
			entry2.callback.AddListener(delegate(BaseEventData data)
			{
				this.ScrollView.OnDrag((PointerEventData)data);
			});
			component.triggers.Add(entry2);
			entry3.eventID = EventTriggerType.EndDrag;
			entry3.callback.AddListener(delegate(BaseEventData data)
			{
				this.ScrollView.OnEndDrag((PointerEventData)data);
			});
			component.triggers.Add(entry3);
			entry4.eventID = EventTriggerType.InitializePotentialDrag;
			entry4.callback.AddListener(delegate(BaseEventData data)
			{
				this.ScrollView.OnInitializePotentialDrag((PointerEventData)data);
			});
			component.triggers.Add(entry4);
			entry5.eventID = EventTriggerType.Scroll;
			entry5.callback.AddListener(delegate(BaseEventData data)
			{
				this.ScrollView.OnScroll((PointerEventData)data);
			});
			component.triggers.Add(entry5);
		}

		// Token: 0x04003389 RID: 13193
		public ScrollRect ScrollView;
	}
}
