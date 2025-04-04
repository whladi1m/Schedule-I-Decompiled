using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000986 RID: 2438
	public class ACEntry : MonoBehaviour
	{
		// Token: 0x0600421E RID: 16926 RVA: 0x001156C8 File Offset: 0x001138C8
		private void Awake()
		{
			if (this.DevOnly && !Application.isEditor)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04003018 RID: 12312
		public bool DevOnly;
	}
}
