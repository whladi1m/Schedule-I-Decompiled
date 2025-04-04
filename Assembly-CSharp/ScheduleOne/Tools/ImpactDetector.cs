using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000849 RID: 2121
	public class ImpactDetector : MonoBehaviour
	{
		// Token: 0x060039FB RID: 14843 RVA: 0x000F46E2 File Offset: 0x000F28E2
		private void OnCollisionEnter(Collision collision)
		{
			this.onImpact.Invoke();
			if (this.DestroyScriptOnImpact)
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x040029DC RID: 10716
		public bool DestroyScriptOnImpact;

		// Token: 0x040029DD RID: 10717
		public UnityEvent onImpact = new UnityEvent();
	}
}
