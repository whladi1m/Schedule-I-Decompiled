using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000858 RID: 2136
	public class SafeBalanceActivationZone : MonoBehaviour
	{
		// Token: 0x06003A36 RID: 14902 RVA: 0x000F50E6 File Offset: 0x000F32E6
		private void Awake()
		{
			this.colliders = base.GetComponentsInChildren<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
			base.InvokeRepeating("Activate", 0f, 0.25f);
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x000F5120 File Offset: 0x000F3320
		private void UpdateCollider()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			Collider[] array = this.colliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = (num < 30f);
			}
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x000F5166 File Offset: 0x000F3366
		private void Activate()
		{
			this.active = true;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x000F5170 File Offset: 0x000F3370
		private void OnTriggerStay(Collider other)
		{
			if (!this.active)
			{
				return;
			}
			this.active = true;
			if (this.exclude.Contains(other))
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				Singleton<HUD>.Instance.SafeBalanceDisplay.SetBalance(this.Safe.GetCash());
				Singleton<HUD>.Instance.SafeBalanceDisplay.Show();
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x04002A05 RID: 10757
		public const float ActivationDistance = 30f;

		// Token: 0x04002A06 RID: 10758
		public Safe Safe;

		// Token: 0x04002A07 RID: 10759
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x04002A08 RID: 10760
		private Collider[] colliders;

		// Token: 0x04002A09 RID: 10761
		private bool active;
	}
}
