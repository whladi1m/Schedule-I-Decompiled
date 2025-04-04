using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000350 RID: 848
	public class StartLabOvenTask : Task
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x0005486D File Offset: 0x00052A6D
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x00054875 File Offset: 0x00052A75
		public LabOven Oven { get; private set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0005487E File Offset: 0x00052A7E
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x00054886 File Offset: 0x00052A86
		public StartLabOvenTask.EStep CurrentStep { get; protected set; }

		// Token: 0x06001316 RID: 4886 RVA: 0x00054890 File Offset: 0x00052A90
		public StartLabOvenTask(LabOven oven)
		{
			this.Oven = oven;
			oven.ResetPourableContainer();
			this.stationItem = oven.CreateStationItems(1)[0];
			this.stationItem.ActivateModule<PourableModule>();
			this.pourableModule = this.stationItem.GetModule<PourableModule>();
			ConfigurableJoint componentInChildren = this.stationItem.GetComponentInChildren<ConfigurableJoint>();
			if (componentInChildren != null)
			{
				UnityEngine.Object.Destroy(componentInChildren);
			}
			Rigidbody componentInChildren2 = this.stationItem.GetComponentInChildren<Rigidbody>();
			if (componentInChildren2 != null)
			{
				UnityEngine.Object.Destroy(componentInChildren2);
			}
			Draggable componentInChildren3 = this.stationItem.GetComponentInChildren<Draggable>();
			if (componentInChildren3 != null)
			{
				componentInChildren3.ClickableEnabled = false;
			}
			this.ingredient = this.Oven.IngredientSlot.ItemInstance.GetCopy(1);
			this.Oven.IngredientSlot.ItemInstance.ChangeQuantity(-1);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			oven.Door.SetInteractable(true);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0005498A File Offset: 0x00052B8A
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			base.CurrentInstruction = StartLabOvenTask.GetStepInstruction(this.CurrentStep);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x000549AC File Offset: 0x00052BAC
		public override void Success()
		{
			string id = (this.ingredient.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (this.ingredient is QualityItemInstance)
			{
				ingredientQuality = (this.ingredient as QualityItemInstance).Quality;
			}
			this.Oven.SendCookOperation(new OvenCookOperation(this.ingredient.ID, ingredientQuality, 1, id));
			base.Success();
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00054A24 File Offset: 0x00052C24
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Oven.IngredientSlot.AddItem(this.ingredient, false);
				this.Oven.LiquidMesh.gameObject.SetActive(false);
			}
			this.stationItem.Destroy();
			if (this.pourRoutine != null)
			{
				this.Oven.PourAnimation.Stop();
				this.Oven.StopCoroutine(this.pourRoutine);
			}
			this.Oven.ClearDecals();
			this.Oven.Door.SetPosition(0f);
			this.Oven.Door.SetInteractable(false);
			this.Oven.WireTray.SetPosition(0f);
			this.Oven.Button.SetInteractable(false);
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(this.Oven, true, true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_Default.position, this.Oven.CameraPosition_Default.rotation, 0.2f, false);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00054B50 File Offset: 0x00052D50
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case StartLabOvenTask.EStep.OpenDoor:
				this.CheckStep_OpenDoor();
				return;
			case StartLabOvenTask.EStep.Pour:
				this.CheckStep_Pour();
				return;
			case StartLabOvenTask.EStep.CloseDoor:
				this.CheckStep_CloseDoor();
				return;
			case StartLabOvenTask.EStep.PressButton:
				this.CheckStep_PressButton();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00054B98 File Offset: 0x00052D98
		private void ProgressStep()
		{
			if (this.CurrentStep == StartLabOvenTask.EStep.PressButton)
			{
				this.Success();
				return;
			}
			StartLabOvenTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == StartLabOvenTask.EStep.Pour)
			{
				this.Oven.WireTray.SetPosition(1f);
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.CloseDoor)
			{
				this.Oven.Door.SetInteractable(true);
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.Pour)
			{
				this.pourRoutine = this.Oven.StartCoroutine(this.PlayPourAnimation());
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.PressButton)
			{
				this.Oven.Button.SetInteractable(true);
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00054C38 File Offset: 0x00052E38
		private void CheckStep_OpenDoor()
		{
			if (this.Oven.Door.TargetPosition > 0.9f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(1f);
			}
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00054C88 File Offset: 0x00052E88
		private void CheckStep_Pour()
		{
			if (this.pourAnimDone)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00054C98 File Offset: 0x00052E98
		private void CheckStep_CloseDoor()
		{
			if (this.Oven.Door.TargetPosition < 0.05f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(0f);
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00054CE8 File Offset: 0x00052EE8
		private void CheckStep_PressButton()
		{
			if (this.Oven.Button.Pressed)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00054D02 File Offset: 0x00052F02
		private IEnumerator PlayPourAnimation()
		{
			this.Oven.SetLiquidColor(this.stationItem.GetModule<CookableModule>().LiquidColor);
			this.Oven.PourAnimation.Play();
			yield return new WaitForSeconds(0.6f);
			float pourTime = 1f;
			for (float i = 0f; i < pourTime; i += Time.deltaTime)
			{
				this.pourableModule.LiquidContainer.SetLiquidLevel(1f - i / pourTime, false);
				yield return null;
			}
			this.pourableModule.LiquidContainer.SetLiquidLevel(0f, false);
			this.pourAnimDone = true;
			yield break;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00054D11 File Offset: 0x00052F11
		public static string GetStepInstruction(StartLabOvenTask.EStep step)
		{
			switch (step)
			{
			case StartLabOvenTask.EStep.OpenDoor:
				return "Open oven door";
			case StartLabOvenTask.EStep.Pour:
				return "Pour liquid into tray";
			case StartLabOvenTask.EStep.CloseDoor:
				return "Close oven door";
			case StartLabOvenTask.EStep.PressButton:
				return "Start oven";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0400125E RID: 4702
		private ItemInstance ingredient;

		// Token: 0x0400125F RID: 4703
		private Coroutine pourRoutine;

		// Token: 0x04001260 RID: 4704
		private StationItem stationItem;

		// Token: 0x04001261 RID: 4705
		private PourableModule pourableModule;

		// Token: 0x04001262 RID: 4706
		private bool pourAnimDone;

		// Token: 0x02000351 RID: 849
		public enum EStep
		{
			// Token: 0x04001264 RID: 4708
			OpenDoor,
			// Token: 0x04001265 RID: 4709
			Pour,
			// Token: 0x04001266 RID: 4710
			CloseDoor,
			// Token: 0x04001267 RID: 4711
			PressButton
		}
	}
}
