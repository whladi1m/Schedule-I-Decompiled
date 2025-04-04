using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AEF RID: 2799
	public class NPCSelector : MonoBehaviour
	{
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004ADC RID: 19164 RVA: 0x0013A3DC File Offset: 0x001385DC
		// (set) Token: 0x06004ADD RID: 19165 RVA: 0x0013A3E4 File Offset: 0x001385E4
		public bool IsOpen { get; protected set; }

		// Token: 0x06004ADE RID: 19166 RVA: 0x0013A3ED File Offset: 0x001385ED
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.AddListener(new UnityAction(this.ClipboardClosed));
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0013A420 File Offset: 0x00138620
		public virtual void Open(string selectionTitle, Type typeRequirement, Action<NPC> _callback)
		{
			this.IsOpen = true;
			this.TypeRequirement = typeRequirement;
			this.callback = _callback;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			Singleton<ManagementInterface>.Instance.EquippedClipboard.OverrideClipboardText(selectionTitle);
			Singleton<ManagementClipboard>.Instance.Close(true);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("npcselector");
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0013A478 File Offset: 0x00138678
		public virtual void Close(bool returnToClipboard)
		{
			this.IsOpen = false;
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "npcselector")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			if (this.highlightedNPC != null)
			{
				this.highlightedNPC.HideOutline();
				this.highlightedNPC = null;
			}
			if (returnToClipboard)
			{
				Singleton<ManagementInterface>.Instance.EquippedClipboard.EndOverride();
				Singleton<ManagementClipboard>.Instance.Open(Singleton<ManagementInterface>.Instance.Configurables, Singleton<ManagementInterface>.Instance.EquippedClipboard);
			}
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0013A508 File Offset: 0x00138708
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.hoveredNPC = this.GetHoveredNPC();
			if (this.hoveredNPC != null && this.IsNPCTypeValid(this.hoveredNPC))
			{
				if (this.hoveredNPC != this.highlightedNPC)
				{
					if (this.highlightedNPC != null)
					{
						this.highlightedNPC.HideOutline();
						this.highlightedNPC = null;
					}
					this.highlightedNPC = this.hoveredNPC;
					this.highlightedNPC.ShowOutline(this.HoverOutlineColor);
				}
			}
			else if (this.highlightedNPC != null)
			{
				this.highlightedNPC.HideOutline();
				this.highlightedNPC = null;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredNPC != null && this.IsNPCTypeValid(this.hoveredNPC))
			{
				this.NPCClicked(this.hoveredNPC);
			}
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0013A5E8 File Offset: 0x001387E8
		private NPC GetHoveredNPC()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.DetectionMask, false, 0.1f))
			{
				return raycastHit.collider.GetComponentInParent<NPC>();
			}
			return null;
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0013A622 File Offset: 0x00138822
		public bool IsNPCTypeValid(NPC npc)
		{
			return this.TypeRequirement == null || this.TypeRequirement.IsAssignableFrom(npc.GetType());
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x0013A645 File Offset: 0x00138845
		public void NPCClicked(NPC npc)
		{
			if (!this.IsNPCTypeValid(npc))
			{
				return;
			}
			Action<NPC> action = this.callback;
			if (action != null)
			{
				action(this.hoveredNPC);
			}
			this.Close(true);
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0013A66F File Offset: 0x0013886F
		private void ClipboardClosed()
		{
			this.Close(false);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0013A678 File Offset: 0x00138878
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				exitAction.used = true;
				this.Close(true);
			}
		}

		// Token: 0x04003856 RID: 14422
		public const float SELECTION_RANGE = 5f;

		// Token: 0x04003858 RID: 14424
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04003859 RID: 14425
		public Color HoverOutlineColor;

		// Token: 0x0400385A RID: 14426
		private Type TypeRequirement;

		// Token: 0x0400385B RID: 14427
		private Action<NPC> callback;

		// Token: 0x0400385C RID: 14428
		private NPC hoveredNPC;

		// Token: 0x0400385D RID: 14429
		private NPC highlightedNPC;
	}
}
