using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x02000553 RID: 1363
	public class VendorZone : MonoBehaviour
	{
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000899E2 File Offset: 0x00087BE2
		public bool isOpen
		{
			get
			{
				return NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime);
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000899FA File Offset: 0x00087BFA
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPassed));
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00089A22 File Offset: 0x00087C22
		private void MinPassed()
		{
			if (this.isOpen)
			{
				this.SetDoorsActive(false);
				return;
			}
			if (!this.IsPlayerWithinVendorZone())
			{
				this.SetDoorsActive(true);
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00089A44 File Offset: 0x00087C44
		private bool IsPlayerWithinVendorZone()
		{
			return this.zoneCollider.bounds.Contains(PlayerSingleton<PlayerMovement>.Instance.transform.position);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00089A74 File Offset: 0x00087C74
		private void SetDoorsActive(bool a)
		{
			for (int i = 0; i < this.doors.Count; i++)
			{
				this.doors[i].SetActive(a);
			}
		}

		// Token: 0x040019B2 RID: 6578
		[Header("References")]
		[SerializeField]
		protected BoxCollider zoneCollider;

		// Token: 0x040019B3 RID: 6579
		[SerializeField]
		protected List<GameObject> doors = new List<GameObject>();

		// Token: 0x040019B4 RID: 6580
		[Header("Settings")]
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x040019B5 RID: 6581
		[SerializeField]
		protected int closeTime = 1800;
	}
}
