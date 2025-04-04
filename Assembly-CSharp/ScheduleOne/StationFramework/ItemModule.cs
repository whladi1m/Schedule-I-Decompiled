using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008AE RID: 2222
	public abstract class ItemModule : MonoBehaviour
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003C75 RID: 15477 RVA: 0x000FE1D6 File Offset: 0x000FC3D6
		// (set) Token: 0x06003C76 RID: 15478 RVA: 0x000FE1DE File Offset: 0x000FC3DE
		public StationItem Item { get; protected set; }

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06003C77 RID: 15479 RVA: 0x000FE1E7 File Offset: 0x000FC3E7
		// (set) Token: 0x06003C78 RID: 15480 RVA: 0x000FE1EF File Offset: 0x000FC3EF
		public bool IsModuleActive { get; protected set; }

		// Token: 0x06003C79 RID: 15481 RVA: 0x000FE1F8 File Offset: 0x000FC3F8
		public virtual void ActivateModule(StationItem item)
		{
			this.IsModuleActive = true;
			this.Item = item;
		}
	}
}
