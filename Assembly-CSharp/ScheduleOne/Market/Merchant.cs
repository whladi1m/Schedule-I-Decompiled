using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x02000551 RID: 1361
	public class Merchant : MonoBehaviour
	{
		// Token: 0x06002169 RID: 8553 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00089950 File Offset: 0x00087B50
		public void Hovered()
		{
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime))
			{
				this.intObj.SetMessage("Browse " + this.shopName);
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage("Closed");
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Interacted()
		{
		}

		// Token: 0x040019AE RID: 6574
		[Header("Settings")]
		[SerializeField]
		protected string shopName = "Store";

		// Token: 0x040019AF RID: 6575
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x040019B0 RID: 6576
		[SerializeField]
		protected int closeTime = 1800;

		// Token: 0x040019B1 RID: 6577
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;
	}
}
