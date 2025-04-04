using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000403 RID: 1027
	[Serializable]
	public class BotanistData : EmployeeData
	{
		// Token: 0x06001563 RID: 5475 RVA: 0x0005F390 File Offset: 0x0005D590
		public BotanistData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x040013D4 RID: 5076
		public MoveItemData MoveItemData;
	}
}
