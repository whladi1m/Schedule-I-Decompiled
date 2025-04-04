using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks.Tasks;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A4C RID: 2636
	public class MixingStationCanvas : Singleton<MixingStationCanvas>
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x0600470D RID: 18189 RVA: 0x00129DE6 File Offset: 0x00127FE6
		// (set) Token: 0x0600470E RID: 18190 RVA: 0x00129DEE File Offset: 0x00127FEE
		public bool isOpen { get; protected set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x0600470F RID: 18191 RVA: 0x00129DF7 File Offset: 0x00127FF7
		// (set) Token: 0x06004710 RID: 18192 RVA: 0x00129DFF File Offset: 0x00127FFF
		public MixingStation MixingStation { get; protected set; }

		// Token: 0x06004711 RID: 18193 RVA: 0x00129E08 File Offset: 0x00128008
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00129E2C File Offset: 0x0012802C
		protected override void Start()
		{
			base.Start();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(true);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00129E6C File Offset: 0x0012806C
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.used = true;
			if (Singleton<NewMixScreen>.Instance.IsOpen)
			{
				Singleton<NewMixScreen>.Instance.Close();
			}
			this.Close(true);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00129EB9 File Offset: 0x001280B9
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				this.UpdateInput();
				this.UpdateUI();
			}
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x000045B1 File Offset: 0x000027B1
		private void UpdateUI()
		{
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00129EEC File Offset: 0x001280EC
		private void UpdateInput()
		{
			this.UpdateDisplayMode();
			this.UpdateInstruction();
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00129EFC File Offset: 0x001280FC
		public void Open(MixingStation station)
		{
			this.isOpen = true;
			this.MixingStation = station;
			this.UpdateUI();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("MixingHintsShown"))
			{
				this.MixerHint.gameObject.SetActive(true);
				this.ProductHint.gameObject.SetActive(true);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("MixingHintsShown", true.ToString(), true);
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			this.ProductSlotUI.AssignSlot(station.ProductSlot);
			this.IngredientSlotUI.AssignSlot(station.MixerSlot);
			this.OutputSlotUI.AssignSlot(station.OutputSlot);
			ItemSlot productSlot = station.ProductSlot;
			productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot mixerSlot = station.MixerSlot;
			mixerSlot.onItemDataChanged = (Action)Delegate.Combine(mixerSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot outputSlot = station.OutputSlot;
			outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> list = new List<ItemSlot>();
			list.Add(station.ProductSlot);
			list.Add(station.MixerSlot);
			list.Add(station.OutputSlot);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			this.UpdateDisplayMode();
			this.UpdateInstruction();
			this.UpdatePreview();
			this.UpdateBeginButton();
			ProductDefinition productDefinition;
			if (station.IsMixingDone && !station.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				List<Property> properties;
				station.CurrentMixOperation.GetOutput(out properties);
				ProductDefinition item = Registry.GetItem<ProductDefinition>(this.MixingStation.CurrentMixOperation.ProductID);
				station.DiscoveryBox.ShowProduct(item, properties);
				station.DiscoveryBox.transform.SetParent(PlayerSingleton<PlayerCamera>.Instance.transform);
				station.DiscoveryBox.transform.localPosition = station.DiscoveryBoxOffset;
				station.DiscoveryBox.transform.localRotation = station.DiscoveryBoxRotation;
				float productMarketValue = ProductManager.CalculateProductValue(item.BasePrice, properties);
				Singleton<NewMixScreen>.Instance.Open(properties, item.DrugType, productMarketValue);
				NewMixScreen instance = Singleton<NewMixScreen>.Instance;
				instance.onMixNamed = (Action<string>)Delegate.Remove(instance.onMixNamed, new Action<string>(this.MixNamed));
				NewMixScreen instance2 = Singleton<NewMixScreen>.Instance;
				instance2.onMixNamed = (Action<string>)Delegate.Combine(instance2.onMixNamed, new Action<string>(this.MixNamed));
			}
			else
			{
				station.onMixDone.RemoveListener(new UnityAction(this.MixingDone));
				station.onMixDone.AddListener(new UnityAction(this.MixingDone));
			}
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0012A210 File Offset: 0x00128410
		public void Close(bool enablePlayerControl = true)
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			this.ProductSlotUI.ClearSlot();
			this.IngredientSlotUI.ClearSlot();
			this.OutputSlotUI.ClearSlot();
			ItemSlot productSlot = this.MixingStation.ProductSlot;
			productSlot.onItemDataChanged = (Action)Delegate.Remove(productSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot mixerSlot = this.MixingStation.MixerSlot;
			mixerSlot.onItemDataChanged = (Action)Delegate.Remove(mixerSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			ItemSlot outputSlot = this.MixingStation.OutputSlot;
			outputSlot.onItemDataChanged = (Action)Delegate.Remove(outputSlot.onItemDataChanged, new Action(this.StationContentsChanged));
			this.MixingStation.onMixDone.RemoveListener(new UnityAction(this.MixingDone));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			if (enablePlayerControl)
			{
				this.MixingStation.Close();
				this.MixingStation = null;
			}
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0012A344 File Offset: 0x00128544
		private void MixingDone()
		{
			ProductDefinition productDefinition;
			if (this.MixingStation.IsMixingDone && !this.MixingStation.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				List<Property> properties;
				this.MixingStation.CurrentMixOperation.GetOutput(out properties);
				ProductDefinition item = Registry.GetItem<ProductDefinition>(this.MixingStation.CurrentMixOperation.ProductID);
				this.MixingStation.DiscoveryBox.ShowProduct(item, properties);
				this.MixingStation.DiscoveryBox.transform.SetParent(PlayerSingleton<PlayerCamera>.Instance.transform);
				this.MixingStation.DiscoveryBox.transform.localPosition = this.MixingStation.DiscoveryBoxOffset;
				this.MixingStation.DiscoveryBox.transform.localRotation = this.MixingStation.DiscoveryBoxRotation;
				float productMarketValue = ProductManager.CalculateProductValue(item.BasePrice, properties);
				Singleton<NewMixScreen>.Instance.Open(properties, item.DrugType, productMarketValue);
				NewMixScreen instance = Singleton<NewMixScreen>.Instance;
				instance.onMixNamed = (Action<string>)Delegate.Remove(instance.onMixNamed, new Action<string>(this.MixNamed));
				NewMixScreen instance2 = Singleton<NewMixScreen>.Instance;
				instance2.onMixNamed = (Action<string>)Delegate.Combine(instance2.onMixNamed, new Action<string>(this.MixNamed));
			}
			this.UpdateDisplayMode();
			this.UpdateInstruction();
			this.UpdatePreview();
			this.UpdateBeginButton();
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0012A498 File Offset: 0x00128698
		private void StationContentsChanged()
		{
			this.UpdateDisplayMode();
			this.UpdatePreview();
			this.UpdateBeginButton();
			if (this.MixingStation.ProductSlot.Quantity > 0)
			{
				this.ProductHint.gameObject.SetActive(false);
			}
			if (this.MixingStation.MixerSlot.Quantity > 0)
			{
				this.MixerHint.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x0012A500 File Offset: 0x00128700
		private void UpdateDisplayMode()
		{
			this.TitleContainer.gameObject.SetActive(true);
			this.MainContainer.gameObject.SetActive(true);
			this.OutputSlotUI.gameObject.SetActive(false);
			if (this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.MainContainer.gameObject.SetActive(false);
				this.OutputSlotUI.gameObject.SetActive(true);
				return;
			}
			ProductDefinition productDefinition;
			if (this.MixingStation.CurrentMixOperation != null && this.MixingStation.IsMixingDone && !this.MixingStation.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				this.TitleContainer.gameObject.SetActive(false);
				this.MainContainer.gameObject.SetActive(false);
				this.OutputSlotUI.gameObject.SetActive(false);
				return;
			}
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x0012A5D8 File Offset: 0x001287D8
		private void UpdateInstruction()
		{
			this.InstructionLabel.enabled = true;
			if (this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.InstructionLabel.text = "Collect output";
				return;
			}
			if (this.MixingStation.CurrentMixOperation != null)
			{
				this.InstructionLabel.text = "Mixing in progress...";
				return;
			}
			if (!this.MixingStation.CanStartMix())
			{
				this.InstructionLabel.text = "Insert unpackaged product and mixing ingredient";
				return;
			}
			this.InstructionLabel.enabled = false;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0012A660 File Offset: 0x00128860
		private void UpdatePreview()
		{
			ProductDefinition product = this.MixingStation.GetProduct();
			PropertyItemDefinition mixer = this.MixingStation.GetMixer();
			if (product != null)
			{
				this.ProductPropertiesLabel.text = this.GetPropertyListString(product.Properties);
				this.ProductPropertiesLabel.enabled = true;
			}
			else
			{
				this.ProductPropertiesLabel.enabled = false;
			}
			if (mixer == null && this.MixingStation.MixerSlot.Quantity > 0)
			{
				this.IngredientProblemLabel.enabled = true;
			}
			else
			{
				this.IngredientProblemLabel.enabled = false;
			}
			this.UnknownOutputIcon.gameObject.SetActive(false);
			if (!(product != null) || !(mixer != null))
			{
				this.PreviewIcon.enabled = false;
				this.PreviewLabel.enabled = false;
				this.PreviewPropertiesLabel.enabled = false;
				return;
			}
			List<Property> outputProperties = this.GetOutputProperties(product, mixer);
			ProductDefinition knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			if (knownProduct == null)
			{
				this.PreviewIcon.sprite = product.Icon;
				this.PreviewIcon.color = Color.black;
				this.PreviewIcon.enabled = true;
				this.PreviewLabel.text = "Unknown";
				this.PreviewLabel.enabled = true;
				this.UnknownOutputIcon.gameObject.SetActive(true);
				this.PreviewPropertiesLabel.text = string.Empty;
				for (int i = 0; i < outputProperties.Count; i++)
				{
					if (product.Properties.Contains(outputProperties[i]))
					{
						if (this.PreviewPropertiesLabel.text.Length > 0)
						{
							TextMeshProUGUI previewPropertiesLabel = this.PreviewPropertiesLabel;
							previewPropertiesLabel.text += "\n";
						}
						TextMeshProUGUI previewPropertiesLabel2 = this.PreviewPropertiesLabel;
						previewPropertiesLabel2.text += this.GetPropertyString(outputProperties[i]);
					}
					else
					{
						if (this.PreviewPropertiesLabel.text.Length > 0)
						{
							TextMeshProUGUI previewPropertiesLabel3 = this.PreviewPropertiesLabel;
							previewPropertiesLabel3.text += "\n";
						}
						TextMeshProUGUI previewPropertiesLabel4 = this.PreviewPropertiesLabel;
						previewPropertiesLabel4.text = previewPropertiesLabel4.text + "<color=#" + ColorUtility.ToHtmlStringRGBA(outputProperties[i].LabelColor) + ">• ?</color>";
					}
				}
				this.PreviewPropertiesLabel.enabled = true;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.PreviewPropertiesLabel.rectTransform);
				return;
			}
			this.PreviewIcon.sprite = knownProduct.Icon;
			this.PreviewIcon.color = Color.white;
			this.PreviewIcon.enabled = true;
			this.PreviewLabel.text = knownProduct.Name;
			this.PreviewLabel.enabled = true;
			this.UnknownOutputIcon.gameObject.SetActive(false);
			this.PreviewPropertiesLabel.text = this.GetPropertyListString(knownProduct.Properties);
			this.PreviewPropertiesLabel.enabled = true;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.PreviewPropertiesLabel.rectTransform);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0012A970 File Offset: 0x00128B70
		private string GetPropertyListString(List<Property> properties)
		{
			string text = "";
			for (int i = 0; i < properties.Count; i++)
			{
				if (i > 0)
				{
					text += "\n";
				}
				text += this.GetPropertyString(properties[i]);
			}
			return text;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x00118F9F File Offset: 0x0011719F
		private string GetPropertyString(Property property)
		{
			return string.Concat(new string[]
			{
				"<color=#",
				ColorUtility.ToHtmlStringRGBA(property.LabelColor),
				">• ",
				property.Name,
				"</color>"
			});
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x0012A9BC File Offset: 0x00128BBC
		private List<Property> GetOutputProperties(ProductDefinition product, PropertyItemDefinition mixer)
		{
			List<Property> properties = product.Properties;
			List<Property> properties2 = mixer.Properties;
			return PropertyMixCalculator.MixProperties(properties, properties2[0], product.DrugType);
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0012A9E8 File Offset: 0x00128BE8
		private bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			knownProduct = null;
			ProductDefinition product = this.MixingStation.GetProduct();
			PropertyItemDefinition mixer = this.MixingStation.GetMixer();
			if (product != null && mixer != null)
			{
				List<Property> outputProperties = this.GetOutputProperties(product, mixer);
				knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			}
			return knownProduct != null;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0012AA54 File Offset: 0x00128C54
		private void UpdateBeginButton()
		{
			if (this.MixingStation.CurrentMixOperation != null || this.MixingStation.OutputSlot.Quantity > 0)
			{
				this.BeginButton.gameObject.SetActive(false);
				return;
			}
			this.BeginButton.gameObject.SetActive(true);
			this.BeginButton.interactable = this.MixingStation.CanStartMix();
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x0012AABC File Offset: 0x00128CBC
		public void BeginButtonPressed()
		{
			int mixQuantity = this.MixingStation.GetMixQuantity();
			if (mixQuantity <= 0)
			{
				Console.LogWarning("Failed to start mixing operation, not enough ingredients or output slot is full", null);
				return;
			}
			bool flag = false;
			if (Application.isEditor && Input.GetKey(KeyCode.R))
			{
				flag = true;
			}
			if (this.MixingStation.RequiresIngredientInsertion && !flag)
			{
				MixingStation mixingStation = this.MixingStation;
				this.Close(false);
				new UseMixingStationTask(mixingStation);
				return;
			}
			ProductItemInstance productItemInstance = this.MixingStation.ProductSlot.ItemInstance as ProductItemInstance;
			string id = this.MixingStation.MixerSlot.ItemInstance.ID;
			this.MixingStation.ProductSlot.ChangeQuantity(-mixQuantity, false);
			this.MixingStation.MixerSlot.ChangeQuantity(-mixQuantity, false);
			this.StartMixOperation(new MixOperation(productItemInstance.ID, productItemInstance.Quality, id, mixQuantity));
			this.Close(true);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0012AB94 File Offset: 0x00128D94
		public void StartMixOperation(MixOperation mixOperation)
		{
			this.MixingStation.SendMixingOperation(mixOperation, 0);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Started", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Started") + 1f).ToString(), true);
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x0012ABDC File Offset: 0x00128DDC
		private void MixNamed(string mixName)
		{
			if (this.MixingStation == null)
			{
				Console.LogWarning("Mixing station is null, cannot finish mix operation", null);
				return;
			}
			if (this.MixingStation.CurrentMixOperation == null)
			{
				Console.LogWarning("Mixing station current mix operation is null, cannot finish mix operation", null);
				return;
			}
			NetworkSingleton<ProductManager>.Instance.FinishAndNameMix(this.MixingStation.CurrentMixOperation.ProductID, this.MixingStation.CurrentMixOperation.IngredientID, mixName);
			this.MixingStation.TryCreateOutputItems();
			this.MixingStation.DiscoveryBox.gameObject.SetActive(false);
			this.UpdateDisplayMode();
		}

		// Token: 0x040034C5 RID: 13509
		[Header("Prefabs")]
		public StationRecipeEntry RecipeEntryPrefab;

		// Token: 0x040034C6 RID: 13510
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040034C7 RID: 13511
		public RectTransform Container;

		// Token: 0x040034C8 RID: 13512
		public ItemSlotUI ProductSlotUI;

		// Token: 0x040034C9 RID: 13513
		public TextMeshProUGUI ProductPropertiesLabel;

		// Token: 0x040034CA RID: 13514
		public ItemSlotUI IngredientSlotUI;

		// Token: 0x040034CB RID: 13515
		public TextMeshProUGUI IngredientProblemLabel;

		// Token: 0x040034CC RID: 13516
		public ItemSlotUI PreviewSlotUI;

		// Token: 0x040034CD RID: 13517
		public Image PreviewIcon;

		// Token: 0x040034CE RID: 13518
		public TextMeshProUGUI PreviewLabel;

		// Token: 0x040034CF RID: 13519
		public RectTransform UnknownOutputIcon;

		// Token: 0x040034D0 RID: 13520
		public TextMeshProUGUI PreviewPropertiesLabel;

		// Token: 0x040034D1 RID: 13521
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040034D2 RID: 13522
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040034D3 RID: 13523
		public RectTransform TitleContainer;

		// Token: 0x040034D4 RID: 13524
		public RectTransform MainContainer;

		// Token: 0x040034D5 RID: 13525
		public Button BeginButton;

		// Token: 0x040034D6 RID: 13526
		public RectTransform ProductHint;

		// Token: 0x040034D7 RID: 13527
		public RectTransform MixerHint;

		// Token: 0x040034D8 RID: 13528
		private StationRecipe selectedRecipe;
	}
}
