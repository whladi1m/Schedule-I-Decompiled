using System;
using ScheduleOne.Growing;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200091A RID: 2330
	[CreateAssetMenu(fileName = "AdditiveDefinition", menuName = "ScriptableObjects/Item Definitions/AdditiveDefinition", order = 1)]
	[Serializable]
	public class AdditiveDefinition : StorableItemDefinition
	{
		// Token: 0x04002DAA RID: 11690
		public Additive AdditivePrefab;
	}
}
