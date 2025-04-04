using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084B RID: 2123
	public class MonoBehaviourEvents : MonoBehaviour
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x000F4751 File Offset: 0x000F2951
		private void Awake()
		{
			UnityEvent unityEvent = this.onAwake;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x000F4763 File Offset: 0x000F2963
		private void Start()
		{
			UnityEvent unityEvent = this.onStart;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x000F4775 File Offset: 0x000F2975
		private void Update()
		{
			UnityEvent unityEvent = this.onUpdate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x040029E0 RID: 10720
		public UnityEvent onAwake;

		// Token: 0x040029E1 RID: 10721
		public UnityEvent onStart;

		// Token: 0x040029E2 RID: 10722
		public UnityEvent onUpdate;
	}
}
