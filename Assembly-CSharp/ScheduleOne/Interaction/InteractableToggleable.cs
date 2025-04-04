using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x02000607 RID: 1543
	public class InteractableToggleable : MonoBehaviour
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002894 RID: 10388 RVA: 0x000A72DC File Offset: 0x000A54DC
		// (set) Token: 0x06002895 RID: 10389 RVA: 0x000A72E4 File Offset: 0x000A54E4
		public bool IsActivated { get; private set; }

		// Token: 0x06002896 RID: 10390 RVA: 0x000A72ED File Offset: 0x000A54ED
		public void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000A7328 File Offset: 0x000A5528
		public void Hovered()
		{
			if (Time.time - this.lastActivated < this.CoolDown)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage(this.IsActivated ? this.DeactivateMessage : this.ActivateMessage);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000A7383 File Offset: 0x000A5583
		public void Interacted()
		{
			this.Toggle();
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000A738C File Offset: 0x000A558C
		public void Toggle()
		{
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.onToggle != null)
			{
				this.onToggle.Invoke();
			}
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000A73E8 File Offset: 0x000A55E8
		public void SetState(bool activated)
		{
			if (this.IsActivated == activated)
			{
				return;
			}
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000A7438 File Offset: 0x000A5638
		public void PoliceDetected()
		{
			if (!this.IsActivated)
			{
				this.Toggle();
			}
		}

		// Token: 0x04001DBB RID: 7611
		public string ActivateMessage = "Activate";

		// Token: 0x04001DBC RID: 7612
		public string DeactivateMessage = "Deactivate";

		// Token: 0x04001DBD RID: 7613
		public float CoolDown;

		// Token: 0x04001DBE RID: 7614
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04001DBF RID: 7615
		public UnityEvent onToggle = new UnityEvent();

		// Token: 0x04001DC0 RID: 7616
		public UnityEvent onActivate = new UnityEvent();

		// Token: 0x04001DC1 RID: 7617
		public UnityEvent onDeactivate = new UnityEvent();

		// Token: 0x04001DC2 RID: 7618
		private float lastActivated;
	}
}
