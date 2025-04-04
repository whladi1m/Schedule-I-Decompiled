using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000900 RID: 2304
	public class Equippable_Trimmers : Equippable_Viewmodel
	{
		// Token: 0x06003E71 RID: 15985 RVA: 0x001079D4 File Offset: 0x00105BD4
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
					if (componentInParent.IsReadyForHarvest(out message))
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
						componentInParent.ConfigureInteraction("Harvest", InteractableObject.EInteractableState.Default, true);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							new HarvestPlant(componentInParent, this.CanClickAndDrag, this.SoundLoopPrefab);
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

		// Token: 0x04002CF6 RID: 11510
		public bool CanClickAndDrag;

		// Token: 0x04002CF7 RID: 11511
		public AudioSourceController SoundLoopPrefab;
	}
}
