using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B2D RID: 2861
	public class CharacterCustomizationUI : MonoBehaviour
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x001411CC File Offset: 0x0013F3CC
		// (set) Token: 0x06004C31 RID: 19505 RVA: 0x001411D4 File Offset: 0x0013F3D4
		public bool IsOpen { get; private set; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x001411DD File Offset: 0x0013F3DD
		// (set) Token: 0x06004C33 RID: 19507 RVA: 0x001411E5 File Offset: 0x0013F3E5
		public CharacterCustomizationCategory ActiveCategory { get; private set; }

		// Token: 0x06004C34 RID: 19508 RVA: 0x001411EE File Offset: 0x0013F3EE
		private void OnValidate()
		{
			this.Categories = base.GetComponentsInChildren<CharacterCustomizationCategory>(true);
			this.TitleText.text = this.Title;
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x00141210 File Offset: 0x0013F410
		private void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.RigRotationSlider.onValueChanged.AddListener(delegate(float value)
			{
				this.rigTargetY = value * 359f;
			});
			this.Categories = base.GetComponentsInChildren<CharacterCustomizationCategory>(true);
			this.TitleText.text = this.Title;
			this.ExitButton.onClick.AddListener(new UnityAction(this.Close));
			for (int i = 0; i < this.Categories.Length; i++)
			{
				Button button = UnityEngine.Object.Instantiate<Button>(this.CategoryButtonPrefab, this.ButtonContainer);
				button.GetComponentInChildren<TextMeshProUGUI>().text = this.Categories[i].CategoryName;
				CharacterCustomizationCategory category = this.Categories[i];
				button.onClick.AddListener(delegate()
				{
					this.SetActiveCategory(category);
				});
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.MainContainer.gameObject.SetActive(false);
			this.AvatarRig.gameObject.SetActive(false);
			this.SetActiveCategory(null);
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x00141330 File Offset: 0x0013F530
		protected virtual void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.RigContainer.localEulerAngles = Vector3.Lerp(this.RigContainer.localEulerAngles, new Vector3(0f, this.rigTargetY, 0f), Time.deltaTime * 5f);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00141384 File Offset: 0x0013F584
		public void SetActiveCategory(CharacterCustomizationCategory category)
		{
			this.ActiveCategory = category;
			for (int i = 0; i < this.Categories.Length; i++)
			{
				this.Categories[i].gameObject.SetActive(this.Categories[i] == category);
				if (this.Categories[i] == category)
				{
					this.Categories[i].Open();
				}
			}
			this.MenuContainer.gameObject.SetActive(category == null);
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			return false;
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x001413FF File Offset: 0x0013F5FF
		public virtual void OptionSelected(CharacterCustomizationOption option)
		{
			this.PreviewIndicator.gameObject.SetActive(!option.purchased);
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x0014141A File Offset: 0x0013F61A
		public virtual void OptionDeselected(CharacterCustomizationOption option)
		{
			Console.Log("Deselected option: " + option.Label, null);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00141432 File Offset: 0x0013F632
		public virtual void OptionPurchased(CharacterCustomizationOption option)
		{
			this.PreviewIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x00141448 File Offset: 0x0013F648
		public virtual void Open()
		{
			if (this.openCloseRoutine != null)
			{
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			this.currentSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(Player.Local.CurrentAvatarSettings);
			this.openCloseRoutine = base.StartCoroutine(this.<Open>g__Close|34_0());
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001414B8 File Offset: 0x0013F6B8
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
				if (this.ActiveCategory != null)
				{
					this.ActiveCategory.Back();
					return;
				}
				this.Close();
			}
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x00141508 File Offset: 0x0013F708
		protected virtual void Close()
		{
			if (this.openCloseRoutine != null)
			{
				return;
			}
			this.SetActiveCategory(null);
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.MainContainer.gameObject.SetActive(false);
			Player.Local.SendAppearance(this.currentSettings);
			this.openCloseRoutine = base.StartCoroutine(this.<Close>g__Close|36_0());
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x0014158D File Offset: 0x0013F78D
		[CompilerGenerated]
		private IEnumerator <Open>g__Close|34_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.IsOpen = true;
			this.Canvas.enabled = true;
			this.MainContainer.gameObject.SetActive(true);
			this.AvatarRig.gameObject.SetActive(true);
			if (this.LoadAvatarSettingsNaked)
			{
				this.AvatarRig.LoadNakedSettings(Player.Local.Avatar.CurrentSettings, 19);
			}
			else
			{
				this.AvatarRig.LoadAvatarSettings(Player.Local.Avatar.CurrentSettings);
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			this.SetActiveCategory(null);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0014159C File Offset: 0x0013F79C
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|36_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.AvatarRig.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x04003995 RID: 14741
		[Header("Settings")]
		public string Title = "Customize";

		// Token: 0x04003996 RID: 14742
		public CharacterCustomizationCategory[] Categories;

		// Token: 0x04003997 RID: 14743
		public bool LoadAvatarSettingsNaked;

		// Token: 0x04003998 RID: 14744
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003999 RID: 14745
		public RectTransform MainContainer;

		// Token: 0x0400399A RID: 14746
		public RectTransform MenuContainer;

		// Token: 0x0400399B RID: 14747
		public TextMeshProUGUI TitleText;

		// Token: 0x0400399C RID: 14748
		public RectTransform ButtonContainer;

		// Token: 0x0400399D RID: 14749
		public Button ExitButton;

		// Token: 0x0400399E RID: 14750
		public Slider RigRotationSlider;

		// Token: 0x0400399F RID: 14751
		public Transform CameraPosition;

		// Token: 0x040039A0 RID: 14752
		public Transform RigContainer;

		// Token: 0x040039A1 RID: 14753
		public ScheduleOne.AvatarFramework.Avatar AvatarRig;

		// Token: 0x040039A2 RID: 14754
		public RectTransform PreviewIndicator;

		// Token: 0x040039A3 RID: 14755
		[Header("Prefab")]
		public Button CategoryButtonPrefab;

		// Token: 0x040039A4 RID: 14756
		private float rigTargetY;

		// Token: 0x040039A5 RID: 14757
		private Coroutine openCloseRoutine;

		// Token: 0x040039A6 RID: 14758
		protected BasicAvatarSettings currentSettings;
	}
}
