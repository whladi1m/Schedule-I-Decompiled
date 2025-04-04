using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using VLB;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005A0 RID: 1440
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[RequireComponent(typeof(VolumetricLightBeamSD))]
	public class VolumetricLightTracker : MonoBehaviour
	{
		// Token: 0x060023D4 RID: 9172 RVA: 0x00091680 File Offset: 0x0008F880
		private void OnValidate()
		{
			if (this.light == null)
			{
				this.light = base.GetComponent<Light>();
			}
			if (this.optimizedLight == null)
			{
				this.optimizedLight = base.GetComponent<OptimizedLight>();
			}
			if (this.beam == null)
			{
				this.beam = base.GetComponent<VolumetricLightBeamSD>();
			}
			if (this.dust == null)
			{
				this.dust = base.GetComponent<VolumetricDustParticles>();
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000916F8 File Offset: 0x0008F8F8
		private void LateUpdate()
		{
			if (this.Override)
			{
				this.beam.enabled = this.Enabled;
			}
			else if (this.optimizedLight != null)
			{
				this.beam.enabled = this.optimizedLight.Enabled;
			}
			else if (this.light != null)
			{
				this.beam.enabled = this.light.enabled;
			}
			if (this.dust != null)
			{
				this.dust.enabled = this.beam.enabled;
			}
		}

		// Token: 0x04001AC4 RID: 6852
		public bool Override;

		// Token: 0x04001AC5 RID: 6853
		public bool Enabled;

		// Token: 0x04001AC6 RID: 6854
		public Light light;

		// Token: 0x04001AC7 RID: 6855
		public OptimizedLight optimizedLight;

		// Token: 0x04001AC8 RID: 6856
		public VolumetricLightBeamSD beam;

		// Token: 0x04001AC9 RID: 6857
		public VolumetricDustParticles dust;
	}
}
