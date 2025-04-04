using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerTasks.Tasks;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000901 RID: 2305
	public class Equippable_WateringCan : Equippable_Pourable
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003E73 RID: 15987 RVA: 0x00107AB3 File Offset: 0x00105CB3
		// (set) Token: 0x06003E74 RID: 15988 RVA: 0x00107ABB File Offset: 0x00105CBB
		public override string InteractionLabel { get; set; } = "Pour water";

		// Token: 0x06003E75 RID: 15989 RVA: 0x00107AC4 File Offset: 0x00105CC4
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.WCInstance = (item as WateringCanInstance);
			this.UpdateVisuals();
			item.onDataChanged = (Action)Delegate.Combine(item.onDataChanged, new Action(this.UpdateVisuals));
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00107B01 File Offset: 0x00105D01
		public override void Unequip()
		{
			base.Unequip();
			if (this.WCInstance != null)
			{
				WateringCanInstance wcinstance = this.WCInstance;
				wcinstance.onDataChanged = (Action)Delegate.Remove(wcinstance.onDataChanged, new Action(this.UpdateVisuals));
			}
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00107B38 File Offset: 0x00105D38
		private void UpdateVisuals()
		{
			if (this.WCInstance == null)
			{
				return;
			}
			this.Visuals.SetFillLevel(this.WCInstance.CurrentFillAmount / 15f);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00107B60 File Offset: 0x00105D60
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel < pot.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (pot.NormalizedWaterLevel >= 0.975f)
			{
				reason = string.Empty;
				return false;
			}
			if ((this.itemInstance as WateringCanInstance).CurrentFillAmount <= 0f)
			{
				reason = "Watering can empty";
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00107BC2 File Offset: 0x00105DC2
		protected override void StartPourTask(Pot pot)
		{
			new PourWaterTask(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x04002CF9 RID: 11513
		public WateringCanVisuals Visuals;

		// Token: 0x04002CFA RID: 11514
		private WateringCanInstance WCInstance;
	}
}
