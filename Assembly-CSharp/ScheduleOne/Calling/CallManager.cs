using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Phone;

namespace ScheduleOne.Calling
{
	// Token: 0x02000766 RID: 1894
	public class CallManager : Singleton<CallManager>
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x000D8E1B File Offset: 0x000D701B
		// (set) Token: 0x060033D7 RID: 13271 RVA: 0x000D8E23 File Offset: 0x000D7023
		public PhoneCallData QueuedCallData { get; private set; }

		// Token: 0x060033D8 RID: 13272 RVA: 0x000D8E2C File Offset: 0x000D702C
		protected override void Start()
		{
			base.Start();
			CallInterface instance = Singleton<CallInterface>.Instance;
			instance.CallCompleted = (Action<PhoneCallData>)Delegate.Combine(instance.CallCompleted, new Action<PhoneCallData>(this.CallCompleted));
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000D8E5A File Offset: 0x000D705A
		public void QueueCall(PhoneCallData data)
		{
			this.QueuedCallData = data;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000D8E63 File Offset: 0x000D7063
		public void ClearQueuedCall()
		{
			this.QueuedCallData = null;
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000D8E6C File Offset: 0x000D706C
		private void CallCompleted(PhoneCallData call)
		{
			if (call == this.QueuedCallData)
			{
				this.ClearQueuedCall();
			}
		}
	}
}
