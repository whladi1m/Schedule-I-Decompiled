using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000842 RID: 2114
	public class ForcePlayerCrouch : MonoBehaviour
	{
		// Token: 0x060039E2 RID: 14818 RVA: 0x000F43E4 File Offset: 0x000F25E4
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				Player componentInParent = other.gameObject.GetComponentInParent<Player>();
				if (componentInParent != null && componentInParent.IsOwner && !PlayerSingleton<PlayerMovement>.Instance.isCrouched)
				{
					PlayerSingleton<PlayerMovement>.Instance.SetCrouched(true);
				}
			}
		}
	}
}
