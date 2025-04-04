using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000859 RID: 2137
	public class SetLayerOnAwake : MonoBehaviour
	{
		// Token: 0x06003A3B RID: 14907 RVA: 0x000F51FD File Offset: 0x000F33FD
		private void Awake()
		{
			base.gameObject.layer = this.Layer.value;
		}

		// Token: 0x04002A0A RID: 10762
		public LayerMask Layer;
	}
}
