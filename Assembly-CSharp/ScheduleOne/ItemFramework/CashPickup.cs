using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Money;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200091D RID: 2333
	public class CashPickup : ItemPickup
	{
		// Token: 0x06003F69 RID: 16233 RVA: 0x0010B5D8 File Offset: 0x001097D8
		protected override void Hovered()
		{
			this.IntObj.SetMessage("Pick up " + MoneyManager.FormatAmount(this.Value, false, false));
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool CanPickup()
		{
			return true;
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0010B608 File Offset: 0x00109808
		protected override void Pickup()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Value, true, false);
			base.Pickup();
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x0010B635 File Offset: 0x00109835
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x0010B64E File Offset: 0x0010984E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x0010B667 File Offset: 0x00109867
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0010B675 File Offset: 0x00109875
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002DB1 RID: 11697
		public float Value = 10f;

		// Token: 0x04002DB2 RID: 11698
		private bool dll_Excuted;

		// Token: 0x04002DB3 RID: 11699
		private bool dll_Excuted;
	}
}
