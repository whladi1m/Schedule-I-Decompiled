using System;
using ScheduleOne.ConstructableScripts;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200071E RID: 1822
	public class ConstructUpdate_Base : MonoBehaviour
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x000CC3D3 File Offset: 0x000CA5D3
		public bool isMoving
		{
			get
			{
				return this.MovedConstructable != null;
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void LateUpdate()
		{
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000CC3E1 File Offset: 0x000CA5E1
		public virtual void ConstructionStop()
		{
			if (this.MovedConstructable != null)
			{
				this.MovedConstructable.RestoreVisibility();
			}
		}

		// Token: 0x0400233A RID: 9018
		public Constructable_GridBased MovedConstructable;
	}
}
