using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E2 RID: 1762
	public abstract class PlayerSingleton<T> : MonoBehaviour where T : PlayerSingleton<T>
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x000C7ED6 File Offset: 0x000C60D6
		public static bool InstanceExists
		{
			get
			{
				return PlayerSingleton<T>.instance != null;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002FEF RID: 12271 RVA: 0x000C7EE8 File Offset: 0x000C60E8
		// (set) Token: 0x06002FF0 RID: 12272 RVA: 0x000C7EEF File Offset: 0x000C60EF
		public static T Instance
		{
			get
			{
				return PlayerSingleton<T>.instance;
			}
			protected set
			{
				PlayerSingleton<T>.instance = value;
			}
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000C7EF7 File Offset: 0x000C60F7
		protected virtual void Awake()
		{
			this.OnStartClient(true);
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000C7F00 File Offset: 0x000C6100
		public virtual void OnStartClient(bool IsOwner)
		{
			if (!IsOwner)
			{
				Console.Log("Destroying non-local player singleton: " + base.name, null);
				UnityEngine.Object.Destroy(this);
				return;
			}
			if (PlayerSingleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Keeping prior instance reference.", null);
				return;
			}
			PlayerSingleton<T>.instance = (T)((object)this);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000C7F66 File Offset: 0x000C6166
		protected virtual void OnDestroy()
		{
			if (PlayerSingleton<T>.instance == this)
			{
				PlayerSingleton<T>.instance = default(T);
			}
		}

		// Token: 0x0400223B RID: 8763
		private static T instance;
	}
}
