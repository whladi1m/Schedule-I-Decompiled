using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000856 RID: 2134
	public class RigidbodyEventBroadcaster : MonoBehaviour
	{
		// Token: 0x06003A32 RID: 14898 RVA: 0x000F502A File Offset: 0x000F322A
		private void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.Invoke(other);
			}
		}

		// Token: 0x04002A01 RID: 10753
		public UnityEvent<Collider> onTriggerEnter;
	}
}
