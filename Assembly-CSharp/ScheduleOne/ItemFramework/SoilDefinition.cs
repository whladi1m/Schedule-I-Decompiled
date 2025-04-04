using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000918 RID: 2328
	[CreateAssetMenu(fileName = "SoilDefinition", menuName = "ScriptableObjects/Item Definitions/SoilDefinition", order = 1)]
	[Serializable]
	public class SoilDefinition : StorableItemDefinition
	{
		// Token: 0x04002DA2 RID: 11682
		public SoilDefinition.ESoilQuality SoilQuality;

		// Token: 0x04002DA3 RID: 11683
		public Material DrySoilMat;

		// Token: 0x04002DA4 RID: 11684
		public Material WetSoilMat;

		// Token: 0x04002DA5 RID: 11685
		public Color ParticleColor;

		// Token: 0x04002DA6 RID: 11686
		public int Uses = 1;

		// Token: 0x02000919 RID: 2329
		public enum ESoilQuality
		{
			// Token: 0x04002DA8 RID: 11688
			Basic,
			// Token: 0x04002DA9 RID: 11689
			Premium
		}
	}
}
