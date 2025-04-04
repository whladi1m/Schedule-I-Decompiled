using System;
using FishNet.Object;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D6 RID: 1750
	public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x000C6801 File Offset: 0x000C4A01
		public static bool InstanceExists
		{
			get
			{
				return NetworkSingleton<T>.instance != null;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002FAB RID: 12203 RVA: 0x000C6813 File Offset: 0x000C4A13
		// (set) Token: 0x06002FAC RID: 12204 RVA: 0x000C681A File Offset: 0x000C4A1A
		public static T Instance
		{
			get
			{
				return NetworkSingleton<T>.instance;
			}
			protected set
			{
				NetworkSingleton<T>.instance = value;
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000C6822 File Offset: 0x000C4A22
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.DevUtilities.NetworkSingleton`1_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000C6836 File Offset: 0x000C4A36
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<T>.instance == this)
			{
				NetworkSingleton<T>.instance = default(T);
			}
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000C6855 File Offset: 0x000C4A55
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000C6868 File Offset: 0x000C4A68
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000C687B File Offset: 0x000C4A7B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000C6889 File Offset: 0x000C4A89
		protected virtual void NetworkSingleton()
		{
			if (NetworkSingleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Keeping prior instance reference.", null);
				return;
			}
			NetworkSingleton<T>.instance = (T)((object)this);
		}

		// Token: 0x040021FE RID: 8702
		private static T instance;

		// Token: 0x040021FF RID: 8703
		protected bool Destroyed;

		// Token: 0x04002200 RID: 8704
		private bool NetworkSingleton;

		// Token: 0x04002201 RID: 8705
		private bool NetworkSingleton;
	}
}
