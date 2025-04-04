using System;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BC0 RID: 3008
	public class FunctionalWateringCan : Pourable
	{
		// Token: 0x06005464 RID: 21604 RVA: 0x00163B34 File Offset: 0x00161D34
		public void Setup(WateringCanInstance instance)
		{
			this.itemInstance = instance;
			this.autoSetCurrentQuantity = false;
			this.currentQuantity = this.itemInstance.CurrentFillAmount;
			this.Visuals.SetFillLevel(this.itemInstance.CurrentFillAmount / 15f);
			base.Rb.isKinematic = false;
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00163B88 File Offset: 0x00161D88
		protected override void PourAmount(float amount)
		{
			this.itemInstance.ChangeFillAmount(-amount);
			this.Visuals.SetFillLevel(this.itemInstance.CurrentFillAmount / 15f);
			base.PourAmount(amount);
		}

		// Token: 0x04003E9D RID: 16029
		public WateringCanVisuals Visuals;

		// Token: 0x04003E9E RID: 16030
		private WateringCanInstance itemInstance;
	}
}
