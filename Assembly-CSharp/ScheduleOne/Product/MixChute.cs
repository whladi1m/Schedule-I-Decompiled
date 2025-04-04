using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008F6 RID: 2294
	public class MixChute : MonoBehaviour
	{
		// Token: 0x06003E39 RID: 15929 RVA: 0x00106B5C File Offset: 0x00104D5C
		private void Update()
		{
			this.UpdateDoor();
			this.IntObj.gameObject.SetActive(!NetworkSingleton<ProductManager>.Instance.IsMixComplete);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00106B84 File Offset: 0x00104D84
		private void UpdateDoor()
		{
			bool flag = false;
			if (NetworkSingleton<ProductManager>.Instance.IsMixComplete && NetworkSingleton<ProductManager>.Instance.CurrentMixOperation != null)
			{
				flag = true;
			}
			else if (Singleton<CreateMixInterface>.Instance.IsOpen)
			{
				flag = true;
			}
			if (flag != this.isDoorOpen)
			{
				this.SetDoorOpen(flag);
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00106BD0 File Offset: 0x00104DD0
		public void Hovered()
		{
			if (!NetworkSingleton<ProductManager>.Instance.IsMixComplete)
			{
				if (NetworkSingleton<ProductManager>.Instance.IsMixingInProgress)
				{
					this.IntObj.SetMessage("Mix will be ready tomorrow");
					this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
					return;
				}
				this.IntObj.SetMessage("Create new mix");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			}
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00106C2E File Offset: 0x00104E2E
		public void Interacted()
		{
			if (!NetworkSingleton<ProductManager>.Instance.IsMixComplete && !NetworkSingleton<ProductManager>.Instance.IsMixingInProgress)
			{
				Singleton<CreateMixInterface>.Instance.Open();
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00106C52 File Offset: 0x00104E52
		public void SetDoorOpen(bool isOpen)
		{
			this.isDoorOpen = isOpen;
			this.DoorAnim.Play(this.isDoorOpen ? "Cabin flap open" : "Cabin flap close");
		}

		// Token: 0x04002CB7 RID: 11447
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04002CB8 RID: 11448
		public Animation DoorAnim;

		// Token: 0x04002CB9 RID: 11449
		private bool isDoorOpen;
	}
}
