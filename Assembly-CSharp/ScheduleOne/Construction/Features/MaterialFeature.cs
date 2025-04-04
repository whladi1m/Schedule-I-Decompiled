using System;
using System.Collections.Generic;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000717 RID: 1815
	public class MaterialFeature : OptionListFeature
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000CBDE6 File Offset: 0x000C9FE6
		public override void SelectOption(int optionIndex)
		{
			base.SelectOption(optionIndex);
			this.ApplyMaterial(this.materials[optionIndex]);
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000CBE04 File Offset: 0x000CA004
		private void ApplyMaterial(MaterialFeature.NamedMaterial mat)
		{
			for (int i = 0; i < this.materialTargets.Count; i++)
			{
				this.materialTargets[i].material = mat.mat;
			}
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000CBE40 File Offset: 0x000CA040
		protected override List<FI_OptionList.Option> GetOptions()
		{
			List<FI_OptionList.Option> list = new List<FI_OptionList.Option>();
			for (int i = 0; i < this.materials.Count; i++)
			{
				list.Add(new FI_OptionList.Option(this.materials[i].matName, this.materials[i].buttonColor, this.materials[i].price));
			}
			return list;
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000CBEC6 File Offset: 0x000CA0C6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000CBEDF File Offset: 0x000CA0DF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000CBEF8 File Offset: 0x000CA0F8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000CBF06 File Offset: 0x000CA106
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400232B RID: 9003
		[Header("References")]
		[SerializeField]
		protected List<MeshRenderer> materialTargets = new List<MeshRenderer>();

		// Token: 0x0400232C RID: 9004
		[Header("Material settings")]
		public List<MaterialFeature.NamedMaterial> materials = new List<MaterialFeature.NamedMaterial>();

		// Token: 0x0400232D RID: 9005
		private bool dll_Excuted;

		// Token: 0x0400232E RID: 9006
		private bool dll_Excuted;

		// Token: 0x02000718 RID: 1816
		[Serializable]
		public class NamedMaterial
		{
			// Token: 0x0400232F RID: 9007
			public string matName;

			// Token: 0x04002330 RID: 9008
			public Color buttonColor;

			// Token: 0x04002331 RID: 9009
			public Material mat;

			// Token: 0x04002332 RID: 9010
			public float price = 100f;
		}
	}
}
