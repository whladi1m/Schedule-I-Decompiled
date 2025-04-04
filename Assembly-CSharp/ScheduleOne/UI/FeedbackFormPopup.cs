using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009C6 RID: 2502
	public class FeedbackFormPopup : MonoBehaviour
	{
		// Token: 0x060043AA RID: 17322 RVA: 0x0011BCB4 File Offset: 0x00119EB4
		public void Open(string text)
		{
			if (this.Label != null)
			{
				this.Label.text = text;
			}
			base.gameObject.SetActive(true);
			this.closeTime = Time.unscaledTime + 4f;
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0011BCED File Offset: 0x00119EED
		private void Update()
		{
			if (this.AutoClose && Time.unscaledTime > this.closeTime)
			{
				this.Close();
			}
		}

		// Token: 0x04003176 RID: 12662
		public TextMeshProUGUI Label;

		// Token: 0x04003177 RID: 12663
		public bool AutoClose = true;

		// Token: 0x04003178 RID: 12664
		private float closeTime;
	}
}
