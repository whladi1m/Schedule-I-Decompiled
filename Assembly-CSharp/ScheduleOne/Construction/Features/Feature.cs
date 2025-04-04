using System;
using FishNet.Object;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000714 RID: 1812
	public abstract class Feature : NetworkBehaviour
	{
		// Token: 0x06003114 RID: 12564 RVA: 0x000CBB31 File Offset: 0x000C9D31
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Construction.Features.Feature_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000609D2 File Offset: 0x0005EBD2
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000CBB45 File Offset: 0x000C9D45
		public virtual FI_Base CreateInterface(Transform parent)
		{
			FI_Base component = UnityEngine.Object.Instantiate<GameObject>(this.featureInterfacePrefab, parent).GetComponent<FI_Base>();
			component.Initialize(this);
			return component;
		}

		// Token: 0x06003117 RID: 12567
		public abstract void Default();

		// Token: 0x06003119 RID: 12569 RVA: 0x000CBB72 File Offset: 0x000C9D72
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000CBB85 File Offset: 0x000C9D85
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000CBB98 File Offset: 0x000C9D98
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void dll()
		{
		}

		// Token: 0x04002318 RID: 8984
		public string featureName = "Feature name";

		// Token: 0x04002319 RID: 8985
		public Sprite featureIcon;

		// Token: 0x0400231A RID: 8986
		public Transform featureIconLocation;

		// Token: 0x0400231B RID: 8987
		public GameObject featureInterfacePrefab;

		// Token: 0x0400231C RID: 8988
		public bool disableRoofDisibility;

		// Token: 0x0400231D RID: 8989
		private bool dll_Excuted;

		// Token: 0x0400231E RID: 8990
		private bool dll_Excuted;
	}
}
