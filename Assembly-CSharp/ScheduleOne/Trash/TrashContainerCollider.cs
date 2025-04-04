using System;
using UnityEngine;

namespace ScheduleOne.Trash
{
	// Token: 0x02000813 RID: 2067
	[RequireComponent(typeof(Rigidbody))]
	public class TrashContainerCollider : MonoBehaviour
	{
		// Token: 0x060038BB RID: 14523 RVA: 0x000F0037 File Offset: 0x000EE237
		public void OnTriggerEnter(Collider other)
		{
			this.Container.TriggerEnter(other);
		}

		// Token: 0x04002924 RID: 10532
		public TrashContainer Container;
	}
}
