using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.PlayerTasks.Tasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000909 RID: 2313
	public class Equippable_Pourable : Equippable_Viewmodel
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x00108651 File Offset: 0x00106851
		// (set) Token: 0x06003EA2 RID: 16034 RVA: 0x00108659 File Offset: 0x00106859
		public virtual string InteractionLabel { get; set; } = "Pour";

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00108664 File Offset: 0x00106864
		protected override void Update()
		{
			base.Update();
			if (Singleton<TaskManager>.Instance.currentTask != null)
			{
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				return;
			}
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.InteractionRange, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				Pot componentInParent = raycastHit.collider.GetComponentInParent<Pot>();
				if (componentInParent == null)
				{
					return;
				}
				string empty = string.Empty;
				if (this.CanPour(componentInParent, out empty))
				{
					if (componentInParent.PlayerUserObject != null)
					{
						componentInParent.ConfigureInteraction("In use by other player", InteractableObject.EInteractableState.Invalid, false);
						return;
					}
					if (componentInParent.NPCUserObject != null)
					{
						componentInParent.ConfigureInteraction("In use by workers", InteractableObject.EInteractableState.Invalid, false);
						return;
					}
					componentInParent.ConfigureInteraction(this.InteractionLabel, InteractableObject.EInteractableState.Default, false);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
					{
						this.StartPourTask(componentInParent);
						return;
					}
				}
				else
				{
					if (empty != string.Empty)
					{
						componentInParent.ConfigureInteraction(empty, InteractableObject.EInteractableState.Invalid, false);
						return;
					}
					componentInParent.ConfigureInteraction(string.Empty, InteractableObject.EInteractableState.Disabled, false);
				}
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0010875C File Offset: 0x0010695C
		protected virtual void StartPourTask(Pot pot)
		{
			new PourIntoPotTask(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00108771 File Offset: 0x00106971
		protected virtual bool CanPour(Pot pot, out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x04002D23 RID: 11555
		[Header("Pourable settings")]
		public float InteractionRange = 2.5f;

		// Token: 0x04002D24 RID: 11556
		public Pourable PourablePrefab;
	}
}
