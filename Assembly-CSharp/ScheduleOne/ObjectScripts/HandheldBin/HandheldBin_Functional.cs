using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.HandheldBin
{
	// Token: 0x02000BCD RID: 3021
	public class HandheldBin_Functional : MonoBehaviour
	{
		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x00165466 File Offset: 0x00163666
		// (set) Token: 0x060054DE RID: 21726 RVA: 0x0016546E File Offset: 0x0016366E
		public float fillLevel { get; protected set; }

		// Token: 0x060054DF RID: 21727 RVA: 0x00165477 File Offset: 0x00163677
		protected virtual void Awake()
		{
			this.UpdateTrashVisuals();
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x00165477 File Offset: 0x00163677
		public void SetAmount(float amount)
		{
			this.UpdateTrashVisuals();
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x0016547F File Offset: 0x0016367F
		protected virtual void UpdateTrashVisuals()
		{
			this.trash.gameObject.SetActive(this.fillLevel > 0f);
		}

		// Token: 0x04003EDF RID: 16095
		[Header("References")]
		public Transform trash;

		// Token: 0x04003EE0 RID: 16096
		[Header("Settings")]
		public float trash_MinY;

		// Token: 0x04003EE1 RID: 16097
		public float trash_MaxY;
	}
}
