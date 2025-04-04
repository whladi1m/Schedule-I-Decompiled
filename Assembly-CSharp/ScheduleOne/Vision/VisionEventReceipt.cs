using System;
using FishNet.Object;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Vision
{
	// Token: 0x02000284 RID: 644
	[Serializable]
	public class VisionEventReceipt
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x0003C245 File Offset: 0x0003A445
		public VisionEventReceipt(NetworkObject targetPlayer, PlayerVisualState.EVisualState state)
		{
			this.TargetPlayer = targetPlayer;
			this.State = state;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0000494F File Offset: 0x00002B4F
		public VisionEventReceipt()
		{
		}

		// Token: 0x04000E0B RID: 3595
		public NetworkObject TargetPlayer;

		// Token: 0x04000E0C RID: 3596
		public PlayerVisualState.EVisualState State;
	}
}
