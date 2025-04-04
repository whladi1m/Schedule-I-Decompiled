using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x020009FA RID: 2554
	public class ArrestNoticeScreen : Singleton<ArrestNoticeScreen>
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060044F1 RID: 17649 RVA: 0x00120FD6 File Offset: 0x0011F1D6
		// (set) Token: 0x060044F2 RID: 17650 RVA: 0x00120FDE File Offset: 0x0011F1DE
		public bool isOpen { get; protected set; }

		// Token: 0x060044F3 RID: 17651 RVA: 0x00120FE8 File Offset: 0x0011F1E8
		protected override void Awake()
		{
			base.Awake();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.CanvasGroup.alpha = 0f;
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 20);
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00121051 File Offset: 0x0011F251
		private void PlayerSpawned()
		{
			Player.Local.onArrested.AddListener(new UnityAction(this.RecordCrimes));
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0012106E File Offset: 0x0011F26E
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
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x00121098 File Offset: 0x0011F298
		public void Open()
		{
			this.ClearEntries();
			this.isOpen = true;
			this.Canvas.enabled = true;
			this.CanvasGroup.alpha = 1f;
			this.CanvasGroup.interactable = true;
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			Crime[] array = this.recordedCrimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.Instantiate<RectTransform>(this.CrimeEntryPrefab, this.CrimeEntryContainer).GetComponentInChildren<TextMeshProUGUI>().text = this.recordedCrimes[array[i]].ToString() + "x " + array[i].CrimeName.ToLower();
			}
			List<string> list = PenaltyHandler.ProcessCrimeList(this.recordedCrimes);
			this.ConfiscateItems(EStealthLevel.None);
			for (int j = 0; j < list.Count; j++)
			{
				UnityEngine.Object.Instantiate<RectTransform>(this.PenaltyEntryPrefab, this.PenaltyEntryContainer).GetComponentInChildren<TextMeshProUGUI>().text = list[j];
			}
			if (this.vehicle != null && !this.vehicle.isOccupied)
			{
				Transform[] possessedVehicleSpawnPoints = Singleton<Map>.Instance.PoliceStation.PossessedVehicleSpawnPoints;
				Transform target = possessedVehicleSpawnPoints[UnityEngine.Random.Range(0, possessedVehicleSpawnPoints.Length - 1)];
				Tuple<Vector3, Quaternion> alignmentTransform = this.vehicle.GetAlignmentTransform(target, EParkingAlignment.RearToKerb);
				this.vehicle.SetTransform_Server(alignmentTransform.Item1, alignmentTransform.Item2);
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Player.Deactivate(true);
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00121217 File Offset: 0x0011F417
		public void Close()
		{
			if (!this.CanvasGroup.interactable || !this.isOpen)
			{
				return;
			}
			this.CanvasGroup.interactable = false;
			base.StartCoroutine(this.<Close>g__CloseRoutine|17_0());
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00121248 File Offset: 0x0011F448
		public void RecordCrimes()
		{
			Debug.Log("Crimes recorded");
			this.recordedCrimes.Clear();
			if (Player.Local.LastDrivenVehicle != null && (Player.Local.TimeSinceVehicleExit < 30f || Player.Local.CrimeData.IsCrimeOnRecord(typeof(TransportingIllicitItems))))
			{
				this.vehicle = Player.Local.LastDrivenVehicle;
			}
			for (int i = 0; i < Player.Local.CrimeData.Crimes.Keys.Count; i++)
			{
				this.recordedCrimes.Add(Player.Local.CrimeData.Crimes.Keys.ElementAt(i), Player.Local.CrimeData.Crimes.Values.ElementAt(i));
			}
			if (Player.Local.CrimeData.EvadedArrest)
			{
				this.recordedCrimes.Add(new Evading(), 1);
			}
			this.RecordPossession(EStealthLevel.None);
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00121344 File Offset: 0x0011F544
		private void RecordPossession(EStealthLevel maxStealthLevel)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			if (Player.Local.LastDrivenVehicle != null && Player.Local.TimeSinceVehicleExit < 30f && Player.Local.LastDrivenVehicle.Storage != null)
			{
				allInventorySlots.AddRange(Player.Local.LastDrivenVehicle.Storage.ItemSlots);
			}
			foreach (ItemSlot itemSlot in allInventorySlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= maxStealthLevel)
						{
							switch (itemSlot.ItemInstance.Definition.legalStatus)
							{
							case ELegalStatus.ControlledSubstance:
								num += productItemInstance.Quantity;
								break;
							case ELegalStatus.LowSeverityDrug:
								num2 += productItemInstance.Quantity;
								break;
							case ELegalStatus.ModerateSeverityDrug:
								num3 += productItemInstance.Quantity;
								break;
							case ELegalStatus.HighSeverityDrug:
								num4 += productItemInstance.Quantity;
								break;
							}
						}
					}
					else
					{
						switch (itemSlot.ItemInstance.Definition.legalStatus)
						{
						case ELegalStatus.ControlledSubstance:
							num += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.LowSeverityDrug:
							num2 += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.ModerateSeverityDrug:
							num3 += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.HighSeverityDrug:
							num4 += itemSlot.ItemInstance.Quantity;
							break;
						}
					}
				}
			}
			if (num > 0)
			{
				this.recordedCrimes.Add(new PossessingControlledSubstances(), num);
			}
			if (num2 > 0)
			{
				this.recordedCrimes.Add(new PossessingLowSeverityDrug(), num2);
			}
			if (num3 > 0)
			{
				this.recordedCrimes.Add(new PossessingModerateSeverityDrug(), num3);
			}
			if (num4 > 0)
			{
				this.recordedCrimes.Add(new PossessingHighSeverityDrug(), num4);
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x00121580 File Offset: 0x0011F780
		private void ConfiscateItems(EStealthLevel maxStealthLevel)
		{
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			if (Player.Local.LastDrivenVehicle != null && Player.Local.TimeSinceVehicleExit < 30f && Player.Local.LastDrivenVehicle.Storage != null)
			{
				allInventorySlots.AddRange(Player.Local.LastDrivenVehicle.Storage.ItemSlots);
			}
			foreach (ItemSlot itemSlot in allInventorySlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= maxStealthLevel)
						{
							itemSlot.ClearStoredInstance(false);
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						itemSlot.ClearStoredInstance(false);
					}
				}
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00121688 File Offset: 0x0011F888
		private void ClearEntries()
		{
			int childCount = this.CrimeEntryContainer.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(this.CrimeEntryContainer.GetChild(i).gameObject);
			}
			childCount = this.PenaltyEntryContainer.childCount;
			for (int j = 0; j < childCount; j++)
			{
				UnityEngine.Object.Destroy(this.PenaltyEntryContainer.GetChild(j).gameObject);
			}
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00121704 File Offset: 0x0011F904
		[CompilerGenerated]
		private IEnumerator <Close>g__CloseRoutine|17_0()
		{
			float lerpTime = 0.3f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CanvasGroup.alpha = Mathf.Lerp(1f, 0f, i / lerpTime);
				Singleton<PostProcessingManager>.Instance.SetBlur(this.CanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			this.CanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Player.Activate();
			this.ClearEntries();
			this.isOpen = false;
			yield break;
		}

		// Token: 0x040032D8 RID: 13016
		public const float VEHICLE_POSSESSION_TIMEOUT = 30f;

		// Token: 0x040032DA RID: 13018
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040032DB RID: 13019
		public CanvasGroup CanvasGroup;

		// Token: 0x040032DC RID: 13020
		public RectTransform CrimeEntryContainer;

		// Token: 0x040032DD RID: 13021
		public RectTransform PenaltyEntryContainer;

		// Token: 0x040032DE RID: 13022
		[Header("Prefabs")]
		public RectTransform CrimeEntryPrefab;

		// Token: 0x040032DF RID: 13023
		public RectTransform PenaltyEntryPrefab;

		// Token: 0x040032E0 RID: 13024
		private Dictionary<Crime, int> recordedCrimes = new Dictionary<Crime, int>();

		// Token: 0x040032E1 RID: 13025
		private LandVehicle vehicle;
	}
}
