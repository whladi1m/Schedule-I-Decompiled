using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x0200099F RID: 2463
	public class BalanceDisplay : MonoBehaviour
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x00116FAD File Offset: 0x001151AD
		// (set) Token: 0x06004290 RID: 17040 RVA: 0x00116FB5 File Offset: 0x001151B5
		public bool active { get; protected set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06004291 RID: 17041 RVA: 0x00116FBE File Offset: 0x001151BE
		// (set) Token: 0x06004292 RID: 17042 RVA: 0x00116FC6 File Offset: 0x001151C6
		public float timeSinceActiveSet { get; protected set; }

		// Token: 0x06004293 RID: 17043 RVA: 0x00116FD0 File Offset: 0x001151D0
		protected virtual void Update()
		{
			this.timeSinceActiveSet += Time.deltaTime;
			if (this.timeSinceActiveSet > 3f)
			{
				this.active = false;
			}
			if (this.Group != null)
			{
				this.Group.alpha = Mathf.MoveTowards(this.Group.alpha, this.active ? 1f : 0f, Time.deltaTime / 0.25f);
			}
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x0011704B File Offset: 0x0011524B
		public void SetBalance(float balance)
		{
			this.BalanceLabel.text = MoneyManager.FormatAmount(balance, false, false);
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00117060 File Offset: 0x00115260
		public void Show()
		{
			this.active = true;
			this.timeSinceActiveSet = 0f;
		}

		// Token: 0x04003080 RID: 12416
		public const float RESIDUAL_TIME = 3f;

		// Token: 0x04003081 RID: 12417
		public const float FADE_TIME = 0.25f;

		// Token: 0x04003082 RID: 12418
		[Header("References")]
		public CanvasGroup Group;

		// Token: 0x04003083 RID: 12419
		public TextMeshProUGUI BalanceLabel;
	}
}
