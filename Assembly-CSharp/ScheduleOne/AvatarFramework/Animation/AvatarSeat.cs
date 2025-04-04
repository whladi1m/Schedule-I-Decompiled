using System;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x0200097E RID: 2430
	public class AvatarSeat : MonoBehaviour
	{
		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x001153A3 File Offset: 0x001135A3
		public bool IsOccupied
		{
			get
			{
				return this.Occupant != null;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x001153B1 File Offset: 0x001135B1
		// (set) Token: 0x06004204 RID: 16900 RVA: 0x001153B9 File Offset: 0x001135B9
		public NPC Occupant { get; protected set; }

		// Token: 0x06004205 RID: 16901 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x001153C2 File Offset: 0x001135C2
		public void SetOccupant(NPC npc)
		{
			if (npc != null && this.IsOccupied)
			{
				Debug.LogWarning("Seat is already occupied");
				return;
			}
			this.Occupant = npc;
		}

		// Token: 0x0400300E RID: 12302
		public Transform SittingPoint;

		// Token: 0x0400300F RID: 12303
		public Transform AccessPoint;
	}
}
