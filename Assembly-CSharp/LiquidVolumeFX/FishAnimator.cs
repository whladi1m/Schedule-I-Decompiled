using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200016F RID: 367
	public class FishAnimator : MonoBehaviour
	{
		// Token: 0x060006F7 RID: 1783 RVA: 0x0001FE08 File Offset: 0x0001E008
		private void Update()
		{
			Vector3 position = Camera.main.transform.position;
			base.transform.LookAt(new Vector3(-position.x, base.transform.position.y, -position.z));
		}
	}
}
