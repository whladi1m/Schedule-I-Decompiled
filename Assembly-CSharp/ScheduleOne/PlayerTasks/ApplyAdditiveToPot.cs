using System;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks.Tasks;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000342 RID: 834
	public class ApplyAdditiveToPot : PourIntoPotTask
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool UseCoverage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00051F73 File Offset: 0x00050173
		protected override Pot.ECameraPosition CameraPosition
		{
			get
			{
				return Pot.ECameraPosition.BirdsEye;
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00051F78 File Offset: 0x00050178
		public ApplyAdditiveToPot(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			this.def = (_itemInstance.Definition as AdditiveDefinition);
			base.CurrentInstruction = "Cover soil with " + this.def.AdditivePrefab.AdditiveName + " (0%)";
			this.removeItemAfterInitialPour = false;
			this.pot.SoilCover.ConfigureAppearance((this.pourable as PourableAdditive).LiquidColor, 0.3f);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00051FF0 File Offset: 0x000501F0
		public override void Update()
		{
			base.Update();
			int num = Mathf.FloorToInt(this.pot.SoilCover.GetNormalizedProgress() * 100f);
			base.CurrentInstruction = string.Concat(new string[]
			{
				"Cover soil with ",
				this.def.AdditivePrefab.AdditiveName,
				" (",
				num.ToString(),
				"%)"
			});
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00052065 File Offset: 0x00050265
		protected override void FullyCovered()
		{
			base.FullyCovered();
			this.pot.SendAdditive((this.pourable as PourableAdditive).AdditiveDefinition.AdditivePrefab.AssetPath, true);
			base.RemoveItem();
			this.Success();
		}

		// Token: 0x0400121C RID: 4636
		private AdditiveDefinition def;
	}
}
