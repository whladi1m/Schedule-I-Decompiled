using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000585 RID: 1413
	public class TransitRouteMaterial : MonoBehaviour
	{
		// Token: 0x06002356 RID: 9046 RVA: 0x000903A7 File Offset: 0x0008E5A7
		private void Awake()
		{
			Material material = base.GetComponent<MeshRenderer>().material;
			material.SetInt("unity_GUIZTestMode", 8);
			material.renderQueue = 3000;
		}
	}
}
