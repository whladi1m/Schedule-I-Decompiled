using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006ED RID: 1773
	public class GUIDUtility : MonoBehaviour
	{
		// Token: 0x06003030 RID: 12336 RVA: 0x000C8B50 File Offset: 0x000C6D50
		[Button]
		public void GenerateGUID()
		{
			Console.Log(Guid.NewGuid().ToString(), null);
		}
	}
}
