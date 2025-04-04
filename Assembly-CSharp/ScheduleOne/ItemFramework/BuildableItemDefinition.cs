using System;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200091B RID: 2331
	[CreateAssetMenu(fileName = "BuildableItemDefinition", menuName = "ScriptableObjects/BuildableItemDefinition", order = 1)]
	[Serializable]
	public class BuildableItemDefinition : StorableItemDefinition
	{
		// Token: 0x04002DAB RID: 11691
		public BuildableItem BuiltItem;

		// Token: 0x04002DAC RID: 11692
		public BuildableItemDefinition.EBuildSoundType BuildSoundType;

		// Token: 0x0200091C RID: 2332
		public enum EBuildSoundType
		{
			// Token: 0x04002DAE RID: 11694
			Cardboard,
			// Token: 0x04002DAF RID: 11695
			Wood,
			// Token: 0x04002DB0 RID: 11696
			Metal
		}
	}
}
