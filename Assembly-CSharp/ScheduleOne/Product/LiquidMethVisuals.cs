using System;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008BF RID: 2239
	public class LiquidMethVisuals : MonoBehaviour
	{
		// Token: 0x06003CC1 RID: 15553 RVA: 0x000FF0B8 File Offset: 0x000FD2B8
		public void Setup(LiquidMethDefinition def)
		{
			if (def == null)
			{
				return;
			}
			if (this.StaticLiquidMesh != null)
			{
				this.StaticLiquidMesh.material.color = def.StaticLiquidColor;
			}
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidColor(def.LiquidVolumeColor, true, true);
			}
			if (this.PourParticles != null)
			{
				this.PourParticles.main.startColor = def.PourParticlesColor;
			}
		}

		// Token: 0x04002BE6 RID: 11238
		public MeshRenderer StaticLiquidMesh;

		// Token: 0x04002BE7 RID: 11239
		public LiquidContainer LiquidContainer;

		// Token: 0x04002BE8 RID: 11240
		public ParticleSystem PourParticles;
	}
}
