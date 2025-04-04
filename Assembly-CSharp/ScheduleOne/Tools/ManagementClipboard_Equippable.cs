using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Misc;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000827 RID: 2087
	public class ManagementClipboard_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003981 RID: 14721 RVA: 0x000F34C4 File Offset: 0x000F16C4
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<ManagementWorldspaceCanvas>.Instance.Open();
			this.Clipboard.transform.position = this.LoweredPosition.position;
			this.OverrideText.gameObject.SetActive(false);
			this.SelectionInfo.gameObject.SetActive(true);
			Singleton<ManagementClipboard>.Instance.IsEquipped = true;
			Singleton<ManagementClipboard>.Instance.onOpened.AddListener(new UnityAction(this.FullscreenEnter));
			Singleton<ManagementClipboard>.Instance.onClosed.AddListener(new UnityAction(this.FullscreenExit));
			Singleton<InputPromptsCanvas>.Instance.LoadModule("clipboard");
			if (Singleton<ManagementClipboard>.Instance.onClipboardEquipped != null)
			{
				Singleton<ManagementClipboard>.Instance.onClipboardEquipped.Invoke();
			}
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000F358C File Offset: 0x000F178C
		public override void Unequip()
		{
			base.Unequip();
			if (Singleton<ManagementClipboard>.Instance.IsOpen)
			{
				Singleton<ManagementClipboard>.Instance.Close(false);
			}
			Singleton<ManagementWorldspaceCanvas>.Instance.Close(false);
			Singleton<ManagementClipboard>.Instance.IsEquipped = false;
			if (Singleton<ManagementClipboard>.Instance.onClipboardUnequipped != null)
			{
				Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.Invoke();
			}
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "clipboard")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x000F3608 File Offset: 0x000F1808
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact) && !GameInput.IsTyping && Singleton<InteractionManager>.Instance.hoveredValidInteractableObject == null)
			{
				if (Singleton<ManagementClipboard>.Instance.IsOpen)
				{
					Singleton<ManagementClipboard>.Instance.Close(false);
					return;
				}
				List<IConfigurable> list = new List<IConfigurable>();
				list.AddRange(Singleton<ManagementWorldspaceCanvas>.Instance.SelectedConfigurables);
				if (Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable != null && !list.Contains(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable))
				{
					list.Add(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable);
				}
				Singleton<ManagementClipboard>.Instance.Open(list, this);
			}
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x000F36A8 File Offset: 0x000F18A8
		private void FullscreenEnter()
		{
			Singleton<ManagementWorldspaceCanvas>.Instance.Close(true);
			this.Clipboard.gameObject.SetActive(false);
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "clipboard")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x000F36E8 File Offset: 0x000F18E8
		private void FullscreenExit()
		{
			this.Clipboard.gameObject.SetActive(true);
			if (!Singleton<ManagementClipboard>.Instance.IsOpen && !Singleton<ManagementClipboard>.Instance.StatePreserved)
			{
				Singleton<ManagementWorldspaceCanvas>.Instance.Open();
				Singleton<InputPromptsCanvas>.Instance.LoadModule("clipboard");
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x000F3737 File Offset: 0x000F1937
		public void OverrideClipboardText(string overriddenText)
		{
			this.OverrideText.text = overriddenText;
			this.OverrideText.gameObject.SetActive(true);
			this.SelectionInfo.gameObject.SetActive(false);
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x000F3767 File Offset: 0x000F1967
		public void EndOverride()
		{
			this.OverrideText.gameObject.SetActive(false);
			this.SelectionInfo.gameObject.SetActive(true);
		}

		// Token: 0x04002985 RID: 10629
		[Header("References")]
		public Transform Clipboard;

		// Token: 0x04002986 RID: 10630
		public Transform LoweredPosition;

		// Token: 0x04002987 RID: 10631
		public Transform RaisedPosition;

		// Token: 0x04002988 RID: 10632
		public ToggleableLight Light;

		// Token: 0x04002989 RID: 10633
		public SelectionInfoUI SelectionInfo;

		// Token: 0x0400298A RID: 10634
		public TextMeshProUGUI OverrideText;

		// Token: 0x0400298B RID: 10635
		private Coroutine moveRoutine;
	}
}
