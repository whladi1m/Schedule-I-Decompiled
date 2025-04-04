using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B24 RID: 2852
	public class HandoverScreenPriceSelector : MonoBehaviour
	{
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x00140124 File Offset: 0x0013E324
		// (set) Token: 0x06004BEF RID: 19439 RVA: 0x0014012C File Offset: 0x0013E32C
		public float Price { get; private set; } = 1f;

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00140138 File Offset: 0x0013E338
		public void SetPrice(float price)
		{
			this.Price = Mathf.Clamp(price, 1f, 9999f);
			this.InputField.SetTextWithoutNotify(this.Price.ToString());
			if (this.onPriceChanged != null)
			{
				this.onPriceChanged.Invoke();
			}
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x00140187 File Offset: 0x0013E387
		public void RefreshPrice()
		{
			this.OnPriceInputChanged(this.InputField.text);
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x0014019C File Offset: 0x0013E39C
		public void OnPriceInputChanged(string value)
		{
			float value2;
			if (float.TryParse(value, out value2))
			{
				this.Price = Mathf.Clamp(value2, 1f, 9999f);
			}
			this.InputField.SetTextWithoutNotify(this.Price.ToString());
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x001401E2 File Offset: 0x0013E3E2
		public void ChangeAmount(float change)
		{
			this.SetPrice(this.Price + change);
		}

		// Token: 0x0400394D RID: 14669
		public const float MinPrice = 1f;

		// Token: 0x0400394E RID: 14670
		public const float MaxPrice = 9999f;

		// Token: 0x0400394F RID: 14671
		public InputField InputField;

		// Token: 0x04003951 RID: 14673
		public UnityEvent onPriceChanged;
	}
}
