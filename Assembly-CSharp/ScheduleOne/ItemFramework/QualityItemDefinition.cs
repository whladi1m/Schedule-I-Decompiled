using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200093A RID: 2362
	[CreateAssetMenu(fileName = "StorableItemDefinition", menuName = "ScriptableObjects/QualityItemDefinition", order = 1)]
	[Serializable]
	public class QualityItemDefinition : StorableItemDefinition
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x0010D4D0 File Offset: 0x0010B6D0
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new QualityItemInstance(this, quantity, this.DefaultQuality);
		}

		// Token: 0x04002E11 RID: 11793
		[Header("Quality")]
		public EQuality DefaultQuality = EQuality.Standard;
	}
}
