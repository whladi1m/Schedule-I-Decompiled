using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000839 RID: 2105
	public class EditionConditionalObject : MonoBehaviour
	{
		// Token: 0x060039C4 RID: 14788 RVA: 0x000F40F5 File Offset: 0x000F22F5
		private void Awake()
		{
			if (this.type == EditionConditionalObject.EType.ActiveInDemo)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040029BB RID: 10683
		public EditionConditionalObject.EType type;

		// Token: 0x0200083A RID: 2106
		public enum EType
		{
			// Token: 0x040029BD RID: 10685
			ActiveInDemo,
			// Token: 0x040029BE RID: 10686
			ActiveInFullGame
		}
	}
}
