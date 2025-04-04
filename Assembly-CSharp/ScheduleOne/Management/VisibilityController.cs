using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x0200057C RID: 1404
	public class VisibilityController : MonoBehaviour
	{
		// Token: 0x06002328 RID: 9000 RVA: 0x0008FD1D File Offset: 0x0008DF1D
		private void Start()
		{
			bool flag = this.visibleOnlyInFullscreen;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0008FD26 File Offset: 0x0008DF26
		private void OnEnterFullScreen()
		{
			if (this.visibleOnlyInFullscreen)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0008FD3C File Offset: 0x0008DF3C
		private void OnExitFullScreen()
		{
			if (this.visibleOnlyInFullscreen)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04001A50 RID: 6736
		public bool visibleOnlyInFullscreen = true;
	}
}
