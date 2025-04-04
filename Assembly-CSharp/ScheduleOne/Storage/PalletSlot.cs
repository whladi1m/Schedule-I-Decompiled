using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000889 RID: 2185
	public class PalletSlot : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x000F9EF1 File Offset: 0x000F80F1
		// (set) Token: 0x06003B4F RID: 15183 RVA: 0x000F9EF9 File Offset: 0x000F80F9
		public Guid GUID { get; protected set; }

		// Token: 0x06003B50 RID: 15184 RVA: 0x000F9F02 File Offset: 0x000F8102
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003B51 RID: 15185 RVA: 0x000F9F11 File Offset: 0x000F8111
		// (set) Token: 0x06003B52 RID: 15186 RVA: 0x000F9F19 File Offset: 0x000F8119
		public Pallet occupant { get; protected set; }

		// Token: 0x06003B53 RID: 15187 RVA: 0x000F9F22 File Offset: 0x000F8122
		public void SetOccupant(Pallet _occupant)
		{
			this.occupant = _occupant;
			if (this.occupant != null)
			{
				if (this.onPalletAdded != null)
				{
					this.onPalletAdded();
					return;
				}
			}
			else if (this.onPalletRemoved != null)
			{
				this.onPalletRemoved();
			}
		}

		// Token: 0x04002AFB RID: 11003
		public Action onPalletAdded;

		// Token: 0x04002AFC RID: 11004
		public Action onPalletRemoved;
	}
}
