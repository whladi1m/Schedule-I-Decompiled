using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.Storage;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009B1 RID: 2481
	public class CreateMixInterface : Singleton<CreateMixInterface>
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060042F8 RID: 17144 RVA: 0x001187FB File Offset: 0x001169FB
		// (set) Token: 0x060042F9 RID: 17145 RVA: 0x00118803 File Offset: 0x00116A03
		public bool IsOpen { get; private set; }

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060042FA RID: 17146 RVA: 0x0011880C File Offset: 0x00116A0C
		private ItemSlot beanSlot
		{
			get
			{
				return this.Storage.ItemSlots[0];
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x0011881F File Offset: 0x00116A1F
		private ItemSlot mixerSlot
		{
			get
			{
				return this.Storage.ItemSlots[1];
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060042FC RID: 17148 RVA: 0x00118832 File Offset: 0x00116A32
		private ItemSlot outputSlot
		{
			get
			{
				return this.Storage.ItemSlots[2];
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00118845 File Offset: 0x00116A45
		private ItemSlot productSlot
		{
			get
			{
				return this.Storage.ItemSlots[3];
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00118858 File Offset: 0x00116A58
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.BeansSlot.AssignSlot(this.beanSlot);
			this.MixerSlot.AssignSlot(this.mixerSlot);
			this.OutputSlot.AssignSlot(this.outputSlot);
			this.ProductSlot.AssignSlot(this.productSlot);
			this.beanSlot.AddFilter(new ItemFilter_ID(new List<string>
			{
				"megabean"
			}));
			this.productSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
			{
				EItemCategory.Product
			}));
			this.outputSlot.SetIsAddLocked(true);
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.ContentsChanged));
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginPressed));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0011894D File Offset: 0x00116B4D
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00118978 File Offset: 0x00116B78
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			List<ItemSlot> secondarySlots = new List<ItemSlot>
			{
				this.beanSlot,
				this.productSlot,
				this.mixerSlot
			};
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), secondarySlots);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.ContentsChanged();
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x00118A78 File Offset: 0x00116C78
		private void ContentsChanged()
		{
			this.UpdateCanBegin();
			this.UpdateOutput();
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00118A88 File Offset: 0x00116C88
		private void UpdateCanBegin()
		{
			this.BeanProblemLabel.enabled = !this.HasBeans();
			this.ProductProblemLabel.enabled = !this.HasProduct();
			if (this.HasProduct())
			{
				ProductDefinition productDefinition = this.productSlot.ItemInstance.Definition as ProductDefinition;
				this.ProductPropertiesLabel.text = this.GetPropertyListString(productDefinition.Properties);
				this.ProductPropertiesLabel.enabled = true;
			}
			else
			{
				this.ProductPropertiesLabel.enabled = false;
			}
			if (this.mixerSlot.Quantity == 0)
			{
				this.MixerProblemLabel.text = "Required";
				this.MixerProblemLabel.enabled = true;
			}
			else if (!this.HasMixer())
			{
				this.MixerProblemLabel.text = "Invalid mixer";
				this.MixerProblemLabel.enabled = true;
			}
			else
			{
				this.MixerProblemLabel.enabled = false;
			}
			this.BeginButton.interactable = this.CanBegin();
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00118B7C File Offset: 0x00116D7C
		private void UpdateOutput()
		{
			ProductDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			if (!(product != null) || !(mixer != null))
			{
				this.OutputIcon.enabled = false;
				this.OutputPropertiesLabel.enabled = false;
				this.OutputProblemLabel.enabled = false;
				return;
			}
			List<Property> outputProperties = this.GetOutputProperties(product, mixer);
			ProductDefinition knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			if (knownProduct == null)
			{
				this.OutputIcon.sprite = product.Icon;
				this.OutputIcon.color = Color.black;
				this.OutputIcon.enabled = true;
				this.UnknownOutputIcon.gameObject.SetActive(true);
				List<Color32> list = new List<Color32>();
				this.OutputPropertiesLabel.text = string.Empty;
				for (int i = 0; i < outputProperties.Count; i++)
				{
					if (this.OutputPropertiesLabel.text.Length > 0)
					{
						TextMeshProUGUI outputPropertiesLabel = this.OutputPropertiesLabel;
						outputPropertiesLabel.text += "\n";
					}
					if (product.Properties.Contains(outputProperties[i]))
					{
						TextMeshProUGUI outputPropertiesLabel2 = this.OutputPropertiesLabel;
						outputPropertiesLabel2.text += this.GetPropertyString(outputProperties[i]);
					}
					else
					{
						list.Add(outputProperties[i].LabelColor);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (this.OutputPropertiesLabel.text.Length > 0)
					{
						TextMeshProUGUI outputPropertiesLabel3 = this.OutputPropertiesLabel;
						outputPropertiesLabel3.text += "\n";
					}
					TextMeshProUGUI outputPropertiesLabel4 = this.OutputPropertiesLabel;
					outputPropertiesLabel4.text = outputPropertiesLabel4.text + "<color=#" + ColorUtility.ToHtmlStringRGBA(list[j]) + ">• ?</color>";
				}
				this.OutputPropertiesLabel.enabled = true;
				this.OutputProblemLabel.enabled = false;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.OutputPropertiesLabel.rectTransform);
				return;
			}
			this.OutputIcon.sprite = knownProduct.Icon;
			this.OutputIcon.color = Color.white;
			this.OutputIcon.enabled = true;
			this.UnknownOutputIcon.gameObject.SetActive(false);
			this.OutputPropertiesLabel.text = this.GetPropertyListString(knownProduct.Properties);
			this.OutputPropertiesLabel.enabled = true;
			this.OutputProblemLabel.text = "Mix already known. ";
			this.OutputProblemLabel.enabled = true;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.OutputPropertiesLabel.rectTransform);
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x00118E28 File Offset: 0x00117028
		private void BeginPressed()
		{
			if (!this.CanBegin())
			{
				return;
			}
			ItemDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			NewMixOperation operation = new NewMixOperation(product.ID, mixer.ID);
			NetworkSingleton<ProductManager>.Instance.SendMixOperation(operation, false);
			this.beanSlot.ChangeQuantity(-5, false);
			this.productSlot.ChangeQuantity(-1, false);
			this.mixerSlot.ChangeQuantity(-1, false);
			this.Close();
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00118E98 File Offset: 0x00117098
		private List<Property> GetOutputProperties(ProductDefinition product, PropertyItemDefinition mixer)
		{
			List<Property> properties = product.Properties;
			List<Property> properties2 = mixer.Properties;
			return PropertyMixCalculator.MixProperties(properties, properties2[0], product.DrugType);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x00118EC4 File Offset: 0x001170C4
		private bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			knownProduct = null;
			ProductDefinition product = this.GetProduct();
			PropertyItemDefinition mixer = this.GetMixer();
			if (product != null && mixer != null)
			{
				List<Property> outputProperties = this.GetOutputProperties(product, mixer);
				knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(product.DrugTypes[0].DrugType, outputProperties);
			}
			return knownProduct != null;
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x00118F24 File Offset: 0x00117124
		private string GetPropertyListString(List<Property> properties)
		{
			this.ProductPropertiesLabel.text = "";
			for (int i = 0; i < properties.Count; i++)
			{
				if (i > 0)
				{
					TextMeshProUGUI productPropertiesLabel = this.ProductPropertiesLabel;
					productPropertiesLabel.text += "\n";
				}
				TextMeshProUGUI productPropertiesLabel2 = this.ProductPropertiesLabel;
				productPropertiesLabel2.text += this.GetPropertyString(properties[i]);
			}
			return this.ProductPropertiesLabel.text;
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x00118F9F File Offset: 0x0011719F
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

		// Token: 0x06004309 RID: 17161 RVA: 0x00118FDC File Offset: 0x001171DC
		private bool CanBegin()
		{
			ProductDefinition productDefinition;
			return this.HasBeans() && this.HasProduct() && this.HasMixer() && !this.IsOutputKnown(out productDefinition);
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x00119010 File Offset: 0x00117210
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			if (this.beanSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.beanSlot.ItemInstance.GetCopy(-1));
				this.beanSlot.ClearStoredInstance(false);
			}
			if (this.productSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.productSlot.ItemInstance.GetCopy(-1));
				this.productSlot.ClearStoredInstance(false);
			}
			if (this.mixerSlot.ItemInstance != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.mixerSlot.ItemInstance.GetCopy(-1));
				this.mixerSlot.ClearStoredInstance(false);
			}
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00119163 File Offset: 0x00117363
		private bool HasProduct()
		{
			return this.GetProduct() != null;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00119171 File Offset: 0x00117371
		private bool HasBeans()
		{
			return this.beanSlot.Quantity >= 5;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00119184 File Offset: 0x00117384
		private bool HasMixer()
		{
			return this.GetMixer() != null;
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00119192 File Offset: 0x00117392
		private ProductDefinition GetProduct()
		{
			if (this.productSlot.ItemInstance != null)
			{
				return this.productSlot.ItemInstance.Definition as ProductDefinition;
			}
			return null;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x001191B8 File Offset: 0x001173B8
		private PropertyItemDefinition GetMixer()
		{
			if (this.mixerSlot.ItemInstance != null)
			{
				PropertyItemDefinition propertyItemDefinition = this.mixerSlot.ItemInstance.Definition as PropertyItemDefinition;
				if (propertyItemDefinition != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(propertyItemDefinition))
				{
					return propertyItemDefinition;
				}
			}
			return null;
		}

		// Token: 0x040030E1 RID: 12513
		public const int BEAN_REQUIREMENT = 5;

		// Token: 0x040030E3 RID: 12515
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040030E4 RID: 12516
		public ItemSlotUI BeansSlot;

		// Token: 0x040030E5 RID: 12517
		public ItemSlotUI ProductSlot;

		// Token: 0x040030E6 RID: 12518
		public ItemSlotUI MixerSlot;

		// Token: 0x040030E7 RID: 12519
		public ItemSlotUI OutputSlot;

		// Token: 0x040030E8 RID: 12520
		public Image OutputIcon;

		// Token: 0x040030E9 RID: 12521
		public Button BeginButton;

		// Token: 0x040030EA RID: 12522
		public WorldStorageEntity Storage;

		// Token: 0x040030EB RID: 12523
		public TextMeshProUGUI ProductPropertiesLabel;

		// Token: 0x040030EC RID: 12524
		public TextMeshProUGUI OutputPropertiesLabel;

		// Token: 0x040030ED RID: 12525
		public TextMeshProUGUI BeanProblemLabel;

		// Token: 0x040030EE RID: 12526
		public TextMeshProUGUI ProductProblemLabel;

		// Token: 0x040030EF RID: 12527
		public TextMeshProUGUI MixerProblemLabel;

		// Token: 0x040030F0 RID: 12528
		public TextMeshProUGUI OutputProblemLabel;

		// Token: 0x040030F1 RID: 12529
		public Transform CameraPosition;

		// Token: 0x040030F2 RID: 12530
		public RectTransform UnknownOutputIcon;

		// Token: 0x040030F3 RID: 12531
		public UnityEvent onOpen;

		// Token: 0x040030F4 RID: 12532
		public UnityEvent onClose;
	}
}
