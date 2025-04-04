using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009A2 RID: 2466
	public class BodySearchScreen : Singleton<BodySearchScreen>
	{
		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060042A4 RID: 17060 RVA: 0x0011724F File Offset: 0x0011544F
		// (set) Token: 0x060042A5 RID: 17061 RVA: 0x00117257 File Offset: 0x00115457
		public bool IsOpen { get; private set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060042A6 RID: 17062 RVA: 0x00117260 File Offset: 0x00115460
		// (set) Token: 0x060042A7 RID: 17063 RVA: 0x00117268 File Offset: 0x00115468
		public bool TutorialOpen { get; private set; }

		// Token: 0x060042A8 RID: 17064 RVA: 0x00117274 File Offset: 0x00115474
		protected override void Start()
		{
			base.Start();
			if (Player.Local != null)
			{
				this.SetupSlots();
			}
			else
			{
				Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.SetupSlots));
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x001172DC File Offset: 0x001154DC
		private void SetupSlots()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.SetupSlots));
			for (int i = 0; i < 8; i++)
			{
				ItemSlotUI slot = UnityEngine.Object.Instantiate<ItemSlotUI>(this.ItemSlotPrefab, this.SlotContainer);
				slot.AssignSlot(PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i]);
				this.slots.Add(slot);
				EventTrigger eventTrigger = slot.Rect.gameObject.AddComponent<EventTrigger>();
				eventTrigger.triggers = new List<EventTrigger.Entry>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerDown;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.SlotHeld(slot);
				});
				eventTrigger.triggers.Add(entry);
				EventTrigger.Entry entry2 = new EventTrigger.Entry();
				entry2.eventID = EventTriggerType.PointerUp;
				entry2.callback.AddListener(delegate(BaseEventData data)
				{
					this.SlotReleased(slot);
				});
				eventTrigger.triggers.Add(entry2);
			}
			this.defaultSlotColor = this.slots[0].normalColor;
			this.defaultSlotHighlightColor = this.slots[0].highlightColor;
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00117424 File Offset: 0x00115624
		private void Update()
		{
			if (this.hoveredSlot != null)
			{
				this.hoveredSlot.SetHighlighted(this.hoveredSlot != this.concealedSlot);
			}
			if (this.IsOpen)
			{
				if (GameInput.GetButton(GameInput.ButtonCode.Jump))
				{
					this.speedBoost = Mathf.MoveTowards(this.speedBoost, 2.5f, Time.deltaTime * 6f);
				}
				else
				{
					this.speedBoost = Mathf.MoveTowards(this.speedBoost, 0f, Time.deltaTime * 6f);
				}
				if (Player.Local != null && Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x001174D4 File Offset: 0x001156D4
		public void Open(NPC _searcher, float searchTime = 0f)
		{
			BodySearchScreen.<>c__DisplayClass37_0 CS$<>8__locals1 = new BodySearchScreen.<>c__DisplayClass37_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.searchTime = searchTime;
			this.IsOpen = true;
			this.searcher = _searcher;
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i].assignedSlot.ItemInstance != null && this.slots[i].assignedSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
				{
					this.slots[i].SetNormalColor(this.SlotRedColor);
					this.slots[i].SetHighlightColor(this.SlotHighlightRedColor);
				}
				else
				{
					this.slots[i].SetNormalColor(this.defaultSlotColor);
					this.slots[i].SetHighlightColor(this.defaultSlotHighlightColor);
				}
				this.slots[i].SetHighlighted(false);
			}
			this.concealedSlot = null;
			base.StartCoroutine(CS$<>8__locals1.<Open>g__Search|0());
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00117649 File Offset: 0x00115849
		private bool IsSlotConcealed(ItemSlotUI slot)
		{
			return this.concealedSlot == slot;
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x00117657 File Offset: 0x00115857
		private void ItemDetected(ItemSlotUI slot)
		{
			this.IndicatorAnimation.Play("Police icon discover");
			if (this.onSearchFail != null)
			{
				this.onSearchFail.Invoke();
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x00117680 File Offset: 0x00115880
		public void SlotHeld(ItemSlotUI ui)
		{
			this.concealedSlot = ui;
			Image[] componentsInChildren = ui.ItemContainer.GetComponentsInChildren<Image>();
			this.defaultItemIconColors = new Color[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.defaultItemIconColors[i] = componentsInChildren[i].color;
				componentsInChildren[i].color = Color.black;
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x001176E0 File Offset: 0x001158E0
		public void SlotReleased(ItemSlotUI ui)
		{
			this.concealedSlot = null;
			Image[] componentsInChildren = ui.ItemContainer.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = this.defaultItemIconColors[i];
			}
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x00117724 File Offset: 0x00115924
		public void Close(bool clear)
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			if (clear && this.onSearchClear != null)
			{
				this.onSearchClear.Invoke();
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x001177C1 File Offset: 0x001159C1
		private void OpenTutorial()
		{
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x001177E7 File Offset: 0x001159E7
		public void CloseTutorial()
		{
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x04003091 RID: 12433
		public const float MAX_SPEED_BOOST = 2.5f;

		// Token: 0x04003094 RID: 12436
		public Color SlotRedColor = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x04003095 RID: 12437
		public Color SlotHighlightRedColor = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x04003096 RID: 12438
		public float GapTime = 0.2f;

		// Token: 0x04003097 RID: 12439
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003098 RID: 12440
		public RectTransform Container;

		// Token: 0x04003099 RID: 12441
		public RectTransform MinigameController;

		// Token: 0x0400309A RID: 12442
		public RectTransform SlotContainer;

		// Token: 0x0400309B RID: 12443
		public ItemSlotUI ItemSlotPrefab;

		// Token: 0x0400309C RID: 12444
		public RectTransform SearchIndicator;

		// Token: 0x0400309D RID: 12445
		public RectTransform SearchIndicatorStart;

		// Token: 0x0400309E RID: 12446
		public RectTransform SearchIndicatorEnd;

		// Token: 0x0400309F RID: 12447
		public Animation IndicatorAnimation;

		// Token: 0x040030A0 RID: 12448
		public Animation TutorialAnimation;

		// Token: 0x040030A1 RID: 12449
		public RectTransform TutorialContainer;

		// Token: 0x040030A2 RID: 12450
		public Animation ResetAnimation;

		// Token: 0x040030A3 RID: 12451
		private List<ItemSlotUI> slots = new List<ItemSlotUI>();

		// Token: 0x040030A4 RID: 12452
		public UnityEvent onSearchClear;

		// Token: 0x040030A5 RID: 12453
		public UnityEvent onSearchFail;

		// Token: 0x040030A6 RID: 12454
		private Color defaultSlotColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040030A7 RID: 12455
		private Color defaultSlotHighlightColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040030A8 RID: 12456
		private ItemSlotUI concealedSlot;

		// Token: 0x040030A9 RID: 12457
		private ItemSlotUI hoveredSlot;

		// Token: 0x040030AA RID: 12458
		private Color[] defaultItemIconColors;

		// Token: 0x040030AB RID: 12459
		private float speedBoost;

		// Token: 0x040030AC RID: 12460
		private NPC searcher;
	}
}
