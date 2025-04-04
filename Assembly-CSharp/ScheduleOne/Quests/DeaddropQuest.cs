using System;
using System.Collections.Generic;
using ScheduleOne.Economy;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E2 RID: 738
	public class DeaddropQuest : Quest
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0004A46C File Offset: 0x0004866C
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x0004A474 File Offset: 0x00048674
		public DeadDrop Drop { get; private set; }

		// Token: 0x06001090 RID: 4240 RVA: 0x0004A47D File Offset: 0x0004867D
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			if (!DeaddropQuest.DeaddropQuests.Contains(this))
			{
				DeaddropQuest.DeaddropQuests.Add(this);
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004A49E File Offset: 0x0004869E
		public void SetDrop(DeadDrop drop)
		{
			this.Drop = drop;
			this.Entries[0].SetPoILocation(this.Drop.transform.position);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0004A4C8 File Offset: 0x000486C8
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Active && this.Drop.Storage.ItemCount == 0)
			{
				this.Entries[0].Complete();
				this.Complete(false);
			}
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0004A503 File Offset: 0x00048703
		private void OnDestroy()
		{
			DeaddropQuest.DeaddropQuests.Remove(this);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0004A511 File Offset: 0x00048711
		public override void End()
		{
			base.End();
			DeaddropQuest.DeaddropQuests.Remove(this);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0004A528 File Offset: 0x00048728
		public override string GetSaveString()
		{
			List<QuestEntryData> list = new List<QuestEntryData>();
			for (int i = 0; i < this.Entries.Count; i++)
			{
				list.Add(this.Entries[i].GetSaveData());
			}
			return new DeaddropQuestData(base.GUID.ToString(), base.QuestState, base.IsTracked, this.title, this.Description, base.Expires, new GameDateTimeData(base.Expiry), list.ToArray(), this.Drop.GUID.ToString()).GetJson(true);
		}

		// Token: 0x040010D4 RID: 4308
		public static List<DeaddropQuest> DeaddropQuests = new List<DeaddropQuest>();
	}
}
