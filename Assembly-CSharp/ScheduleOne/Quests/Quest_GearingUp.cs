using System;
using ScheduleOne.Economy;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002EB RID: 747
	public class Quest_GearingUp : Quest
	{
		// Token: 0x060010B0 RID: 4272 RVA: 0x0004AC91 File Offset: 0x00048E91
		protected override void Start()
		{
			base.Start();
			this.Supplier.onDeaddropReady.AddListener(new UnityAction(this.DropReady));
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0004ACB8 File Offset: 0x00048EB8
		protected override void MinPass()
		{
			base.MinPass();
			if (this.CollectDropEntry.State == EQuestState.Active && !this.setCollectionPosition)
			{
				DeadDrop deadDrop = DeadDrop.DeadDrops.Find((DeadDrop x) => x.Storage.ItemCount > 0);
				if (deadDrop != null)
				{
					this.setCollectionPosition = true;
					this.CollectDropEntry.SetPoILocation(deadDrop.transform.position);
				}
			}
			if (this.WaitForDropEntry.State == EQuestState.Active)
			{
				float num = (float)this.Supplier.minsUntilDeaddropReady;
				if (num > 0f)
				{
					this.WaitForDropEntry.SetEntryTitle("Wait for the dead drop (" + num.ToString() + " mins)");
					return;
				}
				this.WaitForDropEntry.SetEntryTitle("Wait for the dead drop");
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0004AD86 File Offset: 0x00048F86
		private void DropReady()
		{
			if (this.WaitForDropEntry.State == EQuestState.Active)
			{
				this.WaitForDropEntry.Complete();
				this.MinPass();
			}
		}

		// Token: 0x040010E2 RID: 4322
		public QuestEntry WaitForDropEntry;

		// Token: 0x040010E3 RID: 4323
		public QuestEntry CollectDropEntry;

		// Token: 0x040010E4 RID: 4324
		public Supplier Supplier;

		// Token: 0x040010E5 RID: 4325
		private bool setCollectionPosition;
	}
}
