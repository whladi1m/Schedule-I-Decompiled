using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009B2 RID: 2482
	public class CrosshairText : MonoBehaviour
	{
		// Token: 0x06004311 RID: 17169 RVA: 0x0011920E File Offset: 0x0011740E
		private void Awake()
		{
			this.Hide();
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x00119216 File Offset: 0x00117416
		private void LateUpdate()
		{
			if (!this.setThisFrame)
			{
				this.Label.enabled = false;
			}
			this.setThisFrame = false;
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00119234 File Offset: 0x00117434
		public void Show(string text, Color col = default(Color))
		{
			this.setThisFrame = true;
			this.Label.color = ((col != default(Color)) ? col : Color.white);
			this.Label.text = text;
			this.Label.enabled = true;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00119284 File Offset: 0x00117484
		public void Hide()
		{
			this.Label.enabled = false;
		}

		// Token: 0x040030F5 RID: 12533
		public TextMeshProUGUI Label;

		// Token: 0x040030F6 RID: 12534
		private bool setThisFrame;
	}
}
