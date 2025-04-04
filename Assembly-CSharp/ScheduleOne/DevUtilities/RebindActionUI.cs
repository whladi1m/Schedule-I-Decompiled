using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006F8 RID: 1784
	public class RebindActionUI : MonoBehaviour
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x000C8CAD File Offset: 0x000C6EAD
		// (set) Token: 0x0600303A RID: 12346 RVA: 0x000C8CB5 File Offset: 0x000C6EB5
		public InputActionReference actionReference
		{
			get
			{
				return this.m_Action;
			}
			set
			{
				this.m_Action = value;
				this.UpdateActionLabel();
				this.UpdateBindingDisplay();
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x000C8CCA File Offset: 0x000C6ECA
		// (set) Token: 0x0600303C RID: 12348 RVA: 0x000C8CD2 File Offset: 0x000C6ED2
		public string bindingId
		{
			get
			{
				return this.m_BindingId;
			}
			set
			{
				this.m_BindingId = value;
				this.UpdateBindingDisplay();
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x000C8CE1 File Offset: 0x000C6EE1
		// (set) Token: 0x0600303E RID: 12350 RVA: 0x000C8CE9 File Offset: 0x000C6EE9
		public InputBinding.DisplayStringOptions displayStringOptions
		{
			get
			{
				return this.m_DisplayStringOptions;
			}
			set
			{
				this.m_DisplayStringOptions = value;
				this.UpdateBindingDisplay();
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000C8CF8 File Offset: 0x000C6EF8
		// (set) Token: 0x06003040 RID: 12352 RVA: 0x000C8D00 File Offset: 0x000C6F00
		public TextMeshProUGUI actionLabel
		{
			get
			{
				return this.m_ActionLabel;
			}
			set
			{
				this.m_ActionLabel = value;
				this.UpdateActionLabel();
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x000C8D0F File Offset: 0x000C6F0F
		// (set) Token: 0x06003042 RID: 12354 RVA: 0x000C8D17 File Offset: 0x000C6F17
		public TextMeshProUGUI bindingText
		{
			get
			{
				return this.m_BindingText;
			}
			set
			{
				this.m_BindingText = value;
				this.UpdateBindingDisplay();
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x000C8D26 File Offset: 0x000C6F26
		// (set) Token: 0x06003044 RID: 12356 RVA: 0x000C8D2E File Offset: 0x000C6F2E
		public TextMeshProUGUI rebindPrompt
		{
			get
			{
				return this.m_RebindText;
			}
			set
			{
				this.m_RebindText = value;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06003045 RID: 12357 RVA: 0x000C8D37 File Offset: 0x000C6F37
		// (set) Token: 0x06003046 RID: 12358 RVA: 0x000C8D3F File Offset: 0x000C6F3F
		public GameObject rebindOverlay
		{
			get
			{
				return this.m_RebindOverlay;
			}
			set
			{
				this.m_RebindOverlay = value;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x000C8D48 File Offset: 0x000C6F48
		public RebindActionUI.UpdateBindingUIEvent updateBindingUIEvent
		{
			get
			{
				if (this.m_UpdateBindingUIEvent == null)
				{
					this.m_UpdateBindingUIEvent = new RebindActionUI.UpdateBindingUIEvent();
				}
				return this.m_UpdateBindingUIEvent;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x000C8D63 File Offset: 0x000C6F63
		public RebindActionUI.InteractiveRebindEvent startRebindEvent
		{
			get
			{
				if (this.m_RebindStartEvent == null)
				{
					this.m_RebindStartEvent = new RebindActionUI.InteractiveRebindEvent();
				}
				return this.m_RebindStartEvent;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003049 RID: 12361 RVA: 0x000C8D7E File Offset: 0x000C6F7E
		public RebindActionUI.InteractiveRebindEvent stopRebindEvent
		{
			get
			{
				if (this.m_RebindStopEvent == null)
				{
					this.m_RebindStopEvent = new RebindActionUI.InteractiveRebindEvent();
				}
				return this.m_RebindStopEvent;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x000C8D99 File Offset: 0x000C6F99
		public InputActionRebindingExtensions.RebindingOperation ongoingRebind
		{
			get
			{
				return this.m_RebindOperation;
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000C8DA4 File Offset: 0x000C6FA4
		public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
		{
			bindingIndex = -1;
			InputActionReference action2 = this.m_Action;
			action = ((action2 != null) ? action2.action : null);
			if (action == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.m_BindingId))
			{
				return false;
			}
			Guid bindingId = new Guid(this.m_BindingId);
			bindingIndex = action.bindings.IndexOf((InputBinding x) => x.id == bindingId);
			if (bindingIndex == -1)
			{
				Debug.LogError(string.Format("Cannot find binding with ID '{0}' on '{1}'", bindingId, action), this);
				return false;
			}
			return true;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000C8E38 File Offset: 0x000C7038
		public void UpdateBindingDisplay()
		{
			string text = string.Empty;
			string arg = null;
			string arg2 = null;
			InputActionReference action = this.m_Action;
			InputAction inputAction = (action != null) ? action.action : null;
			if (inputAction != null)
			{
				int num = inputAction.bindings.IndexOf((InputBinding x) => x.id.ToString() == this.m_BindingId);
				if (num != -1)
				{
					text = inputAction.GetBindingDisplayString(num, out arg, out arg2, this.displayStringOptions);
				}
			}
			this.m_BindingText.gameObject.SetActive(true);
			if (this.m_BindingText != null)
			{
				this.m_BindingText.text = text;
			}
			RebindActionUI.UpdateBindingUIEvent updateBindingUIEvent = this.m_UpdateBindingUIEvent;
			if (updateBindingUIEvent == null)
			{
				return;
			}
			updateBindingUIEvent.Invoke(this, text, arg, arg2);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000C8EDC File Offset: 0x000C70DC
		public void ResetToDefault()
		{
			InputAction inputAction;
			int num;
			if (!this.ResolveActionAndBinding(out inputAction, out num))
			{
				return;
			}
			if (inputAction.bindings[num].isComposite)
			{
				for (int i = num + 1; i < inputAction.bindings.Count; i++)
				{
					if (!inputAction.bindings[i].isPartOfComposite)
					{
						break;
					}
					inputAction.RemoveBindingOverride(i);
				}
			}
			else
			{
				inputAction.RemoveBindingOverride(num);
			}
			this.UpdateBindingDisplay();
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000C8F60 File Offset: 0x000C7160
		public void StartInteractiveRebind()
		{
			this.m_Action.action.Disable();
			InputAction inputAction;
			int num;
			if (!this.ResolveActionAndBinding(out inputAction, out num))
			{
				return;
			}
			if (inputAction.bindings[num].isComposite)
			{
				int num2 = num + 1;
				if (num2 < inputAction.bindings.Count && inputAction.bindings[num2].isPartOfComposite)
				{
					this.PerformInteractiveRebind(inputAction, num2, true);
					return;
				}
			}
			else
			{
				this.PerformInteractiveRebind(inputAction, num, false);
			}
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000C8FE8 File Offset: 0x000C71E8
		private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
		{
			InputActionRebindingExtensions.RebindingOperation rebindOperation = this.m_RebindOperation;
			if (rebindOperation != null)
			{
				rebindOperation.Cancel();
			}
			this.m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex).OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation operation)
			{
				RebindActionUI.InteractiveRebindEvent rebindStopEvent = this.m_RebindStopEvent;
				if (rebindStopEvent != null)
				{
					rebindStopEvent.Invoke(this, operation);
				}
				if (this.m_RebindOverlay != null)
				{
					GameObject rebindOverlay2 = this.m_RebindOverlay;
					if (rebindOverlay2 != null)
					{
						rebindOverlay2.SetActive(false);
					}
				}
				this.UpdateBindingDisplay();
				base.<PerformInteractiveRebind>g__CleanUp|0();
			}).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation operation)
			{
				GameObject rebindOverlay2 = this.m_RebindOverlay;
				if (rebindOverlay2 != null)
				{
					rebindOverlay2.SetActive(false);
				}
				RebindActionUI.InteractiveRebindEvent rebindStopEvent = this.m_RebindStopEvent;
				if (rebindStopEvent != null)
				{
					rebindStopEvent.Invoke(this, operation);
				}
				this.UpdateBindingDisplay();
				base.<PerformInteractiveRebind>g__CleanUp|0();
				if (allCompositeParts)
				{
					int num = bindingIndex + 1;
					if (num < action.bindings.Count && action.bindings[num].isPartOfComposite)
					{
						this.PerformInteractiveRebind(action, num, true);
					}
				}
				Action action2 = this.onRebind;
				if (action2 == null)
				{
					return;
				}
				action2();
			}).WithControlsExcluding("Mouse");
			if (action.bindings[bindingIndex].isPartOfComposite)
			{
				"Binding '" + action.bindings[bindingIndex].name + "'. ";
			}
			if (this.m_RebindOverlay != null)
			{
				GameObject rebindOverlay = this.m_RebindOverlay;
				if (rebindOverlay != null)
				{
					rebindOverlay.SetActive(true);
				}
			}
			if (this.m_RebindText != null)
			{
				this.m_RebindText.text = "Press key...";
			}
			if (this.m_RebindOverlay == null && this.m_RebindText == null && this.m_RebindStartEvent == null && this.m_BindingText != null)
			{
				this.m_BindingText.text = "<Waiting...>";
			}
			this.m_BindingText.gameObject.SetActive(false);
			RebindActionUI.InteractiveRebindEvent rebindStartEvent = this.m_RebindStartEvent;
			if (rebindStartEvent != null)
			{
				rebindStartEvent.Invoke(this, this.m_RebindOperation);
			}
			this.m_RebindOperation.Start();
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000C9174 File Offset: 0x000C7374
		protected void OnEnable()
		{
			if (RebindActionUI.s_RebindActionUIs == null)
			{
				RebindActionUI.s_RebindActionUIs = new List<RebindActionUI>();
			}
			RebindActionUI.s_RebindActionUIs.Add(this);
			if (RebindActionUI.s_RebindActionUIs.Count == 1)
			{
				InputSystem.onActionChange += RebindActionUI.OnActionChange;
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000C91B0 File Offset: 0x000C73B0
		protected void OnDisable()
		{
			InputActionRebindingExtensions.RebindingOperation rebindOperation = this.m_RebindOperation;
			if (rebindOperation != null)
			{
				rebindOperation.Dispose();
			}
			this.m_RebindOperation = null;
			RebindActionUI.s_RebindActionUIs.Remove(this);
			if (RebindActionUI.s_RebindActionUIs.Count == 0)
			{
				RebindActionUI.s_RebindActionUIs = null;
				InputSystem.onActionChange -= RebindActionUI.OnActionChange;
			}
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000C9204 File Offset: 0x000C7404
		private static void OnActionChange(object obj, InputActionChange change)
		{
			if (change != InputActionChange.BoundControlsChanged)
			{
				return;
			}
			InputAction inputAction = obj as InputAction;
			InputActionMap inputActionMap = ((inputAction != null) ? inputAction.actionMap : null) ?? (obj as InputActionMap);
			InputActionAsset y = ((inputActionMap != null) ? inputActionMap.asset : null) ?? (obj as InputActionAsset);
			for (int i = 0; i < RebindActionUI.s_RebindActionUIs.Count; i++)
			{
				RebindActionUI rebindActionUI = RebindActionUI.s_RebindActionUIs[i];
				InputActionReference actionReference = rebindActionUI.actionReference;
				InputAction inputAction2 = (actionReference != null) ? actionReference.action : null;
				if (inputAction2 != null)
				{
					if (inputAction2 != inputAction && inputAction2.actionMap != inputActionMap)
					{
						InputActionMap actionMap = inputAction2.actionMap;
						if (!(((actionMap != null) ? actionMap.asset : null) == y))
						{
							goto IL_95;
						}
					}
					rebindActionUI.UpdateBindingDisplay();
				}
				IL_95:;
			}
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000C92B8 File Offset: 0x000C74B8
		private void UpdateActionLabel()
		{
			if (this.m_ActionLabel != null)
			{
				InputActionReference action = this.m_Action;
				InputAction inputAction = (action != null) ? action.action : null;
				this.m_ActionLabel.text = ((inputAction != null) ? inputAction.name : string.Empty);
			}
		}

		// Token: 0x0400228F RID: 8847
		public Action onRebind;

		// Token: 0x04002290 RID: 8848
		[Tooltip("Reference to action that is to be rebound from the UI.")]
		[SerializeField]
		private InputActionReference m_Action;

		// Token: 0x04002291 RID: 8849
		[SerializeField]
		private string m_BindingId;

		// Token: 0x04002292 RID: 8850
		[SerializeField]
		private InputBinding.DisplayStringOptions m_DisplayStringOptions;

		// Token: 0x04002293 RID: 8851
		[Tooltip("Text label that will receive the name of the action. Optional. Set to None to have the rebind UI not show a label for the action.")]
		[SerializeField]
		private TextMeshProUGUI m_ActionLabel;

		// Token: 0x04002294 RID: 8852
		[Tooltip("Text label that will receive the current, formatted binding string.")]
		[SerializeField]
		private TextMeshProUGUI m_BindingText;

		// Token: 0x04002295 RID: 8853
		[Tooltip("Optional UI that will be shown while a rebind is in progress.")]
		[SerializeField]
		private GameObject m_RebindOverlay;

		// Token: 0x04002296 RID: 8854
		[Tooltip("Optional text label that will be updated with prompt for user input.")]
		[SerializeField]
		private TextMeshProUGUI m_RebindText;

		// Token: 0x04002297 RID: 8855
		[Tooltip("Event that is triggered when the way the binding is display should be updated. This allows displaying bindings in custom ways, e.g. using images instead of text.")]
		[SerializeField]
		private RebindActionUI.UpdateBindingUIEvent m_UpdateBindingUIEvent;

		// Token: 0x04002298 RID: 8856
		[Tooltip("Event that is triggered when an interactive rebind is being initiated. This can be used, for example, to implement custom UI behavior while a rebind is in progress. It can also be used to further customize the rebind.")]
		[SerializeField]
		private RebindActionUI.InteractiveRebindEvent m_RebindStartEvent;

		// Token: 0x04002299 RID: 8857
		[Tooltip("Event that is triggered when an interactive rebind is complete or has been aborted.")]
		[SerializeField]
		private RebindActionUI.InteractiveRebindEvent m_RebindStopEvent;

		// Token: 0x0400229A RID: 8858
		private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

		// Token: 0x0400229B RID: 8859
		private static List<RebindActionUI> s_RebindActionUIs;

		// Token: 0x020006F9 RID: 1785
		[Serializable]
		public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
		{
		}

		// Token: 0x020006FA RID: 1786
		[Serializable]
		public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation>
		{
		}
	}
}
