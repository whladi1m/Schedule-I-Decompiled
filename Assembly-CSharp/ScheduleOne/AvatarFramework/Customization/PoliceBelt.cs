using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000981 RID: 2433
	public class PoliceBelt : Accessory
	{
		// Token: 0x0600420E RID: 16910 RVA: 0x001154B6 File Offset: 0x001136B6
		public void SetBatonVisible(bool vis)
		{
			this.BatonObject.gameObject.SetActive(vis);
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x001154C9 File Offset: 0x001136C9
		public void SetTaserVisible(bool vis)
		{
			this.TaserObject.gameObject.SetActive(vis);
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x001154DC File Offset: 0x001136DC
		public void SetGunVisible(bool vis)
		{
			this.GunObject.gameObject.SetActive(vis);
		}

		// Token: 0x04003013 RID: 12307
		[Header("References")]
		[SerializeField]
		protected GameObject BatonObject;

		// Token: 0x04003014 RID: 12308
		[SerializeField]
		protected GameObject TaserObject;

		// Token: 0x04003015 RID: 12309
		[SerializeField]
		protected GameObject GunObject;
	}
}
