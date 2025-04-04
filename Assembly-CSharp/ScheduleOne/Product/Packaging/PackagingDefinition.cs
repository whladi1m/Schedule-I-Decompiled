using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product.Packaging
{
	// Token: 0x020008F9 RID: 2297
	[CreateAssetMenu(fileName = "PackagingDefinition", menuName = "ScriptableObjects/Item Definitions/PackagingDefinition", order = 1)]
	[Serializable]
	public class PackagingDefinition : StorableItemDefinition
	{
		// Token: 0x04002CCD RID: 11469
		public int Quantity = 1;

		// Token: 0x04002CCE RID: 11470
		public EStealthLevel StealthLevel;

		// Token: 0x04002CCF RID: 11471
		public FunctionalPackaging FunctionalPackaging;

		// Token: 0x04002CD0 RID: 11472
		public Equippable Equippable_Filled;

		// Token: 0x04002CD1 RID: 11473
		public StoredItem StoredItem_Filled;
	}
}
