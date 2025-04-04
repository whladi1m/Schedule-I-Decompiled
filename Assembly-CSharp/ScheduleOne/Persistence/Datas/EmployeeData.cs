using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000407 RID: 1031
	[Serializable]
	public class EmployeeData : NPCData
	{
		// Token: 0x06001567 RID: 5479 RVA: 0x0005F44C File Offset: 0x0005D64C
		public EmployeeData(string id, string assignedProperty, string firstName, string lastName, bool isMale, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday) : base(id)
		{
			this.AssignedProperty = assignedProperty;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.IsMale = isMale;
			this.AppearanceIndex = appearanceIndex;
			this.Position = position;
			this.Rotation = rotation;
			this.GUID = guid.ToString();
			this.PaidForToday = paidForToday;
		}

		// Token: 0x040013DD RID: 5085
		public string AssignedProperty;

		// Token: 0x040013DE RID: 5086
		public string FirstName;

		// Token: 0x040013DF RID: 5087
		public string LastName;

		// Token: 0x040013E0 RID: 5088
		public bool IsMale;

		// Token: 0x040013E1 RID: 5089
		public int AppearanceIndex;

		// Token: 0x040013E2 RID: 5090
		public Vector3 Position;

		// Token: 0x040013E3 RID: 5091
		public Quaternion Rotation;

		// Token: 0x040013E4 RID: 5092
		public string GUID;

		// Token: 0x040013E5 RID: 5093
		public bool PaidForToday;
	}
}
