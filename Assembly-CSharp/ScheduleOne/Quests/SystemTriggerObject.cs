using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x02000300 RID: 768
	public class SystemTriggerObject : MonoBehaviour
	{
		// Token: 0x06001105 RID: 4357 RVA: 0x0004BDFF File Offset: 0x00049FFF
		[Button]
		public void Trigger()
		{
			this.SystemTrigger.Trigger();
		}

		// Token: 0x0400112D RID: 4397
		public SystemTrigger SystemTrigger;
	}
}
