using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B1F RID: 2847
	public class HandoverScreen : Singleton<HandoverScreen>
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004BCE RID: 19406 RVA: 0x0013ED01 File Offset: 0x0013CF01
		// (set) Token: 0x06004BCF RID: 19407 RVA: 0x0013ED09 File Offset: 0x0013CF09
		public Contract CurrentContract { get; protected set; }

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x0013ED12 File Offset: 0x0013CF12
		// (set) Token: 0x06004BD1 RID: 19409 RVA: 0x0013ED1A File Offset: 0x0013CF1A
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x0013ED23 File Offset: 0x0013CF23
		// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x0013ED2B File Offset: 0x0013CF2B
		public bool TutorialOpen { get; private set; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x0013ED34 File Offset: 0x0013CF34
		// (set) Token: 0x06004BD5 RID: 19413 RVA: 0x0013ED3C File Offset: 0x0013CF3C
		public HandoverScreen.EMode Mode { get; protected set; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x0013ED45 File Offset: 0x0013CF45
		// (set) Token: 0x06004BD7 RID: 19415 RVA: 0x0013ED4D File Offset: 0x0013CF4D
		public Customer CurrentCustomer { get; private set; }

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0013ED58 File Offset: 0x0013CF58
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 8);
			this.VehicleSlotUIs = this.VehicleSlotContainer.GetComponentsInChildren<ItemSlotUI>();
			this.CustomerSlotUIs = this.CustomerSlotContainer.GetComponentsInChildren<ItemSlotUI>();
			this.DoneButton.onClick.AddListener(new UnityAction(this.DonePressed));
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				this.CustomerSlots[i] = new ItemSlot();
				this.CustomerSlotUIs[i].AssignSlot(this.CustomerSlots[i]);
				ItemSlot itemSlot = this.CustomerSlots[i];
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.CustomerItemsChanged));
			}
			this.VehicleSubtitle.text = "This is the vehicle you last drove.\nMust be within " + 20f.ToString() + " meters.";
			this.ClearCustomerSlots(false);
			this.PriceSelector.gameObject.SetActive(false);
			this.PriceSelector.onPriceChanged.AddListener(new UnityAction(this.UpdateSuccessChance));
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.IsOpen = false;
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x0013EE98 File Offset: 0x0013D098
		private void Update()
		{
			if (this.IsOpen && ((Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && Player.Local.CrimeData.TimeSinceSighted < 5f) || Player.Local.CrimeData.CurrentArrestProgress > 0.01f))
			{
				this.Close(HandoverScreen.EHandoverOutcome.Cancelled);
			}
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x0013EEF1 File Offset: 0x0013D0F1
		private void OpenTutorial()
		{
			this.CanvasGroup.alpha = 0f;
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0013EF27 File Offset: 0x0013D127
		public void CloseTutorial()
		{
			this.CanvasGroup.alpha = 1f;
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0013EF54 File Offset: 0x0013D154
		public virtual void Open(Contract contract, Customer customer, HandoverScreen.EMode mode, Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float> callback, Func<List<ItemInstance>, float, float> successChanceMethod)
		{
			if (mode == HandoverScreen.EMode.Contract && contract == null)
			{
				Console.LogWarning("Contract is null", null);
				return;
			}
			this.CurrentContract = contract;
			this.CurrentCustomer = customer;
			this.Mode = mode;
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.TitleLabel.text = "Complete Deal";
			}
			else if (this.Mode == HandoverScreen.EMode.Sample)
			{
				this.TitleLabel.text = "Give Free Sample";
			}
			else if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.TitleLabel.text = "Offer Deal";
			}
			this.DetailPanel.Open(customer);
			this.onHandoverComplete = callback;
			this.SuccessChanceMethod = successChanceMethod;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			List<ItemSlot> secondarySlots = new List<ItemSlot>(this.CustomerSlots);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("ItemAmountSelectionTutorialDone") && GameManager.IS_TUTORIAL)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ItemAmountSelectionTutorialDone", true.ToString(), true);
				this.OpenTutorial();
			}
			else
			{
				Player.Local.VisualState.ApplyState("drugdeal", PlayerVisualState.EVisualState.DrugDealing, 0f);
			}
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.DescriptionLabel.text = customer.NPC.FirstName + " is paying <color=#50E65A>" + MoneyManager.FormatAmount(contract.Payment, false, false) + "</color> for:";
				this.DescriptionLabel.enabled = true;
			}
			else
			{
				this.DescriptionLabel.enabled = false;
			}
			if (this.Mode == HandoverScreen.EMode.Sample)
			{
				EDrugType property = customer.GetOrderedDrugTypes()[0];
				string text = ColorUtility.ToHtmlStringRGB(property.GetColor());
				this.FavouriteDrugLabel.text = string.Concat(new string[]
				{
					customer.NPC.FirstName,
					"'s favourite drug: <color=#",
					text,
					">",
					property.ToString(),
					"</color>"
				});
				this.FavouriteDrugLabel.enabled = true;
				this.FavouritePropertiesLabel.text = customer.NPC.FirstName + "'s favourite effects:";
				for (int i = 0; i < this.PropertiesEntries.Length; i++)
				{
					if (customer.CustomerData.PreferredProperties.Count > i)
					{
						this.PropertiesEntries[i].text = "•  " + customer.CustomerData.PreferredProperties[i].Name;
						this.PropertiesEntries[i].color = customer.CustomerData.PreferredProperties[i].LabelColor;
						this.PropertiesEntries[i].enabled = true;
					}
					else
					{
						this.PropertiesEntries[i].enabled = false;
					}
				}
				this.FavouritePropertiesLabel.gameObject.SetActive(true);
			}
			else
			{
				this.FavouriteDrugLabel.enabled = false;
				this.FavouritePropertiesLabel.gameObject.SetActive(false);
			}
			for (int j = 0; j < this.ExpectationEntries.Length; j++)
			{
				if (contract != null && contract.ProductList.entries.Count > j)
				{
					this.ExpectationEntries[j].Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = "<color=#FFC73D>" + contract.ProductList.entries[j].Quantity.ToString() + "x</color> " + Registry.GetItem(contract.ProductList.entries[j].ProductID).Name;
					this.ExpectationEntries[j].Find("Star").GetComponent<Image>().color = ItemQuality.GetColor(contract.ProductList.entries[j].Quality);
					this.ExpectationEntries[j].Find("Star").GetComponent<RectTransform>().anchoredPosition = new Vector2(-this.ExpectationEntries[j].Find("Title").gameObject.GetComponent<TextMeshProUGUI>().preferredWidth / 2f + 30f, 0f);
					this.ExpectationEntries[j].gameObject.SetActive(true);
				}
				else
				{
					this.ExpectationEntries[j].gameObject.SetActive(false);
				}
			}
			if (Player.Local.LastDrivenVehicle != null && Player.Local.LastDrivenVehicle.Storage != null && Vector3.Distance(Player.Local.LastDrivenVehicle.transform.position, Player.Local.transform.position) < 20f)
			{
				if (Player.Local.LastDrivenVehicle.Storage != null)
				{
					for (int k = 0; k < this.VehicleSlotUIs.Length; k++)
					{
						ItemSlot itemSlot = null;
						if (k < Player.Local.LastDrivenVehicle.Storage.ItemSlots.Count)
						{
							itemSlot = Player.Local.LastDrivenVehicle.Storage.ItemSlots[k];
						}
						if (itemSlot != null)
						{
							this.VehicleSlotUIs[k].AssignSlot(itemSlot);
							this.VehicleSlotUIs[k].gameObject.SetActive(true);
							allInventorySlots.Add(itemSlot);
						}
						else
						{
							this.VehicleSlotUIs[k].gameObject.SetActive(false);
						}
					}
				}
				this.NoVehicle.gameObject.SetActive(false);
				this.VehicleContainer.gameObject.SetActive(true);
			}
			else
			{
				this.NoVehicle.gameObject.SetActive(true);
				this.VehicleContainer.gameObject.SetActive(false);
			}
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.CustomerSubtitle.text = "Place the expected products here";
			}
			else if (this.Mode == HandoverScreen.EMode.Sample)
			{
				this.CustomerSubtitle.text = "Place a product here for " + customer.NPC.FirstName + " to try";
			}
			else if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.CustomerSubtitle.text = "Place product here";
			}
			if (mode == HandoverScreen.EMode.Offer)
			{
				this.PriceSelector.gameObject.SetActive(true);
				this.PriceSelector.SetPrice(1f);
			}
			else
			{
				this.PriceSelector.gameObject.SetActive(false);
			}
			this.RecordOriginalLocations();
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(allInventorySlots, secondarySlots);
			this.CustomerItemsChanged();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.IsOpen = true;
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0013F64C File Offset: 0x0013D84C
		public virtual void Close(HandoverScreen.EHandoverOutcome outcome)
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			List<ItemInstance> list = new List<ItemInstance>();
			if (outcome == HandoverScreen.EHandoverOutcome.Finalize)
			{
				for (int i = 0; i < this.CustomerSlots.Length; i++)
				{
					if (this.CustomerSlots[i].ItemInstance != null)
					{
						list.Add(this.CustomerSlots[i].ItemInstance);
					}
				}
			}
			Singleton<CompassManager>.Instance.SetVisible(true);
			this.CurrentContract = null;
			this.CurrentCustomer = null;
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			float arg = 0f;
			if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.PriceSelector.RefreshPrice();
				arg = this.PriceSelector.Price;
			}
			if (this.onHandoverComplete != null)
			{
				this.onHandoverComplete(outcome, list, arg);
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Player.Local.VisualState.RemoveState("drugdeal", 0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				this.ClearCustomerSlots(true);
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x0013F785 File Offset: 0x0013D985
		public void DonePressed()
		{
			this.Close(HandoverScreen.EHandoverOutcome.Finalize);
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0013F790 File Offset: 0x0013D990
		private void RecordOriginalLocations()
		{
			foreach (HotbarSlot hotbarSlot in PlayerSingleton<PlayerInventory>.Instance.hotbarSlots)
			{
				if (hotbarSlot.ItemInstance != null)
				{
					if (this.OriginalItemLocations.ContainsKey(hotbarSlot.ItemInstance))
					{
						Console.LogWarning("Item already exists in original locations", null);
					}
					else
					{
						this.OriginalItemLocations.Add(hotbarSlot.ItemInstance, HandoverScreen.EItemSource.Player);
					}
				}
			}
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0013F81C File Offset: 0x0013DA1C
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
				if (this.TutorialOpen)
				{
					this.CloseTutorial();
					return;
				}
				this.Close(HandoverScreen.EHandoverOutcome.Cancelled);
			}
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0013F858 File Offset: 0x0013DA58
		public void ClearCustomerSlots(bool returnToOriginals)
		{
			this.ignoreCustomerChangedEvents = true;
			foreach (ItemSlot itemSlot in this.CustomerSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (returnToOriginals)
					{
						PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(itemSlot.ItemInstance);
					}
					itemSlot.ClearStoredInstance(false);
				}
			}
			this.OriginalItemLocations.Clear();
			this.ignoreCustomerChangedEvents = false;
			this.CustomerItemsChanged();
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0013F8C0 File Offset: 0x0013DAC0
		private void CustomerItemsChanged()
		{
			if (this.ignoreCustomerChangedEvents)
			{
				return;
			}
			this.UpdateDoneButton();
			this.UpdateSuccessChance();
			if (this.Mode == HandoverScreen.EMode.Offer)
			{
				float customerItemsValue = this.GetCustomerItemsValue();
				this.PriceSelector.SetPrice(customerItemsValue);
				this.FairPriceLabel.text = "Fair price: " + MoneyManager.FormatAmount(customerItemsValue, false, false);
			}
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x0013F91C File Offset: 0x0013DB1C
		private void UpdateDoneButton()
		{
			string text;
			if (this.GetError(out text))
			{
				this.DoneButton.interactable = false;
				this.ErrorLabel.text = text;
				this.ErrorLabel.enabled = true;
			}
			else
			{
				this.DoneButton.interactable = true;
				this.ErrorLabel.enabled = false;
			}
			string text2;
			if (!this.ErrorLabel.enabled && this.GetWarning(out text2))
			{
				this.WarningLabel.text = text2;
				this.WarningLabel.enabled = true;
				return;
			}
			this.WarningLabel.enabled = false;
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0013F9B0 File Offset: 0x0013DBB0
		private void UpdateSuccessChance()
		{
			if (this.GetCustomerItems(false).Count == 0)
			{
				this.SuccessLabel.enabled = false;
				return;
			}
			float num;
			if (this.Mode == HandoverScreen.EMode.Sample)
			{
				Func<List<ItemInstance>, float, float> successChanceMethod = this.SuccessChanceMethod;
				num = ((successChanceMethod != null) ? successChanceMethod(this.GetCustomerItems(true), 0f) : 0f);
				this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of success";
				this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
				this.SuccessLabel.enabled = true;
				return;
			}
			if (this.Mode != HandoverScreen.EMode.Contract)
			{
				if (this.Mode == HandoverScreen.EMode.Offer)
				{
					float price = this.PriceSelector.Price;
					Func<List<ItemInstance>, float, float> successChanceMethod2 = this.SuccessChanceMethod;
					num = ((successChanceMethod2 != null) ? successChanceMethod2(this.GetCustomerItems(true), price) : 0f);
					this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of success";
					this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
					this.SuccessLabel.enabled = true;
				}
				return;
			}
			if (this.CurrentContract == null)
			{
				Console.LogWarning("Current contract is null", null);
				return;
			}
			int num2;
			num = Mathf.Clamp(this.CurrentContract.GetProductListMatch(this.GetCustomerItems(true), out num2), 0.01f, 1f);
			if (num < 1f)
			{
				this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of customer accepting";
				this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
				this.SuccessLabel.enabled = true;
				return;
			}
			this.SuccessLabel.enabled = false;
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0013FB84 File Offset: 0x0013DD84
		private bool GetError(out string err)
		{
			err = string.Empty;
			if (this.Mode == HandoverScreen.EMode.Contract && this.CurrentContract != null)
			{
				if (this.GetCustomerItemsCount(false) == 0)
				{
					err = string.Empty;
					return true;
				}
				if (NetworkSingleton<GameManager>.Instance.IsTutorial && this.GetCustomerItemsCount(true) > this.CurrentContract.ProductList.GetTotalQuantity())
				{
					err = "You are providing more product than required.";
					return true;
				}
			}
			if ((this.Mode == HandoverScreen.EMode.Sample || this.Mode == HandoverScreen.EMode.Offer) && this.GetCustomerItemsCount(true) == 0)
			{
				bool flag = false;
				for (int i = 0; i < this.CustomerSlots.Length; i++)
				{
					if (this.CustomerSlots[i].ItemInstance != null && this.CustomerSlots[i].ItemInstance is ProductItemInstance && (this.CustomerSlots[i].ItemInstance as ProductItemInstance).AppliedPackaging == null)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					err = "Product must be packaged";
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0013FC74 File Offset: 0x0013DE74
		private bool GetWarning(out string warning)
		{
			warning = string.Empty;
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				if (this.CurrentContract != null)
				{
					int num;
					if (this.CurrentContract.GetProductListMatch(this.GetCustomerItems(true), out num) < 1f)
					{
						warning = "Customer expectations not met";
						return true;
					}
					if (this.GetCustomerItemsCount(false) > this.CurrentContract.ProductList.GetTotalQuantity())
					{
						warning = "You are providing more items than required.";
						return true;
					}
				}
			}
			else if (this.Mode == HandoverScreen.EMode.Sample && this.GetCustomerItemsCount(false) > 1)
			{
				warning = "Only 1 sample product is required.";
				return true;
			}
			bool flag = false;
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null && this.CustomerSlots[i].ItemInstance is ProductItemInstance && (this.CustomerSlots[i].ItemInstance as ProductItemInstance).AppliedPackaging == null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				warning = "Product must be packaged";
				return true;
			}
			return false;
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x0013FD68 File Offset: 0x0013DF68
		private List<ItemInstance> GetCustomerItems(bool onlyPackagedProduct = true)
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null)
				{
					if (onlyPackagedProduct)
					{
						ProductItemInstance productItemInstance = this.CustomerSlots[i].ItemInstance as ProductItemInstance;
						if (productItemInstance == null || productItemInstance.AppliedPackaging == null)
						{
							goto IL_53;
						}
					}
					list.Add(this.CustomerSlots[i].ItemInstance);
				}
				IL_53:;
			}
			return list;
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0013FDD8 File Offset: 0x0013DFD8
		private float GetCustomerItemsValue()
		{
			float num = 0f;
			foreach (ItemInstance itemInstance in this.GetCustomerItems(true))
			{
				if (itemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = itemInstance as ProductItemInstance;
					num += (productItemInstance.Definition as ProductDefinition).MarketValue * (float)productItemInstance.Quantity * (float)productItemInstance.Amount;
				}
			}
			return num;
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0013FE60 File Offset: 0x0013E060
		private int GetCustomerItemsCount(bool onlyPackagedProduct = true)
		{
			int num = 0;
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null)
				{
					ProductItemInstance productItemInstance = this.CustomerSlots[i].ItemInstance as ProductItemInstance;
					if (!onlyPackagedProduct || (productItemInstance != null && !(productItemInstance.AppliedPackaging == null)))
					{
						int num2 = 1;
						if (productItemInstance != null)
						{
							num2 = productItemInstance.Amount;
						}
						num += this.CustomerSlots[i].ItemInstance.Quantity * num2;
					}
				}
			}
			return num;
		}

		// Token: 0x04003912 RID: 14610
		public const int CUSTOMER_SLOT_COUNT = 4;

		// Token: 0x04003913 RID: 14611
		public const float VEHICLE_MAX_DIST = 20f;

		// Token: 0x04003918 RID: 14616
		[Header("Settings")]
		public Gradient SuccessColorMap;

		// Token: 0x04003919 RID: 14617
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400391A RID: 14618
		public GameObject Container;

		// Token: 0x0400391B RID: 14619
		public CanvasGroup CanvasGroup;

		// Token: 0x0400391C RID: 14620
		public TextMeshProUGUI DescriptionLabel;

		// Token: 0x0400391D RID: 14621
		public TextMeshProUGUI CustomerSubtitle;

		// Token: 0x0400391E RID: 14622
		public TextMeshProUGUI FavouriteDrugLabel;

		// Token: 0x0400391F RID: 14623
		public TextMeshProUGUI FavouritePropertiesLabel;

		// Token: 0x04003920 RID: 14624
		public TextMeshProUGUI[] PropertiesEntries;

		// Token: 0x04003921 RID: 14625
		public RectTransform[] ExpectationEntries;

		// Token: 0x04003922 RID: 14626
		public GameObject NoVehicle;

		// Token: 0x04003923 RID: 14627
		public RectTransform VehicleSlotContainer;

		// Token: 0x04003924 RID: 14628
		public RectTransform CustomerSlotContainer;

		// Token: 0x04003925 RID: 14629
		public TextMeshProUGUI VehicleSubtitle;

		// Token: 0x04003926 RID: 14630
		public TextMeshProUGUI SuccessLabel;

		// Token: 0x04003927 RID: 14631
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x04003928 RID: 14632
		public TextMeshProUGUI WarningLabel;

		// Token: 0x04003929 RID: 14633
		public Button DoneButton;

		// Token: 0x0400392A RID: 14634
		public RectTransform VehicleContainer;

		// Token: 0x0400392B RID: 14635
		public TextMeshProUGUI TitleLabel;

		// Token: 0x0400392C RID: 14636
		public HandoverScreenPriceSelector PriceSelector;

		// Token: 0x0400392D RID: 14637
		public TextMeshProUGUI FairPriceLabel;

		// Token: 0x0400392E RID: 14638
		public Animation TutorialAnimation;

		// Token: 0x0400392F RID: 14639
		public RectTransform TutorialContainer;

		// Token: 0x04003930 RID: 14640
		public HandoverScreenDetailPanel DetailPanel;

		// Token: 0x04003931 RID: 14641
		public Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float> onHandoverComplete;

		// Token: 0x04003932 RID: 14642
		public Func<List<ItemInstance>, float, float> SuccessChanceMethod;

		// Token: 0x04003933 RID: 14643
		private ItemSlotUI[] VehicleSlotUIs;

		// Token: 0x04003934 RID: 14644
		private ItemSlotUI[] CustomerSlotUIs;

		// Token: 0x04003935 RID: 14645
		private ItemSlot[] CustomerSlots = new ItemSlot[4];

		// Token: 0x04003936 RID: 14646
		private Dictionary<ItemInstance, HandoverScreen.EItemSource> OriginalItemLocations = new Dictionary<ItemInstance, HandoverScreen.EItemSource>();

		// Token: 0x04003937 RID: 14647
		private bool ignoreCustomerChangedEvents;

		// Token: 0x02000B20 RID: 2848
		public enum EMode
		{
			// Token: 0x0400393A RID: 14650
			Contract,
			// Token: 0x0400393B RID: 14651
			Sample,
			// Token: 0x0400393C RID: 14652
			Offer
		}

		// Token: 0x02000B21 RID: 2849
		public enum EHandoverOutcome
		{
			// Token: 0x0400393E RID: 14654
			Cancelled,
			// Token: 0x0400393F RID: 14655
			Finalize
		}

		// Token: 0x02000B22 RID: 2850
		private enum EItemSource
		{
			// Token: 0x04003941 RID: 14657
			Player,
			// Token: 0x04003942 RID: 14658
			Vehicle
		}
	}
}
