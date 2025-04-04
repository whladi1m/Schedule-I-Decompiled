using System;
using System.Collections.Generic;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000716 RID: 1814
	public class GenericOptionListFeature : OptionListFeature
	{
		// Token: 0x06003122 RID: 12578 RVA: 0x000CBC08 File Offset: 0x000C9E08
		public override void Default()
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].Uninstall();
			}
			this.PurchaseOption(this.defaultOptionIndex);
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000CBC48 File Offset: 0x000C9E48
		protected override List<FI_OptionList.Option> GetOptions()
		{
			List<FI_OptionList.Option> list = new List<FI_OptionList.Option>();
			foreach (GenericOption genericOption in this.options)
			{
				list.Add(new FI_OptionList.Option(genericOption.optionName, genericOption.optionButtonColor, genericOption.optionPrice));
			}
			return list;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000CBCB8 File Offset: 0x000C9EB8
		public override void SelectOption(int optionIndex)
		{
			base.SelectOption(optionIndex);
			if (this.visibleOption != null && this.options[optionIndex] != this.visibleOption)
			{
				this.visibleOption.SetInvisible();
			}
			this.visibleOption = this.options[optionIndex];
			this.visibleOption.SetVisible();
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000CBD1C File Offset: 0x000C9F1C
		public override void PurchaseOption(int optionIndex)
		{
			base.PurchaseOption(optionIndex);
			if (this.installedOption != null && this.options[optionIndex] != this.installedOption)
			{
				this.installedOption.Uninstall();
			}
			this.installedOption = this.options[optionIndex];
			this.installedOption.Install();
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000CBD92 File Offset: 0x000C9F92
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000CBDAB File Offset: 0x000C9FAB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000CBDC4 File Offset: 0x000C9FC4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000CBDD2 File Offset: 0x000C9FD2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002326 RID: 8998
		[Header("References")]
		[SerializeField]
		protected List<GenericOption> options = new List<GenericOption>();

		// Token: 0x04002327 RID: 8999
		private GenericOption visibleOption;

		// Token: 0x04002328 RID: 9000
		private GenericOption installedOption;

		// Token: 0x04002329 RID: 9001
		private bool dll_Excuted;

		// Token: 0x0400232A RID: 9002
		private bool dll_Excuted;
	}
}
