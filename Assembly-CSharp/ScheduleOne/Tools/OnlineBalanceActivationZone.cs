using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084D RID: 2125
	public class OnlineBalanceActivationZone : MonoBehaviour
	{
		// Token: 0x06003A06 RID: 14854 RVA: 0x000F47E9 File Offset: 0x000F29E9
		private void Awake()
		{
			this.collider = base.GetComponent<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x000F480C File Offset: 0x000F2A0C
		private void UpdateCollider()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			this.collider.enabled = (num < 20f);
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x000F4840 File Offset: 0x000F2A40
		private void OnTriggerStay(Collider other)
		{
			if (this.exclude.Contains(other))
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x040029E6 RID: 10726
		public const float ActivationDistance = 20f;

		// Token: 0x040029E7 RID: 10727
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x040029E8 RID: 10728
		private Collider collider;
	}
}
