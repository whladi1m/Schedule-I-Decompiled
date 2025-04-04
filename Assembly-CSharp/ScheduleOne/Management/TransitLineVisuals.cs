using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000583 RID: 1411
	public class TransitLineVisuals : MonoBehaviour
	{
		// Token: 0x06002347 RID: 9031 RVA: 0x00090127 File Offset: 0x0008E327
		public void SetSourcePosition(Vector3 position)
		{
			this.Renderer.SetPosition(0, position);
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x00090136 File Offset: 0x0008E336
		public void SetDestinationPosition(Vector3 position)
		{
			this.Renderer.SetPosition(1, position);
		}

		// Token: 0x04001A6A RID: 6762
		public LineRenderer Renderer;
	}
}
