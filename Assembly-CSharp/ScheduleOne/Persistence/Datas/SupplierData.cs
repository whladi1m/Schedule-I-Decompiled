using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040A RID: 1034
	[Serializable]
	public class SupplierData : NPCData
	{
		// Token: 0x0600156A RID: 5482 RVA: 0x0005F534 File Offset: 0x0005D734
		public SupplierData(string id, int _timeSinceMeetingStart, int _timeSinceLastMeetingEnd, float _debt, int _minsUntilDeadDropReady, StringIntPair[] _deaddropItems, bool _debtReminderSent) : base(id)
		{
			this.timeSinceMeetingStart = _timeSinceMeetingStart;
			this.timeSinceLastMeetingEnd = _timeSinceLastMeetingEnd;
			this.debt = _debt;
			this.minsUntilDeadDropReady = _minsUntilDeadDropReady;
			this.deaddropItems = _deaddropItems;
			this.debtReminderSent = _debtReminderSent;
		}

		// Token: 0x040013EB RID: 5099
		public int timeSinceMeetingStart;

		// Token: 0x040013EC RID: 5100
		public int timeSinceLastMeetingEnd;

		// Token: 0x040013ED RID: 5101
		public float debt;

		// Token: 0x040013EE RID: 5102
		public int minsUntilDeadDropReady;

		// Token: 0x040013EF RID: 5103
		public StringIntPair[] deaddropItems;

		// Token: 0x040013F0 RID: 5104
		public bool debtReminderSent;
	}
}
