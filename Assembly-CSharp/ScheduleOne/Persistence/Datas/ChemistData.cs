using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000404 RID: 1028
	[Serializable]
	public class ChemistData : EmployeeData
	{
		// Token: 0x06001564 RID: 5476 RVA: 0x0005F3BC File Offset: 0x0005D5BC
		public ChemistData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x040013D5 RID: 5077
		public MoveItemData MoveItemData;
	}
}
