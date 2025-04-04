using System;
using FishNet.Object;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000939 RID: 2361
	public class ItemSlotLock
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600401A RID: 16410 RVA: 0x0010D475 File Offset: 0x0010B675
		// (set) Token: 0x0600401B RID: 16411 RVA: 0x0010D47D File Offset: 0x0010B67D
		public ItemSlot Slot { get; protected set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x0010D486 File Offset: 0x0010B686
		// (set) Token: 0x0600401D RID: 16413 RVA: 0x0010D48E File Offset: 0x0010B68E
		public NetworkObject LockOwner { get; protected set; }

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x0010D497 File Offset: 0x0010B697
		// (set) Token: 0x0600401F RID: 16415 RVA: 0x0010D49F File Offset: 0x0010B69F
		public string LockReason { get; protected set; } = "";

		// Token: 0x06004020 RID: 16416 RVA: 0x0010D4A8 File Offset: 0x0010B6A8
		public ItemSlotLock(ItemSlot slot, NetworkObject lockOwner, string lockReason)
		{
			this.Slot = slot;
			this.LockOwner = lockOwner;
			this.LockReason = lockReason;
		}
	}
}
