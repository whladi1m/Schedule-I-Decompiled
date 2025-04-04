using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000953 RID: 2387
	public class Hair : Accessory
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x0011162F File Offset: 0x0010F82F
		// (set) Token: 0x06004116 RID: 16662 RVA: 0x00111637 File Offset: 0x0010F837
		public bool BlockedByHat { get; protected set; }

		// Token: 0x06004117 RID: 16663 RVA: 0x00111640 File Offset: 0x0010F840
		public void SetBlockedByHat(bool blocked)
		{
			this.BlockedByHat = blocked;
			if (blocked)
			{
				this.BlockHair();
				return;
			}
			this.UnBlockHair();
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0011165C File Offset: 0x0010F85C
		protected virtual void BlockHair()
		{
			GameObject[] array = this.hairToHide;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x00111688 File Offset: 0x0010F888
		protected virtual void UnBlockHair()
		{
			GameObject[] array = this.hairToHide;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}

		// Token: 0x04002EF5 RID: 12021
		[SerializeField]
		private GameObject[] hairToHide;
	}
}
