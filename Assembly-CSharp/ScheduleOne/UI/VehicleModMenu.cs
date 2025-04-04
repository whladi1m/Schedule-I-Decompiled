using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.Modification;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A32 RID: 2610
	public class VehicleModMenu : Singleton<VehicleModMenu>
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x001266B1 File Offset: 0x001248B1
		// (set) Token: 0x06004658 RID: 18008 RVA: 0x001266B9 File Offset: 0x001248B9
		public bool IsOpen { get; private set; }

		// Token: 0x06004659 RID: 18009 RVA: 0x001266C2 File Offset: 0x001248C2
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x001266DC File Offset: 0x001248DC
		protected override void Start()
		{
			base.Start();
			this.confirmText_Online.text = "Confirm (" + MoneyManager.FormatAmount(VehicleModMenu.repaintCost, false, true) + ")";
			for (int i = 0; i < Singleton<VehicleColors>.Instance.colorLibrary.Count; i++)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, this.buttonContainer).GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2((0.5f + (float)this.colorButtons.Count) * component.sizeDelta.x, component.anchoredPosition.y);
				component.Find("Image").GetComponent<Image>().color = Singleton<VehicleColors>.Instance.colorLibrary[i].UIColor;
				EVehicleColor c = Singleton<VehicleColors>.Instance.colorLibrary[i].color;
				this.colorButtons.Add(component);
				this.colorToButton.Add(c, component);
				component.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ColorClicked(c);
				});
			}
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00126811 File Offset: 0x00124A11
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
			if (this.openCloseRoutine != null)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00126844 File Offset: 0x00124A44
		protected virtual void Update()
		{
			if (this.IsOpen)
			{
				this.UpdateConfirmButton();
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00126854 File Offset: 0x00124A54
		public void Open(LandVehicle vehicle)
		{
			this.currentVehicle = vehicle;
			this.selectedColor = vehicle.OwnedColor;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.openCloseRoutine = base.StartCoroutine(this.<Open>g__Close|24_0());
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x001268A1 File Offset: 0x00124AA1
		public void Close()
		{
			if (this.currentVehicle != null)
			{
				this.currentVehicle.ApplyOwnedColor();
			}
			this.openCloseRoutine = base.StartCoroutine(this.<Close>g__Close|25_0());
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x001268CE File Offset: 0x00124ACE
		public void ColorClicked(EVehicleColor col)
		{
			this.selectedColor = col;
			this.currentVehicle.ApplyColor(col);
			this.RefreshSelectionIndicator();
			this.UpdateConfirmButton();
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x001268F0 File Offset: 0x00124AF0
		private void UpdateConfirmButton()
		{
			bool flag = NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= VehicleModMenu.repaintCost;
			this.confirmButton_Online.interactable = (flag && this.selectedColor != this.currentVehicle.OwnedColor);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0012693C File Offset: 0x00124B3C
		private void RefreshSelectionIndicator()
		{
			this.tempIndicator.position = this.colorToButton[this.selectedColor].position;
			this.permIndicator.position = this.colorToButton[this.currentVehicle.OwnedColor].position;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00126990 File Offset: 0x00124B90
		public void ConfirmButtonClicked()
		{
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Vehicle repaint", -VehicleModMenu.repaintCost, 1f, string.Empty);
			NetworkSingleton<MoneyManager>.Instance.CashSound.Play();
			this.currentVehicle.SendOwnedColor(this.selectedColor);
			this.RefreshSelectionIndicator();
			if (this.onPaintPurchased != null)
			{
				this.onPaintPurchased.Invoke();
			}
			this.Close();
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00126A2C File Offset: 0x00124C2C
		[CompilerGenerated]
		private IEnumerator <Open>g__Close|24_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.IsOpen = true;
			this.canvas.enabled = true;
			this.currentVehicle.AlignTo(this.VehiclePosition, EParkingAlignment.RearToKerb, true);
			this.RefreshSelectionIndicator();
			this.UpdateConfirmButton();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00126A3B File Offset: 0x00124C3B
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|25_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.currentVehicle = null;
			this.IsOpen = false;
			this.canvas.enabled = false;
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x04003414 RID: 13332
		public static float repaintCost = 100f;

		// Token: 0x04003416 RID: 13334
		[Header("UI References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003417 RID: 13335
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x04003418 RID: 13336
		[SerializeField]
		protected RectTransform tempIndicator;

		// Token: 0x04003419 RID: 13337
		[SerializeField]
		protected RectTransform permIndicator;

		// Token: 0x0400341A RID: 13338
		[SerializeField]
		protected Button confirmButton_Online;

		// Token: 0x0400341B RID: 13339
		[SerializeField]
		protected TextMeshProUGUI confirmText_Online;

		// Token: 0x0400341C RID: 13340
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x0400341D RID: 13341
		public Transform VehiclePosition;

		// Token: 0x0400341E RID: 13342
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x0400341F RID: 13343
		public UnityEvent onPaintPurchased;

		// Token: 0x04003420 RID: 13344
		protected LandVehicle currentVehicle;

		// Token: 0x04003421 RID: 13345
		protected List<RectTransform> colorButtons = new List<RectTransform>();

		// Token: 0x04003422 RID: 13346
		protected Dictionary<EVehicleColor, RectTransform> colorToButton = new Dictionary<EVehicleColor, RectTransform>();

		// Token: 0x04003423 RID: 13347
		protected EVehicleColor selectedColor = EVehicleColor.White;

		// Token: 0x04003424 RID: 13348
		private Coroutine openCloseRoutine;
	}
}
