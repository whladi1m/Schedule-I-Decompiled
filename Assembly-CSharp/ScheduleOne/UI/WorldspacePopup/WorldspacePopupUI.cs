using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000A3D RID: 2621
	public class WorldspacePopupUI : MonoBehaviour
	{
		// Token: 0x060046A0 RID: 18080 RVA: 0x00127997 File Offset: 0x00125B97
		public void SetFill(float fill)
		{
			this.FillImage.fillAmount = fill;
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x001279A5 File Offset: 0x00125BA5
		public void Destroy()
		{
			if (this.onDestroyed != null)
			{
				this.onDestroyed.Invoke();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0400345C RID: 13404
		[HideInInspector]
		public WorldspacePopup Popup;

		// Token: 0x0400345D RID: 13405
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x0400345E RID: 13406
		public Image FillImage;

		// Token: 0x0400345F RID: 13407
		public UnityEvent onDestroyed;
	}
}
