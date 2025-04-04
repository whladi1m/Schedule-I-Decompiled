using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D7 RID: 1239
	public class Shirley : Supplier
	{
		// Token: 0x06001BAD RID: 7085 RVA: 0x00072B34 File Offset: 0x00070D34
		protected override void DeaddropConfirmed(List<PhoneShopInterface.CartEntry> cart, float totalPrice)
		{
			base.DeaddropConfirmed(cart, totalPrice);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ShirleyDeaddropOrders", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("ShirleyDeaddropOrders") + 1f).ToString(), true);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00072B76 File Offset: 0x00070D76
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00072B8F File Offset: 0x00070D8F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00072BA8 File Offset: 0x00070DA8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x00072BB6 File Offset: 0x00070DB6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001714 RID: 5908
		private bool dll_Excuted;

		// Token: 0x04001715 RID: 5909
		private bool dll_Excuted;
	}
}
