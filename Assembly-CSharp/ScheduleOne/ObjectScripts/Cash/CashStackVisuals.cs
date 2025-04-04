using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000BD4 RID: 3028
	public class CashStackVisuals : MonoBehaviour
	{
		// Token: 0x060054FF RID: 21759 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x00165A30 File Offset: 0x00163C30
		public void ShowAmount(float amount)
		{
			this.Visuals_Over100.SetActive(amount >= 100f);
			this.Visuals_Under100.SetActive(amount < 100f);
			if (amount >= 100f)
			{
				int num = Mathf.RoundToInt(amount / 100f);
				for (int i = 0; i < this.Bills.Length; i++)
				{
					this.Bills[i].SetActive(num > i);
				}
				return;
			}
			int num2 = Mathf.Clamp(Mathf.RoundToInt(amount / 10f), 0, 10);
			for (int j = 0; j < this.Notes.Length; j++)
			{
				this.Notes[j].SetActive(num2 > j);
			}
		}

		// Token: 0x04003EFE RID: 16126
		public const float MAX_AMOUNT = 1000f;

		// Token: 0x04003EFF RID: 16127
		[Header("References")]
		public GameObject Visuals_Under100;

		// Token: 0x04003F00 RID: 16128
		public GameObject[] Notes;

		// Token: 0x04003F01 RID: 16129
		public GameObject Visuals_Over100;

		// Token: 0x04003F02 RID: 16130
		public GameObject[] Bills;
	}
}
