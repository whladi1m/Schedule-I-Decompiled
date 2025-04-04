using System;
using ScheduleOne.Equipping;
using ScheduleOne.PlayerTasks.Tasks;

namespace ScheduleOne.ObjectScripts.Soil
{
	// Token: 0x02000BCF RID: 3023
	public class Equippable_Soil : Equippable_Pourable
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x0016549E File Offset: 0x0016369E
		// (set) Token: 0x060054E5 RID: 21733 RVA: 0x001654A6 File Offset: 0x001636A6
		public override string InteractionLabel { get; set; } = "Pour soil";

		// Token: 0x060054E6 RID: 21734 RVA: 0x001654B0 File Offset: 0x001636B0
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel >= pot.SoilCapacity)
			{
				reason = "Pot already full";
				return false;
			}
			if (!string.IsNullOrEmpty(pot.SoilID) && pot.SoilID != this.itemInstance.ID)
			{
				reason = "Soil type mismatch";
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x0016550A File Offset: 0x0016370A
		protected override void StartPourTask(Pot pot)
		{
			new PourSoilTask(pot, this.itemInstance, this.PourablePrefab);
		}
	}
}
