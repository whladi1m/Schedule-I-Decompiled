using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200089C RID: 2204
	public class StoredItemRandomRotation : MonoBehaviour
	{
		// Token: 0x06003C11 RID: 15377 RVA: 0x000FD140 File Offset: 0x000FB340
		public void Awake()
		{
			this.ItemContainer.localEulerAngles = new Vector3(this.ItemContainer.localEulerAngles.x, UnityEngine.Random.Range(0f, 360f), this.ItemContainer.localEulerAngles.z);
		}

		// Token: 0x04002B48 RID: 11080
		public Transform ItemContainer;
	}
}
