using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D2 RID: 1746
	public class LookAt : MonoBehaviour
	{
		// Token: 0x06002F8A RID: 12170 RVA: 0x000C6333 File Offset: 0x000C4533
		private void LateUpdate()
		{
			if (this.Target != null)
			{
				base.transform.LookAt(this.Target);
			}
		}

		// Token: 0x040021EF RID: 8687
		public Transform Target;
	}
}
