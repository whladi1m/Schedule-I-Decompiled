using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E6 RID: 1766
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x000C81C9 File Offset: 0x000C63C9
		public static bool InstanceExists
		{
			get
			{
				return Singleton<T>.instance != null;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x000C81DB File Offset: 0x000C63DB
		// (set) Token: 0x06003006 RID: 12294 RVA: 0x000C81E2 File Offset: 0x000C63E2
		public static T Instance
		{
			get
			{
				return Singleton<T>.instance;
			}
			protected set
			{
				Singleton<T>.instance = value;
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000C81EC File Offset: 0x000C63EC
		protected virtual void Awake()
		{
			if (Singleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Destroying this instance.", null);
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Singleton<T>.instance = (T)((object)this);
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000C823D File Offset: 0x000C643D
		protected virtual void OnDestroy()
		{
			if (Singleton<T>.instance == this)
			{
				Singleton<T>.instance = default(T);
			}
		}

		// Token: 0x04002249 RID: 8777
		private static T instance;

		// Token: 0x0400224A RID: 8778
		protected bool Destroyed;
	}
}
