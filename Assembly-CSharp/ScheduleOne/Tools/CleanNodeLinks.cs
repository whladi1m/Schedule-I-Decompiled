using System;
using EasyButtons;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200082D RID: 2093
	public class CleanNodeLinks : MonoBehaviour
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x000F3A34 File Offset: 0x000F1C34
		[Button]
		public void Clean()
		{
			foreach (NodeLink nodeLink in base.GetComponentsInChildren<NodeLink>())
			{
				if (nodeLink.End == null)
				{
					Console.Log("Destroying link: " + nodeLink.name, null);
					UnityEngine.Object.DestroyImmediate(nodeLink);
				}
			}
		}
	}
}
