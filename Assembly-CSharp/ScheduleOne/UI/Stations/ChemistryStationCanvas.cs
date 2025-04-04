using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A43 RID: 2627
	public class ChemistryStationCanvas : Singleton<ChemistryStationCanvas>
	{
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x001285E6 File Offset: 0x001267E6
		// (set) Token: 0x060046CC RID: 18124 RVA: 0x001285EE File Offset: 0x001267EE
		public bool isOpen { get; protected set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x001285F7 File Offset: 0x001267F7
		// (set) Token: 0x060046CE RID: 18126 RVA: 0x001285FF File Offset: 0x001267FF
		public ChemistryStation ChemistryStation { get; protected set; }

		// Token: 0x060046CF RID: 18127 RVA: 0x00128608 File Offset: 0x00126808
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
			for (int i = 0; i < this.Recipes.Count; i++)
			{
				StationRecipeEntry component = UnityEngine.Object.Instantiate<StationRecipeEntry>(this.RecipeEntryPrefab, this.RecipeContainer).GetComponent<StationRecipeEntry>();
				component.AssignRecipe(this.Recipes[i]);
				this.recipeEntries.Add(component);
			}
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x00128682 File Offset: 0x00126882
		protected override void Start()
		{
			base.Start();
			this.Close(false);
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00128694 File Offset: 0x00126894
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.ChemistryStation.CurrentCookOperation != null)
				{
					this.BeginButton.interactable = (this.ChemistryStation.CurrentCookOperation.CurrentTime >= this.ChemistryStation.CurrentCookOperation.Recipe.CookTime_Mins);
					this.BeginButton.gameObject.SetActive(false);
				}
				else
				{
					this.BeginButton.interactable = (this.selectedRecipe != null && this.selectedRecipe.IsValid && this.ChemistryStation.DoesOutputHaveSpace(this.selectedRecipe.Recipe));
					this.BeginButton.gameObject.SetActive(true);
				}
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
				}
				this.UpdateInput();
				this.UpdateUI();
			}
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00128779 File Offset: 0x00126979
		private void LateUpdate()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.selectedRecipe != null)
			{
				this.SelectionIndicator.position = this.selectedRecipe.transform.position;
			}
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001287B0 File Offset: 0x001269B0
		private void UpdateUI()
		{
			this.ErrorLabel.enabled = false;
			if (this.ChemistryStation.CurrentCookOperation != null)
			{
				this.CookingInProgressContainer.gameObject.SetActive(true);
				this.RecipeSelectionContainer.gameObject.SetActive(false);
				if (this.ChemistryStation.CurrentCookOperation.CurrentTime >= this.ChemistryStation.CurrentCookOperation.Recipe.CookTime_Mins)
				{
					this.InProgressLabel.text = "Ready to finish";
				}
				else
				{
					this.InProgressLabel.text = "Cooking in progress...";
				}
				if (this.InProgressRecipeEntry.Recipe != this.ChemistryStation.CurrentCookOperation.Recipe)
				{
					this.InProgressRecipeEntry.AssignRecipe(this.ChemistryStation.CurrentCookOperation.Recipe);
					return;
				}
			}
			else
			{
				this.RecipeSelectionContainer.gameObject.SetActive(true);
				this.CookingInProgressContainer.gameObject.SetActive(false);
				if (this.selectedRecipe != null && !this.ChemistryStation.DoesOutputHaveSpace(this.selectedRecipe.Recipe))
				{
					this.ErrorLabel.text = "Output slot does not have enough space";
					this.ErrorLabel.enabled = true;
				}
			}
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x001288EC File Offset: 0x00126AEC
		private void UpdateInput()
		{
			if (this.selectedRecipe != null)
			{
				if (GameInput.MouseScrollDelta < 0f || GameInput.GetButtonDown(GameInput.ButtonCode.Backward) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					if (this.recipeEntries.IndexOf(this.selectedRecipe) < this.recipeEntries.Count - 1)
					{
						StationRecipeEntry stationRecipeEntry = this.recipeEntries[this.recipeEntries.IndexOf(this.selectedRecipe) + 1];
						if (stationRecipeEntry.IsValid)
						{
							this.SetSelectedRecipe(stationRecipeEntry);
							return;
						}
					}
				}
				else if ((GameInput.MouseScrollDelta > 0f || GameInput.GetButtonDown(GameInput.ButtonCode.Forward) || Input.GetKeyDown(KeyCode.UpArrow)) && this.recipeEntries.IndexOf(this.selectedRecipe) > 0)
				{
					StationRecipeEntry stationRecipeEntry2 = this.recipeEntries[this.recipeEntries.IndexOf(this.selectedRecipe) - 1];
					if (stationRecipeEntry2.IsValid)
					{
						this.SetSelectedRecipe(stationRecipeEntry2);
					}
				}
			}
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x001289E0 File Offset: 0x00126BE0
		public void Open(ChemistryStation station)
		{
			this.isOpen = true;
			this.ChemistryStation = station;
			this.UpdateUI();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			for (int i = 0; i < station.IngredientSlots.Length; i++)
			{
				this.InputSlotUIs[i].AssignSlot(station.IngredientSlots[i]);
				ItemSlot itemSlot = station.IngredientSlots[i];
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.StationSlotsChanged));
			}
			this.OutputSlotUI.AssignSlot(station.OutputSlot);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> list = new List<ItemSlot>();
			list.AddRange(station.IngredientSlots);
			list.Add(station.OutputSlot);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.StationSlotsChanged();
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x00128B0C File Offset: 0x00126D0C
		public void Close(bool removeUI)
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			for (int i = 0; i < this.InputSlotUIs.Length; i++)
			{
				this.InputSlotUIs[i].ClearSlot();
				if (this.ChemistryStation != null)
				{
					ItemSlot itemSlot = this.ChemistryStation.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Remove(itemSlot.onItemDataChanged, new Action(this.StationSlotsChanged));
				}
			}
			this.OutputSlotUI.ClearSlot();
			if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			this.ChemistryStation = null;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x00128BDB File Offset: 0x00126DDB
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new UseChemistryStationTask(this.ChemistryStation, this.selectedRecipe.Recipe);
			this.Close(false);
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x00128C08 File Offset: 0x00126E08
		private void StationSlotsChanged()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.InputSlotUIs.Length; i++)
			{
				if (this.InputSlotUIs[i].assignedSlot.ItemInstance != null)
				{
					list.Add(this.InputSlotUIs[i].assignedSlot.ItemInstance);
				}
			}
			for (int j = 0; j < this.recipeEntries.Count; j++)
			{
				this.recipeEntries[j].RefreshValidity(list);
			}
			this.SortRecipes(list);
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x00128C8C File Offset: 0x00126E8C
		private void SortRecipes(List<ItemInstance> ingredients)
		{
			Dictionary<StationRecipeEntry, float> recipes = new Dictionary<StationRecipeEntry, float>();
			for (int i = 0; i < this.recipeEntries.Count; i++)
			{
				float ingredientsMatchDelta = this.recipeEntries[i].GetIngredientsMatchDelta(ingredients);
				recipes.Add(this.recipeEntries[i], ingredientsMatchDelta);
			}
			this.recipeEntries.Sort((StationRecipeEntry a, StationRecipeEntry b) => recipes[b].CompareTo(recipes[a]));
			for (int j = 0; j < this.recipeEntries.Count; j++)
			{
				this.recipeEntries[j].transform.SetAsLastSibling();
			}
			if (this.recipeEntries.Count > 0 && this.recipeEntries[0].IsValid)
			{
				this.SetSelectedRecipe(this.recipeEntries[0]);
				return;
			}
			this.SetSelectedRecipe(null);
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x00128D6C File Offset: 0x00126F6C
		private void SetSelectedRecipe(StationRecipeEntry entry)
		{
			this.selectedRecipe = entry;
			if (entry != null)
			{
				this.SelectionIndicator.position = entry.transform.position;
				this.SelectionIndicator.gameObject.SetActive(true);
				return;
			}
			this.SelectionIndicator.gameObject.SetActive(false);
		}

		// Token: 0x04003486 RID: 13446
		public List<StationRecipe> Recipes = new List<StationRecipe>();

		// Token: 0x04003487 RID: 13447
		[Header("Prefabs")]
		public StationRecipeEntry RecipeEntryPrefab;

		// Token: 0x04003488 RID: 13448
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003489 RID: 13449
		public RectTransform Container;

		// Token: 0x0400348A RID: 13450
		public RectTransform InputSlotsContainer;

		// Token: 0x0400348B RID: 13451
		public ItemSlotUI[] InputSlotUIs;

		// Token: 0x0400348C RID: 13452
		public ItemSlotUI OutputSlotUI;

		// Token: 0x0400348D RID: 13453
		public RectTransform RecipeSelectionContainer;

		// Token: 0x0400348E RID: 13454
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x0400348F RID: 13455
		public Button BeginButton;

		// Token: 0x04003490 RID: 13456
		public RectTransform SelectionIndicator;

		// Token: 0x04003491 RID: 13457
		public RectTransform RecipeContainer;

		// Token: 0x04003492 RID: 13458
		public RectTransform CookingInProgressContainer;

		// Token: 0x04003493 RID: 13459
		public StationRecipeEntry InProgressRecipeEntry;

		// Token: 0x04003494 RID: 13460
		public TextMeshProUGUI InProgressLabel;

		// Token: 0x04003495 RID: 13461
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x04003496 RID: 13462
		private List<StationRecipeEntry> recipeEntries = new List<StationRecipeEntry>();

		// Token: 0x04003497 RID: 13463
		private StationRecipeEntry selectedRecipe;
	}
}
