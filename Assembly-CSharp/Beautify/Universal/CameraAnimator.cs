using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001EE RID: 494
	public class CameraAnimator : MonoBehaviour
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x000301FC File Offset: 0x0002E3FC
		private void Update()
		{
			base.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 10f));
		}
	}
}
