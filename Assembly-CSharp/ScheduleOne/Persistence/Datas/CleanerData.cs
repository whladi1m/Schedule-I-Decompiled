using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000405 RID: 1029
	[Serializable]
	public class CleanerData : EmployeeData
	{
		// Token: 0x06001565 RID: 5477 RVA: 0x0005F3E8 File Offset: 0x0005D5E8
		public CleanerData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x040013D6 RID: 5078
		public MoveItemData MoveItemData;
	}
}
