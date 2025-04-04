using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000903 RID: 2307
	public class Equippable_Additive : Equippable_Pourable
	{
		// Token: 0x06003E7E RID: 15998 RVA: 0x00107C2C File Offset: 0x00105E2C
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.additiveDef = (this.itemInstance.Definition as AdditiveDefinition);
			this.InteractionLabel = "Apply " + this.additiveDef.Name;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00107C66 File Offset: 0x00105E66
		protected override void StartPourTask(Pot pot)
		{
			new ApplyAdditiveToPot(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00107C7C File Offset: 0x00105E7C
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel < pot.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (pot.Plant == null)
			{
				reason = "No plant";
				return false;
			}
			if (pot.GetAdditive(this.additiveDef.AdditivePrefab.AdditiveName) != null)
			{
				reason = "Already contains " + this.additiveDef.AdditivePrefab.AdditiveName;
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x04002CFE RID: 11518
		private AdditiveDefinition additiveDef;
	}
}
