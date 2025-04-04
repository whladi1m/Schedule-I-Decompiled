using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000365 RID: 869
	public class LoadEventTransmitter : MonoBehaviour
	{
		// Token: 0x060013AE RID: 5038 RVA: 0x00057B90 File Offset: 0x00055D90
		private void Start()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoadComplete));
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00057BAD File Offset: 0x00055DAD
		private void OnLoadComplete()
		{
			if (this.onLoadComplete != null)
			{
				this.onLoadComplete.Invoke();
			}
		}

		// Token: 0x040012AC RID: 4780
		public UnityEvent onLoadComplete;
	}
}
