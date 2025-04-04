using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BA0 RID: 2976
	[Serializable]
	public class DryingOperation
	{
		// Token: 0x0600515C RID: 20828 RVA: 0x00157075 File Offset: 0x00155275
		public DryingOperation(string itemID, int quantity, EQuality startQuality, int time)
		{
			this.ItemID = itemID;
			this.Quantity = quantity;
			this.StartQuality = startQuality;
			this.Time = time;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x0000494F File Offset: 0x00002B4F
		public DryingOperation()
		{
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0015709A File Offset: 0x0015529A
		public void IncreaseQuality()
		{
			this.StartQuality++;
			this.Time = 0;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001570B1 File Offset: 0x001552B1
		public QualityItemInstance GetQualityItemInstance()
		{
			QualityItemInstance qualityItemInstance = Registry.GetItem(this.ItemID).GetDefaultInstance(this.Quantity) as QualityItemInstance;
			qualityItemInstance.SetQuality(this.StartQuality);
			return qualityItemInstance;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001570DA File Offset: 0x001552DA
		public EQuality GetQuality()
		{
			if (this.Time >= 720)
			{
				return this.StartQuality + 1;
			}
			return this.StartQuality;
		}

		// Token: 0x04003D10 RID: 15632
		public string ItemID;

		// Token: 0x04003D11 RID: 15633
		public int Quantity;

		// Token: 0x04003D12 RID: 15634
		public EQuality StartQuality;

		// Token: 0x04003D13 RID: 15635
		public int Time;
	}
}
