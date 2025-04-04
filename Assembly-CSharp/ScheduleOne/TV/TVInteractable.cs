using System;
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002A5 RID: 677
	public class TVInteractable : MonoBehaviour
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x0003F394 File Offset: 0x0003D594
		private void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0003F3CE File Offset: 0x0003D5CE
		private void Hovered()
		{
			if (this.Interface.CanOpen())
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Use TV");
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0003F406 File Offset: 0x0003D606
		private void Interacted()
		{
			if (this.Interface.CanOpen())
			{
				this.Interface.Open();
			}
		}

		// Token: 0x04000ED7 RID: 3799
		public InteractableObject IntObj;

		// Token: 0x04000ED8 RID: 3800
		public TVInterface Interface;
	}
}
