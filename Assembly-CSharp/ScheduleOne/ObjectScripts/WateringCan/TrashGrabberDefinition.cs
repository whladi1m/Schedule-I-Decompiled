using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BC7 RID: 3015
	[CreateAssetMenu(fileName = "TrashGrabberDefinition", menuName = "ScriptableObjects/Item Definitions/TrashGrabberDefinition", order = 1)]
	[Serializable]
	public class TrashGrabberDefinition : StorableItemDefinition
	{
		// Token: 0x060054C3 RID: 21699 RVA: 0x0016506E File Offset: 0x0016326E
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new TrashGrabberInstance(this, quantity);
		}
	}
}
