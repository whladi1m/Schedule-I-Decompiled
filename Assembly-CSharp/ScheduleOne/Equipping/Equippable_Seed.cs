using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x020008FD RID: 2301
	public class Equippable_Seed : Equippable_Viewmodel
	{
		// Token: 0x06003E5D RID: 15965 RVA: 0x00107434 File Offset: 0x00105634
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
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				Pot componentInParent = raycastHit.collider.GetComponentInParent<Pot>();
				if (componentInParent != null)
				{
					string message;
					if (componentInParent.CanAcceptSeed(out message))
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
						componentInParent.ConfigureInteraction("Sow seed", InteractableObject.EInteractableState.Default, false);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							this.StartSowSeedTask(componentInParent);
							return;
						}
					}
					else
					{
						componentInParent.ConfigureInteraction(message, InteractableObject.EInteractableState.Invalid, false);
					}
				}
			}
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00107504 File Offset: 0x00105704
		protected virtual void StartSowSeedTask(Pot pot)
		{
			new SowSeedTask(pot, this.Seed);
		}

		// Token: 0x04002CE2 RID: 11490
		public SeedDefinition Seed;
	}
}
