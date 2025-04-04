using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200088A RID: 2186
	public class PalletSlotDetector : MonoBehaviour
	{
		// Token: 0x06003B55 RID: 15189 RVA: 0x000F9F60 File Offset: 0x000F8160
		protected virtual void OnTriggerStay(Collider other)
		{
			this.pallet.TriggerStay(other);
		}

		// Token: 0x04002AFD RID: 11005
		public Pallet pallet;
	}
}
