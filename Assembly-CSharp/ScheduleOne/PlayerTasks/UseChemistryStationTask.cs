using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000357 RID: 855
	public class UseChemistryStationTask : Task
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x0005536E File Offset: 0x0005356E
		// (set) Token: 0x0600133F RID: 4927 RVA: 0x00055376 File Offset: 0x00053576
		public ChemistryStation.EStep CurrentStep { get; private set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x0005537F File Offset: 0x0005357F
		// (set) Token: 0x06001341 RID: 4929 RVA: 0x00055387 File Offset: 0x00053587
		public ChemistryStation Station { get; private set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x00055390 File Offset: 0x00053590
		// (set) Token: 0x06001343 RID: 4931 RVA: 0x00055398 File Offset: 0x00053598
		public StationRecipe Recipe { get; private set; }

		// Token: 0x06001344 RID: 4932 RVA: 0x000553A4 File Offset: 0x000535A4
		public static string GetStepDescription(ChemistryStation.EStep step)
		{
			switch (step)
			{
			case ChemistryStation.EStep.CombineIngredients:
				return "Combine ingredients in beaker";
			case ChemistryStation.EStep.Stir:
				return "Stir mixture";
			case ChemistryStation.EStep.LowerBoilingFlask:
				return "Lower boiling flask";
			case ChemistryStation.EStep.PourIntoBoilingFlask:
				return "Pour mixture into boiling flask";
			case ChemistryStation.EStep.RaiseBoilingFlask:
				return "Raise boiling flask above burner";
			case ChemistryStation.EStep.StartHeat:
				return "Start burner";
			case ChemistryStation.EStep.Cook:
				return "Wait for the mixture to finish cooking";
			case ChemistryStation.EStep.LowerBoilingFlaskAgain:
				return "Lower boiling flask";
			case ChemistryStation.EStep.PourThroughFilter:
				return "Pour mixture through filter";
			default:
				return "Unknown step";
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00055418 File Offset: 0x00053618
		public UseChemistryStationTask(ChemistryStation station, StationRecipe recipe)
		{
			this.Station = station;
			this.Recipe = recipe;
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.beaker = station.CreateBeaker();
			station.StaticBeaker.gameObject.SetActive(false);
			base.EnableMultiDragging(station.ItemContainer, 0.1f);
			this.RemovedIngredients = new ItemInstance[station.IngredientSlots.Length];
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = null;
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition2 = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < station.IngredientSlots.Length; j++)
					{
						if (station.IngredientSlots[j].ItemInstance != null && station.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition2.ID)
						{
							storableItemDefinition = storableItemDefinition2;
							this.RemovedIngredients[j] = station.IngredientSlots[j].ItemInstance.GetCopy(recipe.Ingredients[i].Quantity);
							station.IngredientSlots[j].ChangeQuantity(-recipe.Ingredients[i].Quantity, false);
							break;
						}
					}
				}
				if (storableItemDefinition.StationItem == null)
				{
					Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have a station item", null);
				}
				else
				{
					StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, station.ItemContainer);
					stationItem.transform.position = station.IngredientTransforms[i].position;
					stationItem.Initialize(storableItemDefinition);
					stationItem.transform.rotation = station.IngredientTransforms[i].rotation;
					if (stationItem.HasModule<IngredientModule>())
					{
						stationItem.ActivateModule<IngredientModule>();
						foreach (IngredientPiece ingredientPiece in stationItem.GetModule<IngredientModule>().Pieces)
						{
							this.ingredientPieces.Add(ingredientPiece);
							ingredientPiece.GetComponent<Draggable>().CanBeMultiDragged = true;
						}
					}
					else if (stationItem.HasModule<PourableModule>())
					{
						stationItem.ActivateModule<PourableModule>();
						Draggable componentInChildren = stationItem.GetComponentInChildren<Draggable>();
						if (componentInChildren != null)
						{
							componentInChildren.CanBeMultiDragged = false;
						}
					}
					else
					{
						Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have an ingredient or pourable module", null);
					}
					foreach (Draggable draggable in stationItem.GetComponentsInChildren<Draggable>())
					{
						draggable.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
						DraggableConstraint component = draggable.gameObject.GetComponent<DraggableConstraint>();
						if (component != null)
						{
							component.ProportionalZClamp = true;
						}
					}
					this.items.Add(stationItem);
				}
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00055728 File Offset: 0x00053928
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0005573C File Offset: 0x0005393C
		private void UpdateInstruction()
		{
			base.CurrentInstruction = UseChemistryStationTask.GetStepDescription(this.CurrentStep);
			if (this.CurrentStep == ChemistryStation.EStep.Stir)
			{
				base.CurrentInstruction = base.CurrentInstruction + " (" + Mathf.RoundToInt(this.stirProgress * 100f).ToString() + "%)";
			}
			if (this.CurrentStep == ChemistryStation.EStep.StartHeat)
			{
				base.CurrentInstruction = base.CurrentInstruction + " (" + Mathf.RoundToInt(this.timeInTemperatureRange / 2f * 100f).ToString() + "%)";
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000557DC File Offset: 0x000539DC
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case ChemistryStation.EStep.CombineIngredients:
				this.CheckStep_CombineIngredients();
				return;
			case ChemistryStation.EStep.Stir:
				this.CheckStep_StirMixture();
				return;
			case ChemistryStation.EStep.LowerBoilingFlask:
				this.CheckStep_LowerBoilingFlask();
				return;
			case ChemistryStation.EStep.PourIntoBoilingFlask:
				this.CheckStep_PourIntoBoilingFlask();
				return;
			case ChemistryStation.EStep.RaiseBoilingFlask:
				this.CheckStep_RaiseBoilingFlask();
				return;
			case ChemistryStation.EStep.StartHeat:
				this.CheckStep_StartHeat();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00055838 File Offset: 0x00053A38
		private void ProgressStep()
		{
			ChemistryStation.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == ChemistryStation.EStep.Stir)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_Stirring.position, this.Station.CameraPosition_Stirring.rotation, 0.2f, false);
				this.stirringRod = this.Station.CreateStirringRod();
				this.Station.StaticStirringRod.gameObject.SetActive(false);
			}
			if (this.CurrentStep == ChemistryStation.EStep.LowerBoilingFlask)
			{
				if (this.stirringRod != null)
				{
					this.stirringRod.Destroy();
				}
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_Default.position, this.Station.CameraPosition_Default.rotation, 0.2f, false);
				this.Station.LabStand.SetInteractable(true);
			}
			if (this.CurrentStep == ChemistryStation.EStep.PourIntoBoilingFlask)
			{
				this.beaker.SetStatic(false);
				this.beaker.ActivateModule<PourableModule>();
				this.beaker.Fillable.enabled = false;
				PourableModule module = this.beaker.GetModule<PourableModule>();
				module.SetLiquidLevel(module.LiquidContainer.CurrentLiquidLevel);
				module.LiquidColor = module.LiquidContainer.LiquidVolume.liquidColor1;
				module.PourParticlesColor = module.LiquidColor;
			}
			if (this.CurrentStep == ChemistryStation.EStep.RaiseBoilingFlask)
			{
				this.Station.LabStand.SetInteractable(true);
			}
			if (this.CurrentStep == ChemistryStation.EStep.StartHeat)
			{
				this.Station.Burner.SetInteractable(true);
				this.Station.BoilingFlask.SetCanvasVisible(true);
				this.Station.BoilingFlask.SetRecipe(this.Recipe);
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000559E4 File Offset: 0x00053BE4
		private void CheckStep_CombineIngredients()
		{
			bool flag = true;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].HasModule<PourableModule>())
				{
					if (this.items[i].GetModule<PourableModule>().NormalizedLiquidLevel > 0.05f)
					{
						flag = false;
						break;
					}
				}
				else if (this.items[i].HasModule<IngredientModule>())
				{
					IngredientPiece[] pieces = this.items[i].GetModule<IngredientModule>().Pieces;
					for (int j = 0; j < pieces.Length; j++)
					{
						if (pieces[j].CurrentLiquidContainer == null)
						{
							flag = false;
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00055A98 File Offset: 0x00053C98
		private void CheckStep_StirMixture()
		{
			float num = this.stirringRod.CurrentStirringSpeed * Time.deltaTime / 1.5f;
			if (num > 0f)
			{
				this.stirProgress = Mathf.Clamp(this.stirProgress + num, 0f, 1f);
				foreach (IngredientPiece ingredientPiece in this.ingredientPieces)
				{
					ingredientPiece.DissolveAmount(num, num > 0.001f);
				}
			}
			if (this.stirProgress >= 1f)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00055B44 File Offset: 0x00053D44
		private void CheckStep_LowerBoilingFlask()
		{
			if (this.Station.LabStand.CurrentPosition <= this.Station.LabStand.FunnelThreshold)
			{
				this.Station.LabStand.SetPosition(0f);
				this.Station.LabStand.SetInteractable(false);
				this.ProgressStep();
			}
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00055B9F File Offset: 0x00053D9F
		private void CheckStep_PourIntoBoilingFlask()
		{
			if (this.beaker.Pourable.NormalizedLiquidLevel <= 0.01f)
			{
				this.beaker.Pourable.LiquidContainer.SetLiquidLevel(0f, false);
				this.ProgressStep();
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00055BDC File Offset: 0x00053DDC
		private void CheckStep_RaiseBoilingFlask()
		{
			if (this.Station.LabStand.CurrentPosition >= 0.95f)
			{
				this.Station.LabStand.SetPosition(1f);
				this.Station.LabStand.SetInteractable(false);
				this.ProgressStep();
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00055C2C File Offset: 0x00053E2C
		private void CheckStep_StartHeat()
		{
			if (this.Station.BoilingFlask.OverheatScale >= 1f)
			{
				this.Fail();
				NetworkSingleton<CombatManager>.Instance.CreateExplosion(this.Station.ExplosionPoint.transform.position, ExplosionData.DefaultSmall);
				Player.Local.Health.TakeDamage(100f, true, true);
			}
			if (this.Station.BoilingFlask.IsTemperatureInRange)
			{
				this.timeInTemperatureRange += Time.deltaTime;
			}
			else
			{
				this.timeInTemperatureRange = Mathf.Clamp(this.timeInTemperatureRange - Time.deltaTime, 0f, 2f);
			}
			if (this.timeInTemperatureRange >= 2f)
			{
				this.ProgressStep();
				this.Station.BoilingFlask.SetCanvasVisible(false);
				this.Station.Burner.SetInteractable(false);
				this.Success();
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00055D14 File Offset: 0x00053F14
		public override void Success()
		{
			EQuality productQuality = this.Recipe.CalculateQuality(this.RemovedIngredients.ToList<ItemInstance>());
			ChemistryCookOperation op = new ChemistryCookOperation(this.Recipe, productQuality, this.Station.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor1, this.Station.BoilingFlask.LiquidContainer.CurrentLiquidLevel, 0);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Chemical_Operations_Started", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Chemical_Operations_Started") + 1f).ToString(), true);
			this.Station.CreateTrash(this.items);
			this.Station.SendCookOperation(op);
			base.Success();
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success && this.Outcome != Task.EOutcome.Fail)
			{
				for (int i = 0; i < this.RemovedIngredients.Length; i++)
				{
					if (this.RemovedIngredients[i] != null)
					{
						this.Station.IngredientSlots[i].AddItem(this.RemovedIngredients[i], false);
					}
				}
				this.Station.ResetStation();
			}
			if (this.Outcome == Task.EOutcome.Fail)
			{
				this.Station.ResetStation();
			}
			Singleton<ChemistryStationCanvas>.Instance.Open(this.Station);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			this.beaker.Destroy();
			this.Station.StaticBeaker.gameObject.SetActive(true);
			this.Station.StaticFunnel.gameObject.SetActive(true);
			this.Station.StaticStirringRod.gameObject.SetActive(true);
			this.Station.LabStand.SetPosition(1f);
			this.Station.LabStand.SetInteractable(false);
			this.Station.Burner.SetInteractable(false);
			this.Station.BoilingFlask.SetCanvasVisible(false);
			if (this.stirringRod != null)
			{
				this.stirringRod.Destroy();
			}
			foreach (StationItem stationItem in this.items)
			{
				stationItem.Destroy();
			}
			this.items.Clear();
		}

		// Token: 0x0400127E RID: 4734
		public const float STIR_TIME = 1.5f;

		// Token: 0x0400127F RID: 4735
		public const float TEMPERATURE_TIME = 2f;

		// Token: 0x04001283 RID: 4739
		private Beaker beaker;

		// Token: 0x04001284 RID: 4740
		private StirringRod stirringRod;

		// Token: 0x04001285 RID: 4741
		private List<StationItem> items = new List<StationItem>();

		// Token: 0x04001286 RID: 4742
		private List<IngredientPiece> ingredientPieces = new List<IngredientPiece>();

		// Token: 0x04001287 RID: 4743
		private float stirProgress;

		// Token: 0x04001288 RID: 4744
		private float timeInTemperatureRange;

		// Token: 0x04001289 RID: 4745
		private ItemInstance[] RemovedIngredients;
	}
}
