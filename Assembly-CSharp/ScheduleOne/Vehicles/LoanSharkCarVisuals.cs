using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B1 RID: 1969
	public class LoanSharkCarVisuals : MonoBehaviour
	{
		// Token: 0x06003602 RID: 13826 RVA: 0x000E349D File Offset: 0x000E169D
		private void Awake()
		{
			this.Note.gameObject.SetActive(false);
			this.BulletHoleDecals.gameObject.SetActive(false);
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000E34C1 File Offset: 0x000E16C1
		public void Configure(bool enabled, bool noteVisible)
		{
			this.Note.SetActive(noteVisible);
			this.BulletHoleDecals.SetActive(enabled);
		}

		// Token: 0x040026CE RID: 9934
		public GameObject Note;

		// Token: 0x040026CF RID: 9935
		public GameObject BulletHoleDecals;
	}
}
