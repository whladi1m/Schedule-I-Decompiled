using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000409 RID: 1033
	[Serializable]
	public class PackagerData : EmployeeData
	{
		// Token: 0x06001569 RID: 5481 RVA: 0x0005F508 File Offset: 0x0005D708
		public PackagerData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x040013EA RID: 5098
		public MoveItemData MoveItemData;
	}
}
