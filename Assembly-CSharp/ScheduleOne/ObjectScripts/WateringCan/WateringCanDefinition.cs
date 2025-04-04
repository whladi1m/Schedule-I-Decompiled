using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BCA RID: 3018
	[CreateAssetMenu(fileName = "WateringCanDefinition", menuName = "ScriptableObjects/Item Definitions/WateringCanDefinition", order = 1)]
	[Serializable]
	public class WateringCanDefinition : StorableItemDefinition
	{
		// Token: 0x060054D3 RID: 21715 RVA: 0x001652BE File Offset: 0x001634BE
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new WateringCanInstance(this, quantity, 0f);
		}

		// Token: 0x04003ED4 RID: 16084
		public const float Capacity = 15f;

		// Token: 0x04003ED5 RID: 16085
		public GameObject FunctionalWateringCanPrefab;
	}
}
