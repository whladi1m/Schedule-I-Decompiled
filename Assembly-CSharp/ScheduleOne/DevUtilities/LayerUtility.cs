using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D0 RID: 1744
	public static class LayerUtility
	{
		// Token: 0x06002F81 RID: 12161 RVA: 0x000C60E8 File Offset: 0x000C42E8
		public static void SetLayerRecursively(GameObject go, int layerNumber)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layerNumber;
			}
		}
	}
}
