using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000F6 RID: 246
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-from-profile/")]
	public class EffectFromProfile : MonoBehaviour
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00015FBB File Offset: 0x000141BB
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00015FC3 File Offset: 0x000141C3
		public EffectAbstractBase effectProfile
		{
			get
			{
				return this.m_EffectProfile;
			}
			set
			{
				this.m_EffectProfile = value;
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00015FD2 File Offset: 0x000141D2
		public void InitInstanceFromProfile()
		{
			if (this.m_EffectInstance)
			{
				if (this.m_EffectProfile)
				{
					this.m_EffectInstance.InitFrom(this.m_EffectProfile);
					return;
				}
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001600C File Offset: 0x0001420C
		private void OnEnable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = true;
				return;
			}
			if (this.m_EffectProfile)
			{
				this.m_EffectInstance = (base.gameObject.AddComponent(this.m_EffectProfile.GetType()) as EffectAbstractBase);
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00016067 File Offset: 0x00014267
		private void OnDisable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x0400056F RID: 1391
		public const string ClassName = "EffectFromProfile";

		// Token: 0x04000570 RID: 1392
		[SerializeField]
		private EffectAbstractBase m_EffectProfile;

		// Token: 0x04000571 RID: 1393
		private EffectAbstractBase m_EffectInstance;
	}
}
