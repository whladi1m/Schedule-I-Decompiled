using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000855 RID: 2133
	public class RemoveChildColliders : MonoBehaviour
	{
		// Token: 0x06003A30 RID: 14896 RVA: 0x000F5000 File Offset: 0x000F3200
		private void Start()
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}
	}
}
