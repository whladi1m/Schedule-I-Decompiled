using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200088C RID: 2188
	public class Safe : StorageEntity
	{
		// Token: 0x06003B5E RID: 15198 RVA: 0x000FA068 File Offset: 0x000F8268
		public float GetCash()
		{
			float num = 0f;
			for (int i = 0; i < base.ItemSlots.Count; i++)
			{
				if (base.ItemSlots[i].ItemInstance != null && base.ItemSlots[i].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = base.ItemSlots[i].ItemInstance as CashInstance;
					num += cashInstance.Balance;
				}
			}
			return num;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x000FA0E0 File Offset: 0x000F82E0
		public void RemoveCash(float amount)
		{
			amount = Mathf.Abs(amount);
			float num = amount;
			for (int i = 0; i < base.ItemSlots.Count; i++)
			{
				if (base.ItemSlots[i].ItemInstance != null && base.ItemSlots[i].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = base.ItemSlots[i].ItemInstance as CashInstance;
					float num2 = Mathf.Min(cashInstance.Balance, num);
					cashInstance.ChangeBalance(-num2);
					num -= num2;
				}
				if (num <= 0f)
				{
					break;
				}
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x000FA16E File Offset: 0x000F836E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x000FA187 File Offset: 0x000F8387
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x000FA1A0 File Offset: 0x000F83A0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x000FA1AE File Offset: 0x000F83AE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002B01 RID: 11009
		private bool dll_Excuted;

		// Token: 0x04002B02 RID: 11010
		private bool dll_Excuted;
	}
}
