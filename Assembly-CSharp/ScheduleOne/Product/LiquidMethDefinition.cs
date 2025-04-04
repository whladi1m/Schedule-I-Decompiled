using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008BE RID: 2238
	[CreateAssetMenu(fileName = "LiquidMethDefinition", menuName = "ScriptableObjects/LiquidMethDefinition", order = 1)]
	[Serializable]
	public class LiquidMethDefinition : QualityItemDefinition
	{
		// Token: 0x04002BE1 RID: 11233
		[Header("Liquid Meth Color Settings")]
		public Color StaticLiquidColor;

		// Token: 0x04002BE2 RID: 11234
		public Color LiquidVolumeColor;

		// Token: 0x04002BE3 RID: 11235
		public Color PourParticlesColor;

		// Token: 0x04002BE4 RID: 11236
		public Color CookableLiquidColor;

		// Token: 0x04002BE5 RID: 11237
		public Color CookableSolidColor;
	}
}
