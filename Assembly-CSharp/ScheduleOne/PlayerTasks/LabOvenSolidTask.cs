using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200034B RID: 843
	public class LabOvenSolidTask : Task
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00053567 File Offset: 0x00051767
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x0005356F File Offset: 0x0005176F
		public LabOven Oven { get; private set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00053578 File Offset: 0x00051778
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x00053580 File Offset: 0x00051780
		public LabOvenSolidTask.EStep CurrentStep { get; protected set; }

		// Token: 0x060012F3 RID: 4851 RVA: 0x0005358C File Offset: 0x0005178C
		public LabOvenSolidTask(LabOven oven)
		{
			this.Oven = oven;
			this.ingredientQuantity = Mathf.Min(this.Oven.IngredientSlot.Quantity, 10);
			this.stationItems = oven.CreateStationItems(this.ingredientQuantity);
			this.stationDraggables = new Draggable[this.stationItems.Length];
			for (int i = 0; i < this.stationItems.Length; i++)
			{
				this.stationDraggables[i] = this.stationItems[i].GetComponentInChildren<Draggable>();
			}
			this.ingredient = this.Oven.IngredientSlot.ItemInstance.GetCopy(this.ingredientQuantity);
			this.Oven.IngredientSlot.ChangeQuantity(-this.ingredientQuantity, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_PlaceItems.position, this.Oven.CameraPosition_PlaceItems.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.EnableMultiDragging(oven.ItemContainer, 0.12f);
			oven.Door.SetInteractable(true);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x000536BC File Offset: 0x000518BC
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			base.CurrentInstruction = LabOvenSolidTask.GetStepInstruction(this.CurrentStep);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000536DC File Offset: 0x000518DC
		public override void Success()
		{
			string id = (this.ingredient.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (this.ingredient is QualityItemInstance)
			{
				ingredientQuality = (this.ingredient as QualityItemInstance).Quality;
			}
			this.Oven.SendCookOperation(new OvenCookOperation(this.ingredient.ID, ingredientQuality, this.ingredientQuantity, id));
			base.Success();
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00053758 File Offset: 0x00051958
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Oven.IngredientSlot.AddItem(this.ingredient, false);
				this.Oven.LiquidMesh.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.stationItems.Length; i++)
			{
				this.stationItems[i].Destroy();
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

		// Token: 0x060012F7 RID: 4855 RVA: 0x00053870 File Offset: 0x00051A70
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case LabOvenSolidTask.EStep.OpenDoor:
				this.CheckStep_OpenDoor();
				return;
			case LabOvenSolidTask.EStep.PlaceItems:
				this.CheckStep_PlaceItems();
				return;
			case LabOvenSolidTask.EStep.CloseDoor:
				this.CheckStep_CloseDoor();
				return;
			case LabOvenSolidTask.EStep.PressButton:
				this.CheckStep_PressButton();
				return;
			default:
				return;
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000538B8 File Offset: 0x00051AB8
		private void ProgressStep()
		{
			if (this.CurrentStep == LabOvenSolidTask.EStep.PressButton)
			{
				this.Success();
				return;
			}
			LabOvenSolidTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == LabOvenSolidTask.EStep.PlaceItems)
			{
				this.Oven.WireTray.SetPosition(1f);
			}
			if (this.CurrentStep == LabOvenSolidTask.EStep.CloseDoor)
			{
				this.Oven.Door.SetInteractable(true);
				for (int i = 0; i < this.stationDraggables.Length; i++)
				{
					this.stationDraggables[i].ClickableEnabled = false;
					UnityEngine.Object.Destroy(this.stationDraggables[i].Rb);
					this.stationItems[i].transform.SetParent(this.Oven.SquareTray);
				}
			}
			if (this.CurrentStep == LabOvenSolidTask.EStep.PressButton)
			{
				this.Oven.Button.SetInteractable(true);
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00053988 File Offset: 0x00051B88
		private void CheckStep_OpenDoor()
		{
			if (this.Oven.Door.TargetPosition > 0.9f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(1f);
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000539D8 File Offset: 0x00051BD8
		private void CheckStep_PlaceItems()
		{
			for (int i = 0; i < this.stationDraggables.Length; i++)
			{
				if (this.stationDraggables[i].IsHeld)
				{
					return;
				}
				if (this.stationDraggables[i].Rb.velocity.magnitude > 0.02f)
				{
					return;
				}
				if (!this.Oven.TrayDetectionArea.bounds.Contains(this.stationDraggables[i].transform.position))
				{
					return;
				}
			}
			this.ProgressStep();
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00053A60 File Offset: 0x00051C60
		private void CheckStep_CloseDoor()
		{
			if (this.Oven.Door.TargetPosition < 0.05f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(0f);
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00053AB0 File Offset: 0x00051CB0
		private void CheckStep_PressButton()
		{
			if (this.Oven.Button.Pressed)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00053ACA File Offset: 0x00051CCA
		public static string GetStepInstruction(LabOvenSolidTask.EStep step)
		{
			switch (step)
			{
			case LabOvenSolidTask.EStep.OpenDoor:
				return "Open oven door";
			case LabOvenSolidTask.EStep.PlaceItems:
				return "Place items onto tray";
			case LabOvenSolidTask.EStep.CloseDoor:
				return "Close oven door";
			case LabOvenSolidTask.EStep.PressButton:
				return "Start oven";
			default:
				return string.Empty;
			}
		}

		// Token: 0x04001242 RID: 4674
		private ItemInstance ingredient;

		// Token: 0x04001243 RID: 4675
		private int ingredientQuantity = 1;

		// Token: 0x04001244 RID: 4676
		private StationItem[] stationItems;

		// Token: 0x04001245 RID: 4677
		private Draggable[] stationDraggables;

		// Token: 0x0200034C RID: 844
		public enum EStep
		{
			// Token: 0x04001247 RID: 4679
			OpenDoor,
			// Token: 0x04001248 RID: 4680
			PlaceItems,
			// Token: 0x04001249 RID: 4681
			CloseDoor,
			// Token: 0x0400124A RID: 4682
			PressButton
		}
	}
}
