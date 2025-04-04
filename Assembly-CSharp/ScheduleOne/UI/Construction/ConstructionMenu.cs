using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000B50 RID: 2896
	public class ConstructionMenu : Singleton<ConstructionMenu>
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06004CF1 RID: 19697 RVA: 0x00144C43 File Offset: 0x00142E43
		// (set) Token: 0x06004CF2 RID: 19698 RVA: 0x00144C4B File Offset: 0x00142E4B
		public bool isOpen { get; protected set; }

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x00144C54 File Offset: 0x00142E54
		public Constructable SelectedConstructable
		{
			get
			{
				return this.selectedConstructable;
			}
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00144C5C File Offset: 0x00142E5C
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(false);
			ConstructionManager instance = Singleton<ConstructionManager>.Instance;
			instance.onConstructionModeEnabled = (Action)Delegate.Combine(instance.onConstructionModeEnabled, new Action(delegate()
			{
				this.SetIsOpen(true);
			}));
			ConstructionManager instance2 = Singleton<ConstructionManager>.Instance;
			instance2.onConstructionModeDisabled = (Action)Delegate.Combine(instance2.onConstructionModeDisabled, new Action(delegate()
			{
				this.SetIsOpen(false);
			}));
			ConstructionManager instance3 = Singleton<ConstructionManager>.Instance;
			instance3.onNewConstructableBuilt = (ConstructionManager.ConstructableNotification)Delegate.Combine(instance3.onNewConstructableBuilt, new ConstructionManager.ConstructableNotification(this.OnConstructableBuilt));
			ConstructionManager instance4 = Singleton<ConstructionManager>.Instance;
			instance4.onConstructableMoved = (ConstructionManager.ConstructableNotification)Delegate.Combine(instance4.onConstructableMoved, new ConstructionManager.ConstructableNotification(this.SelectConstructable));
			this.GenerateCategories();
			this.SelectCategory(this.categories[0].categoryName);
			this.SetupListings();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), -1);
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00144D43 File Offset: 0x00142F43
		private void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.selectedConstructable != null)
			{
				exit.used = true;
				this.DeselectConstructable();
			}
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x00144D69 File Offset: 0x00142F69
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.CheckConstructableSelection();
			}
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x00144D79 File Offset: 0x00142F79
		private void SetupListings()
		{
			this.AddListing("small_shed", 2500f, "Multipurpose");
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00144D90 File Offset: 0x00142F90
		private void AddListing(string ID, float price, string category)
		{
			if (Registry.GetConstructable(ID) == null)
			{
				Console.LogWarning("ID not valid!", null);
				return;
			}
			ConstructionMenu.ConstructionMenuCategory constructionMenuCategory = this.categories.Find((ConstructionMenu.ConstructionMenuCategory x) => x.categoryName.ToLower() == category.ToLower());
			if (constructionMenuCategory == null)
			{
				Console.LogWarning("Category not found!", null);
				return;
			}
			new ConstructionMenu.ConstructionMenuListing(ID, price, constructionMenuCategory);
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00144DF4 File Offset: 0x00142FF4
		private void SetIsOpen(bool open)
		{
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			else
			{
				this.DeselectConstructable();
				if (PlayerSingleton<PlayerCamera>.InstanceExists)
				{
					PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				}
			}
			this.isOpen = open;
			this.canvas.enabled = open;
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnConstructableBuilt(Constructable c)
		{
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x00144E46 File Offset: 0x00143046
		public void ClearSelectedListing()
		{
			if (this.selectedListing != null)
			{
				this.selectedListing.ListingUnselected();
				this.selectedListing = null;
				Singleton<ConstructionManager>.Instance.StopConstructableDeploy();
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x00144E6C File Offset: 0x0014306C
		public void ListingClicked(ConstructionMenu.ConstructionMenuListing listing)
		{
			this.ClearSelectedListing();
			this.DeselectConstructable();
			Singleton<ConstructionManager>.Instance.DeployConstructable(listing);
			this.selectedListing = listing;
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x00144E8C File Offset: 0x0014308C
		public bool IsHoveringUI()
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.position = Input.mousePosition;
			this.raycaster.Raycast(pointerEventData, list);
			return list.Count > 0;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x00144ED1 File Offset: 0x001430D1
		public void MoveButtonPressed()
		{
			if (this.selectedConstructable != null && this.selectedConstructable is Constructable_GridBased)
			{
				Singleton<ConstructionManager>.Instance.MoveConstructable(this.selectedConstructable as Constructable_GridBased);
				this.DeselectConstructable();
			}
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x000045B1 File Offset: 0x000027B1
		public void CustomizeButtonPressed()
		{
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x00144F0C File Offset: 0x0014310C
		public void BulldozeButtonPressed()
		{
			if (this.selectedConstructable != null)
			{
				Constructable constructable = this.selectedConstructable;
				if (!this.selectedConstructable.CanBeDestroyed())
				{
					Console.Log("Can't be destroyed!", null);
					return;
				}
				this.DeselectConstructable();
				constructable.DestroyConstructable(true);
			}
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x00144F54 File Offset: 0x00143154
		private void CheckConstructableSelection()
		{
			if (this.IsHoveringUI())
			{
				return;
			}
			if (Singleton<ConstructionManager>.Instance.isDeployingConstructable)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				Constructable hoveredConstructable = this.GetHoveredConstructable();
				if (hoveredConstructable != null)
				{
					if (this.selectedConstructable == hoveredConstructable)
					{
						this.DeselectConstructable();
						return;
					}
					this.SelectConstructable(hoveredConstructable);
					return;
				}
				else if (this.selectedConstructable != null)
				{
					this.DeselectConstructable();
				}
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x00144FC0 File Offset: 0x001431C0
		public void SelectConstructable(Constructable c)
		{
			this.SelectConstructable(c, true);
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x00144FCC File Offset: 0x001431CC
		public void SelectConstructable(Constructable c, bool focusCameraTo)
		{
			if (!c.CanBeSelected())
			{
				return;
			}
			if (focusCameraTo)
			{
				this.selectedConstructable = c;
			}
			Singleton<BirdsEyeView>.Instance.SlideCameraOrigin(c.GetCosmeticCenter(), c.GetBoundingBoxLongestSide() * 1.75f, 0f);
			this.infoPopup_ConstructableName.text = c.ConstructableName;
			this.infoPopup_Description.text = c.ConstructableDescription;
			List<Button> list = new List<Button>();
			if (c is Constructable_GridBased)
			{
				this.SetButtonInteractable(this.moveButton, true, this.iconColor_Unselected);
				list.Add(this.moveButton);
			}
			else
			{
				this.moveButton.gameObject.SetActive(false);
			}
			this.customizeButton.gameObject.SetActive(false);
			string empty = string.Empty;
			list.Add(this.destroyButton);
			if (c.CanBeDestroyed(out empty))
			{
				this.destroyButton.GetComponent<Tooltip>().text = "Bulldoze";
				this.SetButtonInteractable(this.destroyButton, true, new Color32(byte.MaxValue, 110, 80, byte.MaxValue));
			}
			else
			{
				this.destroyButton.GetComponent<Tooltip>().text = "Cannot bulldoze (" + empty + ")";
				this.SetButtonInteractable(this.destroyButton, false, new Color32(byte.MaxValue, 110, 80, byte.MaxValue));
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)(-(float)list.Count) * 50f * 0.5f + 50f * ((float)i + 0.5f), -25f);
				list[i].gameObject.SetActive(true);
			}
			if (Singleton<FeaturesManager>.Instance.activeConstructable != this.selectedConstructable)
			{
				Singleton<FeaturesManager>.Instance.Activate(this.selectedConstructable);
			}
			this.infoPopup.gameObject.SetActive(true);
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x001451B4 File Offset: 0x001433B4
		private void SetButtonInteractable(Button b, bool interactable, Color32 iconDefaultColor)
		{
			b.interactable = interactable;
			if (interactable)
			{
				b.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = iconDefaultColor;
				return;
			}
			b.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = new Color32(200, 200, 200, byte.MaxValue);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x00145224 File Offset: 0x00143424
		public void DeselectConstructable()
		{
			this.selectedConstructable = null;
			this.infoPopup.gameObject.SetActive(false);
			if (Singleton<FeaturesManager>.Instance.isActive)
			{
				Singleton<FeaturesManager>.Instance.Deactivate();
			}
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x00145254 File Offset: 0x00143454
		private Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(1000f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Constructable>();
			}
			return null;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0014529C File Offset: 0x0014349C
		private void GenerateCategories()
		{
			for (int i = 0; i < this.categories.Count; i++)
			{
				Button component = UnityEngine.Object.Instantiate<GameObject>(this.categoryButtonPrefab, this.categoryButtonContainer).GetComponent<Button>();
				component.GetComponent<RectTransform>().anchoredPosition = new Vector2((0.5f + (float)(i % 3)) * 50f, -(0.5f + (float)(i / 3)) * 50f);
				component.transform.Find("Outline/Background/Icon").GetComponent<Image>().sprite = this.categories[i].categoryIcon;
				string catName = this.categories[i].categoryName;
				component.onClick.AddListener(delegate()
				{
					this.SelectCategory(catName);
				});
				component.GetComponent<Tooltip>().text = this.categories[i].categoryName;
				this.categories[i].button = component;
				RectTransform component2 = UnityEngine.Object.Instantiate<GameObject>(this.categoryContainerPrefab, this.categoryContainer).GetComponent<RectTransform>();
				component2.name = this.categories[i].categoryName;
				component2.gameObject.SetActive(false);
				this.categories[i].container = component2;
			}
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x001453F0 File Offset: 0x001435F0
		public void SelectCategory(string categoryName)
		{
			this.ClearSelectedListing();
			ConstructionMenu.ConstructionMenuCategory constructionMenuCategory = this.categories.Find((ConstructionMenu.ConstructionMenuCategory x) => x.categoryName.ToLower() == categoryName.ToLower());
			if (this.selectedCategory != null)
			{
				this.selectedCategory.button.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = this.iconColor_Unselected;
				this.selectedCategory.button.interactable = true;
				this.selectedCategory.container.gameObject.SetActive(false);
			}
			constructionMenuCategory.button.interactable = false;
			constructionMenuCategory.button.transform.Find("Outline/Background/Icon").GetComponent<Image>().color = this.iconColor_Selected;
			constructionMenuCategory.container.gameObject.SetActive(true);
			this.categoryNameDisplay.text = constructionMenuCategory.categoryName;
			this.selectedCategory = constructionMenuCategory;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x001454DC File Offset: 0x001436DC
		public float GetListingPrice(string id)
		{
			for (int i = 0; i < this.categories.Count; i++)
			{
				for (int j = 0; j < this.categories[i].listings.Count; j++)
				{
					if (this.categories[i].listings[j].ID == id)
					{
						return this.categories[i].listings[j].price;
					}
				}
			}
			Console.LogWarning("Failed to get listing price for ID: " + id, null);
			return 0f;
		}

		// Token: 0x04003A3D RID: 14909
		public List<ConstructionMenu.ConstructionMenuCategory> categories = new List<ConstructionMenu.ConstructionMenuCategory>();

		// Token: 0x04003A3E RID: 14910
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003A3F RID: 14911
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x04003A40 RID: 14912
		[SerializeField]
		protected Transform categoryButtonContainer;

		// Token: 0x04003A41 RID: 14913
		[SerializeField]
		protected RectTransform categoryContainer;

		// Token: 0x04003A42 RID: 14914
		[SerializeField]
		protected Text categoryNameDisplay;

		// Token: 0x04003A43 RID: 14915
		[SerializeField]
		protected RectTransform infoPopup;

		// Token: 0x04003A44 RID: 14916
		[SerializeField]
		protected TextMeshProUGUI infoPopup_ConstructableName;

		// Token: 0x04003A45 RID: 14917
		[SerializeField]
		protected EventSystem eventSystem;

		// Token: 0x04003A46 RID: 14918
		[SerializeField]
		protected Button destroyButton;

		// Token: 0x04003A47 RID: 14919
		[SerializeField]
		protected Button customizeButton;

		// Token: 0x04003A48 RID: 14920
		[SerializeField]
		protected Button moveButton;

		// Token: 0x04003A49 RID: 14921
		[SerializeField]
		protected TextMeshProUGUI infoPopup_Description;

		// Token: 0x04003A4A RID: 14922
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject categoryButtonPrefab;

		// Token: 0x04003A4B RID: 14923
		[SerializeField]
		protected GameObject categoryContainerPrefab;

		// Token: 0x04003A4C RID: 14924
		public GameObject listingPrefab;

		// Token: 0x04003A4D RID: 14925
		[Header("Settings")]
		[SerializeField]
		protected Color iconColor_Unselected;

		// Token: 0x04003A4E RID: 14926
		[SerializeField]
		protected Color iconColor_Selected;

		// Token: 0x04003A4F RID: 14927
		public Color listingOutlineColor_Unselected;

		// Token: 0x04003A50 RID: 14928
		public Color listingOutlineColor_Selected;

		// Token: 0x04003A51 RID: 14929
		private ConstructionMenu.ConstructionMenuCategory selectedCategory;

		// Token: 0x04003A52 RID: 14930
		private ConstructionMenu.ConstructionMenuListing selectedListing;

		// Token: 0x04003A53 RID: 14931
		private Constructable selectedConstructable;

		// Token: 0x02000B51 RID: 2897
		[Serializable]
		public class ConstructionMenuCategory
		{
			// Token: 0x04003A54 RID: 14932
			public string categoryName = "Category";

			// Token: 0x04003A55 RID: 14933
			public Sprite categoryIcon;

			// Token: 0x04003A56 RID: 14934
			[HideInInspector]
			public Button button;

			// Token: 0x04003A57 RID: 14935
			[HideInInspector]
			public RectTransform container;

			// Token: 0x04003A58 RID: 14936
			[HideInInspector]
			public List<ConstructionMenu.ConstructionMenuListing> listings = new List<ConstructionMenu.ConstructionMenuListing>();
		}

		// Token: 0x02000B52 RID: 2898
		public class ConstructionMenuListing
		{
			// Token: 0x06004D0E RID: 19726 RVA: 0x001455BA File Offset: 0x001437BA
			public ConstructionMenuListing(string id, float _price, ConstructionMenu.ConstructionMenuCategory _cat)
			{
				this.ID = id;
				this.price = _price;
				this.category = _cat;
				this.category.listings.Add(this);
				this.CreateUI();
			}

			// Token: 0x06004D0F RID: 19727 RVA: 0x001455FC File Offset: 0x001437FC
			private void CreateUI()
			{
				int num = this.category.listings.IndexOf(this);
				this.entry = UnityEngine.Object.Instantiate<GameObject>(Singleton<ConstructionMenu>.Instance.listingPrefab, this.category.container).GetComponent<RectTransform>();
				this.entry.anchoredPosition = new Vector2((0.5f + (float)num) * this.entry.sizeDelta.x, this.entry.anchoredPosition.y);
				this.entry.Find("Content/Icon").GetComponent<Image>().sprite = Registry.GetConstructable(this.ID).ConstructableIcon;
				this.entry.Find("Content/Name").GetComponent<Text>().text = Registry.GetConstructable(this.ID).ConstructableName;
				this.entry.Find("Content/Price").GetComponent<Text>().text = MoneyManager.FormatAmount(this.price, false, false);
				this.entry.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ListingClicked));
			}

			// Token: 0x06004D10 RID: 19728 RVA: 0x00145715 File Offset: 0x00143915
			private void ListingClicked()
			{
				if (this.isSelected)
				{
					Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
					return;
				}
				Singleton<ConstructionMenu>.Instance.ListingClicked(this);
				this.SetSelected(true);
			}

			// Token: 0x06004D11 RID: 19729 RVA: 0x0014573C File Offset: 0x0014393C
			public void ListingUnselected()
			{
				this.SetSelected(false);
			}

			// Token: 0x06004D12 RID: 19730 RVA: 0x00145748 File Offset: 0x00143948
			public void SetSelected(bool selected)
			{
				this.isSelected = selected;
				if (selected)
				{
					this.entry.Find("Content/Outline").GetComponent<Image>().color = Singleton<ConstructionMenu>.Instance.listingOutlineColor_Selected;
					this.entry.Find("Content/Name").GetComponent<Text>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					return;
				}
				this.entry.Find("Content/Outline").GetComponent<Image>().color = Singleton<ConstructionMenu>.Instance.listingOutlineColor_Unselected;
				this.entry.Find("Content/Name").GetComponent<Text>().color = new Color32(50, 50, 50, byte.MaxValue);
			}

			// Token: 0x04003A59 RID: 14937
			public string ID = string.Empty;

			// Token: 0x04003A5A RID: 14938
			public float price;

			// Token: 0x04003A5B RID: 14939
			public ConstructionMenu.ConstructionMenuCategory category;

			// Token: 0x04003A5C RID: 14940
			public RectTransform entry;

			// Token: 0x04003A5D RID: 14941
			public bool isSelected;
		}
	}
}
