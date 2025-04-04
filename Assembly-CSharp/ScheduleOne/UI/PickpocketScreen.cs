using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FluffyUnderware.DevTools.Extensions;
using GameKit.Utilities;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.UI.Input;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009F8 RID: 2552
	public class PickpocketScreen : Singleton<PickpocketScreen>
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x00120700 File Offset: 0x0011E900
		// (set) Token: 0x060044D9 RID: 17625 RVA: 0x00120708 File Offset: 0x0011E908
		public bool IsOpen { get; private set; }

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060044DA RID: 17626 RVA: 0x00120711 File Offset: 0x0011E911
		// (set) Token: 0x060044DB RID: 17627 RVA: 0x00120719 File Offset: 0x0011E919
		public bool TutorialOpen { get; private set; }

		// Token: 0x060044DC RID: 17628 RVA: 0x00120722 File Offset: 0x0011E922
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0012075C File Offset: 0x0011E95C
		public void Open(NPC _npc)
		{
			this.IsOpen = true;
			this.npc = _npc;
			this.npc.SetIsBeingPickPocketed(true);
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Player.Local.VisualState.ApplyState("pickpocketing", PlayerVisualState.EVisualState.Pickpocketing, 0f);
			ItemSlot[] array = _npc.Inventory.ItemSlots.ToArray();
			array.Shuffle<ItemSlot>();
			for (int i = 0; i < this.Slots.Length; i++)
			{
				if (i < array.Length)
				{
					this.Slots[i].AssignSlot(array[i]);
				}
				else
				{
					this.Slots[i].ClearSlot();
				}
			}
			Singleton<ItemUIManager>.Instance.EnableQuickMove(new List<ItemSlot>(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots()), array.ToList<ItemSlot>());
			for (int j = 0; j < this.Slots.Length; j++)
			{
				ItemSlotUI itemSlotUI = this.Slots[j];
				this.SetSlotLocked(j, true);
				if (itemSlotUI.assignedSlot == null || itemSlotUI.assignedSlot.Quantity == 0)
				{
					this.GreenAreas[j].gameObject.SetActive(false);
				}
				else
				{
					float monetaryValue = itemSlotUI.assignedSlot.ItemInstance.GetMonetaryValue();
					float num = Mathf.Lerp(this.GreenAreaMaxWidth, this.GreenAreaMinWidth, Mathf.Pow(Mathf.Clamp01(monetaryValue / this.ValueDivisor), 0.3f));
					if (Player.Local.Sneaky)
					{
						num *= 1.5f;
					}
					RectTransform rectTransform = this.GreenAreas[j];
					rectTransform.sizeDelta = new Vector2(num, rectTransform.sizeDelta.y);
					rectTransform.gameObject.SetActive(true);
					rectTransform.anchoredPosition = new Vector2(37.5f + 90f * (float)j, rectTransform.anchoredPosition.y);
				}
			}
			this.InputPrompt.SetLabel("Stop Arrow");
			this.isFail = false;
			this.isSliding = true;
			this.sliderPosition = 0f;
			this.slideDirection = 1;
			this.slideTimeMultiplier = 1f;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x001209D0 File Offset: 0x0011EBD0
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

		// Token: 0x060044DF RID: 17631 RVA: 0x001209FC File Offset: 0x0011EBFC
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (this.isFail)
			{
				return;
			}
			if (Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.Close();
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (this.isSliding)
				{
					this.StopArrow();
				}
				else
				{
					this.InputPrompt.SetLabel("Stop Arrow");
					this.isSliding = true;
					ItemSlotUI hoveredSlot = this.GetHoveredSlot();
					if (((hoveredSlot != null) ? hoveredSlot.assignedSlot : null) != null)
					{
						this.GreenAreas[this.Slots.IndexOf(this.GetHoveredSlot())].gameObject.SetActive(false);
					}
				}
			}
			if (this.isSliding)
			{
				this.slideTimeMultiplier = Mathf.Clamp(this.slideTimeMultiplier + Time.deltaTime / 20f, 0f, this.SlideTimeMaxMultiplier);
				if (this.slideDirection == 1)
				{
					this.sliderPosition = Mathf.Clamp01(this.sliderPosition + Time.deltaTime / this.SlideTime * this.slideTimeMultiplier);
					if (this.sliderPosition >= 1f)
					{
						this.slideDirection = -1;
					}
				}
				else
				{
					this.sliderPosition = Mathf.Clamp01(this.sliderPosition - Time.deltaTime / this.SlideTime * this.slideTimeMultiplier);
					if (this.sliderPosition <= 0f)
					{
						this.slideDirection = 1;
					}
				}
			}
			this.Slider.value = this.sliderPosition;
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00120B5C File Offset: 0x0011ED5C
		private void StopArrow()
		{
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
			this.isSliding = false;
			ItemSlotUI hoveredSlot = this.GetHoveredSlot();
			this.InputPrompt.SetLabel("Continue");
			if (hoveredSlot != null)
			{
				NetworkSingleton<LevelManager>.Instance.AddXP(2);
				this.SetSlotLocked(this.Slots.IndexOf(hoveredSlot), false);
				Customer component = this.npc.GetComponent<Customer>();
				if (component != null && component.TimeSinceLastDealCompleted < 60 && hoveredSlot.assignedSlot != null && hoveredSlot.assignedSlot.ItemInstance != null && hoveredSlot.assignedSlot.ItemInstance is ProductItemInstance)
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.INDIAN_DEALER);
				}
				if (this.onHitGreen != null)
				{
					this.onHitGreen.Invoke();
					return;
				}
			}
			else
			{
				this.Fail();
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00120C30 File Offset: 0x0011EE30
		public void SetSlotLocked(int index, bool locked)
		{
			this.Slots[index].Rect.Find("Locked").gameObject.SetActive(locked);
			this.Slots[index].assignedSlot.SetIsAddLocked(locked);
			this.Slots[index].assignedSlot.SetIsRemovalLocked(locked);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00120C88 File Offset: 0x0011EE88
		private ItemSlotUI GetHoveredSlot()
		{
			for (int i = 0; i < this.GreenAreas.Length; i++)
			{
				if (this.GreenAreas[i].gameObject.activeSelf)
				{
					float num = this.GetGreenAreaNormalizedPosition(i) - this.GetGreenAreaNormalizedWidth(i) / 2f;
					float num2 = this.GetGreenAreaNormalizedPosition(i) + this.GetGreenAreaNormalizedWidth(i) / 2f;
					if (this.Slider.value >= num - this.Tolerance && this.Slider.value <= num2 + this.Tolerance)
					{
						return this.Slots[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00120D1F File Offset: 0x0011EF1F
		private void Fail()
		{
			this.isFail = true;
			if (this.onFail != null)
			{
				this.onFail.Invoke();
			}
			base.StartCoroutine(this.<Fail>g__FailCoroutine|40_0());
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x00120D48 File Offset: 0x0011EF48
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			for (int i = 0; i < this.Slots.Length; i++)
			{
				if (this.Slots[i].assignedSlot != null)
				{
					this.Slots[i].assignedSlot.SetIsRemovalLocked(false);
				}
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			Player.Local.VisualState.RemoveState("pickpocketing", 0f);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			this.npc.SetIsBeingPickPocketed(false);
			if (this.isFail)
			{
				this.npc.responses.PlayerFailedPickpocket(Player.Local);
				this.npc.Inventory.ExpirePickpocket();
			}
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00120E62 File Offset: 0x0011F062
		private void OpenTutorial()
		{
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00120E88 File Offset: 0x0011F088
		public void CloseTutorial()
		{
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00120EA2 File Offset: 0x0011F0A2
		private float GetGreenAreaNormalizedPosition(int index)
		{
			return this.GreenAreas[index].anchoredPosition.x / this.SliderContainer.sizeDelta.x;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00120EC7 File Offset: 0x0011F0C7
		private float GetGreenAreaNormalizedWidth(int index)
		{
			return this.GreenAreas[index].sizeDelta.x / this.SliderContainer.sizeDelta.x;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x00120F53 File Offset: 0x0011F153
		[CompilerGenerated]
		private IEnumerator <Fail>g__FailCoroutine|40_0()
		{
			yield return new WaitForSeconds(0.9f);
			if (this.IsOpen)
			{
				this.Close();
			}
			yield break;
		}

		// Token: 0x040032BA RID: 12986
		public const int PICKPOCKET_XP = 2;

		// Token: 0x040032BD RID: 12989
		[Header("Settings")]
		public float GreenAreaMaxWidth = 70f;

		// Token: 0x040032BE RID: 12990
		public float GreenAreaMinWidth = 5f;

		// Token: 0x040032BF RID: 12991
		public float SlideTime = 1f;

		// Token: 0x040032C0 RID: 12992
		public float SlideTimeMaxMultiplier = 2f;

		// Token: 0x040032C1 RID: 12993
		public float ValueDivisor = 300f;

		// Token: 0x040032C2 RID: 12994
		public float Tolerance = 0.01f;

		// Token: 0x040032C3 RID: 12995
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040032C4 RID: 12996
		public RectTransform Container;

		// Token: 0x040032C5 RID: 12997
		public ItemSlotUI[] Slots;

		// Token: 0x040032C6 RID: 12998
		public RectTransform[] GreenAreas;

		// Token: 0x040032C7 RID: 12999
		public Animation TutorialAnimation;

		// Token: 0x040032C8 RID: 13000
		public RectTransform TutorialContainer;

		// Token: 0x040032C9 RID: 13001
		public RectTransform SliderContainer;

		// Token: 0x040032CA RID: 13002
		public Slider Slider;

		// Token: 0x040032CB RID: 13003
		public InputPrompt InputPrompt;

		// Token: 0x040032CC RID: 13004
		public UnityEvent onFail;

		// Token: 0x040032CD RID: 13005
		public UnityEvent onStop;

		// Token: 0x040032CE RID: 13006
		public UnityEvent onHitGreen;

		// Token: 0x040032CF RID: 13007
		private NPC npc;

		// Token: 0x040032D0 RID: 13008
		private bool isSliding;

		// Token: 0x040032D1 RID: 13009
		private int slideDirection = 1;

		// Token: 0x040032D2 RID: 13010
		private float sliderPosition;

		// Token: 0x040032D3 RID: 13011
		private float slideTimeMultiplier = 1f;

		// Token: 0x040032D4 RID: 13012
		private bool isFail;
	}
}
