using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001CB RID: 459
	[ExecuteInEditMode]
	public class FollowCamera : MonoBehaviour
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x0002D444 File Offset: 0x0002B644
		private void Update()
		{
			Camera main;
			if (this.followCamera != null)
			{
				main = this.followCamera;
			}
			else
			{
				main = Camera.main;
			}
			if (main == null)
			{
				return;
			}
			base.transform.position = main.transform.TransformPoint(this.offset);
		}

		// Token: 0x04000B2F RID: 2863
		public Camera followCamera;

		// Token: 0x04000B30 RID: 2864
		public Vector3 offset = Vector3.zero;
	}
}
