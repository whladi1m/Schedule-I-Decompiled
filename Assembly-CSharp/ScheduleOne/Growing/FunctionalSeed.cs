using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x02000863 RID: 2147
	public class FunctionalSeed : MonoBehaviour
	{
		// Token: 0x06003A5A RID: 14938 RVA: 0x000F5B52 File Offset: 0x000F3D52
		public void TriggerExit(Collider other)
		{
			if (other == this.SeedCollider && this.onSeedExitVial != null)
			{
				this.onSeedExitVial();
			}
		}

		// Token: 0x04002A2F RID: 10799
		public Action onSeedExitVial;

		// Token: 0x04002A30 RID: 10800
		public Draggable Vial;

		// Token: 0x04002A31 RID: 10801
		public Collider SeedBlocker;

		// Token: 0x04002A32 RID: 10802
		public VialCap Cap;

		// Token: 0x04002A33 RID: 10803
		public Collider SeedCollider;

		// Token: 0x04002A34 RID: 10804
		public Rigidbody SeedRigidbody;

		// Token: 0x04002A35 RID: 10805
		public TrashItem TrashPrefab;
	}
}
