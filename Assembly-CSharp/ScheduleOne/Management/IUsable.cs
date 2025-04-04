using System;
using FishNet.Object;

namespace ScheduleOne.Management
{
	// Token: 0x02000574 RID: 1396
	public interface IUsable
	{
		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x0008F615 File Offset: 0x0008D815
		bool IsInUse
		{
			get
			{
				return this.NPCUserObject != null || this.PlayerUserObject != null;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060022F7 RID: 8951
		// (set) Token: 0x060022F8 RID: 8952
		NetworkObject NPCUserObject { get; set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060022F9 RID: 8953
		// (set) Token: 0x060022FA RID: 8954
		NetworkObject PlayerUserObject { get; set; }

		// Token: 0x060022FB RID: 8955
		void SetPlayerUser(NetworkObject playerObject);

		// Token: 0x060022FC RID: 8956
		void SetNPCUser(NetworkObject playerObject);
	}
}
