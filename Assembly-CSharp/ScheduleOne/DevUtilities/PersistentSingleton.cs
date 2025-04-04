using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006DF RID: 1759
	public abstract class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
	{
		// Token: 0x06002FE7 RID: 12263 RVA: 0x000C7C69 File Offset: 0x000C5E69
		protected override void Awake()
		{
			base.Awake();
			if (this.Destroyed)
			{
				return;
			}
			base.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
