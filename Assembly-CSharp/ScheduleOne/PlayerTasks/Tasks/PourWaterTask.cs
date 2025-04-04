using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x0200035B RID: 859
	public class PourWaterTask : PourOntoTargetTask
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool UseCoverage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00014002 File Offset: 0x00012202
		protected override bool FailOnEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00051F73 File Offset: 0x00050173
		protected override Pot.ECameraPosition CameraPosition
		{
			get
			{
				return Pot.ECameraPosition.BirdsEye;
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000566E0 File Offset: 0x000548E0
		public PourWaterTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			base.CurrentInstruction = "Pour water over target";
			this.removeItemAfterInitialPour = false;
			this.pourable.GetComponent<FunctionalWateringCan>().Setup(_itemInstance as WateringCanInstance);
			this.pourable.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			this.pot.SoilCover.ConfigureAppearance(Color.black, 0.6f);
			if (NetworkSingleton<GameManager>.Instance.IsTutorial && !PourWaterTask.hintShown)
			{
				PourWaterTask.hintShown = true;
				Singleton<HintDisplay>.Instance.ShowHint_20s("While dragging an item, press <Input_Left> or <Input_Right> to rotate it.");
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00056784 File Offset: 0x00054984
		public override void StopTask()
		{
			this.pot.PushWaterDataToServer();
			base.StopTask();
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00056798 File Offset: 0x00054998
		public override void TargetReached()
		{
			this.pot.ChangeWaterAmount(0.2f * this.pot.WaterCapacity);
			this.pot.PushWaterDataToServer();
			if (this.pot.NormalizedWaterLevel >= 0.975f)
			{
				this.Success();
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("WateredPotsCount");
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("WateredPotsCount", (value + 1f).ToString(), true);
			}
			base.TargetReached();
		}

		// Token: 0x04001298 RID: 4760
		public const float NORMALIZED_FILL_PER_TARGET = 0.2f;

		// Token: 0x04001299 RID: 4761
		public static bool hintShown;
	}
}
