using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200015D RID: 349
	public class FeaturesNotSupportedMessage : MonoBehaviour
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x0001E39D File Offset: 0x0001C59D
		private void Start()
		{
			if (!Noise3D.isSupported)
			{
				Debug.LogWarning(Noise3D.isNotSupportedString);
			}
		}
	}
}
