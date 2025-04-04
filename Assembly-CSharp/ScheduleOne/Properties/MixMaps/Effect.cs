using System;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200032B RID: 811
	public class Effect : MonoBehaviour
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0004D8FF File Offset: 0x0004BAFF
		public Vector2 Position
		{
			get
			{
				return new Vector2(base.transform.position.x, base.transform.position.z);
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0004D926 File Offset: 0x0004BB26
		public void OnValidate()
		{
			if (this.Property == null)
			{
				return;
			}
			base.gameObject.name = this.Property.Name;
		}

		// Token: 0x0400115C RID: 4444
		public Property Property;

		// Token: 0x0400115D RID: 4445
		[Range(0.05f, 3f)]
		public float Radius = 0.5f;
	}
}
