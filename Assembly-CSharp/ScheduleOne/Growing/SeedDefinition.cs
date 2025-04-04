using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x0200086B RID: 2155
	[CreateAssetMenu(fileName = "SeedDefinition", menuName = "ScriptableObjects/Item Definitions/SeedDefinition", order = 1)]
	[Serializable]
	public class SeedDefinition : StorableItemDefinition
	{
		// Token: 0x04002A59 RID: 10841
		public FunctionalSeed FunctionSeedPrefab;

		// Token: 0x04002A5A RID: 10842
		public Plant PlantPrefab;
	}
}
