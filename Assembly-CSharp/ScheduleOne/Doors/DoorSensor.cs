using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x02000678 RID: 1656
	[RequireComponent(typeof(Rigidbody))]
	public class DoorSensor : MonoBehaviour
	{
		// Token: 0x06002DFC RID: 11772 RVA: 0x000C10E1 File Offset: 0x000BF2E1
		private void Awake()
		{
			this.collider = base.GetComponent<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000C1104 File Offset: 0x000BF304
		private void UpdateCollider()
		{
			if (PlayerSingleton<PlayerCamera>.Instance == null)
			{
				return;
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (InstanceFinder.IsServer)
			{
				Player.GetClosestPlayer(base.transform.position, out num, null);
			}
			this.collider.enabled = (num < 30f);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000C1170 File Offset: 0x000BF370
		private void OnTriggerStay(Collider other)
		{
			if (this.exclude.Contains(other))
			{
				return;
			}
			NPC componentInParent = other.GetComponentInParent<NPC>();
			if (componentInParent != null && componentInParent.IsConscious && !componentInParent.Avatar.Ragdolled && componentInParent.CanOpenDoors)
			{
				this.Door.NPCVicinityDetected(this.DetectorSide);
				return;
			}
			if (other.GetComponentInParent<Player>() != null)
			{
				this.Door.PlayerVicinityDetected(this.DetectorSide);
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x040020BE RID: 8382
		public const float ActivationDistance = 30f;

		// Token: 0x040020BF RID: 8383
		public EDoorSide DetectorSide = EDoorSide.Exterior;

		// Token: 0x040020C0 RID: 8384
		public DoorController Door;

		// Token: 0x040020C1 RID: 8385
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x040020C2 RID: 8386
		private Collider collider;
	}
}
