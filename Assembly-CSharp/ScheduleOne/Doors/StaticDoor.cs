using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Doors
{
	// Token: 0x0200067F RID: 1663
	public class StaticDoor : MonoBehaviour
	{
		// Token: 0x06002E1F RID: 11807 RVA: 0x000C16C4 File Offset: 0x000BF8C4
		protected virtual void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			if (this.Building == null)
			{
				this.Building = base.GetComponentInParent<NPCEnterableBuilding>();
				if (this.Building == null && (this.Usable || this.CanKnock))
				{
					Console.LogWarning("StaticDoor " + base.name + " has no NPCEnterableBuilding!", null);
					this.Usable = false;
					this.CanKnock = false;
				}
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000C176C File Offset: 0x000BF96C
		protected virtual void OnValidate()
		{
			if (this.Building == null)
			{
				this.Building = base.GetComponentInParent<NPCEnterableBuilding>();
			}
			if (this.Building != null && !base.transform.IsChildOf(this.Building.transform))
			{
				Console.LogWarning("StaticDoor " + base.name + " is not a child of " + this.Building.BuildingName, null);
			}
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000C17DF File Offset: 0x000BF9DF
		protected virtual void Update()
		{
			if (this.timeSinceLastKnock < 2f)
			{
				this.timeSinceLastKnock += Time.deltaTime;
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000C1800 File Offset: 0x000BFA00
		protected virtual void Hovered()
		{
			if (!this.CanKnockNow())
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			string message;
			if (this.IsKnockValid(out message))
			{
				this.IntObj.SetMessage("Knock");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000C1861 File Offset: 0x000BFA61
		protected virtual bool CanKnockNow()
		{
			return this.CanKnock && this.timeSinceLastKnock >= 2f && this.Building != null;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000702CA File Offset: 0x0006E4CA
		protected virtual bool IsKnockValid(out string message)
		{
			message = string.Empty;
			return true;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000C1886 File Offset: 0x000BFA86
		protected virtual void Interacted()
		{
			this.Knock();
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000C188E File Offset: 0x000BFA8E
		protected virtual void Knock()
		{
			this.timeSinceLastKnock = 0f;
			if (this.KnockSound != null)
			{
				this.KnockSound.Play();
			}
			base.StartCoroutine(this.<Knock>g__knockRoutine|16_0());
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000C18C4 File Offset: 0x000BFAC4
		protected virtual void NPCSelected(NPC npc)
		{
			npc.behaviour.Summon(this.Building.GUID.ToString(), this.Building.Doors.IndexOf(this), 8f);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000C192C File Offset: 0x000BFB2C
		[CompilerGenerated]
		private IEnumerator <Knock>g__knockRoutine|16_0()
		{
			yield return new WaitForSeconds(0.7f);
			if (this.Building.OccupantCount > 0)
			{
				if (this.Building.OccupantCount == 1)
				{
					this.NPCSelected(this.Building.GetSummonableNPCs()[0]);
				}
				else
				{
					Singleton<NPCSummonMenu>.Instance.Open(this.Building.GetSummonableNPCs(), new Action<NPC>(this.NPCSelected));
				}
			}
			else
			{
				Console.Log("Building is empty!", null);
			}
			yield break;
		}

		// Token: 0x040020E4 RID: 8420
		public const float KNOCK_COOLDOWN = 2f;

		// Token: 0x040020E5 RID: 8421
		public const float SUMMON_DURATION = 8f;

		// Token: 0x040020E6 RID: 8422
		[Header("References")]
		public Transform AccessPoint;

		// Token: 0x040020E7 RID: 8423
		public InteractableObject IntObj;

		// Token: 0x040020E8 RID: 8424
		public AudioSourceController KnockSound;

		// Token: 0x040020E9 RID: 8425
		public NPCEnterableBuilding Building;

		// Token: 0x040020EA RID: 8426
		[Header("Settings")]
		public bool Usable = true;

		// Token: 0x040020EB RID: 8427
		public bool CanKnock = true;

		// Token: 0x040020EC RID: 8428
		private float timeSinceLastKnock = 2f;
	}
}
