using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200092B RID: 2347
	[CreateAssetMenu(fileName = "IntegerItemDefinition", menuName = "ScriptableObjects/IntegerItemDefinition", order = 1)]
	[Serializable]
	public class IntegerItemDefinition : StorableItemDefinition
	{
		// Token: 0x06003F93 RID: 16275 RVA: 0x0010BA27 File Offset: 0x00109C27
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new IntegerItemInstance(this, quantity, this.DefaultValue);
		}

		// Token: 0x04002DC7 RID: 11719
		public int DefaultValue;
	}
}
