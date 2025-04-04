using System;
using ScheduleOne.Vehicles.Modification;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000431 RID: 1073
	[Serializable]
	public class VehicleData : SaveData
	{
		// Token: 0x06001599 RID: 5529 RVA: 0x0005FC44 File Offset: 0x0005DE44
		public VehicleData(Guid guid, string code, Vector3 pos, Quaternion rot, EVehicleColor col)
		{
			this.GUID = guid.ToString();
			this.VehicleCode = code;
			this.Position = pos;
			this.Rotation = rot;
			this.Color = col.ToString();
		}

		// Token: 0x04001469 RID: 5225
		public string GUID;

		// Token: 0x0400146A RID: 5226
		public string VehicleCode;

		// Token: 0x0400146B RID: 5227
		public Vector3 Position;

		// Token: 0x0400146C RID: 5228
		public Quaternion Rotation;

		// Token: 0x0400146D RID: 5229
		public string Color;
	}
}
