using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006C8 RID: 1736
	public class CopyPosition : MonoBehaviour
	{
		// Token: 0x06002F53 RID: 12115 RVA: 0x000C597D File Offset: 0x000C3B7D
		private void LateUpdate()
		{
			base.transform.position = this.ToCopy.position;
		}

		// Token: 0x040021CC RID: 8652
		public Transform ToCopy;
	}
}
