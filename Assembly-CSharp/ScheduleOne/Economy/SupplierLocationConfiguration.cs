using System;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x0200066E RID: 1646
	public class SupplierLocationConfiguration : MonoBehaviour
	{
		// Token: 0x06002D88 RID: 11656 RVA: 0x000BEE6A File Offset: 0x000BD06A
		public void Activate()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400206F RID: 8303
		public string SupplierID;
	}
}
