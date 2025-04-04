using System;

namespace ScheduleOne.Persistence.Datas.Characters
{
	// Token: 0x02000433 RID: 1075
	public class ThomasData : NPCData
	{
		// Token: 0x0600159B RID: 5531 RVA: 0x0005FCB5 File Offset: 0x0005DEB5
		public ThomasData(string id, bool meetingReminderSent, bool handoverReminderSent) : base(id)
		{
			this.MeetingReminderSent = meetingReminderSent;
			this.HandoverReminderSent = handoverReminderSent;
		}

		// Token: 0x04001470 RID: 5232
		public bool MeetingReminderSent;

		// Token: 0x04001471 RID: 5233
		public bool HandoverReminderSent;
	}
}
