using System;
using ScheduleOne.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000B5A RID: 2906
	public class FI_Base : MonoBehaviour
	{
		// Token: 0x06004D39 RID: 19769 RVA: 0x00146209 File Offset: 0x00144409
		public virtual void Initialize(Feature _feature)
		{
			this.feature = _feature;
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x00146212 File Offset: 0x00144412
		public virtual void Close()
		{
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04003A80 RID: 14976
		protected Feature feature;

		// Token: 0x04003A81 RID: 14977
		public UnityEvent onClose;
	}
}
