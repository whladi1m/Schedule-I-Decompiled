using System;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BE0 RID: 3040
	public class Spin : MonoBehaviour
	{
		// Token: 0x06005547 RID: 21831 RVA: 0x00166E8F File Offset: 0x0016508F
		private void Update()
		{
			base.transform.Rotate(this.Axis, this.Speed * Time.deltaTime, Space.Self);
		}

		// Token: 0x04003F47 RID: 16199
		public Vector3 Axis;

		// Token: 0x04003F48 RID: 16200
		public float Speed;
	}
}
