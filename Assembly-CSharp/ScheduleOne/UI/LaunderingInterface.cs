using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts.Cash;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009E3 RID: 2531
	public class LaunderingInterface : MonoBehaviour
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06004448 RID: 17480 RVA: 0x0011DCD5 File Offset: 0x0011BED5
		protected int maxLaunderAmount
		{
			get
			{
				return (int)Mathf.Min(this.business.appliedLaunderLimit, NetworkSingleton<MoneyManager>.Instance.cashBalance);
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06004449 RID: 17481 RVA: 0x0011DCF2 File Offset: 0x0011BEF2
		// (set) Token: 0x0600444A RID: 17482 RVA: 0x0011DCFA File Offset: 0x0011BEFA
		public Business business { get; private set; }

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600444B RID: 17483 RVA: 0x0011DD03 File Offset: 0x0011BF03
		public bool isOpen
		{
			get
			{
				return this.canvas != null && this.canvas.gameObject.activeSelf;
			}
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0011DD28 File Offset: 0x0011BF28
		public void Initialize(Business bus)
		{
			this.business = bus;
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.launderCapacityLabel.text = MoneyManager.FormatAmount(this.business.LaunderCapacity, false, false);
			this.canvas.gameObject.SetActive(false);
			this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(delegate()
			{
				this.canvas.worldCamera = PlayerSingleton<PlayerCamera>.Instance.Camera;
			}));
			foreach (LaunderingOperation op in this.business.LaunderingOperations)
			{
				this.CreateEntry(op);
			}
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationStarted, new Action<LaunderingOperation>(this.CreateEntry));
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationStarted, new Action<LaunderingOperation>(this.UpdateCashStacks));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationFinished, new Action<LaunderingOperation>(this.RemoveEntry));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Combine(Business.onOperationFinished, new Action<LaunderingOperation>(this.UpdateCashStacks));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.CloseAmountSelector();
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0011DEEC File Offset: 0x0011C0EC
		private void OnDestroy()
		{
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationStarted, new Action<LaunderingOperation>(this.CreateEntry));
			Business.onOperationStarted = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationStarted, new Action<LaunderingOperation>(this.UpdateCashStacks));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationFinished, new Action<LaunderingOperation>(this.RemoveEntry));
			Business.onOperationFinished = (Action<LaunderingOperation>)Delegate.Remove(Business.onOperationFinished, new Action<LaunderingOperation>(this.UpdateCashStacks));
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0011DF79 File Offset: 0x0011C179
		protected virtual void MinPass()
		{
			if (this.isOpen)
			{
				this.UpdateTimeline();
				this.RefreshLaunderButton();
				this.UpdateCurrentTotal();
				this.UpdateEntryTimes();
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0011DF9C File Offset: 0x0011C19C
		protected void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.isOpen)
			{
				if (this.amountSelectorScreen.gameObject.activeSelf)
				{
					exit.used = true;
					this.CloseAmountSelector();
					return;
				}
				if (exit.exitType == ExitType.Escape)
				{
					exit.used = true;
					this.Close();
				}
			}
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0011DFF0 File Offset: 0x0011C1F0
		protected void UpdateTimeline()
		{
			foreach (LaunderingOperation launderingOperation in this.business.LaunderingOperations)
			{
				if (!this.operationToNotch.ContainsKey(launderingOperation))
				{
					RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.timelineNotchPrefab, this.notchContainer).GetComponent<RectTransform>();
					component.Find("Amount").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(launderingOperation.amount, false, false);
					this.operationToNotch.Add(launderingOperation, component);
					this.notches.Add(component);
				}
			}
			List<RectTransform> list = (from x in this.operationToNotch
			where this.business.LaunderingOperations.Contains(x.Key)
			select x.Value).ToList<RectTransform>();
			for (int i = 0; i < this.notches.Count; i++)
			{
				if (!list.Contains(this.notches[i]))
				{
					UnityEngine.Object.Destroy(this.notches[i].gameObject);
					this.notches.RemoveAt(i);
					i--;
				}
			}
			foreach (LaunderingOperation launderingOperation2 in this.business.LaunderingOperations)
			{
				this.operationToNotch[launderingOperation2].anchoredPosition = new Vector2(this.notchContainer.rect.width * (float)launderingOperation2.minutesSinceStarted / (float)launderingOperation2.completionTime_Minutes, this.operationToNotch[launderingOperation2].anchoredPosition.y);
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0011E1D0 File Offset: 0x0011C3D0
		protected void UpdateCurrentTotal()
		{
			this.currentTotalAmountLabel.text = MoneyManager.FormatAmount(this.business.currentLaunderTotal, false, false);
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0011E1F0 File Offset: 0x0011C3F0
		private void CreateEntry(LaunderingOperation op)
		{
			if (this.operationToEntry.ContainsKey(op))
			{
				return;
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.entryPrefab, this.entryContainer).GetComponent<RectTransform>();
			component.SetAsLastSibling();
			component.Find("BusinessLabel").GetComponent<TextMeshProUGUI>().text = op.business.PropertyName;
			component.Find("AmountLabel").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(op.amount, false, false);
			this.operationToEntry.Add(op, component);
			this.UpdateEntryTimes();
			if (this.noEntries != null)
			{
				this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0011E2AC File Offset: 0x0011C4AC
		private void RemoveEntry(LaunderingOperation op)
		{
			if (!this.operationToEntry.ContainsKey(op))
			{
				return;
			}
			RectTransform rectTransform = this.operationToEntry[op];
			if (rectTransform != null)
			{
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			this.operationToEntry.Remove(op);
			this.noEntries.gameObject.SetActive(this.operationToEntry.Count == 0);
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0011E314 File Offset: 0x0011C514
		private void UpdateEntryTimes()
		{
			foreach (LaunderingOperation launderingOperation in this.operationToEntry.Keys.ToList<LaunderingOperation>())
			{
				if (this.operationToEntry.ContainsKey(launderingOperation))
				{
					if (this.operationToEntry[launderingOperation] == null)
					{
						Console.LogWarning("Entry is null for operation " + launderingOperation.business.PropertyName, null);
					}
					else
					{
						int num = launderingOperation.completionTime_Minutes - launderingOperation.minutesSinceStarted;
						if (num > 60)
						{
							int num2 = Mathf.CeilToInt((float)num / 60f);
							this.operationToEntry[launderingOperation].Find("TimeLabel").GetComponent<TextMeshProUGUI>().text = num2.ToString() + " hours";
						}
						else
						{
							this.operationToEntry[launderingOperation].Find("TimeLabel").GetComponent<TextMeshProUGUI>().text = num.ToString() + " minutes";
						}
					}
				}
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0011E43C File Offset: 0x0011C63C
		private void UpdateCashStacks(LaunderingOperation op)
		{
			float num = this.business.currentLaunderTotal;
			for (int i = 0; i < this.CashStacks.Length; i++)
			{
				if (num <= 0f)
				{
					this.CashStacks[i].ShowAmount(0f);
				}
				else
				{
					float num2 = Mathf.Min(num, 1000f);
					this.CashStacks[i].ShowAmount(num2);
					num -= num2;
				}
			}
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0011E4A4 File Offset: 0x0011C6A4
		private void RefreshLaunderButton()
		{
			this.launderButton.interactable = (this.business.currentLaunderTotal < this.business.LaunderCapacity && NetworkSingleton<MoneyManager>.Instance.cashBalance > 10f);
			if (this.business.currentLaunderTotal >= this.business.LaunderCapacity)
			{
				this.insufficientCashLabel.text = "The business is already at maximum laundering capacity.";
				this.insufficientCashLabel.gameObject.SetActive(true);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance <= 10f)
			{
				this.insufficientCashLabel.text = "You need at least " + MoneyManager.FormatAmount(10f, false, false) + " cash to launder.";
				this.insufficientCashLabel.gameObject.SetActive(true);
				return;
			}
			this.insufficientCashLabel.gameObject.SetActive(false);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0011E57C File Offset: 0x0011C77C
		public void OpenAmountSelector()
		{
			this.amountSelectorScreen.gameObject.SetActive(true);
			int num = Mathf.Clamp(100, 10, this.maxLaunderAmount);
			this.selectedAmountToLaunder = num;
			this.amountSlider.minValue = 10f;
			this.amountSlider.maxValue = (float)this.maxLaunderAmount;
			this.amountSlider.SetValueWithoutNotify((float)num);
			this.amountInputField.SetTextWithoutNotify(num.ToString());
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0011E5F2 File Offset: 0x0011C7F2
		public void CloseAmountSelector()
		{
			this.amountSelectorScreen.gameObject.SetActive(false);
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0011E608 File Offset: 0x0011C808
		public void ConfirmAmount()
		{
			int num = Mathf.Clamp(this.selectedAmountToLaunder, 10, this.maxLaunderAmount);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance((float)(-(float)num), true, false);
			this.business.StartLaunderingOperation((float)num, 0);
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LaunderingOperationsStarted");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LaunderingOperationsStarted", (value + 1f).ToString(), true);
			this.UpdateTimeline();
			this.UpdateCurrentTotal();
			this.RefreshLaunderButton();
			this.CloseAmountSelector();
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0011E68D File Offset: 0x0011C88D
		public void SliderValueChanged()
		{
			if (this.ignoreSliderChange)
			{
				this.ignoreSliderChange = false;
				return;
			}
			this.selectedAmountToLaunder = (int)this.amountSlider.value;
			this.amountInputField.SetTextWithoutNotify(this.selectedAmountToLaunder.ToString());
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0011E6C8 File Offset: 0x0011C8C8
		public void InputValueChanged()
		{
			this.selectedAmountToLaunder = Mathf.Clamp(int.Parse(this.amountInputField.text), 10, this.maxLaunderAmount);
			this.amountInputField.SetTextWithoutNotify(this.selectedAmountToLaunder.ToString());
			this.amountSlider.SetValueWithoutNotify((float)this.selectedAmountToLaunder);
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0011E720 File Offset: 0x0011C920
		public void Hovered()
		{
			if (!this.business.IsOwned || this.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.business.IsOwned && !this.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.intObj.SetMessage("Manage business");
			}
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0011E780 File Offset: 0x0011C980
		public void Interacted()
		{
			if (this.business.IsOwned && !this.isOpen)
			{
				this.Open();
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0011E7A0 File Offset: 0x0011C9A0
		public virtual void Open()
		{
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.cameraPosition.transform.position, this.cameraPosition.rotation, 0.15f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.15f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.RefreshLaunderButton();
			this.UpdateTimeline();
			this.UpdateCurrentTotal();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0011E858 File Offset: 0x0011CA58
		public virtual void Close()
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003219 RID: 12825
		protected const float fovOverride = 65f;

		// Token: 0x0400321A RID: 12826
		protected const float lerpTime = 0.15f;

		// Token: 0x0400321B RID: 12827
		protected const int minLaunderAmount = 10;

		// Token: 0x0400321D RID: 12829
		[Header("References")]
		[SerializeField]
		protected Transform cameraPosition;

		// Token: 0x0400321E RID: 12830
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x0400321F RID: 12831
		[SerializeField]
		protected Button launderButton;

		// Token: 0x04003220 RID: 12832
		[SerializeField]
		protected GameObject amountSelectorScreen;

		// Token: 0x04003221 RID: 12833
		[SerializeField]
		protected Slider amountSlider;

		// Token: 0x04003222 RID: 12834
		[SerializeField]
		protected TMP_InputField amountInputField;

		// Token: 0x04003223 RID: 12835
		[SerializeField]
		protected RectTransform notchContainer;

		// Token: 0x04003224 RID: 12836
		[SerializeField]
		protected TextMeshProUGUI currentTotalAmountLabel;

		// Token: 0x04003225 RID: 12837
		[SerializeField]
		protected TextMeshProUGUI launderCapacityLabel;

		// Token: 0x04003226 RID: 12838
		[SerializeField]
		protected TextMeshProUGUI insufficientCashLabel;

		// Token: 0x04003227 RID: 12839
		[SerializeField]
		protected RectTransform entryContainer;

		// Token: 0x04003228 RID: 12840
		[SerializeField]
		protected RectTransform noEntries;

		// Token: 0x04003229 RID: 12841
		public CashStackVisuals[] CashStacks;

		// Token: 0x0400322A RID: 12842
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject timelineNotchPrefab;

		// Token: 0x0400322B RID: 12843
		[SerializeField]
		protected GameObject entryPrefab;

		// Token: 0x0400322C RID: 12844
		[Header("UI references")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x0400322D RID: 12845
		private int selectedAmountToLaunder;

		// Token: 0x0400322E RID: 12846
		private Dictionary<LaunderingOperation, RectTransform> operationToNotch = new Dictionary<LaunderingOperation, RectTransform>();

		// Token: 0x0400322F RID: 12847
		private List<RectTransform> notches = new List<RectTransform>();

		// Token: 0x04003230 RID: 12848
		private bool ignoreSliderChange = true;

		// Token: 0x04003231 RID: 12849
		private Dictionary<LaunderingOperation, RectTransform> operationToEntry = new Dictionary<LaunderingOperation, RectTransform>();
	}
}
